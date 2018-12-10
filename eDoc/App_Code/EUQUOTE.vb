Imports Microsoft.VisualBasic

Public Class OP_SiebelTools
    Function getAccountInfo_BYID(ByVal argId As String) As DataTable
        Dim str As String = String.Format("select UID, Customer_Name,Customer_Tel,Customer_Address,Address_Country,Address_City,Address_State,Zip_code,(SELECT CURRENCY FROM QUOTATION_MASTER WHERE QUOTATION_MASTER.QUOTE_ID=SPR.QUOTE_ID) AS CURR,(SELECT Account_Rowid FROM QUOTATION_MASTER WHERE QUOTATION_MASTER.QUOTE_ID=SPR.QUOTE_ID) AS ParentAccountRowID,(SELECT AccountTEAM FROM QUOTATION_MASTER WHERE QUOTATION_MASTER.QUOTE_ID=SPR.QUOTE_ID) AS AccountTEAM from SPR where UID='" & argId & "' and Customer_RowID = ''")
        Dim dt As DataTable = tbOPBase.dbGetDataTable("B2BEU", str)
        Return dt
    End Function
    Function getContactInfo_BYID(ByVal argId As String) As DataTable
        Dim str As String = String.Format("select UID,Attention_Name,Attention_Email,Customer_tel,Customer_RowId from Spr where UID='{0}' AND ContactRowid=''", argId)
        Dim dt As DataTable = tbOPBase.dbGetDataTable("B2BEU", str)
        Return dt
    End Function
    Function getOPTYInfo_BYID(ByVal argId As String) As DataTable
        Dim str As String = String.Format("select quote_id,proj_name,ContactRowId,Comment,(SELECT CURRENCY FROM QUOTATION_MASTER WHERE QUOTATION_MASTER.QUOTE_ID=SPR.QUOTE_ID) AS CURR,(SELECT Account_Rowid FROM QUOTATION_MASTER WHERE QUOTATION_MASTER.QUOTE_ID=SPR.QUOTE_ID) AS AccountRowID from SPR where UID = '{0}' and opty_no=''", argId)
        Dim dt As DataTable = tbOPBase.dbGetDataTable("B2BEU", str)
        Return dt
    End Function
    Function getOPTYinfo_Quotation_BYID(ByVal argId As String) As DataTable
        Dim str As String = String.Format("select quote_id,opty_Name as proj_name,sales_contact as ContactRowId,Quote_note as comment, CURRENCY AS CURR,Account_Rowid AS AccountRowID from quotation_master where quote_id = '{0}' and opty_id='' and quote_id not in(select distinct quote_id from spr) and quote_no=''", argId)
        Dim dt As DataTable = tbOPBase.dbGetDataTable("B2BEU", str)
        Return dt
    End Function
    Function getQuotationInfo_BYID(ByVal argId As String) As DataTable
        Dim str As String = String.Format("Select quote_id,sales_email,OPTY_ID from quotation_master where quote_id='{0}' and QUOTE_NO='' and opty_id<>''", argId)
        Dim dt As DataTable = tbOPBase.dbGetDataTable("B2BEU", str)
        Return dt
    End Function


    Shared Function CreateAccountbySPRid(ByVal argid As String) As String
        Dim MyST As New OP_SiebelTools
        Dim MessageStr As String = ""
        Dim AccountRowID As String = ""
        Dim dt_Account As DataTable = MyST.getAccountInfo_BYID(argid)
        For Each rAccount As DataRow In dt_Account.Rows
            Dim ER As String = ""
            AccountRowID = MyST.CreateAccount(rAccount.Item("Customer_Name"), rAccount.Item("Customer_Tel"), rAccount.Item("Address_Country"), rAccount.Item("Address_City"), rAccount.Item("Zip_Code"), rAccount.Item("Customer_Address"), rAccount.Item("Curr"), rAccount.Item("AccountTeam"), rAccount.Item("ParentAccountRowID"), ER, MessageStr)
            If AccountRowID <> "" Then
                'OP_SPR.updateSPR("UID='" & rAccount.Item("UID") & "'", "Customer_RowID='" & AccountRowID & "'")
            End If
        Next
        Return AccountRowID
    End Function

    Shared Function CreateContactbySPRid(ByVal argid As String) As String
        Dim MyST As New OP_SiebelTools
        Dim MessageStr As String = ""
        Dim ContactRowID As String = ""
        Dim DT_Contact As DataTable = MyST.getContactInfo_BYID(argid)
        For Each rContact As DataRow In DT_Contact.Rows
            Dim Fname As String = ""
            Dim Lname As String = ""
            Fname = rContact.Item("Attention_Name").ToString.Split(".")(1)
            Lname = rContact.Item("Attention_Name").ToString.Split(".")(0)
            ContactRowID = MyST.CreateContact(rContact.Item("Attention_Email"), rContact.Item("Customer_RowId"), Fname, Lname, rContact.Item("Customer_tel"), MessageStr)
            If ContactRowID <> "" Then
                'OP_SPR.updateSPR("UID='" & rContact.Item("UID") & "'", "ContactRowID='" & ContactRowID & "'")
            End If
        Next
        Return ContactRowID
    End Function
    Shared Function UPDATE_Quote_Master(ByVal WHERESTR As String, ByVal SETSTR As String) As Boolean
        Dim STR As String = String.Format("UPDATE quotation_MASTER SET {0} WHERE {1}", SETSTR, WHERESTR)
        tbOPBase.dbExecuteNoQuery("B2BEU", STR)
        Return True
    End Function
    Shared Function GET_Quotation_TotalAmountBy_ID(ByVal ARGID As String) As Decimal

        Dim STR As String = String.Format("select sum(unit_price * qty) as total from quotation_detail where quote_id='{0}' and line_no<>100", ARGID)
        Dim TOTAL As Object = tbOPBase.dbExecuteScalar("B2BEU", STR)
        If Not IsNothing(TOTAL) AndAlso IsNumeric(TOTAL) Then
            Return CType(TOTAL, Decimal)
        Else
            Return 0
        End If
    End Function
    Shared Function GET_Quotation_Owner_Email(ByVal argid As String) As Object
        Dim str As String = String.Format("SELECT TOP 1 ISNULL(Created_by,'') FROM quotation_Master where quote_id='{0}'", argid)
        Dim Sales_Email As Object = tbOPBase.dbExecuteScalar("B2BEU", str)
        Return Sales_Email
    End Function
    Shared Function CreateOptyByQuoteId(ByVal argid As String, ByVal type As String) As String
        Dim MyST As New OP_SiebelTools
        Dim MessageStr As String = ""
        Dim OPTY_NO As String = ""
        Dim DT_OPTY As New DataTable
        If type = "SPR" Then
            'DT_OPTY = MyST.getOPTYInfo_BYID(OP_SPR.getSprIDbyQuoteId(argid))
        ElseIf type = "QUOTE" Then
            DT_OPTY = MyST.getOPTYinfo_Quotation_BYID(argid)
        End If
        For Each rOPTY As DataRow In DT_OPTY.Rows
            Dim ER As String = ""
            Dim odt As DataTable = SiebelTools.GetOwnerOfAccount(rOPTY.Item("AccountROWID"))
            Dim strowner As String = ""
            Dim strPosid As String = ""
            Dim closeDate As Date = Now.AddDays(90)
            If odt.Rows.Count > 0 Then
                strowner = odt.Rows(0).Item("USER_LOGIN")
                strPosid = odt.Rows(0).Item("POSITION_ID")
            End If
            Dim PartDesc As String = OptyGetPartNoDescByQuoteId(rOPTY.Item("quote_id"))
            OPTY_NO = MyST.CreateOpty(PartDesc, rOPTY.Item("AccountROWID"), rOPTY.Item("ContactRowId"), rOPTY.Item("Proj_name"), rOPTY.Item("comment"), GET_Quotation_TotalAmountBy_ID(rOPTY.Item("quote_id")), rOPTY.Item("curr"), closeDate, strPosid, strowner, GET_Quotation_Owner_Email(rOPTY.Item("quote_id")), ER)
            'OPTY_NO = MyST.CreateOpty(rOPTY.Item("AccountROWID"), rOPTY.Item("Proj_name"), rOPTY.Item("comment"), Op_Quotation.GET_Quotation_TotalAmountBy_ID(rOPTY.Item("quote_id")), rOPTY.Item("curr"), ER)
            If OPTY_NO <> "" Then
                If type = "SPR" Then
                    'OP_SPR.updateSPR("quote_id='" & rOPTY.Item("quote_id") & "'", "opty_no='" & OPTY_NO & "'")
                End If
                UPDATE_Quote_Master("quote_id='" & rOPTY.Item("quote_id") & "'", "opty_Id='" & OPTY_NO & "',opty_name='" & rOPTY.Item("proj_name").ToString.Replace("'", "''") & "'")
            End If
        Next
        Return OPTY_NO
    End Function
    Shared Function CreateQuotationbyQuoteid(ByVal argid As String) As String
        Dim MyST As New OP_SiebelTools
        Dim MessageStr As String = ""
        Dim QuoteRowID As String = ""
        Dim DT_Quotation As DataTable = MyST.getQuotationInfo_BYID(argid)
        For Each rQuotation As DataRow In DT_Quotation.Rows

            Dim em As String = ""
            Dim quote_master As DataTable
            Dim cur As String = "", quote_desc As String = "", due_date As String = "", quote_note As String = "", _
            customer_name As String = "", xRowId As String = "", xQuoteNumber As String = "", sales_email As String = "", _
            first_name As String = "", last_name As String = "", del_date1 As String = "", loginName As String = ""
            quote_master = tbOPBase.dbGetDataTable("B2BEU", "select * from quotation_master where quote_id='" & rQuotation.Item("Quote_id") & "'")
            If quote_master.Rows.Count > 0 Then
                cur = quote_master.Rows(0).Item("currency")
                quote_desc = quote_master.Rows(0).Item("description") & "[MA Quote:" & rQuotation.Item("Quote_id") & "]"
                quote_note = quote_master.Rows(0).Item("quote_note")
                customer_name = quote_master.Rows(0).Item("account_rowid")
                sales_email = rQuotation.Item("sales_email")
                del_date1 = quote_master.Rows(0).Item("del_date")
                loginName = quote_master.Rows(0).Item("AccountTeam")
            End If
            Dim quote_detail As New DataTable
            Dim quote_DS As New DataSet
            quote_detail = tbOPBase.dbGetDataTable("B2BEU", "select part_no,qty,unit_price as price,atp_date as del_date from quotation_detail where quote_id='" & rQuotation.Item("Quote_id") & "' order by line_no")
            quote_DS.Tables.Add(quote_detail)
            QuoteRowID = MyST.CreateQuotationWithOpty(xRowId, xQuoteNumber, quote_desc, cur, del_date1, quote_note, customer_name, sales_email, quote_DS, rQuotation.Item("OPTY_ID"), loginName, em, MessageStr)
            If QuoteRowID <> "" Then
                UPDATE_Quote_Master("quote_id='" & rQuotation.Item("Quote_id") & "'", "quote_no='" & QuoteRowID & "'")
            End If
        Next
        Return QuoteRowID
    End Function

    Function getAccountInfo() As DataTable
        Dim str As String = String.Format("select UID, Customer_Name,Customer_Tel,Customer_Address,Address_Country,Address_City,Address_State,Zip_code,(SELECT CURRENCY FROM QUOTATION_MASTER WHERE QUOTATION_MASTER.QUOTE_ID=SPR.QUOTE_ID) AS CURR,(SELECT Account_Rowid FROM QUOTATION_MASTER WHERE QUOTATION_MASTER.QUOTE_ID=SPR.QUOTE_ID) AS ParentAccountRowID,(SELECT AccountTEAM FROM QUOTATION_MASTER WHERE QUOTATION_MASTER.QUOTE_ID=SPR.QUOTE_ID) AS AccountTEAM from SPR where status='" & CInt(COMM.Fixer.eDocStatus.QDRAFT) & "' AND Customer_RowID = ''")
        Dim dt As DataTable = tbOPBase.dbGetDataTable("B2BEU", str)
        Return dt
    End Function
    'Function getContactInfo() As DataTable
    '    Dim str As String = String.Format("select UID,Attention_Name,Attention_Email,Customer_tel,Customer_RowId from Spr where status='DRAFT' AND ContactRowid=''")
    '    Dim dt As DataTable = tbOPBase.dbGetDataTable("B2BEU", str)
    '    Return dt
    'End Function
    'Function getOPTYInfo() As DataTable
    '    Dim str As String = String.Format("select quote_id,proj_name,ContactRowId,Comment,(SELECT CURRENCY FROM QUOTATION_MASTER WHERE QUOTATION_MASTER.QUOTE_ID=SPR.QUOTE_ID) AS CURR,(SELECT Account_Rowid FROM QUOTATION_MASTER WHERE QUOTATION_MASTER.QUOTE_ID=SPR.QUOTE_ID) AS AccountRowID from SPR where status='DRAFT' AND OPTY_NO = ''")
    '    Dim dt As DataTable = tbOPBase.dbGetDataTable("B2BEU", str)
    '    Return dt
    'End Function
    Function getOPTYinfo_Quotation() As DataTable
        Dim str As String = String.Format("select quote_id,opty_name as proj_name,sales_contact as ContactRowId,Quote_note as comment, CURRENCY AS CURR,Account_Rowid AS AccountRowID from quotation_master where opty_id = '' and draftflag='{0}' and quote_id in (select quote_id from quotation_detail group by quote_Id having sum(unit_price * qty)>=20000)", CInt(COMM.Fixer.eDocStatus.QFINISH))
        Dim dt As DataTable = tbOPBase.dbGetDataTable("B2BEU", str)
        Return dt
    End Function
    Function getQuotationInfo() As DataTable
        Dim str As String = String.Format("Select quote_id,sales_email,OPTY_ID from quotation_master where DRAFTFLAG='{0}' AND QUOTE_NO=''", CInt(COMM.Fixer.eDocStatus.QFINISH))
        Dim dt As DataTable = tbOPBase.dbGetDataTable("B2BEU", str)
        Return dt
    End Function

    Shared Function OptyGetPartNoDescByQuoteId(ByVal quoteid As String) As String
        Dim str As String = String.Format("select * from quotation_detail where quote_id='{0}' and part_no not like 'AGS-EW%'", quoteid)
        Dim dt As DataTable = tbOPBase.dbGetDataTable("B2BEU", str)
        Dim strDest As String = ""
        For Each r As DataRow In dt.Rows
            strDest &= " [" & r.Item("part_no") & " × " & r.Item("qty") & "] "
        Next
        Return strDest
    End Function

    Public Function CreateAccountContactQuotationFromSPR() As Boolean
        Dim MessageStr As String = ""
        'Try
        'Dim DT_OPTY As DataTable = getOPTYInfo()
        'If DT_OPTY.Rows.Count > 0 Then
        '    For Each rOPTY As DataRow In DT_OPTY.Rows
        '        Dim ER As String = ""
        '        Dim odt As DataTable = SiebelTools.GetOwnerOfAccount(rOPTY.Item("AccountROWID"))
        '        Dim strowner As String = ""
        '        Dim strPosid As String = ""
        '        Dim closeDate As Date = Now.AddDays(90)
        '        If odt.Rows.Count > 0 Then
        '            strowner = odt.Rows(0).Item("USER_LOGIN")
        '            strPosid = odt.Rows(0).Item("POSITION_ID")
        '        End If
        '        Dim PartDesc As String = OptyGetPartNoDescByQuoteId(rOPTY.Item("quote_id"))
        '        Dim OPTY_NO As String = ""
        '        OPTY_NO = CreateOpty(PartDesc, rOPTY.Item("AccountROWID"), rOPTY.Item("ContactRowId"), rOPTY.Item("Proj_name"), rOPTY.Item("comment"), GET_Quotation_TotalAmountBy_ID(rOPTY.Item("quote_id")), rOPTY.Item("curr"), closeDate, strPosid, strowner, GET_Quotation_Owner_Email(rOPTY.Item("quote_id")), ER)
        '        If OPTY_NO <> "" Then
        '            'OP_SPR.updateSPR("quote_id='" & rOPTY.Item("quote_id") & "'", "opty_no='" & OPTY_NO & "'")
        '            UPDATE_Quote_Master("quote_id='" & rOPTY.Item("quote_id") & "'", "opty_Id='" & OPTY_NO & "',opty_name='" & rOPTY.Item("proj_name").ToString.Replace("'", "''") & "'")
        '        End If
        '    Next
        'End If
        Dim DT_OPTY_Q As DataTable = getOPTYinfo_Quotation()
        If DT_OPTY_Q.Rows.Count > 0 Then
            'OrderUtilities.showDT(DT_OPTY_Q)
            For Each rOPTY As DataRow In DT_OPTY_Q.Rows
                Dim ER As String = ""
                Dim odt As DataTable = SiebelTools.GetOwnerOfAccount(rOPTY.Item("AccountROWID"))
                Dim strowner As String = ""
                Dim strPosid As String = ""
                Dim closeDate As Date = Now.AddDays(90)
                If odt.Rows.Count > 0 Then
                    strowner = odt.Rows(0).Item("USER_LOGIN")
                    strPosid = odt.Rows(0).Item("POSITION_ID")
                End If
                Dim PartDesc As String = OptyGetPartNoDescByQuoteId(rOPTY.Item("quote_id"))
                Dim OPTY_NO As String = ""
                OPTY_NO = CreateOpty(PartDesc, rOPTY.Item("AccountROWID"), rOPTY.Item("ContactRowId"), rOPTY.Item("Proj_name"), rOPTY.Item("comment"), GET_Quotation_TotalAmountBy_ID(rOPTY.Item("quote_id")), rOPTY.Item("curr"), closeDate, strPosid, strowner, GET_Quotation_Owner_Email(rOPTY.Item("quote_id")), ER)

                If OPTY_NO <> "" Then
                    UPDATE_Quote_Master("quote_id='" & rOPTY.Item("quote_id") & "'", "opty_Id='" & OPTY_NO & "',opty_name='" & rOPTY.Item("proj_name").ToString.Replace("'", "''") & "'")
                End If
            Next
        End If

        'Dim DT_Account As DataTable = getAccountInfo()
        'If DT_Account.Rows.Count > 0 Then
        '    For Each rAccount As DataRow In DT_Account.Rows
        '        Dim ER As String = ""
        '        Dim AccountRowID As String = CreateAccount(rAccount.Item("Customer_Name"), rAccount.Item("Customer_Tel"), rAccount.Item("Address_Country"), rAccount.Item("Address_City"), rAccount.Item("Zip_Code"), rAccount.Item("Customer_Address"), rAccount.Item("Curr"), rAccount.Item("AccountTeam"), rAccount.Item("ParentAccountRowID"), ER, MessageStr)
        '        If AccountRowID <> "" Then
        '            'OP_SPR.updateSPR("UID='" & rAccount.Item("UID") & "'", "Customer_RowID='" & AccountRowID & "'")
        '        End If
        '    Next
        'End If

        'Dim DT_Contact As DataTable = getContactInfo()
        'If DT_Contact.Rows.Count > 0 Then
        '    For Each rContact As DataRow In DT_Contact.Rows
        '        Dim Fname As String = ""
        '        Dim Lname As String = ""
        '        Fname = rContact.Item("Attention_Name").ToString.Split(".")(1)
        '        Lname = rContact.Item("Attention_Name").ToString.Split(".")(0)
        '        Dim ContactRowID As String = CreateContact(rContact.Item("Attention_Email"), rContact.Item("Customer_RowId"), Fname, Lname, rContact.Item("Customer_tel"), MessageStr)
        '        If ContactRowID <> "" Then
        '            'OP_SPR.updateSPR("UID='" & rContact.Item("UID") & "'", "ContactRowID='" & ContactRowID & "'")
        '        End If
        '    Next
        'End If

        Dim DT_Quotation As DataTable = getQuotationInfo()
        If DT_Quotation.Rows.Count > 0 Then
            For Each rQuotation As DataRow In DT_Quotation.Rows
                Dim QuoteRowID As String = ""
                Dim em As String = ""
                Dim quote_master As DataTable
                Dim cur As String = "", quote_desc As String = "", due_date As String = "", quote_note As String = "", _
                customer_name As String = "", xRowId As String = "", xQuoteNumber As String = "", sales_email As String = "", _
                first_name As String = "", last_name As String = "", del_date1 As String = "", loginName As String = ""
                quote_master = tbOPBase.dbGetDataTable("B2BEU", "select * from quotation_master where quote_id='" & rQuotation.Item("Quote_id") & "'")
                If quote_master.Rows.Count > 0 Then
                    cur = quote_master.Rows(0).Item("currency")
                    quote_desc = quote_master.Rows(0).Item("description") & "[MA Quote:" & rQuotation.Item("Quote_id") & "]"
                    quote_note = quote_master.Rows(0).Item("quote_note")
                    customer_name = quote_master.Rows(0).Item("account_rowid")
                    sales_email = rQuotation.Item("sales_email")
                    del_date1 = quote_master.Rows(0).Item("del_date")
                    loginName = quote_master.Rows(0).Item("AccountTeam")
                End If
                Dim quote_detail As New DataTable
                Dim quote_DS As New DataSet
                quote_detail = tbOPBase.dbGetDataTable("B2BEU", "select part_no,qty,unit_price as price,atp_date as del_date from quotation_detail where quote_id='" & rQuotation.Item("Quote_id") & "' order by line_no")
                quote_DS.Tables.Add(quote_detail)
                QuoteRowID = CreateQuotationWithOpty(xRowId, xQuoteNumber, quote_desc, cur, del_date1, quote_note, customer_name, sales_email, quote_DS, rQuotation.Item("OPTY_ID"), loginName, em, MessageStr)
                'HttpContext.Current.Response.Write(QuoteRowID) : HttpContext.Current.Response.End()

                If QuoteRowID <> "" Then
                    UPDATE_Quote_Master("quote_id='" & rQuotation.Item("Quote_id") & "'", "quote_no='" & QuoteRowID & "'")
                End If
            Next
        End If
        'Catch ex As Exception
        'Finally
        Util.SendEmail("eBusiness.AEU@advantech.eu", "eBusiness.AEU@advantech.eu", "", "", "Report_CreateAccountContactQuotation:" & Now.ToString("yyyy/MM/dd"), "", MessageStr, "")
        'End Try
        Return True
    End Function




    Function CreateAccount(ByVal AccountName As String, ByVal MainTel As String, ByVal County As String, ByVal city As String, ByVal zipCode As String, ByVal address1 As String, ByVal Curr As String, ByVal AccountTeam As String, ByVal AccountParent As String, ByRef ER As String, ByRef MessageStr As String) As String
        Dim WS As New SiebelWS.Siebel_WS
        WS.Timeout = Integer.MaxValue
        Dim AccountRowID As String = ""
        AccountRowID = WS.CreateNewAccount("", AccountName, "", "", "", "", "", "", AccountTeam, city, County, zipCode, address1, "", Curr, "", "", AccountParent, ER)
        MessageStr &= "Return AccountRowId:" & AccountRowID
        MessageStr &= System.Environment.NewLine
        MessageStr &= "Call Method:" & "WS.CreateNewAccount(""""," & AccountName & ", """", """", """", """", """", """"," & AccountTeam & "," & city & "," & County & "," & zipCode & "," & address1 & ", """"," & Curr & ", """", """"," & AccountParent & "," & ER & ")"
        MessageStr &= System.Environment.NewLine
        WS.Dispose()
        'Global_Inc.Utility_EMailPage("eBusiness.AEU@advantech.eu", "eBusiness.AEU@advantech.eu", "", "", "Report_CreateAccountContactQuotation:" & Now.ToString("yyyy/MM/dd"), "", MessageStr)
        Util.SendEmail("eBusiness.AEU@advantech.eu", "eBusiness.AEU@advantech.eu", "", "", "Report_CreateAccountContactQuotation:" & Now.ToString("yyyy/MM/dd"), "", MessageStr, "")

        Return AccountRowID
    End Function
    Function CreateContact(ByVal Email As String, ByVal AccountRowid As String, ByVal Fname As String, ByVal Lname As String, ByVal Tel As String, ByRef MessageStr As String) As String
        Dim WS As New SiebelWS.Siebel_WS
        WS.Timeout = Integer.MaxValue
        Dim ContactRowID As String = ""
        ContactRowID = WS.CreateNewContact(Email, AccountRowid, Fname, Lname, False, False, "", Tel)
        MessageStr &= "Return ContactRowId:" & ContactRowID
        MessageStr &= System.Environment.NewLine
        MessageStr &= "Call Method:" & "WS.CreateNewContact(" & Email & "," & AccountRowid & "," & Fname & "," & Lname & ", False, """"," & Tel & ")"
        MessageStr &= System.Environment.NewLine

        WS.Dispose()
        'Global_Inc.Utility_EMailPage("eBusiness.AEU@advantech.eu", "eBusiness.AEU@advantech.eu", "", "", "Report_CreateAccountContactQuotation:" & Now.ToString("yyyy/MM/dd"), "", MessageStr)
        Util.SendEmail("eBusiness.AEU@advantech.eu", "eBusiness.AEU@advantech.eu", "", "", "Report_CreateAccountContactQuotation:" & Now.ToString("yyyy/MM/dd"), "", MessageStr, "")

        Return ContactRowID
    End Function
    Function CreateOpty(ByVal PartNoDesc As String, ByVal AccountRowID As String, ByVal contactrowId As String, ByVal PROJ_NAME As String, ByVal PROJ_DESC As String, ByVal TOTALrevenue As String, ByVal Curr As String, ByVal closedate As Date, ByVal strPosid As String, ByVal strOwner As String, ByVal CreatedBY As String, ByRef MessageStr As String) As String
        If TOTALrevenue < 20000 Then
            Return ""
        End If
        Dim strComment As String = PROJ_DESC & " [This is an auto-created OPTY linked with a quotation created by: " & CreatedBY & "]"
        strComment = strComment & " " & PartNoDesc & " "
        Dim WS As New SiebelWS.Siebel_WS
        WS.Timeout = Integer.MaxValue
        Dim opty_no As String = WS.Import_Opportunity(strPosid, strOwner, AccountRowID, contactrowId, PROJ_NAME, strComment, "Funnel Sales Methodology", _
                                  "50% Negotiating", closedate, TOTALrevenue, "50", "Pending", "", Curr)
        MessageStr &= "Return OptyRowId:" & opty_no
        MessageStr &= System.Environment.NewLine
        MessageStr &= "Call Method:" & "WS.Import_Opportunity(" & strPosid & "," & strOwner & "," & AccountRowID & "," & contactrowId & "," & PROJ_NAME & "," & PROJ_DESC & ",Funnel Sales Methodology," & _
                                  "50% Negotiating," & closedate & "," & TOTALrevenue & ",25,Pending,," & Curr & ")"
        MessageStr &= System.Environment.NewLine
        WS.Dispose()
        'Global_Inc.Utility_EMailPage("eBusiness.AEU@advantech.eu", "eBusiness.AEU@advantech.eu", "", "", "Report_CreateAccountContactQuotation:" & Now.ToString("yyyy/MM/dd"), "", MessageStr)
        Util.SendEmail("eBusiness.AEU@advantech.eu", "eBusiness.AEU@advantech.eu", "", "", "Report_CreateAccountContactQuotation:" & Now.ToString("yyyy/MM/dd"), "", MessageStr, "")

        Return opty_no
    End Function
    Function CreateQuotationWithOpty(ByVal xRowId As String, ByVal xQuoteNumber As String, ByVal quote_desc As String, ByVal cur As String, ByVal del_date1 As String, ByVal quote_note As String, ByVal customer_name As String, ByVal sOwner As String, ByVal quote_DS As DataSet, ByVal opty_id As String, ByVal LoginName As String, ByRef EM As String, ByRef MessageStr As String) As String
        Dim WS As New SiebelWS.Siebel_WS
        WS.Timeout = Integer.MaxValue

        WS.CreateSiebelQuotationWithOpportunity2(xRowId, xQuoteNumber, quote_desc, cur, del_date1, quote_note, customer_name, sOwner, quote_DS, opty_id, LoginName, EM)
        MessageStr &= "Return QuotationRowId:" & xRowId
        MessageStr &= System.Environment.NewLine
        MessageStr &= "Call Method:" & "WS.CreateSiebelQuotationWithOpportunity(" & xRowId & "," & xQuoteNumber & "," & quote_desc & ", " & cur & "," & del_date1 & "," & quote_note & "," & customer_name & "," & sOwner & ",quote_DS," & opty_id & "," & LoginName & "," & EM & ")"
        MessageStr &= System.Environment.NewLine
        WS.Dispose()
        'Global_Inc.Utility_EMailPage("eBusiness.AEU@advantech.eu", "eBusiness.AEU@advantech.eu", "", "", "Report_CreateAccountContactQuotation:" & Now.ToString("yyyy/MM/dd"), "", MessageStr)
        Util.SendEmail("eBusiness.AEU@advantech.eu", "eBusiness.AEU@advantech.eu", "", "", "Report_CreateAccountContactQuotation:" & Now.ToString("yyyy/MM/dd"), "", MessageStr, "")

        Return xRowId
    End Function
End Class
