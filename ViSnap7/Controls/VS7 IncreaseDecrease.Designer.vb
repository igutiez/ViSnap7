<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class VS7_IncreaseDecrease
    Inherits System.Windows.Forms.UserControl

    'UserControl reemplaza a Dispose para limpiar la lista de componentes.
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
        Me.Counter = New System.Windows.Forms.Label()
        Me.Increase = New System.Windows.Forms.Button()
        Me.Decrease = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Counter
        '
        Me.Counter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Counter.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Counter.Location = New System.Drawing.Point(84, 6)
        Me.Counter.Name = "Counter"
        Me.Counter.Size = New System.Drawing.Size(83, 24)
        Me.Counter.TabIndex = 0
        Me.Counter.Text = "99999"
        Me.Counter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Increase
        '
        Me.Increase.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Increase.Location = New System.Drawing.Point(173, 3)
        Me.Increase.Name = "Increase"
        Me.Increase.Size = New System.Drawing.Size(75, 30)
        Me.Increase.TabIndex = 1
        Me.Increase.Text = "+"
        Me.Increase.UseVisualStyleBackColor = True
        '
        'Decrease
        '
        Me.Decrease.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Decrease.Location = New System.Drawing.Point(3, 3)
        Me.Decrease.Name = "Decrease"
        Me.Decrease.Size = New System.Drawing.Size(75, 30)
        Me.Decrease.TabIndex = 2
        Me.Decrease.Text = "-"
        Me.Decrease.UseVisualStyleBackColor = True
        '
        'VS7_IncreaseDecrease
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Decrease)
        Me.Controls.Add(Me.Increase)
        Me.Controls.Add(Me.Counter)
        Me.Name = "VS7_IncreaseDecrease"
        Me.Size = New System.Drawing.Size(251, 36)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Counter As Label
    Friend WithEvents Increase As Button
    Friend WithEvents Decrease As Button
End Class
