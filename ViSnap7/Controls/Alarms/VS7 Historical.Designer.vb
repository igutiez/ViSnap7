<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class VS7_Historical
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
        Me.HistoricalText = New System.Windows.Forms.RichTextBox()
        Me.DateSelector = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'HistoricalText
        '
        Me.HistoricalText.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HistoricalText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.HistoricalText.Location = New System.Drawing.Point(23, 64)
        Me.HistoricalText.Name = "HistoricalText"
        Me.HistoricalText.ReadOnly = True
        Me.HistoricalText.Size = New System.Drawing.Size(1078, 290)
        Me.HistoricalText.TabIndex = 0
        Me.HistoricalText.Text = ""
        '
        'DateSelector
        '
        Me.DateSelector.Location = New System.Drawing.Point(23, 36)
        Me.DateSelector.Name = "DateSelector"
        Me.DateSelector.Size = New System.Drawing.Size(200, 22)
        Me.DateSelector.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(23, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(190, 20)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Historical date selection"
        '
        'VS7_Historical
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.DateSelector)
        Me.Controls.Add(Me.HistoricalText)
        Me.Name = "VS7_Historical"
        Me.Size = New System.Drawing.Size(1129, 372)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents HistoricalText As RichTextBox
    Friend WithEvents DateSelector As DateTimePicker
    Friend WithEvents Label1 As Label
End Class
