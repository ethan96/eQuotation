namespace WorkFlowlAPI.ABBGPApproval {
    
    #line 31 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\ABBGPApproval\ABBFindApproverFlow.xaml"
    using System;
    
    #line default
    #line hidden
    
    #line 1 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\ABBGPApproval\ABBFindApproverFlow.xaml"
    using System.Collections;
    
    #line default
    #line hidden
    
    #line 32 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\ABBGPApproval\ABBFindApproverFlow.xaml"
    using System.Collections.Generic;
    
    #line default
    #line hidden
    
    #line 1 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\ABBGPApproval\ABBFindApproverFlow.xaml"
    using System.Activities;
    
    #line default
    #line hidden
    
    #line 1 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\ABBGPApproval\ABBFindApproverFlow.xaml"
    using System.Activities.Expressions;
    
    #line default
    #line hidden
    
    #line 1 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\ABBGPApproval\ABBFindApproverFlow.xaml"
    using System.Activities.Statements;
    
    #line default
    #line hidden
    
    #line 33 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\ABBGPApproval\ABBFindApproverFlow.xaml"
    using System.Data;
    
    #line default
    #line hidden
    
    #line 34 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\ABBGPApproval\ABBFindApproverFlow.xaml"
    using System.Linq;
    
    #line default
    #line hidden
    
    #line 35 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\ABBGPApproval\ABBFindApproverFlow.xaml"
    using System.Text;
    
    #line default
    #line hidden
    
    #line 36 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\ABBGPApproval\ABBFindApproverFlow.xaml"
    using Advantech.Myadvantech.DataAccess;
    
    #line default
    #line hidden
    
    #line 37 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\ABBGPApproval\ABBFindApproverFlow.xaml"
    using Advantech.Myadvantech.Business;
    
    #line default
    #line hidden
    
    #line 38 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\ABBGPApproval\ABBFindApproverFlow.xaml"
    using WorkFlowlAPI;
    
    #line default
    #line hidden
    
    #line 1 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\ABBGPApproval\ABBFindApproverFlow.xaml"
    using System.Activities.XamlIntegration;
    
    #line default
    #line hidden
    
    
    public partial class ABBFindApproverFlow : System.Activities.XamlIntegration.ICompiledExpressionRoot {
        
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
                this.dataContextActivities = ABBFindApproverFlow_TypedDataContext2_ForReadOnly.GetDataContextActivitiesHelper(this.rootActivity, this.forImplementation);
            }
            if ((expressionId == 0)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext0 = ((ABBFindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext0.ValueType___Expr0Get();
            }
            if ((expressionId == 1)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new ABBFindApproverFlow_TypedDataContext2(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext2 refDataContext1 = ((ABBFindApproverFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext1.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>(refDataContext1.ValueType___Expr1Get, refDataContext1.ValueType___Expr1Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 2)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext2 = ((ABBFindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext2.ValueType___Expr2Get();
            }
            if ((expressionId == 3)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new ABBFindApproverFlow_TypedDataContext2(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext2 refDataContext3 = ((ABBFindApproverFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext3.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC>>(refDataContext3.ValueType___Expr3Get, refDataContext3.ValueType___Expr3Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 4)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext4 = ((ABBFindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext4.ValueType___Expr4Get();
            }
            if ((expressionId == 5)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext5 = ((ABBFindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext5.ValueType___Expr5Get();
            }
            if ((expressionId == 6)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new ABBFindApproverFlow_TypedDataContext2(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext2 refDataContext6 = ((ABBFindApproverFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext6.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext6.ValueType___Expr6Get, refDataContext6.ValueType___Expr6Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 7)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext7 = ((ABBFindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext7.ValueType___Expr7Get();
            }
            if ((expressionId == 8)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext3.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new ABBFindApproverFlow_TypedDataContext3(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext3 refDataContext8 = ((ABBFindApproverFlow_TypedDataContext3)(cachedCompiledDataContext[2]));
                return refDataContext8.GetLocation<int>(refDataContext8.ValueType___Expr8Get, refDataContext8.ValueType___Expr8Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 9)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext3.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new ABBFindApproverFlow_TypedDataContext3(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext3 refDataContext9 = ((ABBFindApproverFlow_TypedDataContext3)(cachedCompiledDataContext[2]));
                return refDataContext9.GetLocation<string>(refDataContext9.ValueType___Expr9Get, refDataContext9.ValueType___Expr9Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 10)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext3_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[3] == null)) {
                    cachedCompiledDataContext[3] = new ABBFindApproverFlow_TypedDataContext3_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext3_ForReadOnly valDataContext10 = ((ABBFindApproverFlow_TypedDataContext3_ForReadOnly)(cachedCompiledDataContext[3]));
                return valDataContext10.ValueType___Expr10Get();
            }
            if ((expressionId == 11)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext4_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[4] == null)) {
                    cachedCompiledDataContext[4] = new ABBFindApproverFlow_TypedDataContext4_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext4_ForReadOnly valDataContext11 = ((ABBFindApproverFlow_TypedDataContext4_ForReadOnly)(cachedCompiledDataContext[4]));
                return valDataContext11.ValueType___Expr11Get();
            }
            if ((expressionId == 12)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext4.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[5] == null)) {
                    cachedCompiledDataContext[5] = new ABBFindApproverFlow_TypedDataContext4(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext4 refDataContext12 = ((ABBFindApproverFlow_TypedDataContext4)(cachedCompiledDataContext[5]));
                return refDataContext12.GetLocation<int>(refDataContext12.ValueType___Expr12Get, refDataContext12.ValueType___Expr12Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 13)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext4_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[4] == null)) {
                    cachedCompiledDataContext[4] = new ABBFindApproverFlow_TypedDataContext4_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext4_ForReadOnly valDataContext13 = ((ABBFindApproverFlow_TypedDataContext4_ForReadOnly)(cachedCompiledDataContext[4]));
                return valDataContext13.ValueType___Expr13Get();
            }
            if ((expressionId == 14)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext4_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[4] == null)) {
                    cachedCompiledDataContext[4] = new ABBFindApproverFlow_TypedDataContext4_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext4_ForReadOnly valDataContext14 = ((ABBFindApproverFlow_TypedDataContext4_ForReadOnly)(cachedCompiledDataContext[4]));
                return valDataContext14.ValueType___Expr14Get();
            }
            if ((expressionId == 15)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext4.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[5] == null)) {
                    cachedCompiledDataContext[5] = new ABBFindApproverFlow_TypedDataContext4(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext4 refDataContext15 = ((ABBFindApproverFlow_TypedDataContext4)(cachedCompiledDataContext[5]));
                return refDataContext15.GetLocation<string>(refDataContext15.ValueType___Expr15Get, refDataContext15.ValueType___Expr15Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 16)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext4_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[4] == null)) {
                    cachedCompiledDataContext[4] = new ABBFindApproverFlow_TypedDataContext4_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext4_ForReadOnly valDataContext16 = ((ABBFindApproverFlow_TypedDataContext4_ForReadOnly)(cachedCompiledDataContext[4]));
                return valDataContext16.ValueType___Expr16Get();
            }
            if ((expressionId == 17)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext4.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[5] == null)) {
                    cachedCompiledDataContext[5] = new ABBFindApproverFlow_TypedDataContext4(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext4 refDataContext17 = ((ABBFindApproverFlow_TypedDataContext4)(cachedCompiledDataContext[5]));
                return refDataContext17.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>(refDataContext17.ValueType___Expr17Get, refDataContext17.ValueType___Expr17Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 18)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext4_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[4] == null)) {
                    cachedCompiledDataContext[4] = new ABBFindApproverFlow_TypedDataContext4_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext4_ForReadOnly valDataContext18 = ((ABBFindApproverFlow_TypedDataContext4_ForReadOnly)(cachedCompiledDataContext[4]));
                return valDataContext18.ValueType___Expr18Get();
            }
            if ((expressionId == 19)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext4_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[4] == null)) {
                    cachedCompiledDataContext[4] = new ABBFindApproverFlow_TypedDataContext4_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext4_ForReadOnly valDataContext19 = ((ABBFindApproverFlow_TypedDataContext4_ForReadOnly)(cachedCompiledDataContext[4]));
                return valDataContext19.ValueType___Expr19Get();
            }
            if ((expressionId == 20)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext4_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[4] == null)) {
                    cachedCompiledDataContext[4] = new ABBFindApproverFlow_TypedDataContext4_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext4_ForReadOnly valDataContext20 = ((ABBFindApproverFlow_TypedDataContext4_ForReadOnly)(cachedCompiledDataContext[4]));
                return valDataContext20.ValueType___Expr20Get();
            }
            if ((expressionId == 21)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext4_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[4] == null)) {
                    cachedCompiledDataContext[4] = new ABBFindApproverFlow_TypedDataContext4_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext4_ForReadOnly valDataContext21 = ((ABBFindApproverFlow_TypedDataContext4_ForReadOnly)(cachedCompiledDataContext[4]));
                return valDataContext21.ValueType___Expr21Get();
            }
            if ((expressionId == 22)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext22 = ((ABBFindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext22.ValueType___Expr22Get();
            }
            if ((expressionId == 23)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext23 = ((ABBFindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext23.ValueType___Expr23Get();
            }
            if ((expressionId == 24)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext24 = ((ABBFindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext24.ValueType___Expr24Get();
            }
            if ((expressionId == 25)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new ABBFindApproverFlow_TypedDataContext2(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext2 refDataContext25 = ((ABBFindApproverFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext25.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext25.ValueType___Expr25Get, refDataContext25.ValueType___Expr25Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 26)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext26 = ((ABBFindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext26.ValueType___Expr26Get();
            }
            if ((expressionId == 27)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new ABBFindApproverFlow_TypedDataContext2(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext2 refDataContext27 = ((ABBFindApproverFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext27.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext27.ValueType___Expr27Get, refDataContext27.ValueType___Expr27Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 28)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext28 = ((ABBFindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext28.ValueType___Expr28Get();
            }
            if ((expressionId == 29)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext29 = ((ABBFindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext29.ValueType___Expr29Get();
            }
            if ((expressionId == 30)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new ABBFindApproverFlow_TypedDataContext2(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext2 refDataContext30 = ((ABBFindApproverFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext30.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext30.ValueType___Expr30Get, refDataContext30.ValueType___Expr30Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 31)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new ABBFindApproverFlow_TypedDataContext2(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext2 refDataContext31 = ((ABBFindApproverFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext31.GetLocation<int>(refDataContext31.ValueType___Expr31Get, refDataContext31.ValueType___Expr31Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 32)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext32 = ((ABBFindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext32.ValueType___Expr32Get();
            }
            if ((expressionId == 33)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext5_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[6] == null)) {
                    cachedCompiledDataContext[6] = new ABBFindApproverFlow_TypedDataContext5_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext5_ForReadOnly valDataContext33 = ((ABBFindApproverFlow_TypedDataContext5_ForReadOnly)(cachedCompiledDataContext[6]));
                return valDataContext33.ValueType___Expr33Get();
            }
            if ((expressionId == 34)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext5.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[7] == null)) {
                    cachedCompiledDataContext[7] = new ABBFindApproverFlow_TypedDataContext5(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext5 refDataContext34 = ((ABBFindApproverFlow_TypedDataContext5)(cachedCompiledDataContext[7]));
                return refDataContext34.GetLocation<int>(refDataContext34.ValueType___Expr34Get, refDataContext34.ValueType___Expr34Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 35)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext5_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[6] == null)) {
                    cachedCompiledDataContext[6] = new ABBFindApproverFlow_TypedDataContext5_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext5_ForReadOnly valDataContext35 = ((ABBFindApproverFlow_TypedDataContext5_ForReadOnly)(cachedCompiledDataContext[6]));
                return valDataContext35.ValueType___Expr35Get();
            }
            if ((expressionId == 36)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext5.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[7] == null)) {
                    cachedCompiledDataContext[7] = new ABBFindApproverFlow_TypedDataContext5(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext5 refDataContext36 = ((ABBFindApproverFlow_TypedDataContext5)(cachedCompiledDataContext[7]));
                return refDataContext36.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>(refDataContext36.ValueType___Expr36Get, refDataContext36.ValueType___Expr36Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 37)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext5_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[6] == null)) {
                    cachedCompiledDataContext[6] = new ABBFindApproverFlow_TypedDataContext5_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext5_ForReadOnly valDataContext37 = ((ABBFindApproverFlow_TypedDataContext5_ForReadOnly)(cachedCompiledDataContext[6]));
                return valDataContext37.ValueType___Expr37Get();
            }
            if ((expressionId == 38)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext5_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[6] == null)) {
                    cachedCompiledDataContext[6] = new ABBFindApproverFlow_TypedDataContext5_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext5_ForReadOnly valDataContext38 = ((ABBFindApproverFlow_TypedDataContext5_ForReadOnly)(cachedCompiledDataContext[6]));
                return valDataContext38.ValueType___Expr38Get();
            }
            if ((expressionId == 39)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext5_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[6] == null)) {
                    cachedCompiledDataContext[6] = new ABBFindApproverFlow_TypedDataContext5_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext5_ForReadOnly valDataContext39 = ((ABBFindApproverFlow_TypedDataContext5_ForReadOnly)(cachedCompiledDataContext[6]));
                return valDataContext39.ValueType___Expr39Get();
            }
            if ((expressionId == 40)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext5_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[6] == null)) {
                    cachedCompiledDataContext[6] = new ABBFindApproverFlow_TypedDataContext5_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext5_ForReadOnly valDataContext40 = ((ABBFindApproverFlow_TypedDataContext5_ForReadOnly)(cachedCompiledDataContext[6]));
                return valDataContext40.ValueType___Expr40Get();
            }
            if ((expressionId == 41)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext41 = ((ABBFindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext41.ValueType___Expr41Get();
            }
            if ((expressionId == 42)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ABBFindApproverFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 8);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new ABBFindApproverFlow_TypedDataContext2(locations, activityContext, true);
                }
                ABBFindApproverFlow_TypedDataContext2 refDataContext42 = ((ABBFindApproverFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext42.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext42.ValueType___Expr42Get, refDataContext42.ValueType___Expr42Set, expressionId, this.rootActivity, activityContext);
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
                ABBFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext0 = new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext0.ValueType___Expr0Get();
            }
            if ((expressionId == 1)) {
                ABBFindApproverFlow_TypedDataContext2 refDataContext1 = new ABBFindApproverFlow_TypedDataContext2(locations, true);
                return refDataContext1.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>(refDataContext1.ValueType___Expr1Get, refDataContext1.ValueType___Expr1Set);
            }
            if ((expressionId == 2)) {
                ABBFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext2 = new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext2.ValueType___Expr2Get();
            }
            if ((expressionId == 3)) {
                ABBFindApproverFlow_TypedDataContext2 refDataContext3 = new ABBFindApproverFlow_TypedDataContext2(locations, true);
                return refDataContext3.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC>>(refDataContext3.ValueType___Expr3Get, refDataContext3.ValueType___Expr3Set);
            }
            if ((expressionId == 4)) {
                ABBFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext4 = new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext4.ValueType___Expr4Get();
            }
            if ((expressionId == 5)) {
                ABBFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext5 = new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext5.ValueType___Expr5Get();
            }
            if ((expressionId == 6)) {
                ABBFindApproverFlow_TypedDataContext2 refDataContext6 = new ABBFindApproverFlow_TypedDataContext2(locations, true);
                return refDataContext6.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext6.ValueType___Expr6Get, refDataContext6.ValueType___Expr6Set);
            }
            if ((expressionId == 7)) {
                ABBFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext7 = new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext7.ValueType___Expr7Get();
            }
            if ((expressionId == 8)) {
                ABBFindApproverFlow_TypedDataContext3 refDataContext8 = new ABBFindApproverFlow_TypedDataContext3(locations, true);
                return refDataContext8.GetLocation<int>(refDataContext8.ValueType___Expr8Get, refDataContext8.ValueType___Expr8Set);
            }
            if ((expressionId == 9)) {
                ABBFindApproverFlow_TypedDataContext3 refDataContext9 = new ABBFindApproverFlow_TypedDataContext3(locations, true);
                return refDataContext9.GetLocation<string>(refDataContext9.ValueType___Expr9Get, refDataContext9.ValueType___Expr9Set);
            }
            if ((expressionId == 10)) {
                ABBFindApproverFlow_TypedDataContext3_ForReadOnly valDataContext10 = new ABBFindApproverFlow_TypedDataContext3_ForReadOnly(locations, true);
                return valDataContext10.ValueType___Expr10Get();
            }
            if ((expressionId == 11)) {
                ABBFindApproverFlow_TypedDataContext4_ForReadOnly valDataContext11 = new ABBFindApproverFlow_TypedDataContext4_ForReadOnly(locations, true);
                return valDataContext11.ValueType___Expr11Get();
            }
            if ((expressionId == 12)) {
                ABBFindApproverFlow_TypedDataContext4 refDataContext12 = new ABBFindApproverFlow_TypedDataContext4(locations, true);
                return refDataContext12.GetLocation<int>(refDataContext12.ValueType___Expr12Get, refDataContext12.ValueType___Expr12Set);
            }
            if ((expressionId == 13)) {
                ABBFindApproverFlow_TypedDataContext4_ForReadOnly valDataContext13 = new ABBFindApproverFlow_TypedDataContext4_ForReadOnly(locations, true);
                return valDataContext13.ValueType___Expr13Get();
            }
            if ((expressionId == 14)) {
                ABBFindApproverFlow_TypedDataContext4_ForReadOnly valDataContext14 = new ABBFindApproverFlow_TypedDataContext4_ForReadOnly(locations, true);
                return valDataContext14.ValueType___Expr14Get();
            }
            if ((expressionId == 15)) {
                ABBFindApproverFlow_TypedDataContext4 refDataContext15 = new ABBFindApproverFlow_TypedDataContext4(locations, true);
                return refDataContext15.GetLocation<string>(refDataContext15.ValueType___Expr15Get, refDataContext15.ValueType___Expr15Set);
            }
            if ((expressionId == 16)) {
                ABBFindApproverFlow_TypedDataContext4_ForReadOnly valDataContext16 = new ABBFindApproverFlow_TypedDataContext4_ForReadOnly(locations, true);
                return valDataContext16.ValueType___Expr16Get();
            }
            if ((expressionId == 17)) {
                ABBFindApproverFlow_TypedDataContext4 refDataContext17 = new ABBFindApproverFlow_TypedDataContext4(locations, true);
                return refDataContext17.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>(refDataContext17.ValueType___Expr17Get, refDataContext17.ValueType___Expr17Set);
            }
            if ((expressionId == 18)) {
                ABBFindApproverFlow_TypedDataContext4_ForReadOnly valDataContext18 = new ABBFindApproverFlow_TypedDataContext4_ForReadOnly(locations, true);
                return valDataContext18.ValueType___Expr18Get();
            }
            if ((expressionId == 19)) {
                ABBFindApproverFlow_TypedDataContext4_ForReadOnly valDataContext19 = new ABBFindApproverFlow_TypedDataContext4_ForReadOnly(locations, true);
                return valDataContext19.ValueType___Expr19Get();
            }
            if ((expressionId == 20)) {
                ABBFindApproverFlow_TypedDataContext4_ForReadOnly valDataContext20 = new ABBFindApproverFlow_TypedDataContext4_ForReadOnly(locations, true);
                return valDataContext20.ValueType___Expr20Get();
            }
            if ((expressionId == 21)) {
                ABBFindApproverFlow_TypedDataContext4_ForReadOnly valDataContext21 = new ABBFindApproverFlow_TypedDataContext4_ForReadOnly(locations, true);
                return valDataContext21.ValueType___Expr21Get();
            }
            if ((expressionId == 22)) {
                ABBFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext22 = new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext22.ValueType___Expr22Get();
            }
            if ((expressionId == 23)) {
                ABBFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext23 = new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext23.ValueType___Expr23Get();
            }
            if ((expressionId == 24)) {
                ABBFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext24 = new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext24.ValueType___Expr24Get();
            }
            if ((expressionId == 25)) {
                ABBFindApproverFlow_TypedDataContext2 refDataContext25 = new ABBFindApproverFlow_TypedDataContext2(locations, true);
                return refDataContext25.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext25.ValueType___Expr25Get, refDataContext25.ValueType___Expr25Set);
            }
            if ((expressionId == 26)) {
                ABBFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext26 = new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext26.ValueType___Expr26Get();
            }
            if ((expressionId == 27)) {
                ABBFindApproverFlow_TypedDataContext2 refDataContext27 = new ABBFindApproverFlow_TypedDataContext2(locations, true);
                return refDataContext27.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext27.ValueType___Expr27Get, refDataContext27.ValueType___Expr27Set);
            }
            if ((expressionId == 28)) {
                ABBFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext28 = new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext28.ValueType___Expr28Get();
            }
            if ((expressionId == 29)) {
                ABBFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext29 = new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext29.ValueType___Expr29Get();
            }
            if ((expressionId == 30)) {
                ABBFindApproverFlow_TypedDataContext2 refDataContext30 = new ABBFindApproverFlow_TypedDataContext2(locations, true);
                return refDataContext30.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext30.ValueType___Expr30Get, refDataContext30.ValueType___Expr30Set);
            }
            if ((expressionId == 31)) {
                ABBFindApproverFlow_TypedDataContext2 refDataContext31 = new ABBFindApproverFlow_TypedDataContext2(locations, true);
                return refDataContext31.GetLocation<int>(refDataContext31.ValueType___Expr31Get, refDataContext31.ValueType___Expr31Set);
            }
            if ((expressionId == 32)) {
                ABBFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext32 = new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext32.ValueType___Expr32Get();
            }
            if ((expressionId == 33)) {
                ABBFindApproverFlow_TypedDataContext5_ForReadOnly valDataContext33 = new ABBFindApproverFlow_TypedDataContext5_ForReadOnly(locations, true);
                return valDataContext33.ValueType___Expr33Get();
            }
            if ((expressionId == 34)) {
                ABBFindApproverFlow_TypedDataContext5 refDataContext34 = new ABBFindApproverFlow_TypedDataContext5(locations, true);
                return refDataContext34.GetLocation<int>(refDataContext34.ValueType___Expr34Get, refDataContext34.ValueType___Expr34Set);
            }
            if ((expressionId == 35)) {
                ABBFindApproverFlow_TypedDataContext5_ForReadOnly valDataContext35 = new ABBFindApproverFlow_TypedDataContext5_ForReadOnly(locations, true);
                return valDataContext35.ValueType___Expr35Get();
            }
            if ((expressionId == 36)) {
                ABBFindApproverFlow_TypedDataContext5 refDataContext36 = new ABBFindApproverFlow_TypedDataContext5(locations, true);
                return refDataContext36.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>(refDataContext36.ValueType___Expr36Get, refDataContext36.ValueType___Expr36Set);
            }
            if ((expressionId == 37)) {
                ABBFindApproverFlow_TypedDataContext5_ForReadOnly valDataContext37 = new ABBFindApproverFlow_TypedDataContext5_ForReadOnly(locations, true);
                return valDataContext37.ValueType___Expr37Get();
            }
            if ((expressionId == 38)) {
                ABBFindApproverFlow_TypedDataContext5_ForReadOnly valDataContext38 = new ABBFindApproverFlow_TypedDataContext5_ForReadOnly(locations, true);
                return valDataContext38.ValueType___Expr38Get();
            }
            if ((expressionId == 39)) {
                ABBFindApproverFlow_TypedDataContext5_ForReadOnly valDataContext39 = new ABBFindApproverFlow_TypedDataContext5_ForReadOnly(locations, true);
                return valDataContext39.ValueType___Expr39Get();
            }
            if ((expressionId == 40)) {
                ABBFindApproverFlow_TypedDataContext5_ForReadOnly valDataContext40 = new ABBFindApproverFlow_TypedDataContext5_ForReadOnly(locations, true);
                return valDataContext40.ValueType___Expr40Get();
            }
            if ((expressionId == 41)) {
                ABBFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext41 = new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext41.ValueType___Expr41Get();
            }
            if ((expressionId == 42)) {
                ABBFindApproverFlow_TypedDataContext2 refDataContext42 = new ABBFindApproverFlow_TypedDataContext2(locations, true);
                return refDataContext42.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext42.ValueType___Expr42Get, refDataContext42.ValueType___Expr42Set);
            }
            return null;
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool CanExecuteExpression(string expressionText, bool isReference, System.Collections.Generic.IList<System.Activities.LocationReference> locations, out int expressionId) {
            if (((isReference == false) 
                        && ((expressionText == "new List<WorkFlowApproval>()") 
                        && (ABBFindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 0;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "ApprovalList") 
                        && (ABBFindApproverFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 1;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "GPControlBusinessLogic.GetBBApprovalListByCompanyIDSalesEmailAndSalesCode(QuoteTo" +
                            "ERPID, SalesEmail, SalesCode)") 
                        && (ABBFindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 2;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "GPRuleList") 
                        && (ABBFindApproverFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 3;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "GPRuleList.Count > 0") 
                        && (ABBFindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 4;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "FindApproverResult.GPRuleNotFound") 
                        && (ABBFindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 5;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "FindApproverResult") 
                        && (ABBFindApproverFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 6;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationDetails") 
                        && (ABBFindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 7;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "levelNum") 
                        && (ABBFindApproverFlow_TypedDataContext3.Validate(locations, true, 0) == true)))) {
                expressionId = 8;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "item.GPStatus") 
                        && (ABBFindApproverFlow_TypedDataContext3.Validate(locations, true, 0) == true)))) {
                expressionId = 9;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "GPRuleList") 
                        && (ABBFindApproverFlow_TypedDataContext3_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 10;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "levelNum+1") 
                        && (ABBFindApproverFlow_TypedDataContext4_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 11;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "levelNum") 
                        && (ABBFindApproverFlow_TypedDataContext4.Validate(locations, true, 0) == true)))) {
                expressionId = 12;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "(double)item.Margin/100 < rule.gp_level") 
                        && (ABBFindApproverFlow_TypedDataContext4_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 13;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "\"<\" + (rule.gp_level * 100 ).ToString() + \"%\"") 
                        && (ABBFindApproverFlow_TypedDataContext4_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 14;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "item.GPStatus") 
                        && (ABBFindApproverFlow_TypedDataContext4.Validate(locations, true, 0) == true)))) {
                expressionId = 15;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "ApproverType.Sales.ToString()") 
                        && (ABBFindApproverFlow_TypedDataContext4_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 16;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "ApprovalList") 
                        && (ABBFindApproverFlow_TypedDataContext4.Validate(locations, true, 0) == true)))) {
                expressionId = 17;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "levelNum") 
                        && (ABBFindApproverFlow_TypedDataContext4_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 18;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuoteId") 
                        && (ABBFindApproverFlow_TypedDataContext4_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 19;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "Url") 
                        && (ABBFindApproverFlow_TypedDataContext4_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 20;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "rule.approver") 
                        && (ABBFindApproverFlow_TypedDataContext4_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 21;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "(ExpiredDate.Date - QuoteDate.Date).TotalDays > 90") 
                        && (ABBFindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 22;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "ApprovalList!=null && ApprovalList.Count > 0") 
                        && (ABBFindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 23;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "FindApproverResult.NoNeed") 
                        && (ABBFindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 24;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "FindApproverResult") 
                        && (ABBFindApproverFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 25;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "FindApproverResult.NeedApprovalForGP") 
                        && (ABBFindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 26;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "FindApproverResult") 
                        && (ABBFindApproverFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 27;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "ApprovalList!=null && ApprovalList.Count > 0") 
                        && (ABBFindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 28;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "FindApproverResult.NeedApprovalForExpiredDate") 
                        && (ABBFindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 29;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "FindApproverResult") 
                        && (ABBFindApproverFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 30;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "levelNum") 
                        && (ABBFindApproverFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 31;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "GPRuleList") 
                        && (ABBFindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 32;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "levelNum+1") 
                        && (ABBFindApproverFlow_TypedDataContext5_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 33;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "levelNum") 
                        && (ABBFindApproverFlow_TypedDataContext5.Validate(locations, true, 0) == true)))) {
                expressionId = 34;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "ApproverType.Sales.ToString()") 
                        && (ABBFindApproverFlow_TypedDataContext5_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 35;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "ApprovalList") 
                        && (ABBFindApproverFlow_TypedDataContext5.Validate(locations, true, 0) == true)))) {
                expressionId = 36;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "levelNum") 
                        && (ABBFindApproverFlow_TypedDataContext5_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 37;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuoteId") 
                        && (ABBFindApproverFlow_TypedDataContext5_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 38;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "Url") 
                        && (ABBFindApproverFlow_TypedDataContext5_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 39;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "rule.approver") 
                        && (ABBFindApproverFlow_TypedDataContext5_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 40;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "FindApproverResult.NeedApprovalForGPAndExpiredDate") 
                        && (ABBFindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 41;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "FindApproverResult") 
                        && (ABBFindApproverFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 42;
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
                return new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr0GetTree();
            }
            if ((expressionId == 1)) {
                return new ABBFindApproverFlow_TypedDataContext2(locationReferences).@__Expr1GetTree();
            }
            if ((expressionId == 2)) {
                return new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr2GetTree();
            }
            if ((expressionId == 3)) {
                return new ABBFindApproverFlow_TypedDataContext2(locationReferences).@__Expr3GetTree();
            }
            if ((expressionId == 4)) {
                return new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr4GetTree();
            }
            if ((expressionId == 5)) {
                return new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr5GetTree();
            }
            if ((expressionId == 6)) {
                return new ABBFindApproverFlow_TypedDataContext2(locationReferences).@__Expr6GetTree();
            }
            if ((expressionId == 7)) {
                return new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr7GetTree();
            }
            if ((expressionId == 8)) {
                return new ABBFindApproverFlow_TypedDataContext3(locationReferences).@__Expr8GetTree();
            }
            if ((expressionId == 9)) {
                return new ABBFindApproverFlow_TypedDataContext3(locationReferences).@__Expr9GetTree();
            }
            if ((expressionId == 10)) {
                return new ABBFindApproverFlow_TypedDataContext3_ForReadOnly(locationReferences).@__Expr10GetTree();
            }
            if ((expressionId == 11)) {
                return new ABBFindApproverFlow_TypedDataContext4_ForReadOnly(locationReferences).@__Expr11GetTree();
            }
            if ((expressionId == 12)) {
                return new ABBFindApproverFlow_TypedDataContext4(locationReferences).@__Expr12GetTree();
            }
            if ((expressionId == 13)) {
                return new ABBFindApproverFlow_TypedDataContext4_ForReadOnly(locationReferences).@__Expr13GetTree();
            }
            if ((expressionId == 14)) {
                return new ABBFindApproverFlow_TypedDataContext4_ForReadOnly(locationReferences).@__Expr14GetTree();
            }
            if ((expressionId == 15)) {
                return new ABBFindApproverFlow_TypedDataContext4(locationReferences).@__Expr15GetTree();
            }
            if ((expressionId == 16)) {
                return new ABBFindApproverFlow_TypedDataContext4_ForReadOnly(locationReferences).@__Expr16GetTree();
            }
            if ((expressionId == 17)) {
                return new ABBFindApproverFlow_TypedDataContext4(locationReferences).@__Expr17GetTree();
            }
            if ((expressionId == 18)) {
                return new ABBFindApproverFlow_TypedDataContext4_ForReadOnly(locationReferences).@__Expr18GetTree();
            }
            if ((expressionId == 19)) {
                return new ABBFindApproverFlow_TypedDataContext4_ForReadOnly(locationReferences).@__Expr19GetTree();
            }
            if ((expressionId == 20)) {
                return new ABBFindApproverFlow_TypedDataContext4_ForReadOnly(locationReferences).@__Expr20GetTree();
            }
            if ((expressionId == 21)) {
                return new ABBFindApproverFlow_TypedDataContext4_ForReadOnly(locationReferences).@__Expr21GetTree();
            }
            if ((expressionId == 22)) {
                return new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr22GetTree();
            }
            if ((expressionId == 23)) {
                return new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr23GetTree();
            }
            if ((expressionId == 24)) {
                return new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr24GetTree();
            }
            if ((expressionId == 25)) {
                return new ABBFindApproverFlow_TypedDataContext2(locationReferences).@__Expr25GetTree();
            }
            if ((expressionId == 26)) {
                return new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr26GetTree();
            }
            if ((expressionId == 27)) {
                return new ABBFindApproverFlow_TypedDataContext2(locationReferences).@__Expr27GetTree();
            }
            if ((expressionId == 28)) {
                return new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr28GetTree();
            }
            if ((expressionId == 29)) {
                return new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr29GetTree();
            }
            if ((expressionId == 30)) {
                return new ABBFindApproverFlow_TypedDataContext2(locationReferences).@__Expr30GetTree();
            }
            if ((expressionId == 31)) {
                return new ABBFindApproverFlow_TypedDataContext2(locationReferences).@__Expr31GetTree();
            }
            if ((expressionId == 32)) {
                return new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr32GetTree();
            }
            if ((expressionId == 33)) {
                return new ABBFindApproverFlow_TypedDataContext5_ForReadOnly(locationReferences).@__Expr33GetTree();
            }
            if ((expressionId == 34)) {
                return new ABBFindApproverFlow_TypedDataContext5(locationReferences).@__Expr34GetTree();
            }
            if ((expressionId == 35)) {
                return new ABBFindApproverFlow_TypedDataContext5_ForReadOnly(locationReferences).@__Expr35GetTree();
            }
            if ((expressionId == 36)) {
                return new ABBFindApproverFlow_TypedDataContext5(locationReferences).@__Expr36GetTree();
            }
            if ((expressionId == 37)) {
                return new ABBFindApproverFlow_TypedDataContext5_ForReadOnly(locationReferences).@__Expr37GetTree();
            }
            if ((expressionId == 38)) {
                return new ABBFindApproverFlow_TypedDataContext5_ForReadOnly(locationReferences).@__Expr38GetTree();
            }
            if ((expressionId == 39)) {
                return new ABBFindApproverFlow_TypedDataContext5_ForReadOnly(locationReferences).@__Expr39GetTree();
            }
            if ((expressionId == 40)) {
                return new ABBFindApproverFlow_TypedDataContext5_ForReadOnly(locationReferences).@__Expr40GetTree();
            }
            if ((expressionId == 41)) {
                return new ABBFindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr41GetTree();
            }
            if ((expressionId == 42)) {
                return new ABBFindApproverFlow_TypedDataContext2(locationReferences).@__Expr42GetTree();
            }
            return null;
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class ABBFindApproverFlow_TypedDataContext0 : System.Activities.XamlIntegration.CompiledDataContext {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public ABBFindApproverFlow_TypedDataContext0(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ABBFindApproverFlow_TypedDataContext0(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ABBFindApproverFlow_TypedDataContext0(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
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
        private class ABBFindApproverFlow_TypedDataContext0_ForReadOnly : System.Activities.XamlIntegration.CompiledDataContext {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public ABBFindApproverFlow_TypedDataContext0_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ABBFindApproverFlow_TypedDataContext0_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ABBFindApproverFlow_TypedDataContext0_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
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
        private class ABBFindApproverFlow_TypedDataContext1 : ABBFindApproverFlow_TypedDataContext0 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            protected System.DateTime QuoteDate;
            
            protected System.DateTime ExpiredDate;
            
            protected WorkFlowlAPI.FindApproverResult FindApproverResult;
            
            public ABBFindApproverFlow_TypedDataContext1(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ABBFindApproverFlow_TypedDataContext1(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ABBFindApproverFlow_TypedDataContext1(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected string SalesEmail {
                get {
                    return ((string)(this.GetVariableValue((0 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((0 + locationsOffset), value);
                }
            }
            
            protected string QuoteToERPID {
                get {
                    return ((string)(this.GetVariableValue((1 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((1 + locationsOffset), value);
                }
            }
            
            protected string SalesCode {
                get {
                    return ((string)(this.GetVariableValue((2 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((2 + locationsOffset), value);
                }
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> ApprovalList {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>)(this.GetVariableValue((4 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((4 + locationsOffset), value);
                }
            }
            
            protected string QuoteId {
                get {
                    return ((string)(this.GetVariableValue((7 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((7 + locationsOffset), value);
                }
            }
            
            protected string Url {
                get {
                    return ((string)(this.GetVariableValue((8 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((8 + locationsOffset), value);
                }
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> QuotationDetails {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)(this.GetVariableValue((9 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((9 + locationsOffset), value);
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
                this.QuoteDate = ((System.DateTime)(this.GetVariableValue((3 + locationsOffset))));
                this.ExpiredDate = ((System.DateTime)(this.GetVariableValue((5 + locationsOffset))));
                this.FindApproverResult = ((WorkFlowlAPI.FindApproverResult)(this.GetVariableValue((6 + locationsOffset))));
                base.GetValueTypeValues();
            }
            
            protected override void SetValueTypeValues() {
                this.SetVariableValue((3 + locationsOffset), this.QuoteDate);
                this.SetVariableValue((5 + locationsOffset), this.ExpiredDate);
                this.SetVariableValue((6 + locationsOffset), this.FindApproverResult);
                base.SetValueTypeValues();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 10))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 10);
                }
                expectedLocationsCount = 10;
                if (((locationReferences[(offset + 0)].Name != "SalesEmail") 
                            || (locationReferences[(offset + 0)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 1)].Name != "QuoteToERPID") 
                            || (locationReferences[(offset + 1)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 2)].Name != "SalesCode") 
                            || (locationReferences[(offset + 2)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 4)].Name != "ApprovalList") 
                            || (locationReferences[(offset + 4)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>)))) {
                    return false;
                }
                if (((locationReferences[(offset + 7)].Name != "QuoteId") 
                            || (locationReferences[(offset + 7)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 8)].Name != "Url") 
                            || (locationReferences[(offset + 8)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 9)].Name != "QuotationDetails") 
                            || (locationReferences[(offset + 9)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)))) {
                    return false;
                }
                if (((locationReferences[(offset + 3)].Name != "QuoteDate") 
                            || (locationReferences[(offset + 3)].Type != typeof(System.DateTime)))) {
                    return false;
                }
                if (((locationReferences[(offset + 5)].Name != "ExpiredDate") 
                            || (locationReferences[(offset + 5)].Type != typeof(System.DateTime)))) {
                    return false;
                }
                if (((locationReferences[(offset + 6)].Name != "FindApproverResult") 
                            || (locationReferences[(offset + 6)].Type != typeof(WorkFlowlAPI.FindApproverResult)))) {
                    return false;
                }
                return ABBFindApproverFlow_TypedDataContext0.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class ABBFindApproverFlow_TypedDataContext1_ForReadOnly : ABBFindApproverFlow_TypedDataContext0_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            protected System.DateTime QuoteDate;
            
            protected System.DateTime ExpiredDate;
            
            protected WorkFlowlAPI.FindApproverResult FindApproverResult;
            
            public ABBFindApproverFlow_TypedDataContext1_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ABBFindApproverFlow_TypedDataContext1_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ABBFindApproverFlow_TypedDataContext1_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected string SalesEmail {
                get {
                    return ((string)(this.GetVariableValue((0 + locationsOffset))));
                }
            }
            
            protected string QuoteToERPID {
                get {
                    return ((string)(this.GetVariableValue((1 + locationsOffset))));
                }
            }
            
            protected string SalesCode {
                get {
                    return ((string)(this.GetVariableValue((2 + locationsOffset))));
                }
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> ApprovalList {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>)(this.GetVariableValue((4 + locationsOffset))));
                }
            }
            
            protected string QuoteId {
                get {
                    return ((string)(this.GetVariableValue((7 + locationsOffset))));
                }
            }
            
            protected string Url {
                get {
                    return ((string)(this.GetVariableValue((8 + locationsOffset))));
                }
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> QuotationDetails {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)(this.GetVariableValue((9 + locationsOffset))));
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
                this.QuoteDate = ((System.DateTime)(this.GetVariableValue((3 + locationsOffset))));
                this.ExpiredDate = ((System.DateTime)(this.GetVariableValue((5 + locationsOffset))));
                this.FindApproverResult = ((WorkFlowlAPI.FindApproverResult)(this.GetVariableValue((6 + locationsOffset))));
                base.GetValueTypeValues();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 10))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 10);
                }
                expectedLocationsCount = 10;
                if (((locationReferences[(offset + 0)].Name != "SalesEmail") 
                            || (locationReferences[(offset + 0)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 1)].Name != "QuoteToERPID") 
                            || (locationReferences[(offset + 1)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 2)].Name != "SalesCode") 
                            || (locationReferences[(offset + 2)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 4)].Name != "ApprovalList") 
                            || (locationReferences[(offset + 4)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>)))) {
                    return false;
                }
                if (((locationReferences[(offset + 7)].Name != "QuoteId") 
                            || (locationReferences[(offset + 7)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 8)].Name != "Url") 
                            || (locationReferences[(offset + 8)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 9)].Name != "QuotationDetails") 
                            || (locationReferences[(offset + 9)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)))) {
                    return false;
                }
                if (((locationReferences[(offset + 3)].Name != "QuoteDate") 
                            || (locationReferences[(offset + 3)].Type != typeof(System.DateTime)))) {
                    return false;
                }
                if (((locationReferences[(offset + 5)].Name != "ExpiredDate") 
                            || (locationReferences[(offset + 5)].Type != typeof(System.DateTime)))) {
                    return false;
                }
                if (((locationReferences[(offset + 6)].Name != "FindApproverResult") 
                            || (locationReferences[(offset + 6)].Type != typeof(WorkFlowlAPI.FindApproverResult)))) {
                    return false;
                }
                return ABBFindApproverFlow_TypedDataContext0_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class ABBFindApproverFlow_TypedDataContext2 : ABBFindApproverFlow_TypedDataContext1 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            protected double MinMargin;
            
            protected int levelNum;
            
            public ABBFindApproverFlow_TypedDataContext2(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ABBFindApproverFlow_TypedDataContext2(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ABBFindApproverFlow_TypedDataContext2(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC> GPRuleList {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC>)(this.GetVariableValue((11 + locationsOffset))));
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
                
                #line 77 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>> expression = () => 
              ApprovalList;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> @__Expr1Get() {
                
                #line 77 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
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
                
                #line 77 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                
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
                
                #line 91 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC>>> expression = () => 
                  GPRuleList;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC> @__Expr3Get() {
                
                #line 91 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                  GPRuleList;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC> ValueType___Expr3Get() {
                this.GetValueTypeValues();
                return this.@__Expr3Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr3Set(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC> value) {
                
                #line 91 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                
                  GPRuleList = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr3Set(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC> value) {
                this.GetValueTypeValues();
                this.@__Expr3Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr6GetTree() {
                
                #line 391 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.FindApproverResult>> expression = () => 
                          FindApproverResult;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.FindApproverResult @__Expr6Get() {
                
                #line 391 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                          FindApproverResult;
                
                #line default
                #line hidden
            }
            
            public WorkFlowlAPI.FindApproverResult ValueType___Expr6Get() {
                this.GetValueTypeValues();
                return this.@__Expr6Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr6Set(WorkFlowlAPI.FindApproverResult value) {
                
                #line 391 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                
                          FindApproverResult = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr6Set(WorkFlowlAPI.FindApproverResult value) {
                this.GetValueTypeValues();
                this.@__Expr6Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr25GetTree() {
                
                #line 369 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.FindApproverResult>> expression = () => 
                                      FindApproverResult;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.FindApproverResult @__Expr25Get() {
                
                #line 369 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                      FindApproverResult;
                
                #line default
                #line hidden
            }
            
            public WorkFlowlAPI.FindApproverResult ValueType___Expr25Get() {
                this.GetValueTypeValues();
                return this.@__Expr25Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr25Set(WorkFlowlAPI.FindApproverResult value) {
                
                #line 369 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                
                                      FindApproverResult = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr25Set(WorkFlowlAPI.FindApproverResult value) {
                this.GetValueTypeValues();
                this.@__Expr25Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr27GetTree() {
                
                #line 353 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.FindApproverResult>> expression = () => 
                                      FindApproverResult;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.FindApproverResult @__Expr27Get() {
                
                #line 353 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                      FindApproverResult;
                
                #line default
                #line hidden
            }
            
            public WorkFlowlAPI.FindApproverResult ValueType___Expr27Get() {
                this.GetValueTypeValues();
                return this.@__Expr27Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr27Set(WorkFlowlAPI.FindApproverResult value) {
                
                #line 353 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                
                                      FindApproverResult = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr27Set(WorkFlowlAPI.FindApproverResult value) {
                this.GetValueTypeValues();
                this.@__Expr27Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr30GetTree() {
                
                #line 327 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.FindApproverResult>> expression = () => 
                                      FindApproverResult;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.FindApproverResult @__Expr30Get() {
                
                #line 327 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                      FindApproverResult;
                
                #line default
                #line hidden
            }
            
            public WorkFlowlAPI.FindApproverResult ValueType___Expr30Get() {
                this.GetValueTypeValues();
                return this.@__Expr30Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr30Set(WorkFlowlAPI.FindApproverResult value) {
                
                #line 327 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                
                                      FindApproverResult = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr30Set(WorkFlowlAPI.FindApproverResult value) {
                this.GetValueTypeValues();
                this.@__Expr30Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr31GetTree() {
                
                #line 252 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<int>> expression = () => 
                                            levelNum;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public int @__Expr31Get() {
                
                #line 252 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                            levelNum;
                
                #line default
                #line hidden
            }
            
            public int ValueType___Expr31Get() {
                this.GetValueTypeValues();
                return this.@__Expr31Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr31Set(int value) {
                
                #line 252 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                
                                            levelNum = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr31Set(int value) {
                this.GetValueTypeValues();
                this.@__Expr31Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr42GetTree() {
                
                #line 237 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.FindApproverResult>> expression = () => 
                                      FindApproverResult;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.FindApproverResult @__Expr42Get() {
                
                #line 237 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                      FindApproverResult;
                
                #line default
                #line hidden
            }
            
            public WorkFlowlAPI.FindApproverResult ValueType___Expr42Get() {
                this.GetValueTypeValues();
                return this.@__Expr42Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr42Set(WorkFlowlAPI.FindApproverResult value) {
                
                #line 237 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                
                                      FindApproverResult = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr42Set(WorkFlowlAPI.FindApproverResult value) {
                this.GetValueTypeValues();
                this.@__Expr42Set(value);
                this.SetValueTypeValues();
            }
            
            protected override void GetValueTypeValues() {
                this.MinMargin = ((double)(this.GetVariableValue((10 + locationsOffset))));
                this.levelNum = ((int)(this.GetVariableValue((12 + locationsOffset))));
                base.GetValueTypeValues();
            }
            
            protected override void SetValueTypeValues() {
                this.SetVariableValue((10 + locationsOffset), this.MinMargin);
                this.SetVariableValue((12 + locationsOffset), this.levelNum);
                base.SetValueTypeValues();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 13))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 13);
                }
                expectedLocationsCount = 13;
                if (((locationReferences[(offset + 11)].Name != "GPRuleList") 
                            || (locationReferences[(offset + 11)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC>)))) {
                    return false;
                }
                if (((locationReferences[(offset + 10)].Name != "MinMargin") 
                            || (locationReferences[(offset + 10)].Type != typeof(double)))) {
                    return false;
                }
                if (((locationReferences[(offset + 12)].Name != "levelNum") 
                            || (locationReferences[(offset + 12)].Type != typeof(int)))) {
                    return false;
                }
                return ABBFindApproverFlow_TypedDataContext1.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class ABBFindApproverFlow_TypedDataContext2_ForReadOnly : ABBFindApproverFlow_TypedDataContext1_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            protected double MinMargin;
            
            protected int levelNum;
            
            public ABBFindApproverFlow_TypedDataContext2_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ABBFindApproverFlow_TypedDataContext2_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ABBFindApproverFlow_TypedDataContext2_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC> GPRuleList {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC>)(this.GetVariableValue((11 + locationsOffset))));
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
                
                #line 82 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>> expression = () => 
              new List<WorkFlowApproval>();
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> @__Expr0Get() {
                
                #line 82 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
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
                
                #line 96 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC>>> expression = () => 
                  GPControlBusinessLogic.GetBBApprovalListByCompanyIDSalesEmailAndSalesCode(QuoteToERPID, SalesEmail, SalesCode);
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC> @__Expr2Get() {
                
                #line 96 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                  GPControlBusinessLogic.GetBBApprovalListByCompanyIDSalesEmailAndSalesCode(QuoteToERPID, SalesEmail, SalesCode);
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC> ValueType___Expr2Get() {
                this.GetValueTypeValues();
                return this.@__Expr2Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr4GetTree() {
                
                #line 103 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<bool>> expression = () => 
                  GPRuleList.Count > 0;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public bool @__Expr4Get() {
                
                #line 103 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                  GPRuleList.Count > 0;
                
                #line default
                #line hidden
            }
            
            public bool ValueType___Expr4Get() {
                this.GetValueTypeValues();
                return this.@__Expr4Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr5GetTree() {
                
                #line 396 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.FindApproverResult>> expression = () => 
                          FindApproverResult.GPRuleNotFound;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.FindApproverResult @__Expr5Get() {
                
                #line 396 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                          FindApproverResult.GPRuleNotFound;
                
                #line default
                #line hidden
            }
            
            public WorkFlowlAPI.FindApproverResult ValueType___Expr5Get() {
                this.GetValueTypeValues();
                return this.@__Expr5Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr7GetTree() {
                
                #line 110 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                          QuotationDetails;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr7Get() {
                
                #line 110 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                          QuotationDetails;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr7Get() {
                this.GetValueTypeValues();
                return this.@__Expr7Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr22GetTree() {
                
                #line 225 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<bool>> expression = () => 
                          (ExpiredDate.Date - QuoteDate.Date).TotalDays > 90;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public bool @__Expr22Get() {
                
                #line 225 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                          (ExpiredDate.Date - QuoteDate.Date).TotalDays > 90;
                
                #line default
                #line hidden
            }
            
            public bool ValueType___Expr22Get() {
                this.GetValueTypeValues();
                return this.@__Expr22Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr23GetTree() {
                
                #line 346 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<bool>> expression = () => 
                              ApprovalList!=null && ApprovalList.Count > 0;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public bool @__Expr23Get() {
                
                #line 346 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                              ApprovalList!=null && ApprovalList.Count > 0;
                
                #line default
                #line hidden
            }
            
            public bool ValueType___Expr23Get() {
                this.GetValueTypeValues();
                return this.@__Expr23Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr24GetTree() {
                
                #line 374 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.FindApproverResult>> expression = () => 
                                      FindApproverResult.NoNeed;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.FindApproverResult @__Expr24Get() {
                
                #line 374 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                      FindApproverResult.NoNeed;
                
                #line default
                #line hidden
            }
            
            public WorkFlowlAPI.FindApproverResult ValueType___Expr24Get() {
                this.GetValueTypeValues();
                return this.@__Expr24Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr26GetTree() {
                
                #line 358 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.FindApproverResult>> expression = () => 
                                      FindApproverResult.NeedApprovalForGP;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.FindApproverResult @__Expr26Get() {
                
                #line 358 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                      FindApproverResult.NeedApprovalForGP;
                
                #line default
                #line hidden
            }
            
            public WorkFlowlAPI.FindApproverResult ValueType___Expr26Get() {
                this.GetValueTypeValues();
                return this.@__Expr26Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr28GetTree() {
                
                #line 230 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<bool>> expression = () => 
                              ApprovalList!=null && ApprovalList.Count > 0;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public bool @__Expr28Get() {
                
                #line 230 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                              ApprovalList!=null && ApprovalList.Count > 0;
                
                #line default
                #line hidden
            }
            
            public bool ValueType___Expr28Get() {
                this.GetValueTypeValues();
                return this.@__Expr28Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr29GetTree() {
                
                #line 332 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.FindApproverResult>> expression = () => 
                                      FindApproverResult.NeedApprovalForExpiredDate;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.FindApproverResult @__Expr29Get() {
                
                #line 332 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                      FindApproverResult.NeedApprovalForExpiredDate;
                
                #line default
                #line hidden
            }
            
            public WorkFlowlAPI.FindApproverResult ValueType___Expr29Get() {
                this.GetValueTypeValues();
                return this.@__Expr29Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr32GetTree() {
                
                #line 262 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC>>> expression = () => 
                                            GPRuleList;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC> @__Expr32Get() {
                
                #line 262 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                            GPRuleList;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC> ValueType___Expr32Get() {
                this.GetValueTypeValues();
                return this.@__Expr32Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr41GetTree() {
                
                #line 242 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.FindApproverResult>> expression = () => 
                                      FindApproverResult.NeedApprovalForGPAndExpiredDate;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.FindApproverResult @__Expr41Get() {
                
                #line 242 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                      FindApproverResult.NeedApprovalForGPAndExpiredDate;
                
                #line default
                #line hidden
            }
            
            public WorkFlowlAPI.FindApproverResult ValueType___Expr41Get() {
                this.GetValueTypeValues();
                return this.@__Expr41Get();
            }
            
            protected override void GetValueTypeValues() {
                this.MinMargin = ((double)(this.GetVariableValue((10 + locationsOffset))));
                this.levelNum = ((int)(this.GetVariableValue((12 + locationsOffset))));
                base.GetValueTypeValues();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 13))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 13);
                }
                expectedLocationsCount = 13;
                if (((locationReferences[(offset + 11)].Name != "GPRuleList") 
                            || (locationReferences[(offset + 11)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC>)))) {
                    return false;
                }
                if (((locationReferences[(offset + 10)].Name != "MinMargin") 
                            || (locationReferences[(offset + 10)].Type != typeof(double)))) {
                    return false;
                }
                if (((locationReferences[(offset + 12)].Name != "levelNum") 
                            || (locationReferences[(offset + 12)].Type != typeof(int)))) {
                    return false;
                }
                return ABBFindApproverFlow_TypedDataContext1_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class ABBFindApproverFlow_TypedDataContext3 : ABBFindApproverFlow_TypedDataContext2 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public ABBFindApproverFlow_TypedDataContext3(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ABBFindApproverFlow_TypedDataContext3(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ABBFindApproverFlow_TypedDataContext3(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationDetail item {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationDetail)(this.GetVariableValue((13 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((13 + locationsOffset), value);
                }
            }
            
            internal new static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public new virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
                base.SetLocationsOffset(locationsOffset);
            }
            
            internal System.Linq.Expressions.Expression @__Expr8GetTree() {
                
                #line 121 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<int>> expression = () => 
                                levelNum;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public int @__Expr8Get() {
                
                #line 121 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                levelNum;
                
                #line default
                #line hidden
            }
            
            public int ValueType___Expr8Get() {
                this.GetValueTypeValues();
                return this.@__Expr8Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr8Set(int value) {
                
                #line 121 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                
                                levelNum = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr8Set(int value) {
                this.GetValueTypeValues();
                this.@__Expr8Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr9GetTree() {
                
                #line 131 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                                item.GPStatus;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr9Get() {
                
                #line 131 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                item.GPStatus;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr9Get() {
                this.GetValueTypeValues();
                return this.@__Expr9Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr9Set(string value) {
                
                #line 131 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                
                                item.GPStatus = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr9Set(string value) {
                this.GetValueTypeValues();
                this.@__Expr9Set(value);
                this.SetValueTypeValues();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 14))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 14);
                }
                expectedLocationsCount = 14;
                if (((locationReferences[(offset + 13)].Name != "item") 
                            || (locationReferences[(offset + 13)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationDetail)))) {
                    return false;
                }
                return ABBFindApproverFlow_TypedDataContext2.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class ABBFindApproverFlow_TypedDataContext3_ForReadOnly : ABBFindApproverFlow_TypedDataContext2_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public ABBFindApproverFlow_TypedDataContext3_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ABBFindApproverFlow_TypedDataContext3_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ABBFindApproverFlow_TypedDataContext3_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationDetail item {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationDetail)(this.GetVariableValue((13 + locationsOffset))));
                }
            }
            
            internal new static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public new virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
                base.SetLocationsOffset(locationsOffset);
            }
            
            internal System.Linq.Expressions.Expression @__Expr10GetTree() {
                
                #line 141 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC>>> expression = () => 
                                GPRuleList;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC> @__Expr10Get() {
                
                #line 141 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                GPRuleList;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC> ValueType___Expr10Get() {
                this.GetValueTypeValues();
                return this.@__Expr10Get();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 14))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 14);
                }
                expectedLocationsCount = 14;
                if (((locationReferences[(offset + 13)].Name != "item") 
                            || (locationReferences[(offset + 13)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationDetail)))) {
                    return false;
                }
                return ABBFindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class ABBFindApproverFlow_TypedDataContext4 : ABBFindApproverFlow_TypedDataContext3 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public ABBFindApproverFlow_TypedDataContext4(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ABBFindApproverFlow_TypedDataContext4(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ABBFindApproverFlow_TypedDataContext4(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC rule {
                get {
                    return ((Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC)(this.GetVariableValue((14 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((14 + locationsOffset), value);
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
                
                #line 152 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<int>> expression = () => 
                                      levelNum;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public int @__Expr12Get() {
                
                #line 152 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                      levelNum;
                
                #line default
                #line hidden
            }
            
            public int ValueType___Expr12Get() {
                this.GetValueTypeValues();
                return this.@__Expr12Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr12Set(int value) {
                
                #line 152 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                
                                      levelNum = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr12Set(int value) {
                this.GetValueTypeValues();
                this.@__Expr12Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr15GetTree() {
                
                #line 172 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                                            item.GPStatus;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr15Get() {
                
                #line 172 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                            item.GPStatus;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr15Get() {
                this.GetValueTypeValues();
                return this.@__Expr15Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr15Set(string value) {
                
                #line 172 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                
                                            item.GPStatus = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr15Set(string value) {
                this.GetValueTypeValues();
                this.@__Expr15Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr17GetTree() {
                
                #line 184 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>> expression = () => 
                                            ApprovalList;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> @__Expr17Get() {
                
                #line 184 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                            ApprovalList;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> ValueType___Expr17Get() {
                this.GetValueTypeValues();
                return this.@__Expr17Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr17Set(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> value) {
                
                #line 184 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                
                                            ApprovalList = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr17Set(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> value) {
                this.GetValueTypeValues();
                this.@__Expr17Set(value);
                this.SetValueTypeValues();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 15))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 15);
                }
                expectedLocationsCount = 15;
                if (((locationReferences[(offset + 14)].Name != "rule") 
                            || (locationReferences[(offset + 14)].Type != typeof(Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC)))) {
                    return false;
                }
                return ABBFindApproverFlow_TypedDataContext3.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class ABBFindApproverFlow_TypedDataContext4_ForReadOnly : ABBFindApproverFlow_TypedDataContext3_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public ABBFindApproverFlow_TypedDataContext4_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ABBFindApproverFlow_TypedDataContext4_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ABBFindApproverFlow_TypedDataContext4_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC rule {
                get {
                    return ((Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC)(this.GetVariableValue((14 + locationsOffset))));
                }
            }
            
            internal new static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public new virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
                base.SetLocationsOffset(locationsOffset);
            }
            
            internal System.Linq.Expressions.Expression @__Expr11GetTree() {
                
                #line 157 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<int>> expression = () => 
                                      levelNum+1;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public int @__Expr11Get() {
                
                #line 157 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                      levelNum+1;
                
                #line default
                #line hidden
            }
            
            public int ValueType___Expr11Get() {
                this.GetValueTypeValues();
                return this.@__Expr11Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr13GetTree() {
                
                #line 164 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<bool>> expression = () => 
                                      (double)item.Margin/100 < rule.gp_level;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public bool @__Expr13Get() {
                
                #line 164 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                      (double)item.Margin/100 < rule.gp_level;
                
                #line default
                #line hidden
            }
            
            public bool ValueType___Expr13Get() {
                this.GetValueTypeValues();
                return this.@__Expr13Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr14GetTree() {
                
                #line 177 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                                            "<" + (rule.gp_level * 100 ).ToString() + "%";
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr14Get() {
                
                #line 177 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                            "<" + (rule.gp_level * 100 ).ToString() + "%";
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr14Get() {
                this.GetValueTypeValues();
                return this.@__Expr14Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr16GetTree() {
                
                #line 194 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                                            ApproverType.Sales.ToString();
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr16Get() {
                
                #line 194 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                            ApproverType.Sales.ToString();
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr16Get() {
                this.GetValueTypeValues();
                return this.@__Expr16Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr18GetTree() {
                
                #line 199 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<double>> expression = () => 
                                            levelNum;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public double @__Expr18Get() {
                
                #line 199 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                            levelNum;
                
                #line default
                #line hidden
            }
            
            public double ValueType___Expr18Get() {
                this.GetValueTypeValues();
                return this.@__Expr18Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr19GetTree() {
                
                #line 204 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                                            QuoteId;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr19Get() {
                
                #line 204 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                            QuoteId;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr19Get() {
                this.GetValueTypeValues();
                return this.@__Expr19Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr20GetTree() {
                
                #line 209 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                                            Url;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr20Get() {
                
                #line 209 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                            Url;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr20Get() {
                this.GetValueTypeValues();
                return this.@__Expr20Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr21GetTree() {
                
                #line 189 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                                            rule.approver;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr21Get() {
                
                #line 189 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                            rule.approver;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr21Get() {
                this.GetValueTypeValues();
                return this.@__Expr21Get();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 15))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 15);
                }
                expectedLocationsCount = 15;
                if (((locationReferences[(offset + 14)].Name != "rule") 
                            || (locationReferences[(offset + 14)].Type != typeof(Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC)))) {
                    return false;
                }
                return ABBFindApproverFlow_TypedDataContext3_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class ABBFindApproverFlow_TypedDataContext5 : ABBFindApproverFlow_TypedDataContext2 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public ABBFindApproverFlow_TypedDataContext5(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ABBFindApproverFlow_TypedDataContext5(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ABBFindApproverFlow_TypedDataContext5(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC rule {
                get {
                    return ((Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC)(this.GetVariableValue((13 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((13 + locationsOffset), value);
                }
            }
            
            internal new static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public new virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
                base.SetLocationsOffset(locationsOffset);
            }
            
            internal System.Linq.Expressions.Expression @__Expr34GetTree() {
                
                #line 273 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<int>> expression = () => 
                                                  levelNum;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public int @__Expr34Get() {
                
                #line 273 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                                  levelNum;
                
                #line default
                #line hidden
            }
            
            public int ValueType___Expr34Get() {
                this.GetValueTypeValues();
                return this.@__Expr34Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr34Set(int value) {
                
                #line 273 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                
                                                  levelNum = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr34Set(int value) {
                this.GetValueTypeValues();
                this.@__Expr34Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr36GetTree() {
                
                #line 285 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>> expression = () => 
                                                  ApprovalList;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> @__Expr36Get() {
                
                #line 285 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                                  ApprovalList;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> ValueType___Expr36Get() {
                this.GetValueTypeValues();
                return this.@__Expr36Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr36Set(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> value) {
                
                #line 285 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                
                                                  ApprovalList = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr36Set(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> value) {
                this.GetValueTypeValues();
                this.@__Expr36Set(value);
                this.SetValueTypeValues();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 14))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 14);
                }
                expectedLocationsCount = 14;
                if (((locationReferences[(offset + 13)].Name != "rule") 
                            || (locationReferences[(offset + 13)].Type != typeof(Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC)))) {
                    return false;
                }
                return ABBFindApproverFlow_TypedDataContext2.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class ABBFindApproverFlow_TypedDataContext5_ForReadOnly : ABBFindApproverFlow_TypedDataContext2_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public ABBFindApproverFlow_TypedDataContext5_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ABBFindApproverFlow_TypedDataContext5_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ABBFindApproverFlow_TypedDataContext5_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC rule {
                get {
                    return ((Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC)(this.GetVariableValue((13 + locationsOffset))));
                }
            }
            
            internal new static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public new virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
                base.SetLocationsOffset(locationsOffset);
            }
            
            internal System.Linq.Expressions.Expression @__Expr33GetTree() {
                
                #line 278 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<int>> expression = () => 
                                                  levelNum+1;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public int @__Expr33Get() {
                
                #line 278 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                                  levelNum+1;
                
                #line default
                #line hidden
            }
            
            public int ValueType___Expr33Get() {
                this.GetValueTypeValues();
                return this.@__Expr33Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr35GetTree() {
                
                #line 295 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                                                  ApproverType.Sales.ToString();
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr35Get() {
                
                #line 295 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                                  ApproverType.Sales.ToString();
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr35Get() {
                this.GetValueTypeValues();
                return this.@__Expr35Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr37GetTree() {
                
                #line 300 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<double>> expression = () => 
                                                  levelNum;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public double @__Expr37Get() {
                
                #line 300 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                                  levelNum;
                
                #line default
                #line hidden
            }
            
            public double ValueType___Expr37Get() {
                this.GetValueTypeValues();
                return this.@__Expr37Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr38GetTree() {
                
                #line 305 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                                                  QuoteId;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr38Get() {
                
                #line 305 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                                  QuoteId;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr38Get() {
                this.GetValueTypeValues();
                return this.@__Expr38Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr39GetTree() {
                
                #line 310 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                                                  Url;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr39Get() {
                
                #line 310 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                                  Url;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr39Get() {
                this.GetValueTypeValues();
                return this.@__Expr39Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr40GetTree() {
                
                #line 290 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                                                  rule.approver;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr40Get() {
                
                #line 290 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ABBGPAPPROVAL\ABBFINDAPPROVERFLOW.XAML"
                return 
                                                  rule.approver;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr40Get() {
                this.GetValueTypeValues();
                return this.@__Expr40Get();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 14))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 14);
                }
                expectedLocationsCount = 14;
                if (((locationReferences[(offset + 13)].Name != "rule") 
                            || (locationReferences[(offset + 13)].Type != typeof(Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC)))) {
                    return false;
                }
                return ABBFindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
    }
}
