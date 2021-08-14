Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Windows.Forms.Design

''' <summary>
''' PLC Checkbox user control 
''' </summary>

<System.ComponentModel.Designer(GetType(PLCCheckboxDesigner))>
Class VS7_Checkbox
    Inherits CheckBox
    Private _PLC As Integer
    Private _DataArea As General.DataArea = DataArea.DB
    Private _DB As Integer
    Private _Byte As Integer
    Private _Bit As Integer
    Private _DataType As General.DataType = DataType.BOOL

    Private _Length As Integer
    Private _txt As String
    Public pLC_Value As String
    Public controlFocused As Boolean
    Public pendingWrite As Boolean
#Region "PLC Properties"

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
    Public Sub CheckboxClick(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Click
        If sender.checked Then
            sender.PLC_Value = True

        Else
            sender.PLC_Value = False
        End If
        pendingWrite = True

    End Sub


#End Region
#Region "Plc reading and writing"
    Public Sub UpdateValue(ByRef _PLC As PlcClient)
        'Reading if control is no pending and not write pending.
        If FirstExecution Or (Not controlFocused And Not pendingWrite) Then



            Select Case Me.PLC_DataArea
                Case DataArea.DB
                    Me.Checked = TakeValue(_PLC.DBData(Me.PLC_DB), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)
                    Me.pLC_Value = Me.Checked
                Case DataArea.INPUT
                    Me.Checked = TakeValue(_PLC.InputData(0), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)
                    Me.pLC_Value = Me.Checked
                Case DataArea.MARK
                    Me.Checked = TakeValue(_PLC.MarksData(0), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)
                    Me.pLC_Value = Me.Checked
                Case DataArea.OUTPUT
                    Me.Checked = TakeValue(_PLC.OutputData(0), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)
                    Me.pLC_Value = Me.Checked
                Case Else
            End Select

        End If

        'Write in case of pendind write
        If pendingWrite Then
            pendingWrite = False
            If PLC(PLC_Number).Connected And KGlobalConnectionEnabled Then
                WriteValue(Me.pLC_Value, Me.PLC_Number, Me.PLC_DataArea, Me.PLC_DataType, Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_Length)
            End If

        End If
    End Sub
    Private Sub WriteValue(_Text As String, _PLC_Number As Integer, _PLC_DataArea As DataArea, _DataType As DataType, _DB As Integer, _Byte As Integer, _Bit As Integer, _Length As Integer)
        Select Case Me.PLC_DataArea
            Case DataArea.DB
                WriteOnPlc(_Text, _PLC_Number, ViSnap7.S7AreaDB, _DataType, _DB, _Byte, _Bit, _Length)
                Me.pLC_Value = Me.Checked
            Case DataArea.INPUT
                WriteOnPlc(_Text, _PLC_Number, ViSnap7.S7AreaPE, _DataType, 0, _Byte, _Bit, _Length)
                Me.pLC_Value = Me.Checked
            Case DataArea.MARK
                WriteOnPlc(_Text, _PLC_Number, ViSnap7.S7AreaMK, _DataType, 0, _Byte, _Bit, _Length)
                Me.pLC_Value = Me.Checked
            Case DataArea.OUTPUT
                WriteOnPlc(_Text, _PLC_Number, ViSnap7.S7AreaPA, _DataType, 0, _Byte, _Bit, _Length)
                Me.pLC_Value = Me.Checked
            Case Else
        End Select

    End Sub

    Sub WriteOnPlc(_Text As String, _PLC_Number As Integer, _DataArea As Byte, _DataType As DataType, _DB As Integer, _Byte As Integer, _Bit As Integer, _Length As Integer)
        Select Case _DataType
            Case DataType.BOOL
                Dim Buffer(0) As Byte
                Buffer(0) = CByte(CBool(_Text))
                PLC(_PLC_Number).Client.WriteArea(_DataArea, _DB, _Byte * 8 + _Bit, 1, ViSnap7.S7Consts.S7WLBit, Buffer)

            Case Else

        End Select
    End Sub



    Private Function TakeValue(_DBData As PlcClient.ByteData, _PLC_DB As Integer, _PLC_Byte As Integer, _PLC_Bit As Integer, _PLC_DataType As Integer, _PLC_Length As Integer) As String
        Dim txt As String = ""
        Select Case _PLC_DataType
            Case DataType.BOOL
                txt = ViSnap7.S7.GetBitAt(_DBData.Data, _PLC_Byte, _PLC_Bit)

            Case DataType.CHR
                txt = ViSnap7.S7.GetWCharsAt(_DBData.Data, _PLC_Byte, 1)
            Case DataType.DINT
                txt = ViSnap7.S7.GetDIntAt(_DBData.Data, _PLC_Byte)
            Case DataType.INT
                txt = ViSnap7.S7.GetIntAt(_DBData.Data, _PLC_Byte)
            Case DataType.REAL
                txt = ViSnap7.S7.GetRealAt(_DBData.Data, _PLC_Byte)
            Case DataType.SINT
                txt = ViSnap7.S7.GetSIntAt(_DBData.Data, _PLC_Byte)
            Case DataType.STR
                txt = ViSnap7.S7.GetStringAt(_DBData.Data, _PLC_Byte)
            Case DataType.UINT
                txt = ViSnap7.S7.GetUIntAt(_DBData.Data, _PLC_Byte)
            Case Else
                txt = ""
        End Select
        Return txt
    End Function

#End Region
End Class
#Region "PLCCheckbox Smart tags"
Public Class PLCCheckboxDesigner
    Inherits ControlDesigner

    Private _actionListCollection As DesignerActionListCollection

    Public Overrides ReadOnly Property ActionLists() As System.ComponentModel.Design.DesignerActionListCollection
        Get
            If _actionListCollection Is Nothing Then
                _actionListCollection = New DesignerActionListCollection()
                _actionListCollection.Add(New PLCCheckboxActionList(Me.Control))
            End If

            Return _actionListCollection
        End Get
    End Property
End Class

Friend Class PLCCheckboxActionList
    Inherits DesignerActionList

    Private ctr As VS7_Checkbox
    Private designerActionSvc As DesignerActionUIService

    Public Sub New(ByVal component As IComponent)
        MyBase.New(component)

        ctr = DirectCast(component, VS7_Checkbox)
        designerActionSvc = CType(GetService(GetType(DesignerActionUIService)), DesignerActionUIService)
    End Sub

#Region " Properties to display in the Smart-Tag panel "

    Public Property PLC_DataArea As General.DataArea
        Get
            Return ctr.PLC_DataArea
            If (PLC_DataArea = DataArea.INPUT) Then

                PLC_DataArea = DataArea.MARK
            End If
        End Get
        Set(value As General.DataArea)
            GetPropertyByName(ctr, "PLC_DataArea").SetValue(ctr, value)
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
        items.Add(New DesignerActionPropertyItem("PLC_Number", KPlcNumberLabel, KPlcAdressingCategory, KPlcTipPlcNumber))
        If PLC_DataArea = DataArea.DB Then
            items.Add(New DesignerActionPropertyItem("PLC_DB", KPlcDBLabel, KPlcAdressingCategory, KPlcTipPlcDB))
        End If
        items.Add(New DesignerActionPropertyItem("PLC_Byte", KPlcByteLabel, KPlcAdressingCategory, KPlcTipPlcByte))

        items.Add(New DesignerActionPropertyItem("PLC_Bit", KPlcBitLabel, KPlcAdressingCategory, KPlcTipPlcBit))

        'Return the ActionItemCollection
        Return items
    End Function
End Class

#End Region
