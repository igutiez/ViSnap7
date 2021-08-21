Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Windows.Forms.Design


<System.ComponentModel.Designer(GetType(ViSnap7_LoadSaveForm.SaveFormDesigner))>
Class ViSnap7_LoadSaveForm
    Inherits Button
    Private _folderExplorer As String
    Private _formNumber As Integer = 1
    Private _loadForm As Boolean
    Private _extension As String = "dat"

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

    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KExtensionTip)>
    Public Property Extension As String
        Get
            Return _extension
        End Get
        Set(value As String)
            _extension = value
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
            Dim _loadDialog As New LoadDialog
            Dim _selectedrecipe As String
            _loadDialog.RecipeListLabel.Text = KLoadLabel
            LoadAllRecipes(_loadDialog, Me.SaveFolder, Me.Extension)

            If _loadDialog.ShowDialog = vbOK Then

            End If
        Else
            'Actions for saving data-form
            Dim _saveDialog As New SaveDialog
            _saveDialog.SaveLabel.Text = KSaveLabel
            _saveDialog.FileName.Text = _defaultSaveName
            _saveDialog.FileName.Select()

            If _saveDialog.ShowDialog = DialogResult.OK Then
                SaveFormData(Me.SaveFolder, _saveDialog.FileName.Text, Me.Extension, Me.PLC_FormNumber)
                MsgBox(KDataSaved)

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

        Public Property Extension() As String
            Get
                Return ctr.Extension
            End Get
            Set(ByVal value As String)

                GetPropertyByName(ctr, "Extension").SetValue(ctr, value)

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
            items.Add(New DesignerActionPropertyItem("Extension", KExtensionFile, KPlcFolderCategory, KExtensionTip))

            'Return the ActionItemCollection
            Return items
        End Function
    End Class

#End Region
End Class





Public Module LoadSaveData

    Public Sub SaveFormData(ByVal folder As String, ByVal fileName As String, ByVal extension As String, ByVal formNumber As Integer)
        Dim completeFileName As String = folder + "\" + fileName + "." + extension
        Dim c As Integer
        Dim fileOverwrite As Boolean = False

        Dim fileExist = IO.File.Exists(completeFileName)

        If fileExist Then
            If MsgBox("File alreade exist. Overwrite?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                fileOverwrite = True
            End If

        End If

        If Not fileExist Or fileOverwrite Then

            'If file exists, it must be delete in order to save new data
            IO.File.Delete(completeFileName)
            Dim recipeText As String = ""
            For c = 0 To totalPlcNumber - 1
                For Each ctr As Object In plc(c).controlsCollection
                    If [Enum].IsDefined(GetType(Control_List.PlcCrtCanBeForms), ctr.GetType.Name) Then
                        If ctr.PLC_FormNumber <> 0 And ctr.PLC_FormNumber = formNumber Then

                            With ctr
                                recipeText = recipeText + .Name.ToString + KColumSeparator
                                recipeText = recipeText + .plc_Value.ToString + KColumSeparator
                                recipeText = recipeText + .PLC_Number.ToString + KColumSeparator
                                recipeText = recipeText + .PLC_DataArea.ToString + KColumSeparator
                                recipeText = recipeText + .PLC_DataType.ToString + KColumSeparator
                                recipeText = recipeText + .PLC_DB.ToString + KColumSeparator
                                recipeText = recipeText + .PLC_Byte.ToString + KColumSeparator
                                recipeText = recipeText + .PLC_Bit.ToString + KColumSeparator
                                recipeText = recipeText + .PLC_Length.ToString
                                recipeText = recipeText + vbCr
                            End With
                        End If
                    End If
                Next
            Next
            IO.File.WriteAllText(completeFileName, recipeText)
        End If
    End Sub
    Public Sub LoadAllRecipes(ByRef frm As LoadDialog, ByVal folder As String, ByVal fileExtension As String)

        For Each newfile As IO.FileInfo In New IO.DirectoryInfo(folder).GetFiles


            If UCase(Trim(newfile.Extension)) = UCase(Trim("." + fileExtension)) Then
                frm.RecipeList.Items.Add(newfile.Name.Replace("." + fileExtension, ""))
            End If
        Next
    End Sub
End Module
