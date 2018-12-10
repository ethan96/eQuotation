using System;
using System.Collections.Generic;
using System.Linq;
using Advantech.Myadvantech.DataAccess;
using System.Data.SqlClient;
using System.Data;
using System.Activities;
using System.Threading;
using System.Runtime.DurableInstancing;
using System.Activities.DurableInstancing;
using System.Configuration;
using WorkFlowlAPI.Utility;

namespace WorkFlowlAPI
{
    public class QuoteApprovalDog
    {
        public static ApprovalResult ApprovalResult { get; private set; }
        private static string relatedQuoteId;
        public static void StartFlow(string quoteId, string url, List<WorkFlowApproval> waitingApprovals, string region)
        {
            relatedQuoteId = quoteId;

            if (waitingApprovals != null)
            {
                //AutoResetEvent allows threads that need access to a resource to communicate with each other by signaling. 
                //A thread waits for a signal by calling WaitOne on the AutoResetEvent. 
                //If the AutoResetEvent is the non-signaled state (a thread becomes in signaled state by calling Set on AutoResetEvent), 
                //the thread blocks waiting for the thread that currently controls the resource to signal that the resource is available by calling Set.
                AutoResetEvent syncEvent = new AutoResetEvent(false);
                Dictionary<string, object> inputs = new Dictionary<string, object>();

                inputs.Add("Region", region);
                inputs.Add("QuoteId", quoteId);
                inputs.Add("Url", url);
                //inputs.Add("WaitingApprovalList", waitingApprovals);

                WorkflowApplication wfApp = GetWorkFlowApplication(inputs, syncEvent);
                wfApp.Extensions.Add(waitingApprovals);
                wfApp.Run();
                syncEvent.WaitOne();
            }
            else
            {
                SendErrorMail("There are no waiting approvals, start flow fail!");
                ApprovalResult = ApprovalResult.WaitingApproverNotFound;
            }
        }

        public static void ResumeFlow(string quoteId, List<WorkFlowApproval> addtionlApprovals)
        {
            relatedQuoteId = quoteId;
            WorkFlowApproval approval = eQuotationContext.Current.WorkFlowApproval.OrderByDescending(q => q.LevelNum).FirstOrDefault(q => q.TypeID == quoteId && q.WorkFlowID != "");

            if (approval != null)
            {
                string strSqlCmd =
                    String.Format(
                        "select top 1 Id,BlockingBookmarks from [System.Activities.DurableInstancing].[InstancesTable] where id='{0}'",
                        approval.WorkFlowID);
                DataTable dt = dbGetDataTable("WFDB", strSqlCmd);
                if (dt.Rows.Count == 1)
                {
                    string bookmark = dt.Rows[0]["BlockingBookmarks"].ToString();
                    string bookmarkName = bookmark.Substring(1, bookmark.IndexOf(":") - 1);

                    AutoResetEvent syncEvent = new AutoResetEvent(false);
                    WorkflowApplication wfApp = GetWorkFlowApplication(null, syncEvent);
                    Guid instanceId = new Guid(approval.WorkFlowID);
                    wfApp.Load(instanceId);
                    wfApp.ResumeBookmark(bookmarkName, addtionlApprovals);
                    wfApp.Run();
                    syncEvent.WaitOne();

                }
                else
                {
                    SendErrorMail("Bookmark is not found in InstancesTable, resume flow fail!");
                    ApprovalResult = ApprovalResult.ApprovalBookmarkNotFound;
                }


            }
            else
            {
                SendErrorMail("Approval is not found in WorkFlowApproval Table, resume flow fail!");
                ApprovalResult = ApprovalResult.WaitingApproverNotFound;
            }


        }


        private static WorkflowApplication GetWorkFlowApplication(Dictionary<string, object> inputs, AutoResetEvent syncEvent)
        {
            AutoResetEvent InstanceUnloaded = new AutoResetEvent(false);
            InstanceStore store = new SqlWorkflowInstanceStore(ConfigurationManager.ConnectionStrings["WFDB"].ConnectionString);

            var accountFlowIdentity = new WorkflowIdentity { Name = "test", Version = new Version(1, 0, 0, 1) };
            WorkflowApplication.DeleteDefaultInstanceOwner(store);
            WorkflowApplication.CreateDefaultInstanceOwner(store, null, WorkflowIdentityFilter.Any, new TimeSpan(0, 0, 0, 10));

            WorkflowApplication wfApp = null;

            if (inputs == null)
                wfApp = new WorkflowApplication(new QuoteApprovalFlow(), accountFlowIdentity);
            else
                wfApp = new WorkflowApplication(new QuoteApprovalFlow(), inputs, accountFlowIdentity);

            wfApp.InstanceStore = store;

            wfApp.PersistableIdle = (e) =>
            {
                ApprovalResult = ApprovalResult.WaitingForApproval;

                return PersistableIdleAction.Unload;

            };


            wfApp.Unloaded = (workflowApplicationEventArgs) =>
            {
                syncEvent.Set();
                /*InstanceUnloaded.Set();*/  //a thread becomes in signaled state by calling Set on AutoResetEvent

            };

            //wfApp.OnUnhandledException = (e) =>
            //{
            //    var test = e.ToString();
            //    return UnhandledExceptionAction.Terminate;
            //};

            wfApp.Completed = (e) =>
            {

                if (e.CompletionState == ActivityInstanceState.Faulted)
                {
                    ApprovalResult = ApprovalResult.Exception;
                }
                else if (e.CompletionState == ActivityInstanceState.Canceled)
                {

                }
                else
                {
                    ApprovalResult = (ApprovalResult)e.Outputs["Result"];
                }

                syncEvent.Set();

            };

            wfApp.Aborted = (e) =>
            {
                syncEvent.Set();
            };

            wfApp.OnUnhandledException = (e) =>
            {
                ApprovalResult = ApprovalResult.Exception;
                //string subject = String.Format("eQ3.0 Workflow exception, quote Id: {0}", relatedQuoteId);
                //string strContent = String.Format("<p> eQ3.0 Workflow exception in {0}</p>", DateTime.Now.ToString());
                //strContent += String.Format("<p>Quote Id: {0}</p>", relatedQuoteId);
                //strContent += String.Format("<p>Workflow Id: {0}</p>", e.InstanceId);
                //strContent += String.Format("<p>Error message: {0}</p>", e.UnhandledException.Message);

                //MailHelper.SendMail("myadvantech@advantech.com", subject, strContent);
                var message = String.Format("<p>Workflow ID: {0}</p>", e.InstanceId);
                message += String.Format("<p>Error message: {0}</p>", e.UnhandledException.Message);
                SendErrorMail(message);
                return UnhandledExceptionAction.Terminate;
            };
            return wfApp;
        }

        private static void SendErrorMail(string message)
        {
            string subject = String.Format("eQ3.0 approval process errors, quote Id: {0}", relatedQuoteId);
            string strContent = String.Format("<p> eQ3.0 approval process errors in {0}</p>", DateTime.Now.ToString());
            strContent += String.Format("<p>Quote Id: {0}</p>", relatedQuoteId);
            strContent += message;
            MailHelper.SendMail("myadvantech@advantech.com", subject, strContent);
        }


        private static DataTable dbGetDataTable(string connectionName, string sql)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionName].ConnectionString);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            da.SelectCommand.CommandTimeout = 300;
            try
            {
                da.Fill(dt);
            }
            catch
            {
                da.Dispose();
                conn.Close();
                conn.Dispose();
            }
            return dt;
        }
    }
}
