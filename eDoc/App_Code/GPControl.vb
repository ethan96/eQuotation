Imports Microsoft.VisualBasic

Public Class GPControl

    'Shared Function setData(ByVal Header As struct_GP_Master, ByVal Detail As List(Of struct_GP_Detail)) As String
    '    Dim myGPMaster As New GP_Master("EQ", "GP_Master")
    '    Dim myGPDetail As New GP_Detail("EQ", "GP_Detail")
    '    Dim uid As String = System.Guid.NewGuid.ToString
    '    myGPMaster.Add(uid, Header.accountRowId, Header.accountName, Header.accountErpId, Header.refId, Header.gpType, Header.Requestor, Now.Date)
    '    For Each x As struct_GP_Detail In Detail
    '        myGPDetail.Add(uid, x.lineNo, x.PartNo, x.Price, x.Itp, x.QTY, x.SPRNO)
    '    Next
    '    Return uid
    'End Function

    'Shared Sub ClearDataByRefId(ByVal refId As String)
    '    Dim myGPMaster As New GP_Master("EQ", "GP_Master")
    '    Dim myGPDetail As New GP_Detail("EQ", "GP_Detail")
    '    Dim GPID As String = getGPIdByRefId(refId)
    '    myGPMaster.Delete(String.Format("refId='{0}'", refId))
    '    myGPDetail.Delete(String.Format("GPId='{0}'", GPID))
    'End Sub


    'Shared Function getGPIdByRefId(ByVal refId As String) As String
    '    Dim GPID As String = ""
    '    Dim myGPMaster As New GP_Master("EQ", "GP_Master")
    '    Dim DT As DataTable = myGPMaster.GetDT(String.Format("refId='{0}'", refId), "")
    '    If DT.Rows.Count > 0 Then
    '        GPID = DT.Rows(0).Item("GPID")
    '    End If
    '    Return GPID
    'End Function
    'Shared Function getRefIdByGPID(ByVal GPID As String) As String
    '    Dim refID As String = ""
    '    Dim myGPMaster As New GP_Master("EQ", "GP_Master")
    '    Dim DT As DataTable = myGPMaster.GetDT(String.Format("GPId='{0}'", GPID), "")
    '    If DT.Rows.Count > 0 Then
    '        refID = DT.Rows(0).Item("refID")
    '    End If
    '    Return refID
    'End Function
    Public Shared Function GPDetailValidation(ByVal Detail As List(Of struct_GP_Detail)) As List(Of struct_GP_Detail)

        'Ryan 20160721 Only remove btos items (line no >= 100)
        'Ryan 20160506 Remove partno where its product line in 'EPCS','DIST','ECBS','EDOS','ECMS','ESMS'
        Dim a As New ArrayList
        For Each d As struct_GP_Detail In Detail
            a.Add("'" + d.PartNo + "'")
        Next
        Dim str_partno As String = "(" + String.Join(",", a.ToArray()) + ")"

        Dim sql_str As String = " select PART_NO,PRODUCT_LINE from SAP_PRODUCT " & _
            " where PRODUCT_LINE in ('EPCS','DIST','ECBS','EDOS','ECMS','ESMS') " & _
            " and PART_NO in " + str_partno
        Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", sql_str)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            For i As Integer = Detail.Count - 1 To 0 Step -1
                If Detail(i).lineNo >= 100 Then
                    If dt.Select("PART_NO ='" + Detail(i).PartNo + "'").Length > 0 Then
                        Detail.RemoveAt(i)
                    End If
                End If
            Next
        End If
        'End
        Return Detail

    End Function

    Shared Function getLevelWithOutAGS(ByVal rowid As String, ByVal erpid As String, ByVal Detail As List(Of struct_GP_Detail), Optional ByVal type As SAPDAL.SAPDAL.itpType = SAPDAL.SAPDAL.itpType.EU, Optional ByVal office As String = "", Optional ByVal group As String = "") As Integer
        Dim level As Integer = 0
        Dim totalMargin As Decimal = getMarginWithOutAGS(Detail)

        Dim dt As New DataTable
        getLevelandAppoverList(rowid, erpid, dt, type, office, group)
        'Util.showMessage(getLevelandAppoverList(rowid, erpid, dt))
        If dt.Rows.Count > 0 Then

            'Frank
            If totalMargin = -99999 Then
                'level = dt.Rows.Count : Return level
                level = 0 : Return level
            End If

            For I As Integer = dt.Rows.Count - 1 To 0 Step -1
                If totalMargin <> -99999 And totalMargin < CType(dt.Rows(I).Item("gp_level").ToString, Decimal) Then
                    level = I + 1
                    Exit For
                End If
            Next
        End If
        Return level
    End Function

    Shared Function getLevel(ByVal rowid As String, ByVal erpid As String, ByVal Detail As List(Of struct_GP_Detail), Optional ByVal type As SAPDAL.SAPDAL.itpType = SAPDAL.SAPDAL.itpType.EU, Optional ByVal office As String = "", Optional ByVal group As String = "") As Integer

        'Ryan 20160506 Add items validation
        Detail = GPDetailValidation(Detail)

        'TC 2014/03/24:if GP of standard part is not below threshold, then it checks p-trade’s GP.
        Dim StandardGPLevel As Integer = getLevelWithOutAGS(rowid, erpid, Detail, type, office, group)
        If StandardGPLevel > 0 Then Return StandardGPLevel

        Dim PTradeGPLevel As Integer = getLevelPTD(rowid, erpid, Detail, type, office, group)
        If PTradeGPLevel > 0 Then Return PTradeGPLevel

        Return 0

        'Return Math.Max(getLevelWithOutAGS(rowid, erpid, Detail, type, office, group), getLevelPTD(rowid, erpid, Detail, type, office, group))

    End Function

    Shared Function getMarginWithOutAGS(ByVal Detail As List(Of struct_GP_Detail)) As Decimal

        Dim r As Decimal = -99999
        Dim sumAmt As Decimal = 0, sumITP As Decimal = 0
        For Each x As struct_GP_Detail In Detail
            'If (Not Business.IsPTD(x.PartNo)) _
            '    And SAPDAL.CommonLogic.NoStandardSensitiveITP(x.PartNo) = False Then
            If Not Business.IsPTD(x.PartNo) _
                And Not Advantech.Myadvantech.Business.PartBusinessLogic.IsNonStandardSensitiveITP(x.PartNo) Then
                sumAmt += x.Price * x.QTY
                sumITP += x.Itp * x.QTY
            End If
        Next
        If sumAmt <> 0 Then
            r = (sumAmt - sumITP) / sumAmt
        End If
        Return r
    End Function

    Shared Function getLevelPTD(ByVal rowid As String, ByVal erpid As String, ByVal Detail As List(Of struct_GP_Detail), Optional ByVal type As SAPDAL.SAPDAL.itpType = SAPDAL.SAPDAL.itpType.EU, Optional ByVal office As String = "", Optional ByVal group As String = "") As Integer
        Dim gpptd As Decimal = 0.05
        If type = SAPDAL.SAPDAL.itpType.JP Then
            gpptd = 0.15
        End If
        Dim level As Integer = 0
        Dim margin As Decimal = getMarginPTD(Detail)
        Dim dt As New DataTable
        getLevelandAppoverList(rowid, erpid, dt, type, office, group)
        If dt.Rows.Count > 0 AndAlso (margin <> -99999 And margin < gpptd) Then
            level = 1
        End If
        Return level
    End Function

    Shared Function getMarginPTD(ByVal Detail As List(Of struct_GP_Detail)) As Decimal

        Dim r As Decimal = -99999
        Dim sumAmt As Decimal = 0, sumITP As Decimal = 0
        For Each x As struct_GP_Detail In Detail
            If Business.IsPTD(x.PartNo) Then
                sumAmt += x.Price * x.QTY
                sumITP += x.Itp * x.QTY
            End If
        Next
        If sumAmt <> 0 Then
            r = (sumAmt - sumITP) / sumAmt
        End If
        Return r
    End Function

    Shared Function getTotalMargin(ByVal Detail As List(Of struct_GP_Detail)) As Decimal

        Dim r As Decimal = -99999
        Dim sumAmt As Decimal = 0, sumITP As Decimal = 0
        For Each x As struct_GP_Detail In Detail
            sumAmt += x.Price * x.QTY
            sumITP += x.Itp * x.QTY
        Next
        If sumAmt <> 0 Then
            r = (sumAmt - sumITP) / sumAmt
        End If
        Return r
    End Function
    Shared Function isApprover(ByVal USER As String, ByVal UID As String) As Boolean

        Dim STR As String = String.Format("quote_id= '{0}' and approver='{1}'", UID, USER)
        Dim o As New quotation_approval("EQ", "quotation_approval")
        If o.IsExists(STR) = 1 Then
            Return True
        End If
        Return False
    End Function
    Shared Function getAOnlineGrp() As String
        Return "315"
    End Function
    Shared Function tryValidateOfficeAndGroupFromMaster(ByVal office As String, ByVal group As String) As Boolean
        Return True
    End Function
    Shared Sub getOfficeGroupByCompanyIdOrg(ByVal companyId As String, ByVal org As String, ByRef office As String, ByRef group As String)
        Dim ofic As Object = Nothing
        Dim ogrp As Object = Nothing

        ofic = tbOPBase.dbExecuteScalar("B2B", "SELECT top 1 isnull(SalesOffice,'') from sap_dimcompany where org_ID='" & org & "' AND company_id='" & companyId.ToString & "'")
        ogrp = tbOPBase.dbExecuteScalar("B2B", "SELECT top 1 isnull(SalesGroup,'') from sap_dimcompany where org_ID='" & org & "' AND company_id='" & companyId.ToString & "'")

        If Not IsNothing(ofic) AndAlso ofic.ToString <> "" Then
            office = ofic.ToString
        End If
        If Not IsNothing(ogrp) AndAlso ogrp.ToString <> "" Then
            group = ogrp.ToString
        End If

    End Sub
    Shared Function getLevelandAppoverList(ByVal rowId As String, ByVal erpid As String, ByRef GPLevel_and_Approver As DataTable, Optional ByVal type As SAPDAL.SAPDAL.itpType = SAPDAL.SAPDAL.itpType.EU, Optional ByVal office As String = "", Optional ByVal group As String = "") As String
        Dim Mtable As String = "gpblock_logic"
        Dim Company_id As String = erpid
        Dim org As String = "EU10"
        Dim isJP As Boolean = False

        If type = SAPDAL.SAPDAL.itpType.JP Then ' Nada:to make sure only JP type can pass in parameters office and group from outside
            isJP = True
            org = "JP01"
        Else
            If office <> "3410" Then
                office = "" : group = ""
            End If
        End If

        'Main Process, divided into two parts (with ERPID and without ERPID)
        If Company_id <> "" Then 'Nomal Case
            If office = "" OrElse group = "" Then
                getOfficeGroupByCompanyIdOrg(Company_id, org, office, group)
                If isJP Then
                    If office.ToString.Trim = "" OrElse group.ToString.Trim = "" Then
                        office = "9998"
                        group = "995"
                    End If
                End If
            End If
        Else 'define office and group if no erpid
            If isJP Then
                office = "9998"
                group = "995"
            ElseIf office = "3410" AndAlso Not office = "" AndAlso Not group = "" Then 'B+B with complete data, no need to do anything
                office = office
                group = group
            Else
                'office = "9999"
                'Nada for GP control discriminated by Office Name .. account Without ERP ID 
                Dim aptSiebAccount As New SiebelDSTableAdapters.SIEBEL_ACCOUNTTableAdapter
                Dim dtAccount As SiebelDS.SIEBEL_ACCOUNTDataTable = aptSiebAccount.GetAccountByRowId(rowId)
                If dtAccount.Count > 0 Then
                    'JJ 2014/2/13：Company_id如果空白時需要Office和Sales Group因為gpblock_logic這個table需要知道office和sales Group才能知道要appove給誰
                    '由Sales的Emal去SIEBEL_POSITION的PRIMARY_POSITION_NAME查出字串如：AFR/Embedded/AOnline-eSales-SEU-Agnes_Iglesias，PRIMARY_POSITION_NAME 鎖定Embedded或iA
                    '再由字串split(1)分出來Sales Group是EC Aonline(Embedded是EC Aonline、iA是iA Aonline)
                    '由SIEBEL_POSITION  join SIEBEL_CONTACT，再由SIEBEL_CONTACT的OrgID就能知道office
                    '如果查不到就由317(EC Aonline)的Fei.Khong@advantech.com和Ween.Niu@advantech.com來appove
                    Dim UserEmail As String = HttpContext.Current.User.Identity.Name
                    'Dim sql_str As String = "SELECT TOP 1 a.PRIMARY_POSITION_NAME + ',' + b.OrgID FROM SIEBEL_POSITION a " &
                    '    " left join SIEBEL_CONTACT b on a.CONTACT_ID = b.ROW_ID WHERE a.EMAIL_ADDR ='" & UserEmail & "' and " &
                    '    " (a.PRIMARY_POSITION_NAME like '%Embedded%' or a.PRIMARY_POSITION_NAME like '%iA%' " &
                    '    " or a.PRIMARY_POSITION_NAME like '%iHospital%' or a.PRIMARY_POSITION_NAME like '%iRetail%' " &
                    '    " or a.PRIMARY_POSITION_NAME like '%iService%' or a.PRIMARY_POSITION_NAME like '%iSys%' or a.PRIMARY_POSITION_NAME like '%NCG%') "

                    'Dim sql_str As String = "select TOP 1 b.DIVISION + ',' + a.OrgID from SIEBEL_CONTACT a " &
                    '    "left join SIEBEL_SALES_HIERARCHY b on a.OwnerId=b.POSITION_ID where a.EMAIL_ADDRESS like '" & UserEmail & "' order by a.CREATED desc"

                    Dim sql_str As String = "select top 1  b.DIVISION + ',' + a.BU_NAME as DIVISION_BU, a.PRIMARY_POSITION_NAME, a.POSITION_TYPE from SIEBEL_POSITION a " &
                         "inner join SIEBEL_DIV_HIERARCHY b on a.PRIMARY_POSITION_DIVISION_ROW_ID=b.ROW_ID where a.EMAIL_ADDR ='" & UserEmail & "' order by a.CREATED desc"


                    'Dim U As New Object
                    'U = tbOPBase.dbExecuteScalar("MY", sql_str)

                    Dim _Division As String = ""
                    Dim _PositionName As String = ""
                    Dim _PositionType As String = ""
                    Dim _dt1 As DataTable = tbOPBase.dbGetDataTable("MY", sql_str)

                    If _dt1 IsNot Nothing AndAlso _dt1.Rows.Count > 0 Then
                        _Division = _dt1.Rows(0).Item("DIVISION_BU") & ""
                        _PositionName = _dt1.Rows(0).Item("PRIMARY_POSITION_NAME") & ""
                        _PositionType = _dt1.Rows(0).Item("POSITION_TYPE") & ""
                    End If

                    '例：ABN/iA/AOnline-Manager-NEU-Wenyu_Lai,AUK
                    If Not IsNothing(_Division) AndAlso _Division.ToString <> "" Then
                        Dim sOffice As String = _Division.ToString.Split(",")(1) '例：AUK
                        Dim sGroup As String = _Division.ToString.Split(",")(0)
                        If _Division.ToString.Contains("/") Then
                            sGroup = _Division.ToString.Split(",")(0).Split("/")(1) '例：iA
                        End If

                        'Frank 20180109
                        'ANR is the region name from Siebel and needs to be converted to "Nordic" named in SAP
                        If sOffice.Equals("ANR", StringComparison.InvariantCultureIgnoreCase) Then
                            sOffice = "Nordic"
                        End If

                        'ICC 2016/2/22 + Ryan 2016/03/17 改寫歐洲報價單，遇到沒有 ERP ID 的簽核流程
                        '因為 gpblock_logic 的 group name 時常會有變動，所以 Position 的 group 不一定可以對應，所以省去之前的作法，直接判定 group
                        '只要 Siebel_Position 的 sGroup，有相關關鍵字，就將 group 設成其對應的編號 並新增例外判斷
                        If sGroup.IndexOf("IA", StringComparison.OrdinalIgnoreCase) > -1 Then
                            group = "312" 'IA group
                            'If _Division.ToString.Contains("AOnline") Then
                            '    group = "315" 'IA Aonline group
                            'End If
                            If _PositionType.IndexOf("DMF", StringComparison.InvariantCultureIgnoreCase) > -1 _
                                OrElse _PositionName.IndexOf("AOnline", StringComparison.InvariantCultureIgnoreCase) > -1 Then
                                group = "315"
                            End If
                        ElseIf sGroup.IndexOf("iHospital", StringComparison.OrdinalIgnoreCase) > -1 Then
                            group = "314" 'iHospital
                        ElseIf sGroup.IndexOf("iService", StringComparison.OrdinalIgnoreCase) > -1 Then
                            group = "317" 'iS Aonline
                        ElseIf sGroup.IndexOf("Embedded", StringComparison.OrdinalIgnoreCase) > -1 Then
                            group = "318" 'EC Aonline
                        ElseIf sGroup.IndexOf("iSys", StringComparison.OrdinalIgnoreCase) > -1 Then
                            group = "320" 'iSystem KA
                        ElseIf sGroup.IndexOf("NC", StringComparison.OrdinalIgnoreCase) > -1 Then
                            group = "323" 'NC KA
                        ElseIf sGroup.IndexOf("ACG", StringComparison.OrdinalIgnoreCase) > -1 Then
                            group = "324" 'ACG
                        ElseIf sGroup.IndexOf("iRetail", StringComparison.OrdinalIgnoreCase) > -1 Then
                            group = "327" 'iRetail
                        Else
                            group = "318"
                        End If

                        'Frank 20170510 New RBU AIB and ANO have not been maintained office code on SAP
                        'So let AIB and ANO's quotes be appproved by original approval flow 
                        If Not String.IsNullOrEmpty(sOffice) Then
                            'If sOffice.Equals("AIB") Then
                            '    sOffice = "AFR"
                            'End If
                            If sOffice.Equals("ANO") Then
                                sOffice = "ABN"
                            End If
                        End If

                        Dim Test_DT As DataTable = getApprovalTableByOfficeNameAndGroup(Mtable, sOffice, group)
                        If IsNothing(Test_DT) OrElse Test_DT.Rows.Count = 0 Then
                            office = "3000" : group = "318"
                        ElseIf Not IsNothing(Test_DT) AndAlso Test_DT.Rows.Count > 0 Then
                            office = Test_DT.Rows(0).Item("Office_code")
                        End If


                        '轉換Sales Group Name
                        'If sGroup = "Embedded" Then
                        '    sGroup = "EC Aonline"
                        'ElseIf sGroup = "iA" Then
                        '    sGroup = "iA Aonline"
                        'End If

                        ''找出office code
                        'Dim T As New Object
                        'T = tbOPBase.dbExecuteScalar("EQ", "SELECT TOP 1 OFFICE_CODE from " & Mtable & " where OFFICE_NAME='" & sOffice & "' and active=1 AND TYPE='GP' and group_name='" & sGroup & "'")
                        'If Not IsNothing(T) AndAlso T.ToString <> "" Then
                        '    office = T.ToString

                        '    '找出Group code
                        '    Dim G As New Object
                        '    G = tbOPBase.dbExecuteScalar("EQ", "SELECT TOP 1 GROUP_CODE from " & Mtable & " where OFFICE_NAME='" & sOffice & "' and active=1 AND TYPE='GP' and group_name='" & sGroup & "'")
                        '    If Not IsNothing(G) AndAlso G.ToString <> "" Then
                        '        group = G.ToString
                        '    Else
                        '        group = "317"  '找不到就都帶317 (EC Aonline)
                        '    End If
                        'Else
                        '    '都沒有就由Account RBU去查office code，而group code就固定是317 (EC Aonline)
                        '    Dim R As New Object
                        '    R = tbOPBase.dbExecuteScalar("EQ", "SELECT TOP 1 OFFICE_CODE from " & Mtable & " where OFFICE_NAME='" & dtAccount(0).RBU & "' and active=1 AND TYPE='GP' and group_name='317'")
                        '    If Not IsNothing(R) AndAlso R.ToString <> "" Then
                        '        office = R.ToString
                        '        group = "317"  '找不到就都帶317 (EC Aonline)
                        '    End If
                        'End If


                        'Dim T As New Object
                        'T = tbOPBase.dbExecuteScalar("EQ", "SELECT TOP 1 OFFICE_CODE from " & Mtable & " where OFFICE_NAME='" & dtAccount(0).RBU & "' and active=1 AND TYPE='GP' and GROUP_CODE='" & getAOnlineGrp() & "'")
                        'If Not IsNothing(T) AndAlso T.ToString <> "" Then
                        '    office = T.ToString
                        '    group = getAOnlineGrp()
                        'End If
                    End If

                End If

                'JJ 2014/2/25 都找不到預設就帶ADL
                If office = "" Then
                    office = "3000"
                End If

                'JJ 2014/2/25 都找不到預設就帶EC Aonline
                If group = "" Then
                    group = "318"
                End If

                'If office = "9999" Then
                '    Dim Account_rowId As String = rowId
                '    If Account_rowId <> "" Then
                '        Dim GroupName As String = SiebelTools.get_PriOwnerDiv_By_RowId(Account_rowId)
                '        If GroupName <> "" Then
                '            If GroupName.ToString.ToLower.IndexOf("dmf") <> -1 Then
                '                group = "996"
                '            ElseIf GroupName.ToString.ToLower.IndexOf("ep") <> -1 Then
                '                group = "998"
                '            ElseIf GroupName.ToString.ToLower.IndexOf("ia") <> -1 Then
                '                group = "997"
                '            Else
                '                GroupName = SiebelTools.get_PriOwnerPos_By_RowId(Account_rowId)
                '                If GroupName.ToString.ToLower.IndexOf("dmf") <> -1 Then
                '                    group = "996"
                '                ElseIf GroupName.ToString.ToLower.IndexOf("ep") <> -1 Then
                '                    group = "998"
                '                ElseIf GroupName.ToString.ToLower.IndexOf("ia") <> -1 Then
                '                    group = "997"
                '                Else
                '                    group = "999"
                '                End If
                '            End If
                '        Else
                '            group = "999"
                '        End If
                '    Else
                '        group = "999"
                '    End If
                'End If
            End If
        End If
        If office <> "" And group <> "" Then
            Dim Approver_DT As DataTable = getApprovalTableByOfficeAndGroup(Mtable, office, group)
            If Not IsNothing(Approver_DT) AndAlso Approver_DT.Rows.Count > 0 Then
                GPLevel_and_Approver = Approver_DT
            Else
                If isJP Then 'Nada: for JP type, if invalid group and office have been passed in , try to retrieve them by normal way
                    getOfficeGroupByCompanyIdOrg(Company_id, org, office, group)
                    Approver_DT = getApprovalTableByOfficeAndGroup(Mtable, office, group)
                    If Not IsNothing(Approver_DT) AndAlso Approver_DT.Rows.Count > 0 Then
                        GPLevel_and_Approver = Approver_DT
                    End If
                Else
                    Return "no GP Approval record for this company"
                End If
            End If
        Else

            Return "no SalesOffice or SalesGroup."
        End If
        Return "S"
    End Function
    Public Shared Function getApprovalTableByOfficeAndGroup(ByVal tbName As String, ByVal office As String, ByVal group As String) As DataTable
        Dim APPROVER_DT As DataTable = tbOPBase.dbGetDataTable("EQ", "SELECT GP_LEVEL,APPROVER from " & tbName & " where OFFICE_CODE='" & office & "' AND GROUP_CODE='" & group & "' and active=1 AND TYPE='GP' order by GP_LEVEL DESC")
        If Not IsNothing(APPROVER_DT) AndAlso APPROVER_DT.Rows.Count > 0 Then
            Return APPROVER_DT
        End If
        Return Nothing
    End Function
    Public Shared Function getApprovalTableByOfficeNameAndGroup(ByVal tbName As String, ByVal office As String, ByVal group As String) As DataTable
        Dim APPROVER_DT As DataTable = tbOPBase.dbGetDataTable("EQ", "SELECT GP_LEVEL,APPROVER,Office_code from " & tbName & " where OFFICE_NAME='" & office & "' AND GROUP_CODE='" & group & "' and active=1 AND TYPE='GP' order by GP_LEVEL DESC")
        If Not IsNothing(APPROVER_DT) AndAlso APPROVER_DT.Rows.Count > 0 Then
            Return APPROVER_DT
        End If
        Return Nothing
    End Function
    Private Shared Function replaceApproverForAJP(ByVal UID As String, ByVal approver As String, ByVal oType As COMM.Fixer.eDocType) As String
        If approver = "FROMEMP" Then

            Dim dtm As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(UID, oType)
            Dim str As String = String.Format(" select top 1 b.EMAIL_ADDR as Manager_Email from MyAdvantechGlobal.dbo.EZ_EMPLOYEE a inner join " & _
                                              " MyAdvantechGlobal.dbo.EZ_EMPLOYEE b on a.MANAGER=b.EZROWID where a.EMAIL_ADDR='{0}'", dtm.CreatedBy)
            Dim o As New Object
            o = tbOPBase.dbExecuteScalar("MY", str)
            If Not IsNothing(o) AndAlso o.ToString <> "" Then
                approver = o.ToString
            End If
        End If
        Return approver
    End Function
    Shared Sub InitApprovalFlow(ByVal UID As String, ByVal rowid As String, ByVal erpid As String, ByVal Detail As List(Of struct_GP_Detail), Optional ByVal type As SAPDAL.SAPDAL.itpType = SAPDAL.SAPDAL.itpType.EU, Optional ByVal office As String = "", Optional ByVal group As String = "")
        'Dim UID As String = setData(Header, Detail)
        If isApproved(UID) Or isInApproval(UID) Or isRejected(UID) Then
            'Exit Sub
        End If
        Dim myQuoteApproval As New quotation_approval("EQ", "quotation_approval")


        'Frank 20170829 get quote's partner VE code
        If type = SAPDAL.SAPDAL.itpType.EU Then
            Dim _SalesCode As String = Advantech.Myadvantech.Business.QuoteBusinessLogic.GetSalesCodeByQuoteID(UID)
            If Advantech.Myadvantech.Business.UserRoleBusinessLogic.IsBBIrelandBySalesID(_SalesCode) Then
                Advantech.Myadvantech.Business.UserRoleBusinessLogic.GetSalesOfficeAndGroupCodeBySalesCode(_SalesCode, office, group)
            End If
        End If



        Dim dt As New DataTable
        getLevelandAppoverList(rowid, erpid, dt, type, office, group)
        Dim Level As Integer = getLevel(rowid, erpid, Detail, type, office, group)
        myQuoteApproval.Delete(String.Format("quote_Id='{0}'", UID))

        Dim isNeedApprovalFlg As Boolean = False
        If Level > 0 Then
            For i As Integer = Level - 1 To 0 Step -1
                Dim MobileApproveYes As String = System.Guid.NewGuid.ToString()
                Dim MobileApproveNo As String = System.Guid.NewGuid.ToString()
                tbOPBase.dbExecuteNoQuery("EQ", "Insert into Quotation_Approval values('" & UID & "','" & replaceApproverForAJP(UID, dt.Rows(i).Item("Approver").ToString, COMM.Fixer.eDocType.EQ) & _
                                        "','" & dt.Rows(i).Item("gp_level").ToString & "','" & i + 1 & "','" & 0 & "','" & Now.ToShortDateString & "','" & MobileApproveYes & "','" & MobileApproveNo & "','Quotation')")

            Next
            isNeedApprovalFlg = True
        End If

        'If isNeedApprovalFlg = False Then
        'Dim UP As Decimal = 0
        'Dim ITP As Decimal = 0
        'Dim isCDLGR As Boolean = False
        'For Each r As struct_GP_Detail In Detail
        '    Dim str As String = String.Format("select product_Line from SAP_PRODUCT where part_no='{0}' and product_line='DLGR'", r.PartNo)
        '    Dim dtDLGR As New DataTable
        '    dtDLGR = tbOPBase.dbGetDataTable("B2B", str)

        '    If dtDLGR.Rows.Count > 0 Then
        '        UP = UP + (r.Price * r.QTY)
        '        ITP = ITP + (r.Itp * r.QTY)
        '        isCDLGR = True
        '    End If
        'Next
        'If type = SAPDAL.SAPDAL.itpType.JP Then
        '    isCDLGR = False
        'End If
        'If isCDLGR Then
        '    If UP = 0 Or (UP > 0 AndAlso (UP - ITP) / UP < 0.2) Then
        '        tbOPBase.dbExecuteNoQuery("EQ", "delete from Quotation_Approval where quote_Id='" & UID & "'")
        '        Dim MobileApproveYes As String = System.Guid.NewGuid.ToString()
        '        Dim MobileApproveNo As String = System.Guid.NewGuid.ToString()
        '        tbOPBase.dbExecuteNoQuery("EQ", "Insert into Quotation_Approval values('" & UID & "','" & "Simon.Purslove@advantech.eu" & _
        '                                "','" & "0.25" & "','" & "1" & "','" & 0 & "','" & Now.ToShortDateString & "','" & MobileApproveYes & "','" & MobileApproveNo & "','Quotation')")
        '        tbOPBase.dbExecuteNoQuery("EQ", "Insert into Quotation_Approval values('" & UID & "','" & "Hans-Peter.Nuedling@advantech.de" & _
        '                                "','" & "0.20" & "','" & "2" & "','" & 0 & "','" & Now.ToShortDateString & "','" & MobileApproveYes & "','" & MobileApproveNo & "','Quotation')")
        '        isNeedApprovalFlg = True
        '    End If
        '    If UP > 0 AndAlso 0.2 < (UP - ITP) / UP < 0.25 Then
        '        tbOPBase.dbExecuteNoQuery("EQ", "delete from Quotation_Approval where quote_Id='" & UID & "'")
        '        Dim MobileApproveYes As String = System.Guid.NewGuid.ToString()
        '        Dim MobileApproveNo As String = System.Guid.NewGuid.ToString()
        '        tbOPBase.dbExecuteNoQuery("EQ", "Insert into Quotation_Approval values('" & UID & "','" & "Simon.Purslove@advantech.eu" & _
        '                               "','" & "0.25" & "','" & "1" & "','" & 0 & "','" & Now.ToShortDateString & "','" & MobileApproveYes & "','" & MobileApproveNo & "','Quotation')")
        '        isNeedApprovalFlg = True
        '    End If
        'End If

        'End If

        If isNeedApprovalFlg = False Then
            tbOPBase.dbExecuteNoQuery("EQ", "Insert into Quotation_Approval values('" & UID & "','system',0,0,0,'" & Now.ToShortDateString & "','','','Quotation')")
        End If

    End Sub
    Shared Function isDLGR(ByVal Detail As List(Of struct_GP_Detail)) As Boolean
        Dim UP As Decimal = 0
        Dim ITP As Decimal = 0
        Dim isCDLGR As Boolean = False
        For Each r As struct_GP_Detail In Detail
            Dim str As String = String.Format("select product_Line from SAP_PRODUCT where part_no='{0}' and product_line='DLGR'", r.PartNo)
            Dim dtDLGR As New DataTable
            dtDLGR = tbOPBase.dbGetDataTable("B2B", str)
            If dtDLGR.Rows.Count > 0 Then
                UP = UP + (r.Price * r.QTY)
                ITP = ITP + (r.Itp * r.QTY)
                isCDLGR = True
            End If
        Next
        If isCDLGR = True Then
            If UP = 0 Or (UP > 0 AndAlso (UP - ITP) / UP < 0.25) Then
                Return True
            End If
        End If

        Return False
    End Function
    Shared Function isInApproval(ByVal UID As String) As Boolean
        Dim myQuoteApproval As New quotation_approval("EQ", "quotation_approval")
        If myQuoteApproval.GetDTbySelectStr(String.Format("select quote_id from {0} group by quote_id having quote_id='{1}' and sum(approval_level)<>sum(status)", myQuoteApproval.tb, UID)).Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function
    Shared Function isApproved(ByVal UId As String) As Boolean
        Dim myQuoteApproval As New quotation_approval("EQ", "quotation_approval")
        If myQuoteApproval.GetDTbySelectStr(String.Format("select quote_id from {0} group by quote_id having quote_id='{1}' and sum(approval_level)=sum(status)", myQuoteApproval.tb, UId)).Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function
    Shared Function isRejected(ByVal UId As String) As Boolean
        Dim myQuoteApproval As New quotation_approval("EQ", "quotation_approval")
        If myQuoteApproval.IsExists(String.Format("quote_Id='{0}' and status=-1", UId)) = 1 Then
            Return True
        End If
        Return False
    End Function

    '......................................................

    Shared Function getApproverStr(ByVal argId As String) As String
        Dim str As String = ""
        Dim dt As New DataTable
        Dim sb As New StringBuilder
        Dim TableName As String = "quotation_approval"
        Dim obj As Object = tbOPBase.dbExecuteScalar("EQ", String.Format("select TOP 1 isnull(ApprovalFlowType,'') as AFT   from  dbo.QuotationExtension  where QuoteID='{0}'", argId))
        If obj IsNot Nothing AndAlso obj.ToString.Trim = "2" Then  TableName = "Quotation_Approval_Expiration"
        Dim myQuoteApproval As New quotation_approval("EQ", TableName)
        If obj IsNot Nothing AndAlso obj.ToString.Trim = "2" Then
            dt = myQuoteApproval.GetDT(String.Format("quoteid='{0}'", argId), "  Approvallevel desc", "status,approver")
        Else
            dt = myQuoteApproval.GetDT(String.Format("quote_id='{0}'", argId), "  Approval_level desc", "status,approver")
        End If
            If dt.Rows.Count > 0 Then
                For Each x As DataRow In dt.Rows
                    Dim colorF As String = "#CCCCCC"
                    If x.Item("status") = -1 Then
                        colorF = "#FF0000"
                    End If
                    If x.Item("status") > 0 Or x.Item("approver").ToString.ToUpper = "SYSTEM" Then
                        colorF = "#00FF00"
                    End If
                    sb.AppendLine("<FONT COLOR=""" & colorF & """>" & x.Item("approver") & "</FONT><br/> ")
                Next
            End If
            Return sb.ToString().Trim("") 'str.Trim(">")
    End Function
    ''' <summary>
    ''' 判断Quote是否正在审核中...
    ''' </summary>
    ''' <param name="QuoteID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function IsWaitingForApproval(ByVal QuoteID As String) As Boolean
        Dim dt As DataTable = tbOPBase.dbGetDataTable("EQ", String.Format(" SELECT Quote_ID  FROM quotation_approval  where  status <> -1 and  Approval_level <> status and  Quote_ID='{0}' union SELECT  quoteid from Quotation_Approval_Expiration where status=0 and QuoteID='{0}'", QuoteID))
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function

    Shared Function Agent(ByVal email As String) As String
        Dim agents As String = ""
        Dim dt As DataTable = tbOPBase.dbGetDataTable("EQ", "select aemail ,seq from agent where email='" & email & "' and getdate() between fdate and tdate order by seq")
        If dt.Rows.Count > 0 Then
            For Each r As DataRow In dt.Rows
                agents &= r.Item("aemail")
                agents &= ","
            Next
        End If
        Return agents
    End Function
    Shared Function getNextLevel(ByVal argId As String) As Integer
        Dim myQA As New quotation_approval("EQ", "quotation_approval")
        Dim DTA As DataTable = myQA.GetDT(String.Format("quote_Id='{0}' and status=0", argId), "Approval_Level")
        If DTA.Rows.Count > 0 And (Not isApproved(argId)) And (Not isRejected(argId)) Then
            Return DTA.Rows(0).Item("Approval_Level")
        End If
        Return 0
    End Function
    Shared Function getNextApprover(ByVal argId As String) As String
        Dim myQA As New quotation_approval("EQ", "quotation_approval")
        Dim L As Integer = getNextLevel(argId)
        If L <> 0 Then
            Dim DTA As DataTable = myQA.GetDT(String.Format("quote_Id='{0}' and Approval_Level='{1}'", argId, L), "")
            If DTA.Rows.Count <> 0 Then
                Return DTA.Rows(0).Item("approver")
            End If
        End If
        Return ""
    End Function
    Shared Function getLastApprovalType(ByVal argId As String) As Integer
        Dim o As New Object
        o = tbOPBase.dbExecuteScalar("EQ", String.Format("select top 1 [status] from quotation_approval where quote_Id='{0}' and [status]<>0 order by Approval_Level desc", argId))
        If IsNumeric(o) Then
            Return CInt(o)
        End If
        Return 1
    End Function
    Shared Function getLastProcessor(ByVal argId As String) As String
        Dim o As New Object
        o = tbOPBase.dbExecuteScalar("EQ", String.Format("select top 1 [Approver] from quotation_approval where quote_Id='{0}' and [status]<>0 order by Approval_Level desc", argId))
        If Not IsNothing(o) AndAlso o.ToString <> "" Then
            Return o.ToString
        End If
        Return "System@System"
    End Function
    Shared Function getLastApprover(ByVal argId As String) As String
        Dim myQA As New quotation_approval("EQ", "quotation_approval")
        Dim dt As DataTable = myQA.GetDT(String.Format("quote_id='{0}' and status>0", argId), "status desc")
        If dt.Rows.Count > 0 Then
            Return dt.Rows(0).Item("approver")
        End If
        Return "System@System"
    End Function
    Shared Function getLastRejecter(ByVal argId As String) As String
        Dim myQA As New quotation_approval("EQ", "quotation_approval")
        Dim dt As DataTable = myQA.GetDT(String.Format("quote_id='{0}' and status=-1", argId), "")
        If dt.Rows.Count > 0 Then
            Return dt.Rows(0).Item("approver")
        End If
        Return "System@System"
    End Function
    'Shared Function getApplicant(ByVal argId As String) As String
    '    Dim myGPMaster As New GP_Master("EQ", "GP_Master")
    '    Dim dt As DataTable = myGPMaster.GetDT(String.Format("GPId='{0}'", argId), "")
    '    If dt.Rows.Count > 0 Then
    '        Return dt.Rows(0).Item("Requestor")
    '    End If
    '    Return ""
    'End Function

    Shared Function GP_getMobileYesOrNoUniqCodeByLevel(ByVal argid As String, ByVal level As Integer, ByVal YesOrNo As String) As String
        If YesOrNo.ToUpper = "YES" Then
            Return tbOPBase.dbExecuteScalar("EQ", "select MobileapproveYES from quotation_approval where approval_level='" & level & "' and quote_id='" & argid & "'")
        Else
            Return tbOPBase.dbExecuteScalar("EQ", "select MobileapproveNO from quotation_approval where approval_level='" & level & "' and quote_id='" & argid & "'")
        End If
        Return ""
    End Function

    Shared Function doApprove(ByVal argID As String, ByVal approveId As String, ByVal comment As String) As Boolean
        Dim level As Integer = getNextLevel(argID)
        Dim uniqidY As String = GP_getMobileYesOrNoUniqCodeByLevel(argID, level, "YES")
        Dim uniqidN As String = GP_getMobileYesOrNoUniqCodeByLevel(argID, level, "NO")
        If uniqidY = approveId Then
            tbOPBase.dbExecuteNoQuery("EQ", "update quotation_approval set status=approval_level where quote_id='" & argID & "' and approval_level='" & level & "'")
        ElseIf uniqidN = approveId Then
            tbOPBase.dbExecuteNoQuery("EQ", "update quotation_approval set status=-1 where quote_id='" & argID & "' and approval_level='" & level & "'")
        End If
        Return False
    End Function
End Class
