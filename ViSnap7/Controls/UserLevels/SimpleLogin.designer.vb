<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SimpleLogin
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SimpleLogin))
        Me.PlC_GestorUsuarios1 = New VS7_UserManagement
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'PlC_GestorUsuarios1
        '
        Me.PlC_GestorUsuarios1.AutoScroll = True
        Me.PlC_GestorUsuarios1.Location = New System.Drawing.Point(12, 12)
        Me.PlC_GestorUsuarios1.Name = "PlC_GestorUsuarios1"
        Me.PlC_GestorUsuarios1.Size = New System.Drawing.Size(467, 40)
        Me.PlC_GestorUsuarios1.TabIndex = 0
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        '
        'LoginSimple
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(483, 57)
        Me.Controls.Add(Me.PlC_GestorUsuarios1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "LoginSimple"
        Me.Text = "Acceso de usuario"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PlC_GestorUsuarios1 As VS7_UserManagement
    Friend WithEvents Timer1 As Timer
End Class
