Public Class SimpleLogin
    Private Sub LoginSimple_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.PlC_GestorUsuarios1.ShowEditor = False

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If ActiveUserLogin Then
            Me.Close()
        End If
    End Sub
End Class