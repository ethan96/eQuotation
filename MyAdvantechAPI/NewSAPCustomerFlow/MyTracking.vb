Imports System.Activities.Tracking
Imports System.Text
Imports System.Xml
Imports System.Runtime.Serialization
Imports System.Configuration
Public Class MyTracking
    Inherits TrackingParticipant

    Protected Overrides Sub Track(record As TrackingRecord, timeout As TimeSpan)
        Dim conn As New SqlClient.SqlConnection("Data Source=aclecampaign\MATEST;Initial Catalog=MyWorkflows;Persist Security Info=True;User ID=b2bsa;Password=@dvantech!")
        If conn.State <> ConnectionState.Open Then conn.Open()
        Dim cmd As New SqlClient.SqlCommand

        Dim workflowInstanceRecord As WorkflowInstanceRecord = TryCast(record, WorkflowInstanceRecord)
        If workflowInstanceRecord IsNot Nothing Then
            cmd = CreateTrackingCommand(workflowInstanceRecord)
        End If

        Dim activityStateRecord As ActivityStateRecord = TryCast(record, ActivityStateRecord)
        If activityStateRecord IsNot Nothing Then
            cmd = CreateTrackingCommand(activityStateRecord)
        End If

        Dim faultPropagationRecord As FaultPropagationRecord = TryCast(record, FaultPropagationRecord)
        If faultPropagationRecord IsNot Nothing Then
            cmd = CreateTrackingCommand(faultPropagationRecord)
        End If

        Dim customTrackingRecord As CustomTrackingRecord = TryCast(record, CustomTrackingRecord)
        If customTrackingRecord IsNot Nothing Then
            cmd = CreateTrackingCommand(customTrackingRecord)
        End If

        cmd.Connection = conn : cmd.CommandTimeout = 60
        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Console.Write(ex.Message)
        End Try
    End Sub

    Public Function CreateTrackingCommand(Record As WorkflowInstanceRecord) As SqlClient.SqlCommand
        Dim cmd As New SqlClient.SqlCommand
        Dim sql As New StringBuilder
        With sql
            .AppendLine(" INSERT INTO MY_TRACKING_LOG_WORKFLOW values ")
            .AppendLine(" (@WorkflowInstanceId,@WorkflowActivityDefinition,@RecordNumber,@State,@TraceLevelId,@AnnotationsXml,@TimeCreated,@ExceptionDetails,@Reason) ")
        End With
        cmd.CommandText = sql.ToString
        With cmd.Parameters
            .AddWithValue("WorkflowInstanceId", Record.InstanceId)
            .AddWithValue("WorkflowActivityDefinition", Record.ActivityDefinitionId)
            .AddWithValue("RecordNumber", Record.RecordNumber)
            .AddWithValue("State", Record.State)
            .AddWithValue("TraceLevelId", Record.Level)
            .AddWithValue("AnnotationsXml", "")
            .AddWithValue("TimeCreated", Record.EventTime)
            Dim unhandledExceptionRecord As WorkflowInstanceUnhandledExceptionRecord = TryCast(Record, WorkflowInstanceUnhandledExceptionRecord)
            If unhandledExceptionRecord IsNot Nothing Then
                .AddWithValue("ExceptionDetails", unhandledExceptionRecord.UnhandledException.ToString)
            Else
                .AddWithValue("ExceptionDetails", "")
            End If
            Dim terminatedRecord As WorkflowInstanceTerminatedRecord = TryCast(Record, WorkflowInstanceTerminatedRecord)
            If terminatedRecord IsNot Nothing Then
                .AddWithValue("Reason", terminatedRecord.Reason)
            Else
                Dim instanceAbortedRecord As WorkflowInstanceAbortedRecord = TryCast(Record, WorkflowInstanceAbortedRecord)
                If instanceAbortedRecord IsNot Nothing Then
                    .AddWithValue("Reason", instanceAbortedRecord.Reason)
                Else
                    Dim suspendedRecord As WorkflowInstanceSuspendedRecord = TryCast(Record, WorkflowInstanceSuspendedRecord)
                    If suspendedRecord IsNot Nothing Then
                        .AddWithValue("Reason", suspendedRecord.Reason)
                    Else
                        .AddWithValue("Reason", "")
                    End If
                End If
            End If
        End With
        Return cmd
    End Function

    Public Function CreateTrackingCommand(Record As ActivityStateRecord) As SqlClient.SqlCommand
        Dim cmd As New SqlClient.SqlCommand
        Dim sql As New StringBuilder
        With sql
            .AppendLine(" INSERT INTO MY_TRACKING_LOG_ACTIVITY ")
            .AppendLine(" (WorkflowInstanceId,ActivityInstanceId,ActivityId,ActivityRecordType,ActivityName,State,RecordNumber,TraceLevelId,ActivityType,CreatedTime) ")
            .AppendLine(" values ")
            .AppendLine(" (@WorkflowInstanceId,@ActivityInstanceId,@ActivityId,@ActivityRecordType,@ActivityName,@State,@RecordNumber,@TraceLevelId,@ActivityType,@CreatedTime) ")
        End With
        cmd.CommandText = sql.ToString
        With cmd.Parameters
            .AddWithValue("WorkflowInstanceId", Record.InstanceId)
            .AddWithValue("ActivityInstanceId", Record.Activity.InstanceId)
            .AddWithValue("ActivityId", Record.Activity.Id)
            .AddWithValue("ActivityRecordType", "ActivityStateRecord")
            .AddWithValue("ActivityName", Record.Activity.Name)
            .AddWithValue("State", Record.State)
            .AddWithValue("RecordNumber", Record.RecordNumber)
            .AddWithValue("TraceLevelId", Record.Level)
            .AddWithValue("ActivityType", Record.Activity.TypeName)
            .AddWithValue("CreatedTime", Record.EventTime)
        End With
        Return cmd
    End Function

    Public Function CreateTrackingCommand(Record As CustomTrackingRecord) As SqlClient.SqlCommand
        Dim cmd As New SqlClient.SqlCommand
        Dim sql As New StringBuilder
        With sql
            .AppendLine(" INSERT INTO MY_TRACKING_LOG_ACTIVITY ")
            .AppendLine(" (WorkflowInstanceId,ActivityInstanceId,ActivityId,ActivityRecordType,ActivityName,RecordNumber,TraceLevelId,ActivityType,AnnotationsXml,CreatedTime,CustomRecordName,CustomRecordDataXml) ")
            .AppendLine(" values ")
            .AppendLine(" (@WorkflowInstanceId,@ActivityInstanceId,@ActivityId,@ActivityRecordType,@ActivityName,@RecordNumber,@TraceLevelId,@ActivityType,@AnnotationsXml,@CreatedTime,@CustomRecordName,@CustomRecordDataXml) ")
        End With
        cmd.CommandText = sql.ToString
        With cmd.Parameters
            .AddWithValue("WorkflowInstanceId", Record.InstanceId)
            .AddWithValue("ActivityInstanceId", Record.Activity.InstanceId)
            .AddWithValue("ActivityId", Record.Activity.Id)
            .AddWithValue("ActivityRecordType", "CustomTrackingRecord")
            .AddWithValue("ActivityName", Record.Activity.Name)
            .AddWithValue("RecordNumber", Record.RecordNumber)
            .AddWithValue("TraceLevelId", Record.Level)
            .AddWithValue("ActivityType", Record.Activity.TypeName)
            .AddWithValue("AnnotationsXml", "")
            .AddWithValue("CreatedTime", Record.EventTime)
            .AddWithValue("CustomRecordName", Record.Name)
            .AddWithValue("CustomRecordDataXml", Nothing)
        End With
        Return cmd
    End Function
    Public Function CreateTrackingCommand(Record As FaultPropagationRecord) As SqlClient.SqlCommand
        Dim cmd As New SqlClient.SqlCommand
        Dim sql As New StringBuilder
        With sql
            .AppendLine(" INSERT INTO MY_TRACKING_LOG_ACTIVITY ")
            .AppendLine(" (WorkflowInstanceId,ActivityInstanceId,ActivityId,ActivityRecordType,ActivityName,RecordNumber,TraceLevelId,ActivityType,AnnotationsXml,CreatedTime,FaultDetails,FaultHandlerActivityName,FaultHandlerActivityId,FaultHandlerActivityInstanceId,FaultHandlerActivityType) ")
            .AppendLine(" values ")
            .AppendLine(" (@WorkflowInstanceId,@ActivityInstanceId,@ActivityId,@ActivityRecordType,@ActivityName,@RecordNumber,@TraceLevelId,@ActivityType,@AnnotationsXml,@CreatedTime,@FaultDetails,@FaultHandlerActivityName,@FaultHandlerActivityId,@FaultHandlerActivityInstanceId,@FaultHandlerActivityType) ")
        End With
        cmd.CommandText = sql.ToString
        With cmd.Parameters
            .AddWithValue("WorkflowInstanceId", Record.InstanceId)
            .AddWithValue("ActivityInstanceId", Record.FaultSource.InstanceId)
            .AddWithValue("ActivityId", Record.FaultSource.Id)
            .AddWithValue("ActivityRecordType", "FaultPropagationRecord")
            .AddWithValue("ActivityName", Record.FaultSource.Name)
            .AddWithValue("RecordNumber", Record.RecordNumber)
            .AddWithValue("TraceLevelId", Record.Level)
            .AddWithValue("ActivityType", Record.FaultSource.TypeName)
            .AddWithValue("AnnotationsXml", "")
            .AddWithValue("CreatedTime", Record.EventTime)
            .AddWithValue("FaultDetails", Record.Fault.ToString)
            If Record.FaultHandler IsNot Nothing Then
                .AddWithValue("FaultHandlerActivityName", Record.FaultHandler.Name)
                .AddWithValue("FaultHandlerActivityId", Record.FaultHandler.Id)
                .AddWithValue("FaultHandlerActivityInstanceId", Record.FaultHandler.InstanceId)
                .AddWithValue("FaultHandlerActivityType", Record.FaultHandler.TypeName)
            Else
                .AddWithValue("FaultHandlerActivityName", "")
                .AddWithValue("FaultHandlerActivityId", "")
                .AddWithValue("FaultHandlerActivityInstanceId", "")
                .AddWithValue("FaultHandlerActivityType", "")
            End If
        End With
        Return cmd
    End Function

End Class
