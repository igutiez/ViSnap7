Imports System.ComponentModel

Module BackgroudTasksLaunch
    Public thisLoop As Date
    Public timeBetweenLoops As Integer
    Public timeLastLoop As Integer
    Public loopError As Integer
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
        Dim sleepTime As Double
        'Task in the background for updating the controls
        'It is made just at the firts loop
        If firstExecution Then
            udateControls = True
        End If
        'Read all active controls
        UpdateFormsTasks.UpdateControls(udateControls)
        'Terminated the first execution
        firstExecution = False

        timeBetweenLoops = CInt(Now.TimeOfDay.TotalMilliseconds - thisLoop.TimeOfDay.TotalMilliseconds)
        sleepTime = KReadingIntervalMiliseconds - timeBetweenLoops - loopError
        If sleepTime < 0 Then
            sleepTime = 0
        End If
        'Delay the reading the desired interval
        Threading.Thread.Sleep(sleepTime)
        timeLastLoop = CInt(Now.TimeOfDay.TotalMilliseconds - thisLoop.TimeOfDay.TotalMilliseconds)
        loopError = Math.Abs(timeLastLoop - KReadingIntervalMiliseconds)
        thisLoop = Now

        'Next loop launch
        CyclicLoop.RunWorkerAsync()

    End Sub
End Module
