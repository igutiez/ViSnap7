<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class PLC_UserManagement
    Inherits System.Windows.Forms.UserControl

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
        Me.components = New System.ComponentModel.Container()
        Me.UserEdition = New System.Windows.Forms.Button()
        Me.LoginUser = New System.Windows.Forms.TextBox()
        Me.LoginPass = New System.Windows.Forms.TextBox()
        Me.UserAccess = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.UserExit = New System.Windows.Forms.Button()
        Me.UserText = New System.Windows.Forms.Label()
        Me.TxtName = New System.Windows.Forms.Label()
        Me.TxtPassword = New System.Windows.Forms.Label()
        Me.TxtConfirmacion = New System.Windows.Forms.Label()
        Me.TxtLevel = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'UserEdition
        '
        Me.UserEdition.Enabled = False
        Me.UserEdition.Location = New System.Drawing.Point(748, 7)
        Me.UserEdition.Margin = New System.Windows.Forms.Padding(4)
        Me.UserEdition.Name = "UserEdition"
        Me.UserEdition.Size = New System.Drawing.Size(136, 28)
        Me.UserEdition.TabIndex = 0
        Me.UserEdition.Text = "User Edition"
        Me.UserEdition.UseVisualStyleBackColor = True
        Me.UserEdition.Visible = False
        '
        'LoginUser
        '
        Me.LoginUser.Location = New System.Drawing.Point(100, 11)
        Me.LoginUser.Margin = New System.Windows.Forms.Padding(4)
        Me.LoginUser.Name = "LoginUser"
        Me.LoginUser.Size = New System.Drawing.Size(132, 22)
        Me.LoginUser.TabIndex = 1
        '
        'LoginPass
        '
        Me.LoginPass.Location = New System.Drawing.Point(371, 11)
        Me.LoginPass.Margin = New System.Windows.Forms.Padding(4)
        Me.LoginPass.Name = "LoginPass"
        Me.LoginPass.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.LoginPass.Size = New System.Drawing.Size(132, 22)
        Me.LoginPass.TabIndex = 2
        '
        'UserAccess
        '
        Me.UserAccess.Location = New System.Drawing.Point(512, 9)
        Me.UserAccess.Margin = New System.Windows.Forms.Padding(4)
        Me.UserAccess.Name = "UserAccess"
        Me.UserAccess.Size = New System.Drawing.Size(100, 28)
        Me.UserAccess.TabIndex = 3
        Me.UserAccess.Text = "Login"
        Me.UserAccess.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(4, 12)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(55, 20)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "User:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(241, 14)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(97, 20)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Password:"
        '
        'UserExit
        '
        Me.UserExit.Location = New System.Drawing.Point(640, 7)
        Me.UserExit.Margin = New System.Windows.Forms.Padding(4)
        Me.UserExit.Name = "UserExit"
        Me.UserExit.Size = New System.Drawing.Size(100, 28)
        Me.UserExit.TabIndex = 6
        Me.UserExit.Text = "Exit"
        Me.UserExit.UseVisualStyleBackColor = True
        '
        'UserText
        '
        Me.UserText.AutoSize = True
        Me.UserText.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UserText.Location = New System.Drawing.Point(744, 71)
        Me.UserText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.UserText.Name = "UserText"
        Me.UserText.Size = New System.Drawing.Size(65, 20)
        Me.UserText.TabIndex = 7
        Me.UserText.Text = "Label3"
        '
        'TxtName
        '
        Me.TxtName.AutoSize = True
        Me.TxtName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtName.Location = New System.Drawing.Point(25, 71)
        Me.TxtName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.TxtName.Name = "TxtName"
        Me.TxtName.Size = New System.Drawing.Size(57, 20)
        Me.TxtName.TabIndex = 8
        Me.TxtName.Text = "Name"
        '
        'TxtPassword
        '
        Me.TxtPassword.AutoSize = True
        Me.TxtPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtPassword.Location = New System.Drawing.Point(165, 71)
        Me.TxtPassword.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.TxtPassword.Name = "TxtPassword"
        Me.TxtPassword.Size = New System.Drawing.Size(91, 20)
        Me.TxtPassword.TabIndex = 9
        Me.TxtPassword.Text = "Password"
        '
        'TxtConfirmacion
        '
        Me.TxtConfirmacion.AutoSize = True
        Me.TxtConfirmacion.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtConfirmacion.Location = New System.Drawing.Point(307, 71)
        Me.TxtConfirmacion.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.TxtConfirmacion.Name = "TxtConfirmacion"
        Me.TxtConfirmacion.Size = New System.Drawing.Size(116, 20)
        Me.TxtConfirmacion.TabIndex = 10
        Me.TxtConfirmacion.Text = "Confirmation"
        '
        'TxtLevel
        '
        Me.TxtLevel.AutoSize = True
        Me.TxtLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtLevel.Location = New System.Drawing.Point(445, 71)
        Me.TxtLevel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.TxtLevel.Name = "TxtLevel"
        Me.TxtLevel.Size = New System.Drawing.Size(54, 20)
        Me.TxtLevel.TabIndex = 11
        Me.TxtLevel.Text = "Level"
        '
        'Timer1
        '
        Me.Timer1.Interval = 2000
        '
        'PLC_UserManagement
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.TxtLevel)
        Me.Controls.Add(Me.TxtConfirmacion)
        Me.Controls.Add(Me.TxtPassword)
        Me.Controls.Add(Me.TxtName)
        Me.Controls.Add(Me.UserText)
        Me.Controls.Add(Me.UserExit)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.UserAccess)
        Me.Controls.Add(Me.LoginPass)
        Me.Controls.Add(Me.LoginUser)
        Me.Controls.Add(Me.UserEdition)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "PLC_UserManagement"
        Me.Size = New System.Drawing.Size(984, 543)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents UserEdition As Button
    Friend WithEvents LoginUser As TextBox
    Friend WithEvents LoginPass As TextBox
    Friend WithEvents UserAccess As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents UserExit As Button
    Friend WithEvents UserText As Label
    Friend WithEvents TxtName As Label
    Friend WithEvents TxtPassword As Label
    Friend WithEvents TxtConfirmacion As Label
    Friend WithEvents TxtLevel As Label
    Friend WithEvents Timer1 As Timer
End Class
