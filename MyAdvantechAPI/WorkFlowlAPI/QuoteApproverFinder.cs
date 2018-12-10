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
using WorkFlowlAPI.ACNGPApproval;

namespace WorkFlowlAPI
{
    public class QuoteApproverFinder
    {
        public static WorkFlowlAPI.FindApproverResult FindApproverResult { get; private set; }

        public static List<WorkFlowApproval> WaitingApprovals { get; private set; }

        public static QuotationMaster QuoteMasterWithGPStatus { get; private set; }

        public static void StartFlow(QuotationMaster quote, string url, string region)
        {
            //AutoResetEvent allows threads that need access to a resource to communicate with each other by signaling. 
            //A thread waits for a signal by calling WaitOne on the AutoResetEvent. 
            //If the AutoResetEvent is the non-signaled state (a thread becomes in signaled state by calling Set on AutoResetEvent), 
            //the thread blocks waiting for the thread that currently controls the resource to signal that the resource is available by calling Set.
            AutoResetEvent syncEvent = new AutoResetEvent(false);
            Dictionary<string, object> inputs = new Dictionary<string, object>();

            inputs.Add("Region", region);
            inputs.Add("Url", url);
            inputs.Add("QuotationMaster", quote);

            WorkflowApplication wfApp = GetWorkFlowApplication(inputs, syncEvent);
            wfApp.Run();
            syncEvent.WaitOne();
        }


        private static WorkflowApplication GetWorkFlowApplication(Dictionary<string, object> inputs, AutoResetEvent syncEvent)
        {
            InstanceStore store = new SqlWorkflowInstanceStore(ConfigurationManager.ConnectionStrings["WFDB"].ConnectionString);

            var accountFlowIdentity = new WorkflowIdentity { Name = "test", Version = new Version(1, 0, 0, 1) };
            WorkflowApplication.DeleteDefaultInstanceOwner(store);
            WorkflowApplication.CreateDefaultInstanceOwner(store, null, WorkflowIdentityFilter.Any, new TimeSpan(0, 0, 0, 10));

            WorkflowApplication wfApp = new WorkflowApplication(new FindApproverFlow(), inputs, accountFlowIdentity);

            wfApp.InstanceStore = store;

            wfApp.PersistableIdle = (e) =>
            {
                 //a thread becomes in signaled state by calling Set on AutoResetEvent
                return PersistableIdleAction.Unload;

            };
            wfApp.Unloaded = (workflowApplicationEventArgs) =>
            {
                syncEvent.Set();
                //InstanceUnloaded.Set();
            };

            wfApp.Completed = (e) =>
            {
                FindApproverResult = (FindApproverResult)e.Outputs["Result"];
                WaitingApprovals = (List<WorkFlowApproval>)e.Outputs["ApprovalList"];
                QuoteMasterWithGPStatus = (QuotationMaster)e.Outputs["QuotationMaster"];
                //syncEvent.Set();

            };

            wfApp.Aborted = (e) =>
            {
                string test = Convert.ToString(e.Reason.Message);
            };

            wfApp.OnUnhandledException = (e) =>
            {
                return UnhandledExceptionAction.Terminate;
            };
            return wfApp;
        }

    }


}
