namespace WorkFlowlAPI {
    
    #line 29 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\FindApproverFlow.xaml"
    using System;
    
    #line default
    #line hidden
    
    #line 1 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\FindApproverFlow.xaml"
    using System.Collections;
    
    #line default
    #line hidden
    
    #line 30 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\FindApproverFlow.xaml"
    using System.Collections.Generic;
    
    #line default
    #line hidden
    
    #line 1 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\FindApproverFlow.xaml"
    using System.Activities;
    
    #line default
    #line hidden
    
    #line 1 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\FindApproverFlow.xaml"
    using System.Activities.Expressions;
    
    #line default
    #line hidden
    
    #line 1 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\FindApproverFlow.xaml"
    using System.Activities.Statements;
    
    #line default
    #line hidden
    
    #line 31 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\FindApproverFlow.xaml"
    using System.Data;
    
    #line default
    #line hidden
    
    #line 32 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\FindApproverFlow.xaml"
    using System.Linq;
    
    #line default
    #line hidden
    
    #line 33 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\FindApproverFlow.xaml"
    using System.Text;
    
    #line default
    #line hidden
    
    #line 34 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\FindApproverFlow.xaml"
    using WorkFlowlAPI;
    
    #line default
    #line hidden
    
    #line 35 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\FindApproverFlow.xaml"
    using Advantech.Myadvantech.DataAccess;
    
    #line default
    #line hidden
    
    #line 36 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\FindApproverFlow.xaml"
    using Advantech.Myadvantech.Business;
    
    #line default
    #line hidden
    
    #line 1 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\FindApproverFlow.xaml"
    using System.Activities.XamlIntegration;
    
    #line default
    #line hidden
    
    
    public partial class FindApproverFlow : System.Activities.XamlIntegration.ICompiledExpressionRoot {
        
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
                this.dataContextActivities = FindApproverFlow_TypedDataContext2_ForReadOnly.GetDataContextActivitiesHelper(this.rootActivity, this.forImplementation);
            }
            if ((expressionId == 0)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext0 = ((FindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext0.ValueType___Expr0Get();
            }
            if ((expressionId == 1)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new FindApproverFlow_TypedDataContext2(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2 refDataContext1 = ((FindApproverFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext1.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>(refDataContext1.ValueType___Expr1Get, refDataContext1.ValueType___Expr1Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 2)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext2 = ((FindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext2.ValueType___Expr2Get();
            }
            if ((expressionId == 3)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new FindApproverFlow_TypedDataContext2(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2 refDataContext3 = ((FindApproverFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext3.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>>(refDataContext3.ValueType___Expr3Get, refDataContext3.ValueType___Expr3Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 4)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext4 = ((FindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext4.ValueType___Expr4Get();
            }
            if ((expressionId == 5)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new FindApproverFlow_TypedDataContext2(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2 refDataContext5 = ((FindApproverFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext5.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>>(refDataContext5.ValueType___Expr5Get, refDataContext5.ValueType___Expr5Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 6)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext6 = ((FindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext6.ValueType___Expr6Get();
            }
            if ((expressionId == 7)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext7 = ((FindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext7.ValueType___Expr7Get();
            }
            if ((expressionId == 8)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new FindApproverFlow_TypedDataContext2(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2 refDataContext8 = ((FindApproverFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext8.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext8.ValueType___Expr8Get, refDataContext8.ValueType___Expr8Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 9)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext9 = ((FindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext9.ValueType___Expr9Get();
            }
            if ((expressionId == 10)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext10 = ((FindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext10.ValueType___Expr10Get();
            }
            if ((expressionId == 11)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new FindApproverFlow_TypedDataContext2(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2 refDataContext11 = ((FindApproverFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext11.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>(refDataContext11.ValueType___Expr11Get, refDataContext11.ValueType___Expr11Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 12)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new FindApproverFlow_TypedDataContext2(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2 refDataContext12 = ((FindApproverFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext12.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext12.ValueType___Expr12Get, refDataContext12.ValueType___Expr12Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 13)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext13 = ((FindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext13.ValueType___Expr13Get();
            }
            if ((expressionId == 14)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext14 = ((FindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext14.ValueType___Expr14Get();
            }
            if ((expressionId == 15)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new FindApproverFlow_TypedDataContext2(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2 refDataContext15 = ((FindApproverFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext15.GetLocation<string>(refDataContext15.ValueType___Expr15Get, refDataContext15.ValueType___Expr15Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 16)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext16 = ((FindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext16.ValueType___Expr16Get();
            }
            if ((expressionId == 17)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext17 = ((FindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext17.ValueType___Expr17Get();
            }
            if ((expressionId == 18)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext18 = ((FindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext18.ValueType___Expr18Get();
            }
            if ((expressionId == 19)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext19 = ((FindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext19.ValueType___Expr19Get();
            }
            if ((expressionId == 20)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext20 = ((FindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext20.ValueType___Expr20Get();
            }
            if ((expressionId == 21)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new FindApproverFlow_TypedDataContext2(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2 refDataContext21 = ((FindApproverFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext21.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>(refDataContext21.ValueType___Expr21Get, refDataContext21.ValueType___Expr21Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 22)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext22 = ((FindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext22.ValueType___Expr22Get();
            }
            if ((expressionId == 23)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new FindApproverFlow_TypedDataContext2(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2 refDataContext23 = ((FindApproverFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext23.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext23.ValueType___Expr23Get, refDataContext23.ValueType___Expr23Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 24)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext24 = ((FindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext24.ValueType___Expr24Get();
            }
            if ((expressionId == 25)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext25 = ((FindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext25.ValueType___Expr25Get();
            }
            if ((expressionId == 26)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext26 = ((FindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext26.ValueType___Expr26Get();
            }
            if ((expressionId == 27)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext27 = ((FindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext27.ValueType___Expr27Get();
            }
            if ((expressionId == 28)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext28 = ((FindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext28.ValueType___Expr28Get();
            }
            if ((expressionId == 29)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext29 = ((FindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext29.ValueType___Expr29Get();
            }
            if ((expressionId == 30)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new FindApproverFlow_TypedDataContext2(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2 refDataContext30 = ((FindApproverFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext30.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>(refDataContext30.ValueType___Expr30Get, refDataContext30.ValueType___Expr30Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 31)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new FindApproverFlow_TypedDataContext2(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2 refDataContext31 = ((FindApproverFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext31.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext31.ValueType___Expr31Get, refDataContext31.ValueType___Expr31Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 32)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext32 = ((FindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext32.ValueType___Expr32Get();
            }
            if ((expressionId == 33)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext33 = ((FindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext33.ValueType___Expr33Get();
            }
            if ((expressionId == 34)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = FindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 2);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext34 = ((FindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext34.ValueType___Expr34Get();
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
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext0 = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext0.ValueType___Expr0Get();
            }
            if ((expressionId == 1)) {
                FindApproverFlow_TypedDataContext2 refDataContext1 = new FindApproverFlow_TypedDataContext2(locations, true);
                return refDataContext1.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>(refDataContext1.ValueType___Expr1Get, refDataContext1.ValueType___Expr1Set);
            }
            if ((expressionId == 2)) {
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext2 = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext2.ValueType___Expr2Get();
            }
            if ((expressionId == 3)) {
                FindApproverFlow_TypedDataContext2 refDataContext3 = new FindApproverFlow_TypedDataContext2(locations, true);
                return refDataContext3.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>>(refDataContext3.ValueType___Expr3Get, refDataContext3.ValueType___Expr3Set);
            }
            if ((expressionId == 4)) {
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext4 = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext4.ValueType___Expr4Get();
            }
            if ((expressionId == 5)) {
                FindApproverFlow_TypedDataContext2 refDataContext5 = new FindApproverFlow_TypedDataContext2(locations, true);
                return refDataContext5.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>>(refDataContext5.ValueType___Expr5Get, refDataContext5.ValueType___Expr5Set);
            }
            if ((expressionId == 6)) {
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext6 = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext6.ValueType___Expr6Get();
            }
            if ((expressionId == 7)) {
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext7 = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext7.ValueType___Expr7Get();
            }
            if ((expressionId == 8)) {
                FindApproverFlow_TypedDataContext2 refDataContext8 = new FindApproverFlow_TypedDataContext2(locations, true);
                return refDataContext8.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext8.ValueType___Expr8Get, refDataContext8.ValueType___Expr8Set);
            }
            if ((expressionId == 9)) {
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext9 = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext9.ValueType___Expr9Get();
            }
            if ((expressionId == 10)) {
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext10 = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext10.ValueType___Expr10Get();
            }
            if ((expressionId == 11)) {
                FindApproverFlow_TypedDataContext2 refDataContext11 = new FindApproverFlow_TypedDataContext2(locations, true);
                return refDataContext11.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>(refDataContext11.ValueType___Expr11Get, refDataContext11.ValueType___Expr11Set);
            }
            if ((expressionId == 12)) {
                FindApproverFlow_TypedDataContext2 refDataContext12 = new FindApproverFlow_TypedDataContext2(locations, true);
                return refDataContext12.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext12.ValueType___Expr12Get, refDataContext12.ValueType___Expr12Set);
            }
            if ((expressionId == 13)) {
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext13 = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext13.ValueType___Expr13Get();
            }
            if ((expressionId == 14)) {
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext14 = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext14.ValueType___Expr14Get();
            }
            if ((expressionId == 15)) {
                FindApproverFlow_TypedDataContext2 refDataContext15 = new FindApproverFlow_TypedDataContext2(locations, true);
                return refDataContext15.GetLocation<string>(refDataContext15.ValueType___Expr15Get, refDataContext15.ValueType___Expr15Set);
            }
            if ((expressionId == 16)) {
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext16 = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext16.ValueType___Expr16Get();
            }
            if ((expressionId == 17)) {
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext17 = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext17.ValueType___Expr17Get();
            }
            if ((expressionId == 18)) {
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext18 = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext18.ValueType___Expr18Get();
            }
            if ((expressionId == 19)) {
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext19 = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext19.ValueType___Expr19Get();
            }
            if ((expressionId == 20)) {
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext20 = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext20.ValueType___Expr20Get();
            }
            if ((expressionId == 21)) {
                FindApproverFlow_TypedDataContext2 refDataContext21 = new FindApproverFlow_TypedDataContext2(locations, true);
                return refDataContext21.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>(refDataContext21.ValueType___Expr21Get, refDataContext21.ValueType___Expr21Set);
            }
            if ((expressionId == 22)) {
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext22 = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext22.ValueType___Expr22Get();
            }
            if ((expressionId == 23)) {
                FindApproverFlow_TypedDataContext2 refDataContext23 = new FindApproverFlow_TypedDataContext2(locations, true);
                return refDataContext23.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext23.ValueType___Expr23Get, refDataContext23.ValueType___Expr23Set);
            }
            if ((expressionId == 24)) {
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext24 = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext24.ValueType___Expr24Get();
            }
            if ((expressionId == 25)) {
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext25 = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext25.ValueType___Expr25Get();
            }
            if ((expressionId == 26)) {
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext26 = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext26.ValueType___Expr26Get();
            }
            if ((expressionId == 27)) {
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext27 = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext27.ValueType___Expr27Get();
            }
            if ((expressionId == 28)) {
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext28 = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext28.ValueType___Expr28Get();
            }
            if ((expressionId == 29)) {
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext29 = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext29.ValueType___Expr29Get();
            }
            if ((expressionId == 30)) {
                FindApproverFlow_TypedDataContext2 refDataContext30 = new FindApproverFlow_TypedDataContext2(locations, true);
                return refDataContext30.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>(refDataContext30.ValueType___Expr30Get, refDataContext30.ValueType___Expr30Set);
            }
            if ((expressionId == 31)) {
                FindApproverFlow_TypedDataContext2 refDataContext31 = new FindApproverFlow_TypedDataContext2(locations, true);
                return refDataContext31.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext31.ValueType___Expr31Get, refDataContext31.ValueType___Expr31Set);
            }
            if ((expressionId == 32)) {
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext32 = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext32.ValueType___Expr32Get();
            }
            if ((expressionId == 33)) {
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext33 = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext33.ValueType___Expr33Get();
            }
            if ((expressionId == 34)) {
                FindApproverFlow_TypedDataContext2_ForReadOnly valDataContext34 = new FindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext34.ValueType___Expr34Get();
            }
            return null;
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool CanExecuteExpression(string expressionText, bool isReference, System.Collections.Generic.IList<System.Activities.LocationReference> locations, out int expressionId) {
            if (((isReference == false) 
                        && ((expressionText == "new List<WorkFlowApproval>()") 
                        && (FindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 0;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "ApprovalList") 
                        && (FindApproverFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 1;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuoteBusinessLogic.RemoveServicePartInQuotationDetails(QuotationMaster.QuotationD" +
                            "etail)") 
                        && (FindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 2;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "QuotationDetailsWithoutServicePart") 
                        && (FindApproverFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 3;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationMaster.QuotationDetail.Where(q=>q.IsEWpartnoX == true).ToList()") 
                        && (FindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 4;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "EWQuotationDetails") 
                        && (FindApproverFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 5;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "Region") 
                        && (FindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 6;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "FindApproverResult.NoNeed") 
                        && (FindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 7;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "Result") 
                        && (FindApproverFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 8;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationMaster.GetQuotationSalesRepresentative()") 
                        && (FindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 9;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationMaster.GetQuotationSalesRepresentativeSalesCode()") 
                        && (FindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 10;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "ApprovalList") 
                        && (FindApproverFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 11;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "Result") 
                        && (FindApproverFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 12;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationMaster.quoteId") 
                        && (FindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 13;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "Url") 
                        && (FindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 14;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "BU_Sector") 
                        && (FindApproverFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 15;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationDetailsWithoutServicePart") 
                        && (FindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 16;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationMaster.GetQuotationSalesRepresentative()") 
                        && (FindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 17;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationMaster.quoteToErpId") 
                        && (FindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 18;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationMaster.GetQuotationSalesRepresentativeSalesCode()") 
                        && (FindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 19;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationMaster.quoteDate.Value") 
                        && (FindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 20;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "ApprovalList") 
                        && (FindApproverFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 21;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationMaster.expiredDate.Value") 
                        && (FindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 22;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "Result") 
                        && (FindApproverFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 23;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationMaster.quoteId") 
                        && (FindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 24;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "Url") 
                        && (FindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 25;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationDetailsWithoutServicePart") 
                        && (FindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 26;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationMaster.GetQuotationSalesRepresentative()") 
                        && (FindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 27;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationMaster.GetQuotationSalesRepresentativeSalesCode()") 
                        && (FindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 28;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "EWQuotationDetails") 
                        && (FindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 29;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "ApprovalList") 
                        && (FindApproverFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 30;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "Result") 
                        && (FindApproverFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 31;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationMaster.quoteId") 
                        && (FindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 32;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "Url") 
                        && (FindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 33;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationDetailsWithoutServicePart") 
                        && (FindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 34;
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
                return new FindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr0GetTree();
            }
            if ((expressionId == 1)) {
                return new FindApproverFlow_TypedDataContext2(locationReferences).@__Expr1GetTree();
            }
            if ((expressionId == 2)) {
                return new FindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr2GetTree();
            }
            if ((expressionId == 3)) {
                return new FindApproverFlow_TypedDataContext2(locationReferences).@__Expr3GetTree();
            }
            if ((expressionId == 4)) {
                return new FindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr4GetTree();
            }
            if ((expressionId == 5)) {
                return new FindApproverFlow_TypedDataContext2(locationReferences).@__Expr5GetTree();
            }
            if ((expressionId == 6)) {
                return new FindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr6GetTree();
            }
            if ((expressionId == 7)) {
                return new FindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr7GetTree();
            }
            if ((expressionId == 8)) {
                return new FindApproverFlow_TypedDataContext2(locationReferences).@__Expr8GetTree();
            }
            if ((expressionId == 9)) {
                return new FindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr9GetTree();
            }
            if ((expressionId == 10)) {
                return new FindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr10GetTree();
            }
            if ((expressionId == 11)) {
                return new FindApproverFlow_TypedDataContext2(locationReferences).@__Expr11GetTree();
            }
            if ((expressionId == 12)) {
                return new FindApproverFlow_TypedDataContext2(locationReferences).@__Expr12GetTree();
            }
            if ((expressionId == 13)) {
                return new FindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr13GetTree();
            }
            if ((expressionId == 14)) {
                return new FindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr14GetTree();
            }
            if ((expressionId == 15)) {
                return new FindApproverFlow_TypedDataContext2(locationReferences).@__Expr15GetTree();
            }
            if ((expressionId == 16)) {
                return new FindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr16GetTree();
            }
            if ((expressionId == 17)) {
                return new FindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr17GetTree();
            }
            if ((expressionId == 18)) {
                return new FindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr18GetTree();
            }
            if ((expressionId == 19)) {
                return new FindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr19GetTree();
            }
            if ((expressionId == 20)) {
                return new FindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr20GetTree();
            }
            if ((expressionId == 21)) {
                return new FindApproverFlow_TypedDataContext2(locationReferences).@__Expr21GetTree();
            }
            if ((expressionId == 22)) {
                return new FindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr22GetTree();
            }
            if ((expressionId == 23)) {
                return new FindApproverFlow_TypedDataContext2(locationReferences).@__Expr23GetTree();
            }
            if ((expressionId == 24)) {
                return new FindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr24GetTree();
            }
            if ((expressionId == 25)) {
                return new FindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr25GetTree();
            }
            if ((expressionId == 26)) {
                return new FindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr26GetTree();
            }
            if ((expressionId == 27)) {
                return new FindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr27GetTree();
            }
            if ((expressionId == 28)) {
                return new FindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr28GetTree();
            }
            if ((expressionId == 29)) {
                return new FindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr29GetTree();
            }
            if ((expressionId == 30)) {
                return new FindApproverFlow_TypedDataContext2(locationReferences).@__Expr30GetTree();
            }
            if ((expressionId == 31)) {
                return new FindApproverFlow_TypedDataContext2(locationReferences).@__Expr31GetTree();
            }
            if ((expressionId == 32)) {
                return new FindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr32GetTree();
            }
            if ((expressionId == 33)) {
                return new FindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr33GetTree();
            }
            if ((expressionId == 34)) {
                return new FindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr34GetTree();
            }
            return null;
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class FindApproverFlow_TypedDataContext0 : System.Activities.XamlIntegration.CompiledDataContext {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public FindApproverFlow_TypedDataContext0(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public FindApproverFlow_TypedDataContext0(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public FindApproverFlow_TypedDataContext0(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
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
        private class FindApproverFlow_TypedDataContext0_ForReadOnly : System.Activities.XamlIntegration.CompiledDataContext {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public FindApproverFlow_TypedDataContext0_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public FindApproverFlow_TypedDataContext0_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public FindApproverFlow_TypedDataContext0_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
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
        private class FindApproverFlow_TypedDataContext1 : FindApproverFlow_TypedDataContext0 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            protected WorkFlowlAPI.FindApproverResult Result;
            
            public FindApproverFlow_TypedDataContext1(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public FindApproverFlow_TypedDataContext1(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public FindApproverFlow_TypedDataContext1(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationMaster QuotationMaster {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationMaster)(this.GetVariableValue((0 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((0 + locationsOffset), value);
                }
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> ApprovalList {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>)(this.GetVariableValue((1 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((1 + locationsOffset), value);
                }
            }
            
            protected string Url {
                get {
                    return ((string)(this.GetVariableValue((3 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((3 + locationsOffset), value);
                }
            }
            
            protected string Region {
                get {
                    return ((string)(this.GetVariableValue((4 + locationsOffset))));
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
            
            protected override void GetValueTypeValues() {
                this.Result = ((WorkFlowlAPI.FindApproverResult)(this.GetVariableValue((2 + locationsOffset))));
                base.GetValueTypeValues();
            }
            
            protected override void SetValueTypeValues() {
                this.SetVariableValue((2 + locationsOffset), this.Result);
                base.SetValueTypeValues();
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
                if (((locationReferences[(offset + 0)].Name != "QuotationMaster") 
                            || (locationReferences[(offset + 0)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationMaster)))) {
                    return false;
                }
                if (((locationReferences[(offset + 1)].Name != "ApprovalList") 
                            || (locationReferences[(offset + 1)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>)))) {
                    return false;
                }
                if (((locationReferences[(offset + 3)].Name != "Url") 
                            || (locationReferences[(offset + 3)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 4)].Name != "Region") 
                            || (locationReferences[(offset + 4)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 2)].Name != "Result") 
                            || (locationReferences[(offset + 2)].Type != typeof(WorkFlowlAPI.FindApproverResult)))) {
                    return false;
                }
                return FindApproverFlow_TypedDataContext0.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class FindApproverFlow_TypedDataContext1_ForReadOnly : FindApproverFlow_TypedDataContext0_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            protected WorkFlowlAPI.FindApproverResult Result;
            
            public FindApproverFlow_TypedDataContext1_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public FindApproverFlow_TypedDataContext1_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public FindApproverFlow_TypedDataContext1_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationMaster QuotationMaster {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationMaster)(this.GetVariableValue((0 + locationsOffset))));
                }
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> ApprovalList {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>)(this.GetVariableValue((1 + locationsOffset))));
                }
            }
            
            protected string Url {
                get {
                    return ((string)(this.GetVariableValue((3 + locationsOffset))));
                }
            }
            
            protected string Region {
                get {
                    return ((string)(this.GetVariableValue((4 + locationsOffset))));
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
                this.Result = ((WorkFlowlAPI.FindApproverResult)(this.GetVariableValue((2 + locationsOffset))));
                base.GetValueTypeValues();
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
                if (((locationReferences[(offset + 0)].Name != "QuotationMaster") 
                            || (locationReferences[(offset + 0)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationMaster)))) {
                    return false;
                }
                if (((locationReferences[(offset + 1)].Name != "ApprovalList") 
                            || (locationReferences[(offset + 1)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>)))) {
                    return false;
                }
                if (((locationReferences[(offset + 3)].Name != "Url") 
                            || (locationReferences[(offset + 3)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 4)].Name != "Region") 
                            || (locationReferences[(offset + 4)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 2)].Name != "Result") 
                            || (locationReferences[(offset + 2)].Type != typeof(WorkFlowlAPI.FindApproverResult)))) {
                    return false;
                }
                return FindApproverFlow_TypedDataContext0_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class FindApproverFlow_TypedDataContext2 : FindApproverFlow_TypedDataContext1 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            protected bool BBGPRuleIsFound;
            
            protected WorkFlowlAPI.FindApproverResult FindApproverResult;
            
            protected bool IsNeedPSMApprover;
            
            public FindApproverFlow_TypedDataContext2(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public FindApproverFlow_TypedDataContext2(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public FindApproverFlow_TypedDataContext2(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected string BU_Sector {
                get {
                    return ((string)(this.GetVariableValue((5 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((5 + locationsOffset), value);
                }
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> QuotationDetailsWithoutServicePart {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)(this.GetVariableValue((7 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((7 + locationsOffset), value);
                }
            }
            
            protected string[] Approvers {
                get {
                    return ((string[])(this.GetVariableValue((9 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((9 + locationsOffset), value);
                }
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> EWQuotationDetails {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)(this.GetVariableValue((11 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((11 + locationsOffset), value);
                }
            }
            
            internal new static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public new virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
                base.SetLocationsOffset(locationsOffset);
            }
            
            internal System.Linq.Expressions.Expression @__Expr1GetTree() {
                
                #line 79 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>> expression = () => 
              ApprovalList;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> @__Expr1Get() {
                
                #line 79 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
              ApprovalList;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> ValueType___Expr1Get() {
                this.GetValueTypeValues();
                return this.@__Expr1Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr1Set(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> value) {
                
                #line 79 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                
              ApprovalList = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr1Set(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> value) {
                this.GetValueTypeValues();
                this.@__Expr1Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr3GetTree() {
                
                #line 93 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                  QuotationDetailsWithoutServicePart;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr3Get() {
                
                #line 93 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                  QuotationDetailsWithoutServicePart;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr3Get() {
                this.GetValueTypeValues();
                return this.@__Expr3Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr3Set(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> value) {
                
                #line 93 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                
                  QuotationDetailsWithoutServicePart = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr3Set(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> value) {
                this.GetValueTypeValues();
                this.@__Expr3Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr5GetTree() {
                
                #line 107 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                      EWQuotationDetails;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr5Get() {
                
                #line 107 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                      EWQuotationDetails;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr5Get() {
                this.GetValueTypeValues();
                return this.@__Expr5Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr5Set(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> value) {
                
                #line 107 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                
                      EWQuotationDetails = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr5Set(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> value) {
                this.GetValueTypeValues();
                this.@__Expr5Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr8GetTree() {
                
                #line 123 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.FindApproverResult>> expression = () => 
                              Result;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.FindApproverResult @__Expr8Get() {
                
                #line 123 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                              Result;
                
                #line default
                #line hidden
            }
            
            public WorkFlowlAPI.FindApproverResult ValueType___Expr8Get() {
                this.GetValueTypeValues();
                return this.@__Expr8Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr8Set(WorkFlowlAPI.FindApproverResult value) {
                
                #line 123 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                
                              Result = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr8Set(WorkFlowlAPI.FindApproverResult value) {
                this.GetValueTypeValues();
                this.@__Expr8Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr11GetTree() {
                
                #line 239 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>> expression = () => 
                            ApprovalList;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> @__Expr11Get() {
                
                #line 239 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                            ApprovalList;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> ValueType___Expr11Get() {
                this.GetValueTypeValues();
                return this.@__Expr11Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr11Set(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> value) {
                
                #line 239 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                
                            ApprovalList = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr11Set(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> value) {
                this.GetValueTypeValues();
                this.@__Expr11Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr12GetTree() {
                
                #line 249 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.FindApproverResult>> expression = () => 
                            Result;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.FindApproverResult @__Expr12Get() {
                
                #line 249 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                            Result;
                
                #line default
                #line hidden
            }
            
            public WorkFlowlAPI.FindApproverResult ValueType___Expr12Get() {
                this.GetValueTypeValues();
                return this.@__Expr12Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr12Set(WorkFlowlAPI.FindApproverResult value) {
                
                #line 249 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                
                            Result = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr12Set(WorkFlowlAPI.FindApproverResult value) {
                this.GetValueTypeValues();
                this.@__Expr12Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr15GetTree() {
                
                #line 244 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                            BU_Sector;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr15Get() {
                
                #line 244 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                            BU_Sector;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr15Get() {
                this.GetValueTypeValues();
                return this.@__Expr15Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr15Set(string value) {
                
                #line 244 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                
                            BU_Sector = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr15Set(string value) {
                this.GetValueTypeValues();
                this.@__Expr15Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr21GetTree() {
                
                #line 185 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>> expression = () => 
                            ApprovalList;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> @__Expr21Get() {
                
                #line 185 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                            ApprovalList;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> ValueType___Expr21Get() {
                this.GetValueTypeValues();
                return this.@__Expr21Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr21Set(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> value) {
                
                #line 185 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                
                            ApprovalList = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr21Set(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> value) {
                this.GetValueTypeValues();
                this.@__Expr21Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr23GetTree() {
                
                #line 195 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.FindApproverResult>> expression = () => 
                            Result;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.FindApproverResult @__Expr23Get() {
                
                #line 195 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                            Result;
                
                #line default
                #line hidden
            }
            
            public WorkFlowlAPI.FindApproverResult ValueType___Expr23Get() {
                this.GetValueTypeValues();
                return this.@__Expr23Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr23Set(WorkFlowlAPI.FindApproverResult value) {
                
                #line 195 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                
                            Result = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr23Set(WorkFlowlAPI.FindApproverResult value) {
                this.GetValueTypeValues();
                this.@__Expr23Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr30GetTree() {
                
                #line 141 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>> expression = () => 
                            ApprovalList;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> @__Expr30Get() {
                
                #line 141 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                            ApprovalList;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> ValueType___Expr30Get() {
                this.GetValueTypeValues();
                return this.@__Expr30Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr30Set(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> value) {
                
                #line 141 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                
                            ApprovalList = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr30Set(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> value) {
                this.GetValueTypeValues();
                this.@__Expr30Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr31GetTree() {
                
                #line 151 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.FindApproverResult>> expression = () => 
                            Result;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.FindApproverResult @__Expr31Get() {
                
                #line 151 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                            Result;
                
                #line default
                #line hidden
            }
            
            public WorkFlowlAPI.FindApproverResult ValueType___Expr31Get() {
                this.GetValueTypeValues();
                return this.@__Expr31Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr31Set(WorkFlowlAPI.FindApproverResult value) {
                
                #line 151 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                
                            Result = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr31Set(WorkFlowlAPI.FindApproverResult value) {
                this.GetValueTypeValues();
                this.@__Expr31Set(value);
                this.SetValueTypeValues();
            }
            
            protected override void GetValueTypeValues() {
                this.BBGPRuleIsFound = ((bool)(this.GetVariableValue((6 + locationsOffset))));
                this.FindApproverResult = ((WorkFlowlAPI.FindApproverResult)(this.GetVariableValue((8 + locationsOffset))));
                this.IsNeedPSMApprover = ((bool)(this.GetVariableValue((10 + locationsOffset))));
                base.GetValueTypeValues();
            }
            
            protected override void SetValueTypeValues() {
                this.SetVariableValue((6 + locationsOffset), this.BBGPRuleIsFound);
                this.SetVariableValue((8 + locationsOffset), this.FindApproverResult);
                this.SetVariableValue((10 + locationsOffset), this.IsNeedPSMApprover);
                base.SetValueTypeValues();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 12))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 12);
                }
                expectedLocationsCount = 12;
                if (((locationReferences[(offset + 5)].Name != "BU_Sector") 
                            || (locationReferences[(offset + 5)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 7)].Name != "QuotationDetailsWithoutServicePart") 
                            || (locationReferences[(offset + 7)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)))) {
                    return false;
                }
                if (((locationReferences[(offset + 9)].Name != "Approvers") 
                            || (locationReferences[(offset + 9)].Type != typeof(string[])))) {
                    return false;
                }
                if (((locationReferences[(offset + 11)].Name != "EWQuotationDetails") 
                            || (locationReferences[(offset + 11)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)))) {
                    return false;
                }
                if (((locationReferences[(offset + 6)].Name != "BBGPRuleIsFound") 
                            || (locationReferences[(offset + 6)].Type != typeof(bool)))) {
                    return false;
                }
                if (((locationReferences[(offset + 8)].Name != "FindApproverResult") 
                            || (locationReferences[(offset + 8)].Type != typeof(WorkFlowlAPI.FindApproverResult)))) {
                    return false;
                }
                if (((locationReferences[(offset + 10)].Name != "IsNeedPSMApprover") 
                            || (locationReferences[(offset + 10)].Type != typeof(bool)))) {
                    return false;
                }
                return FindApproverFlow_TypedDataContext1.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class FindApproverFlow_TypedDataContext2_ForReadOnly : FindApproverFlow_TypedDataContext1_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            protected bool BBGPRuleIsFound;
            
            protected WorkFlowlAPI.FindApproverResult FindApproverResult;
            
            protected bool IsNeedPSMApprover;
            
            public FindApproverFlow_TypedDataContext2_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public FindApproverFlow_TypedDataContext2_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public FindApproverFlow_TypedDataContext2_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected string BU_Sector {
                get {
                    return ((string)(this.GetVariableValue((5 + locationsOffset))));
                }
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> QuotationDetailsWithoutServicePart {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)(this.GetVariableValue((7 + locationsOffset))));
                }
            }
            
            protected string[] Approvers {
                get {
                    return ((string[])(this.GetVariableValue((9 + locationsOffset))));
                }
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> EWQuotationDetails {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)(this.GetVariableValue((11 + locationsOffset))));
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
                
                #line 84 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>> expression = () => 
              new List<WorkFlowApproval>();
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> @__Expr0Get() {
                
                #line 84 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
              new List<WorkFlowApproval>();
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> ValueType___Expr0Get() {
                this.GetValueTypeValues();
                return this.@__Expr0Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr2GetTree() {
                
                #line 98 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                  QuoteBusinessLogic.RemoveServicePartInQuotationDetails(QuotationMaster.QuotationDetail);
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr2Get() {
                
                #line 98 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                  QuoteBusinessLogic.RemoveServicePartInQuotationDetails(QuotationMaster.QuotationDetail);
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr2Get() {
                this.GetValueTypeValues();
                return this.@__Expr2Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr4GetTree() {
                
                #line 112 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                      QuotationMaster.QuotationDetail.Where(q=>q.IsEWpartnoX == true).ToList();
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr4Get() {
                
                #line 112 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                      QuotationMaster.QuotationDetail.Where(q=>q.IsEWpartnoX == true).ToList();
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr4Get() {
                this.GetValueTypeValues();
                return this.@__Expr4Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr6GetTree() {
                
                #line 135 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                      Region;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr6Get() {
                
                #line 135 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
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
                
                #line 128 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.FindApproverResult>> expression = () => 
                              FindApproverResult.NoNeed;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.FindApproverResult @__Expr7Get() {
                
                #line 128 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                              FindApproverResult.NoNeed;
                
                #line default
                #line hidden
            }
            
            public WorkFlowlAPI.FindApproverResult ValueType___Expr7Get() {
                this.GetValueTypeValues();
                return this.@__Expr7Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr9GetTree() {
                
                #line 269 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                            QuotationMaster.GetQuotationSalesRepresentative();
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr9Get() {
                
                #line 269 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                            QuotationMaster.GetQuotationSalesRepresentative();
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr9Get() {
                this.GetValueTypeValues();
                return this.@__Expr9Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr10GetTree() {
                
                #line 264 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                            QuotationMaster.GetQuotationSalesRepresentativeSalesCode();
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr10Get() {
                
                #line 264 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                            QuotationMaster.GetQuotationSalesRepresentativeSalesCode();
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr10Get() {
                this.GetValueTypeValues();
                return this.@__Expr10Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr13GetTree() {
                
                #line 259 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                            QuotationMaster.quoteId;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr13Get() {
                
                #line 259 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                            QuotationMaster.quoteId;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr13Get() {
                this.GetValueTypeValues();
                return this.@__Expr13Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr14GetTree() {
                
                #line 274 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                            Url;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr14Get() {
                
                #line 274 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                            Url;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr14Get() {
                this.GetValueTypeValues();
                return this.@__Expr14Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr16GetTree() {
                
                #line 254 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                            QuotationDetailsWithoutServicePart;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr16Get() {
                
                #line 254 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                            QuotationDetailsWithoutServicePart;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr16Get() {
                this.GetValueTypeValues();
                return this.@__Expr16Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr17GetTree() {
                
                #line 225 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                            QuotationMaster.GetQuotationSalesRepresentative();
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr17Get() {
                
                #line 225 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                            QuotationMaster.GetQuotationSalesRepresentative();
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr17Get() {
                this.GetValueTypeValues();
                return this.@__Expr17Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr18GetTree() {
                
                #line 215 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                            QuotationMaster.quoteToErpId;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr18Get() {
                
                #line 215 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                            QuotationMaster.quoteToErpId;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr18Get() {
                this.GetValueTypeValues();
                return this.@__Expr18Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr19GetTree() {
                
                #line 220 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                            QuotationMaster.GetQuotationSalesRepresentativeSalesCode();
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr19Get() {
                
                #line 220 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                            QuotationMaster.GetQuotationSalesRepresentativeSalesCode();
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr19Get() {
                this.GetValueTypeValues();
                return this.@__Expr19Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr20GetTree() {
                
                #line 205 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.DateTime>> expression = () => 
                            QuotationMaster.quoteDate.Value;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.DateTime @__Expr20Get() {
                
                #line 205 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                            QuotationMaster.quoteDate.Value;
                
                #line default
                #line hidden
            }
            
            public System.DateTime ValueType___Expr20Get() {
                this.GetValueTypeValues();
                return this.@__Expr20Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr22GetTree() {
                
                #line 190 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.DateTime>> expression = () => 
                            QuotationMaster.expiredDate.Value;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.DateTime @__Expr22Get() {
                
                #line 190 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                            QuotationMaster.expiredDate.Value;
                
                #line default
                #line hidden
            }
            
            public System.DateTime ValueType___Expr22Get() {
                this.GetValueTypeValues();
                return this.@__Expr22Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr24GetTree() {
                
                #line 210 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                            QuotationMaster.quoteId;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr24Get() {
                
                #line 210 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                            QuotationMaster.quoteId;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr24Get() {
                this.GetValueTypeValues();
                return this.@__Expr24Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr25GetTree() {
                
                #line 230 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                            Url;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr25Get() {
                
                #line 230 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                            Url;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr25Get() {
                this.GetValueTypeValues();
                return this.@__Expr25Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr26GetTree() {
                
                #line 200 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                            QuotationDetailsWithoutServicePart;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr26Get() {
                
                #line 200 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                            QuotationDetailsWithoutServicePart;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr26Get() {
                this.GetValueTypeValues();
                return this.@__Expr26Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr27GetTree() {
                
                #line 171 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                            QuotationMaster.GetQuotationSalesRepresentative();
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr27Get() {
                
                #line 171 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                            QuotationMaster.GetQuotationSalesRepresentative();
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr27Get() {
                this.GetValueTypeValues();
                return this.@__Expr27Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr28GetTree() {
                
                #line 166 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                            QuotationMaster.GetQuotationSalesRepresentativeSalesCode();
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr28Get() {
                
                #line 166 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                            QuotationMaster.GetQuotationSalesRepresentativeSalesCode();
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr28Get() {
                this.GetValueTypeValues();
                return this.@__Expr28Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr29GetTree() {
                
                #line 146 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                            EWQuotationDetails;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr29Get() {
                
                #line 146 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                            EWQuotationDetails;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr29Get() {
                this.GetValueTypeValues();
                return this.@__Expr29Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr32GetTree() {
                
                #line 161 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                            QuotationMaster.quoteId;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr32Get() {
                
                #line 161 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                            QuotationMaster.quoteId;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr32Get() {
                this.GetValueTypeValues();
                return this.@__Expr32Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr33GetTree() {
                
                #line 176 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                            Url;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr33Get() {
                
                #line 176 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                            Url;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr33Get() {
                this.GetValueTypeValues();
                return this.@__Expr33Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr34GetTree() {
                
                #line 156 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                            QuotationDetailsWithoutServicePart;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr34Get() {
                
                #line 156 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\FINDAPPROVERFLOW.XAML"
                return 
                            QuotationDetailsWithoutServicePart;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr34Get() {
                this.GetValueTypeValues();
                return this.@__Expr34Get();
            }
            
            protected override void GetValueTypeValues() {
                this.BBGPRuleIsFound = ((bool)(this.GetVariableValue((6 + locationsOffset))));
                this.FindApproverResult = ((WorkFlowlAPI.FindApproverResult)(this.GetVariableValue((8 + locationsOffset))));
                this.IsNeedPSMApprover = ((bool)(this.GetVariableValue((10 + locationsOffset))));
                base.GetValueTypeValues();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 12))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 12);
                }
                expectedLocationsCount = 12;
                if (((locationReferences[(offset + 5)].Name != "BU_Sector") 
                            || (locationReferences[(offset + 5)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 7)].Name != "QuotationDetailsWithoutServicePart") 
                            || (locationReferences[(offset + 7)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)))) {
                    return false;
                }
                if (((locationReferences[(offset + 9)].Name != "Approvers") 
                            || (locationReferences[(offset + 9)].Type != typeof(string[])))) {
                    return false;
                }
                if (((locationReferences[(offset + 11)].Name != "EWQuotationDetails") 
                            || (locationReferences[(offset + 11)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)))) {
                    return false;
                }
                if (((locationReferences[(offset + 6)].Name != "BBGPRuleIsFound") 
                            || (locationReferences[(offset + 6)].Type != typeof(bool)))) {
                    return false;
                }
                if (((locationReferences[(offset + 8)].Name != "FindApproverResult") 
                            || (locationReferences[(offset + 8)].Type != typeof(WorkFlowlAPI.FindApproverResult)))) {
                    return false;
                }
                if (((locationReferences[(offset + 10)].Name != "IsNeedPSMApprover") 
                            || (locationReferences[(offset + 10)].Type != typeof(bool)))) {
                    return false;
                }
                return FindApproverFlow_TypedDataContext1_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
    }
}
