Imports System.Configuration
Public Class CommonLogic
    Public Shared Function NoStandardSensitiveITP(ByVal partNo As String) As Boolean
        If partNo.ToUpper.ToString.StartsWith("CTOS-") Then
            Return True
        End If
        If partNo.ToUpper.ToString.StartsWith("C-CU-") Then
            Return True
        End If
        If partNo.ToUpper.ToString.StartsWith("AGS-") Then
            Return True
        End If
        If partNo.ToUpper.ToString.StartsWith("IMG-") Then
            Return True
        End If

        Return False
    End Function

    'Shared Function getEWItemByMonth(ByVal month As Integer) As String
    '    If IsNumeric(month) AndAlso month > 0 And month.ToString.Length < 3 Or month = 999 Then
    '        If month = 99 Then
    '            Return "AGS-EW-AD"
    '        End If
    '        If month = 999 Then
    '            Return "AGS-EW/DOA-03"
    '        End If
    '        Return "AGS-EW-" & month.ToString("00")
    '    End If
    '    Return ""
    'End Function

    'Shared Function getRateByEWItem(ByVal itemNo As String, ByVal Plant As String) As Double

    '    If Plant.Length = 4 AndAlso Plant.StartsWith("US", StringComparison.OrdinalIgnoreCase) Then
    '        Select Case itemNo.ToUpper.Trim()
    '            Case "AGS-EW-03"
    '                Return 0.03
    '            Case "AGS-EW-06"
    '                Return 0.05
    '            Case "AGS-EW-12"
    '                Return 0.1
    '            Case "AGS-EW-24"
    '                Return 0.15
    '            Case "AGS-EW-36"
    '                Return 0.25
    '        End Select
    '    ElseIf Plant.Length = 4 AndAlso Plant.StartsWith("JP", StringComparison.OrdinalIgnoreCase) Then
    '        Select Case itemNo.ToUpper.Trim()
    '            Case "AGS-EW-03"
    '                Return 0.03
    '            Case "AGS-EW-06"
    '                Return 0.055
    '            Case "AGS-EW-12"
    '                Return 0.1
    '            Case "AGS-EW-24"
    '                Return 0.18
    '            Case "AGS-EW-36"
    '                Return 0.25
    '        End Select
    '    Else
    '        Select Case itemNo.ToUpper.Trim()
    '            Case "AGS-EW-03"
    '                Return 0.0125
    '            Case "AGS-EW-06"
    '                Return 0.025
    '            Case "AGS-EW-09"
    '                Return 0.05
    '            Case "AGS-EW-12"
    '                Return 0.06
    '            Case "AGS-EW-15"
    '                Return 0.07
    '            Case "AGS-EW-21"
    '                Return 0.08
    '            Case "AGS-EW-24"
    '                Return 0.1
    '            Case "AGS-EW-36"
    '                Return 0.15
    '            Case "AGS-EW-AD"
    '                Return 0.05
    '            Case "AGS-EW/DOA-03"
    '                Return 0.01
    '        End Select
    '    End If

    '    Return 0
    'End Function
    'Shared Function isWarrantable(ByVal PN As String) As [Boolean]
    '    'Frank 2012/10/16: If it is BTOS main item, then just return true to let Quotation.ewFlag can be updated by extended warranty months
    '    If String.IsNullOrEmpty(PN) = False AndAlso PN.EndsWith("-BTO", StringComparison.InvariantCultureIgnoreCase) Then
    '        Return True
    '    End If

    '    Dim DT As DataTable = getSAPProductLine(PN)
    '    If DT.Rows.Count = 0 Then Return True
    '    If isSoftware(PN) Then
    '        Return False
    '    ElseIf PN.StartsWith("AGS", StringComparison.OrdinalIgnoreCase) OrElse PN.StartsWith("OPTION", StringComparison.OrdinalIgnoreCase) Then
    '        'assembly or special option
    '        Return False

    '    ElseIf DT.Rows(0).Item("PRODUCT_LINE") IsNot Nothing AndAlso DT.Rows(0).Item("PRODUCT_LINE") = "ASS#" Then
    '        'assembly charges
    '        Return False
    '    Else
    '        Return True
    '    End If
    'End Function

    'Shared Function isSoftware(ByVal PN As String) As [Boolean]

    '    Dim DT As New DataTable
    '    DT = getSAPProductLine(PN)
    '    If DT.Rows.Count = 0 Then Return True

    '    If PN.StartsWith("96SW") OrElse PN.StartsWith("98MQ") OrElse PN.StartsWith("968Q") Then
    '        Return True
    '    ElseIf DT.Rows(0).Item("PRODUCT_LINE") IsNot Nothing AndAlso (DT.Rows(0).Item("PRODUCT_LINE") = "EPCS" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "EDOS" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "WCOM" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "WAUT" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "DAAS") Then
    '        Return True
    '    Else
    '        Return False
    '    End If
    'End Function

    Shared Function getSAPProductLine(ByVal PN As String) As DataTable
        Dim DT As New DataTable
        Dim STR As String = String.Format(" SELECT     PART_NO, MODEL_NO, MATERIAL_GROUP, DIVISION, PRODUCT_HIERARCHY, PRODUCT_GROUP, PRODUCT_DIVISION, PRODUCT_LINE, " & _
       " GENITEMCATGRP, PRODUCT_DESC, ROHS_FLAG, EGROUP, STATUS, EDIVISION, NET_WEIGHT, VOLUME, WEIGHT_UNIT, GROSS_WEIGHT, " & _
       " VOLUME_UNIT, PRODUCT_TYPE, LAST_UPD_DATE, CREATE_DATE, SIZE_DIMENSIONS, GIP_CODE " & _
       " FROM SAP_PRODUCT " & _
       " WHERE     (PART_NO = '{0}')", PN)
        DT = dbUtil.dbGetDataTable("MY", STR)
        Return DT
    End Function

    Shared Function getListPrice(ByVal partno As String, ByVal org As String, ByVal CURR As String) As Decimal
        Dim ListPrice As Decimal = 0, objLp As Object = Nothing
        'Dim cmd As New SqlClient.SqlCommand( _
        '    "select TOP 1 LIST_PRICE from PRODUCT_LIST_PRICE where ORG=@ORG and CURRENCY=@CUR and PART_NO = @PN and LIST_PRICE>=0 order by LIST_PRICE desc", _
        '    New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("EQ").ConnectionString))
        'cmd.Parameters.AddWithValue("ORG", org) : cmd.Parameters.AddWithValue("CUR", CURR) : cmd.Parameters.AddWithValue("PN", partno)
        'cmd.Connection.Open()
        'objLp = cmd.ExecuteScalar()
        'cmd.Connection.Close()

        If objLp IsNot Nothing Then
            Return CType(objLp, Decimal)
        Else
            Dim spd As New SAPDAL
            If org = "US01" Then
                Dim LpDt As DataTable = Nothing
                spd.getSAPPriceByTable(partno, org, "UEPP5001", "USD", LpDt)
                If LpDt IsNot Nothing AndAlso LpDt.Rows.Count > 0 Then
                    Return LpDt.Rows(0).Item("Kzwi1")
                End If
            End If
            If org = "JP01" Then
                Dim LpDt As DataTable = Nothing
                If CURR = "JPY" Then
                    spd.getSAPPriceByTable(partno, org, "JJCBOM", CURR, LpDt)
                ElseIf CURR = "USD" Then
                    spd.getSAPPriceByTable(partno, org, "UUAASC", CURR, LpDt)
                End If
                If LpDt IsNot Nothing AndAlso LpDt.Rows.Count > 0 Then
                    Return LpDt.Rows(0).Item("Kzwi1")
                End If
            End If
            If String.Equals(org, "EU10", StringComparison.CurrentCultureIgnoreCase) Then
                Dim dummyERPID As String = "EDDEVI07"
                If String.Equals(CURR, "GBP", StringComparison.CurrentCultureIgnoreCase) Then
                    dummyERPID = "EKGBEC02"
                ElseIf String.Equals(CURR, "USD", StringComparison.CurrentCultureIgnoreCase) Then
                    dummyERPID = "EFESAL01"
                End If
                Dim LpDt As DataTable = Nothing
                spd.getSAPPriceByTable(partno, org, dummyERPID, CURR, LpDt)
                If LpDt IsNot Nothing AndAlso LpDt.Rows.Count > 0 Then
                    Return LpDt.Rows(0).Item("Kzwi1")
                End If
            End If
        End If
        Return 0
    End Function
    'Shared Function isANAPnBelowGP(ByVal PN As String, ByVal unitPrice As Decimal, ByRef gpPrice As Decimal, ByRef errmsg As String) As Boolean
    '    If Not SAPDAL.isNeedANAGPControl(PN) Then
    '        Return False
    '    End If
    '    Dim ws As New SAPDAL
    '    gpPrice = ws.getPriceByProdLine(PN, ws.getProdPricingGrp(PN), errmsg)

    '    If gpPrice > unitPrice Then
    '        Dim listprice As Decimal = 0
    '        listprice = getListPrice(PN, "US01", "USD")
    '        If listprice < gpPrice Then
    '            gpPrice = listprice
    '        End If
    '    End If

    '    If gpPrice > 0 AndAlso unitPrice < gpPrice Then
    '        Return True
    '    End If
    '    'PPerc = getANAGPPercByPN(PN, div)
    '    'Dim Pcost As Decimal = getCostForANAPn(PN, Plant)
    '    'If (unitPrice - Pcost) < (PPerc * Pcost) Then
    '    '    Return True
    '    'Else
    '    '    Return False
    '    'End If
    '    Return False
    'End Function

End Class
