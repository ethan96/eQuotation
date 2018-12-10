namespace WorkFlowlAPI.ASGGPApproval {
    
    #line 29 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\ASGGPApproval\ASGFindApproverFlow.xaml"
    using System;
    
    #line default
    #line hidden
    
    #line 1 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\ASGGPApproval\ASGFindApproverFlow.xaml"
    using System.Collections;
    
    #line default
    #line hidden
    
    #line 30 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\ASGGPApproval\ASGFindApproverFlow.xaml"
    using System.Collections.Generic;
    
    #line default
    #line hidden
    
    #line 1 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\ASGGPApproval\ASGFindApproverFlow.xaml"
    using System.Activities;
    
    #line default
    #line hidden
    
    #line 1 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\ASGGPApproval\ASGFindApproverFlow.xaml"
    using System.Activities.Expressions;
    
    #line default
    #line hidden
    
    #line 1 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\ASGGPApproval\ASGFindApproverFlow.xaml"
    using System.Activities.Statements;
    
    #line default
    #line hidden
    
    #line 31 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\ASGGPApproval\ASGFindApproverFlow.xaml"
    using System.Data;
    
    #line default
    #line hidden
    
    #line 32 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\ASGGPApproval\ASGFindApproverFlow.xaml"
    using System.Linq;
    
    #line default
    #line hidden
    
    #line 33 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\ASGGPApproval\ASGFindApproverFlow.xaml"
    using System.Text;
    
    #line default
    #line hidden
    
    #line 34 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\ASGGPApproval\ASGFindApproverFlow.xaml"
    using Advantech.Myadvantech.DataAccess;
    
    #line default
    #line hidden
    
    #line 35 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\ASGGPApproval\ASGFindApproverFlow.xaml"
    using Advantech.Myadvantech.Business;
    
    #line default
    #line hidden
    
    #line 36 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\ASGGPApproval\ASGFindApproverFlow.xaml"
    using WorkFlowlAPI;
    
    #line default
    #line hidden
    
    #line 1 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\ASGGPApproval\ASGFindApproverFlow.xaml"
    using System.Activities.XamlIntegration;
    
    #line default
    #line hidden
    
    
    public partial class ASGFindApproverFlow : System.Activities.XamlIntegration.ICompiledExpressionRoot {
        
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
                this.dataContextActivities = ASGFindApproverFlow_TypedDataContext2_ForReadOnly.GetDataContextActivitiesHelper(this.rootActivity, this.forImplementation);
            }
            if ((expressionId == 0)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext0 = ((ASGFindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext0.ValueType___Expr0Get();
            }
            if ((expressionId == 1)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new ASGFindApproverFlow_TypedDataContext2(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext2 refDataContext1 = ((ASGFindApproverFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext1.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>(refDataContext1.ValueType___Expr1Get, refDataContext1.ValueType___Expr1Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 2)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext2 = ((ASGFindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext2.ValueType___Expr2Get();
            }
            if ((expressionId == 3)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext3 = ((ASGFindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext3.ValueType___Expr3Get();
            }
            if ((expressionId == 4)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new ASGFindApproverFlow_TypedDataContext2(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext2 refDataContext4 = ((ASGFindApproverFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext4.GetLocation<string>(refDataContext4.ValueType___Expr4Get, refDataContext4.ValueType___Expr4Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 5)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext5 = ((ASGFindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext5.ValueType___Expr5Get();
            }
            if ((expressionId == 6)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext6 = ((ASGFindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext6.ValueType___Expr6Get();
            }
            if ((expressionId == 7)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new ASGFindApproverFlow_TypedDataContext2(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext2 refDataContext7 = ((ASGFindApproverFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext7.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext7.ValueType___Expr7Get, refDataContext7.ValueType___Expr7Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 8)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext3_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new ASGFindApproverFlow_TypedDataContext3_ForReadOnly(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext3_ForReadOnly valDataContext8 = ((ASGFindApproverFlow_TypedDataContext3_ForReadOnly)(cachedCompiledDataContext[2]));
                return valDataContext8.ValueType___Expr8Get();
            }
            if ((expressionId == 9)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext3.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[3] == null)) {
                    cachedCompiledDataContext[3] = new ASGFindApproverFlow_TypedDataContext3(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext3 refDataContext9 = ((ASGFindApproverFlow_TypedDataContext3)(cachedCompiledDataContext[3]));
                return refDataContext9.GetLocation<double>(refDataContext9.ValueType___Expr9Get, refDataContext9.ValueType___Expr9Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 10)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext3_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new ASGFindApproverFlow_TypedDataContext3_ForReadOnly(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext3_ForReadOnly valDataContext10 = ((ASGFindApproverFlow_TypedDataContext3_ForReadOnly)(cachedCompiledDataContext[2]));
                return valDataContext10.ValueType___Expr10Get();
            }
            if ((expressionId == 11)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext3.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[3] == null)) {
                    cachedCompiledDataContext[3] = new ASGFindApproverFlow_TypedDataContext3(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext3 refDataContext11 = ((ASGFindApproverFlow_TypedDataContext3)(cachedCompiledDataContext[3]));
                return refDataContext11.GetLocation<string[]>(refDataContext11.ValueType___Expr11Get, refDataContext11.ValueType___Expr11Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 12)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext3_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new ASGFindApproverFlow_TypedDataContext3_ForReadOnly(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext3_ForReadOnly valDataContext12 = ((ASGFindApproverFlow_TypedDataContext3_ForReadOnly)(cachedCompiledDataContext[2]));
                return valDataContext12.ValueType___Expr12Get();
            }
            if ((expressionId == 13)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext3_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new ASGFindApproverFlow_TypedDataContext3_ForReadOnly(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext3_ForReadOnly valDataContext13 = ((ASGFindApproverFlow_TypedDataContext3_ForReadOnly)(cachedCompiledDataContext[2]));
                return valDataContext13.ValueType___Expr13Get();
            }
            if ((expressionId == 14)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext3.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[3] == null)) {
                    cachedCompiledDataContext[3] = new ASGFindApproverFlow_TypedDataContext3(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext3 refDataContext14 = ((ASGFindApproverFlow_TypedDataContext3)(cachedCompiledDataContext[3]));
                return refDataContext14.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext14.ValueType___Expr14Get, refDataContext14.ValueType___Expr14Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 15)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext3_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new ASGFindApproverFlow_TypedDataContext3_ForReadOnly(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext3_ForReadOnly valDataContext15 = ((ASGFindApproverFlow_TypedDataContext3_ForReadOnly)(cachedCompiledDataContext[2]));
                return valDataContext15.ValueType___Expr15Get();
            }
            if ((expressionId == 16)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext4_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[4] == null)) {
                    cachedCompiledDataContext[4] = new ASGFindApproverFlow_TypedDataContext4_ForReadOnly(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext4_ForReadOnly valDataContext16 = ((ASGFindApproverFlow_TypedDataContext4_ForReadOnly)(cachedCompiledDataContext[4]));
                return valDataContext16.ValueType___Expr16Get();
            }
            if ((expressionId == 17)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext4.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[5] == null)) {
                    cachedCompiledDataContext[5] = new ASGFindApproverFlow_TypedDataContext4(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext4 refDataContext17 = ((ASGFindApproverFlow_TypedDataContext4)(cachedCompiledDataContext[5]));
                return refDataContext17.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>(refDataContext17.ValueType___Expr17Get, refDataContext17.ValueType___Expr17Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 18)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext4_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[4] == null)) {
                    cachedCompiledDataContext[4] = new ASGFindApproverFlow_TypedDataContext4_ForReadOnly(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext4_ForReadOnly valDataContext18 = ((ASGFindApproverFlow_TypedDataContext4_ForReadOnly)(cachedCompiledDataContext[4]));
                return valDataContext18.ValueType___Expr18Get();
            }
            if ((expressionId == 19)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext4_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[4] == null)) {
                    cachedCompiledDataContext[4] = new ASGFindApproverFlow_TypedDataContext4_ForReadOnly(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext4_ForReadOnly valDataContext19 = ((ASGFindApproverFlow_TypedDataContext4_ForReadOnly)(cachedCompiledDataContext[4]));
                return valDataContext19.ValueType___Expr19Get();
            }
            if ((expressionId == 20)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext4_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[4] == null)) {
                    cachedCompiledDataContext[4] = new ASGFindApproverFlow_TypedDataContext4_ForReadOnly(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext4_ForReadOnly valDataContext20 = ((ASGFindApproverFlow_TypedDataContext4_ForReadOnly)(cachedCompiledDataContext[4]));
                return valDataContext20.ValueType___Expr20Get();
            }
            if ((expressionId == 21)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext4_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[4] == null)) {
                    cachedCompiledDataContext[4] = new ASGFindApproverFlow_TypedDataContext4_ForReadOnly(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext4_ForReadOnly valDataContext21 = ((ASGFindApproverFlow_TypedDataContext4_ForReadOnly)(cachedCompiledDataContext[4]));
                return valDataContext21.ValueType___Expr21Get();
            }
            if ((expressionId == 22)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext22 = ((ASGFindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext22.ValueType___Expr22Get();
            }
            if ((expressionId == 23)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext23 = ((ASGFindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext23.ValueType___Expr23Get();
            }
            if ((expressionId == 24)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext24 = ((ASGFindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext24.ValueType___Expr24Get();
            }
            if ((expressionId == 25)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new ASGFindApproverFlow_TypedDataContext2(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext2 refDataContext25 = ((ASGFindApproverFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext25.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext25.ValueType___Expr25Get, refDataContext25.ValueType___Expr25Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 26)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext26 = ((ASGFindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext26.ValueType___Expr26Get();
            }
            if ((expressionId == 27)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new ASGFindApproverFlow_TypedDataContext2(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext2 refDataContext27 = ((ASGFindApproverFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext27.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext27.ValueType___Expr27Get, refDataContext27.ValueType___Expr27Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 28)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext28 = ((ASGFindApproverFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[0]));
                return valDataContext28.ValueType___Expr28Get();
            }
            if ((expressionId == 29)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new ASGFindApproverFlow_TypedDataContext2(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext2 refDataContext29 = ((ASGFindApproverFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext29.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext29.ValueType___Expr29Get, refDataContext29.ValueType___Expr29Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 30)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = ASGFindApproverFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 6);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new ASGFindApproverFlow_TypedDataContext2(locations, activityContext, true);
                }
                ASGFindApproverFlow_TypedDataContext2 refDataContext30 = ((ASGFindApproverFlow_TypedDataContext2)(cachedCompiledDataContext[1]));
                return refDataContext30.GetLocation<string>(refDataContext30.ValueType___Expr30Get, refDataContext30.ValueType___Expr30Set, expressionId, this.rootActivity, activityContext);
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
                ASGFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext0 = new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext0.ValueType___Expr0Get();
            }
            if ((expressionId == 1)) {
                ASGFindApproverFlow_TypedDataContext2 refDataContext1 = new ASGFindApproverFlow_TypedDataContext2(locations, true);
                return refDataContext1.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>(refDataContext1.ValueType___Expr1Get, refDataContext1.ValueType___Expr1Set);
            }
            if ((expressionId == 2)) {
                ASGFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext2 = new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext2.ValueType___Expr2Get();
            }
            if ((expressionId == 3)) {
                ASGFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext3 = new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext3.ValueType___Expr3Get();
            }
            if ((expressionId == 4)) {
                ASGFindApproverFlow_TypedDataContext2 refDataContext4 = new ASGFindApproverFlow_TypedDataContext2(locations, true);
                return refDataContext4.GetLocation<string>(refDataContext4.ValueType___Expr4Get, refDataContext4.ValueType___Expr4Set);
            }
            if ((expressionId == 5)) {
                ASGFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext5 = new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext5.ValueType___Expr5Get();
            }
            if ((expressionId == 6)) {
                ASGFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext6 = new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext6.ValueType___Expr6Get();
            }
            if ((expressionId == 7)) {
                ASGFindApproverFlow_TypedDataContext2 refDataContext7 = new ASGFindApproverFlow_TypedDataContext2(locations, true);
                return refDataContext7.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext7.ValueType___Expr7Get, refDataContext7.ValueType___Expr7Set);
            }
            if ((expressionId == 8)) {
                ASGFindApproverFlow_TypedDataContext3_ForReadOnly valDataContext8 = new ASGFindApproverFlow_TypedDataContext3_ForReadOnly(locations, true);
                return valDataContext8.ValueType___Expr8Get();
            }
            if ((expressionId == 9)) {
                ASGFindApproverFlow_TypedDataContext3 refDataContext9 = new ASGFindApproverFlow_TypedDataContext3(locations, true);
                return refDataContext9.GetLocation<double>(refDataContext9.ValueType___Expr9Get, refDataContext9.ValueType___Expr9Set);
            }
            if ((expressionId == 10)) {
                ASGFindApproverFlow_TypedDataContext3_ForReadOnly valDataContext10 = new ASGFindApproverFlow_TypedDataContext3_ForReadOnly(locations, true);
                return valDataContext10.ValueType___Expr10Get();
            }
            if ((expressionId == 11)) {
                ASGFindApproverFlow_TypedDataContext3 refDataContext11 = new ASGFindApproverFlow_TypedDataContext3(locations, true);
                return refDataContext11.GetLocation<string[]>(refDataContext11.ValueType___Expr11Get, refDataContext11.ValueType___Expr11Set);
            }
            if ((expressionId == 12)) {
                ASGFindApproverFlow_TypedDataContext3_ForReadOnly valDataContext12 = new ASGFindApproverFlow_TypedDataContext3_ForReadOnly(locations, true);
                return valDataContext12.ValueType___Expr12Get();
            }
            if ((expressionId == 13)) {
                ASGFindApproverFlow_TypedDataContext3_ForReadOnly valDataContext13 = new ASGFindApproverFlow_TypedDataContext3_ForReadOnly(locations, true);
                return valDataContext13.ValueType___Expr13Get();
            }
            if ((expressionId == 14)) {
                ASGFindApproverFlow_TypedDataContext3 refDataContext14 = new ASGFindApproverFlow_TypedDataContext3(locations, true);
                return refDataContext14.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext14.ValueType___Expr14Get, refDataContext14.ValueType___Expr14Set);
            }
            if ((expressionId == 15)) {
                ASGFindApproverFlow_TypedDataContext3_ForReadOnly valDataContext15 = new ASGFindApproverFlow_TypedDataContext3_ForReadOnly(locations, true);
                return valDataContext15.ValueType___Expr15Get();
            }
            if ((expressionId == 16)) {
                ASGFindApproverFlow_TypedDataContext4_ForReadOnly valDataContext16 = new ASGFindApproverFlow_TypedDataContext4_ForReadOnly(locations, true);
                return valDataContext16.ValueType___Expr16Get();
            }
            if ((expressionId == 17)) {
                ASGFindApproverFlow_TypedDataContext4 refDataContext17 = new ASGFindApproverFlow_TypedDataContext4(locations, true);
                return refDataContext17.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>(refDataContext17.ValueType___Expr17Get, refDataContext17.ValueType___Expr17Set);
            }
            if ((expressionId == 18)) {
                ASGFindApproverFlow_TypedDataContext4_ForReadOnly valDataContext18 = new ASGFindApproverFlow_TypedDataContext4_ForReadOnly(locations, true);
                return valDataContext18.ValueType___Expr18Get();
            }
            if ((expressionId == 19)) {
                ASGFindApproverFlow_TypedDataContext4_ForReadOnly valDataContext19 = new ASGFindApproverFlow_TypedDataContext4_ForReadOnly(locations, true);
                return valDataContext19.ValueType___Expr19Get();
            }
            if ((expressionId == 20)) {
                ASGFindApproverFlow_TypedDataContext4_ForReadOnly valDataContext20 = new ASGFindApproverFlow_TypedDataContext4_ForReadOnly(locations, true);
                return valDataContext20.ValueType___Expr20Get();
            }
            if ((expressionId == 21)) {
                ASGFindApproverFlow_TypedDataContext4_ForReadOnly valDataContext21 = new ASGFindApproverFlow_TypedDataContext4_ForReadOnly(locations, true);
                return valDataContext21.ValueType___Expr21Get();
            }
            if ((expressionId == 22)) {
                ASGFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext22 = new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext22.ValueType___Expr22Get();
            }
            if ((expressionId == 23)) {
                ASGFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext23 = new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext23.ValueType___Expr23Get();
            }
            if ((expressionId == 24)) {
                ASGFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext24 = new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext24.ValueType___Expr24Get();
            }
            if ((expressionId == 25)) {
                ASGFindApproverFlow_TypedDataContext2 refDataContext25 = new ASGFindApproverFlow_TypedDataContext2(locations, true);
                return refDataContext25.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext25.ValueType___Expr25Get, refDataContext25.ValueType___Expr25Set);
            }
            if ((expressionId == 26)) {
                ASGFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext26 = new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext26.ValueType___Expr26Get();
            }
            if ((expressionId == 27)) {
                ASGFindApproverFlow_TypedDataContext2 refDataContext27 = new ASGFindApproverFlow_TypedDataContext2(locations, true);
                return refDataContext27.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext27.ValueType___Expr27Get, refDataContext27.ValueType___Expr27Set);
            }
            if ((expressionId == 28)) {
                ASGFindApproverFlow_TypedDataContext2_ForReadOnly valDataContext28 = new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext28.ValueType___Expr28Get();
            }
            if ((expressionId == 29)) {
                ASGFindApproverFlow_TypedDataContext2 refDataContext29 = new ASGFindApproverFlow_TypedDataContext2(locations, true);
                return refDataContext29.GetLocation<WorkFlowlAPI.FindApproverResult>(refDataContext29.ValueType___Expr29Get, refDataContext29.ValueType___Expr29Set);
            }
            if ((expressionId == 30)) {
                ASGFindApproverFlow_TypedDataContext2 refDataContext30 = new ASGFindApproverFlow_TypedDataContext2(locations, true);
                return refDataContext30.GetLocation<string>(refDataContext30.ValueType___Expr30Get, refDataContext30.ValueType___Expr30Set);
            }
            return null;
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool CanExecuteExpression(string expressionText, bool isReference, System.Collections.Generic.IList<System.Activities.LocationReference> locations, out int expressionId) {
            if (((isReference == false) 
                        && ((expressionText == "new List<WorkFlowApproval>()") 
                        && (ASGFindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 0;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "ApprovalList") 
                        && (ASGFindApproverFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 1;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuoteBusinessLogic.IsOnlyContain968MSSW(QuotationDetails)") 
                        && (ASGFindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 2;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "UserRoleBusinessLogic.getSectorBySalesEmailSalesCode(SalesEmail,SalesCode, \"ASG\")" +
                            "") 
                        && (ASGFindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 3;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "BU_Sector") 
                        && (ASGFindApproverFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 4;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "!string.IsNullOrEmpty(BU_Sector)") 
                        && (ASGFindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 5;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "FindApproverResult.SalesSectorNotFound") 
                        && (ASGFindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 6;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "FindApproverResult") 
                        && (ASGFindApproverFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 7;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "(double)QuoteBusinessLogic.GetTotalMarginByQuotationDetails(QuotationDetails)") 
                        && (ASGFindApproverFlow_TypedDataContext3_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 8;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "TotalMargin") 
                        && (ASGFindApproverFlow_TypedDataContext3.Validate(locations, true, 0) == true)))) {
                expressionId = 9;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "GPControlBusinessLogic.FindApproversByMarginAndSalesTeam((decimal)TotalMargin, BU" +
                            "_Sector)") 
                        && (ASGFindApproverFlow_TypedDataContext3_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 10;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "Approvers") 
                        && (ASGFindApproverFlow_TypedDataContext3.Validate(locations, true, 0) == true)))) {
                expressionId = 11;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "Approvers.Count() > 0") 
                        && (ASGFindApproverFlow_TypedDataContext3_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 12;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "FindApproverResult.GPRuleNotFound") 
                        && (ASGFindApproverFlow_TypedDataContext3_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 13;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "FindApproverResult") 
                        && (ASGFindApproverFlow_TypedDataContext3.Validate(locations, true, 0) == true)))) {
                expressionId = 14;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "Approvers") 
                        && (ASGFindApproverFlow_TypedDataContext3_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 15;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "ApproverType.Sales.ToString()") 
                        && (ASGFindApproverFlow_TypedDataContext4_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 16;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "ApprovalList") 
                        && (ASGFindApproverFlow_TypedDataContext4.Validate(locations, true, 0) == true)))) {
                expressionId = 17;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "Array.IndexOf(Approvers,item)+1") 
                        && (ASGFindApproverFlow_TypedDataContext4_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 18;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuoteId") 
                        && (ASGFindApproverFlow_TypedDataContext4_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 19;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "Url") 
                        && (ASGFindApproverFlow_TypedDataContext4_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 20;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "item") 
                        && (ASGFindApproverFlow_TypedDataContext4_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 21;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "TotalMargin < 0.0") 
                        && (ASGFindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 22;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "ApprovalList!=null && ApprovalList.Count > 0") 
                        && (ASGFindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 23;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "FindApproverResult.NoNeed") 
                        && (ASGFindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 24;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "FindApproverResult") 
                        && (ASGFindApproverFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 25;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "FindApproverResult.NeedApprovalForGP") 
                        && (ASGFindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 26;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "FindApproverResult") 
                        && (ASGFindApproverFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 27;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "FindApproverResult.NegativeGP") 
                        && (ASGFindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 28;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "FindApproverResult") 
                        && (ASGFindApproverFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 29;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "BU_Sector") 
                        && (ASGFindApproverFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 30;
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
                return new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr0GetTree();
            }
            if ((expressionId == 1)) {
                return new ASGFindApproverFlow_TypedDataContext2(locationReferences).@__Expr1GetTree();
            }
            if ((expressionId == 2)) {
                return new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr2GetTree();
            }
            if ((expressionId == 3)) {
                return new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr3GetTree();
            }
            if ((expressionId == 4)) {
                return new ASGFindApproverFlow_TypedDataContext2(locationReferences).@__Expr4GetTree();
            }
            if ((expressionId == 5)) {
                return new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr5GetTree();
            }
            if ((expressionId == 6)) {
                return new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr6GetTree();
            }
            if ((expressionId == 7)) {
                return new ASGFindApproverFlow_TypedDataContext2(locationReferences).@__Expr7GetTree();
            }
            if ((expressionId == 8)) {
                return new ASGFindApproverFlow_TypedDataContext3_ForReadOnly(locationReferences).@__Expr8GetTree();
            }
            if ((expressionId == 9)) {
                return new ASGFindApproverFlow_TypedDataContext3(locationReferences).@__Expr9GetTree();
            }
            if ((expressionId == 10)) {
                return new ASGFindApproverFlow_TypedDataContext3_ForReadOnly(locationReferences).@__Expr10GetTree();
            }
            if ((expressionId == 11)) {
                return new ASGFindApproverFlow_TypedDataContext3(locationReferences).@__Expr11GetTree();
            }
            if ((expressionId == 12)) {
                return new ASGFindApproverFlow_TypedDataContext3_ForReadOnly(locationReferences).@__Expr12GetTree();
            }
            if ((expressionId == 13)) {
                return new ASGFindApproverFlow_TypedDataContext3_ForReadOnly(locationReferences).@__Expr13GetTree();
            }
            if ((expressionId == 14)) {
                return new ASGFindApproverFlow_TypedDataContext3(locationReferences).@__Expr14GetTree();
            }
            if ((expressionId == 15)) {
                return new ASGFindApproverFlow_TypedDataContext3_ForReadOnly(locationReferences).@__Expr15GetTree();
            }
            if ((expressionId == 16)) {
                return new ASGFindApproverFlow_TypedDataContext4_ForReadOnly(locationReferences).@__Expr16GetTree();
            }
            if ((expressionId == 17)) {
                return new ASGFindApproverFlow_TypedDataContext4(locationReferences).@__Expr17GetTree();
            }
            if ((expressionId == 18)) {
                return new ASGFindApproverFlow_TypedDataContext4_ForReadOnly(locationReferences).@__Expr18GetTree();
            }
            if ((expressionId == 19)) {
                return new ASGFindApproverFlow_TypedDataContext4_ForReadOnly(locationReferences).@__Expr19GetTree();
            }
            if ((expressionId == 20)) {
                return new ASGFindApproverFlow_TypedDataContext4_ForReadOnly(locationReferences).@__Expr20GetTree();
            }
            if ((expressionId == 21)) {
                return new ASGFindApproverFlow_TypedDataContext4_ForReadOnly(locationReferences).@__Expr21GetTree();
            }
            if ((expressionId == 22)) {
                return new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr22GetTree();
            }
            if ((expressionId == 23)) {
                return new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr23GetTree();
            }
            if ((expressionId == 24)) {
                return new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr24GetTree();
            }
            if ((expressionId == 25)) {
                return new ASGFindApproverFlow_TypedDataContext2(locationReferences).@__Expr25GetTree();
            }
            if ((expressionId == 26)) {
                return new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr26GetTree();
            }
            if ((expressionId == 27)) {
                return new ASGFindApproverFlow_TypedDataContext2(locationReferences).@__Expr27GetTree();
            }
            if ((expressionId == 28)) {
                return new ASGFindApproverFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr28GetTree();
            }
            if ((expressionId == 29)) {
                return new ASGFindApproverFlow_TypedDataContext2(locationReferences).@__Expr29GetTree();
            }
            if ((expressionId == 30)) {
                return new ASGFindApproverFlow_TypedDataContext2(locationReferences).@__Expr30GetTree();
            }
            return null;
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class ASGFindApproverFlow_TypedDataContext0 : System.Activities.XamlIntegration.CompiledDataContext {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public ASGFindApproverFlow_TypedDataContext0(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ASGFindApproverFlow_TypedDataContext0(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ASGFindApproverFlow_TypedDataContext0(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
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
        private class ASGFindApproverFlow_TypedDataContext0_ForReadOnly : System.Activities.XamlIntegration.CompiledDataContext {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public ASGFindApproverFlow_TypedDataContext0_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ASGFindApproverFlow_TypedDataContext0_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ASGFindApproverFlow_TypedDataContext0_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
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
        private class ASGFindApproverFlow_TypedDataContext1 : ASGFindApproverFlow_TypedDataContext0 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            protected WorkFlowlAPI.FindApproverResult FindApproverResult;
            
            public ASGFindApproverFlow_TypedDataContext1(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ASGFindApproverFlow_TypedDataContext1(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ASGFindApproverFlow_TypedDataContext1(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
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
            
            protected string SalesCode {
                get {
                    return ((string)(this.GetVariableValue((1 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((1 + locationsOffset), value);
                }
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> ApprovalList {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>)(this.GetVariableValue((2 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((2 + locationsOffset), value);
                }
            }
            
            protected string QuoteId {
                get {
                    return ((string)(this.GetVariableValue((4 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((4 + locationsOffset), value);
                }
            }
            
            protected string Url {
                get {
                    return ((string)(this.GetVariableValue((5 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((5 + locationsOffset), value);
                }
            }
            
            protected string BU_Sector {
                get {
                    return ((string)(this.GetVariableValue((6 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((6 + locationsOffset), value);
                }
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> QuotationDetails {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)(this.GetVariableValue((7 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((7 + locationsOffset), value);
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
                this.FindApproverResult = ((WorkFlowlAPI.FindApproverResult)(this.GetVariableValue((3 + locationsOffset))));
                base.GetValueTypeValues();
            }
            
            protected override void SetValueTypeValues() {
                this.SetVariableValue((3 + locationsOffset), this.FindApproverResult);
                base.SetValueTypeValues();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 8))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 8);
                }
                expectedLocationsCount = 8;
                if (((locationReferences[(offset + 0)].Name != "SalesEmail") 
                            || (locationReferences[(offset + 0)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 1)].Name != "SalesCode") 
                            || (locationReferences[(offset + 1)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 2)].Name != "ApprovalList") 
                            || (locationReferences[(offset + 2)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>)))) {
                    return false;
                }
                if (((locationReferences[(offset + 4)].Name != "QuoteId") 
                            || (locationReferences[(offset + 4)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 5)].Name != "Url") 
                            || (locationReferences[(offset + 5)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 6)].Name != "BU_Sector") 
                            || (locationReferences[(offset + 6)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 7)].Name != "QuotationDetails") 
                            || (locationReferences[(offset + 7)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)))) {
                    return false;
                }
                if (((locationReferences[(offset + 3)].Name != "FindApproverResult") 
                            || (locationReferences[(offset + 3)].Type != typeof(WorkFlowlAPI.FindApproverResult)))) {
                    return false;
                }
                return ASGFindApproverFlow_TypedDataContext0.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class ASGFindApproverFlow_TypedDataContext1_ForReadOnly : ASGFindApproverFlow_TypedDataContext0_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            protected WorkFlowlAPI.FindApproverResult FindApproverResult;
            
            public ASGFindApproverFlow_TypedDataContext1_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ASGFindApproverFlow_TypedDataContext1_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ASGFindApproverFlow_TypedDataContext1_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected string SalesEmail {
                get {
                    return ((string)(this.GetVariableValue((0 + locationsOffset))));
                }
            }
            
            protected string SalesCode {
                get {
                    return ((string)(this.GetVariableValue((1 + locationsOffset))));
                }
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> ApprovalList {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>)(this.GetVariableValue((2 + locationsOffset))));
                }
            }
            
            protected string QuoteId {
                get {
                    return ((string)(this.GetVariableValue((4 + locationsOffset))));
                }
            }
            
            protected string Url {
                get {
                    return ((string)(this.GetVariableValue((5 + locationsOffset))));
                }
            }
            
            protected string BU_Sector {
                get {
                    return ((string)(this.GetVariableValue((6 + locationsOffset))));
                }
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> QuotationDetails {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)(this.GetVariableValue((7 + locationsOffset))));
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
                this.FindApproverResult = ((WorkFlowlAPI.FindApproverResult)(this.GetVariableValue((3 + locationsOffset))));
                base.GetValueTypeValues();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 8))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 8);
                }
                expectedLocationsCount = 8;
                if (((locationReferences[(offset + 0)].Name != "SalesEmail") 
                            || (locationReferences[(offset + 0)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 1)].Name != "SalesCode") 
                            || (locationReferences[(offset + 1)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 2)].Name != "ApprovalList") 
                            || (locationReferences[(offset + 2)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>)))) {
                    return false;
                }
                if (((locationReferences[(offset + 4)].Name != "QuoteId") 
                            || (locationReferences[(offset + 4)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 5)].Name != "Url") 
                            || (locationReferences[(offset + 5)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 6)].Name != "BU_Sector") 
                            || (locationReferences[(offset + 6)].Type != typeof(string)))) {
                    return false;
                }
                if (((locationReferences[(offset + 7)].Name != "QuotationDetails") 
                            || (locationReferences[(offset + 7)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)))) {
                    return false;
                }
                if (((locationReferences[(offset + 3)].Name != "FindApproverResult") 
                            || (locationReferences[(offset + 3)].Type != typeof(WorkFlowlAPI.FindApproverResult)))) {
                    return false;
                }
                return ASGFindApproverFlow_TypedDataContext0_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class ASGFindApproverFlow_TypedDataContext2 : ASGFindApproverFlow_TypedDataContext1 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            protected int levelNum;
            
            protected double TotalMargin;
            
            public ASGFindApproverFlow_TypedDataContext2(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ASGFindApproverFlow_TypedDataContext2(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ASGFindApproverFlow_TypedDataContext2(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC> GPRuleList {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC>)(this.GetVariableValue((8 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((8 + locationsOffset), value);
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
                
                #line 75 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>> expression = () => 
              ApprovalList;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> @__Expr1Get() {
                
                #line 75 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
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
                
                #line 75 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                
              ApprovalList = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr1Set(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> value) {
                this.GetValueTypeValues();
                this.@__Expr1Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr4GetTree() {
                
                #line 311 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                      BU_Sector;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr4Get() {
                
                #line 311 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                return 
                      BU_Sector;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr4Get() {
                this.GetValueTypeValues();
                return this.@__Expr4Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr4Set(string value) {
                
                #line 311 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                
                      BU_Sector = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr4Set(string value) {
                this.GetValueTypeValues();
                this.@__Expr4Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr7GetTree() {
                
                #line 291 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.FindApproverResult>> expression = () => 
                              FindApproverResult;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.FindApproverResult @__Expr7Get() {
                
                #line 291 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                return 
                              FindApproverResult;
                
                #line default
                #line hidden
            }
            
            public WorkFlowlAPI.FindApproverResult ValueType___Expr7Get() {
                this.GetValueTypeValues();
                return this.@__Expr7Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr7Set(WorkFlowlAPI.FindApproverResult value) {
                
                #line 291 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                
                              FindApproverResult = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr7Set(WorkFlowlAPI.FindApproverResult value) {
                this.GetValueTypeValues();
                this.@__Expr7Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr25GetTree() {
                
                #line 269 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.FindApproverResult>> expression = () => 
                                          FindApproverResult;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.FindApproverResult @__Expr25Get() {
                
                #line 269 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
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
                
                #line 269 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                
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
                
                #line 253 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.FindApproverResult>> expression = () => 
                                          FindApproverResult;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.FindApproverResult @__Expr27Get() {
                
                #line 253 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
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
                
                #line 253 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                
                                          FindApproverResult = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr27Set(WorkFlowlAPI.FindApproverResult value) {
                this.GetValueTypeValues();
                this.@__Expr27Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr29GetTree() {
                
                #line 232 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.FindApproverResult>> expression = () => 
                                      FindApproverResult;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.FindApproverResult @__Expr29Get() {
                
                #line 232 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                return 
                                      FindApproverResult;
                
                #line default
                #line hidden
            }
            
            public WorkFlowlAPI.FindApproverResult ValueType___Expr29Get() {
                this.GetValueTypeValues();
                return this.@__Expr29Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr29Set(WorkFlowlAPI.FindApproverResult value) {
                
                #line 232 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                
                                      FindApproverResult = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr29Set(WorkFlowlAPI.FindApproverResult value) {
                this.GetValueTypeValues();
                this.@__Expr29Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr30GetTree() {
                
                #line 94 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                      BU_Sector;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr30Get() {
                
                #line 94 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                return 
                      BU_Sector;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr30Get() {
                this.GetValueTypeValues();
                return this.@__Expr30Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr30Set(string value) {
                
                #line 94 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                
                      BU_Sector = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr30Set(string value) {
                this.GetValueTypeValues();
                this.@__Expr30Set(value);
                this.SetValueTypeValues();
            }
            
            protected override void GetValueTypeValues() {
                this.levelNum = ((int)(this.GetVariableValue((9 + locationsOffset))));
                this.TotalMargin = ((double)(this.GetVariableValue((10 + locationsOffset))));
                base.GetValueTypeValues();
            }
            
            protected override void SetValueTypeValues() {
                this.SetVariableValue((9 + locationsOffset), this.levelNum);
                this.SetVariableValue((10 + locationsOffset), this.TotalMargin);
                base.SetValueTypeValues();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 11))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 11);
                }
                expectedLocationsCount = 11;
                if (((locationReferences[(offset + 8)].Name != "GPRuleList") 
                            || (locationReferences[(offset + 8)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC>)))) {
                    return false;
                }
                if (((locationReferences[(offset + 9)].Name != "levelNum") 
                            || (locationReferences[(offset + 9)].Type != typeof(int)))) {
                    return false;
                }
                if (((locationReferences[(offset + 10)].Name != "TotalMargin") 
                            || (locationReferences[(offset + 10)].Type != typeof(double)))) {
                    return false;
                }
                return ASGFindApproverFlow_TypedDataContext1.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class ASGFindApproverFlow_TypedDataContext2_ForReadOnly : ASGFindApproverFlow_TypedDataContext1_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            protected int levelNum;
            
            protected double TotalMargin;
            
            public ASGFindApproverFlow_TypedDataContext2_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ASGFindApproverFlow_TypedDataContext2_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ASGFindApproverFlow_TypedDataContext2_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC> GPRuleList {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC>)(this.GetVariableValue((8 + locationsOffset))));
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
                
                #line 80 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>> expression = () => 
              new List<WorkFlowApproval>();
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> @__Expr0Get() {
                
                #line 80 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
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
                
                #line 87 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<bool>> expression = () => 
              QuoteBusinessLogic.IsOnlyContain968MSSW(QuotationDetails);
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public bool @__Expr2Get() {
                
                #line 87 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                return 
              QuoteBusinessLogic.IsOnlyContain968MSSW(QuotationDetails);
                
                #line default
                #line hidden
            }
            
            public bool ValueType___Expr2Get() {
                this.GetValueTypeValues();
                return this.@__Expr2Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr3GetTree() {
                
                #line 316 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                      UserRoleBusinessLogic.getSectorBySalesEmailSalesCode(SalesEmail,SalesCode, "ASG");
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr3Get() {
                
                #line 316 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                return 
                      UserRoleBusinessLogic.getSectorBySalesEmailSalesCode(SalesEmail,SalesCode, "ASG");
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr3Get() {
                this.GetValueTypeValues();
                return this.@__Expr3Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr5GetTree() {
                
                #line 104 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<bool>> expression = () => 
                      !string.IsNullOrEmpty(BU_Sector);
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public bool @__Expr5Get() {
                
                #line 104 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                return 
                      !string.IsNullOrEmpty(BU_Sector);
                
                #line default
                #line hidden
            }
            
            public bool ValueType___Expr5Get() {
                this.GetValueTypeValues();
                return this.@__Expr5Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr6GetTree() {
                
                #line 296 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.FindApproverResult>> expression = () => 
                              FindApproverResult.SalesSectorNotFound;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.FindApproverResult @__Expr6Get() {
                
                #line 296 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                return 
                              FindApproverResult.SalesSectorNotFound;
                
                #line default
                #line hidden
            }
            
            public WorkFlowlAPI.FindApproverResult ValueType___Expr6Get() {
                this.GetValueTypeValues();
                return this.@__Expr6Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr22GetTree() {
                
                #line 225 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<bool>> expression = () => 
                              TotalMargin < 0.0;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public bool @__Expr22Get() {
                
                #line 225 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                return 
                              TotalMargin < 0.0;
                
                #line default
                #line hidden
            }
            
            public bool ValueType___Expr22Get() {
                this.GetValueTypeValues();
                return this.@__Expr22Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr23GetTree() {
                
                #line 246 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<bool>> expression = () => 
                                  ApprovalList!=null && ApprovalList.Count > 0;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public bool @__Expr23Get() {
                
                #line 246 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
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
                
                #line 274 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.FindApproverResult>> expression = () => 
                                          FindApproverResult.NoNeed;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.FindApproverResult @__Expr24Get() {
                
                #line 274 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
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
                
                #line 258 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.FindApproverResult>> expression = () => 
                                          FindApproverResult.NeedApprovalForGP;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.FindApproverResult @__Expr26Get() {
                
                #line 258 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
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
                
                #line 237 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.FindApproverResult>> expression = () => 
                                      FindApproverResult.NegativeGP;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.FindApproverResult @__Expr28Get() {
                
                #line 237 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                return 
                                      FindApproverResult.NegativeGP;
                
                #line default
                #line hidden
            }
            
            public WorkFlowlAPI.FindApproverResult ValueType___Expr28Get() {
                this.GetValueTypeValues();
                return this.@__Expr28Get();
            }
            
            protected override void GetValueTypeValues() {
                this.levelNum = ((int)(this.GetVariableValue((9 + locationsOffset))));
                this.TotalMargin = ((double)(this.GetVariableValue((10 + locationsOffset))));
                base.GetValueTypeValues();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 11))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 11);
                }
                expectedLocationsCount = 11;
                if (((locationReferences[(offset + 8)].Name != "GPRuleList") 
                            || (locationReferences[(offset + 8)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.GPBLOCK_LOGIC>)))) {
                    return false;
                }
                if (((locationReferences[(offset + 9)].Name != "levelNum") 
                            || (locationReferences[(offset + 9)].Type != typeof(int)))) {
                    return false;
                }
                if (((locationReferences[(offset + 10)].Name != "TotalMargin") 
                            || (locationReferences[(offset + 10)].Type != typeof(double)))) {
                    return false;
                }
                return ASGFindApproverFlow_TypedDataContext1_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class ASGFindApproverFlow_TypedDataContext3 : ASGFindApproverFlow_TypedDataContext2 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            protected bool ViewGP;
            
            public ASGFindApproverFlow_TypedDataContext3(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ASGFindApproverFlow_TypedDataContext3(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ASGFindApproverFlow_TypedDataContext3(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected string[] Approvers {
                get {
                    return ((string[])(this.GetVariableValue((12 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((12 + locationsOffset), value);
                }
            }
            
            internal new static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public new virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
                base.SetLocationsOffset(locationsOffset);
            }
            
            internal System.Linq.Expressions.Expression @__Expr9GetTree() {
                
                #line 118 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<double>> expression = () => 
                                    TotalMargin;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public double @__Expr9Get() {
                
                #line 118 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                return 
                                    TotalMargin;
                
                #line default
                #line hidden
            }
            
            public double ValueType___Expr9Get() {
                this.GetValueTypeValues();
                return this.@__Expr9Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr9Set(double value) {
                
                #line 118 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                
                                    TotalMargin = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr9Set(double value) {
                this.GetValueTypeValues();
                this.@__Expr9Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr11GetTree() {
                
                #line 132 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string[]>> expression = () => 
                                        Approvers;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string[] @__Expr11Get() {
                
                #line 132 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                return 
                                        Approvers;
                
                #line default
                #line hidden
            }
            
            public string[] ValueType___Expr11Get() {
                this.GetValueTypeValues();
                return this.@__Expr11Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr11Set(string[] value) {
                
                #line 132 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                
                                        Approvers = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr11Set(string[] value) {
                this.GetValueTypeValues();
                this.@__Expr11Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr14GetTree() {
                
                #line 199 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.FindApproverResult>> expression = () => 
                                                FindApproverResult;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.FindApproverResult @__Expr14Get() {
                
                #line 199 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                return 
                                                FindApproverResult;
                
                #line default
                #line hidden
            }
            
            public WorkFlowlAPI.FindApproverResult ValueType___Expr14Get() {
                this.GetValueTypeValues();
                return this.@__Expr14Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr14Set(WorkFlowlAPI.FindApproverResult value) {
                
                #line 199 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                
                                                FindApproverResult = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr14Set(WorkFlowlAPI.FindApproverResult value) {
                this.GetValueTypeValues();
                this.@__Expr14Set(value);
                this.SetValueTypeValues();
            }
            
            protected override void GetValueTypeValues() {
                this.ViewGP = ((bool)(this.GetVariableValue((11 + locationsOffset))));
                base.GetValueTypeValues();
            }
            
            protected override void SetValueTypeValues() {
                this.SetVariableValue((11 + locationsOffset), this.ViewGP);
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
                if (((locationReferences[(offset + 12)].Name != "Approvers") 
                            || (locationReferences[(offset + 12)].Type != typeof(string[])))) {
                    return false;
                }
                if (((locationReferences[(offset + 11)].Name != "ViewGP") 
                            || (locationReferences[(offset + 11)].Type != typeof(bool)))) {
                    return false;
                }
                return ASGFindApproverFlow_TypedDataContext2.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class ASGFindApproverFlow_TypedDataContext3_ForReadOnly : ASGFindApproverFlow_TypedDataContext2_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            protected bool ViewGP;
            
            public ASGFindApproverFlow_TypedDataContext3_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ASGFindApproverFlow_TypedDataContext3_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ASGFindApproverFlow_TypedDataContext3_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected string[] Approvers {
                get {
                    return ((string[])(this.GetVariableValue((12 + locationsOffset))));
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
                
                #line 123 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<double>> expression = () => 
                                    (double)QuoteBusinessLogic.GetTotalMarginByQuotationDetails(QuotationDetails);
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public double @__Expr8Get() {
                
                #line 123 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                return 
                                    (double)QuoteBusinessLogic.GetTotalMarginByQuotationDetails(QuotationDetails);
                
                #line default
                #line hidden
            }
            
            public double ValueType___Expr8Get() {
                this.GetValueTypeValues();
                return this.@__Expr8Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr10GetTree() {
                
                #line 137 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string[]>> expression = () => 
                                        GPControlBusinessLogic.FindApproversByMarginAndSalesTeam((decimal)TotalMargin, BU_Sector);
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string[] @__Expr10Get() {
                
                #line 137 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                return 
                                        GPControlBusinessLogic.FindApproversByMarginAndSalesTeam((decimal)TotalMargin, BU_Sector);
                
                #line default
                #line hidden
            }
            
            public string[] ValueType___Expr10Get() {
                this.GetValueTypeValues();
                return this.@__Expr10Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr12GetTree() {
                
                #line 144 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<bool>> expression = () => 
                                        Approvers.Count() > 0;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public bool @__Expr12Get() {
                
                #line 144 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                return 
                                        Approvers.Count() > 0;
                
                #line default
                #line hidden
            }
            
            public bool ValueType___Expr12Get() {
                this.GetValueTypeValues();
                return this.@__Expr12Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr13GetTree() {
                
                #line 204 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<WorkFlowlAPI.FindApproverResult>> expression = () => 
                                                FindApproverResult.GPRuleNotFound;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public WorkFlowlAPI.FindApproverResult @__Expr13Get() {
                
                #line 204 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                return 
                                                FindApproverResult.GPRuleNotFound;
                
                #line default
                #line hidden
            }
            
            public WorkFlowlAPI.FindApproverResult ValueType___Expr13Get() {
                this.GetValueTypeValues();
                return this.@__Expr13Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr15GetTree() {
                
                #line 151 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.IEnumerable<string>>> expression = () => 
                                                Approvers;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.IEnumerable<string> @__Expr15Get() {
                
                #line 151 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                return 
                                                Approvers;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.IEnumerable<string> ValueType___Expr15Get() {
                this.GetValueTypeValues();
                return this.@__Expr15Get();
            }
            
            protected override void GetValueTypeValues() {
                this.ViewGP = ((bool)(this.GetVariableValue((11 + locationsOffset))));
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
                if (((locationReferences[(offset + 12)].Name != "Approvers") 
                            || (locationReferences[(offset + 12)].Type != typeof(string[])))) {
                    return false;
                }
                if (((locationReferences[(offset + 11)].Name != "ViewGP") 
                            || (locationReferences[(offset + 11)].Type != typeof(bool)))) {
                    return false;
                }
                return ASGFindApproverFlow_TypedDataContext2_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class ASGFindApproverFlow_TypedDataContext4 : ASGFindApproverFlow_TypedDataContext3 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public ASGFindApproverFlow_TypedDataContext4(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ASGFindApproverFlow_TypedDataContext4(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ASGFindApproverFlow_TypedDataContext4(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected string item {
                get {
                    return ((string)(this.GetVariableValue((13 + locationsOffset))));
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
            
            internal System.Linq.Expressions.Expression @__Expr17GetTree() {
                
                #line 161 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval>>> expression = () => 
                                                    ApprovalList;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.WorkFlowApproval> @__Expr17Get() {
                
                #line 161 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
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
                
                #line 161 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                
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
                            && (locationReferences.Count < 14))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 14);
                }
                expectedLocationsCount = 14;
                if (((locationReferences[(offset + 13)].Name != "item") 
                            || (locationReferences[(offset + 13)].Type != typeof(string)))) {
                    return false;
                }
                return ASGFindApproverFlow_TypedDataContext3.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class ASGFindApproverFlow_TypedDataContext4_ForReadOnly : ASGFindApproverFlow_TypedDataContext3_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public ASGFindApproverFlow_TypedDataContext4_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ASGFindApproverFlow_TypedDataContext4_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public ASGFindApproverFlow_TypedDataContext4_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected string item {
                get {
                    return ((string)(this.GetVariableValue((13 + locationsOffset))));
                }
            }
            
            internal new static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public new virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
                base.SetLocationsOffset(locationsOffset);
            }
            
            internal System.Linq.Expressions.Expression @__Expr16GetTree() {
                
                #line 171 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                                                    ApproverType.Sales.ToString();
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr16Get() {
                
                #line 171 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
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
                
                #line 176 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<double>> expression = () => 
                                                    Array.IndexOf(Approvers,item)+1;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public double @__Expr18Get() {
                
                #line 176 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                return 
                                                    Array.IndexOf(Approvers,item)+1;
                
                #line default
                #line hidden
            }
            
            public double ValueType___Expr18Get() {
                this.GetValueTypeValues();
                return this.@__Expr18Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr19GetTree() {
                
                #line 181 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                                                    QuoteId;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr19Get() {
                
                #line 181 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
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
                
                #line 186 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                                                    Url;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr20Get() {
                
                #line 186 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
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
                
                #line 166 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                                                    item;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr21Get() {
                
                #line 166 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\ASGGPAPPROVAL\ASGFINDAPPROVERFLOW.XAML"
                return 
                                                    item;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr21Get() {
                this.GetValueTypeValues();
                return this.@__Expr21Get();
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
                            || (locationReferences[(offset + 13)].Type != typeof(string)))) {
                    return false;
                }
                return ASGFindApproverFlow_TypedDataContext3_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
    }
}
