Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Windows.Forms.Design

''' <summary>
''' ClearForm Button user control 
''' </summary>

<System.ComponentModel.Designer(GetType(ClearFormDesigner))>
Class VS7_ClearForm
    Inherits Button
    Private _formNumber As Integer
#Region "PLC Properties"
    Public Property PLC_FormNumber As Integer
        Get
            Return _formNumber
        End Get
        Set(value As Integer)
            _formNumber = value
        End Set
    End Property
#End Region

#Region "Control Events"
    Public Sub ButtonClick(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Click
        ClearForm(Me.PLC_FormNumber)
    End Sub

#End Region







End Class





#Region "UpdateForm Smart tags"

Public Class ClearFormDesigner
    Inherits ControlDesigner

    Private _actionListCollection As DesignerActionListCollection

    Public Overrides ReadOnly Property ActionLists() As System.ComponentModel.Design.DesignerActionListCollection
        Get
            If _actionListCollection Is Nothing Then
                _actionListCollection = New DesignerActionListCollection()
                _actionListCollection.Add(New ClearFormActionList(Me.Control))
            End If

            Return _actionListCollection
        End Get
    End Property
End Class

Friend Class ClearFormActionList
    Inherits DesignerActionList

    Private ctr As VS7_ClearForm
    Private designerActionSvc As DesignerActionUIService

    Public Sub New(ByVal component As IComponent)
        MyBase.New(component)

        ctr = DirectCast(component, VS7_ClearForm)
        designerActionSvc = CType(GetService(GetType(DesignerActionUIService)), DesignerActionUIService)
    End Sub

#Region " Properties to display in the Smart-Tag panel "
    Public Property PLC_FormNumber() As Integer
        Get
            Return ctr.PLC_FormNumber
        End Get
        Set(ByVal value As Integer)
            GetPropertyByName(ctr, "PLC_FormNumber").SetValue(ctr, value)
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

        'Add the properties
        items.Add(New DesignerActionPropertyItem("PLC_FormNumber", KPlcFormNumber, KPlcAdressingCategory, KPlcTipPlcFormNumber))

        'Return the ActionItemCollection
        Return items
    End Function
End Class

#End Region
