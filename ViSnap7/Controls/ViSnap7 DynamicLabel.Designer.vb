<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class VS7_DynamicLabel
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
        Me.LblValue = New System.Windows.Forms.Label()
        Me.ComboArea = New System.Windows.Forms.ComboBox()
        Me.ComboType = New System.Windows.Forms.ComboBox()
        Me.TextBoxDB = New System.Windows.Forms.TextBox()
        Me.LblDB = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBoxByte = New System.Windows.Forms.TextBox()
        Me.LblBit = New System.Windows.Forms.Label()
        Me.TextBoxBIT = New System.Windows.Forms.TextBox()
        Me.LblLen = New System.Windows.Forms.Label()
        Me.TextBoxLen = New System.Windows.Forms.TextBox()
        Me.ButtonApply = New System.Windows.Forms.Button()
        Me.ButtonCont = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'LblValue
        '
        Me.LblValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblValue.Location = New System.Drawing.Point(3, 12)
        Me.LblValue.Name = "LblValue"
        Me.LblValue.Size = New System.Drawing.Size(204, 16)
        Me.LblValue.TabIndex = 0
        Me.LblValue.Text = "99999"
        Me.LblValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboArea
        '
        Me.ComboArea.FormattingEnabled = True
        Me.ComboArea.Location = New System.Drawing.Point(213, 10)
        Me.ComboArea.Name = "ComboArea"
        Me.ComboArea.Size = New System.Drawing.Size(80, 21)
        Me.ComboArea.TabIndex = 1
        '
        'ComboType
        '
        Me.ComboType.FormattingEnabled = True
        Me.ComboType.Location = New System.Drawing.Point(299, 10)
        Me.ComboType.Name = "ComboType"
        Me.ComboType.Size = New System.Drawing.Size(80, 21)
        Me.ComboType.TabIndex = 2
        '
        'TextBoxDB
        '
        Me.TextBoxDB.Location = New System.Drawing.Point(413, 10)
        Me.TextBoxDB.Name = "TextBoxDB"
        Me.TextBoxDB.Size = New System.Drawing.Size(54, 20)
        Me.TextBoxDB.TabIndex = 3
        '
        'LblDB
        '
        Me.LblDB.AutoSize = True
        Me.LblDB.Location = New System.Drawing.Point(385, 14)
        Me.LblDB.Name = "LblDB"
        Me.LblDB.Size = New System.Drawing.Size(22, 13)
        Me.LblDB.TabIndex = 4
        Me.LblDB.Text = "DB"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(474, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(35, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "BYTE"
        '
        'TextBoxByte
        '
        Me.TextBoxByte.Location = New System.Drawing.Point(515, 10)
        Me.TextBoxByte.Name = "TextBoxByte"
        Me.TextBoxByte.Size = New System.Drawing.Size(54, 20)
        Me.TextBoxByte.TabIndex = 5
        '
        'LblBit
        '
        Me.LblBit.AutoSize = True
        Me.LblBit.Location = New System.Drawing.Point(577, 14)
        Me.LblBit.Name = "LblBit"
        Me.LblBit.Size = New System.Drawing.Size(24, 13)
        Me.LblBit.TabIndex = 8
        Me.LblBit.Text = "BIT"
        '
        'TextBoxBIT
        '
        Me.TextBoxBIT.Location = New System.Drawing.Point(607, 10)
        Me.TextBoxBIT.Name = "TextBoxBIT"
        Me.TextBoxBIT.Size = New System.Drawing.Size(54, 20)
        Me.TextBoxBIT.TabIndex = 7
        '
        'LblLen
        '
        Me.LblLen.AutoSize = True
        Me.LblLen.Location = New System.Drawing.Point(668, 14)
        Me.LblLen.Name = "LblLen"
        Me.LblLen.Size = New System.Drawing.Size(28, 13)
        Me.LblLen.TabIndex = 10
        Me.LblLen.Text = "LEN"
        '
        'TextBoxLen
        '
        Me.TextBoxLen.Location = New System.Drawing.Point(698, 10)
        Me.TextBoxLen.Name = "TextBoxLen"
        Me.TextBoxLen.Size = New System.Drawing.Size(54, 20)
        Me.TextBoxLen.TabIndex = 9
        '
        'ButtonApply
        '
        Me.ButtonApply.Location = New System.Drawing.Point(769, 9)
        Me.ButtonApply.Name = "ButtonApply"
        Me.ButtonApply.Size = New System.Drawing.Size(75, 23)
        Me.ButtonApply.TabIndex = 11
        Me.ButtonApply.Text = "Apply"
        Me.ButtonApply.UseVisualStyleBackColor = True
        '
        'ButtonCont
        '
        Me.ButtonCont.Location = New System.Drawing.Point(850, 8)
        Me.ButtonCont.Name = "ButtonCont"
        Me.ButtonCont.Size = New System.Drawing.Size(75, 23)
        Me.ButtonCont.TabIndex = 12
        Me.ButtonCont.Text = "Continuous"
        Me.ButtonCont.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        '
        'ViSnap7_DynamicLabel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ButtonCont)
        Me.Controls.Add(Me.ButtonApply)
        Me.Controls.Add(Me.LblLen)
        Me.Controls.Add(Me.TextBoxLen)
        Me.Controls.Add(Me.LblBit)
        Me.Controls.Add(Me.TextBoxBIT)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TextBoxByte)
        Me.Controls.Add(Me.LblDB)
        Me.Controls.Add(Me.TextBoxDB)
        Me.Controls.Add(Me.ComboType)
        Me.Controls.Add(Me.ComboArea)
        Me.Controls.Add(Me.LblValue)
        Me.Name = "ViSnap7_DynamicLabel"
        Me.Size = New System.Drawing.Size(942, 39)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LblValue As Label
    Friend WithEvents ComboArea As ComboBox
    Friend WithEvents ComboType As ComboBox
    Friend WithEvents TextBoxDB As TextBox
    Friend WithEvents LblDB As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBoxByte As TextBox
    Friend WithEvents LblBit As Label
    Friend WithEvents TextBoxBIT As TextBox
    Friend WithEvents LblLen As Label
    Friend WithEvents TextBoxLen As TextBox
    Friend WithEvents ButtonApply As Button
    Friend WithEvents ButtonCont As Button
    Friend WithEvents Timer1 As Timer
End Class
