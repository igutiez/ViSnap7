Public Class VS7_AlphanumericKeyboard
    Public VS7textbox As VS7_Textbox
    Private _upper As Boolean
    Private _caps As Boolean
    Sub New(ByRef txtbox As VS7_Textbox)

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
        VS7textbox = txtbox
        'Me.ButtonDot.Enabled = False
        ' Me.ButtonDot.Text = culture.NumberFormat.NumberDecimalSeparator

    End Sub
    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click

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

    End Sub

    Private Sub VS7_AlphanumericKeyboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub





    Sub WriteOnForm(ByVal value As String)
        With VS7textbox
            .controlFocused = True
        End With
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
        WriteOnForm("u")

    End Sub
End Class