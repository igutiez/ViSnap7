Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Windows.Forms.Design

''' <summary>
''' PLC Value Label user control 
''' </summary>
<System.ComponentModel.Designer(GetType(DynamicLabelDesigner))>
Class ViSnap7_DynamicLabel
    Inherits UserControl
#Region "PLC Properties"
    Public pLC_Value As String


    Private _PLC As Integer
    Private _DataArea As General.DataArea = DataArea.DB
    Private _DB As Integer
    Private _Byte As Integer
    Private _Bit As Integer
    Private _DataType As General.DataType = DataType.INT
    Private _Length As Integer
    Private _txt As String
    Private _updateMyControl As Boolean
    Private _New_PLC As Integer
    Private _New_DataArea As General.DataArea = DataArea.DB
    Private _New_DB As Integer
    Private _New_Byte As Integer
    Private _New_Bit As Integer
    Private _New_DataType As General.DataType = DataType.INT
    Private _New_Length As Integer
    Private _New_txt As String
    Private _ContinueUpdating As Boolean = False

    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcNumberLabel)>
    Public Property PLC_Number As Integer
        Get
            Return _PLC
        End Get
        Set(value As Integer)
            _PLC = value
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcDataAreaLabel)>
    Public Property PLC_DataArea As General.DataArea
        Get
            Return _DataArea
        End Get
        Set(value As General.DataArea)
            _DataArea = value
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcDBLabel)>
    Public Property PLC_DB As Integer
        Get
            Return _DB
        End Get
        Set(value As Integer)
            _DB = value
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcByteLabel)>
    Public Property PLC_Byte As Integer
        Get
            Return _Byte
        End Get
        Set(value As Integer)
            _Byte = value
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcValueTypeLabel)>
    Public Property PLC_DataType As General.DataType
        Get
            Return _DataType
        End Get
        Set(value As General.DataType)
            _DataType = value

        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcBitLabel)>
    Public Property PLC_Bit As Integer
        Get
            Return _Bit
        End Get
        Set(value As Integer)
            _Bit = value
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcLengthLabel)>
    Public Property PLC_Length As Integer
        Get
            Return _Length
        End Get
        Set(value As Integer)
            _Length = value
        End Set
    End Property





#End Region
#Region "Control Events"



#End Region
#Region "Plc reading and writing"
    Public Sub UpdateControl(ByRef _PLC As PlcClient)

        If (_updateMyControl And Not UpdateNumberOfControlsActive) Or _ContinueUpdating Then

            _updateMyControl = False

            Select Case Me.PLC_DataArea
                Case DataArea.DB
                    Me.LblValue.Text = TakeValue(_PLC.dbData(Me.PLC_DB), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)
                    Me.pLC_Value = Me.LblValue.Text
                Case DataArea.INPUT
                    Me.LblValue.Text = TakeValue(_PLC.inputData(0), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)
                    Me.pLC_Value = Me.LblValue.Text
                Case DataArea.MARK
                    Me.LblValue.Text = TakeValue(_PLC.marksData(0), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)
                    Me.pLC_Value = Me.LblValue.Text
                Case DataArea.OUTPUT
                    Me.LblValue.Text = TakeValue(_PLC.outputData(0), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)
                    Me.pLC_Value = Me.LblValue.Text
                Case Else
            End Select


        End If

    End Sub




    Private Function TakeValue(_DBData As PlcClient.ByteData, _PLC_DB As Integer, _PLC_Byte As Integer, _PLC_Bit As Integer, _PLC_DataType As Integer, _PLC_Length As Integer) As String
        Dim txt As String = ""
        Select Case _PLC_DataType
            Case DataType.BOOL
                txt = ViSnap7.S7.GetBitAt(_DBData.data, _PLC_Byte, _PLC_Bit)

            Case DataType.CHR
                txt = ViSnap7.S7.GetCharsAt(_DBData.data, _PLC_Byte, 1)
            Case DataType.DINT
                txt = ViSnap7.S7.GetDIntAt(_DBData.data, _PLC_Byte)
            Case DataType.INT
                txt = ViSnap7.S7.GetIntAt(_DBData.data, _PLC_Byte)
            Case DataType.REAL
                txt = ViSnap7.S7.GetRealAt(_DBData.data, _PLC_Byte)
            Case DataType.SINT
                txt = ViSnap7.S7.GetSIntAt(_DBData.data, _PLC_Byte)
            Case DataType.STR
                txt = ViSnap7.S7.GetStringAt(_DBData.data, _PLC_Byte)
            Case DataType.UINT
                txt = ViSnap7.S7.GetUIntAt(_DBData.data, _PLC_Byte)
            Case Else
                txt = ""
        End Select
        Return txt
    End Function

    Private Sub ViSnap7_DynamicLabel_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.ComboArea.DataSource = [Enum].GetValues(GetType(DataArea))
        Me.ComboType.DataSource = [Enum].GetValues(GetType(DataType))
        Me.TextBoxDB.Text = Me.PLC_DB
        Me.TextBoxByte.Text = Me.PLC_Byte
        Me.TextBoxBIT.Text = Me.PLC_Bit
        Me.TextBoxLen.Text = Me.PLC_Length

    End Sub

    Private Sub ComboArea_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboArea.SelectedIndexChanged
        _New_DataArea = ComboArea.SelectedItem
    End Sub

    Private Sub ComboType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboType.SelectedIndexChanged
        _New_DataType = ComboType.SelectedItem
    End Sub

    Private Sub ButtonApply_Click(sender As Object, e As EventArgs) Handles ButtonApply.Click
        Me.PLC_DataArea = _New_DataArea
        Me.PLC_DataType = _New_DataType
        Me.PLC_Byte = Me.TextBoxByte.Text
        Me.PLC_Bit = Me.TextBoxBIT.Text
        Me.PLC_DB = Me.TextBoxDB.Text
        Me.PLC_Length = Me.TextBoxLen.Text


        UpdateNumberOfControlsActive = True
        _updateMyControl = True
    End Sub

    Private Sub ButtonCont_Click(sender As Object, e As EventArgs) Handles ButtonCont.Click
        _ContinueUpdating = Not _ContinueUpdating
        Me.ButtonApply.Enabled = Not _ContinueUpdating
        If _ContinueUpdating Then
            ButtonCont.Text = "STOP"

        Else
            ButtonCont.Text = "START"
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        If ComboArea.SelectedItem <> DataArea.DB Then
            Me.TextBoxDB.Enabled = False
            Me.LblDB.Enabled = False
        Else
            Me.TextBoxDB.Enabled = True
            Me.LblDB.Enabled = True
        End If

        If ComboType.SelectedItem <> DataType.BOOL Then
            Me.TextBoxBIT.Enabled = False
            Me.LblBit.Enabled = False
        Else
            Me.TextBoxBIT.Enabled = True
            Me.LblBit.Enabled = True
        End If
        If ComboType.SelectedItem <> DataType.STR Then
            Me.TextBoxLen.Enabled = False
            Me.LblLen.Enabled = False
        Else
            Me.TextBoxLen.Enabled = True
            Me.LblLen.Enabled = True

        End If
        If IsNumeric(TextBoxDB.Text) Then
            If TextBoxDB.Text <= 0 Then
                TextBoxDB.Text = 1
            End If
        Else
            TextBoxDB.Text = 1
        End If

        If IsNumeric(TextBoxByte.Text) Then
            If TextBoxByte.Text < 0 Then
                TextBoxByte.Text = 0
            End If
        Else
            TextBoxByte.Text = 0
        End If
        If IsNumeric(TextBoxBIT.Text) Then
            If TextBoxBIT.Text < 0 Then
                TextBoxBIT.Text = 0
            End If
        Else
            TextBoxBIT.Text = 0
        End If
        If IsNumeric(TextBoxLen.Text) Then
            If TextBoxLen.Text <= 0 Then
                TextBoxLen.Text = 1
            End If
        Else
            TextBoxLen.Text = 0
        End If

        Timer1.Start()
    End Sub

#End Region
End Class
#Region "Label Smart tags"

Public Class DynamicLabelDesigner
    Inherits ControlDesigner

    Private _actionListCollection As DesignerActionListCollection

    Public Overrides ReadOnly Property ActionLists() As System.ComponentModel.Design.DesignerActionListCollection
        Get
            If _actionListCollection Is Nothing Then
                _actionListCollection = New DesignerActionListCollection()
                _actionListCollection.Add(New DynamicLabellActionList(Me.Control))
            End If

            Return _actionListCollection
        End Get
    End Property
End Class

Friend Class DynamicLabellActionList
    Inherits DesignerActionList

    Private ctr As ViSnap7_DynamicLabel
    Private designerActionSvc As DesignerActionUIService

    Public Sub New(ByVal component As IComponent)
        MyBase.New(component)

        ctr = DirectCast(component, ViSnap7_DynamicLabel)
        designerActionSvc = CType(GetService(GetType(DesignerActionUIService)), DesignerActionUIService)
    End Sub

#Region " Properties to display in the Smart-Tag panel "

    Public Property PLC_DataArea As General.DataArea
        Get
            Return ctr.PLC_DataArea

        End Get
        Set(value As General.DataArea)
            GetPropertyByName(ctr, "PLC_DataArea").SetValue(ctr, value)
            designerActionSvc.Refresh(ctr)
            If (value <> DataArea.DB) And (PLC_DataType = DataType.STR) Then
                GetPropertyByName(ctr, "PLC_DataType").SetValue(ctr, DataType.INT)
                PLC_DataType = DataType.INT
            End If

        End Set
    End Property

    Public Property PLC_DataType As DataType
        Get
            Return ctr.PLC_DataType

        End Get
        Set(value As DataType)
            GetPropertyByName(ctr, "PLC_DataType").SetValue(ctr, value)
            If (value = DataType.STR) Then
                GetPropertyByName(ctr, "PLC_DataArea").SetValue(ctr, DataArea.DB)
                PLC_DataArea = DataArea.DB
            End If

            designerActionSvc.Refresh(ctr)

        End Set
    End Property

    Public Property PLC_Number() As Integer
        Get
            Return ctr.PLC_Number
        End Get
        Set(ByVal value As Integer)
            GetPropertyByName(ctr, "PLC_Number").SetValue(ctr, value)
            designerActionSvc.Refresh(ctr)

        End Set
    End Property

    Public Property PLC_DB() As Integer
        Get
            Return ctr.PLC_DB
        End Get
        Set(ByVal value As Integer)
            GetPropertyByName(ctr, "PLC_DB").SetValue(ctr, value)
            designerActionSvc.Refresh(ctr)

        End Set
    End Property

    Public Property PLC_Byte() As Integer
        Get
            Return ctr.PLC_Byte
        End Get
        Set(ByVal value As Integer)
            GetPropertyByName(ctr, "PLC_Byte").SetValue(ctr, value)
            designerActionSvc.Refresh(ctr)

        End Set
    End Property

    Public Property PLC_Bit() As Integer
        Get
            Return ctr.PLC_Bit
        End Get
        Set(ByVal value As Integer)
            GetPropertyByName(ctr, "PLC_Bit").SetValue(ctr, value)
            designerActionSvc.Refresh(ctr)

        End Set
    End Property

    Public Property PLC_Length() As Integer
        Get
            Return ctr.PLC_Length
        End Get
        Set(ByVal value As Integer)
            GetPropertyByName(ctr, "PLC_Length").SetValue(ctr, value)
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
        items.Add(New DesignerActionHeaderItem(KPlcAdressingCategory))

        'Add the properties
        items.Add(New DesignerActionPropertyItem("PLC_DataArea", KPlcValueTypeLabel, KPlcAdressingCategory, KPlcTipDataArea))
        items.Add(New DesignerActionPropertyItem("PLC_DataType", KPlcValueTypeLabel, KPlcAdressingCategory, KPlcTipDataType))
        items.Add(New DesignerActionPropertyItem("PLC_Number", KPlcNumberLabel, KPlcAdressingCategory, KPlcTipPlcNumber))
        If PLC_DataArea = DataArea.DB Then
            items.Add(New DesignerActionPropertyItem("PLC_DB", KPlcDBLabel, KPlcAdressingCategory, KPlcTipPlcDB))
        End If
        items.Add(New DesignerActionPropertyItem("PLC_Byte", KPlcByteLabel, KPlcAdressingCategory, KPlcTipPlcByte))

        If PLC_DataType = DataType.BOOL Then
            items.Add(New DesignerActionPropertyItem("PLC_Bit", KPlcBitLabel, KPlcAdressingCategory, KPlcTipPlcBit))
        End If
        If PLC_DataType = DataType.STR Then
            items.Add(New DesignerActionPropertyItem("PLC_Length", KPlcLengthLabel, KPlcAdressingCategory, KPlcTipStrLength))
        End If

        'Return the ActionItemCollection
        Return items
    End Function
End Class

#End Region

