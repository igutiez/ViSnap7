Public Class Main
    Private Home As New FrmHome
    Private Page1 As New FrmPage1
    Private ColorActive As Color = Color.FromName("ActiveCaption")
    Private ColorInactive As Color = Color.FromName("ButtonFace")
    Private ColorBackGround As Color = Color.FromName("DimGray")
    Private ColorLateralMenu As Color = Color.FromName("GradientInactiveCaption")
    Private Declare Auto Function SetWindowLong Lib "User32.Dll" (ByVal hWnd As IntPtr, ByVal nIndex As Integer, ByVal dwNewLong As Integer) As Integer
    Private Declare Auto Function GetWindowLong Lib "User32.Dll" (ByVal hWnd As System.IntPtr, ByVal nIndex As Integer) As Integer
    Private Const GWL_EXSTYLE = (-20)
    Private Const WS_EX_CLIENTEDGE = &H200
    Private X, Y As Single

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'MAXIMIZE WINDOW
        'Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        'Me.WindowState = FormWindowState.Maximized

        'DO NOT CHANGE THIS LINE
        General.LaunchCommunications()

        'BACKGROUND MENU COLOR
        Me.PanelMenu.BackColor = ColorLateralMenu


        'CHILD FORMS
        'Home 
        ChildConfig(Home)
        'Page2
        ChildConfig(Page1)

        'SHOW FORMS
        Page1.Show()
        Home.Show()

        'SHOW FIRST FORM
        Home.BringToFront()
        ButtonActiveColoration(BtnHome)

        ' SET BACKGROUND COLOR AND REMOVE BORDER FROM MDICLIENT CONTROL
        For Each c As Control In Me.Controls()
            If TypeOf (c) Is MdiClient Then
                c.BackColor = ColorBackGround
                Dim windowLong As Integer = GetWindowLong(c.Handle, GWL_EXSTYLE)
                windowLong = windowLong And (Not WS_EX_CLIENTEDGE)
                SetWindowLong(c.Handle, GWL_EXSTYLE, windowLong)
                c.Width = c.Width + 1
                Exit For
            End If
        Next
        X = Me.Width
        Y = Me.Height
        setTag(Me)
    End Sub

    Private Sub ChildConfig(ByRef frm As Form)
        frm.MdiParent = Me
        frm.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        frm.ControlBox = False
        frm.MaximizeBox = False
        frm.MinimizeBox = False
        frm.ShowIcon = False
        frm.Text = ""
        frm.Dock = DockStyle.Fill

    End Sub

    Private Sub ButtonActiveColoration(btn As Button)

        Me.BtnHome.BackColor = ColorInactive
        Me.BtnPage1.BackColor = ColorInactive

        btn.BackColor = ColorActive

    End Sub

    Private Sub BtnHome_Click(sender As Object, e As EventArgs) Handles BtnHome.Click
        Me.Home.BringToFront()
        ButtonActiveColoration(BtnHome)
    End Sub

    Private Sub BtnPage1_Click(sender As Object, e As EventArgs) Handles BtnPage1.Click
        Me.Page1.BringToFront()
        ButtonActiveColoration(BtnPage1)
    End Sub

    Private Sub setTag(ByVal cons As Control)
        For Each con As Control In cons.Controls
            con.Tag = con.Width & ":" & con.Height & ":" & con.Left & ":" & con.Top & ":" & con.Font.Size
        Next
    End Sub

    Private Sub setControls(ByVal newX As Single, ByVal newY As Single, ByVal cons As Control)
        For Each con As Control In cons.Controls
            Dim mytag As String() = con.Tag.ToString().Split(New Char() {":"c})
            Dim a As Single = Convert.ToSingle(mytag(0)) * newX
            con.Width = CInt((a))
            a = Convert.ToSingle(mytag(1)) * newY
            con.Height = CInt((a))
            a = Convert.ToSingle(mytag(2)) * newX
            con.Left = CInt((a))
            a = Convert.ToSingle(mytag(3)) * newY
            con.Top = CInt((a))
            Dim currentSize As Single = Convert.ToSingle(mytag(4)) * newY
            con.Font = New Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit)
        Next
    End Sub

    Private Sub Main_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        Dim newX As Single = Me.Width / X
        Dim newY As Single = Me.Height / Y
        If X > 0 And Y > 0 Then
            setControls(newX, newY, Me)
        End If

    End Sub
End Class
