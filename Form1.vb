
Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'DO NOT CHANGE THIS LINE
        General.LaunchCommunications()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        VS7_AlarmsController1.AckAlarms()
    End Sub
End Class
