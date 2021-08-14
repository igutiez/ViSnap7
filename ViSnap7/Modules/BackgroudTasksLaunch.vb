Imports System.ComponentModel

Module BackgroudTasksLaunch
    ''' <summary>
    ''' Object that makes all communication task in the background
    ''' </summary>
    Public WithEvents CyclicLoop As New BackgroundWorker

    ''' <summary>
    ''' Tasks for loading data from PLC
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CyclicLoop_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles CyclicLoop.DoWork
        'Tasks in the backgroud for reading/writing in the PLC
        BackgroundTasks.AccomodatePlcData()
        BackgroundTasks.FillPlcData()
    End Sub

    ''' <summary>
    ''' Once the data from the PLCs has been read, the controls update tasks are carried out
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CyclicLoop_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles CyclicLoop.RunWorkerCompleted
        'Task in the background for updating the controls
        'It is made just at the firts loop
        If firstExecution Then
            udateControls = True
        End If
        'Read all active controls
        UpdateFormsTasks.ReadControls(udateControls)
        'Next loop launch
        CyclicLoop.RunWorkerAsync()
        'Terminated the first execution
        firstExecution = False
    End Sub
End Module
