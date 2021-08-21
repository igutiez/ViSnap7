Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Windows.Forms.Design

<System.ComponentModel.Designer(GetType(ViSnap7_LoadSaveForm.SaveFormDesigner))>
Class ViSnap7_LoadSaveForm
    Inherits Button
    Private _folderExplorer As String
    Private _formNumber As Integer
    Private _loadForm As Boolean
    Public _defaultSaveName As String

    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KFolderSaveTip)>
    Public Property SaveFolder As String
        Get
            Return _folderExplorer
        End Get
        Set(value As String)
            _folderExplorer = value
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KFolderLoadTip)>
    Public Property LoadForm As Boolean
        Get
            Return _loadForm
        End Get
        Set(value As Boolean)
            _loadForm = value
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcFormNumber)>
    Public Property PLC_FormNumber As Integer
        Get
            Return _formNumber
        End Get
        Set(value As Integer)
            _formNumber = value
        End Set
    End Property
#Region "Control Events"
    Public Sub ButtonClick(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Click
        If Me.LoadForm Then
            'Actions for loading data-form
            MsgBox("Leyendo datos")
        Else
            'Actions for saving data-form
            Dim _saveDialog As New SaveDialog
            _saveDialog.SaveLabel.Text = KSaveLabel
            _saveDialog.FileName.Text = _defaultSaveName

            If _saveDialog.ShowDialog = DialogResult.OK Then
                SaveFormData(Me._folderExplorer, _saveDialog.FileName.Text, Me.PLC_FormNumber)
                MsgBox("datos salvados")

            End If
            _defaultSaveName = _saveDialog.FileName.Text

        End If
    End Sub

#End Region
#Region "ViSnap7_SaveForm Smart tags"
    Public Class SaveFormDesigner
        Inherits ControlDesigner

        Private _actionListCollection As DesignerActionListCollection

        Public Overrides ReadOnly Property ActionLists() As System.ComponentModel.Design.DesignerActionListCollection
            Get
                If _actionListCollection Is Nothing Then
                    _actionListCollection = New DesignerActionListCollection()
                    _actionListCollection.Add(New SaveFormActionList(Me.Control))
                End If

                Return _actionListCollection
            End Get
        End Property
    End Class

    Friend Class SaveFormActionList
        Inherits DesignerActionList

        Private ctr As ViSnap7_LoadSaveForm
        Private designerActionSvc As DesignerActionUIService

        Public Sub New(ByVal component As IComponent)
            MyBase.New(component)

            ctr = DirectCast(component, ViSnap7_LoadSaveForm)
            designerActionSvc = CType(GetService(GetType(DesignerActionUIService)), DesignerActionUIService)
        End Sub

#Region " Properties to display in the Smart-Tag panel "


        Public Property SaveFolder() As String
            Get
                Return ctr.SaveFolder
            End Get
            Set(ByVal value As String)

                GetPropertyByName(ctr, "SaveFolder").SetValue(ctr, value)

            End Set
        End Property

        Public Property PLC_FormNumber() As Integer
            Get
                Return ctr.PLC_FormNumber
            End Get
            Set(ByVal value As Integer)
                GetPropertyByName(ctr, "PLC_FormNumber").SetValue(ctr, value)
                designerActionSvc.Refresh(ctr)

            End Set
        End Property
        Public Property LoadForm() As Boolean
            Get
                Return ctr.LoadForm
            End Get
            Set(ByVal value As Boolean)
                GetPropertyByName(ctr, "LoadForm").SetValue(ctr, value)
                designerActionSvc.Refresh(ctr)

            End Set
        End Property
#End Region

#Region " Methods to display in the Smart-Tag panel "

        Public Sub OnDock()
            If ctr.Dock = DockStyle.Fill Then
                ctr.Dock = DockStyle.None
            Else
                ctr.Dock = DockStyle.Fill
            End If
            designerActionSvc.Refresh(ctr)
        End Sub
        Public Sub SelectFolder()
            Dim FolderSelector As New FolderBrowserDialog
            If FolderSelector.ShowDialog = vbOK Then
                GetPropertyByName(ctr, "SaveFolder").SetValue(ctr, FolderSelector.SelectedPath)
                designerActionSvc.Refresh(ctr)

            End If
        End Sub

#End Region

        Public Overrides Function GetSortedActionItems() As System.ComponentModel.Design.DesignerActionItemCollection
            Dim str As String
            Dim items As New DesignerActionItemCollection

            If ctr.Dock = DockStyle.Fill Then
                str = "Undock in parent container."
            Else
                str = "Dock in parent container."
            End If

            'Add a few Header Items (categories)
            items.Add(New DesignerActionHeaderItem(KPlcFormCategory))
            items.Add(New DesignerActionHeaderItem(KPlcFolderCategory))

            'Add the properties
            items.Add(New DesignerActionPropertyItem("PLC_FormNumber", KPlcFormNumber, KPlcFormCategory, KPlcTipPlcFormNumber))
            items.Add(New DesignerActionPropertyItem("LoadForm", KPlcLoadForm, KPlcFormCategory, KFolderLoadTip))

            items.Add(New DesignerActionMethodItem(Me, "SelectFolder", KSelectFolder))
            items.Add(New DesignerActionPropertyItem("SaveFolder", KSaveFolder, KPlcFolderCategory, KFolderSaveTip))

            'Return the ActionItemCollection
            Return items
        End Function
    End Class

#End Region
End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SaveDialog
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.FileName = New System.Windows.Forms.TextBox()
        Me.SaveLabel = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(396, 224)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(195, 36)
        Me.TableLayoutPanel1.TabIndex = 0
        Me.TableLayoutPanel1.UseWaitCursor = True
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(4, 4)
        Me.OK_Button.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(89, 28)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "Aceptar"
        Me.OK_Button.UseWaitCursor = True
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(101, 4)
        Me.Cancel_Button.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(89, 28)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancelar"
        Me.Cancel_Button.UseWaitCursor = True
        '
        'FileName
        '
        Me.FileName.Location = New System.Drawing.Point(183, 108)
        Me.FileName.Name = "FileName"
        Me.FileName.Size = New System.Drawing.Size(218, 22)
        Me.FileName.TabIndex = 1
        '
        'SaveLabel
        '
        Me.SaveLabel.AutoSize = True
        Me.SaveLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SaveLabel.Location = New System.Drawing.Point(206, 70)
        Me.SaveLabel.Name = "SaveLabel"
        Me.SaveLabel.Size = New System.Drawing.Size(159, 20)
        Me.SaveLabel.TabIndex = 2
        Me.SaveLabel.Text = "SAVE THE FORM"
        '
        'Dialog1
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(607, 275)
        Me.Controls.Add(Me.SaveLabel)
        Me.Controls.Add(Me.FileName)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Dialog1"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.UseWaitCursor = True
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents FileName As TextBox
    Friend WithEvents SaveLabel As Label
End Class

Public Class SaveDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

End Class

Public Module LoadSaveData
    Public Sub SaveFormData(ByVal folder As String, ByVal fileName As String, formNumber As Integer)
        Dim completeFileName As String = folder + "\" + fileName

    End Sub
End Module
