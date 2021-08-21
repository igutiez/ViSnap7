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
        Me.ViSnap7_LoadSaveForm2 = New Proyecto_ViSnap7.ViSnap7_LoadSaveForm()
        Me.VS7_Textbox3 = New Proyecto_ViSnap7.VS7_Textbox()
        Me.VS7_Textbox2 = New Proyecto_ViSnap7.VS7_Textbox()
        Me.VS7_Textbox1 = New Proyecto_ViSnap7.VS7_Textbox()
        Me.ViSnap7_LoadSaveForm1 = New Proyecto_ViSnap7.ViSnap7_LoadSaveForm()
        Me.SuspendLayout()
        '
        'ViSnap7_LoadSaveForm2
        '
        Me.ViSnap7_LoadSaveForm2.Extension = "dat"
        Me.ViSnap7_LoadSaveForm2.LoadForm = True
        Me.ViSnap7_LoadSaveForm2.Location = New System.Drawing.Point(128, 159)
        Me.ViSnap7_LoadSaveForm2.Name = "ViSnap7_LoadSaveForm2"
        Me.ViSnap7_LoadSaveForm2.PLC_FormNumber = 1
        Me.ViSnap7_LoadSaveForm2.SaveFolder = "C:\recipes"
        Me.ViSnap7_LoadSaveForm2.Size = New System.Drawing.Size(75, 23)
        Me.ViSnap7_LoadSaveForm2.TabIndex = 4
        Me.ViSnap7_LoadSaveForm2.Text = "ViSnap7_LoadSaveForm2"
        '
        'VS7_Textbox3
        '
        Me.VS7_Textbox3.Location = New System.Drawing.Point(443, 101)
        Me.VS7_Textbox3.Name = "VS7_Textbox3"
        Me.VS7_Textbox3.PLC_Bit = 0
        Me.VS7_Textbox3.PLC_Byte = 410
        Me.VS7_Textbox3.PLC_DataArea = Proyecto_ViSnap7.General.DataArea.MARK
        Me.VS7_Textbox3.PLC_DataType = Proyecto_ViSnap7.General.DataType.REAL
        Me.VS7_Textbox3.PLC_DB = 1
        Me.VS7_Textbox3.PLC_FormActive = True
        Me.VS7_Textbox3.PLC_FormNumber = 1
        Me.VS7_Textbox3.PLC_Length = 0
        Me.VS7_Textbox3.PLC_Number = 0
        Me.VS7_Textbox3.Size = New System.Drawing.Size(100, 20)
        Me.VS7_Textbox3.TabIndex = 3
        '
        'VS7_Textbox2
        '
        Me.VS7_Textbox2.Location = New System.Drawing.Point(299, 101)
        Me.VS7_Textbox2.Name = "VS7_Textbox2"
        Me.VS7_Textbox2.PLC_Bit = 0
        Me.VS7_Textbox2.PLC_Byte = 402
        Me.VS7_Textbox2.PLC_DataArea = Proyecto_ViSnap7.General.DataArea.MARK
        Me.VS7_Textbox2.PLC_DataType = Proyecto_ViSnap7.General.DataType.INT
        Me.VS7_Textbox2.PLC_DB = 1
        Me.VS7_Textbox2.PLC_FormActive = True
        Me.VS7_Textbox2.PLC_FormNumber = 1
        Me.VS7_Textbox2.PLC_Length = 0
        Me.VS7_Textbox2.PLC_Number = 0
        Me.VS7_Textbox2.Size = New System.Drawing.Size(100, 20)
        Me.VS7_Textbox2.TabIndex = 2
        '
        'VS7_Textbox1
        '
        Me.VS7_Textbox1.Location = New System.Drawing.Point(143, 101)
        Me.VS7_Textbox1.Name = "VS7_Textbox1"
        Me.VS7_Textbox1.PLC_Bit = 0
        Me.VS7_Textbox1.PLC_Byte = 400
        Me.VS7_Textbox1.PLC_DataArea = Proyecto_ViSnap7.General.DataArea.MARK
        Me.VS7_Textbox1.PLC_DataType = Proyecto_ViSnap7.General.DataType.INT
        Me.VS7_Textbox1.PLC_DB = 1
        Me.VS7_Textbox1.PLC_FormActive = True
        Me.VS7_Textbox1.PLC_FormNumber = 1
        Me.VS7_Textbox1.PLC_Length = 0
        Me.VS7_Textbox1.PLC_Number = 0
        Me.VS7_Textbox1.Size = New System.Drawing.Size(100, 20)
        Me.VS7_Textbox1.TabIndex = 1
        '
        'ViSnap7_LoadSaveForm1
        '
        Me.ViSnap7_LoadSaveForm1.Extension = "dat"
        Me.ViSnap7_LoadSaveForm1.LoadForm = False
        Me.ViSnap7_LoadSaveForm1.Location = New System.Drawing.Point(269, 159)
        Me.ViSnap7_LoadSaveForm1.Name = "ViSnap7_LoadSaveForm1"
        Me.ViSnap7_LoadSaveForm1.PLC_FormNumber = 1
        Me.ViSnap7_LoadSaveForm1.SaveFolder = "C:\recipes"
        Me.ViSnap7_LoadSaveForm1.Size = New System.Drawing.Size(75, 23)
        Me.ViSnap7_LoadSaveForm1.TabIndex = 0
        Me.ViSnap7_LoadSaveForm1.Text = "ViSnap7_LoadSaveForm1"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(848, 552)
        Me.Controls.Add(Me.ViSnap7_LoadSaveForm2)
        Me.Controls.Add(Me.VS7_Textbox3)
        Me.Controls.Add(Me.VS7_Textbox2)
        Me.Controls.Add(Me.VS7_Textbox1)
        Me.Controls.Add(Me.ViSnap7_LoadSaveForm1)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents VS7_ValueLabel1 As VS7_ValueLabel
    Friend WithEvents ViSnap7_LoadSaveForm1 As ViSnap7_LoadSaveForm
    Friend WithEvents VS7_Textbox1 As VS7_Textbox
    Friend WithEvents VS7_Textbox2 As VS7_Textbox
    Friend WithEvents VS7_Textbox3 As VS7_Textbox
    Friend WithEvents ViSnap7_LoadSaveForm2 As ViSnap7_LoadSaveForm
End Class
