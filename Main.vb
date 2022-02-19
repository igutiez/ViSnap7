Public Class Main
    Private Home As New FrmHome
    Private Page1 As New FrmPage1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'DO NOT CHANGE THIS LINE
        General.LaunchCommunications()


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

    Private Sub GoHome_Click(sender As Object, e As EventArgs) Handles GoHome.Click
        Me.Home.BringToFront()
    End Sub

    Private Sub Page1ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Page1ToolStripMenuItem.Click
        Me.Page1.BringToFront()

    End Sub
End Class
