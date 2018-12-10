using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.Myadvantech.DataAccess
{

    public enum ABRTaxType
    {
        BX10,
        BX13,
        BX23,
        BX40,
        BX41,
        BX72,
        BX82,
        BX94,
        BX95,
        BX96,
        FK00
    }

    public enum CreditControlAreaOptions
    {
        CNC1,
        CNC2,
        CNC3,
        CNC4,
        CN01,
        CN02,
        CN08,
        HK05,
        ID01,
        IN01,
        EU01,
        EU80,
        USC1,
        USC2,
        TW01,
        TW02,
        TW03,
        TW04,
        TW05,
        TW06,
        TW07,
        TW08,
        TW09,
        TW10,
        TW16,
        TW99,
        JP01,
        KR01,
        MY01,
        SG01,
        TL01,
        AU01,
        BR01
    }

    /// <summary>
    /// Nadia 20170824:Sales can select
    /// 1) Nego price only ==> Nego / nego vat / total
    /// 2) List price only ==> List / List vat / total
    /// 3) List & Nego ==> List/ Nego / Nego vat / total
    /// and this value will be saved in quotationmaster.isShowListPrice
    /// </summary>
    public enum AKRQuotingPriceMethod 
    { 
        ListAndNegoPrice = 0,
        ListPriceOnly = 1,
        NegoPriceOnly = 2
    }

    public enum AOnlineRegion
    {
        AUS_AOnline,
        AUS_AOnline_IAG,
        AUS_AOnline_iSystem,
        AUS_AENC,
        AEU,
        ATW_AOnline,
        ATW,
        ACN,
        AJP,
        AKR,
        NA
    }

    public enum SAPCurrency
    {
        CNY,
        EUR,
        GBP,
        JPY,
        KRW,
        MYR,
        SGD,
        TWD,
        USD
    }

    public enum SAPOrderType
    {
        ZOR,
        ZOR2,
        ZORR,
        ZQTC, //ABR quote type
        ZQTI, //ABR quote type
        ZQTR, //ABR quote type
        AG,
        QT
    }

    public enum SAPFreightType
    {
        ZHD1,
        ZHDA
    }

    public enum OrderPartnerType
    {
        SoldTo = 0,
        ShipTo = 1,
        BillTo = 2,
        Employee1 = 3,
        Employee2 = 4,
        Employee3 = 5,
        KeyInPerson = 6,
        EndCoutomer = 7,
        EmployeeResponse = 8
    }
    public enum LineItemType
    {        
        LooseItem = 0 ,
        BTOSParent = -1,
        BTOSChild = 1
   
    }

    public enum PISLiteratureType
    {
        Product_Photo_Main,
        Product_Photo_Main_Thumbnail,
        Product_Photo_Big,
        Product_Photo_Small,
        Product_Datasheet,

    }

    public enum LanguageCode
    {
        en_us, //English
        zh_tw, //Traditional Chinese
        zh_cn, //Simplified Chinese
        ja,    //Japanese
        ko,    //Korean
        de,    //German
        ru,    //Russian
        es,    //Spanish
        eu,    //Basque
        he,    //Hebrew
        hi,    //Hindi
        id,    //Indonesian
        ms,    //Malay
        pt,    //Portuguese
        tr,    //Turkish
        vi,    //Vietnamese
        other  //unknow 
    }

    public enum OrderStatus
    {
        Draft = 0,
        Finish = 1,
        Deleted = 2
    }

    public enum ModelType
    {
        Model,
        Bundle,
        PreconfigSystem
    }
   

    public enum eQApprovalFlowType
    {
        Normal=0,
        GP=1,
        ThirtyDaysExpiration = 2,
        GPandExpiration=12

    }
    public enum QuoteItemType
    {

        BtosParent = -1,
        Part = 0,
        BtosPart = 1
    }
    public enum QuoteApprovalStatus
    {

        Rejected = -1,
        Wait_for_review = 0,
        Approved = 1

    }

    public enum QuoteDocStatus
    {
        All = 0,
        Finish = 1,
        Active = 2
    }

    public enum RiskOrderInputType
    { 
        Quote,
        Cart,
        Order
    }

    public enum Compatibility
    {
        Incompatible = 2,
        Compatible = 1
    }

    public enum AcnProjectCompanyID
    {
        C103379
    }

    public enum BBeStoreOrderStatus
    {
        UnProcess,
        SuccessToSAP,
        FailedToSAP,
        NeedERPID,
        ReadyToSAP,
        ToBeVerifiedShipToAddr
    }

    public enum CCTransactionType
    {
        Authorization,
        Void,
        Capture,
        Refund
    }

    public enum WebSource
    {
        eQuotation,
        Myadvantech
    }
}
