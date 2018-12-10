Public Class QuotationViewOption
    Inherits System.Web.UI.UserControl


    Public Property QuoteID As String
        Get
            Return Me.HFQuoteID.Value
        End Get
        Set(ByVal value As String)
            Me.HFQuoteID.Value = value
            Me.LoadData(Me.HFQuoteID.Value)
        End Set
    End Property

    Public ReadOnly Property NCNR As Boolean
        Get
            Return cbNCNR.Checked
        End Get
    End Property


    Public ReadOnly Property IsChangeTitle As Boolean
        Get
            Return cbChangeQuoteTitle.Checked
        End Get
    End Property


    Public Property PrintOutFormat As EnumSetting.USPrintOutFormat
        Get
            Return Me.RadioButtonList_FormatOptions.SelectedValue
        End Get
        Set(ByVal value As EnumSetting.USPrintOutFormat)
            Me.RadioButtonList_FormatOptions.SelectedValue = value
        End Set
    End Property

    Private _IsUSAENC As Boolean = False
    Private _IsUSAOnlineEC As Boolean = False
    Private _IsIntercon As Boolean = False
    Private _IsAAC As Boolean = False

    Private Sub LoadData(ByVal QuoteID As String)
        Dim QMDA As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(QuoteID, COMM.Fixer.eDocType.EQ)
        Me.RadioButtonList_FormatOptions.SelectedValue = QMDA.PRINTOUT_FORMAT
        Me.rbtnIsVirtualItmeOnly.SelectedValue = QMDA.ISVIRPARTONLY

        _IsUSAOnlineEC = Role.IsAonlineUsa
        _IsUSAENC = Role.IsUsaAenc
        _IsIntercon = Role.IsHQDCSales
        _IsAAC = Role.IsUSAACSales

        If Role.IsEUSales Then
            Me.RadioButtonList_FormatOptions.Visible = False
        End If

        If Role.IsTWAonlineSales Then
            Me.RadioButtonList_FormatOptions.Visible = False
            Me.rbtnIsVirtualItmeOnly.Visible = False
            Me.RadioButtonList_ATWFormatOptions.Visible = True
            Me.RadioButtonList_ATWFormatOptions.SelectedValue = QMDA.PRINTOUT_FORMAT
        ElseIf Role.IsJPAonlineSales Then
            Me.RadioButtonList_FormatOptions.Visible = False
            Me.rbtnIsVirtualItmeOnly.Visible = False
            Me.RadioButtonList_AJPFormatOptions.Visible = True

            Dim QMD As IBUS.iDocHeader = Pivot.NewObjDocHeader
            QMD.Update(Me.HFQuoteID.Value, String.Format("PRINTOUT_FORMAT='{0}'", Me.RadioButtonList_AJPFormatOptions.SelectedValue), COMM.Fixer.eDocType.EQ)
            If Not MyQuoteX.IsHaveBtos(QuoteID) Then
                Me.RadioButtonList_AJPFormatOptions.Items.RemoveAt(1)
            End If
        End If


        'If Role.IsTWAonlineSales Then
        '    RadioButtonList_FormatOptions.Items(2).Enabled = False
        'End If

        'AENC's option control
        'If Not (_IsUSAENC) AndAlso Not (COMM.Util.IsTesting() AndAlso _IsUSAOnlineEC) Then
        If Not (_IsUSAENC) Then
            RadioButtonList_FormatOptions.Items.RemoveAt(6)
        End If

        'AAC's option control
        If Not (_IsAAC AndAlso Pivot.CurrentProfile.SalesOfficeCode.Contains("2100")) Then
            RadioButtonList_FormatOptions.Items.RemoveAt(5)
            RadioButtonList_FormatOptions.Items.RemoveAt(4)
        End If



        'Ryan 20160217 Show NCNR check box only if sales are USA-Aonline sales and is validation return true
        'If Role.IsAonlineUsa() AndAlso COMM.Util.IsTesting() Then
        If _IsUSAOnlineEC Then
            If Advantech.Myadvantech.Business.OrderBusinessLogic.IsRiskOrder(QuoteID, Advantech.Myadvantech.DataAccess.RiskOrderInputType.Quote) Then
                cbNCNR.Checked = True
                cbNCNR.Visible = True
            End If
        End If


        If Role.IsMexicoAonlineSales OrElse Role.IsEUSales Then
            Me.rbtnIsVirtualItmeOnly.Visible = True
        Else
            Me.rbtnIsVirtualItmeOnly.Visible = False
        End If

        If _IsIntercon Then
            Me.cbChangeQuoteTitle.Visible = True
        End If

    End Sub

   
    Private Sub RadioButtonList_FormatOptions_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonList_FormatOptions.SelectedIndexChanged
        Dim QMDA As IBUS.iDocHeader = Pivot.NewObjDocHeader
        QMDA.Update(Me.HFQuoteID.Value, String.Format("PRINTOUT_FORMAT='{0}'", Me.RadioButtonList_FormatOptions.SelectedValue), COMM.Fixer.eDocType.EQ)
    End Sub

    Private Sub rbtnIsVirtualItmeOnly_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnIsVirtualItmeOnly.SelectedIndexChanged
        Dim QMDA As IBUS.iDocHeader = Pivot.NewObjDocHeader
        QMDA.Update(Me.HFQuoteID.Value, String.Format("isvirpartOnly='{0}'", Me.rbtnIsVirtualItmeOnly.SelectedValue), COMM.Fixer.eDocType.EQ)
    End Sub

    Protected Sub RadioButtonList_ATWFormatOptions_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RadioButtonList_ATWFormatOptions.SelectedIndexChanged
        Dim QMDA As IBUS.iDocHeader = Pivot.NewObjDocHeader
        QMDA.Update(Me.HFQuoteID.Value, String.Format("PRINTOUT_FORMAT='{0}'", Me.RadioButtonList_ATWFormatOptions.SelectedValue), COMM.Fixer.eDocType.EQ)

    End Sub

    Protected Sub RadioButtonList_AJPFormatOptions_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RadioButtonList_AJPFormatOptions.SelectedIndexChanged
        Dim QMDA As IBUS.iDocHeader = Pivot.NewObjDocHeader
        QMDA.Update(Me.HFQuoteID.Value, String.Format("PRINTOUT_FORMAT='{0}'", Me.RadioButtonList_AJPFormatOptions.SelectedValue), COMM.Fixer.eDocType.EQ)
    End Sub
End Class