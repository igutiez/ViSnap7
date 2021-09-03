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
        Me.VS7_Trends1 = New Proyecto_ViSnap7.VS7_Trends()
        Me.SuspendLayout()
        '
        'VS7_Trends1
        '
        Me.VS7_Trends1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.VS7_Trends1.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.VS7_Trends1.Location = New System.Drawing.Point(12, 63)
        Me.VS7_Trends1.Name = "VS7_Trends1"
        Me.VS7_Trends1.PLC_Bit = 0
        Me.VS7_Trends1.PLC_Byte = 250
        Me.VS7_Trends1.PLC_DataArea = Proyecto_ViSnap7.General.DataArea.MARK
        Me.VS7_Trends1.PLC_DataType = Proyecto_ViSnap7.General.DataType.INT
        Me.VS7_Trends1.PLC_DB = 1
        Me.VS7_Trends1.PLC_Length = 0
        Me.VS7_Trends1.PLC_Number = 0
        Me.VS7_Trends1.PLC_RegisterNumbers = 120
        Me.VS7_Trends1.PLC_SerieName = "Example"
        Me.VS7_Trends1.PLC_TimeInterval = 1000
        Me.VS7_Trends1.PLC_XAxis = "X"
        Me.VS7_Trends1.PLC_YAxis = "Y"
        Me.VS7_Trends1.Size = New System.Drawing.Size(1265, 438)
        Me.VS7_Trends1.TabIndex = 0
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1289, 626)
        Me.Controls.Add(Me.VS7_Trends1)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents VS7_ValueLabel1 As VS7_ValueLabel
    Friend WithEvents VS7_Button1 As VS7_Button
    Friend WithEvents VS7_Trends1 As VS7_Trends
End Class
