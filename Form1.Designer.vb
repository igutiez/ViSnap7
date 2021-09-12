<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
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
        Me.VS7_AlarmsController1 = New Proyecto_ViSnap7.VS7_AlarmsController()
        Me.SuspendLayout()
        '
        'VS7_AlarmsController1
        '
        Me.VS7_AlarmsController1.AutoScroll = True
        Me.VS7_AlarmsController1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.VS7_AlarmsController1.Location = New System.Drawing.Point(12, 12)
        Me.VS7_AlarmsController1.Name = "VS7_AlarmsController1"
        Me.VS7_AlarmsController1.PLC_ActivateLog = False
        Me.VS7_AlarmsController1.PLC_Bit = 0
        Me.VS7_AlarmsController1.PLC_Byte = 0
        Me.VS7_AlarmsController1.PLC_ColorAckAlarm = System.Drawing.Color.Yellow
        Me.VS7_AlarmsController1.PLC_ColorActiveAlarm = System.Drawing.Color.Red
        Me.VS7_AlarmsController1.PLC_ColorClosedAlarm = System.Drawing.Color.Transparent
        Me.VS7_AlarmsController1.PLC_DataArea = Proyecto_ViSnap7.General.DataArea.DB
        Me.VS7_AlarmsController1.PLC_DB = 0
        Me.VS7_AlarmsController1.PLC_FileExplorer = Nothing
        Me.VS7_AlarmsController1.PLC_Length = 1
        Me.VS7_AlarmsController1.PLC_Number = 0
        Me.VS7_AlarmsController1.PLC_SaveFolder = Nothing
        Me.VS7_AlarmsController1.Size = New System.Drawing.Size(1578, 510)
        Me.VS7_AlarmsController1.TabIndex = 0
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1719, 770)
        Me.Controls.Add(Me.VS7_AlarmsController1)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents VS7_ValueLabel1 As VS7_ValueLabel
    Friend WithEvents VS7_Button1 As VS7_Button
    Friend WithEvents VS7_AlarmsController1 As VS7_AlarmsController
End Class
