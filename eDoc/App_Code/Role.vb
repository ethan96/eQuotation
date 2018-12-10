Imports Microsoft.VisualBasic
Imports Advantech.Myadvantech.Business

Public Class Role

    Public Shared Function IseQuotationBusinessOwner(ByVal userid As String) As Boolean
        If userid.Equals("Patty.Chen@advantech.com.tw", StringComparison.InvariantCultureIgnoreCase) Then Return True
        If userid.Equals("TC.Chen@advantech.com.tw", StringComparison.InvariantCultureIgnoreCase) Then Return True
        If userid.Equals("Frank.Chung@advantech.com.tw", StringComparison.InvariantCultureIgnoreCase) Then Return True
    End Function

    ''' <summary>
    ''' Can user overwrite quote's unit price. 2015/2/12 Change this function name to IsAUSpowerUser. By ICC
    ''' </summary>
    ''' <param name="userid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsAUSpowerUser(ByVal userid As String) As Boolean

        If Role.IsAdmin Then Return True
        If String.IsNullOrEmpty(userid) Then Return False

        If userid.Equals("Fei.Khong@advantech.com", StringComparison.InvariantCultureIgnoreCase) Then Return True
        If userid.Equals("Denise.Kwong@advantech.com", StringComparison.InvariantCultureIgnoreCase) Then Return True
        If userid.Equals("Gary.Lee@advantech.com.tw", StringComparison.InvariantCultureIgnoreCase) Then Return True

        Return False

    End Function

    Public Shared Function IsAACpowerUser(ByVal userid As String) As Boolean

        If Role.IsAdmin Then Return True
        If String.IsNullOrEmpty(userid) Then Return False

        If userid.Equals("Lynette.Andersen@advantech.com", StringComparison.InvariantCultureIgnoreCase) Then Return True

        Return False

    End Function



    ''' <summary>
    ''' Can user see margin in quoteDetail. 2015/2/12 By ICC
    ''' </summary>
    ''' <param name="userid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsAllowSeeCost(ByVal userid As String) As Boolean
        If IsAUSpowerUser(userid) Then Return True

        If userid.Equals("Cathee.Cao@advantech.com", StringComparison.InvariantCultureIgnoreCase) Then Return True
        If userid.Equals("Nels.Toriano@advantech.com", StringComparison.InvariantCultureIgnoreCase) Then Return True
        If userid.Equals("tim.sterling@advantech.com", StringComparison.InvariantCultureIgnoreCase) Then Return True
        If userid.Equals("Kurt.Kleinschmidt@advantech.com", StringComparison.InvariantCultureIgnoreCase) Then Return True
        If userid.Equals("mike.arcure@advantech.com", StringComparison.InvariantCultureIgnoreCase) Then Return True
        If userid.Equals("Gabriela.Meza@advantech.com", StringComparison.InvariantCultureIgnoreCase) Then Return True
        Return False
    End Function

    Public Shared Function IsANAAOnlineTeamsByOfficeCode() As Boolean

        If Role.IsAonlineUsa() AndAlso Pivot.CurrentProfile.SalesOfficeCode.Contains("2700") Then
            Return True
        End If

        If Role.IsAonlineUsaIag() AndAlso Pivot.CurrentProfile.SalesOfficeCode.Contains("2110") Then
            Return True
        End If

        If Role.IsAonlineUsaISystem() AndAlso Pivot.CurrentProfile.SalesOfficeCode.Contains("2110") Then
            Return True
        End If


        Return False
    End Function


    ''' <summary>
    ''' 判断美国AAC用户
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsUSAACSales() As Boolean
        If Not IsNothing(Pivot.CurrentProfile) Then
            'Dim _mailgroup As New PROF.MailGroup
            'If _mailgroup.IsAUSAACCheck(Pivot.CurrentProfile.GroupBelTo) Then
            '    Return True
            'End If
            'If (Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.AAC) = Pivot.CurrentProfile.CurrDocReg _
            '    AndAlso Pivot.CurrentProfile.SalesOfficeCode.Contains("2100") Then

            'If (Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.AAC) = Pivot.CurrentProfile.CurrDocReg Then
            If UserRoleBusinessLogic.IsInGroupSalesIagUsa(Pivot.CurrentProfile.GroupBelTo) Then
                Return True
            End If
        End If
        Return False
    End Function


    ''' <summary> 
    ''' 判断当前用户是否是Aonline Usa ISystem用户
    ''' </summary> 
    ''' <returns></returns> 
    ''' <remarks></remarks> 
    Public Shared Function IsAonlineUsaISystem() As Boolean
        If Not IsNothing(Pivot.CurrentProfile) Then
            'Dim _mailgroup As New PROF.MailGroup
            If UserRoleBusinessLogic.IsInGroupAonlineUsaISystem(Pivot.CurrentProfile.GroupBelTo) Then
                Return True
            End If
        End If
        Return False
    End Function


    ''' <summary> 
    ''' 判断当前用户是否是AonlineUsaIag用户
    ''' </summary> 
    ''' <returns></returns> 
    ''' <remarks></remarks> 
    Public Shared Function IsAonlineUsaIag() As Boolean
        If Not IsNothing(Pivot.CurrentProfile) Then
            'Dim _mailgroup As New PROF.MailGroup
            'If SAPDAL.UserRole.IsInGroupAonlineUsaIag(Pivot.CurrentProfile.GroupBelTo) Then
            If UserRoleBusinessLogic.IsInGroupAonlineUsaIag(Pivot.CurrentProfile.GroupBelTo) Then
                Return True
            End If
        End If
        Return False
    End Function
    ''' <summary> 
    ''' 判断当前用户是否是AonlineUsa用户
    ''' </summary> 
    ''' <returns></returns> 
    ''' <remarks></remarks> 
    Public Shared Function IsAonlineUsa() As Boolean
        If Not IsNothing(Pivot.CurrentProfile) Then
            'Dim _mailgroup As New PROF.MailGroup
            'If SAPDAL.UserRole.IsInGroupAonlineUsa(Pivot.CurrentProfile.GroupBelTo) Then
            If UserRoleBusinessLogic.IsInGroupAonlineUsa(Pivot.CurrentProfile.GroupBelTo) Then
                Return True
            End If
        End If
        Return False
    End Function



    ''' <summary> 
    ''' Check is PAPS.eStore user. ICC 2015/2/10
    ''' </summary> 
    ''' <returns></returns> 
    ''' <remarks></remarks> 
    Public Shared Function IsPAPSeStore() As Boolean
        If Not IsNothing(Pivot.CurrentProfile) Then
            'Dim _mailgroup As New PROF.MailGroup
            If UserRoleBusinessLogic.IsGroupPAPSeStore(Pivot.CurrentProfile.GroupBelTo) Then
                Return True
            End If
        End If
        Return False
    End Function
    ''' <summary> 
    ''' Check is AENC user. ICC 2015/5/4
    ''' </summary> 
    ''' <returns></returns> 
    ''' <remarks></remarks> 
    Public Shared Function IsUsaAenc() As Boolean
        If Not IsNothing(Pivot.CurrentProfile) Then
            If UserRoleBusinessLogic.IsGroupUsaAenc(Pivot.CurrentProfile.GroupBelTo) Then
                Return True
            End If
        End If
        Return False
    End Function

    ''' <summary>
    ''' 判断全美国用户
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsUsaUser() As Boolean
        If Not IsNothing(Pivot.CurrentProfile) Then
            'If (Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.ANA) = Pivot.CurrentProfile.CurrDocReg Then
            '    Return True
            'End If
            'ICC 2015/2/10 Change old rule from ANA to AUS because AUS cover ANA, AAC, PAPS
            If (Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.AUS) = Pivot.CurrentProfile.CurrDocReg Then
                Return True
            End If
        End If
        Return False
    End Function
    ''' <summary>
    ''' '判断全美国用户
    ''' </summary>
    ''' <param name="Email">传递userid</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsUsaUser(ByVal Email As String) As Boolean
        If String.IsNullOrEmpty(Email) Then Return False
        Dim _UserProObj As IBUS.iRole = Pivot.NewObjProfile
        Dim ur As IBUS.iRole = _UserProObj.getRoleByUser(Email)
        If Not IsNothing(ur) Then
            'If (ur.CurrDocReg And COMM.Fixer.eDocReg.ANA) = ur.CurrDocReg Then
            '    Return True
            'End If
            'Frank 2015/2/10 Change old rule from ANA to AUS because AUS cover ANA, AAC, PAPS
            If (ur.CurrDocReg And COMM.Fixer.eDocReg.AUS) = ur.CurrDocReg Then
                Return True
            End If
        End If
        Return False
    End Function
    Public Shared Function IsUSSaless() As Boolean
        If Not IsNothing(Pivot.CurrentProfile) Then
            If (Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.AUS) = Pivot.CurrentProfile.CurrDocReg Then
                Return True
            End If
        End If
        Return False
    End Function
    ''' <summary>
    ''' 判断墨西哥用户
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsMexicoAonlineSales() As Boolean
        If Not IsNothing(Pivot.CurrentProfile) Then
            If (Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.AMX) = Pivot.CurrentProfile.CurrDocReg Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Shared Function IsKRAonlineSales() As Boolean
        If Not IsNothing(Pivot.CurrentProfile) Then
            If (Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.AKR) = Pivot.CurrentProfile.CurrDocReg Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Shared Function IsABRSales() As Boolean
        If Not IsNothing(Pivot.CurrentProfile) Then
            If (Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.ABR) = Pivot.CurrentProfile.CurrDocReg Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Shared Function IsAAUSales() As Boolean
        If Not IsNothing(Pivot.CurrentProfile) Then
            If (Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.AAU) = Pivot.CurrentProfile.CurrDocReg Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Shared Function IsInterconIA() As Boolean
        If Not IsNothing(Pivot.CurrentProfile) Then
            If (Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.HQDC_IA) = Pivot.CurrentProfile.CurrDocReg Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Shared Function IsInterconEC() As Boolean
        If Not IsNothing(Pivot.CurrentProfile) Then
            If (Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.HQDC_EC) = Pivot.CurrentProfile.CurrDocReg Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Shared Function IsInterconIS() As Boolean
        If Not IsNothing(Pivot.CurrentProfile) Then
            If (Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.HQDC_IS) = Pivot.CurrentProfile.CurrDocReg Then
                Return True
            End If
        End If
        Return False
    End Function



    Public Shared Function IsHQDCSales() As Boolean
        If Not IsNothing(Pivot.CurrentProfile) Then
            'If (Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.HQDC) = Pivot.CurrentProfile.CurrDocReg Then
            '    Return True
            'End If
            If (Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.HQDC_IA) = Pivot.CurrentProfile.CurrDocReg Then
                Return True
            End If
            If (Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.HQDC_EC) = Pivot.CurrentProfile.CurrDocReg Then
                Return True
            End If
            If (Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.HQDC_IS) = Pivot.CurrentProfile.CurrDocReg Then
                Return True
            End If

        End If
        Return False
    End Function


    Public Shared Function IsJPAonlineSales() As Boolean
        If Not IsNothing(Pivot.CurrentProfile) Then
            If (Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.AJP) = Pivot.CurrentProfile.CurrDocReg Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Shared Function IsInternalUser() As Boolean
        If Not IsNothing(Pivot.CurrentProfile) Then
            If Pivot.CurrentProfile.Role = COMM.Fixer.eRole.isInternal Then
                Return True
            End If
        End If
        Return False
    End Function
    Public Shared Function IsAdmin() As Boolean
        If Not IsNothing(Pivot.CurrentProfile) Then
            If Pivot.CurrentProfile.Role = COMM.Fixer.eRole.isAdmin Then
                Return True
            End If
        End If
        Return False
    End Function
    Public Shared Function IsFranchiser() As Boolean
        If Not IsNothing(Pivot.CurrentProfile) Then
            If (Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.AFC) = Pivot.CurrentProfile.CurrDocReg Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Shared Function IsEUSales() As Boolean
        If Not IsNothing(Pivot.CurrentProfile) Then
            If (Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.AEU) = Pivot.CurrentProfile.CurrDocReg Then
                Return True
            End If
        End If
        Return False
    End Function
    Public Shared Function IsCNAonlineSales() As Boolean
        If Not IsNothing(Pivot.CurrentProfile) Then
            If (Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.ACN) = Pivot.CurrentProfile.CurrDocReg Then
                Return True
            End If
        End If
        Return False
    End Function
    Public Shared Function IsTWAonlineSales() As Boolean
        If Not IsNothing(Pivot.CurrentProfile) Then
            If (Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.ATW) = Pivot.CurrentProfile.CurrDocReg Then
                Return True
            End If
        End If
        Return False
    End Function
    Public Shared Function IsATWKASales(ByVal GA As ArrayList) As Boolean
        If GA.Contains(("Sales.ATW.KA.ES-KA1").ToUpper) _
        OrElse GA.Contains(("Sales.ATW.KA.ES-KA2").ToUpper) _
        OrElse GA.Contains(("Sales.ATW.KA.ES-KA3").ToUpper) _
        OrElse GA.Contains(("Sales.ATW.KA.ES-KA4").ToUpper) _
        OrElse GA.Contains(("Sales.ATW.KA.IAKA").ToUpper) Then
            Return True
        End If
        Return False
    End Function

    Public Shared Function IsATWAiSTSales(ByVal GA As ArrayList) As Boolean
        If GA.Contains(("Sales.ATW.AiS").ToUpper) Then
            Return True
        End If
        Return False
    End Function


    Public Shared Function IsATWAonlineSales(ByVal GA As ArrayList) As Boolean
        If GA.Contains(("Sales.ATW.AOL-Neihu(IIoT)").ToUpper) _
            OrElse GA.Contains(("Sales.ATW.AOL-EC").ToUpper) _
            OrElse GA.Contains(("CallCenter.IA.ACL").ToUpper) _
            OrElse GA.Contains(("Sales.ATW.AOL-ATC(IIoT)").ToUpper) Then
            Return True
        End If
        Return False
    End Function

    Public Shared Function IsCAPS() As Boolean
        If Not IsNothing(Pivot.CurrentProfile) Then
            If (Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.CAPS) = Pivot.CurrentProfile.CurrDocReg Then
                Return True
            End If
        End If
        Return False
    End Function

End Class
