Public Class VS7_NumericKeyboard

    Public VS7textbox As VS7_Textbox

    Sub New(ByRef txtbox As VS7_Textbox)

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
        VS7textbox = txtbox
        Me.ButtonDot.Enabled = False
        Me.ButtonDot.Text = culture.NumberFormat.NumberDecimalSeparator

    End Sub
    Private Sub VS7_NumericKeyboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        With DirectCast(VS7textbox, VS7_Textbox)
            .controlFocused = True
            If .PLC_DataType = DataType.REAL Then
                Me.ButtonDot.Enabled = True
            End If
            Me.Text = "Write a " + .PLC_DataType.ToString
        End With
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'AppActivate(My.Application.Info.AssemblyName)
        WriteOnForm("1")
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        WriteOnForm("2")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        WriteOnForm("3")
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        WriteOnForm("4")
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        WriteOnForm("5")
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        WriteOnForm("6")
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        WriteOnForm("7")
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        WriteOnForm("8")
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        WriteOnForm("9")
    End Sub

    Private Sub Button0_Click(sender As Object, e As EventArgs) Handles Button0.Click
        WriteOnForm("0")
    End Sub

    Private Sub ButtonDot_Click(sender As Object, e As EventArgs) Handles ButtonDot.Click
        If IsNumeric(VS7textbox.Text) And (Not VS7textbox.Text.Contains(culture.NumberFormat.NumberDecimalSeparator)) Then
            WriteOnForm(culture.NumberFormat.NumberDecimalSeparator)
        End If

    End Sub

    Private Sub ButtonNeg_Click(sender As Object, e As EventArgs) Handles ButtonNeg.Click
        If VS7textbox.Text.Length = 0 Then
            WriteOnForm("-")
        ElseIf IsNumeric(VS7textbox.Text) Then
            If VS7textbox.Text = 0 Then
                VS7textbox.Text = "-"
            End If
        End If
    End Sub

    Private Sub ButtonBackSpace_Click(sender As Object, e As EventArgs) Handles ButtonBackSpace.Click
        If VS7textbox.Text.Length > 0 Then
            VS7textbox.Text = Mid(VS7textbox.Text, 1, VS7textbox.Text.Length - 1)
        End If
    End Sub



    Sub WriteOnForm(ByVal value As String)
        With VS7textbox
            .controlFocused = True
        End With

        VS7textbox.Text = VS7textbox.Text + value
        If IsNumeric(VS7textbox.Text) And UCase(value) <> culture.NumberFormat.NumberDecimalSeparator Then
            VS7textbox.Text = CDbl(VS7textbox.Text)
        End If
    End Sub

    Private Sub ButtonEnter_Click(sender As Object, e As EventArgs) Handles ButtonEnter.Click
        With VS7textbox
            If Not .PLC_FormActive Then
                .pendingWrite = True
            End If
            .controlFocused = False
        End With

        Me.Close()

    End Sub

    Private Sub VS7_NumericKeyboard_Deactivate(sender As Object, e As EventArgs) Handles Me.Deactivate
        Me.Close()

    End Sub

    Private Sub ButtonClear_Click(sender As Object, e As EventArgs) Handles ButtonClear.Click
        VS7textbox.Text = 0
    End Sub

    Private Sub ButtonPlus1_Click(sender As Object, e As EventArgs) Handles ButtonPlus1.Click
        Aritmethic(1)
    End Sub

    Sub Aritmethic(ByVal value As Double)
        If IsNumeric(VS7textbox.Text) Then
            VS7textbox.Text = CStr(CDbl(VS7textbox.Text) + value)
        End If
    End Sub

    Private Sub ButtonPlus10_Click(sender As Object, e As EventArgs) Handles ButtonPlus10.Click
        Aritmethic(10)

    End Sub

    Private Sub ButtonMinus1_Click(sender As Object, e As EventArgs) Handles ButtonMinus1.Click
        Aritmethic(-1)

    End Sub

    Private Sub ButtonMinus10_Click(sender As Object, e As EventArgs) Handles ButtonMinus10.Click
        Aritmethic(-10)

    End Sub

    Private Sub VS7_NumericKeyboard_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        BackgroundTasks.InibitUpdateControls = True

    End Sub
End Class