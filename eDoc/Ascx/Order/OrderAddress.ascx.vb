Public Class OrderAddress
    Inherits System.Web.UI.UserControl
  Private _IsCanPick As Boolean = True
    Public Property IsCanPick As Boolean
        Get
            Return _IsCanPick
        End Get
        Set(ByVal value As Boolean)
            Me._IsCanPick = value
            If value = False Then trAttention.Visible = False
        End Set
    End Property
    Private _QM As IBUS.iDocHeaderLine = Nothing
    Public Property QM As IBUS.iDocHeaderLine
        Get
            Return _QM
        End Get
        Set(ByVal value As IBUS.iDocHeaderLine)
            _QM = value
        End Set
    End Property
    Private _editable As Boolean
    ''' <summary>
    ''' this property is only used for new bill addresss setting, set to true
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Editable As Boolean
        Get
            Return _editable
        End Get
        Set(ByVal value As Boolean)
            Me._editable = value
            txtShipToTel.Enabled = value
            txtShipToStreet.Enabled = value
            txtShipToStreet2.Enabled = value
            txtShipToCountry.Enabled = value
            txtShipToZipcode.Enabled = value
            txtShipToState.Enabled = value
            txtShipToCity.Enabled = value
            txtShipToAttention.Enabled = value
            trerpid.Visible = Not value
           
        End Set
    End Property

    Private _type As String
    Public Property Type As String
        Get
            Return _type
        End Get
        Set(ByVal value As String)
            Me._type = value
        End Set
    End Property
    Protected Sub btnShipPick_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'If ucShipTo.Visible Then
        '    Me.ucShipTo.GetData()
        'Else
        Dim WhereStr As String = ""
        If Me.Type = "B" Then
            WhereStr = "'Z001','Z003'"
        ElseIf Me.Type = "S" Then
            WhereStr = "'Z001','Z002'"
        End If
        Me.ucShipToUS.GetData(WhereStr, Me.Type)
        'End If
        Me.up_shipto_c.Update() : Me.MP_shipto.Show()
    End Sub

    Public Property ERPID As String
        Get
            Return txtShipTo.Text
        End Get
        Set(ByVal value As String)
            txtShipTo.Text = value
        End Set
    End Property
    Public Property Name As String
        Get
            Return txtShipToName.Text
        End Get
        Set(ByVal value As String)
            txtShipToName.Text = value
        End Set
    End Property

    Public Property Attention As String
        Get
            Return txtShipToAttention.Text
        End Get
        Set(ByVal value As String)
            txtShipToAttention.Text = value
        End Set
    End Property

    Public Property Tel As String
        Get
            Return txtShipToTel.Text
        End Get
        Set(ByVal value As String)
            txtShipToTel.Text = value
        End Set
    End Property
    Public Property Street As String
        Get
            Return txtShipToStreet.Text
        End Get
        Set(ByVal value As String)
            txtShipToStreet.Text = value
        End Set
    End Property
    Public Property Street2 As String
        Get
            Return txtShipToStreet2.Text
        End Get
        Set(ByVal value As String)
            txtShipToStreet2.Text = value
        End Set
    End Property
    Public Property City As String
        Get
            Return txtShipToCity.Text
        End Get
        Set(ByVal value As String)
            txtShipToCity.Text = value
        End Set
    End Property
    Public Property State As String
        Get
            Return txtShipToState.Text
        End Get
        Set(ByVal value As String)
            txtShipToState.Text = value
        End Set
    End Property
    Public Property Zipcode As String
        Get
            Return txtShipToZipcode.Text
        End Get
        Set(ByVal value As String)
            txtShipToZipcode.Text = value
        End Set
    End Property
    Public Property Country As String
        Get
            Return txtShipToCountry.Text
        End Get
        Set(ByVal value As String)
            txtShipToCountry.Text = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsNothing(QM) Then
            Me.ucShipToUS.QM = Me.QM
            If Not IsPostBack Then
                If QM.AccErpId.ToString.Equals("ULTR00001", StringComparison.OrdinalIgnoreCase) OrElse _
                   Role.IsUsaUser Then
                    'trErpName.Visible = True
                    trerpid.Visible = True
                    If Me.Type = "S" Then
                        txtShipToName.Enabled = True
                    End If
                    'Else
                    'trErpName.Visible = False
                End If

                If Not IsCanPick Then
                    btnShipPick.Visible = False
                End If
                If Me.Type = "SOLDTO" OrElse Me.Type = "S" Then
                    If String.IsNullOrEmpty(txtShipTo.Text.Trim) Then
                        txtShipTo.Text = QM.AccErpId
                    End If
                End If
                If Me.Type = "S" AndAlso Role.IsInternalUser() Then
                    txtShipToStreet.Enabled = True
                    txtShipToStreet2.Enabled = True
                End If

                If Me.Type = "B" Then
                    txtShipToAttention.Enabled = False
                    If QM.org.ToString.Equals("US01", StringComparison.OrdinalIgnoreCase) Then
                        Dim billto As String = QM.AccErpId
                        If String.IsNullOrEmpty(txtShipTo.Text) AndAlso Not String.IsNullOrEmpty(billto) Then
                            txtShipTo.Text = billto
                        End If
                    End If
                End If
                If Not String.IsNullOrEmpty(txtShipTo.Text.Trim) Then
                    If String.IsNullOrEmpty(txtShipToName.Text.Trim) Then
                        txtShipToName.Text = getCompanyName(txtShipTo.Text.Trim)
                    End If
                    Dim Ptnrdt As SAPDAL.SalesOrder.PartnerAddressesDataTable = SAPDAL.SAPDAL.GetSAPPartnerAddressesTableByKunnr(txtShipTo.Text.Trim)
                    If Ptnrdt.Rows.Count > 0 Then
                        Dim PtnrRow As SAPDAL.SalesOrder.PartnerAddressesRow = Ptnrdt.Rows(0)
                        With PtnrRow
                            If Not IsDBNull(.Name) Then
                                txtShipToName.Text = .Name.ToUpper().Trim
                            End If
                            If String.IsNullOrEmpty(txtShipToAttention.Text.Trim) Then
                                txtShipToAttention.Text = .C_O_Name
                            End If
                            If String.IsNullOrEmpty(txtShipToTel.Text.Trim) Then
                                txtShipToTel.Text = .Tel1_Numbr
                            End If
                            If String.IsNullOrEmpty(txtShipToStreet.Text.Trim) Then
                                txtShipToStreet.Text = .Street
                            End If
                            If String.IsNullOrEmpty(txtShipToStreet2.Text.Trim) Then
                                txtShipToStreet2.Text = .Str_Suppl3
                            End If
                            If String.IsNullOrEmpty(txtShipToCity.Text.Trim) Then
                                txtShipToCity.Text = .City
                            End If
                            If String.IsNullOrEmpty(txtShipToState.Text.Trim) Then
                                txtShipToState.Text = .Region_str
                            End If
                            If String.IsNullOrEmpty(txtShipToZipcode.Text.Trim) Then
                                txtShipToZipcode.Text = .Postl_Cod1
                            End If
                            If String.IsNullOrEmpty(txtShipToCountry.Text.Trim) Then
                                txtShipToCountry.Text = .Country
                            End If
                        End With

                    End If
                End If

            End If
        End If
    End Sub
    Function getCompanyName(ByVal Company_id As String) As String
        Dim CompanyName As Object = tbOPBase.dbExecuteScalar("MY", "select top 1 isnull(company_name,'') as companyname  from SAP_DIMCOMPANY where company_id='" & Company_id & "'")
        If Not IsNothing(CompanyName) Then
            Return CompanyName
        End If
        Return ""
    End Function


End Class