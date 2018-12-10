using Advantech.Myadvantech.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using System.Threading;
using System.Runtime.DurableInstancing;
using System.Activities.DurableInstancing;
using System.Configuration;

namespace WorkFlowlAPI.SimulatePrice
{
    public class QuoteSimulatePriceV2
    {
        private static string ErrorMessage;
        public static void StartFlow(ref QuotationMaster quote, ref string errorMsg)
        {
            //AutoResetEvent allows threads that need access to a resource to communicate with each other by signaling. 
            //A thread waits for a signal by calling WaitOne on the AutoResetEvent. 
            //If the AutoResetEvent is the non-signaled state (a thread becomes in signaled state by calling Set on AutoResetEvent), 
            //the thread blocks waiting for the thread that currently controls the resource to signal that the resource is available by calling Set.

            ErrorMessage = "";
            AutoResetEvent syncEvent = new AutoResetEvent(false);
            Dictionary<string, object> inputs = new Dictionary<string, object>();

            inputs.Add("QuotationMaster", quote);
            //inputs.Add("ErrorMessage", errorMsg);

            WorkflowApplication wfApp = GetWorkFlowApplication(inputs, syncEvent);
            wfApp.Run();
            syncEvent.WaitOne();

            // move to outside?
            errorMsg = ErrorMessage;
            quote.ReorderQuotationDetail();
            quote.InitializeQuotationDetail();

        }


        private static WorkflowApplication GetWorkFlowApplication(Dictionary<string, object> inputs, AutoResetEvent syncEvent)
        {
            InstanceStore store = new SqlWorkflowInstanceStore(ConfigurationManager.ConnectionStrings["WFDB"].ConnectionString);

            var accountFlowIdentity = new WorkflowIdentity { Name = "test", Version = new Version(1, 0, 0, 1) };
            //WorkflowApplication.DeleteDefaultInstanceOwner(store);
            WorkflowApplication.CreateDefaultInstanceOwner(store, null, WorkflowIdentityFilter.Any, new TimeSpan(0, 0, 0, 10));

            WorkflowApplication wfApp = new WorkflowApplication(new SimulateSAPPriceITPFlow(), inputs, accountFlowIdentity);

            wfApp.InstanceStore = store;

            //wfApp.PersistableIdle = (e) =>
            //{
            //    //a thread becomes in signaled state by calling Set on AutoResetEvent
            //    return PersistableIdleAction.Unload;

            //};
            //wfApp.Unloaded = (workflowApplicationEventArgs) =>
            //{
            //    syncEvent.Set();
            //};

            wfApp.Completed = (e) =>
            {
                ErrorMessage += (string)e.Outputs["ErrorMessage"];
                syncEvent.Set();
            };

            wfApp.Aborted = (e) =>
            {
                string test = Convert.ToString(e.Reason.Message);
                //syncEvent.Set();
            };

            wfApp.OnUnhandledException = (e) =>
            {
                ErrorMessage += e.UnhandledException.Message;
                return UnhandledExceptionAction.Terminate;
            };
            return wfApp;
        }
    }
}
