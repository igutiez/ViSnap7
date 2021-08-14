<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2
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
        Me.VS7_ValueLabel1 = New Proyecto_ViSnap7.VS7_ValueLabel()
        Me.VS7_Button2 = New Proyecto_ViSnap7.VS7_Button()
        Me.VS7_Button1 = New Proyecto_ViSnap7.VS7_Button()
        Me.VS7_Led2 = New Proyecto_ViSnap7.VS7_Led()
        Me.VS7_Led1 = New Proyecto_ViSnap7.VS7_Led()
        Me.SuspendLayout()
        '
        'VS7_ValueLabel1
        '
        Me.VS7_ValueLabel1.AutoSize = True
        Me.VS7_ValueLabel1.Location = New System.Drawing.Point(259, 219)
        Me.VS7_ValueLabel1.Name = "VS7_ValueLabel1"
        Me.VS7_ValueLabel1.PLC_Bit = 0
        Me.VS7_ValueLabel1.PLC_Byte = 400
        Me.VS7_ValueLabel1.PLC_DataArea = Proyecto_ViSnap7.General.DataArea.MARK
        Me.VS7_ValueLabel1.PLC_DataType = Proyecto_ViSnap7.General.DataType.DINT
        Me.VS7_ValueLabel1.PLC_DB = 0
        Me.VS7_ValueLabel1.PLC_Length = 0
        Me.VS7_ValueLabel1.PLC_Number = 0
        Me.VS7_ValueLabel1.Size = New System.Drawing.Size(121, 17)
        Me.VS7_ValueLabel1.TabIndex = 4
        Me.VS7_ValueLabel1.Text = "VS7_ValueLabel1"
        '
        'VS7_Button2
        '
        Me.VS7_Button2.Location = New System.Drawing.Point(230, 104)
        Me.VS7_Button2.Name = "VS7_Button2"
        Me.VS7_Button2.PLC_Bit = 0
        Me.VS7_Button2.PLC_ButtonType = False
        Me.VS7_Button2.PLC_Byte = 500
        Me.VS7_Button2.PLC_ColorFalse = System.Drawing.SystemColors.Window
        Me.VS7_Button2.PLC_ColorTrue = System.Drawing.Color.Lime
        Me.VS7_Button2.PLC_DataArea = Proyecto_ViSnap7.General.DataArea.MARK
        Me.VS7_Button2.PLC_DataType = Proyecto_ViSnap7.General.DataType.BOOL
        Me.VS7_Button2.PLC_DB = 0
        Me.VS7_Button2.PLC_Length = 0
        Me.VS7_Button2.PLC_Number = 0
        Me.VS7_Button2.Size = New System.Drawing.Size(75, 23)
        Me.VS7_Button2.TabIndex = 3
        Me.VS7_Button2.Text = "VS7_Button2"
        '
        'VS7_Button1
        '
        Me.VS7_Button1.Location = New System.Drawing.Point(230, 71)
        Me.VS7_Button1.Name = "VS7_Button1"
        Me.VS7_Button1.PLC_Bit = 0
        Me.VS7_Button1.PLC_ButtonType = False
        Me.VS7_Button1.PLC_Byte = 40
        Me.VS7_Button1.PLC_ColorFalse = System.Drawing.SystemColors.Window
        Me.VS7_Button1.PLC_ColorTrue = System.Drawing.Color.Lime
        Me.VS7_Button1.PLC_DataArea = Proyecto_ViSnap7.General.DataArea.MARK
        Me.VS7_Button1.PLC_DataType = Proyecto_ViSnap7.General.DataType.BOOL
        Me.VS7_Button1.PLC_DB = 0
        Me.VS7_Button1.PLC_Length = 0
        Me.VS7_Button1.PLC_Number = 0
        Me.VS7_Button1.Size = New System.Drawing.Size(75, 23)
        Me.VS7_Button1.TabIndex = 2
        Me.VS7_Button1.Text = "VS7_Button1"
        '
        'VS7_Led2
        '
        Me.VS7_Led2.BackColor = System.Drawing.SystemColors.Window
        Me.VS7_Led2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.VS7_Led2.Location = New System.Drawing.Point(113, 103)
        Me.VS7_Led2.Name = "VS7_Led2"
        Me.VS7_Led2.PLC_Bit = 0
        Me.VS7_Led2.PLC_Byte = 500
        Me.VS7_Led2.PLC_ColorFalse = System.Drawing.SystemColors.Window
        Me.VS7_Led2.PLC_ColorTrue = System.Drawing.Color.Lime
        Me.VS7_Led2.PLC_DataArea = Proyecto_ViSnap7.General.DataArea.MARK
        Me.VS7_Led2.PLC_DataType = Proyecto_ViSnap7.General.DataType.BOOL
        Me.VS7_Led2.PLC_DB = 0
        Me.VS7_Led2.PLC_Length = 0
        Me.VS7_Led2.PLC_Number = 0
        Me.VS7_Led2.PLC_ShapeType = Proyecto_ViSnap7.VS7_Led.ShapeType.Normal
        Me.VS7_Led2.Size = New System.Drawing.Size(25, 25)
        Me.VS7_Led2.TabIndex = 1
        '
        'VS7_Led1
        '
        Me.VS7_Led1.BackColor = System.Drawing.SystemColors.Window
        Me.VS7_Led1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.VS7_Led1.Location = New System.Drawing.Point(113, 71)
        Me.VS7_Led1.Name = "VS7_Led1"
        Me.VS7_Led1.PLC_Bit = 0
        Me.VS7_Led1.PLC_Byte = 40
        Me.VS7_Led1.PLC_ColorFalse = System.Drawing.SystemColors.Window
        Me.VS7_Led1.PLC_ColorTrue = System.Drawing.Color.Lime
        Me.VS7_Led1.PLC_DataArea = Proyecto_ViSnap7.General.DataArea.MARK
        Me.VS7_Led1.PLC_DataType = Proyecto_ViSnap7.General.DataType.BOOL
        Me.VS7_Led1.PLC_DB = 0
        Me.VS7_Led1.PLC_Length = 0
        Me.VS7_Led1.PLC_Number = 0
        Me.VS7_Led1.PLC_ShapeType = Proyecto_ViSnap7.VS7_Led.ShapeType.Normal
        Me.VS7_Led1.Size = New System.Drawing.Size(25, 25)
        Me.VS7_Led1.TabIndex = 0
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.VS7_ValueLabel1)
        Me.Controls.Add(Me.VS7_Button2)
        Me.Controls.Add(Me.VS7_Button1)
        Me.Controls.Add(Me.VS7_Led2)
        Me.Controls.Add(Me.VS7_Led1)
        Me.Name = "Form2"
        Me.Text = "Form2"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents VS7_Led1 As VS7_Led
    Friend WithEvents VS7_Led2 As VS7_Led
    Friend WithEvents VS7_Button1 As VS7_Button
    Friend WithEvents VS7_Button2 As VS7_Button
    Friend WithEvents VS7_ValueLabel1 As VS7_ValueLabel
End Class
