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
        Me.SuspendLayout()
        '
        'VS7_ValueLabel2
        '
        Me.VS7_ValueLabel2.AutoSize = True
        Me.VS7_ValueLabel2.Location = New System.Drawing.Point(272, 136)
        Me.VS7_ValueLabel2.Name = "VS7_ValueLabel2"
        Me.VS7_ValueLabel2.PLC_Bit = 0
        Me.VS7_ValueLabel2.PLC_Byte = 400
        Me.VS7_ValueLabel2.PLC_DataArea = Proyecto_ViSnap7.General.DataArea.MARK
        Me.VS7_ValueLabel2.PLC_DataType = Proyecto_ViSnap7.General.DataType.DINT
        Me.VS7_ValueLabel2.PLC_DB = 0
        Me.VS7_ValueLabel2.PLC_Length = 0
        Me.VS7_ValueLabel2.PLC_Number = 0
        Me.VS7_ValueLabel2.Size = New System.Drawing.Size(121, 17)
        Me.VS7_ValueLabel2.TabIndex = 0
        Me.VS7_ValueLabel2.Text = "VS7_ValueLabel2"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1131, 680)
        Me.Controls.Add(Me.VS7_ValueLabel2)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents VS7_ValueLabel1 As VS7_ValueLabel
    Friend WithEvents VS7_ValueLabel2 As VS7_ValueLabel
End Class
