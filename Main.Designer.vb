<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Main
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.VS7_Textbox2 = New Proyecto_ViSnap7.VS7_Textbox()
        Me.VS7_Textbox3 = New Proyecto_ViSnap7.VS7_Textbox()
        Me.SuspendLayout()
        '
        'VS7_Textbox2
        '
        Me.VS7_Textbox2.Enabled = False
        Me.VS7_Textbox2.Location = New System.Drawing.Point(294, 263)
        Me.VS7_Textbox2.Name = "VS7_Textbox2"
        Me.VS7_Textbox2.PLC_Bit = 0
        Me.VS7_Textbox2.PLC_Byte = 0
        Me.VS7_Textbox2.PLC_DataArea = Proyecto_ViSnap7.General.DataArea.DB
        Me.VS7_Textbox2.PLC_DataType = Proyecto_ViSnap7.General.DataType.INT
        Me.VS7_Textbox2.PLC_DB = 1
        Me.VS7_Textbox2.PLC_FormActive = False
        Me.VS7_Textbox2.PLC_FormNumber = 0
        Me.VS7_Textbox2.PLC_keyboard = True
        Me.VS7_Textbox2.PLC_Length = 0
        Me.VS7_Textbox2.PLC_Number = 0
        Me.VS7_Textbox2.PLC_UserLevel = Proyecto_ViSnap7.General.UserLevels.None
        Me.VS7_Textbox2.Size = New System.Drawing.Size(100, 22)
        Me.VS7_Textbox2.TabIndex = 0
        Me.VS7_Textbox2.Text = "VS7_Textbox2"
        '
        'VS7_Textbox3
        '
        Me.VS7_Textbox3.Enabled = False
        Me.VS7_Textbox3.Location = New System.Drawing.Point(468, 263)
        Me.VS7_Textbox3.Name = "VS7_Textbox3"
        Me.VS7_Textbox3.PLC_Bit = 0
        Me.VS7_Textbox3.PLC_Byte = 6
        Me.VS7_Textbox3.PLC_DataArea = Proyecto_ViSnap7.General.DataArea.DB
        Me.VS7_Textbox3.PLC_DataType = Proyecto_ViSnap7.General.DataType.STR
        Me.VS7_Textbox3.PLC_DB = 1
        Me.VS7_Textbox3.PLC_FormActive = False
        Me.VS7_Textbox3.PLC_FormNumber = 0
        Me.VS7_Textbox3.PLC_keyboard = True
        Me.VS7_Textbox3.PLC_Length = 38
        Me.VS7_Textbox3.PLC_Number = 0
        Me.VS7_Textbox3.PLC_UserLevel = Proyecto_ViSnap7.General.UserLevels.None
        Me.VS7_Textbox3.Size = New System.Drawing.Size(100, 22)
        Me.VS7_Textbox3.TabIndex = 1
        Me.VS7_Textbox3.Text = "VS7_Textbox3"
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1540, 770)
        Me.Controls.Add(Me.VS7_Textbox3)
        Me.Controls.Add(Me.VS7_Textbox2)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "Main"
        Me.Text = "https://visnap7.org"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents VS7_ValueLabel1 As VS7_ValueLabel
    Friend WithEvents VS7_Button1 As VS7_Button
    Friend WithEvents VS7_Textbox1 As VS7_Textbox
    Friend WithEvents VS7_Textbox2 As VS7_Textbox
    Friend WithEvents VS7_Textbox3 As VS7_Textbox
End Class
