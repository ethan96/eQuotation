Imports System.Activities.Tracking
Imports System.Text
Imports System.Xml
Imports System.Runtime.Serialization
Imports System.Configuration

Namespace WorkFlowlAPI

    Public Class SQLTrackingParticipant
    Inherits TrackingParticipant

    Public WorkflowList As List(Of CURATION_LEAD_TRACKING_LOG_WORKFLOW), ActivityList As List(Of CURATION_LEAD_TRACKING_LOG_ACTIVITY)
    Protected Overrides Sub Track(record As TrackingRecord, timeout As TimeSpan)
        'Dim conn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("CPOOL").ConnectionString)
        'If conn.State <> ConnectionState.Open Then conn.Open()
        'Dim cmd As New SqlClient.SqlCommand

        Dim workflowInstanceRecord As WorkflowInstanceRecord = TryCast(record, WorkflowInstanceRecord)
        If workflowInstanceRecord IsNot Nothing Then
            'cmd = CreateTrackingCommand(workflowInstanceRecord)
            CreateTrackingLog(workflowInstanceRecord)
        End If

        Dim activityStateRecord As ActivityStateRecord = TryCast(record, ActivityStateRecord)
        If activityStateRecord IsNot Nothing Then
            'cmd = CreateTrackingCommand(activityStateRecord)
            CreateTrackingLog(activityStateRecord)
        End If

        Dim faultPropagationRecord As FaultPropagationRecord = TryCast(record, FaultPropagationRecord)
        If faultPropagationRecord IsNot Nothing Then
            'cmd = CreateTrackingCommand(faultPropagationRecord)
            CreateTrackingLog(faultPropagationRecord)
        End If

        Dim customTrackingRecord As CustomTrackingRecord = TryCast(record, CustomTrackingRecord)
        If customTrackingRecord IsNot Nothing Then
            'cmd = CreateTrackingCommand(customTrackingRecord)
            CreateTrackingLog(customTrackingRecord)
        End If

        'cmd.Connection = conn : cmd.CommandTimeout = 60
        'Try
        '    'cmd.ExecuteNonQuery()
        'Catch ex As Exception
        '    Dim para As New Dictionary(Of String, Object)
        '    para.Add("SQL", cmd.CommandText)
        '    ErrorHandler.InsertErrorLog("ImportTrackingLog", para, ex.ToString)
        'End Try
    End Sub

    Public Sub CreateTrackingLog(Record As WorkflowInstanceRecord)
        Dim workflowIns As New CURATION_LEAD_TRACKING_LOG_WORKFLOW
        With workflowIns
            .WorkflowInstanceId = Record.InstanceId
            .WorkflowActivityDefinition = Record.ActivityDefinitionId
            .RecordNumber = Record.RecordNumber
            .State = Record.State
            .TraceLevelId = Record.Level
            .AnnotationsXml = ""
            .CreatedTime = Now
            Dim unhandledExceptionRecord As WorkflowInstanceUnhandledExceptionRecord = TryCast(Record, WorkflowInstanceUnhandledExceptionRecord)
            If unhandledExceptionRecord IsNot Nothing Then
                .ExceptionDetails = unhandledExceptionRecord.UnhandledException.ToString
            End If
            Dim terminatedRecord As WorkflowInstanceTerminatedRecord = TryCast(Record, WorkflowInstanceTerminatedRecord)
            If terminatedRecord IsNot Nothing Then
                .Reason = terminatedRecord.Reason
            Else
                Dim instanceAbortedRecord As WorkflowInstanceAbortedRecord = TryCast(Record, WorkflowInstanceAbortedRecord)
                If instanceAbortedRecord IsNot Nothing Then
                    .Reason = instanceAbortedRecord.Reason
                Else
                    Dim suspendedRecord As WorkflowInstanceSuspendedRecord = TryCast(Record, WorkflowInstanceSuspendedRecord)
                    If suspendedRecord IsNot Nothing Then
                        .Reason = suspendedRecord.Reason
                    End If
                End If
            End If
        End With
        Me.WorkflowList.Add(workflowIns)
    End Sub

    Public Sub CreateTrackingLog(Record As ActivityStateRecord)
        Dim activityIns As New CURATION_LEAD_TRACKING_LOG_ACTIVITY
        With activityIns
            .WorkflowInstanceId = Record.InstanceId
            .ActivityInstanceId = Record.Activity.InstanceId
            .ActivityId = Record.Activity.Id
            .ActivityRecordType = "ActivityStateRecord"
            .ActivityName = Record.Activity.Name
            .State = Record.State
            .RecordNumber = Record.RecordNumber
            .TraceLevelId = Record.Level
            .ActivityType = Record.Activity.TypeName
            .ArgumentsXml = ArgumentsToString(Record.Arguments)
            .VariablesXml = SerializeData(Record.Variables)
            .AnnotationsXml = SerializeData(Record.Annotations)
            .CreatedTime = Now
        End With
        Me.ActivityList.Add(activityIns)
    End Sub

    Public Sub CreateTrackingLog(Record As ActivityScheduledRecord)
        Dim activityIns As New CURATION_LEAD_TRACKING_LOG_ACTIVITY
        With activityIns
            .WorkflowInstanceId = Record.InstanceId
            .ActivityInstanceId = Record.Activity.InstanceId
            .ActivityId = Record.Activity.Id
            .ActivityRecordType = "ActivityScheduledRecord"
            .ActivityName = Record.Activity.Name
            .RecordNumber = Record.RecordNumber
            .TraceLevelId = Record.Level
            .ActivityType = Record.Activity.TypeName
            .AnnotationsXml = """"
            .CreatedTime = Now
            .ChildActivityName = Record.Child.Name
            .ChildActivityId = Record.Child.Id
            .ChildActivityInstanceId = Record.Child.InstanceId
            .ChildActivityType = Record.Child.TypeName
        End With
        Me.ActivityList.Add(activityIns)
    End Sub

    Public Sub CreateTrackingLog(Record As CustomTrackingRecord)
        Dim activityIns As New CURATION_LEAD_TRACKING_LOG_ACTIVITY
        With activityIns
            .WorkflowInstanceId = Record.InstanceId
            .ActivityInstanceId = Record.Activity.InstanceId
            .ActivityId = Record.Activity.Id
            .ActivityRecordType = "CustomTrackingRecord"
            .ActivityName = Record.Activity.Name
            .RecordNumber = Record.RecordNumber
            .TraceLevelId = Record.Level
            .ActivityType = Record.Activity.TypeName
            .AnnotationsXml = ""
            .CreatedTime = Now
            .CustomRecordName = Record.Name
            .CustomRecordDataXml = ArgumentsToString(Record.Data)
        End With
        Me.ActivityList.Add(activityIns)
    End Sub

    Public Sub CreateTrackingLog(Record As FaultPropagationRecord)
        Dim activityIns As New CURATION_LEAD_TRACKING_LOG_ACTIVITY
        With activityIns
            .WorkflowInstanceId = Record.InstanceId
            .ActivityInstanceId = Record.FaultSource.InstanceId
            .ActivityId = Record.FaultSource.Id
            .ActivityRecordType = "FaultPropagationRecord"
            .ActivityName = Record.FaultSource.Name
            .RecordNumber = Record.RecordNumber
            .TraceLevelId = Record.Level
            .ActivityType = Record.FaultSource.TypeName
            .AnnotationsXml = ""
            .CreatedTime = Now
            .FaultDetails = Record.Fault.ToString
            If Record.FaultHandler IsNot Nothing Then
                .FaultHandlerActivityName = Record.FaultHandler.Name
                .FaultHandlerActivityId = Record.FaultHandler.Id
                .FaultHandlerActivityInstanceId = Record.FaultHandler.InstanceId
                .FaultHandlerActivityType = Record.FaultHandler.TypeName
            End If
        End With
        Me.ActivityList.Add(activityIns)
    End Sub

    'Public Function CreateTrackingCommand(Record As WorkflowInstanceRecord) As SqlClient.SqlCommand
    '    Dim cmd As New SqlClient.SqlCommand
    '    Dim sql As New StringBuilder
    '    With sql
    '        .AppendLine(" INSERT INTO CurationPool.dbo.CURATION_LEAD_TRACKING_LOG_WORKFLOW values ")
    '        .AppendLine(" (@WorkflowInstanceId,@WorkflowActivityDefinition,@RecordNumber,@State,@TraceLevelId,@AnnotationsXml,@TimeCreated,@ExceptionDetails,@Reason) ")
    '    End With
    '    cmd.CommandText = sql.ToString
    '    With cmd.Parameters
    '        .AddWithValue("WorkflowInstanceId", Record.InstanceId.ToString)
    '        .AddWithValue("WorkflowActivityDefinition", Record.ActivityDefinitionId)
    '        .AddWithValue("RecordNumber", Record.RecordNumber)
    '        .AddWithValue("State", Record.State)
    '        .AddWithValue("TraceLevelId", Record.Level)
    '        .AddWithValue("AnnotationsXml", "")
    '        .AddWithValue("TimeCreated", Now)
    '        Dim unhandledExceptionRecord As WorkflowInstanceUnhandledExceptionRecord = TryCast(Record, WorkflowInstanceUnhandledExceptionRecord)
    '        If unhandledExceptionRecord IsNot Nothing Then
    '            .AddWithValue("ExceptionDetails", unhandledExceptionRecord.UnhandledException.ToString)
    '        Else
    '            .AddWithValue("ExceptionDetails", "")
    '        End If
    '        Dim terminatedRecord As WorkflowInstanceTerminatedRecord = TryCast(Record, WorkflowInstanceTerminatedRecord)
    '        If terminatedRecord IsNot Nothing Then
    '            .AddWithValue("Reason", terminatedRecord.Reason)
    '        Else
    '            Dim instanceAbortedRecord As WorkflowInstanceAbortedRecord = TryCast(Record, WorkflowInstanceAbortedRecord)
    '            If instanceAbortedRecord IsNot Nothing Then
    '                .AddWithValue("Reason", instanceAbortedRecord.Reason)
    '            Else
    '                Dim suspendedRecord As WorkflowInstanceSuspendedRecord = TryCast(Record, WorkflowInstanceSuspendedRecord)
    '                If suspendedRecord IsNot Nothing Then
    '                    .AddWithValue("Reason", suspendedRecord.Reason)
    '                Else
    '                    .AddWithValue("Reason", "")
    '                End If
    '            End If
    '        End If
    '    End With

    '    Return cmd
    'End Function

    'Public Function CreateTrackingCommand(Record As ActivityStateRecord) As SqlClient.SqlCommand
    '    Dim cmd As New SqlClient.SqlCommand
    '    Dim sql As New StringBuilder
    '    With sql
    '        .AppendLine(" INSERT INTO CurationPool.dbo.CURATION_LEAD_TRACKING_LOG_ACTIVITY ")
    '        .AppendLine(" (WorkflowInstanceId,ActivityInstanceId,ActivityId,ActivityRecordType,ActivityName,State,RecordNumber,TraceLevelId,ActivityType,ArgumentsXml,VariablesXml,AnnotationsXml,CreatedTime) ")
    '        .AppendLine(" values ")
    '        .AppendLine(" (@WorkflowInstanceId,@ActivityInstanceId,@ActivityId,@ActivityRecordType,@ActivityName,@State,@RecordNumber,@TraceLevelId,@ActivityType,@ArgumentsXml,@VariablesXml,@AnnotationsXml,@CreatedTime) ")
    '    End With
    '    cmd.CommandText = sql.ToString
    '    With cmd.Parameters
    '        .AddWithValue("WorkflowInstanceId", Record.InstanceId)
    '        .AddWithValue("ActivityInstanceId", Record.Activity.InstanceId)
    '        .AddWithValue("ActivityId", Record.Activity.Id)
    '        .AddWithValue("ActivityRecordType", "ActivityStateRecord")
    '        .AddWithValue("ActivityName", Record.Activity.Name)
    '        .AddWithValue("State", Record.State)
    '        .AddWithValue("RecordNumber", Record.RecordNumber)
    '        .AddWithValue("TraceLevelId", Record.Level)
    '        .AddWithValue("ActivityType", Record.Activity.TypeName)
    '        .AddWithValue("ArgumentsXml", ArgumentsToString(Record.Arguments))
    '        .AddWithValue("VariablesXml", SerializeData(Record.Variables))
    '        .AddWithValue("AnnotationsXml", SerializeData(Record.Annotations))
    '        .AddWithValue("CreatedTime", Now)
    '    End With
    '    Return cmd
    'End Function

    'Public Function CreateTrackingCommand(Record As ActivityScheduledRecord) As SqlClient.SqlCommand
    '    Dim cmd As New SqlClient.SqlCommand
    '    Dim sql As New StringBuilder
    '    With sql
    '        .AppendLine(" INSERT INTO CurationPool.dbo.CURATION_LEAD_TRACKING_LOG_ACTIVITY ")
    '        .AppendLine(" (WorkflowInstanceId,ActivityInstanceId,ActivityId,ActivityRecordType,ActivityName,RecordNumber,TraceLevelId,ActivityType,AnnotationsXml,CreatedTime,ChildActivityName,ChildActivityId,ChildActivityInstanceId,ChildActivityType) ")
    '        .AppendLine(" values ")
    '        .AppendLine(" (@WorkflowInstanceId,@ActivityInstanceId,@ActivityId,@ActivityRecordType,@ActivityName,@RecordNumber,@TraceLevelId,@ActivityType,@AnnotationsXml,@CreatedTime,@ChildActivityName,@ChildActivityId,@ChildActivityInstanceId,@ChildActivityType) ")
    '    End With
    '    cmd.CommandText = sql.ToString
    '    With cmd.Parameters
    '        .AddWithValue("WorkflowInstanceId", Record.InstanceId)
    '        .AddWithValue("ActivityInstanceId", Record.Activity.InstanceId)
    '        .AddWithValue("ActivityId", Record.Activity.Id)
    '        .AddWithValue("ActivityRecordType", "ActivityScheduledRecord")
    '        .AddWithValue("ActivityName", Record.Activity.Name)
    '        .AddWithValue("RecordNumber", Record.RecordNumber)
    '        .AddWithValue("TraceLevelId", Record.Level)
    '        .AddWithValue("ActivityType", Record.Activity.TypeName)
    '        .AddWithValue("AnnotationsXml", "")
    '        .AddWithValue("CreatedTime", Now)
    '        .AddWithValue("ChildActivityName", Record.Child.Name)
    '        .AddWithValue("ChildActivityId", Record.Child.Id)
    '        .AddWithValue("ChildActivityInstanceId", Record.Child.InstanceId)
    '        .AddWithValue("ChildActivityType", Record.Child.TypeName)
    '    End With
    '    Return cmd
    'End Function

    'Public Function CreateTrackingCommand(Record As CustomTrackingRecord) As SqlClient.SqlCommand
    '    Dim cmd As New SqlClient.SqlCommand
    '    Dim sql As New StringBuilder
    '    With sql
    '        .AppendLine(" INSERT INTO CurationPool.dbo.CURATION_LEAD_TRACKING_LOG_ACTIVITY ")
    '        .AppendLine(" (WorkflowInstanceId,ActivityInstanceId,ActivityId,ActivityRecordType,ActivityName,RecordNumber,TraceLevelId,ActivityType,AnnotationsXml,CreatedTime,CustomRecordName,CustomRecordDataXml) ")
    '        .AppendLine(" values ")
    '        .AppendLine(" (@WorkflowInstanceId,@ActivityInstanceId,@ActivityId,@ActivityRecordType,@ActivityName,@RecordNumber,@TraceLevelId,@ActivityType,@AnnotationsXml,@CreatedTime,@CustomRecordName,@CustomRecordDataXml) ")
    '    End With
    '    cmd.CommandText = sql.ToString
    '    With cmd.Parameters
    '        .AddWithValue("WorkflowInstanceId", Record.InstanceId)
    '        .AddWithValue("ActivityInstanceId", Record.Activity.InstanceId)
    '        .AddWithValue("ActivityId", Record.Activity.Id)
    '        .AddWithValue("ActivityRecordType", "CustomTrackingRecord")
    '        .AddWithValue("ActivityName", Record.Activity.Name)
    '        .AddWithValue("RecordNumber", Record.RecordNumber)
    '        .AddWithValue("TraceLevelId", Record.Level)
    '        .AddWithValue("ActivityType", Record.Activity.TypeName)
    '        .AddWithValue("AnnotationsXml", "")
    '        .AddWithValue("CreatedTime", Now)
    '        .AddWithValue("CustomRecordName", Record.Name)
    '        .AddWithValue("CustomRecordDataXml", ArgumentsToString(Record.Data))
    '    End With
    '    Return cmd
    'End Function

    'Public Function CreateTrackingCommand(Record As FaultPropagationRecord) As SqlClient.SqlCommand
    '    Dim cmd As New SqlClient.SqlCommand
    '    Dim sql As New StringBuilder
    '    With sql
    '        .AppendLine(" INSERT INTO CurationPool.dbo.CURATION_LEAD_TRACKING_LOG_ACTIVITY ")
    '        .AppendLine(" (WorkflowInstanceId,ActivityInstanceId,ActivityId,ActivityRecordType,ActivityName,RecordNumber,TraceLevelId,ActivityType,AnnotationsXml,CreatedTime,FaultDetails,FaultHandlerActivityName,FaultHandlerActivityId,FaultHandlerActivityInstanceId,FaultHandlerActivityType) ")
    '        .AppendLine(" values ")
    '        .AppendLine(" (@WorkflowInstanceId,@ActivityInstanceId,@ActivityId,@ActivityRecordType,@ActivityName,@RecordNumber,@TraceLevelId,@ActivityType,@AnnotationsXml,@CreatedTime,@FaultDetails,@FaultHandlerActivityName,@FaultHandlerActivityId,@FaultHandlerActivityInstanceId,@FaultHandlerActivityType) ")
    '    End With
    '    cmd.CommandText = sql.ToString
    '    With cmd.Parameters
    '        .AddWithValue("WorkflowInstanceId", Record.InstanceId)
    '        .AddWithValue("ActivityInstanceId", Record.FaultSource.InstanceId)
    '        .AddWithValue("ActivityId", Record.FaultSource.Id)
    '        .AddWithValue("ActivityRecordType", "FaultPropagationRecord")
    '        .AddWithValue("ActivityName", Record.FaultSource.Name)
    '        .AddWithValue("RecordNumber", Record.RecordNumber)
    '        .AddWithValue("TraceLevelId", Record.Level)
    '        .AddWithValue("ActivityType", Record.FaultSource.TypeName)
    '        .AddWithValue("AnnotationsXml", "")
    '        .AddWithValue("CreatedTime", Now)
    '        .AddWithValue("FaultDetails", Record.Fault.ToString)
    '        If Record.FaultHandler IsNot Nothing Then
    '            .AddWithValue("FaultHandlerActivityName", Record.FaultHandler.Name)
    '            .AddWithValue("FaultHandlerActivityId", Record.FaultHandler.Id)
    '            .AddWithValue("FaultHandlerActivityInstanceId", Record.FaultHandler.InstanceId)
    '            .AddWithValue("FaultHandlerActivityType", Record.FaultHandler.TypeName)
    '        Else
    '            .AddWithValue("FaultHandlerActivityName", "")
    '            .AddWithValue("FaultHandlerActivityId", "")
    '            .AddWithValue("FaultHandlerActivityInstanceId", "")
    '            .AddWithValue("FaultHandlerActivityType", "")
    '        End If
    '    End With
    '    Return cmd
    'End Function

    Public Function ArgumentsToString(ByVal data As IDictionary(Of String, Object)) As String
        If data IsNot Nothing AndAlso data.Count > 0 Then
            Dim sb As New StringBuilder
            sb.Append("<Arguments>")
            For Each argument In data
                'If argument.Key = "Value" OrElse argument.Key = "Result" Then
                If TryCast(argument.Value, List(Of UNICALeadsFlow.SSO.LDRBU)) IsNot Nothing Then
                    sb.AppendFormat("<Name Id='ListOfHighestSBUs'>")
                    For Each bu In CType(argument.Value, List(Of UNICALeadsFlow.SSO.LDRBU))
                        If bu = SSO.LDRBU.ESG Then
                            sb.AppendFormat("<Value>ISG</Value>")
                        ElseIf bu = SSO.LDRBU.Service Then
                            sb.AppendFormat("<Value>Retail</Value>")
                        Else
                            sb.AppendFormat("<Value>{0}</Value>", bu.ToString)
                        End If

                    Next
                    sb.AppendFormat("</Name>")
                ElseIf TryCast(argument.Value, List(Of UNICALeadsFlow.LDR)) IsNot Nothing Then
                    sb.AppendFormat("<Name Id='LDR'>")
                    For Each ldr In CType(argument.Value, List(Of UNICALeadsFlow.LDR))
                        sb.AppendFormat("<Value>{0}</Value>", ldr.LDR_EMAIL + "|" + ldr.LDR_LOGIN_NAME + "|" + ldr.POSITION_NAME)
                    Next
                    sb.AppendFormat("</Name>")
                ElseIf TryCast(argument.Value, [Enum]) IsNot Nothing Then
                    sb.AppendFormat("<Name Id='FinalSBU'>")
                    Dim bu As UNICALeadsFlow.SSO.LDRBU = CType(argument.Value, UNICALeadsFlow.SSO.LDRBU)
                    If bu = SSO.LDRBU.ESG Then
                        sb.AppendFormat("<Value>ISG</Value>")
                    ElseIf bu = SSO.LDRBU.Service Then
                        sb.AppendFormat("<Value>Retail</Value>")
                    Else
                        sb.AppendFormat("<Value>{0}</Value>", bu.ToString)
                    End If
                    sb.AppendFormat("</Name>")
                ElseIf TryCast(argument.Value, List(Of UNICALeadsFlow.SELECT_TOPIC)) IsNot Nothing Then
                    sb.AppendFormat("<Name Id='ListOfSelectTopics'>")
                    For Each t In CType(argument.Value, List(Of UNICALeadsFlow.SELECT_TOPIC))
                        sb.AppendFormat("<Value>{0}</Value>", t.Topic)
                    Next
                    sb.AppendFormat("</Name>")
                ElseIf TryCast(argument.Value, UNICALeadsFlow.Account) IsNot Nothing Then
                    Dim acc As Account = TryCast(argument.Value, Account)
                    sb.AppendFormat("<Name Id='AccountInfo'>")
                    sb.AppendFormat("<Value>{0}</Value>", acc.AccountName + " (" + acc.RowID + ")")
                    sb.AppendFormat("</Name>")
                ElseIf TryCast(argument.Value, list(Of UNICALeadsFlow.CheckPositionObject)) IsNot Nothing Then
                    Dim ValidPositions As List(Of CheckPositionObject) = CType(argument.Value, List(Of CheckPositionObject)).Where(Function(x) x.IsValid = True).ToList()
                    Dim InvalidPositions As List(Of CheckPositionObject) = CType(argument.Value, List(Of CheckPositionObject)).Where(Function(x) x.IsValid = False).ToList()
                    sb.AppendFormat("<Name Id='PositionValidation'>")
                    If ValidPositions IsNot Nothing AndAlso ValidPositions.Count > 0 Then
                        sb.AppendFormat("<Value>Valid Position: {0}</Value>", String.Join(",", ValidPositions.Select(Function(x) x.PositionName).ToArray()))
                    End If
                    If InvalidPositions IsNot Nothing AndAlso InvalidPositions.Count > 0 Then
                        sb.AppendFormat("<Value>Invalid Position: {0}</Value>", String.Join(",", InvalidPositions.Select(Function(x) x.PositionName).ToArray()))
                    End If
                    sb.AppendFormat("</Name>")
                ElseIf TryCast(argument.Value, List(Of UNICALeadsFlow.Lead_Point)) IsNot Nothing Then
                    sb.AppendFormat("<Name Id='Lead_Point'>")
                    Dim SBUName As New List(Of String)
                    For Each point In CType(argument.Value, List(Of UNICALeadsFlow.Lead_Point))
                        If Not SBUName.Contains(point.SBU) Then SBUName.Add(point.SBU)
                    Next
                    For Each sbu As String In SBUName
                        sb.AppendFormat("<Value>{0}</Value>", sbu + ":" + LRBiz.GetSBUPoint(CType(argument.Value, List(Of UNICALeadsFlow.Lead_Point)), sbu).ToString)
                    Next
                    sb.AppendFormat("</Name>")
                Else
                    sb.AppendFormat("<Name Id='{0}'><Value>{1}</Value></Name>", argument.Key, argument.Value)
                End If
                'End If
            Next
            sb.Append("</Arguments>")
            Return sb.ToString
        End If
        Return ""
    End Function

    Public Function SerializeData(ByVal data As Object) As String
        'If data IsNot Nothing AndAlso data.Count > 0 Then
        Dim sb As New StringBuilder
        Dim settings As New XmlWriterSettings
        settings.OmitXmlDeclaration = True

        Using writer As XmlWriter = XmlWriter.Create(sb, settings)
            Dim dataSerializer As New NetDataContractSerializer
            Try
                dataSerializer.WriteObject(writer, data)
            Catch ex As Exception

            End Try
            writer.Flush()
            Return sb.ToString
        End Using
        'Else
        'Return String.Empty
        'End If
    End Function

    Public Sub New()
        Me.WorkflowList = New List(Of CURATION_LEAD_TRACKING_LOG_WORKFLOW)
        Me.ActivityList = New List(Of CURATION_LEAD_TRACKING_LOG_ACTIVITY)
    End Sub
End Class

End Namespace  

