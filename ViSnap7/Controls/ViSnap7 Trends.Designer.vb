﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
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
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Me.MyChart = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        CType(Me.MyChart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MyChart
        '
        ChartArea1.Name = "ChartArea1"
        Me.MyChart.ChartAreas.Add(ChartArea1)
        Legend1.Name = "Legend1"
        Me.MyChart.Legends.Add(Legend1)
        Me.MyChart.Location = New System.Drawing.Point(24, 21)
        Me.MyChart.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MyChart.Name = "MyChart"
        Me.MyChart.Size = New System.Drawing.Size(983, 492)
        Me.MyChart.TabIndex = 0
        Me.MyChart.Text = "Chart1"
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'VS7_Trends
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Controls.Add(Me.MyChart)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "VS7_Trends"
        Me.Size = New System.Drawing.Size(1039, 539)
        CType(Me.MyChart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents MyChart As DataVisualization.Charting.Chart
    Friend WithEvents Timer1 As Timer
End Class
