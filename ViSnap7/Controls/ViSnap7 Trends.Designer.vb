<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class VS7_Trends
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
        Me.components = New System.ComponentModel.Container()
        Dim ChartArea10 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend10 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Me.MyChart = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.CheckSerie1 = New System.Windows.Forms.CheckBox()
        Me.CheckSerie2 = New System.Windows.Forms.CheckBox()
        Me.CheckSerie3 = New System.Windows.Forms.CheckBox()
        Me.CheckSerie4 = New System.Windows.Forms.CheckBox()
        Me.CheckSerie5 = New System.Windows.Forms.CheckBox()
        Me.CheckSerie6 = New System.Windows.Forms.CheckBox()
        Me.CheckSerie7 = New System.Windows.Forms.CheckBox()
        Me.CheckSerie8 = New System.Windows.Forms.CheckBox()
        Me.CheckSerie9 = New System.Windows.Forms.CheckBox()
        Me.CheckSerie10 = New System.Windows.Forms.CheckBox()
        CType(Me.MyChart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MyChart
        '
        ChartArea10.Name = "ChartArea1"
        Me.MyChart.ChartAreas.Add(ChartArea10)
        Legend10.Name = "Legend1"
        Me.MyChart.Legends.Add(Legend10)
        Me.MyChart.Location = New System.Drawing.Point(24, 21)
        Me.MyChart.Margin = New System.Windows.Forms.Padding(4)
        Me.MyChart.Name = "MyChart"
        Me.MyChart.Size = New System.Drawing.Size(983, 472)
        Me.MyChart.TabIndex = 0
        Me.MyChart.Text = "Chart1"
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'CheckSerie1
        '
        Me.CheckSerie1.AutoSize = True
        Me.CheckSerie1.Location = New System.Drawing.Point(24, 500)
        Me.CheckSerie1.Name = "CheckSerie1"
        Me.CheckSerie1.Size = New System.Drawing.Size(81, 20)
        Me.CheckSerie1.TabIndex = 1
        Me.CheckSerie1.Text = "Show S1"
        Me.CheckSerie1.UseVisualStyleBackColor = True
        '
        'CheckSerie2
        '
        Me.CheckSerie2.AutoSize = True
        Me.CheckSerie2.Location = New System.Drawing.Point(111, 500)
        Me.CheckSerie2.Name = "CheckSerie2"
        Me.CheckSerie2.Size = New System.Drawing.Size(81, 20)
        Me.CheckSerie2.TabIndex = 2
        Me.CheckSerie2.Text = "Show S2"
        Me.CheckSerie2.UseVisualStyleBackColor = True
        '
        'CheckSerie3
        '
        Me.CheckSerie3.AutoSize = True
        Me.CheckSerie3.Location = New System.Drawing.Point(198, 500)
        Me.CheckSerie3.Name = "CheckSerie3"
        Me.CheckSerie3.Size = New System.Drawing.Size(81, 20)
        Me.CheckSerie3.TabIndex = 3
        Me.CheckSerie3.Text = "Show S3"
        Me.CheckSerie3.UseVisualStyleBackColor = True
        '
        'CheckSerie4
        '
        Me.CheckSerie4.AutoSize = True
        Me.CheckSerie4.Location = New System.Drawing.Point(285, 500)
        Me.CheckSerie4.Name = "CheckSerie4"
        Me.CheckSerie4.Size = New System.Drawing.Size(81, 20)
        Me.CheckSerie4.TabIndex = 4
        Me.CheckSerie4.Text = "Show S4"
        Me.CheckSerie4.UseVisualStyleBackColor = True
        '
        'CheckSerie5
        '
        Me.CheckSerie5.AutoSize = True
        Me.CheckSerie5.Location = New System.Drawing.Point(372, 500)
        Me.CheckSerie5.Name = "CheckSerie5"
        Me.CheckSerie5.Size = New System.Drawing.Size(81, 20)
        Me.CheckSerie5.TabIndex = 5
        Me.CheckSerie5.Text = "Show S5"
        Me.CheckSerie5.UseVisualStyleBackColor = True
        '
        'CheckSerie6
        '
        Me.CheckSerie6.AutoSize = True
        Me.CheckSerie6.Location = New System.Drawing.Point(459, 500)
        Me.CheckSerie6.Name = "CheckSerie6"
        Me.CheckSerie6.Size = New System.Drawing.Size(81, 20)
        Me.CheckSerie6.TabIndex = 6
        Me.CheckSerie6.Text = "Show S6"
        Me.CheckSerie6.UseVisualStyleBackColor = True
        '
        'CheckSerie7
        '
        Me.CheckSerie7.AutoSize = True
        Me.CheckSerie7.Location = New System.Drawing.Point(546, 500)
        Me.CheckSerie7.Name = "CheckSerie7"
        Me.CheckSerie7.Size = New System.Drawing.Size(81, 20)
        Me.CheckSerie7.TabIndex = 7
        Me.CheckSerie7.Text = "Show S7"
        Me.CheckSerie7.UseVisualStyleBackColor = True
        '
        'CheckSerie8
        '
        Me.CheckSerie8.AutoSize = True
        Me.CheckSerie8.Location = New System.Drawing.Point(633, 500)
        Me.CheckSerie8.Name = "CheckSerie8"
        Me.CheckSerie8.Size = New System.Drawing.Size(81, 20)
        Me.CheckSerie8.TabIndex = 8
        Me.CheckSerie8.Text = "Show S8"
        Me.CheckSerie8.UseVisualStyleBackColor = True
        '
        'CheckSerie9
        '
        Me.CheckSerie9.AutoSize = True
        Me.CheckSerie9.Location = New System.Drawing.Point(720, 500)
        Me.CheckSerie9.Name = "CheckSerie9"
        Me.CheckSerie9.Size = New System.Drawing.Size(81, 20)
        Me.CheckSerie9.TabIndex = 9
        Me.CheckSerie9.Text = "Show S9"
        Me.CheckSerie9.UseVisualStyleBackColor = True
        '
        'CheckSerie10
        '
        Me.CheckSerie10.AutoSize = True
        Me.CheckSerie10.Location = New System.Drawing.Point(807, 500)
        Me.CheckSerie10.Name = "CheckSerie10"
        Me.CheckSerie10.Size = New System.Drawing.Size(88, 20)
        Me.CheckSerie10.TabIndex = 10
        Me.CheckSerie10.Text = "Show S10"
        Me.CheckSerie10.UseVisualStyleBackColor = True
        '
        'VS7_Trends
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Controls.Add(Me.CheckSerie10)
        Me.Controls.Add(Me.CheckSerie9)
        Me.Controls.Add(Me.CheckSerie8)
        Me.Controls.Add(Me.CheckSerie7)
        Me.Controls.Add(Me.CheckSerie6)
        Me.Controls.Add(Me.CheckSerie5)
        Me.Controls.Add(Me.CheckSerie4)
        Me.Controls.Add(Me.CheckSerie3)
        Me.Controls.Add(Me.CheckSerie2)
        Me.Controls.Add(Me.CheckSerie1)
        Me.Controls.Add(Me.MyChart)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "VS7_Trends"
        Me.Size = New System.Drawing.Size(1039, 539)
        CType(Me.MyChart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MyChart As DataVisualization.Charting.Chart
    Friend WithEvents Timer1 As Timer
    Friend WithEvents CheckSerie1 As CheckBox
    Friend WithEvents CheckSerie2 As CheckBox
    Friend WithEvents CheckSerie3 As CheckBox
    Friend WithEvents CheckSerie4 As CheckBox
    Friend WithEvents CheckSerie5 As CheckBox
    Friend WithEvents CheckSerie6 As CheckBox
    Friend WithEvents CheckSerie7 As CheckBox
    Friend WithEvents CheckSerie8 As CheckBox
    Friend WithEvents CheckSerie9 As CheckBox
    Friend WithEvents CheckSerie10 As CheckBox
End Class
