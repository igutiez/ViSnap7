Imports System.IO
Imports System.Reflection

Public Class VS7_UserManagement
    Public ShowEditor As Boolean
    Sub New()

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
        Me.UserEdition.Enabled = ActiveUserLogin
        Me.UserEdition.Visible = ActiveUserLogin
        Me.UserAccess.Visible = Not ActiveUserLogin
        Me.UserText.Text = ActiveUserName

        If ActiveUserLogin Then
            Me.UserText.Visible = True
            Me.UserText.Text = "Usuario: " & ActiveUserName & " (" & ActiveUserLevel & ")"
        Else
            Me.UserText.Visible = False
            Me.UserExit.Visible = False
        End If

        Me.TxtName.Visible = False
        Me.TxtPassword.Visible = False
        Me.TxtConfirmacion.Visible = False
        Me.TxtLevel.Visible = False
        Me.AutoScroll = True

    End Sub


    Public Shared Function GetEnumDescription(ByVal EnumConstant As [Enum]) As String
        Dim fi As FieldInfo = EnumConstant.GetType().GetField(EnumConstant.ToString())
        Dim attr() As DescriptionAttribute =
                  DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute),
                  False), DescriptionAttribute())

        If attr.Length > 0 Then
            Return attr(0).Description
        Else
            Return EnumConstant.ToString()
        End If
    End Function
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles UserEdition.Click
        ListarUsuarios()
    End Sub
    Sub ListarUsuarios()
        Dim folder As New DirectoryInfo(UserFolder)
        Dim sr As StreamReader
        Dim NombreUsuario As String
        Dim PassHash As String
        Dim NumeroUsuarios As Integer
        Dim Usuarios() As User
        Dim ContadorUsuarios As Integer
        Dim PassRecuperado As String
        Dim NivelUsuario As Integer
        Dim NuevoUsuario As User
        If Not IO.Directory.Exists(UserFolder) Then
            IO.Directory.CreateDirectory(UserFolder)
        End If
        NumeroUsuarios = folder.GetFiles.Length
        ReDim Usuarios(NumeroUsuarios)
        EliminarUsuarios()
        Me.TxtName.Visible = True
        Me.TxtPassword.Visible = True
        Me.TxtConfirmacion.Visible = True
        Me.TxtLevel.Visible = True
        For Each file As FileInfo In folder.GetFiles()
            NombreUsuario = file.Name
            sr = New StreamReader(file.FullName)
            PassHash = sr.ReadLine
            sr.Close()
            PassRecuperado = RecoverPassword(PassHash)
            NivelUsuario = PassRecuperado.Substring(PassRecuperado.Length - 1, 1)
            If ActiveUserLevel > NivelUsuario Then

                Usuarios(ContadorUsuarios) = New User
                Usuarios(ContadorUsuarios).Level.Items.Clear()
                If ActiveUserLevel > 1 Then
                    For indice = 0 To ActiveUserLevel - 1

                        Usuarios(ContadorUsuarios).Level.Items.Add(GetEnumDescription(DirectCast(indice, UserLevels)))

                    Next
                End If

                Usuarios(ContadorUsuarios).Name = NombreUsuario
                Usuarios(ContadorUsuarios).UserName.Enabled = False
                Me.Controls.Add(Usuarios(ContadorUsuarios))
                Usuarios(ContadorUsuarios).Location = New Point(10, 80 + Usuarios(ContadorUsuarios).Height * ContadorUsuarios)
                Usuarios(ContadorUsuarios).UserName.Text = NombreUsuario
                Usuarios(ContadorUsuarios).Password.Text = PassRecuperado.Substring(0, PassRecuperado.Length - 1)
                Usuarios(ContadorUsuarios).Password2.Text = PassRecuperado.Substring(0, PassRecuperado.Length - 1)
                Usuarios(ContadorUsuarios).Level.SelectedIndex = PassRecuperado.Substring(PassRecuperado.Length - 1, 1)
                ContadorUsuarios = ContadorUsuarios + 1
            End If



        Next

        Try
            NuevoUsuario = New User

            NuevoUsuario.Level.Items.Clear()
            If ActiveUserLevel > 1 Then
                For indice = 0 To ActiveUserLevel - 1
                    NuevoUsuario.Level.Items.Add(GetEnumDescription(DirectCast(indice, UserLevels)))
                Next
            End If
            NuevoUsuario.DeleteUser.Visible = False
            NuevoUsuario.Acept.Text = "Crear"
            NuevoUsuario.Name = "NuevoUsuario"
            NuevoUsuario.UserName.Enabled = True
            Me.Controls.Add(NuevoUsuario)
            NuevoUsuario.Location = New Point(10, 80 + NuevoUsuario.Height * (ContadorUsuarios + 1))
            NuevoUsuario.UserName.Text = ""
            NuevoUsuario.Password.Text = ""
            NuevoUsuario.Password2.Text = ""

            NuevoUsuario.Level.SelectedIndex = 0
        Catch ex As Exception

        End Try



    End Sub
    Private Sub EliminarUsuarios()
        Dim listadocontroles As New List(Of User)
        For Each ctr As Control In Me.Controls
            If TypeOf (ctr) Is User Then
                listadocontroles.Add(ctr)
            End If
        Next
        For contador = 0 To listadocontroles.Count - 1
            Me.Controls.Remove(listadocontroles(contador))
        Next
        Me.TxtName.Visible = False
        Me.TxtPassword.Visible = False
        Me.TxtConfirmacion.Visible = False
        Me.TxtLevel.Visible = False
        Try
            Me.Invalidate()
            Me.Update()
            Me.Refresh()
        Catch ex As Exception

        End Try


    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles UserAccess.Click
        GestionarUsuarios()
    End Sub
    Public Sub GestionarUsuarios()
        If LoginUser.Text = SuperAdminNombre And LoginPass.Text = SuperAdminPwd Then
            ActiveUserLevel = 10
            ActiveUserLogin = True
            Me.UserEdition.Enabled = Me.ShowEditor
            Me.UserEdition.Visible = Me.ShowEditor
            Me.UserAccess.Visible = False
            Me.LoginUser.Text = ""
            Me.LoginPass.Text = ""
            Me.UserText.Visible = True
            Me.UserText.Text = "Usuario: " & SuperAdminNombre & " (" & ActiveUserLevel & ")"
            ActiveUserName = SuperAdminNombre
            Me.UserExit.Visible = True
        Else
            SearchUser()
        End If
    End Sub

    Private Sub SearchUser()
        Dim folder As New DirectoryInfo(UserFolder)
        Dim sr As StreamReader
        Dim UserName As String
        Dim PassHash As String
        Dim NumberOfUsers As Integer = folder.GetFiles.Length
        Dim RecoveredPass As String
        Dim UserLevel As Integer
        Dim Pass As String
        For Each file As FileInfo In folder.GetFiles()
            UserName = file.Name
            sr = New StreamReader(file.FullName)
            PassHash = sr.ReadLine
            sr.Close()
            RecoveredPass = RecoverPassword(PassHash)
            Pass = RecoveredPass.Substring(0, RecoveredPass.Length - 1)
            Try
                UserLevel = RecoveredPass.Substring(RecoveredPass.Length - 1, 1)

            Catch ex As Exception
                UserLevel = 0
            End Try

            If Me.LoginUser.Text = file.Name And Me.LoginPass.Text = Pass Then
                ActiveUserLevel = UserLevel
                ActiveUserLogin = True
                Me.UserEdition.Enabled = Me.ShowEditor
                Me.UserEdition.Visible = Me.ShowEditor
                Me.UserAccess.Visible = False
                Me.LoginUser.Text = ""
                Me.LoginPass.Text = ""
                Me.UserText.Text = "Usuario: " & file.Name & " (" & UserLevel & ")"
                Me.UserExit.Visible = True
                Exit For
            End If
        Next
        If Not ActiveUserLogin Then
            MsgBox(KUserPassDontMach)
        End If
    End Sub
    Private Sub SalirUsuario_Click(sender As Object, e As EventArgs) Handles UserExit.Click
        LogoutUser()

    End Sub
    Private Sub LogoutUser()
        ActiveUserLevel = 0
        ActiveUserLogin = False
        ActiveUserName = ""
        EliminarUsuarios()

        Me.UserText.Text = ""
        Me.UserEdition.Visible = False
        Me.UserEdition.Enabled = False
        Me.UserAccess.Visible = True
        Me.UserExit.Visible = False

    End Sub
    Public Function RecoverPassword(value As String) As String
        Dim randomValue As Integer
        Dim indice As Integer
        Dim asciiCode As Integer
        Dim hashValue As String = ""

        randomValue = Asc(value(0)) - 65
        indice = randomValue
        For counter = 1 To value.Length - 1
            asciiCode = Asc((value(counter))) - indice
            hashValue = hashValue & Chr(asciiCode)
            indice = indice + 1
        Next
        Return hashValue
    End Function
#Region "USER"
    Partial Class User

        Sub New()

            ' Esta llamada es exigida por el diseñador.
            InitializeComponent()

            ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
            Me.Level.SelectedIndex = 0
        End Sub

        Dim objStreamWriter As StreamWriter
        Private Sub Aceptar_Click(sender As Object, e As EventArgs) Handles Acept.Click
            Dim FileName As String = UserFolder & "\" & Me.UserName.Text
            If UCase(UserName.Text) <> UCase(SuperAdminNombre) Then

                If Password.Text = Password2.Text Then


                    'Comprobamos si existe el directorio de usuarios, si no existe, se crea.
                    If Not IO.Directory.Exists(UserFolder) Then
                        IO.Directory.CreateDirectory(UserFolder)
                    End If
                    'Borramos el fichero del usuario si existe
                    If IO.File.Exists(FileName) Then
                        IO.File.Delete(FileName)
                    End If
                    If Me.Level.SelectedIndex >= ActiveUserLevel Then
                        Me.Level.SelectedIndex = ActiveUserLevel - 1

                    End If
                    If Me.Level.SelectedIndex <= 0 Then
                        Me.Level.SelectedIndex = 0
                    End If
                    Try
                        'Creamos el fichero y guardamos el password cofificado
                        objStreamWriter = New StreamWriter(FileName)
                        Hash = DevolverHash(Me.Password.Text & Me.Level.SelectedIndex)
                        objStreamWriter.WriteLine(Hash)
                        objStreamWriter.Close()
                        sender.parent.parent.ListarUsuarios()
                    Catch ex As Exception
                        MsgBox(KUserNameError)
                    End Try

                Else
                    MsgBox(KUserPassDontMach)
                End If

            Else
                MsgBox(KUserAdminError)
            End If

        End Sub


        Private Function DevolverHash(ByVal Valor As String) As String
            Dim contador, indice As Integer
            Dim Aleatorio As Integer
            Dim CodigoAscii As Integer
            Dim ValorHash As String

            Aleatorio = CInt(VBMath.Rnd() * 10)
            ValorHash = Chr(65 + Aleatorio)
            indice = Aleatorio
            For contador = 0 To Valor.Length - 1
                CodigoAscii = Asc((Valor(contador))) + indice
                ValorHash = ValorHash & Chr(CodigoAscii)
                indice = indice + 1
            Next
            Return ValorHash
        End Function

        Private Sub Borrar_Click(sender As Object, e As EventArgs) Handles DeleteUser.Click
            Dim NombreFichero As String = UserFolder & "\" & Me.UserName.Text
            If MsgBox(KAreSureDeleteUser, MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                If IO.File.Exists(NombreFichero) Then
                    IO.File.Delete(NombreFichero)
                    MsgBox(KUser & " " & Me.UserName.Text & " " & KDeleted)
                    Me.UserName.Text = ""
                    Me.Password.Text = ""
                    Me.Level.SelectedIndex = 0
                    sender.parent.parent.ListarUsuarios()
                Else
                    MsgBox(KUserNotExist)
                End If
            End If
        End Sub


    End Class

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class User
        Inherits System.Windows.Forms.UserControl
        Private Sub Nivel_KeyPress(sender As Object, e As EventArgs) Handles Level.KeyDown
            Try
                If Me.Level.SelectedIndex >= ActiveUserLevel Then
                    Me.Level.SelectedIndex = ActiveUserLevel
                End If
                If Me.Level.SelectedIndex <= 0 Then
                    Me.Level.SelectedIndex = 0

                End If
            Catch ex As Exception
                Me.Level.SelectedIndex = 0
            End Try

        End Sub
        'UserControl reemplaza a Dispose para limpiar la lista de componentes.
        <System.Diagnostics.DebuggerNonUserCode()>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Requerido por el Diseñador de Windows Forms
        Private components As System.ComponentModel.IContainer

        'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
        'Se puede modificar usando el Diseñador de Windows Forms.  
        'No lo modifique con el editor de código.
        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.UserName = New System.Windows.Forms.TextBox()
            Me.Password = New System.Windows.Forms.TextBox()
            Me.Level = New System.Windows.Forms.ComboBox()
            Me.Acept = New System.Windows.Forms.Button()
            Me.DeleteUser = New System.Windows.Forms.Button()
            Me.Password2 = New System.Windows.Forms.TextBox()
            Me.SuspendLayout()
            '
            'Nombre
            '
            Me.UserName.Location = New System.Drawing.Point(7, 6)
            Me.UserName.Name = "Name"
            Me.UserName.Size = New System.Drawing.Size(100, 20)
            Me.UserName.TabIndex = 0
            '
            'Password
            '
            Me.Password.Location = New System.Drawing.Point(113, 6)
            Me.Password.Name = "Password"
            Me.Password.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
            Me.Password.Size = New System.Drawing.Size(100, 20)
            Me.Password.TabIndex = 1
            '
            'Nivel
            '
            Me.Level.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
            Me.Level.FormattingEnabled = True
            Me.Level.Size = New System.Drawing.Size(70, 20)
            Me.Level.Items.AddRange(New Object() {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"})
            Me.Level.Location = New System.Drawing.Point(325, 5)
            Me.Level.Name = "Level"
            Me.Level.TabIndex = 3
            '
            'Aceptar
            '
            Me.Acept.Location = New System.Drawing.Point(399, 3)
            Me.Acept.Name = "Aceptar"
            Me.Acept.Size = New System.Drawing.Size(75, 23)
            Me.Acept.TabIndex = 4
            Me.Acept.Text = "Acept"
            Me.Acept.UseVisualStyleBackColor = True
            '
            'Borrar
            '
            Me.DeleteUser.Location = New System.Drawing.Point(480, 3)
            Me.DeleteUser.Name = "Borrar"
            Me.DeleteUser.Size = New System.Drawing.Size(75, 23)
            Me.DeleteUser.TabIndex = 5
            Me.DeleteUser.Text = "Delete"
            Me.DeleteUser.UseVisualStyleBackColor = True
            '
            'Password2
            '
            Me.Password2.Location = New System.Drawing.Point(219, 6)
            Me.Password2.Name = "Password2"
            Me.Password2.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
            Me.Password2.Size = New System.Drawing.Size(100, 20)
            Me.Password2.TabIndex = 2
            '
            'Usuario
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.Password2)
            Me.Controls.Add(Me.DeleteUser)
            Me.Controls.Add(Me.Acept)
            Me.Controls.Add(Me.Level)
            Me.Controls.Add(Me.Password)
            Me.Controls.Add(Me.UserName)
            Me.Name = "Usuario"
            Me.Size = New System.Drawing.Size(1024, 29)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        Friend WithEvents UserName As TextBox
        Friend WithEvents Password As TextBox
        Friend WithEvents Level As ComboBox
        Friend WithEvents Acept As Button
        Friend WithEvents DeleteUser As Button
        Friend WithEvents Password2 As TextBox
    End Class

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Not ActiveUserLogin Then
            LogoutUser()
        End If
    End Sub

    Private Sub PLC_UserManagementLoad(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Timer1.Start()
    End Sub

#End Region
End Class


Public Class VS7_Login
    Inherits Button

    Private _timer1 As New Timer

    Private _login As String = KLogin
    Private _logout As String = KLogout

    Public Property Login As String
        Get
            Return _login
        End Get
        Set(value As String)
            _login = value
        End Set
    End Property
    Public Property Logout As String
        Get
            Return _logout
        End Get
        Set(value As String)
            _logout = value
        End Set
    End Property
    Sub New()
        _timer1.Interval = 2000
        _timer1.Enabled = True
        AddHandler _timer1.Tick, AddressOf CheckStatus
    End Sub
    Private Sub CheckStatus()
        If ActiveUserLogin Then
            Me.Text = Me.Logout
        Else
            Me.Text = Me.Login
        End If

    End Sub

    Sub ButtonClick(sender As Object, e As EventArgs) Handles Me.Click
        If ActiveUserLogin Then
            ActiveUserLevel = 0
            ActiveUserLogin = False
            ActiveUserName = ""
            Me.Text = _login
        Else
            Dim _simpleLogin As New SimpleLogin
            InhibitUpdateControls = True
            _simpleLogin.ShowDialog()
            If ActiveUserLogin Then
                Me.Text = _logout
            Else
                Me.Text = _login
            End If
        End If

    End Sub

End Class

Public Class VS7_AutomaticLogout
    Inherits Windows.Forms.Timer
    Private XAnterior, YAnterior As Integer
    Sub New()
        XAnterior = Form.MousePosition.X
        YAnterior = Form.MousePosition.Y
        Me.Start()
    End Sub
    Sub chequeo(sender As Object, e As EventArgs) Handles Me.Tick
        If ActiveUserLogin Then
            If (Form.MousePosition.X > XAnterior - 5) And (Form.MousePosition.X < XAnterior + 5) And (Form.MousePosition.Y > YAnterior - 5) And (Form.MousePosition.Y < YAnterior + 5) Then
                ActiveUserLevel = 0
                ActiveUserLogin = False
                ActiveUserName = ""
            End If
            XAnterior = Form.MousePosition.X
            YAnterior = Form.MousePosition.Y
        End If
    End Sub
End Class