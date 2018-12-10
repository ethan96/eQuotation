namespace WorkFlowlAPI {
    
    #line 24 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\QuoteApprovalFlow.xaml"
    using System;
    
    #line default
    #line hidden
    
    #line 1 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\QuoteApprovalFlow.xaml"
    using System.Collections;
    
    #line default
    #line hidden
    
    #line 25 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\QuoteApprovalFlow.xaml"
    using System.Collections.Generic;
    
    #line default
    #line hidden
    
    #line 1 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\QuoteApprovalFlow.xaml"
    using System.Activities;
    
    #line default
    #line hidden
    
    #line 1 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\QuoteApprovalFlow.xaml"
    using System.Activities.Expressions;
    
    #line default
    #line hidden
    
    #line 1 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\QuoteApprovalFlow.xaml"
    using System.Activities.Statements;
    
    #line default
    #line hidden
    
    #line 26 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\QuoteApprovalFlow.xaml"
    using System.Data;
    
    #line default
    #line hidden
    
    #line 27 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\QuoteApprovalFlow.xaml"
    using System.Linq;
    
    #line default
    #line hidden
    
    #line 28 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\QuoteApprovalFlow.xaml"
    using System.Text;
    
    #line default
    #line hidden
    
    #line 29 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\QuoteApprovalFlow.xaml"
    using WorkFlowlAPI;
    
    #line default
    #line hidden
    
    #line 30 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\QuoteApprovalFlow.xaml"
    using Advantech.Myadvantech.Business;
    
    #line default
    #line hidden
    
    #line 31 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\QuoteApprovalFlow.xaml"
    using Advantech.Myadvantech.DataAccess;
    
    #line default
    #line hidden
    
    #line 1 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\QuoteApprovalFlow.xaml"
    using System.Activities.XamlIntegration;
    
    #line default
    #line hidden
    
    
    public partial class QuoteApprovalFlow : System.Activities.XamlIntegration.ICompiledExpressionRoot {
        
        private System.Activities.Activity rootActivity;
        
        private object dataContextActivities;
        
        private bool forImplementation = true;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public string GetLanguage() {
            return "C#";
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public object InvokeExpression(int expressionId, System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext) {
            if ((this.rootActivity == null)) {
                this.rootActivity = this;
            }
            if ((this.dataContextActivities == null)) {
                this.dataContextActivities = QuoteApprovalFlow_TypedDataContext2_ForReadOnly.GetDataContextActivitiesHelper(this.rootActivity, this.forImplementation);
            }
            if ((expressionId == 0)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = QuoteApprovalFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 3);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                QuoteApprovalFlow_TypedDataContext2_ForReadOnly valDataContext0 = ((QuoteApprovalFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext0.ValueType___Expr0Get();
            }
            if ((expressionId == 1)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = QuoteApprovalFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 3);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                QuoteApprovalFlow_TypedDataContext2_ForReadOnly valDataContext1 = ((QuoteApprovalFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext1.ValueType___Expr1Get();
            }
            if ((expressionId == 2)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = QuoteApprovalFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 3);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                QuoteApprovalFlow_TypedDataContext2_ForReadOnly valDataContext2 = ((QuoteApprovalFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext2.ValueType___Expr2Get();
            }
            if ((expressionId == 3)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = QuoteApprovalFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 3);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                QuoteApprovalFlow_TypedDataContext2_ForReadOnly valDataContext3 = ((QuoteApprovalFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext3.ValueType___Expr3Get();
            }
            if ((expressionId == 4)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = QuoteApprovalFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 3);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new QuoteApprovalFlow_TypedDataContext2(locations, activityContext, true);
                }
                QuoteApprovalFlow_TypedDataContext2 refDataContext4 = ((QuoteApprovalFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext4.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>(refDataContext4.ValueType___Expr4Get, refDataContext4.ValueType___Expr4Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 5)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = QuoteApprovalFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 3);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                QuoteApprovalFlow_TypedDataContext2_ForReadOnly valDataContext5 = ((QuoteApprovalFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext5.ValueType___Expr5Get();
            }
            if ((expressionId == 6)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = QuoteApprovalFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 3);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                QuoteApprovalFlow_TypedDataContext2_ForReadOnly valDataContext6 = ((QuoteApprovalFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext6.ValueType___Expr6Get();
            }
            if ((expressionId == 7)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = QuoteApprovalFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 3);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                QuoteApprovalFlow_TypedDataContext2_ForReadOnly valDataContext7 = ((QuoteApprovalFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext7.ValueType___Expr7Get();
            }
            if ((expressionId == 8)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = QuoteApprovalFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 3);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                QuoteApprovalFlow_TypedDataContext2_ForReadOnly valDataContext8 = ((QuoteApprovalFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext8.ValueType___Expr8Get();
            }
            if ((expressionId == 9)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = QuoteApprovalFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 3);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                QuoteApprovalFlow_TypedDataContext2_ForReadOnly valDataContext9 = ((QuoteApprovalFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext9.ValueType___Expr9Get();
            }
            if ((expressionId == 10)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = QuoteApprovalFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 3);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new QuoteApprovalFlow_TypedDataContext2(locations, activityContext, true);
                }
                QuoteApprovalFlow_TypedDataContext2 refDataContext10 = ((QuoteApprovalFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext10.GetLocation<WorkFlowlAPI.ApprovalResult>(refDataContext10.ValueType___Expr10Get, refDataContext10.ValueType___Expr10Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 11)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = QuoteApprovalFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 3);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                QuoteApprovalFlow_TypedDataContext2_ForReadOnly valDataContext11 = ((QuoteApprovalFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext11.ValueType___Expr11Get();
            }
            if ((expressionId == 12)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = QuoteApprovalFlow_TypedDataContext3_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 3);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new QuoteApprovalFlow_TypedDataContext3_ForReadOnly(locations, activityContext, true);
                }
                QuoteApprovalFlow_TypedDataContext3_ForReadOnly valDataContext12 = ((QuoteApprovalFlow_TypedDataContext3_ForReadOnly)(cachedCompiledDataContext[2]));
                return valDataContext12.ValueType___Expr12Get();
            }
            if ((expressionId == 13)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = QuoteApprovalFlow_TypedDataContext3_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 3);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new QuoteApprovalFlow_TypedDataContext3_ForReadOnly(locations, activityContext, true);
                }
                QuoteApprovalFlow_TypedDataContext3_ForReadOnly valDataContext13 = ((QuoteApprovalFlow_TypedDataContext3_ForReadOnly)(cachedCompiledDataContext[2]));
                return valDataContext13.ValueType___Expr13Get();
            }
            if ((expressionId == 14)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = QuoteApprovalFlow_TypedDataContext3_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 3);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new QuoteApprovalFlow_TypedDataContext3_ForReadOnly(locations, activityContext, true);
                }
                QuoteApprovalFlow_TypedDataContext3_ForReadOnly valDataContext14 = ((QuoteApprovalFlow_TypedDataContext3_ForReadOnly)(cachedCompiledDataContext[2]));
                return valDataContext14.ValueType___Expr14Get();
            }
            if ((expressionId == 15)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = QuoteApprovalFlow_TypedDataContext3_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 3);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new QuoteApprovalFlow_TypedDataContext3_ForReadOnly(locations, activityContext, true);
                }
                QuoteApprovalFlow_TypedDataContext3_ForReadOnly valDataContext15 = ((QuoteApprovalFlow_TypedDataContext3_ForReadOnly)(cachedCompiledDataContext[2]));
                return valDataContext15.ValueType___Expr15Get();
            }
            if ((expressionId == 16)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = QuoteApprovalFlow_TypedDataContext3_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 3);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new QuoteApprovalFlow_TypedDataContext3_ForReadOnly(locations, activityContext, true);
                }
                QuoteApprovalFlow_TypedDataContext3_ForReadOnly valDataContext16 = ((QuoteApprovalFlow_TypedDataContext3_ForReadOnly)(cachedCompiledDataContext[2]));
                return valDataContext16.ValueType___Expr16Get();
            }
            if ((expressionId == 17)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = QuoteApprovalFlow_TypedDataContext3_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 3);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new QuoteApprovalFlow_TypedDataContext3_ForReadOnly(locations, activityContext, true);
                }
                QuoteApprovalFlow_TypedDataContext3_ForReadOnly valDataContext17 = ((QuoteApprovalFlow_TypedDataContext3_ForReadOnly)(cachedCompiledDataContext[2]));
                return valDataContext17.ValueType___Expr17Get();
            }
            if ((expressionId == 18)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = QuoteApprovalFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 3);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                QuoteApprovalFlow_TypedDataContext2_ForReadOnly valDataContext18 = ((QuoteApprovalFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext18.ValueType___Expr18Get();
            }
            if ((expressionId == 19)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = QuoteApprovalFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 3);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                QuoteApprovalFlow_TypedDataContext2_ForReadOnly valDataContext19 = ((QuoteApprovalFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext19.ValueType___Expr19Get();
            }
            if ((expressionId == 20)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = QuoteApprovalFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 3);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                QuoteApprovalFlow_TypedDataContext2_ForReadOnly valDataContext20 = ((QuoteApprovalFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext20.ValueType___Expr20Get();
            }
            return null;
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public object InvokeExpression(int expressionId, System.Collections.Generic.IList<System.Activities.Location> locations) {
            if ((this.rootActivity == null)) {
                this.rootActivity = this;
            }
            if ((expressionId == 0)) {
                QuoteApprovalFlow_TypedDataContext2_ForReadOnly valDataContext0 = new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext0.ValueType___Expr0Get();
            }
            if ((expressionId == 1)) {
                QuoteApprovalFlow_TypedDataContext2_ForReadOnly valDataContext1 = new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext1.ValueType___Expr1Get();
            }
            if ((expressionId == 2)) {
                QuoteApprovalFlow_TypedDataContext2_ForReadOnly valDataContext2 = new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext2.ValueType___Expr2Get();
            }
            if ((expressionId == 3)) {
                QuoteApprovalFlow_TypedDataContext2_ForReadOnly valDataContext3 = new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext3.ValueType___Expr3Get();
            }
            if ((expressionId == 4)) {
                QuoteApprovalFlow_TypedDataContext2 refDataContext4 = new QuoteApprovalFlow_TypedDataContext2(locations, true);
                return refDataContext4.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>(refDataContext4.ValueType___Expr4Get, refDataContext4.ValueType___Expr4Set);
            }
            if ((expressionId == 5)) {
                QuoteApprovalFlow_TypedDataContext2_ForReadOnly valDataContext5 = new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext5.ValueType___Expr5Get();
            }
            if ((expressionId == 6)) {
                QuoteApprovalFlow_TypedDataContext2_ForReadOnly valDataContext6 = new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext6.ValueType___Expr6Get();
            }
            if ((expressionId == 7)) {
                QuoteApprovalFlow_TypedDataContext2_ForReadOnly valDataContext7 = new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext7.ValueType___Expr7Get();
            }
            if ((expressionId == 8)) {
                QuoteApprovalFlow_TypedDataContext2_ForReadOnly valDataContext8 = new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext8.ValueType___Expr8Get();
            }
            if ((expressionId == 9)) {
                QuoteApprovalFlow_TypedDataContext2_ForReadOnly valDataContext9 = new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext9.ValueType___Expr9Get();
            }
            if ((expressionId == 10)) {
                QuoteApprovalFlow_TypedDataContext2 refDataContext10 = new QuoteApprovalFlow_TypedDataContext2(locations, true);
                return refDataContext10.GetLocation<WorkFlowlAPI.ApprovalResult>(refDataContext10.ValueType___Expr10Get, refDataContext10.ValueType___Expr10Set);
            }
            if ((expressionId == 11)) {
                QuoteApprovalFlow_TypedDataContext2_ForReadOnly valDataContext11 = new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext11.ValueType___Expr11Get();
            }
            if ((expressionId == 12)) {
                QuoteApprovalFlow_TypedDataContext3_ForReadOnly valDataContext12 = new QuoteApprovalFlow_TypedDataContext3_ForReadOnly(locations, true);
                return valDataContext12.ValueType___Expr12Get();
            }
            if ((expressionId == 13)) {
                QuoteApprovalFlow_TypedDataContext3_ForReadOnly valDataContext13 = new QuoteApprovalFlow_TypedDataContext3_ForReadOnly(locations, true);
                return valDataContext13.ValueType___Expr13Get();
            }
            if ((expressionId == 14)) {
                QuoteApprovalFlow_TypedDataContext3_ForReadOnly valDataContext14 = new QuoteApprovalFlow_TypedDataContext3_ForReadOnly(locations, true);
                return valDataContext14.ValueType___Expr14Get();
            }
            if ((expressionId == 15)) {
                QuoteApprovalFlow_TypedDataContext3_ForReadOnly valDataContext15 = new QuoteApprovalFlow_TypedDataContext3_ForReadOnly(locations, true);
                return valDataContext15.ValueType___Expr15Get();
            }
            if ((expressionId == 16)) {
                QuoteApprovalFlow_TypedDataContext3_ForReadOnly valDataContext16 = new QuoteApprovalFlow_TypedDataContext3_ForReadOnly(locations, true);
                return valDataContext16.ValueType___Expr16Get();
            }
            if ((expressionId == 17)) {
                QuoteApprovalFlow_TypedDataContext3_ForReadOnly valDataContext17 = new QuoteApprovalFlow_TypedDataContext3_ForReadOnly(locations, true);
                return valDataContext17.ValueType___Expr17Get();
            }
            if ((expressionId == 18)) {
                QuoteApprovalFlow_TypedDataContext2_ForReadOnly valDataContext18 = new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext18.ValueType___Expr18Get();
            }
            if ((expressionId == 19)) {
                QuoteApprovalFlow_TypedDataContext2_ForReadOnly valDataContext19 = new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext19.ValueType___Expr19Get();
            }
            if ((expressionId == 20)) {
                QuoteApprovalFlow_TypedDataContext2_ForReadOnly valDataContext20 = new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext20.ValueType___Expr20Get();
            }
            return null;
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool CanExecuteExpression(string expressionText, bool isReference, System.Collections.Generic.IList<System.Activities.LocationReference> locations, out int expressionId) {
            if (((isReference == false) 
                        && ((expressionText == "GPControlBusinessLogic.GetQuoteWaitApprovalsByQuoteId(QuoteId).Any()") 
                        && (QuoteApprovalFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 0;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuoteBusinessLogic.IsRejectedByApprover(QuoteId) == true") 
                        && (QuoteApprovalFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 1;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "AdditionalApprovals != null && AdditionalApprovals.Count()>0") 
                        && (QuoteApprovalFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 2;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuoteBusinessLogic.IsApprovedByAllApprovers(QuoteId) == true") 
                        && (QuoteApprovalFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 3;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "AdditionalApprovals") 
                        && (QuoteApprovalFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 4;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuoteId") 
                        && (QuoteApprovalFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 5;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "Region") 
                        && (QuoteApprovalFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 6;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuoteId") 
                        && (QuoteApprovalFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 7;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "Region") 
                        && (QuoteApprovalFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 8;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "ApprovalResult.Finish") 
                        && (QuoteApprovalFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 9;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "Result") 
                        && (QuoteApprovalFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 10;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "AdditionalApprovals") 
                        && (QuoteApprovalFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 11;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "approval.ApproverType") 
                        && (QuoteApprovalFlow_TypedDataContext3_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 12;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "(double)approval.LevelNum") 
                        && (QuoteApprovalFlow_TypedDataContext3_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 13;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuoteId") 
                        && (QuoteApprovalFlow_TypedDataContext3_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 14;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "Url") 
                        && (QuoteApprovalFlow_TypedDataContext3_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 15;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "Convert.ToBoolean(approval.ViewGP)") 
                        && (QuoteApprovalFlow_TypedDataContext3_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 16;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "approval.Approver") 
                        && (QuoteApprovalFlow_TypedDataContext3_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 17;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuoteId") 
                        && (QuoteApprovalFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 18;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "Region") 
                        && (QuoteApprovalFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 19;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuoteId") 
                        && (QuoteApprovalFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 20;
                return true;
            }
            expressionId = -1;
            return false;
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public System.Collections.Generic.IList<string> GetRequiredLocations(int expressionId) {
            return null;
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public System.Linq.Expressions.Expression GetExpressionTreeForExpression(int expressionId, System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) {
            if ((expressionId == 0)) {
                return new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr0GetTree();
            }
            if ((expressionId == 1)) {
                return new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr1GetTree();
            }
            if ((expressionId == 2)) {
                return new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr2GetTree();
            }
            if ((expressionId == 3)) {
                return new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr3GetTree();
            }
            if ((expressionId == 4)) {
                return new QuoteApprovalFlow_TypedDataContext2(locationReferences).@__Expr4GetTree();
            }
            if ((expressionId == 5)) {
                return new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr5GetTree();
            }
            if ((expressionId == 6)) {
                return new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr6GetTree();
            }
            if ((expressionId == 7)) {
                return new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr7GetTree();
            }
            if ((expressionId == 8)) {
                return new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr8GetTree();
            }
            if ((expressionId == 9)) {
                return new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr9GetTree();
            }
            if ((expressionId == 10)) {
                return new QuoteApprovalFlow_TypedDataContext2(locationReferences).@__Expr10GetTree();
            }
            if ((expressionId == 11)) {
                return new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr11GetTree();
            }
            if ((expressionId == 12)) {
                return new QuoteApprovalFlow_TypedDataContext3_ForReadOnly(locationReferences).@__Expr12GetTree();
            }
            if ((expressionId == 13)) {
                return new QuoteApprovalFlow_TypedDataContext3_ForReadOnly(locationReferences).@__Expr13GetTree();
            }
            if ((expressionId == 14)) {
                return new QuoteApprovalFlow_TypedDataContext3_ForReadOnly(locationReferences).@__Expr14GetTree();
            }
            if ((expressionId == 15)) {
                return new QuoteApprovalFlow_TypedDataContext3_ForReadOnly(locationReferences).@__Expr15GetTree();
            }
            if ((expressionId == 16)) {
                return new QuoteApprovalFlow_TypedDataContext3_ForReadOnly(locationReferences).@__Expr16GetTree();
            }
            if ((expressionId == 17)) {
                return new QuoteApprovalFlow_TypedDataContext3_ForReadOnly(locationReferences).@__Expr17GetTree();
            }
            if ((expressionId == 18)) {
                return new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr18GetTree();
            }
            if ((expressionId == 19)) {
                return new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr19GetTree();
            }
            if ((expressionId == 20)) {
                return new QuoteApprovalFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr20GetTree();
            }
            return null;
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class QuoteApprovalFlow_TypedDataContext0 : System.Activities.XamlIntegration.CompiledDataContext {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public QuoteApprovalFlow_TypedDataContext0(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public QuoteApprovalFlow_TypedDataContext0(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public QuoteApprovalFlow_TypedDataContext0(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            internal static object GetDataContextActivitiesHelper(System.Activities.Activity compiledRoot, bool forImplementation) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetDataContextActivities(compiledRoot, forImplementation);
            }
            
            internal static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
            }
            
            public static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 0))) {
                    return false;
                }
                expectedLocationsCount = 0;
                return true;
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class QuoteApprovalFlow_TypedDataContext0_ForReadOnly : System.Activities.XamlIntegration.CompiledDataContext {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public QuoteApprovalFlow_TypedDataContext0_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public QuoteApprovalFlow_TypedDataContext0_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public QuoteApprovalFlow_TypedDataContext0_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            internal static object GetDataContextActivitiesHelper(System.Activities.Activity compiledRoot, bool forImplementation) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetDataContextActivities(compiledRoot, forImplementation);
            }
            
            internal static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
            }
            
            public static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 0))) {
                    return false;
                }
                expectedLocationsCount = 0;
                return true;
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class QuoteApprovalFlow_TypedDataContext1 : QuoteApprovalFlow_TypedDataContext0 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            protected WorkFlowlAPI.ApprovalResult Result;
            
            public QuoteApprovalFlow_TypedDataContext1(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public QuoteApprovalFlow_TypedDataContext1(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public QuoteApprovalFlow_TypedDataContext1(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected string QuoteId {
                get {
                    return ((string)(this.GetVariableValue((1 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((1 + locationsOffset), value);
                }
            }
            
            protected string Url {
                get {
                    return ((string)(this.GetVariableValue((2 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((2 + locationsOffset), value);
                }
            }
            
            protected string Region {
                get {
                    return ((string)(this.GetVariableValue((3 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((3 + locationsOffset), value);
                }
            }
            
            internal new static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public new virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
                base.SetLocationsOffset(locationsOffset);
            }
            
            protected override void GetValueTypeValues() {
                this.Result = ((WorkFlowlAPI.ApprovalResult)(this.GetVariableValue((0 + locationsOffset))));
                base.GetValueTypeValues();
            }
            
            protected override void SetValueTypeValues() {
                this.SetVariableValue((0 + locationsOffset), this.Result);
                base.SetValueTypeValues();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 4))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 4);
                }
                expectedLocationsCount = 4;
                if (((locationReferences[(offset + 1)].Name != "QuoteId") 
                            || (locationReferences[(offset + 1)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 2)].Name != "Url") 
                            || (locationReferences[(offset + 2)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 3)].Name != "Region") 
                            || (locationReferences[(offset + 3)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 0)].Name != "Result") 
                            || (locationReferences[(offset + 0)].Type != typeof(WorkFlowlAPI.ApprovalResult)))) {
                    return false;
                }
                return QuoteApprovalFlow_TypedDataContext0.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class QuoteApprovalFlow_TypedDataContext1_ForReadOnly : QuoteApprovalFlow_TypedDataContext0_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            protected WorkFlowlAPI.ApprovalResult Result;
            
            public QuoteApprovalFlow_TypedDataContext1_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public QuoteApprovalFlow_TypedDataContext1_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public QuoteApprovalFlow_TypedDataContext1_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected string QuoteId {
                get {
                    return ((string)(this.GetVariableValue((1 + locationsOffset))));
                }
            }
            
            protected string Url {
                get {
                    return ((string)(this.GetVariableValue((2 + locationsOffset))));
                }
            }
            
            protected string Region {
                get {
                    return ((string)(this.GetVariableValue((3 + locationsOffset))));
                }
            }
            
            internal new static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public new virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
                base.SetLocationsOffset(locationsOffset);
            }
            
            protected override void GetValueTypeValues() {
                this.Result = ((WorkFlowlAPI.ApprovalResult)(this.GetVariableValue((0 + locationsOffset))));
                base.GetValueTypeValues();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 4))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 4);
                }
                expectedLocationsCount = 4;
                if (((locationReferences[(offset + 1)].Name != "QuoteId") 
                            || (locationReferences[(offset + 1)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 2)].Name != "Url") 
                            || (locationReferences[(offset + 2)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 3)].Name != "Region") 
                            || (locationReferences[(offset + 3)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 0)].Name != "Result") 
                            || (locationReferences[(offset + 0)].Type != typeof(WorkFlowlAPI.ApprovalResult)))) {
                    return false;
                }
                return QuoteApprovalFlow_TypedDataContext0_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class QuoteApprovalFlow_TypedDataContext2 : QuoteApprovalFlow_TypedDataContext1 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public QuoteApprovalFlow_TypedDataContext2(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public QuoteApprovalFlow_TypedDataContext2(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public QuoteApprovalFlow_TypedDataContext2(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> AdditionalApprovals {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>)(this.GetVariableValue((4 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((4 + locationsOffset), value);
                }
            }
            
            internal new static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public new virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
                base.SetLocationsOffset(locationsOffset);
            }
            
            internal System.Linq.Expressions.Expression @__Expr4GetTree() {
                
                #line 201 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>> expression = () => 
                                      AdditionalApprovals;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> @__Expr4Get() {
                
                #line 201 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                return 
                                      AdditionalApprovals;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> ValueType___Expr4Get() {
                this.GetValueTypeValues();
                return this.@__Expr4Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr4Set(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> value) {
                
                #line 201 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                
                                      AdditionalApprovals = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr4Set(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> value) {
                this.GetValueTypeValues();
                this.@__Expr4Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr10GetTree() {
                
                #line 101 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.ApprovalResult>> expression = () => 
                              Result;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.ApprovalResult @__Expr10Get() {
                
                #line 101 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                return 
                              Result;
                
                #line default
                #line hidden
            }
            
            public WorkFlowlAPI.ApprovalResult ValueType___Expr10Get() {
                this.GetValueTypeValues();
                return this.@__Expr10Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr10Set(WorkFlowlAPI.ApprovalResult value) {
                
                #line 101 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                
                              Result = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr10Set(WorkFlowlAPI.ApprovalResult value) {
                this.GetValueTypeValues();
                this.@__Expr10Set(value);
                this.SetValueTypeValues();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 5))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 5);
                }
                expectedLocationsCount = 5;
                if (((locationReferences[(offset + 4)].Name != "AdditionalApprovals") 
                            || (locationReferences[(offset + 4)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>)))) {
                    return false;
                }
                return QuoteApprovalFlow_TypedDataContext1.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class QuoteApprovalFlow_TypedDataContext2_ForReadOnly : QuoteApprovalFlow_TypedDataContext1_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public QuoteApprovalFlow_TypedDataContext2_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public QuoteApprovalFlow_TypedDataContext2_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public QuoteApprovalFlow_TypedDataContext2_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> AdditionalApprovals {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>)(this.GetVariableValue((4 + locationsOffset))));
                }
            }
            
            internal new static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public new virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
                base.SetLocationsOffset(locationsOffset);
            }
            
            internal System.Linq.Expressions.Expression @__Expr0GetTree() {
                
                #line 66 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<bool>> expression = () => 
          GPControlBusinessLogic.GetQuoteWaitApprovalsByQuoteId(QuoteId).Any();
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public bool @__Expr0Get() {
                
                #line 66 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                return 
          GPControlBusinessLogic.GetQuoteWaitApprovalsByQuoteId(QuoteId).Any();
                
                #line default
                #line hidden
            }
            
            public bool ValueType___Expr0Get() {
                this.GetValueTypeValues();
                return this.@__Expr0Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr1GetTree() {
                
                #line 80 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<bool>> expression = () => 
                  QuoteBusinessLogic.IsRejectedByApprover(QuoteId) == true;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public bool @__Expr1Get() {
                
                #line 80 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                return 
                  QuoteBusinessLogic.IsRejectedByApprover(QuoteId) == true;
                
                #line default
                #line hidden
            }
            
            public bool ValueType___Expr1Get() {
                this.GetValueTypeValues();
                return this.@__Expr1Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr2GetTree() {
                
                #line 117 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<bool>> expression = () => 
                      AdditionalApprovals != null && AdditionalApprovals.Count()>0;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public bool @__Expr2Get() {
                
                #line 117 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                return 
                      AdditionalApprovals != null && AdditionalApprovals.Count()>0;
                
                #line default
                #line hidden
            }
            
            public bool ValueType___Expr2Get() {
                this.GetValueTypeValues();
                return this.@__Expr2Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr3GetTree() {
                
                #line 175 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<bool>> expression = () => 
                              QuoteBusinessLogic.IsApprovedByAllApprovers(QuoteId) == true;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public bool @__Expr3Get() {
                
                #line 175 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                return 
                              QuoteBusinessLogic.IsApprovedByAllApprovers(QuoteId) == true;
                
                #line default
                #line hidden
            }
            
            public bool ValueType___Expr3Get() {
                this.GetValueTypeValues();
                return this.@__Expr3Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr5GetTree() {
                
                #line 206 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                                      QuoteId;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr5Get() {
                
                #line 206 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                return 
                                      QuoteId;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr5Get() {
                this.GetValueTypeValues();
                return this.@__Expr5Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr6GetTree() {
                
                #line 211 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                                      Region;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr6Get() {
                
                #line 211 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                return 
                                      Region;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr6Get() {
                this.GetValueTypeValues();
                return this.@__Expr6Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr7GetTree() {
                
                #line 182 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                                      QuoteId;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr7Get() {
                
                #line 182 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                return 
                                      QuoteId;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr7Get() {
                this.GetValueTypeValues();
                return this.@__Expr7Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr8GetTree() {
                
                #line 187 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                                      Region;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr8Get() {
                
                #line 187 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                return 
                                      Region;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr8Get() {
                this.GetValueTypeValues();
                return this.@__Expr8Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr9GetTree() {
                
                #line 106 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.ApprovalResult>> expression = () => 
                              ApprovalResult.Finish;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.ApprovalResult @__Expr9Get() {
                
                #line 106 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                return 
                              ApprovalResult.Finish;
                
                #line default
                #line hidden
            }
            
            public WorkFlowlAPI.ApprovalResult ValueType___Expr9Get() {
                this.GetValueTypeValues();
                return this.@__Expr9Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr11GetTree() {
                
                #line 127 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>> expression = () => 
                                    AdditionalApprovals;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.WorkFlowApproval> @__Expr11Get() {
                
                #line 127 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                return 
                                    AdditionalApprovals;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.WorkFlowApproval> ValueType___Expr11Get() {
                this.GetValueTypeValues();
                return this.@__Expr11Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr18GetTree() {
                
                #line 87 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                          QuoteId;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr18Get() {
                
                #line 87 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                return 
                          QuoteId;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr18Get() {
                this.GetValueTypeValues();
                return this.@__Expr18Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr19GetTree() {
                
                #line 92 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                          Region;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr19Get() {
                
                #line 92 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                return 
                          Region;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr19Get() {
                this.GetValueTypeValues();
                return this.@__Expr19Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr20GetTree() {
                
                #line 73 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                  QuoteId;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr20Get() {
                
                #line 73 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                return 
                  QuoteId;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr20Get() {
                this.GetValueTypeValues();
                return this.@__Expr20Get();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 5))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 5);
                }
                expectedLocationsCount = 5;
                if (((locationReferences[(offset + 4)].Name != "AdditionalApprovals") 
                            || (locationReferences[(offset + 4)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>)))) {
                    return false;
                }
                return QuoteApprovalFlow_TypedDataContext1_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class QuoteApprovalFlow_TypedDataContext3 : QuoteApprovalFlow_TypedDataContext2 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public QuoteApprovalFlow_TypedDataContext3(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public QuoteApprovalFlow_TypedDataContext3(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public QuoteApprovalFlow_TypedDataContext3(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.WorkFlowApproval approval {
                get {
                    return ((Advantech.Myadvantech.DataAccess.WorkFlowApproval)(this.GetVariableValue((5 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((5 + locationsOffset), value);
                }
            }
            
            internal new static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public new virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
                base.SetLocationsOffset(locationsOffset);
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 6))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 6);
                }
                expectedLocationsCount = 6;
                if (((locationReferences[(offset + 5)].Name != "approval") 
                            || (locationReferences[(offset + 5)].Type != typeof(Advantech.Myadvantech.DataAccess.WorkFlowApproval)))) {
                    return false;
                }
                return QuoteApprovalFlow_TypedDataContext2.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class QuoteApprovalFlow_TypedDataContext3_ForReadOnly : QuoteApprovalFlow_TypedDataContext2_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public QuoteApprovalFlow_TypedDataContext3_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public QuoteApprovalFlow_TypedDataContext3_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public QuoteApprovalFlow_TypedDataContext3_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.WorkFlowApproval approval {
                get {
                    return ((Advantech.Myadvantech.DataAccess.WorkFlowApproval)(this.GetVariableValue((5 + locationsOffset))));
                }
            }
            
            internal new static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public new virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
                base.SetLocationsOffset(locationsOffset);
            }
            
            internal System.Linq.Expressions.Expression @__Expr12GetTree() {
                
                #line 142 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                                        approval.ApproverType;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr12Get() {
                
                #line 142 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                return 
                                        approval.ApproverType;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr12Get() {
                this.GetValueTypeValues();
                return this.@__Expr12Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr13GetTree() {
                
                #line 147 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<double>> expression = () => 
                                        (double)approval.LevelNum;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public double @__Expr13Get() {
                
                #line 147 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                return 
                                        (double)approval.LevelNum;
                
                #line default
                #line hidden
            }
            
            public double ValueType___Expr13Get() {
                this.GetValueTypeValues();
                return this.@__Expr13Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr14GetTree() {
                
                #line 152 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                                        QuoteId;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr14Get() {
                
                #line 152 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                return 
                                        QuoteId;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr14Get() {
                this.GetValueTypeValues();
                return this.@__Expr14Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr15GetTree() {
                
                #line 157 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                                        Url;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr15Get() {
                
                #line 157 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                return 
                                        Url;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr15Get() {
                this.GetValueTypeValues();
                return this.@__Expr15Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr16GetTree() {
                
                #line 162 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<bool>> expression = () => 
                                        Convert.ToBoolean(approval.ViewGP);
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public bool @__Expr16Get() {
                
                #line 162 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                return 
                                        Convert.ToBoolean(approval.ViewGP);
                
                #line default
                #line hidden
            }
            
            public bool ValueType___Expr16Get() {
                this.GetValueTypeValues();
                return this.@__Expr16Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr17GetTree() {
                
                #line 137 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                                        approval.Approver;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr17Get() {
                
                #line 137 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\QUOTEAPPROVALFLOW.XAML"
                return 
                                        approval.Approver;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr17Get() {
                this.GetValueTypeValues();
                return this.@__Expr17Get();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 6))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 6);
                }
                expectedLocationsCount = 6;
                if (((locationReferences[(offset + 5)].Name != "approval") 
                            || (locationReferences[(offset + 5)].Type != typeof(Advantech.Myadvantech.DataAccess.WorkFlowApproval)))) {
                    return false;
                }
                return QuoteApprovalFlow_TypedDataContext2_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
    }
}
