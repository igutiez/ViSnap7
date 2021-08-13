Imports System.ComponentModel

Module BackgroudTasksLaunch
    Public WithEvents cyclicLoop As New BackgroundWorker
    Public contador As Integer


    Private Sub CyclicLoop_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles CyclicLoop.DoWork
        'Tasks in the backgroud for reading/writing in the PLC
        BackgroundTasks.AccomodatePlcData()
        BackgroundTasks.FillPlcData()


    End Sub

    Private Sub CyclicLoop_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles CyclicLoop.RunWorkerCompleted
        'Task in the background for updating the controls
        If firstExecution Then
            udateControls = True
        End If
        UpdateFormsTasks.ReadControls(udateControls)
        cyclicLoop.RunWorkerAsync()
        'Terminated the first execution
        firstExecution = False
    End Sub
End Module
