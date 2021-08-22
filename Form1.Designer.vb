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
        Me.VS7_Textbox1 = New Proyecto_ViSnap7.VS7_Textbox()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.VS7_UpdateForm1 = New Proyecto_ViSnap7.VS7_UpdateForm()
        Me.VS7_SubmitForm1 = New Proyecto_ViSnap7.VS7_SubmitForm()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.VS7_RadioButton2 = New Proyecto_ViSnap7.VS7_RadioButton()
        Me.VS7_RadioButton1 = New Proyecto_ViSnap7.VS7_RadioButton()
        Me.VS7_Checkbox1 = New Proyecto_ViSnap7.VS7_Checkbox()
        Me.VS7_Textbox4 = New Proyecto_ViSnap7.VS7_Textbox()
        Me.ViSnap7_LoadSaveForm2 = New Proyecto_ViSnap7.ViSnap7_LoadSaveForm()
        Me.VS7_Textbox2 = New Proyecto_ViSnap7.VS7_Textbox()
        Me.VS7_Textbox3 = New Proyecto_ViSnap7.VS7_Textbox()
        Me.ViSnap7_LoadSaveForm1 = New Proyecto_ViSnap7.ViSnap7_LoadSaveForm()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'VS7_Textbox1
        '
        Me.VS7_Textbox1.Location = New System.Drawing.Point(65, 112)
        Me.VS7_Textbox1.Name = "VS7_Textbox1"
        Me.VS7_Textbox1.PLC_Bit = 0
        Me.VS7_Textbox1.PLC_Byte = 400
        Me.VS7_Textbox1.PLC_DataArea = Proyecto_ViSnap7.General.DataArea.MARK
        Me.VS7_Textbox1.PLC_DataType = Proyecto_ViSnap7.General.DataType.DINT
        Me.VS7_Textbox1.PLC_DB = 0
        Me.VS7_Textbox1.PLC_FormActive = False
        Me.VS7_Textbox1.PLC_FormNumber = 0
        Me.VS7_Textbox1.PLC_Length = 0
        Me.VS7_Textbox1.PLC_Number = 0
        Me.VS7_Textbox1.Size = New System.Drawing.Size(100, 22)
        Me.VS7_Textbox1.TabIndex = 0
        Me.VS7_Textbox1.Text = "VS7_Textbox1"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(199, 12)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(767, 416)
        Me.TabControl1.TabIndex = 1
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.VS7_UpdateForm1)
        Me.TabPage1.Controls.Add(Me.VS7_SubmitForm1)
        Me.TabPage1.Controls.Add(Me.GroupBox1)
        Me.TabPage1.Controls.Add(Me.VS7_Checkbox1)
        Me.TabPage1.Controls.Add(Me.VS7_Textbox4)
        Me.TabPage1.Controls.Add(Me.ViSnap7_LoadSaveForm2)
        Me.TabPage1.Controls.Add(Me.VS7_Textbox2)
        Me.TabPage1.Controls.Add(Me.VS7_Textbox3)
        Me.TabPage1.Controls.Add(Me.ViSnap7_LoadSaveForm1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 25)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(759, 387)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "TabPage1"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Location = New System.Drawing.Point(4, 25)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(192, 71)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "TabPage2"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'VS7_UpdateForm1
        '
        Me.VS7_UpdateForm1.Location = New System.Drawing.Point(46, 243)
        Me.VS7_UpdateForm1.Name = "VS7_UpdateForm1"
        Me.VS7_UpdateForm1.PLC_FormNumber = 1
        Me.VS7_UpdateForm1.Size = New System.Drawing.Size(100, 23)
        Me.VS7_UpdateForm1.TabIndex = 19
        Me.VS7_UpdateForm1.Text = "UPDATE"
        '
        'VS7_SubmitForm1
        '
        Me.VS7_SubmitForm1.Location = New System.Drawing.Point(254, 243)
        Me.VS7_SubmitForm1.Name = "VS7_SubmitForm1"
        Me.VS7_SubmitForm1.PLC_FormNumber = 1
        Me.VS7_SubmitForm1.Size = New System.Drawing.Size(100, 23)
        Me.VS7_SubmitForm1.TabIndex = 18
        Me.VS7_SubmitForm1.Text = "SUBMIT"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.VS7_RadioButton2)
        Me.GroupBox1.Controls.Add(Me.VS7_RadioButton1)
        Me.GroupBox1.Location = New System.Drawing.Point(429, 243)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(284, 100)
        Me.GroupBox1.TabIndex = 17
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "GroupBox1"
        '
        'VS7_RadioButton2
        '
        Me.VS7_RadioButton2.AutoSize = True
        Me.VS7_RadioButton2.Location = New System.Drawing.Point(26, 58)
        Me.VS7_RadioButton2.Name = "VS7_RadioButton2"
        Me.VS7_RadioButton2.PLC_Bit = 1
        Me.VS7_RadioButton2.PLC_Byte = 46
        Me.VS7_RadioButton2.PLC_DataArea = Proyecto_ViSnap7.General.DataArea.DB
        Me.VS7_RadioButton2.PLC_DataType = Proyecto_ViSnap7.General.DataType.BOOL
        Me.VS7_RadioButton2.PLC_DB = 1
        Me.VS7_RadioButton2.PLC_FormActive = True
        Me.VS7_RadioButton2.PLC_FormNumber = 1
        Me.VS7_RadioButton2.PLC_Length = 0
        Me.VS7_RadioButton2.PLC_Number = 0
        Me.VS7_RadioButton2.Size = New System.Drawing.Size(149, 21)
        Me.VS7_RadioButton2.TabIndex = 12
        Me.VS7_RadioButton2.Text = "VS7_RadioButton2"
        '
        'VS7_RadioButton1
        '
        Me.VS7_RadioButton1.AutoSize = True
        Me.VS7_RadioButton1.Location = New System.Drawing.Point(26, 31)
        Me.VS7_RadioButton1.Name = "VS7_RadioButton1"
        Me.VS7_RadioButton1.PLC_Bit = 0
        Me.VS7_RadioButton1.PLC_Byte = 46
        Me.VS7_RadioButton1.PLC_DataArea = Proyecto_ViSnap7.General.DataArea.DB
        Me.VS7_RadioButton1.PLC_DataType = Proyecto_ViSnap7.General.DataType.BOOL
        Me.VS7_RadioButton1.PLC_DB = 1
        Me.VS7_RadioButton1.PLC_FormActive = True
        Me.VS7_RadioButton1.PLC_FormNumber = 1
        Me.VS7_RadioButton1.PLC_Length = 0
        Me.VS7_RadioButton1.PLC_Number = 0
        Me.VS7_RadioButton1.Size = New System.Drawing.Size(149, 21)
        Me.VS7_RadioButton1.TabIndex = 11
        Me.VS7_RadioButton1.Text = "VS7_RadioButton1"
        '
        'VS7_Checkbox1
        '
        Me.VS7_Checkbox1.AutoSize = True
        Me.VS7_Checkbox1.Location = New System.Drawing.Point(254, 43)
        Me.VS7_Checkbox1.Name = "VS7_Checkbox1"
        Me.VS7_Checkbox1.PLC_Bit = 2
        Me.VS7_Checkbox1.PLC_Byte = 46
        Me.VS7_Checkbox1.PLC_DataArea = Proyecto_ViSnap7.General.DataArea.DB
        Me.VS7_Checkbox1.PLC_DataType = Proyecto_ViSnap7.General.DataType.BOOL
        Me.VS7_Checkbox1.PLC_DB = 1
        Me.VS7_Checkbox1.PLC_FormActive = True
        Me.VS7_Checkbox1.PLC_FormNumber = 1
        Me.VS7_Checkbox1.PLC_Length = 0
        Me.VS7_Checkbox1.PLC_Number = 0
        Me.VS7_Checkbox1.Size = New System.Drawing.Size(133, 21)
        Me.VS7_Checkbox1.TabIndex = 16
        Me.VS7_Checkbox1.Text = "VS7_Checkbox1"
        '
        'VS7_Textbox4
        '
        Me.VS7_Textbox4.Location = New System.Drawing.Point(46, 43)
        Me.VS7_Textbox4.Name = "VS7_Textbox4"
        Me.VS7_Textbox4.PLC_Bit = 0
        Me.VS7_Textbox4.PLC_Byte = 6
        Me.VS7_Textbox4.PLC_DataArea = Proyecto_ViSnap7.General.DataArea.DB
        Me.VS7_Textbox4.PLC_DataType = Proyecto_ViSnap7.General.DataType.STR
        Me.VS7_Textbox4.PLC_DB = 1
        Me.VS7_Textbox4.PLC_FormActive = True
        Me.VS7_Textbox4.PLC_FormNumber = 1
        Me.VS7_Textbox4.PLC_Length = 38
        Me.VS7_Textbox4.PLC_Number = 0
        Me.VS7_Textbox4.Size = New System.Drawing.Size(132, 22)
        Me.VS7_Textbox4.TabIndex = 15
        '
        'ViSnap7_LoadSaveForm2
        '
        Me.ViSnap7_LoadSaveForm2.Extension = "dat"
        Me.ViSnap7_LoadSaveForm2.LoadForm = True
        Me.ViSnap7_LoadSaveForm2.Location = New System.Drawing.Point(46, 170)
        Me.ViSnap7_LoadSaveForm2.Margin = New System.Windows.Forms.Padding(4)
        Me.ViSnap7_LoadSaveForm2.Name = "ViSnap7_LoadSaveForm2"
        Me.ViSnap7_LoadSaveForm2.PLC_FormNumber = 1
        Me.ViSnap7_LoadSaveForm2.SaveFolder = "C:\recipes"
        Me.ViSnap7_LoadSaveForm2.Size = New System.Drawing.Size(100, 28)
        Me.ViSnap7_LoadSaveForm2.TabIndex = 14
        Me.ViSnap7_LoadSaveForm2.Text = "LOAD"
        '
        'VS7_Textbox2
        '
        Me.VS7_Textbox2.Location = New System.Drawing.Point(254, 98)
        Me.VS7_Textbox2.Margin = New System.Windows.Forms.Padding(4)
        Me.VS7_Textbox2.Name = "VS7_Textbox2"
        Me.VS7_Textbox2.PLC_Bit = 0
        Me.VS7_Textbox2.PLC_Byte = 2
        Me.VS7_Textbox2.PLC_DataArea = Proyecto_ViSnap7.General.DataArea.DB
        Me.VS7_Textbox2.PLC_DataType = Proyecto_ViSnap7.General.DataType.REAL
        Me.VS7_Textbox2.PLC_DB = 1
        Me.VS7_Textbox2.PLC_FormActive = True
        Me.VS7_Textbox2.PLC_FormNumber = 1
        Me.VS7_Textbox2.PLC_Length = 0
        Me.VS7_Textbox2.PLC_Number = 0
        Me.VS7_Textbox2.Size = New System.Drawing.Size(132, 22)
        Me.VS7_Textbox2.TabIndex = 13
        '
        'VS7_Textbox3
        '
        Me.VS7_Textbox3.Location = New System.Drawing.Point(46, 98)
        Me.VS7_Textbox3.Margin = New System.Windows.Forms.Padding(4)
        Me.VS7_Textbox3.Name = "VS7_Textbox3"
        Me.VS7_Textbox3.PLC_Bit = 0
        Me.VS7_Textbox3.PLC_Byte = 0
        Me.VS7_Textbox3.PLC_DataArea = Proyecto_ViSnap7.General.DataArea.DB
        Me.VS7_Textbox3.PLC_DataType = Proyecto_ViSnap7.General.DataType.INT
        Me.VS7_Textbox3.PLC_DB = 1
        Me.VS7_Textbox3.PLC_FormActive = True
        Me.VS7_Textbox3.PLC_FormNumber = 1
        Me.VS7_Textbox3.PLC_Length = 0
        Me.VS7_Textbox3.PLC_Number = 0
        Me.VS7_Textbox3.Size = New System.Drawing.Size(132, 22)
        Me.VS7_Textbox3.TabIndex = 12
        '
        'ViSnap7_LoadSaveForm1
        '
        Me.ViSnap7_LoadSaveForm1.Extension = "dat"
        Me.ViSnap7_LoadSaveForm1.LoadForm = False
        Me.ViSnap7_LoadSaveForm1.Location = New System.Drawing.Point(254, 170)
        Me.ViSnap7_LoadSaveForm1.Margin = New System.Windows.Forms.Padding(4)
        Me.ViSnap7_LoadSaveForm1.Name = "ViSnap7_LoadSaveForm1"
        Me.ViSnap7_LoadSaveForm1.PLC_FormNumber = 1
        Me.ViSnap7_LoadSaveForm1.SaveFolder = "C:\recipes"
        Me.ViSnap7_LoadSaveForm1.Size = New System.Drawing.Size(100, 28)
        Me.ViSnap7_LoadSaveForm1.TabIndex = 11
        Me.ViSnap7_LoadSaveForm1.Text = "SAVE"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1131, 679)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.VS7_Textbox1)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents VS7_ValueLabel1 As VS7_ValueLabel
    Friend WithEvents VS7_Textbox1 As VS7_Textbox
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents VS7_UpdateForm1 As VS7_UpdateForm
    Friend WithEvents VS7_SubmitForm1 As VS7_SubmitForm
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents VS7_RadioButton2 As VS7_RadioButton
    Friend WithEvents VS7_RadioButton1 As VS7_RadioButton
    Friend WithEvents VS7_Checkbox1 As VS7_Checkbox
    Friend WithEvents VS7_Textbox4 As VS7_Textbox
    Friend WithEvents ViSnap7_LoadSaveForm2 As ViSnap7_LoadSaveForm
    Friend WithEvents VS7_Textbox2 As VS7_Textbox
    Friend WithEvents VS7_Textbox3 As VS7_Textbox
    Friend WithEvents ViSnap7_LoadSaveForm1 As ViSnap7_LoadSaveForm
    Friend WithEvents TabPage2 As TabPage
End Class
