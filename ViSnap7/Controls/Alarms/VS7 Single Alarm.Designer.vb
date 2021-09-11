<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class VS7_Single_Alarm
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
        Me.AlarmNumber = New System.Windows.Forms.Label()
        Me.AlarmText = New System.Windows.Forms.Label()
        Me.OpenedTime = New System.Windows.Forms.Label()
        Me.AckTime = New System.Windows.Forms.Label()
        Me.ClosedTime = New System.Windows.Forms.Label()
        Me.Status = New System.Windows.Forms.Label()

        Me.SuspendLayout()
        '
        'AlarmNumber
        '
        Me.AlarmNumber.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AlarmNumber.AutoSize = True
        Me.AlarmNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AlarmNumber.Location = New System.Drawing.Point(7, 7)
        Me.AlarmNumber.Name = "AlarmNumber"
        Me.AlarmNumber.Size = New System.Drawing.Size(36, 20)
        Me.AlarmNumber.TabIndex = 0
        Me.AlarmNumber.Text = "999"
        '
        'AlarmText
        '
        Me.AlarmText.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AlarmText.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AlarmText.Location = New System.Drawing.Point(660, 9)
        Me.AlarmText.Name = "AlarmText"
        Me.AlarmText.Size = New System.Drawing.Size(804, 17)
        Me.AlarmText.TabIndex = 1
        Me.AlarmText.Text = "Tex alarm lore impsum"
        '
        'OpenedTime
        '
        Me.OpenedTime.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.OpenedTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OpenedTime.Location = New System.Drawing.Point(129, 7)
        Me.OpenedTime.Name = "OpenedTime"
        Me.OpenedTime.Size = New System.Drawing.Size(171, 20)
        Me.OpenedTime.TabIndex = 2
        Me.OpenedTime.Text = "01/01/2021 12:12:12"
        '
        'AckTime
        '
        Me.AckTime.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AckTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AckTime.Location = New System.Drawing.Point(306, 7)
        Me.AckTime.Name = "AckTime"
        Me.AckTime.Size = New System.Drawing.Size(171, 20)
        Me.AckTime.TabIndex = 3
        Me.AckTime.Text = "01/01/2021 12:12:12"
        '
        'ClosedTime
        '
        Me.ClosedTime.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ClosedTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ClosedTime.Location = New System.Drawing.Point(483, 7)
        Me.ClosedTime.Name = "ClosedTime"
        Me.ClosedTime.Size = New System.Drawing.Size(171, 20)
        Me.ClosedTime.TabIndex = 4
        Me.ClosedTime.Text = "01/01/2021 12:12:12"
        '
        'Status
        '
        Me.Status.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Status.AutoSize = True
        Me.Status.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Status.Location = New System.Drawing.Point(49, 7)
        Me.Status.Name = "Status"
        Me.Status.Size = New System.Drawing.Size(68, 20)
        Me.Status.TabIndex = 5
        Me.Status.Text = "ACTIVE"
        '
        'VS7_Single_Alarm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Status)
        Me.Controls.Add(Me.ClosedTime)
        Me.Controls.Add(Me.AckTime)
        Me.Controls.Add(Me.OpenedTime)
        Me.Controls.Add(Me.AlarmText)
        Me.Controls.Add(Me.AlarmNumber)
        Me.Name = "VS7_Single_Alarm"
        Me.Size = New System.Drawing.Size(1644, 34)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents AlarmNumber As Label
    Friend WithEvents AlarmText As Label
    Friend WithEvents OpenedTime As Label
    Friend WithEvents AckTime As Label
    Friend WithEvents ClosedTime As Label
    Friend WithEvents Status As Label
End Class
