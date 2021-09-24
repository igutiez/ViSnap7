Public Class VS7_AlphanumericKeyboard
    Public VS7textbox As VS7_Textbox
    Private _upper As Boolean
    Private _caps As Boolean
    Sub New(ByRef txtbox As VS7_Textbox)

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
        VS7textbox = txtbox

    End Sub
    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        WriteOnForm("r")
    End Sub

    Private Sub Button37_Click(sender As Object, e As EventArgs) Handles Button37.Click
        If Not _upper Then
            ButtonsToUpperCase()
            _upper = True
        ElseIf Not _caps Then
            _caps = True
            Button37.Image = My.Resources.caps_lock
        Else
            _caps = False
            _upper = False
            Button37.Image = My.Resources.icons8_shift_48
            ButtonsToLowerCase()
        End If

    End Sub
    Sub ButtonsToUpperCase()
        For Each ctr As Control In Me.Controls
            If ctr.GetType Is GetType(Button) Then
                ctr.Text = ctr.Text.ToUpper()

            End If
        Next
    End Sub
    Sub ButtonsToLowerCase()
        For Each ctr As Control In Me.Controls
            If ctr.GetType Is GetType(Button) Then
                ctr.Text = ctr.Text.ToLower()
            End If
        Next
    End Sub
    Private Sub Button40_Click(sender As Object, e As EventArgs) Handles Button40.Click
        VS7textbox.Text = ""
    End Sub

    Private Sub VS7_AlphanumericKeyboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub





    Sub WriteOnForm(ByVal value As String)
        VS7textbox.controlFocused = True

        If _upper Then
            VS7textbox.Text = VS7textbox.Text + UCase(value)
        Else
            VS7textbox.Text = VS7textbox.Text + LCase(value)
        End If
        If _upper And Not _caps Then
            _upper = False
            ButtonsToLowerCase()
        End If
    End Sub

    Private Sub Button50_Click(sender As Object, e As EventArgs) Handles Button50.Click
        WriteOnForm("1")

    End Sub

    Private Sub Button49_Click(sender As Object, e As EventArgs) Handles Button49.Click
        WriteOnForm("2")

    End Sub

    Private Sub Button48_Click(sender As Object, e As EventArgs) Handles Button48.Click
        WriteOnForm("3")

    End Sub

    Private Sub Button47_Click(sender As Object, e As EventArgs) Handles Button47.Click
        WriteOnForm("4")

    End Sub

    Private Sub Button46_Click(sender As Object, e As EventArgs) Handles Button46.Click
        WriteOnForm("5")

    End Sub

    Private Sub Button45_Click(sender As Object, e As EventArgs) Handles Button45.Click
        WriteOnForm("6")

    End Sub

    Private Sub Button44_Click(sender As Object, e As EventArgs) Handles Button44.Click
        WriteOnForm("7")

    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        WriteOnForm("8")

    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        WriteOnForm("9")

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        WriteOnForm("0")

    End Sub

    Private Sub Button19_Click(sender As Object, e As EventArgs) Handles Button19.Click
        WriteOnForm("q")

    End Sub

    Private Sub Button18_Click(sender As Object, e As EventArgs) Handles Button18.Click
        WriteOnForm("w")

    End Sub

    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        WriteOnForm("e")
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        WriteOnForm("t")
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        WriteOnForm("y")
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        WriteOnForm("u")
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        WriteOnForm("i")
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        WriteOnForm("o")
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        WriteOnForm("p")
    End Sub

    Private Sub Button27_Click(sender As Object, e As EventArgs) Handles Button27.Click
        WriteOnForm("a")
    End Sub

    Private Sub Button26_Click(sender As Object, e As EventArgs) Handles Button26.Click
        WriteOnForm("s")
    End Sub

    Private Sub Button25_Click(sender As Object, e As EventArgs) Handles Button25.Click
        WriteOnForm("d")
    End Sub

    Private Sub Button24_Click(sender As Object, e As EventArgs) Handles Button24.Click
        WriteOnForm("f")
    End Sub

    Private Sub Button23_Click(sender As Object, e As EventArgs) Handles Button23.Click
        WriteOnForm("g")
    End Sub

    Private Sub Button22_Click(sender As Object, e As EventArgs) Handles Button22.Click
        WriteOnForm("h")
    End Sub

    Private Sub Button21_Click(sender As Object, e As EventArgs) Handles Button21.Click
        WriteOnForm("j")
    End Sub

    Private Sub Button20_Click(sender As Object, e As EventArgs) Handles Button20.Click
        WriteOnForm("k")
    End Sub

    Private Sub Button28_Click(sender As Object, e As EventArgs) Handles Button28.Click
        WriteOnForm("l")
    End Sub

    Private Sub Button43_Click(sender As Object, e As EventArgs) Handles Button43.Click
        WriteOnForm("ñ")
    End Sub

    Private Sub Button36_Click(sender As Object, e As EventArgs) Handles Button36.Click
        WriteOnForm("z")
    End Sub

    Private Sub Button35_Click(sender As Object, e As EventArgs) Handles Button35.Click
        WriteOnForm("x")
    End Sub

    Private Sub Button34_Click(sender As Object, e As EventArgs) Handles Button34.Click
        WriteOnForm("c")
    End Sub

    Private Sub Button33_Click(sender As Object, e As EventArgs) Handles Button33.Click
        WriteOnForm("v")
    End Sub

    Private Sub Button32_Click(sender As Object, e As EventArgs) Handles Button32.Click
        WriteOnForm("b")
    End Sub

    Private Sub Button31_Click(sender As Object, e As EventArgs) Handles Button31.Click
        WriteOnForm("n")
    End Sub

    Private Sub Button30_Click(sender As Object, e As EventArgs) Handles Button30.Click
        WriteOnForm("m")
    End Sub

    Private Sub Button42_Click(sender As Object, e As EventArgs) Handles Button42.Click
        WriteOnForm(";")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        WriteOnForm(":")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        WriteOnForm(".")
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        WriteOnForm("_")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        WriteOnForm("-")
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        WriteOnForm("/")
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        WriteOnForm("#")
    End Sub

    Private Sub Button38_Click(sender As Object, e As EventArgs) Handles Button38.Click
        WriteOnForm(" ")
    End Sub

    Private Sub KeyboardDeactivate(sender As Object, e As EventArgs) Handles Me.Deactivate
        VS7textbox.controlFocused = False
        Me.Close()

    End Sub

    Private Sub Button39_Click(sender As Object, e As EventArgs) Handles Button39.Click
        With VS7textbox
            If Not .PLC_FormActive Then
                .pendingWrite = True
            End If
            .controlFocused = False
        End With

        Me.Close()

    End Sub

    Private Sub Button29_Click(sender As Object, e As EventArgs) Handles Button29.Click
        If VS7textbox.Text.Length > 0 Then
            VS7textbox.Text = Mid(VS7textbox.Text, 1, VS7textbox.Text.Length - 1)
        End If
    End Sub

    Private Sub Button41_Click(sender As Object, e As EventArgs) Handles Button41.Click
        WriteOnForm("?")
    End Sub

    Private Sub VS7_AlphanumericKeyboard_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        BackgroundTasks.InibitUpdateControls = True
    End Sub
End Class