namespace WorkFlowlAPI.SimulatePrice {
    
    #line 22 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\SimulatePrice\SimulateSAPPriceITPFlow.xaml"
    using System;
    
    #line default
    #line hidden
    
    #line 1 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\SimulatePrice\SimulateSAPPriceITPFlow.xaml"
    using System.Collections;
    
    #line default
    #line hidden
    
    #line 23 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\SimulatePrice\SimulateSAPPriceITPFlow.xaml"
    using System.Collections.Generic;
    
    #line default
    #line hidden
    
    #line 1 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\SimulatePrice\SimulateSAPPriceITPFlow.xaml"
    using System.Activities;
    
    #line default
    #line hidden
    
    #line 1 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\SimulatePrice\SimulateSAPPriceITPFlow.xaml"
    using System.Activities.Expressions;
    
    #line default
    #line hidden
    
    #line 1 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\SimulatePrice\SimulateSAPPriceITPFlow.xaml"
    using System.Activities.Statements;
    
    #line default
    #line hidden
    
    #line 24 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\SimulatePrice\SimulateSAPPriceITPFlow.xaml"
    using System.Data;
    
    #line default
    #line hidden
    
    #line 25 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\SimulatePrice\SimulateSAPPriceITPFlow.xaml"
    using System.Linq;
    
    #line default
    #line hidden
    
    #line 26 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\SimulatePrice\SimulateSAPPriceITPFlow.xaml"
    using System.Text;
    
    #line default
    #line hidden
    
    #line 27 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\SimulatePrice\SimulateSAPPriceITPFlow.xaml"
    using Advantech.Myadvantech.DataAccess;
    
    #line default
    #line hidden
    
    #line 1 "D:\Git\GitHub\eQuotation\MyAdvantechAPI\WorkFlowlAPI\SimulatePrice\SimulateSAPPriceITPFlow.xaml"
    using System.Activities.XamlIntegration;
    
    #line default
    #line hidden
    
    
    public partial class SimulateSAPPriceITPFlow : System.Activities.XamlIntegration.ICompiledExpressionRoot {
        
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
                this.dataContextActivities = SimulateSAPPriceITPFlow_TypedDataContext1.GetDataContextActivitiesHelper(this.rootActivity, this.forImplementation);
            }
            if ((expressionId == 0)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext1.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new SimulateSAPPriceITPFlow_TypedDataContext1(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext1 refDataContext0 = ((SimulateSAPPriceITPFlow_TypedDataContext1)(cachedCompiledDataContext[0]));
                return refDataContext0.GetLocation<Advantech.Myadvantech.DataAccess.QuotationMaster>(refDataContext0.ValueType___Expr0Get, refDataContext0.ValueType___Expr0Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 1)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext1.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new SimulateSAPPriceITPFlow_TypedDataContext1(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext1 refDataContext1 = ((SimulateSAPPriceITPFlow_TypedDataContext1)(cachedCompiledDataContext[0]));
                return refDataContext1.GetLocation<string>(refDataContext1.ValueType___Expr1Get, refDataContext1.ValueType___Expr1Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 2)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext1_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new SimulateSAPPriceITPFlow_TypedDataContext1_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext1_ForReadOnly valDataContext2 = ((SimulateSAPPriceITPFlow_TypedDataContext1_ForReadOnly)(cachedCompiledDataContext[1]));
                return valDataContext2.ValueType___Expr2Get();
            }
            if ((expressionId == 3)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext1.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new SimulateSAPPriceITPFlow_TypedDataContext1(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext1 refDataContext3 = ((SimulateSAPPriceITPFlow_TypedDataContext1)(cachedCompiledDataContext[0]));
                return refDataContext3.GetLocation<Advantech.Myadvantech.DataAccess.QuotationMaster>(refDataContext3.ValueType___Expr3Get, refDataContext3.ValueType___Expr3Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 4)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext1.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new SimulateSAPPriceITPFlow_TypedDataContext1(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext1 refDataContext4 = ((SimulateSAPPriceITPFlow_TypedDataContext1)(cachedCompiledDataContext[0]));
                return refDataContext4.GetLocation<string>(refDataContext4.ValueType___Expr4Get, refDataContext4.ValueType___Expr4Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 5)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext5 = ((SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[2]));
                return valDataContext5.ValueType___Expr5Get();
            }
            if ((expressionId == 6)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext6 = ((SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[2]));
                return valDataContext6.ValueType___Expr6Get();
            }
            if ((expressionId == 7)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext7 = ((SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[2]));
                return valDataContext7.ValueType___Expr7Get();
            }
            if ((expressionId == 8)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext8 = ((SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[2]));
                return valDataContext8.ValueType___Expr8Get();
            }
            if ((expressionId == 9)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext9 = ((SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[2]));
                return valDataContext9.ValueType___Expr9Get();
            }
            if ((expressionId == 10)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[3] == null)) {
                    cachedCompiledDataContext[3] = new SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly valDataContext10 = ((SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly)(cachedCompiledDataContext[3]));
                return valDataContext10.ValueType___Expr10Get();
            }
            if ((expressionId == 11)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[3] == null)) {
                    cachedCompiledDataContext[3] = new SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly valDataContext11 = ((SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly)(cachedCompiledDataContext[3]));
                return valDataContext11.ValueType___Expr11Get();
            }
            if ((expressionId == 12)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[3] == null)) {
                    cachedCompiledDataContext[3] = new SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly valDataContext12 = ((SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly)(cachedCompiledDataContext[3]));
                return valDataContext12.ValueType___Expr12Get();
            }
            if ((expressionId == 13)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext13 = ((SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[2]));
                return valDataContext13.ValueType___Expr13Get();
            }
            if ((expressionId == 14)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext4_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[4] == null)) {
                    cachedCompiledDataContext[4] = new SimulateSAPPriceITPFlow_TypedDataContext4_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext4_ForReadOnly valDataContext14 = ((SimulateSAPPriceITPFlow_TypedDataContext4_ForReadOnly)(cachedCompiledDataContext[4]));
                return valDataContext14.ValueType___Expr14Get();
            }
            if ((expressionId == 15)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext4_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[4] == null)) {
                    cachedCompiledDataContext[4] = new SimulateSAPPriceITPFlow_TypedDataContext4_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext4_ForReadOnly valDataContext15 = ((SimulateSAPPriceITPFlow_TypedDataContext4_ForReadOnly)(cachedCompiledDataContext[4]));
                return valDataContext15.ValueType___Expr15Get();
            }
            if ((expressionId == 16)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext16 = ((SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[2]));
                return valDataContext16.ValueType___Expr16Get();
            }
            if ((expressionId == 17)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[5] == null)) {
                    cachedCompiledDataContext[5] = new SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly valDataContext17 = ((SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly)(cachedCompiledDataContext[5]));
                return valDataContext17.ValueType___Expr17Get();
            }
            if ((expressionId == 18)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[5] == null)) {
                    cachedCompiledDataContext[5] = new SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly valDataContext18 = ((SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly)(cachedCompiledDataContext[5]));
                return valDataContext18.ValueType___Expr18Get();
            }
            if ((expressionId == 19)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[5] == null)) {
                    cachedCompiledDataContext[5] = new SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly valDataContext19 = ((SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly)(cachedCompiledDataContext[5]));
                return valDataContext19.ValueType___Expr19Get();
            }
            if ((expressionId == 20)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext20 = ((SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[2]));
                return valDataContext20.ValueType___Expr20Get();
            }
            if ((expressionId == 21)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext6_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[6] == null)) {
                    cachedCompiledDataContext[6] = new SimulateSAPPriceITPFlow_TypedDataContext6_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext6_ForReadOnly valDataContext21 = ((SimulateSAPPriceITPFlow_TypedDataContext6_ForReadOnly)(cachedCompiledDataContext[6]));
                return valDataContext21.ValueType___Expr21Get();
            }
            if ((expressionId == 22)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext6_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[6] == null)) {
                    cachedCompiledDataContext[6] = new SimulateSAPPriceITPFlow_TypedDataContext6_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext6_ForReadOnly valDataContext22 = ((SimulateSAPPriceITPFlow_TypedDataContext6_ForReadOnly)(cachedCompiledDataContext[6]));
                return valDataContext22.ValueType___Expr22Get();
            }
            if ((expressionId == 23)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext23 = ((SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[2]));
                return valDataContext23.ValueType___Expr23Get();
            }
            if ((expressionId == 24)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[7] == null)) {
                    cachedCompiledDataContext[7] = new SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly valDataContext24 = ((SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly)(cachedCompiledDataContext[7]));
                return valDataContext24.ValueType___Expr24Get();
            }
            if ((expressionId == 25)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[7] == null)) {
                    cachedCompiledDataContext[7] = new SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly valDataContext25 = ((SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly)(cachedCompiledDataContext[7]));
                return valDataContext25.ValueType___Expr25Get();
            }
            if ((expressionId == 26)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[7] == null)) {
                    cachedCompiledDataContext[7] = new SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly valDataContext26 = ((SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly)(cachedCompiledDataContext[7]));
                return valDataContext26.ValueType___Expr26Get();
            }
            if ((expressionId == 27)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext27 = ((SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[2]));
                return valDataContext27.ValueType___Expr27Get();
            }
            if ((expressionId == 28)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext8_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[8] == null)) {
                    cachedCompiledDataContext[8] = new SimulateSAPPriceITPFlow_TypedDataContext8_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext8_ForReadOnly valDataContext28 = ((SimulateSAPPriceITPFlow_TypedDataContext8_ForReadOnly)(cachedCompiledDataContext[8]));
                return valDataContext28.ValueType___Expr28Get();
            }
            if ((expressionId == 29)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext8_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[8] == null)) {
                    cachedCompiledDataContext[8] = new SimulateSAPPriceITPFlow_TypedDataContext8_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext8_ForReadOnly valDataContext29 = ((SimulateSAPPriceITPFlow_TypedDataContext8_ForReadOnly)(cachedCompiledDataContext[8]));
                return valDataContext29.ValueType___Expr29Get();
            }
            if ((expressionId == 30)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext30 = ((SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[2]));
                return valDataContext30.ValueType___Expr30Get();
            }
            if ((expressionId == 31)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[9] == null)) {
                    cachedCompiledDataContext[9] = new SimulateSAPPriceITPFlow_TypedDataContext2(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext2 refDataContext31 = ((SimulateSAPPriceITPFlow_TypedDataContext2)(cachedCompiledDataContext[9]));
                return refDataContext31.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>>(refDataContext31.ValueType___Expr31Get, refDataContext31.ValueType___Expr31Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 32)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext32 = ((SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[2]));
                return valDataContext32.ValueType___Expr32Get();
            }
            if ((expressionId == 33)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext2.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[9] == null)) {
                    cachedCompiledDataContext[9] = new SimulateSAPPriceITPFlow_TypedDataContext2(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext2 refDataContext33 = ((SimulateSAPPriceITPFlow_TypedDataContext2)(cachedCompiledDataContext[9]));
                return refDataContext33.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>>(refDataContext33.ValueType___Expr33Get, refDataContext33.ValueType___Expr33Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 34)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext9.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[10] == null)) {
                    cachedCompiledDataContext[10] = new SimulateSAPPriceITPFlow_TypedDataContext9(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext9 refDataContext34 = ((SimulateSAPPriceITPFlow_TypedDataContext9)(cachedCompiledDataContext[10]));
                return refDataContext34.GetLocation<Advantech.Myadvantech.DataAccess.QuotationMaster>(refDataContext34.ValueType___Expr34Get, refDataContext34.ValueType___Expr34Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 35)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext9_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[11] == null)) {
                    cachedCompiledDataContext[11] = new SimulateSAPPriceITPFlow_TypedDataContext9_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext9_ForReadOnly valDataContext35 = ((SimulateSAPPriceITPFlow_TypedDataContext9_ForReadOnly)(cachedCompiledDataContext[11]));
                return valDataContext35.ValueType___Expr35Get();
            }
            if ((expressionId == 36)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext10_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[12] == null)) {
                    cachedCompiledDataContext[12] = new SimulateSAPPriceITPFlow_TypedDataContext10_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext10_ForReadOnly valDataContext36 = ((SimulateSAPPriceITPFlow_TypedDataContext10_ForReadOnly)(cachedCompiledDataContext[12]));
                return valDataContext36.ValueType___Expr36Get();
            }
            if ((expressionId == 37)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext10_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[12] == null)) {
                    cachedCompiledDataContext[12] = new SimulateSAPPriceITPFlow_TypedDataContext10_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext10_ForReadOnly valDataContext37 = ((SimulateSAPPriceITPFlow_TypedDataContext10_ForReadOnly)(cachedCompiledDataContext[12]));
                return valDataContext37.ValueType___Expr37Get();
            }
            if ((expressionId == 38)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext38 = ((SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[2]));
                return valDataContext38.ValueType___Expr38Get();
            }
            if ((expressionId == 39)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext11_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[13] == null)) {
                    cachedCompiledDataContext[13] = new SimulateSAPPriceITPFlow_TypedDataContext11_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext11_ForReadOnly valDataContext39 = ((SimulateSAPPriceITPFlow_TypedDataContext11_ForReadOnly)(cachedCompiledDataContext[13]));
                return valDataContext39.ValueType___Expr39Get();
            }
            if ((expressionId == 40)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext11_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[13] == null)) {
                    cachedCompiledDataContext[13] = new SimulateSAPPriceITPFlow_TypedDataContext11_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext11_ForReadOnly valDataContext40 = ((SimulateSAPPriceITPFlow_TypedDataContext11_ForReadOnly)(cachedCompiledDataContext[13]));
                return valDataContext40.ValueType___Expr40Get();
            }
            if ((expressionId == 41)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext41 = ((SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[2]));
                return valDataContext41.ValueType___Expr41Get();
            }
            if ((expressionId == 42)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext12_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[14] == null)) {
                    cachedCompiledDataContext[14] = new SimulateSAPPriceITPFlow_TypedDataContext12_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext12_ForReadOnly valDataContext42 = ((SimulateSAPPriceITPFlow_TypedDataContext12_ForReadOnly)(cachedCompiledDataContext[14]));
                return valDataContext42.ValueType___Expr42Get();
            }
            if ((expressionId == 43)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext12_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[14] == null)) {
                    cachedCompiledDataContext[14] = new SimulateSAPPriceITPFlow_TypedDataContext12_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext12_ForReadOnly valDataContext43 = ((SimulateSAPPriceITPFlow_TypedDataContext12_ForReadOnly)(cachedCompiledDataContext[14]));
                return valDataContext43.ValueType___Expr43Get();
            }
            if ((expressionId == 44)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[2] == null)) {
                    cachedCompiledDataContext[2] = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext44 = ((SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly)(cachedCompiledDataContext[2]));
                return valDataContext44.ValueType___Expr44Get();
            }
            if ((expressionId == 45)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext13_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[15] == null)) {
                    cachedCompiledDataContext[15] = new SimulateSAPPriceITPFlow_TypedDataContext13_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext13_ForReadOnly valDataContext45 = ((SimulateSAPPriceITPFlow_TypedDataContext13_ForReadOnly)(cachedCompiledDataContext[15]));
                return valDataContext45.ValueType___Expr45Get();
            }
            if ((expressionId == 46)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext13_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[15] == null)) {
                    cachedCompiledDataContext[15] = new SimulateSAPPriceITPFlow_TypedDataContext13_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext13_ForReadOnly valDataContext46 = ((SimulateSAPPriceITPFlow_TypedDataContext13_ForReadOnly)(cachedCompiledDataContext[15]));
                return valDataContext46.ValueType___Expr46Get();
            }
            if ((expressionId == 47)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext1.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new SimulateSAPPriceITPFlow_TypedDataContext1(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext1 refDataContext47 = ((SimulateSAPPriceITPFlow_TypedDataContext1)(cachedCompiledDataContext[0]));
                return refDataContext47.GetLocation<Advantech.Myadvantech.DataAccess.QuotationMaster>(refDataContext47.ValueType___Expr47Get, refDataContext47.ValueType___Expr47Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 48)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext1_ForReadOnly.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[1] == null)) {
                    cachedCompiledDataContext[1] = new SimulateSAPPriceITPFlow_TypedDataContext1_ForReadOnly(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext1_ForReadOnly valDataContext48 = ((SimulateSAPPriceITPFlow_TypedDataContext1_ForReadOnly)(cachedCompiledDataContext[1]));
                return valDataContext48.ValueType___Expr48Get();
            }
            if ((expressionId == 49)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext1.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new SimulateSAPPriceITPFlow_TypedDataContext1(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext1 refDataContext49 = ((SimulateSAPPriceITPFlow_TypedDataContext1)(cachedCompiledDataContext[0]));
                return refDataContext49.GetLocation<Advantech.Myadvantech.DataAccess.QuotationMaster>(refDataContext49.ValueType___Expr49Get, refDataContext49.ValueType___Expr49Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 50)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext1.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new SimulateSAPPriceITPFlow_TypedDataContext1(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext1 refDataContext50 = ((SimulateSAPPriceITPFlow_TypedDataContext1)(cachedCompiledDataContext[0]));
                return refDataContext50.GetLocation<Advantech.Myadvantech.DataAccess.QuotationMaster>(refDataContext50.ValueType___Expr50Get, refDataContext50.ValueType___Expr50Set, expressionId, this.rootActivity, activityContext);
            }
            if ((expressionId == 51)) {
                System.Activities.XamlIntegration.CompiledDataContext[] cachedCompiledDataContext = SimulateSAPPriceITPFlow_TypedDataContext1.GetCompiledDataContextCacheHelper(this.dataContextActivities, activityContext, this.rootActivity, this.forImplementation, 16);
                if ((cachedCompiledDataContext[0] == null)) {
                    cachedCompiledDataContext[0] = new SimulateSAPPriceITPFlow_TypedDataContext1(locations, activityContext, true);
                }
                SimulateSAPPriceITPFlow_TypedDataContext1 refDataContext51 = ((SimulateSAPPriceITPFlow_TypedDataContext1)(cachedCompiledDataContext[0]));
                return refDataContext51.GetLocation<string>(refDataContext51.ValueType___Expr51Get, refDataContext51.ValueType___Expr51Set, expressionId, this.rootActivity, activityContext);
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
                SimulateSAPPriceITPFlow_TypedDataContext1 refDataContext0 = new SimulateSAPPriceITPFlow_TypedDataContext1(locations, true);
                return refDataContext0.GetLocation<Advantech.Myadvantech.DataAccess.QuotationMaster>(refDataContext0.ValueType___Expr0Get, refDataContext0.ValueType___Expr0Set);
            }
            if ((expressionId == 1)) {
                SimulateSAPPriceITPFlow_TypedDataContext1 refDataContext1 = new SimulateSAPPriceITPFlow_TypedDataContext1(locations, true);
                return refDataContext1.GetLocation<string>(refDataContext1.ValueType___Expr1Get, refDataContext1.ValueType___Expr1Set);
            }
            if ((expressionId == 2)) {
                SimulateSAPPriceITPFlow_TypedDataContext1_ForReadOnly valDataContext2 = new SimulateSAPPriceITPFlow_TypedDataContext1_ForReadOnly(locations, true);
                return valDataContext2.ValueType___Expr2Get();
            }
            if ((expressionId == 3)) {
                SimulateSAPPriceITPFlow_TypedDataContext1 refDataContext3 = new SimulateSAPPriceITPFlow_TypedDataContext1(locations, true);
                return refDataContext3.GetLocation<Advantech.Myadvantech.DataAccess.QuotationMaster>(refDataContext3.ValueType___Expr3Get, refDataContext3.ValueType___Expr3Set);
            }
            if ((expressionId == 4)) {
                SimulateSAPPriceITPFlow_TypedDataContext1 refDataContext4 = new SimulateSAPPriceITPFlow_TypedDataContext1(locations, true);
                return refDataContext4.GetLocation<string>(refDataContext4.ValueType___Expr4Get, refDataContext4.ValueType___Expr4Set);
            }
            if ((expressionId == 5)) {
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext5 = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext5.ValueType___Expr5Get();
            }
            if ((expressionId == 6)) {
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext6 = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext6.ValueType___Expr6Get();
            }
            if ((expressionId == 7)) {
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext7 = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext7.ValueType___Expr7Get();
            }
            if ((expressionId == 8)) {
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext8 = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext8.ValueType___Expr8Get();
            }
            if ((expressionId == 9)) {
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext9 = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext9.ValueType___Expr9Get();
            }
            if ((expressionId == 10)) {
                SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly valDataContext10 = new SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly(locations, true);
                return valDataContext10.ValueType___Expr10Get();
            }
            if ((expressionId == 11)) {
                SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly valDataContext11 = new SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly(locations, true);
                return valDataContext11.ValueType___Expr11Get();
            }
            if ((expressionId == 12)) {
                SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly valDataContext12 = new SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly(locations, true);
                return valDataContext12.ValueType___Expr12Get();
            }
            if ((expressionId == 13)) {
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext13 = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext13.ValueType___Expr13Get();
            }
            if ((expressionId == 14)) {
                SimulateSAPPriceITPFlow_TypedDataContext4_ForReadOnly valDataContext14 = new SimulateSAPPriceITPFlow_TypedDataContext4_ForReadOnly(locations, true);
                return valDataContext14.ValueType___Expr14Get();
            }
            if ((expressionId == 15)) {
                SimulateSAPPriceITPFlow_TypedDataContext4_ForReadOnly valDataContext15 = new SimulateSAPPriceITPFlow_TypedDataContext4_ForReadOnly(locations, true);
                return valDataContext15.ValueType___Expr15Get();
            }
            if ((expressionId == 16)) {
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext16 = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext16.ValueType___Expr16Get();
            }
            if ((expressionId == 17)) {
                SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly valDataContext17 = new SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly(locations, true);
                return valDataContext17.ValueType___Expr17Get();
            }
            if ((expressionId == 18)) {
                SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly valDataContext18 = new SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly(locations, true);
                return valDataContext18.ValueType___Expr18Get();
            }
            if ((expressionId == 19)) {
                SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly valDataContext19 = new SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly(locations, true);
                return valDataContext19.ValueType___Expr19Get();
            }
            if ((expressionId == 20)) {
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext20 = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext20.ValueType___Expr20Get();
            }
            if ((expressionId == 21)) {
                SimulateSAPPriceITPFlow_TypedDataContext6_ForReadOnly valDataContext21 = new SimulateSAPPriceITPFlow_TypedDataContext6_ForReadOnly(locations, true);
                return valDataContext21.ValueType___Expr21Get();
            }
            if ((expressionId == 22)) {
                SimulateSAPPriceITPFlow_TypedDataContext6_ForReadOnly valDataContext22 = new SimulateSAPPriceITPFlow_TypedDataContext6_ForReadOnly(locations, true);
                return valDataContext22.ValueType___Expr22Get();
            }
            if ((expressionId == 23)) {
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext23 = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext23.ValueType___Expr23Get();
            }
            if ((expressionId == 24)) {
                SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly valDataContext24 = new SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly(locations, true);
                return valDataContext24.ValueType___Expr24Get();
            }
            if ((expressionId == 25)) {
                SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly valDataContext25 = new SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly(locations, true);
                return valDataContext25.ValueType___Expr25Get();
            }
            if ((expressionId == 26)) {
                SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly valDataContext26 = new SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly(locations, true);
                return valDataContext26.ValueType___Expr26Get();
            }
            if ((expressionId == 27)) {
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext27 = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext27.ValueType___Expr27Get();
            }
            if ((expressionId == 28)) {
                SimulateSAPPriceITPFlow_TypedDataContext8_ForReadOnly valDataContext28 = new SimulateSAPPriceITPFlow_TypedDataContext8_ForReadOnly(locations, true);
                return valDataContext28.ValueType___Expr28Get();
            }
            if ((expressionId == 29)) {
                SimulateSAPPriceITPFlow_TypedDataContext8_ForReadOnly valDataContext29 = new SimulateSAPPriceITPFlow_TypedDataContext8_ForReadOnly(locations, true);
                return valDataContext29.ValueType___Expr29Get();
            }
            if ((expressionId == 30)) {
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext30 = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext30.ValueType___Expr30Get();
            }
            if ((expressionId == 31)) {
                SimulateSAPPriceITPFlow_TypedDataContext2 refDataContext31 = new SimulateSAPPriceITPFlow_TypedDataContext2(locations, true);
                return refDataContext31.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>>(refDataContext31.ValueType___Expr31Get, refDataContext31.ValueType___Expr31Set);
            }
            if ((expressionId == 32)) {
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext32 = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext32.ValueType___Expr32Get();
            }
            if ((expressionId == 33)) {
                SimulateSAPPriceITPFlow_TypedDataContext2 refDataContext33 = new SimulateSAPPriceITPFlow_TypedDataContext2(locations, true);
                return refDataContext33.GetLocation<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>>(refDataContext33.ValueType___Expr33Get, refDataContext33.ValueType___Expr33Set);
            }
            if ((expressionId == 34)) {
                SimulateSAPPriceITPFlow_TypedDataContext9 refDataContext34 = new SimulateSAPPriceITPFlow_TypedDataContext9(locations, true);
                return refDataContext34.GetLocation<Advantech.Myadvantech.DataAccess.QuotationMaster>(refDataContext34.ValueType___Expr34Get, refDataContext34.ValueType___Expr34Set);
            }
            if ((expressionId == 35)) {
                SimulateSAPPriceITPFlow_TypedDataContext9_ForReadOnly valDataContext35 = new SimulateSAPPriceITPFlow_TypedDataContext9_ForReadOnly(locations, true);
                return valDataContext35.ValueType___Expr35Get();
            }
            if ((expressionId == 36)) {
                SimulateSAPPriceITPFlow_TypedDataContext10_ForReadOnly valDataContext36 = new SimulateSAPPriceITPFlow_TypedDataContext10_ForReadOnly(locations, true);
                return valDataContext36.ValueType___Expr36Get();
            }
            if ((expressionId == 37)) {
                SimulateSAPPriceITPFlow_TypedDataContext10_ForReadOnly valDataContext37 = new SimulateSAPPriceITPFlow_TypedDataContext10_ForReadOnly(locations, true);
                return valDataContext37.ValueType___Expr37Get();
            }
            if ((expressionId == 38)) {
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext38 = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext38.ValueType___Expr38Get();
            }
            if ((expressionId == 39)) {
                SimulateSAPPriceITPFlow_TypedDataContext11_ForReadOnly valDataContext39 = new SimulateSAPPriceITPFlow_TypedDataContext11_ForReadOnly(locations, true);
                return valDataContext39.ValueType___Expr39Get();
            }
            if ((expressionId == 40)) {
                SimulateSAPPriceITPFlow_TypedDataContext11_ForReadOnly valDataContext40 = new SimulateSAPPriceITPFlow_TypedDataContext11_ForReadOnly(locations, true);
                return valDataContext40.ValueType___Expr40Get();
            }
            if ((expressionId == 41)) {
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext41 = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext41.ValueType___Expr41Get();
            }
            if ((expressionId == 42)) {
                SimulateSAPPriceITPFlow_TypedDataContext12_ForReadOnly valDataContext42 = new SimulateSAPPriceITPFlow_TypedDataContext12_ForReadOnly(locations, true);
                return valDataContext42.ValueType___Expr42Get();
            }
            if ((expressionId == 43)) {
                SimulateSAPPriceITPFlow_TypedDataContext12_ForReadOnly valDataContext43 = new SimulateSAPPriceITPFlow_TypedDataContext12_ForReadOnly(locations, true);
                return valDataContext43.ValueType___Expr43Get();
            }
            if ((expressionId == 44)) {
                SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly valDataContext44 = new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locations, true);
                return valDataContext44.ValueType___Expr44Get();
            }
            if ((expressionId == 45)) {
                SimulateSAPPriceITPFlow_TypedDataContext13_ForReadOnly valDataContext45 = new SimulateSAPPriceITPFlow_TypedDataContext13_ForReadOnly(locations, true);
                return valDataContext45.ValueType___Expr45Get();
            }
            if ((expressionId == 46)) {
                SimulateSAPPriceITPFlow_TypedDataContext13_ForReadOnly valDataContext46 = new SimulateSAPPriceITPFlow_TypedDataContext13_ForReadOnly(locations, true);
                return valDataContext46.ValueType___Expr46Get();
            }
            if ((expressionId == 47)) {
                SimulateSAPPriceITPFlow_TypedDataContext1 refDataContext47 = new SimulateSAPPriceITPFlow_TypedDataContext1(locations, true);
                return refDataContext47.GetLocation<Advantech.Myadvantech.DataAccess.QuotationMaster>(refDataContext47.ValueType___Expr47Get, refDataContext47.ValueType___Expr47Set);
            }
            if ((expressionId == 48)) {
                SimulateSAPPriceITPFlow_TypedDataContext1_ForReadOnly valDataContext48 = new SimulateSAPPriceITPFlow_TypedDataContext1_ForReadOnly(locations, true);
                return valDataContext48.ValueType___Expr48Get();
            }
            if ((expressionId == 49)) {
                SimulateSAPPriceITPFlow_TypedDataContext1 refDataContext49 = new SimulateSAPPriceITPFlow_TypedDataContext1(locations, true);
                return refDataContext49.GetLocation<Advantech.Myadvantech.DataAccess.QuotationMaster>(refDataContext49.ValueType___Expr49Get, refDataContext49.ValueType___Expr49Set);
            }
            if ((expressionId == 50)) {
                SimulateSAPPriceITPFlow_TypedDataContext1 refDataContext50 = new SimulateSAPPriceITPFlow_TypedDataContext1(locations, true);
                return refDataContext50.GetLocation<Advantech.Myadvantech.DataAccess.QuotationMaster>(refDataContext50.ValueType___Expr50Get, refDataContext50.ValueType___Expr50Set);
            }
            if ((expressionId == 51)) {
                SimulateSAPPriceITPFlow_TypedDataContext1 refDataContext51 = new SimulateSAPPriceITPFlow_TypedDataContext1(locations, true);
                return refDataContext51.GetLocation<string>(refDataContext51.ValueType___Expr51Get, refDataContext51.ValueType___Expr51Set);
            }
            return null;
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool CanExecuteExpression(string expressionText, bool isReference, System.Collections.Generic.IList<System.Activities.LocationReference> locations, out int expressionId) {
            if (((isReference == true) 
                        && ((expressionText == "QuotationMaster") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext1.Validate(locations, true, 0) == true)))) {
                expressionId = 0;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "ErrorMessage") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext1.Validate(locations, true, 0) == true)))) {
                expressionId = 1;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationMaster.org") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext1_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 2;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "QuotationMaster") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext1.Validate(locations, true, 0) == true)))) {
                expressionId = 3;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "ErrorMessage") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext1.Validate(locations, true, 0) == true)))) {
                expressionId = 4;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "new List<QuotationDetail>()") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 5;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "new List<QuotationDetail>()") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 6;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "new List<QuotationDetail>()") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 7;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "new List<QuotationDetail>()") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 8;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationMaster.QuotationDetail") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 9;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "item.isACNOSParts") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 10;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "OSParts") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 11;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "item") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 12;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "OSParts") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 13;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationMaster.QuotationDetail") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext4_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 14;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "item") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext4_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 15;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationMaster.QuotationDetail") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 16;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "item.isACNLocalNumberParts") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 17;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "LocalNumberParts") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 18;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "item") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 19;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "LocalNumberParts") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 20;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationMaster.QuotationDetail") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext6_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 21;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "item") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext6_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 22;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationMaster.QuotationDetail") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 23;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "item.isACNLocalPTDParts") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 24;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "LocalPTDParts") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 25;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "item") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 26;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "LocalPTDParts") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 27;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationMaster.QuotationDetail") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext8_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 28;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "item") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext8_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 29;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "String.IsNullOrEmpty(QuotationMaster.quoteToErpId) ? QuotationMaster.quoteToErpId" +
                            " : QuotationMaster.DefaultERPID") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 30;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "OSParts") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 31;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "String.IsNullOrEmpty(QuotationMaster.quoteToErpId) ? QuotationMaster.quoteToErpId" +
                            " : QuotationMaster.DefaultERPID") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 32;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "LocalNumberParts") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext2.Validate(locations, true, 0) == true)))) {
                expressionId = 33;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "QuotationMaster") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext9.Validate(locations, true, 0) == true)))) {
                expressionId = 34;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "RemainQuotationDetails") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext9_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 35;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationMaster.QuotationDetail") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext10_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 36;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "item") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext10_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 37;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "OSParts") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 38;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationMaster.QuotationDetail") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext11_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 39;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "item") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext11_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 40;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "LocalNumberParts") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 41;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationMaster.QuotationDetail") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext12_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 42;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "item") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext12_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 43;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "LocalPTDParts") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 44;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationMaster.QuotationDetail") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext13_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 45;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "item") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext13_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 46;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "QuotationMaster") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext1.Validate(locations, true, 0) == true)))) {
                expressionId = 47;
                return true;
            }
            if (((isReference == false) 
                        && ((expressionText == "QuotationMaster.org") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext1_ForReadOnly.Validate(locations, true, 0) == true)))) {
                expressionId = 48;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "QuotationMaster") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext1.Validate(locations, true, 0) == true)))) {
                expressionId = 49;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "QuotationMaster") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext1.Validate(locations, true, 0) == true)))) {
                expressionId = 50;
                return true;
            }
            if (((isReference == true) 
                        && ((expressionText == "ErrorMessage") 
                        && (SimulateSAPPriceITPFlow_TypedDataContext1.Validate(locations, true, 0) == true)))) {
                expressionId = 51;
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
                return new SimulateSAPPriceITPFlow_TypedDataContext1(locationReferences).@__Expr0GetTree();
            }
            if ((expressionId == 1)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext1(locationReferences).@__Expr1GetTree();
            }
            if ((expressionId == 2)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext1_ForReadOnly(locationReferences).@__Expr2GetTree();
            }
            if ((expressionId == 3)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext1(locationReferences).@__Expr3GetTree();
            }
            if ((expressionId == 4)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext1(locationReferences).@__Expr4GetTree();
            }
            if ((expressionId == 5)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr5GetTree();
            }
            if ((expressionId == 6)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr6GetTree();
            }
            if ((expressionId == 7)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr7GetTree();
            }
            if ((expressionId == 8)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr8GetTree();
            }
            if ((expressionId == 9)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr9GetTree();
            }
            if ((expressionId == 10)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly(locationReferences).@__Expr10GetTree();
            }
            if ((expressionId == 11)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly(locationReferences).@__Expr11GetTree();
            }
            if ((expressionId == 12)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly(locationReferences).@__Expr12GetTree();
            }
            if ((expressionId == 13)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr13GetTree();
            }
            if ((expressionId == 14)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext4_ForReadOnly(locationReferences).@__Expr14GetTree();
            }
            if ((expressionId == 15)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext4_ForReadOnly(locationReferences).@__Expr15GetTree();
            }
            if ((expressionId == 16)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr16GetTree();
            }
            if ((expressionId == 17)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly(locationReferences).@__Expr17GetTree();
            }
            if ((expressionId == 18)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly(locationReferences).@__Expr18GetTree();
            }
            if ((expressionId == 19)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly(locationReferences).@__Expr19GetTree();
            }
            if ((expressionId == 20)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr20GetTree();
            }
            if ((expressionId == 21)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext6_ForReadOnly(locationReferences).@__Expr21GetTree();
            }
            if ((expressionId == 22)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext6_ForReadOnly(locationReferences).@__Expr22GetTree();
            }
            if ((expressionId == 23)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr23GetTree();
            }
            if ((expressionId == 24)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly(locationReferences).@__Expr24GetTree();
            }
            if ((expressionId == 25)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly(locationReferences).@__Expr25GetTree();
            }
            if ((expressionId == 26)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly(locationReferences).@__Expr26GetTree();
            }
            if ((expressionId == 27)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr27GetTree();
            }
            if ((expressionId == 28)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext8_ForReadOnly(locationReferences).@__Expr28GetTree();
            }
            if ((expressionId == 29)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext8_ForReadOnly(locationReferences).@__Expr29GetTree();
            }
            if ((expressionId == 30)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr30GetTree();
            }
            if ((expressionId == 31)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext2(locationReferences).@__Expr31GetTree();
            }
            if ((expressionId == 32)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr32GetTree();
            }
            if ((expressionId == 33)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext2(locationReferences).@__Expr33GetTree();
            }
            if ((expressionId == 34)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext9(locationReferences).@__Expr34GetTree();
            }
            if ((expressionId == 35)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext9_ForReadOnly(locationReferences).@__Expr35GetTree();
            }
            if ((expressionId == 36)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext10_ForReadOnly(locationReferences).@__Expr36GetTree();
            }
            if ((expressionId == 37)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext10_ForReadOnly(locationReferences).@__Expr37GetTree();
            }
            if ((expressionId == 38)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr38GetTree();
            }
            if ((expressionId == 39)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext11_ForReadOnly(locationReferences).@__Expr39GetTree();
            }
            if ((expressionId == 40)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext11_ForReadOnly(locationReferences).@__Expr40GetTree();
            }
            if ((expressionId == 41)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr41GetTree();
            }
            if ((expressionId == 42)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext12_ForReadOnly(locationReferences).@__Expr42GetTree();
            }
            if ((expressionId == 43)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext12_ForReadOnly(locationReferences).@__Expr43GetTree();
            }
            if ((expressionId == 44)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(locationReferences).@__Expr44GetTree();
            }
            if ((expressionId == 45)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext13_ForReadOnly(locationReferences).@__Expr45GetTree();
            }
            if ((expressionId == 46)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext13_ForReadOnly(locationReferences).@__Expr46GetTree();
            }
            if ((expressionId == 47)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext1(locationReferences).@__Expr47GetTree();
            }
            if ((expressionId == 48)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext1_ForReadOnly(locationReferences).@__Expr48GetTree();
            }
            if ((expressionId == 49)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext1(locationReferences).@__Expr49GetTree();
            }
            if ((expressionId == 50)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext1(locationReferences).@__Expr50GetTree();
            }
            if ((expressionId == 51)) {
                return new SimulateSAPPriceITPFlow_TypedDataContext1(locationReferences).@__Expr51GetTree();
            }
            return null;
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class SimulateSAPPriceITPFlow_TypedDataContext0 : System.Activities.XamlIntegration.CompiledDataContext {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext0(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext0(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext0(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
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
        private class SimulateSAPPriceITPFlow_TypedDataContext0_ForReadOnly : System.Activities.XamlIntegration.CompiledDataContext {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext0_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext0_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext0_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
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
        private class SimulateSAPPriceITPFlow_TypedDataContext1 : SimulateSAPPriceITPFlow_TypedDataContext0 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext1(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext1(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext1(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
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
            
            protected string ErrorMessage {
                get {
                    return ((string)(this.GetVariableValue((1 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((1 + locationsOffset), value);
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
                
                #line 66 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<Advantech.Myadvantech.DataAccess.QuotationMaster>> expression = () => 
              QuotationMaster;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public Advantech.Myadvantech.DataAccess.QuotationMaster @__Expr0Get() {
                
                #line 66 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
              QuotationMaster;
                
                #line default
                #line hidden
            }
            
            public Advantech.Myadvantech.DataAccess.QuotationMaster ValueType___Expr0Get() {
                this.GetValueTypeValues();
                return this.@__Expr0Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr0Set(Advantech.Myadvantech.DataAccess.QuotationMaster value) {
                
                #line 66 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                
              QuotationMaster = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr0Set(Advantech.Myadvantech.DataAccess.QuotationMaster value) {
                this.GetValueTypeValues();
                this.@__Expr0Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr1GetTree() {
                
                #line 61 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
              ErrorMessage;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr1Get() {
                
                #line 61 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
              ErrorMessage;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr1Get() {
                this.GetValueTypeValues();
                return this.@__Expr1Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr1Set(string value) {
                
                #line 61 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                
              ErrorMessage = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr1Set(string value) {
                this.GetValueTypeValues();
                this.@__Expr1Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr3GetTree() {
                
                #line 520 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<Advantech.Myadvantech.DataAccess.QuotationMaster>> expression = () => 
                    QuotationMaster;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public Advantech.Myadvantech.DataAccess.QuotationMaster @__Expr3Get() {
                
                #line 520 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                    QuotationMaster;
                
                #line default
                #line hidden
            }
            
            public Advantech.Myadvantech.DataAccess.QuotationMaster ValueType___Expr3Get() {
                this.GetValueTypeValues();
                return this.@__Expr3Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr3Set(Advantech.Myadvantech.DataAccess.QuotationMaster value) {
                
                #line 520 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                
                    QuotationMaster = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr3Set(Advantech.Myadvantech.DataAccess.QuotationMaster value) {
                this.GetValueTypeValues();
                this.@__Expr3Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr4GetTree() {
                
                #line 515 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                    ErrorMessage;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr4Get() {
                
                #line 515 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                    ErrorMessage;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr4Get() {
                this.GetValueTypeValues();
                return this.@__Expr4Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr4Set(string value) {
                
                #line 515 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                
                    ErrorMessage = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr4Set(string value) {
                this.GetValueTypeValues();
                this.@__Expr4Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr47GetTree() {
                
                #line 481 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<Advantech.Myadvantech.DataAccess.QuotationMaster>> expression = () => 
                        QuotationMaster;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public Advantech.Myadvantech.DataAccess.QuotationMaster @__Expr47Get() {
                
                #line 481 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                        QuotationMaster;
                
                #line default
                #line hidden
            }
            
            public Advantech.Myadvantech.DataAccess.QuotationMaster ValueType___Expr47Get() {
                this.GetValueTypeValues();
                return this.@__Expr47Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr47Set(Advantech.Myadvantech.DataAccess.QuotationMaster value) {
                
                #line 481 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                
                        QuotationMaster = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr47Set(Advantech.Myadvantech.DataAccess.QuotationMaster value) {
                this.GetValueTypeValues();
                this.@__Expr47Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr49GetTree() {
                
                #line 488 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<Advantech.Myadvantech.DataAccess.QuotationMaster>> expression = () => 
                        QuotationMaster;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public Advantech.Myadvantech.DataAccess.QuotationMaster @__Expr49Get() {
                
                #line 488 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                        QuotationMaster;
                
                #line default
                #line hidden
            }
            
            public Advantech.Myadvantech.DataAccess.QuotationMaster ValueType___Expr49Get() {
                this.GetValueTypeValues();
                return this.@__Expr49Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr49Set(Advantech.Myadvantech.DataAccess.QuotationMaster value) {
                
                #line 488 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                
                        QuotationMaster = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr49Set(Advantech.Myadvantech.DataAccess.QuotationMaster value) {
                this.GetValueTypeValues();
                this.@__Expr49Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr50GetTree() {
                
                #line 505 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<Advantech.Myadvantech.DataAccess.QuotationMaster>> expression = () => 
                    QuotationMaster;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public Advantech.Myadvantech.DataAccess.QuotationMaster @__Expr50Get() {
                
                #line 505 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                    QuotationMaster;
                
                #line default
                #line hidden
            }
            
            public Advantech.Myadvantech.DataAccess.QuotationMaster ValueType___Expr50Get() {
                this.GetValueTypeValues();
                return this.@__Expr50Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr50Set(Advantech.Myadvantech.DataAccess.QuotationMaster value) {
                
                #line 505 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                
                    QuotationMaster = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr50Set(Advantech.Myadvantech.DataAccess.QuotationMaster value) {
                this.GetValueTypeValues();
                this.@__Expr50Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr51GetTree() {
                
                #line 500 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                    ErrorMessage;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr51Get() {
                
                #line 500 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                    ErrorMessage;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr51Get() {
                this.GetValueTypeValues();
                return this.@__Expr51Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr51Set(string value) {
                
                #line 500 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                
                    ErrorMessage = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr51Set(string value) {
                this.GetValueTypeValues();
                this.@__Expr51Set(value);
                this.SetValueTypeValues();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 2))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 2);
                }
                expectedLocationsCount = 2;
                if (((locationReferences[(offset + 0)].Name != "QuotationMaster") 
                            || (locationReferences[(offset + 0)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationMaster)))) {
                    return false;
                }
                if (((locationReferences[(offset + 1)].Name != "ErrorMessage") 
                            || (locationReferences[(offset + 1)].Type != typeof(string)))) {
                    return false;
                }
                return SimulateSAPPriceITPFlow_TypedDataContext0.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class SimulateSAPPriceITPFlow_TypedDataContext1_ForReadOnly : SimulateSAPPriceITPFlow_TypedDataContext0_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext1_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext1_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext1_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationMaster QuotationMaster {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationMaster)(this.GetVariableValue((0 + locationsOffset))));
                }
            }
            
            protected string ErrorMessage {
                get {
                    return ((string)(this.GetVariableValue((1 + locationsOffset))));
                }
            }
            
            internal new static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public new virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
                base.SetLocationsOffset(locationsOffset);
            }
            
            internal System.Linq.Expressions.Expression @__Expr2GetTree() {
                
                #line 73 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
              QuotationMaster.org;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr2Get() {
                
                #line 73 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
              QuotationMaster.org;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr2Get() {
                this.GetValueTypeValues();
                return this.@__Expr2Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr48GetTree() {
                
                #line 476 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                        QuotationMaster.org;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr48Get() {
                
                #line 476 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                        QuotationMaster.org;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr48Get() {
                this.GetValueTypeValues();
                return this.@__Expr48Get();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 2))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 2);
                }
                expectedLocationsCount = 2;
                if (((locationReferences[(offset + 0)].Name != "QuotationMaster") 
                            || (locationReferences[(offset + 0)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationMaster)))) {
                    return false;
                }
                if (((locationReferences[(offset + 1)].Name != "ErrorMessage") 
                            || (locationReferences[(offset + 1)].Type != typeof(string)))) {
                    return false;
                }
                return SimulateSAPPriceITPFlow_TypedDataContext0_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class SimulateSAPPriceITPFlow_TypedDataContext2 : SimulateSAPPriceITPFlow_TypedDataContext1 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext2(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext2(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext2(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> RemainQuotationDetails {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)(this.GetVariableValue((2 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((2 + locationsOffset), value);
                }
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> OSParts {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)(this.GetVariableValue((3 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((3 + locationsOffset), value);
                }
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> LocalPTDParts {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)(this.GetVariableValue((4 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((4 + locationsOffset), value);
                }
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> LocalNumberParts {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)(this.GetVariableValue((5 + locationsOffset))));
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
            
            internal System.Linq.Expressions.Expression @__Expr31GetTree() {
                
                #line 315 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                                OSParts;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr31Get() {
                
                #line 315 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                OSParts;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr31Get() {
                this.GetValueTypeValues();
                return this.@__Expr31Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr31Set(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> value) {
                
                #line 315 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                
                                OSParts = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr31Set(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> value) {
                this.GetValueTypeValues();
                this.@__Expr31Set(value);
                this.SetValueTypeValues();
            }
            
            internal System.Linq.Expressions.Expression @__Expr33GetTree() {
                
                #line 334 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                                LocalNumberParts;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr33Get() {
                
                #line 334 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                LocalNumberParts;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr33Get() {
                this.GetValueTypeValues();
                return this.@__Expr33Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr33Set(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> value) {
                
                #line 334 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                
                                LocalNumberParts = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr33Set(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> value) {
                this.GetValueTypeValues();
                this.@__Expr33Set(value);
                this.SetValueTypeValues();
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
                if (((locationReferences[(offset + 2)].Name != "RemainQuotationDetails") 
                            || (locationReferences[(offset + 2)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)))) {
                    return false;
                }
                if (((locationReferences[(offset + 3)].Name != "OSParts") 
                            || (locationReferences[(offset + 3)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)))) {
                    return false;
                }
                if (((locationReferences[(offset + 4)].Name != "LocalPTDParts") 
                            || (locationReferences[(offset + 4)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)))) {
                    return false;
                }
                if (((locationReferences[(offset + 5)].Name != "LocalNumberParts") 
                            || (locationReferences[(offset + 5)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)))) {
                    return false;
                }
                return SimulateSAPPriceITPFlow_TypedDataContext1.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly : SimulateSAPPriceITPFlow_TypedDataContext1_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> RemainQuotationDetails {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)(this.GetVariableValue((2 + locationsOffset))));
                }
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> OSParts {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)(this.GetVariableValue((3 + locationsOffset))));
                }
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> LocalPTDParts {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)(this.GetVariableValue((4 + locationsOffset))));
                }
            }
            
            protected System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> LocalNumberParts {
                get {
                    return ((System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)(this.GetVariableValue((5 + locationsOffset))));
                }
            }
            
            internal new static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public new virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
                base.SetLocationsOffset(locationsOffset);
            }
            
            internal System.Linq.Expressions.Expression @__Expr5GetTree() {
                
                #line 81 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                        new List<QuotationDetail>();
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr5Get() {
                
                #line 81 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                        new List<QuotationDetail>();
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr5Get() {
                this.GetValueTypeValues();
                return this.@__Expr5Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr6GetTree() {
                
                #line 86 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                        new List<QuotationDetail>();
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr6Get() {
                
                #line 86 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                        new List<QuotationDetail>();
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr6Get() {
                this.GetValueTypeValues();
                return this.@__Expr6Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr7GetTree() {
                
                #line 91 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                        new List<QuotationDetail>();
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr7Get() {
                
                #line 91 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                        new List<QuotationDetail>();
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr7Get() {
                this.GetValueTypeValues();
                return this.@__Expr7Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr8GetTree() {
                
                #line 96 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                        new List<QuotationDetail>();
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr8Get() {
                
                #line 96 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                        new List<QuotationDetail>();
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr8Get() {
                this.GetValueTypeValues();
                return this.@__Expr8Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr9GetTree() {
                
                #line 106 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                              QuotationMaster.QuotationDetail;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr9Get() {
                
                #line 106 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                              QuotationMaster.QuotationDetail;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr9Get() {
                this.GetValueTypeValues();
                return this.@__Expr9Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr13GetTree() {
                
                #line 143 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                                  OSParts;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr13Get() {
                
                #line 143 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                  OSParts;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr13Get() {
                this.GetValueTypeValues();
                return this.@__Expr13Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr16GetTree() {
                
                #line 167 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                                      QuotationMaster.QuotationDetail;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr16Get() {
                
                #line 167 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                      QuotationMaster.QuotationDetail;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr16Get() {
                this.GetValueTypeValues();
                return this.@__Expr16Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr20GetTree() {
                
                #line 204 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                                          LocalNumberParts;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr20Get() {
                
                #line 204 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                          LocalNumberParts;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr20Get() {
                this.GetValueTypeValues();
                return this.@__Expr20Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr23GetTree() {
                
                #line 228 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                                              QuotationMaster.QuotationDetail;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr23Get() {
                
                #line 228 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                              QuotationMaster.QuotationDetail;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr23Get() {
                this.GetValueTypeValues();
                return this.@__Expr23Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr27GetTree() {
                
                #line 265 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                                                  LocalPTDParts;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr27Get() {
                
                #line 265 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                                  LocalPTDParts;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr27Get() {
                this.GetValueTypeValues();
                return this.@__Expr27Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr30GetTree() {
                
                #line 310 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                                String.IsNullOrEmpty(QuotationMaster.quoteToErpId) ? QuotationMaster.quoteToErpId : QuotationMaster.DefaultERPID;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr30Get() {
                
                #line 310 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                String.IsNullOrEmpty(QuotationMaster.quoteToErpId) ? QuotationMaster.quoteToErpId : QuotationMaster.DefaultERPID;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr30Get() {
                this.GetValueTypeValues();
                return this.@__Expr30Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr32GetTree() {
                
                #line 329 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<string>> expression = () => 
                                String.IsNullOrEmpty(QuotationMaster.quoteToErpId) ? QuotationMaster.quoteToErpId : QuotationMaster.DefaultERPID;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public string @__Expr32Get() {
                
                #line 329 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                String.IsNullOrEmpty(QuotationMaster.quoteToErpId) ? QuotationMaster.quoteToErpId : QuotationMaster.DefaultERPID;
                
                #line default
                #line hidden
            }
            
            public string ValueType___Expr32Get() {
                this.GetValueTypeValues();
                return this.@__Expr32Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr38GetTree() {
                
                #line 394 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                              OSParts;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr38Get() {
                
                #line 394 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                              OSParts;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr38Get() {
                this.GetValueTypeValues();
                return this.@__Expr38Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr41GetTree() {
                
                #line 418 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                                  LocalNumberParts;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr41Get() {
                
                #line 418 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                  LocalNumberParts;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr41Get() {
                this.GetValueTypeValues();
                return this.@__Expr41Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr44GetTree() {
                
                #line 442 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                                      LocalPTDParts;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr44Get() {
                
                #line 442 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                      LocalPTDParts;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr44Get() {
                this.GetValueTypeValues();
                return this.@__Expr44Get();
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
                if (((locationReferences[(offset + 2)].Name != "RemainQuotationDetails") 
                            || (locationReferences[(offset + 2)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)))) {
                    return false;
                }
                if (((locationReferences[(offset + 3)].Name != "OSParts") 
                            || (locationReferences[(offset + 3)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)))) {
                    return false;
                }
                if (((locationReferences[(offset + 4)].Name != "LocalPTDParts") 
                            || (locationReferences[(offset + 4)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)))) {
                    return false;
                }
                if (((locationReferences[(offset + 5)].Name != "LocalNumberParts") 
                            || (locationReferences[(offset + 5)].Type != typeof(System.Collections.Generic.List<Advantech.Myadvantech.DataAccess.QuotationDetail>)))) {
                    return false;
                }
                return SimulateSAPPriceITPFlow_TypedDataContext1_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class SimulateSAPPriceITPFlow_TypedDataContext3 : SimulateSAPPriceITPFlow_TypedDataContext2 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext3(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext3(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext3(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationDetail item {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationDetail)(this.GetVariableValue((6 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((6 + locationsOffset), value);
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
                            && (locationReferences.Count < 7))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 7);
                }
                expectedLocationsCount = 7;
                if (((locationReferences[(offset + 6)].Name != "item") 
                            || (locationReferences[(offset + 6)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationDetail)))) {
                    return false;
                }
                return SimulateSAPPriceITPFlow_TypedDataContext2.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly : SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext3_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationDetail item {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationDetail)(this.GetVariableValue((6 + locationsOffset))));
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
                
                #line 117 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<bool>> expression = () => 
                                    item.isACNOSParts;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public bool @__Expr10Get() {
                
                #line 117 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                    item.isACNOSParts;
                
                #line default
                #line hidden
            }
            
            public bool ValueType___Expr10Get() {
                this.GetValueTypeValues();
                return this.@__Expr10Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr11GetTree() {
                
                #line 129 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                                        OSParts;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr11Get() {
                
                #line 129 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                        OSParts;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr11Get() {
                this.GetValueTypeValues();
                return this.@__Expr11Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr12GetTree() {
                
                #line 125 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<Advantech.Myadvantech.DataAccess.QuotationDetail>> expression = () => 
                                          item;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public Advantech.Myadvantech.DataAccess.QuotationDetail @__Expr12Get() {
                
                #line 125 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                          item;
                
                #line default
                #line hidden
            }
            
            public Advantech.Myadvantech.DataAccess.QuotationDetail ValueType___Expr12Get() {
                this.GetValueTypeValues();
                return this.@__Expr12Get();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 7))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 7);
                }
                expectedLocationsCount = 7;
                if (((locationReferences[(offset + 6)].Name != "item") 
                            || (locationReferences[(offset + 6)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationDetail)))) {
                    return false;
                }
                return SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class SimulateSAPPriceITPFlow_TypedDataContext4 : SimulateSAPPriceITPFlow_TypedDataContext2 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext4(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext4(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext4(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationDetail item {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationDetail)(this.GetVariableValue((6 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((6 + locationsOffset), value);
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
                            && (locationReferences.Count < 7))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 7);
                }
                expectedLocationsCount = 7;
                if (((locationReferences[(offset + 6)].Name != "item") 
                            || (locationReferences[(offset + 6)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationDetail)))) {
                    return false;
                }
                return SimulateSAPPriceITPFlow_TypedDataContext2.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class SimulateSAPPriceITPFlow_TypedDataContext4_ForReadOnly : SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext4_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext4_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext4_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationDetail item {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationDetail)(this.GetVariableValue((6 + locationsOffset))));
                }
            }
            
            internal new static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public new virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
                base.SetLocationsOffset(locationsOffset);
            }
            
            internal System.Linq.Expressions.Expression @__Expr14GetTree() {
                
                #line 157 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                                    QuotationMaster.QuotationDetail;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr14Get() {
                
                #line 157 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                    QuotationMaster.QuotationDetail;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr14Get() {
                this.GetValueTypeValues();
                return this.@__Expr14Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr15GetTree() {
                
                #line 153 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<Advantech.Myadvantech.DataAccess.QuotationDetail>> expression = () => 
                                      item;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public Advantech.Myadvantech.DataAccess.QuotationDetail @__Expr15Get() {
                
                #line 153 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                      item;
                
                #line default
                #line hidden
            }
            
            public Advantech.Myadvantech.DataAccess.QuotationDetail ValueType___Expr15Get() {
                this.GetValueTypeValues();
                return this.@__Expr15Get();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 7))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 7);
                }
                expectedLocationsCount = 7;
                if (((locationReferences[(offset + 6)].Name != "item") 
                            || (locationReferences[(offset + 6)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationDetail)))) {
                    return false;
                }
                return SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class SimulateSAPPriceITPFlow_TypedDataContext5 : SimulateSAPPriceITPFlow_TypedDataContext2 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext5(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext5(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext5(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationDetail item {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationDetail)(this.GetVariableValue((6 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((6 + locationsOffset), value);
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
                            && (locationReferences.Count < 7))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 7);
                }
                expectedLocationsCount = 7;
                if (((locationReferences[(offset + 6)].Name != "item") 
                            || (locationReferences[(offset + 6)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationDetail)))) {
                    return false;
                }
                return SimulateSAPPriceITPFlow_TypedDataContext2.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly : SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext5_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationDetail item {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationDetail)(this.GetVariableValue((6 + locationsOffset))));
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
                
                #line 178 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<bool>> expression = () => 
                                            item.isACNLocalNumberParts;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public bool @__Expr17Get() {
                
                #line 178 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                            item.isACNLocalNumberParts;
                
                #line default
                #line hidden
            }
            
            public bool ValueType___Expr17Get() {
                this.GetValueTypeValues();
                return this.@__Expr17Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr18GetTree() {
                
                #line 190 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                                                LocalNumberParts;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr18Get() {
                
                #line 190 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                                LocalNumberParts;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr18Get() {
                this.GetValueTypeValues();
                return this.@__Expr18Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr19GetTree() {
                
                #line 186 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<Advantech.Myadvantech.DataAccess.QuotationDetail>> expression = () => 
                                                  item;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public Advantech.Myadvantech.DataAccess.QuotationDetail @__Expr19Get() {
                
                #line 186 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                                  item;
                
                #line default
                #line hidden
            }
            
            public Advantech.Myadvantech.DataAccess.QuotationDetail ValueType___Expr19Get() {
                this.GetValueTypeValues();
                return this.@__Expr19Get();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 7))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 7);
                }
                expectedLocationsCount = 7;
                if (((locationReferences[(offset + 6)].Name != "item") 
                            || (locationReferences[(offset + 6)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationDetail)))) {
                    return false;
                }
                return SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class SimulateSAPPriceITPFlow_TypedDataContext6 : SimulateSAPPriceITPFlow_TypedDataContext2 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext6(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext6(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext6(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationDetail item {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationDetail)(this.GetVariableValue((6 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((6 + locationsOffset), value);
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
                            && (locationReferences.Count < 7))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 7);
                }
                expectedLocationsCount = 7;
                if (((locationReferences[(offset + 6)].Name != "item") 
                            || (locationReferences[(offset + 6)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationDetail)))) {
                    return false;
                }
                return SimulateSAPPriceITPFlow_TypedDataContext2.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class SimulateSAPPriceITPFlow_TypedDataContext6_ForReadOnly : SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext6_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext6_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext6_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationDetail item {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationDetail)(this.GetVariableValue((6 + locationsOffset))));
                }
            }
            
            internal new static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public new virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
                base.SetLocationsOffset(locationsOffset);
            }
            
            internal System.Linq.Expressions.Expression @__Expr21GetTree() {
                
                #line 218 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                                            QuotationMaster.QuotationDetail;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr21Get() {
                
                #line 218 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                            QuotationMaster.QuotationDetail;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr21Get() {
                this.GetValueTypeValues();
                return this.@__Expr21Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr22GetTree() {
                
                #line 214 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<Advantech.Myadvantech.DataAccess.QuotationDetail>> expression = () => 
                                              item;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public Advantech.Myadvantech.DataAccess.QuotationDetail @__Expr22Get() {
                
                #line 214 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                              item;
                
                #line default
                #line hidden
            }
            
            public Advantech.Myadvantech.DataAccess.QuotationDetail ValueType___Expr22Get() {
                this.GetValueTypeValues();
                return this.@__Expr22Get();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 7))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 7);
                }
                expectedLocationsCount = 7;
                if (((locationReferences[(offset + 6)].Name != "item") 
                            || (locationReferences[(offset + 6)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationDetail)))) {
                    return false;
                }
                return SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class SimulateSAPPriceITPFlow_TypedDataContext7 : SimulateSAPPriceITPFlow_TypedDataContext2 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext7(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext7(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext7(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationDetail item {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationDetail)(this.GetVariableValue((6 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((6 + locationsOffset), value);
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
                            && (locationReferences.Count < 7))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 7);
                }
                expectedLocationsCount = 7;
                if (((locationReferences[(offset + 6)].Name != "item") 
                            || (locationReferences[(offset + 6)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationDetail)))) {
                    return false;
                }
                return SimulateSAPPriceITPFlow_TypedDataContext2.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly : SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext7_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationDetail item {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationDetail)(this.GetVariableValue((6 + locationsOffset))));
                }
            }
            
            internal new static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public new virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
                base.SetLocationsOffset(locationsOffset);
            }
            
            internal System.Linq.Expressions.Expression @__Expr24GetTree() {
                
                #line 239 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<bool>> expression = () => 
                                                    item.isACNLocalPTDParts;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public bool @__Expr24Get() {
                
                #line 239 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                                    item.isACNLocalPTDParts;
                
                #line default
                #line hidden
            }
            
            public bool ValueType___Expr24Get() {
                this.GetValueTypeValues();
                return this.@__Expr24Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr25GetTree() {
                
                #line 251 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                                                        LocalPTDParts;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr25Get() {
                
                #line 251 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                                        LocalPTDParts;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr25Get() {
                this.GetValueTypeValues();
                return this.@__Expr25Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr26GetTree() {
                
                #line 247 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<Advantech.Myadvantech.DataAccess.QuotationDetail>> expression = () => 
                                                          item;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public Advantech.Myadvantech.DataAccess.QuotationDetail @__Expr26Get() {
                
                #line 247 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                                          item;
                
                #line default
                #line hidden
            }
            
            public Advantech.Myadvantech.DataAccess.QuotationDetail ValueType___Expr26Get() {
                this.GetValueTypeValues();
                return this.@__Expr26Get();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 7))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 7);
                }
                expectedLocationsCount = 7;
                if (((locationReferences[(offset + 6)].Name != "item") 
                            || (locationReferences[(offset + 6)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationDetail)))) {
                    return false;
                }
                return SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class SimulateSAPPriceITPFlow_TypedDataContext8 : SimulateSAPPriceITPFlow_TypedDataContext2 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext8(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext8(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext8(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationDetail item {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationDetail)(this.GetVariableValue((6 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((6 + locationsOffset), value);
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
                            && (locationReferences.Count < 7))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 7);
                }
                expectedLocationsCount = 7;
                if (((locationReferences[(offset + 6)].Name != "item") 
                            || (locationReferences[(offset + 6)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationDetail)))) {
                    return false;
                }
                return SimulateSAPPriceITPFlow_TypedDataContext2.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class SimulateSAPPriceITPFlow_TypedDataContext8_ForReadOnly : SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext8_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext8_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext8_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationDetail item {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationDetail)(this.GetVariableValue((6 + locationsOffset))));
                }
            }
            
            internal new static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public new virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
                base.SetLocationsOffset(locationsOffset);
            }
            
            internal System.Linq.Expressions.Expression @__Expr28GetTree() {
                
                #line 279 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                                                    QuotationMaster.QuotationDetail;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr28Get() {
                
                #line 279 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                                    QuotationMaster.QuotationDetail;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr28Get() {
                this.GetValueTypeValues();
                return this.@__Expr28Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr29GetTree() {
                
                #line 275 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<Advantech.Myadvantech.DataAccess.QuotationDetail>> expression = () => 
                                                      item;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public Advantech.Myadvantech.DataAccess.QuotationDetail @__Expr29Get() {
                
                #line 275 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                                      item;
                
                #line default
                #line hidden
            }
            
            public Advantech.Myadvantech.DataAccess.QuotationDetail ValueType___Expr29Get() {
                this.GetValueTypeValues();
                return this.@__Expr29Get();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 7))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 7);
                }
                expectedLocationsCount = 7;
                if (((locationReferences[(offset + 6)].Name != "item") 
                            || (locationReferences[(offset + 6)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationDetail)))) {
                    return false;
                }
                return SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class SimulateSAPPriceITPFlow_TypedDataContext9 : SimulateSAPPriceITPFlow_TypedDataContext2 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext9(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext9(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext9(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationMaster TempQuoteMaster {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationMaster)(this.GetVariableValue((6 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((6 + locationsOffset), value);
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
                
                #line 352 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<Advantech.Myadvantech.DataAccess.QuotationMaster>> expression = () => 
                                QuotationMaster;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public Advantech.Myadvantech.DataAccess.QuotationMaster @__Expr34Get() {
                
                #line 352 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                QuotationMaster;
                
                #line default
                #line hidden
            }
            
            public Advantech.Myadvantech.DataAccess.QuotationMaster ValueType___Expr34Get() {
                this.GetValueTypeValues();
                return this.@__Expr34Get();
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public void @__Expr34Set(Advantech.Myadvantech.DataAccess.QuotationMaster value) {
                
                #line 352 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                
                                QuotationMaster = value;
                
                #line default
                #line hidden
            }
            
            public void ValueType___Expr34Set(Advantech.Myadvantech.DataAccess.QuotationMaster value) {
                this.GetValueTypeValues();
                this.@__Expr34Set(value);
                this.SetValueTypeValues();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 7))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 7);
                }
                expectedLocationsCount = 7;
                if (((locationReferences[(offset + 6)].Name != "TempQuoteMaster") 
                            || (locationReferences[(offset + 6)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationMaster)))) {
                    return false;
                }
                return SimulateSAPPriceITPFlow_TypedDataContext2.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class SimulateSAPPriceITPFlow_TypedDataContext9_ForReadOnly : SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext9_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext9_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext9_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationMaster TempQuoteMaster {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationMaster)(this.GetVariableValue((6 + locationsOffset))));
                }
            }
            
            internal new static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public new virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
                base.SetLocationsOffset(locationsOffset);
            }
            
            internal System.Linq.Expressions.Expression @__Expr35GetTree() {
                
                #line 361 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                                    RemainQuotationDetails;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr35Get() {
                
                #line 361 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                    RemainQuotationDetails;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.IEnumerable<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr35Get() {
                this.GetValueTypeValues();
                return this.@__Expr35Get();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 7))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 7);
                }
                expectedLocationsCount = 7;
                if (((locationReferences[(offset + 6)].Name != "TempQuoteMaster") 
                            || (locationReferences[(offset + 6)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationMaster)))) {
                    return false;
                }
                return SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class SimulateSAPPriceITPFlow_TypedDataContext10 : SimulateSAPPriceITPFlow_TypedDataContext9 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext10(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext10(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext10(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationDetail item {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationDetail)(this.GetVariableValue((7 + locationsOffset))));
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
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 8))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 8);
                }
                expectedLocationsCount = 8;
                if (((locationReferences[(offset + 7)].Name != "item") 
                            || (locationReferences[(offset + 7)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationDetail)))) {
                    return false;
                }
                return SimulateSAPPriceITPFlow_TypedDataContext9.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class SimulateSAPPriceITPFlow_TypedDataContext10_ForReadOnly : SimulateSAPPriceITPFlow_TypedDataContext9_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext10_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext10_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext10_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationDetail item {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationDetail)(this.GetVariableValue((7 + locationsOffset))));
                }
            }
            
            internal new static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public new virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
                base.SetLocationsOffset(locationsOffset);
            }
            
            internal System.Linq.Expressions.Expression @__Expr36GetTree() {
                
                #line 375 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                                      QuotationMaster.QuotationDetail;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr36Get() {
                
                #line 375 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                      QuotationMaster.QuotationDetail;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr36Get() {
                this.GetValueTypeValues();
                return this.@__Expr36Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr37GetTree() {
                
                #line 371 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<Advantech.Myadvantech.DataAccess.QuotationDetail>> expression = () => 
                                        item;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public Advantech.Myadvantech.DataAccess.QuotationDetail @__Expr37Get() {
                
                #line 371 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                        item;
                
                #line default
                #line hidden
            }
            
            public Advantech.Myadvantech.DataAccess.QuotationDetail ValueType___Expr37Get() {
                this.GetValueTypeValues();
                return this.@__Expr37Get();
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
                if (((locationReferences[(offset + 7)].Name != "item") 
                            || (locationReferences[(offset + 7)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationDetail)))) {
                    return false;
                }
                return SimulateSAPPriceITPFlow_TypedDataContext9_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class SimulateSAPPriceITPFlow_TypedDataContext11 : SimulateSAPPriceITPFlow_TypedDataContext2 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext11(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext11(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext11(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationDetail item {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationDetail)(this.GetVariableValue((6 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((6 + locationsOffset), value);
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
                            && (locationReferences.Count < 7))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 7);
                }
                expectedLocationsCount = 7;
                if (((locationReferences[(offset + 6)].Name != "item") 
                            || (locationReferences[(offset + 6)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationDetail)))) {
                    return false;
                }
                return SimulateSAPPriceITPFlow_TypedDataContext2.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class SimulateSAPPriceITPFlow_TypedDataContext11_ForReadOnly : SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext11_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext11_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext11_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationDetail item {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationDetail)(this.GetVariableValue((6 + locationsOffset))));
                }
            }
            
            internal new static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public new virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
                base.SetLocationsOffset(locationsOffset);
            }
            
            internal System.Linq.Expressions.Expression @__Expr39GetTree() {
                
                #line 408 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                                QuotationMaster.QuotationDetail;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr39Get() {
                
                #line 408 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                QuotationMaster.QuotationDetail;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr39Get() {
                this.GetValueTypeValues();
                return this.@__Expr39Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr40GetTree() {
                
                #line 404 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<Advantech.Myadvantech.DataAccess.QuotationDetail>> expression = () => 
                                  item;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public Advantech.Myadvantech.DataAccess.QuotationDetail @__Expr40Get() {
                
                #line 404 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                  item;
                
                #line default
                #line hidden
            }
            
            public Advantech.Myadvantech.DataAccess.QuotationDetail ValueType___Expr40Get() {
                this.GetValueTypeValues();
                return this.@__Expr40Get();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 7))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 7);
                }
                expectedLocationsCount = 7;
                if (((locationReferences[(offset + 6)].Name != "item") 
                            || (locationReferences[(offset + 6)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationDetail)))) {
                    return false;
                }
                return SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class SimulateSAPPriceITPFlow_TypedDataContext12 : SimulateSAPPriceITPFlow_TypedDataContext2 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext12(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext12(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext12(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationDetail item {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationDetail)(this.GetVariableValue((6 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((6 + locationsOffset), value);
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
                            && (locationReferences.Count < 7))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 7);
                }
                expectedLocationsCount = 7;
                if (((locationReferences[(offset + 6)].Name != "item") 
                            || (locationReferences[(offset + 6)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationDetail)))) {
                    return false;
                }
                return SimulateSAPPriceITPFlow_TypedDataContext2.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class SimulateSAPPriceITPFlow_TypedDataContext12_ForReadOnly : SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext12_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext12_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext12_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationDetail item {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationDetail)(this.GetVariableValue((6 + locationsOffset))));
                }
            }
            
            internal new static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public new virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
                base.SetLocationsOffset(locationsOffset);
            }
            
            internal System.Linq.Expressions.Expression @__Expr42GetTree() {
                
                #line 432 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                                    QuotationMaster.QuotationDetail;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr42Get() {
                
                #line 432 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                    QuotationMaster.QuotationDetail;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr42Get() {
                this.GetValueTypeValues();
                return this.@__Expr42Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr43GetTree() {
                
                #line 428 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<Advantech.Myadvantech.DataAccess.QuotationDetail>> expression = () => 
                                      item;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public Advantech.Myadvantech.DataAccess.QuotationDetail @__Expr43Get() {
                
                #line 428 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                      item;
                
                #line default
                #line hidden
            }
            
            public Advantech.Myadvantech.DataAccess.QuotationDetail ValueType___Expr43Get() {
                this.GetValueTypeValues();
                return this.@__Expr43Get();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 7))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 7);
                }
                expectedLocationsCount = 7;
                if (((locationReferences[(offset + 6)].Name != "item") 
                            || (locationReferences[(offset + 6)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationDetail)))) {
                    return false;
                }
                return SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class SimulateSAPPriceITPFlow_TypedDataContext13 : SimulateSAPPriceITPFlow_TypedDataContext2 {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext13(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext13(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext13(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationDetail item {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationDetail)(this.GetVariableValue((6 + locationsOffset))));
                }
                set {
                    this.SetVariableValue((6 + locationsOffset), value);
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
                            && (locationReferences.Count < 7))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 7);
                }
                expectedLocationsCount = 7;
                if (((locationReferences[(offset + 6)].Name != "item") 
                            || (locationReferences[(offset + 6)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationDetail)))) {
                    return false;
                }
                return SimulateSAPPriceITPFlow_TypedDataContext2.Validate(locationReferences, false, offset);
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0")]
        [System.ComponentModel.BrowsableAttribute(false)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private class SimulateSAPPriceITPFlow_TypedDataContext13_ForReadOnly : SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly {
            
            private int locationsOffset;
            
            private static int expectedLocationsCount;
            
            public SimulateSAPPriceITPFlow_TypedDataContext13_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locations, System.Activities.ActivityContext activityContext, bool computelocationsOffset) : 
                    base(locations, activityContext, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext13_ForReadOnly(System.Collections.Generic.IList<System.Activities.Location> locations, bool computelocationsOffset) : 
                    base(locations, false) {
                if ((computelocationsOffset == true)) {
                    this.SetLocationsOffset((locations.Count - expectedLocationsCount));
                }
            }
            
            public SimulateSAPPriceITPFlow_TypedDataContext13_ForReadOnly(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences) : 
                    base(locationReferences) {
            }
            
            protected Advantech.Myadvantech.DataAccess.QuotationDetail item {
                get {
                    return ((Advantech.Myadvantech.DataAccess.QuotationDetail)(this.GetVariableValue((6 + locationsOffset))));
                }
            }
            
            internal new static System.Activities.XamlIntegration.CompiledDataContext[] GetCompiledDataContextCacheHelper(object dataContextActivities, System.Activities.ActivityContext activityContext, System.Activities.Activity compiledRoot, bool forImplementation, int compiledDataContextCount) {
                return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount);
            }
            
            public new virtual void SetLocationsOffset(int locationsOffsetValue) {
                locationsOffset = locationsOffsetValue;
                base.SetLocationsOffset(locationsOffset);
            }
            
            internal System.Linq.Expressions.Expression @__Expr45GetTree() {
                
                #line 456 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail>>> expression = () => 
                                        QuotationMaster.QuotationDetail;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail> @__Expr45Get() {
                
                #line 456 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                        QuotationMaster.QuotationDetail;
                
                #line default
                #line hidden
            }
            
            public System.Collections.Generic.ICollection<Advantech.Myadvantech.DataAccess.QuotationDetail> ValueType___Expr45Get() {
                this.GetValueTypeValues();
                return this.@__Expr45Get();
            }
            
            internal System.Linq.Expressions.Expression @__Expr46GetTree() {
                
                #line 452 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                System.Linq.Expressions.Expression<System.Func<Advantech.Myadvantech.DataAccess.QuotationDetail>> expression = () => 
                                          item;
                
                #line default
                #line hidden
                return base.RewriteExpressionTree(expression);
            }
            
            [System.Diagnostics.DebuggerHiddenAttribute()]
            public Advantech.Myadvantech.DataAccess.QuotationDetail @__Expr46Get() {
                
                #line 452 "D:\GIT\GITHUB\EQUOTATION\MYADVANTECHAPI\WORKFLOWLAPI\SIMULATEPRICE\SIMULATESAPPRICEITPFLOW.XAML"
                return 
                                          item;
                
                #line default
                #line hidden
            }
            
            public Advantech.Myadvantech.DataAccess.QuotationDetail ValueType___Expr46Get() {
                this.GetValueTypeValues();
                return this.@__Expr46Get();
            }
            
            public new static bool Validate(System.Collections.Generic.IList<System.Activities.LocationReference> locationReferences, bool validateLocationCount, int offset) {
                if (((validateLocationCount == true) 
                            && (locationReferences.Count < 7))) {
                    return false;
                }
                if ((validateLocationCount == true)) {
                    offset = (locationReferences.Count - 7);
                }
                expectedLocationsCount = 7;
                if (((locationReferences[(offset + 6)].Name != "item") 
                            || (locationReferences[(offset + 6)].Type != typeof(Advantech.Myadvantech.DataAccess.QuotationDetail)))) {
                    return false;
                }
                return SimulateSAPPriceITPFlow_TypedDataContext2_ForReadOnly.Validate(locationReferences, false, offset);
            }
        }
    }
}
