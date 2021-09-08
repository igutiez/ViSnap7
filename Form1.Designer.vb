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
        Me.VS7_ValueLabel2 = New Proyecto_ViSnap7.VS7_ValueLabel()
        Me.VS7_HScrollBar1 = New Proyecto_ViSnap7.VS7_HScrollBar()
        Me.VS7_UpdateForm1 = New Proyecto_ViSnap7.VS7_UpdateForm()
        Me.VS7_Textbox1 = New Proyecto_ViSnap7.VS7_Textbox()
        Me.SuspendLayout()
        '
        'VS7_ValueLabel2
        '
        Me.VS7_ValueLabel2.AutoSize = True
        Me.VS7_ValueLabel2.Location = New System.Drawing.Point(418, 167)
        Me.VS7_ValueLabel2.Name = "VS7_ValueLabel2"
        Me.VS7_ValueLabel2.PLC_Bit = 0
        Me.VS7_ValueLabel2.PLC_Byte = 450
        Me.VS7_ValueLabel2.PLC_DataArea = Proyecto_ViSnap7.General.DataArea.MARK
        Me.VS7_ValueLabel2.PLC_DataType = Proyecto_ViSnap7.General.DataType.INT
        Me.VS7_ValueLabel2.PLC_DB = 0
        Me.VS7_ValueLabel2.PLC_Length = 0
        Me.VS7_ValueLabel2.PLC_Number = 0
        Me.VS7_ValueLabel2.Size = New System.Drawing.Size(121, 17)
        Me.VS7_ValueLabel2.TabIndex = 1
        Me.VS7_ValueLabel2.Text = "VS7_ValueLabel2"
        '
        'VS7_HScrollBar1
        '
        Me.VS7_HScrollBar1.Location = New System.Drawing.Point(124, 75)
        Me.VS7_HScrollBar1.Minimum = -100
        Me.VS7_HScrollBar1.Name = "VS7_HScrollBar1"
        Me.VS7_HScrollBar1.PLC_Bit = 0
        Me.VS7_HScrollBar1.PLC_Byte = 450
        Me.VS7_HScrollBar1.PLC_DataArea = Proyecto_ViSnap7.General.DataArea.MARK
        Me.VS7_HScrollBar1.PLC_DataType = Proyecto_ViSnap7.General.DataType.INT
        Me.VS7_HScrollBar1.PLC_DB = 0
        Me.VS7_HScrollBar1.PLC_FormActive = True
        Me.VS7_HScrollBar1.PLC_FormNumber = 1
        Me.VS7_HScrollBar1.PLC_Length = 0
        Me.VS7_HScrollBar1.PLC_Maximum = 100
        Me.VS7_HScrollBar1.PLC_Minimum = -100
        Me.VS7_HScrollBar1.PLC_Number = 0
        Me.VS7_HScrollBar1.PLC_ReadOnly = True
        Me.VS7_HScrollBar1.Size = New System.Drawing.Size(515, 23)
        Me.VS7_HScrollBar1.TabIndex = 0
        '
        'VS7_UpdateForm1
        '
        Me.VS7_UpdateForm1.Location = New System.Drawing.Point(202, 261)
        Me.VS7_UpdateForm1.Name = "VS7_UpdateForm1"
        Me.VS7_UpdateForm1.PLC_FormNumber = 1
        Me.VS7_UpdateForm1.Size = New System.Drawing.Size(75, 23)
        Me.VS7_UpdateForm1.TabIndex = 2
        Me.VS7_UpdateForm1.Text = "VS7_UpdateForm1"
        '
        'VS7_Textbox1
        '
        Me.VS7_Textbox1.Location = New System.Drawing.Point(418, 296)
        Me.VS7_Textbox1.Name = "VS7_Textbox1"
        Me.VS7_Textbox1.PLC_Bit = 0
        Me.VS7_Textbox1.PLC_Byte = 450
        Me.VS7_Textbox1.PLC_DataArea = Proyecto_ViSnap7.General.DataArea.MARK
        Me.VS7_Textbox1.PLC_DataType = Proyecto_ViSnap7.General.DataType.INT
        Me.VS7_Textbox1.PLC_DB = 0
        Me.VS7_Textbox1.PLC_FormActive = False
        Me.VS7_Textbox1.PLC_FormNumber = 0
        Me.VS7_Textbox1.PLC_Length = 0
        Me.VS7_Textbox1.PLC_Number = 0
        Me.VS7_Textbox1.Size = New System.Drawing.Size(100, 22)
        Me.VS7_Textbox1.TabIndex = 3
        Me.VS7_Textbox1.Text = "VS7_Textbox1"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1719, 770)
        Me.Controls.Add(Me.VS7_Textbox1)
        Me.Controls.Add(Me.VS7_UpdateForm1)
        Me.Controls.Add(Me.VS7_ValueLabel2)
        Me.Controls.Add(Me.VS7_HScrollBar1)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents VS7_ValueLabel1 As VS7_ValueLabel
    Friend WithEvents VS7_Button1 As VS7_Button
    Friend WithEvents VS7_HScrollBar1 As VS7_HScrollBar
    Friend WithEvents VS7_ValueLabel2 As VS7_ValueLabel
    Friend WithEvents VS7_UpdateForm1 As VS7_UpdateForm
    Friend WithEvents VS7_Textbox1 As VS7_Textbox
End Class
