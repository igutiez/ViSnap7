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
        Me.ViSnap7_LoadSaveForm1 = New Proyecto_ViSnap7.ViSnap7_LoadSaveForm()
        Me.SuspendLayout()
        '
        'ViSnap7_LoadSaveForm2
        '
        Me.ViSnap7_LoadSaveForm2.LoadForm = False
        Me.ViSnap7_LoadSaveForm2.Location = New System.Drawing.Point(446, 167)
        Me.ViSnap7_LoadSaveForm2.Name = "ViSnap7_LoadSaveForm2"
        Me.ViSnap7_LoadSaveForm2.PLC_FormNumber = 0
        Me.ViSnap7_LoadSaveForm2.SaveFolder = "D:\OneDrive\Música"
        Me.ViSnap7_LoadSaveForm2.Size = New System.Drawing.Size(75, 23)
        Me.ViSnap7_LoadSaveForm2.TabIndex = 1
        Me.ViSnap7_LoadSaveForm2.Text = "ViSnap7_LoadSaveForm2"
        '
        'ViSnap7_LoadSaveForm1
        '
        Me.ViSnap7_LoadSaveForm1.LoadForm = True
        Me.ViSnap7_LoadSaveForm1.Location = New System.Drawing.Point(325, 167)
        Me.ViSnap7_LoadSaveForm1.Name = "ViSnap7_LoadSaveForm1"
        Me.ViSnap7_LoadSaveForm1.PLC_FormNumber = 0
        Me.ViSnap7_LoadSaveForm1.SaveFolder = Nothing
        Me.ViSnap7_LoadSaveForm1.Size = New System.Drawing.Size(75, 23)
        Me.ViSnap7_LoadSaveForm1.TabIndex = 0
        Me.ViSnap7_LoadSaveForm1.Text = "ViSnap7_LoadSaveForm1"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1131, 679)
        Me.Controls.Add(Me.ViSnap7_LoadSaveForm2)
        Me.Controls.Add(Me.ViSnap7_LoadSaveForm1)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents VS7_ValueLabel1 As VS7_ValueLabel
    Friend WithEvents ViSnap7_LoadSaveForm1 As ViSnap7_LoadSaveForm
    Friend WithEvents ViSnap7_LoadSaveForm2 As ViSnap7_LoadSaveForm
End Class
