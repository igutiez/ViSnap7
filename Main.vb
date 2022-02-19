Public Class Main
    Private Home As New FrmHome
    Private Page2 As New FrmPage2

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'DO NOT CHANGE THIS LINE
        General.LaunchCommunications()


        'CHILD FORMS
        'Home 
        ChildConfig(Home)
        'Page2
        ChildConfig(Page2)

        'SHOW FORMS
        Page2.Show()
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
End Class
