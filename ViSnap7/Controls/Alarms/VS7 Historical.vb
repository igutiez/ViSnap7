Imports System.Windows.Forms.Design
<System.ComponentModel.Designer(GetType(HistoricalControllerDesigner))>
Public Class VS7_Historical
    Private _folderExplorer As String
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KFolderLog)>
    Public Property PLC_SaveFolder As String
        Get
            Return _folderExplorer
        End Get
        Set(value As String)
            _folderExplorer = value
        End Set
    End Property


    Private Sub DateTimePicker_ValueChanged(sender As Object, e As EventArgs) Handles DateSelector.ValueChanged
        Try
            Dim fileDate As String = DateSelector.Value.ToShortDateString.Replace("/", "-")
            Dim filePath = Me.PLC_SaveFolder + "\" + fileDate + ".txt"
            Dim texts() = IO.File.ReadAllLines(filePath)

            Me.HistoricalText.Text = "HOUR" + vbTab + "N#" + vbTab + "STATUS" + vbTab + "ALARM TEXT" + vbCrLf + vbCrLf
            For Each line As String In texts
                If line.Contains("ACK") Then

                End If
                Me.HistoricalText.Text = HistoricalText.Text + line + vbCrLf

            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub VS7_Historical_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
#Region "Label Smart tags"

Public Class HistoricalControllerDesigner
    Inherits ControlDesigner

    Private _actionListCollection As DesignerActionListCollection

    Public Overrides ReadOnly Property ActionLists() As System.ComponentModel.Design.DesignerActionListCollection
        Get
            If _actionListCollection Is Nothing Then
                _actionListCollection = New DesignerActionListCollection()
                _actionListCollection.Add(New HistoricalControllerActionList(Me.Control))
            End If

            Return _actionListCollection
        End Get
    End Property
End Class

Friend Class HistoricalControllerActionList
    Inherits DesignerActionList

    Private ctr As VS7_Historical
    Private designerActionSvc As DesignerActionUIService

    Public Sub New(ByVal component As IComponent)
        MyBase.New(component)

        ctr = DirectCast(component, VS7_Historical)
        designerActionSvc = CType(GetService(GetType(DesignerActionUIService)), DesignerActionUIService)
    End Sub

#Region " Properties to display in the Smart-Tag panel "



    Public Property PLC_SaveFolder() As String
        Get
            Return ctr.PLC_SaveFolder
        End Get
        Set(ByVal value As String)

            GetPropertyByName(ctr, "PLC_SaveFolder").SetValue(ctr, value)

        End Set
    End Property

#End Region

#Region " Methods to display in the Smart-Tag panel "

    Private Sub SelectFolder()
        Dim FolderSelector As New FolderBrowserDialog
        If FolderSelector.ShowDialog = DialogResult.OK Then
            GetPropertyByName(ctr, "PLC_SaveFolder").SetValue(ctr, FolderSelector.SelectedPath)
            designerActionSvc.Refresh(ctr)

        End If
    End Sub




    Public Sub OnDock()
        If ctr.Dock = DockStyle.Fill Then
            ctr.Dock = DockStyle.None
        Else
            ctr.Dock = DockStyle.Fill
        End If

        designerActionSvc.Refresh(ctr)
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
        items.Add(New DesignerActionHeaderItem(KPlcAdressingCategory))

        'Add the properties
        items.Add(New DesignerActionPropertyItem("PLC_SaveFolder", KSaveFolder, KPlcFolderCategory, KFolderSaveTip))
        items.Add(New DesignerActionMethodItem(Me, "SelectFolder", KSelectFolder))


        'Return the ActionItemCollection
        Return items
    End Function
End Class

#End Region