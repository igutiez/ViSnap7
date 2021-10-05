<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class UserEditorForm
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UserEditorForm))
        Me.UserManagement1 = New Proyecto_ViSnap7.VS7_UserManagement()
        Me.SuspendLayout()
        '
        'UserManagement1
        '
        Me.UserManagement1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UserManagement1.AutoScroll = True
        Me.UserManagement1.Location = New System.Drawing.Point(16, 15)
        Me.UserManagement1.Margin = New System.Windows.Forms.Padding(5)
        Me.UserManagement1.Name = "UserManagement1"
        Me.UserManagement1.Size = New System.Drawing.Size(1089, 838)
        Me.UserManagement1.TabIndex = 0
        '
        'UserEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(1121, 868)
        Me.Controls.Add(Me.UserManagement1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "UserEditor"
        Me.Text = "Users Editor"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents UserManagement1 As VS7_UserManagement
End Class
