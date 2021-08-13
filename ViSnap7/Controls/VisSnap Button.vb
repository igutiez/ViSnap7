Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Windows.Forms.Design

''' <summary>
''' PLC Button user control 
''' </summary>

<System.ComponentModel.Designer(GetType(PLCButtonDesigner))>
Class VS7_Button
    Inherits Button
    Private _plc As Integer
    Private _dataArea As General.DataArea = DataArea.DB
    Private _db As Integer
    Private _byte As Integer
    Private _bit As Integer
    Private _dataType As General.DataType = DataType.BOOL
    Private _length As Integer
    Private _txt As String
    Private _ColorTrue As Color = Color.FromKnownColor(KnownColor.Lime)
    Private _colorFalse As Color = Color.FromKnownColor(KnownColor.Window)
    Private _buttonType As Boolean
    Private _Caption As String

    Public plc_Value As String
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
            Return _dataArea
        End Get
        Set(value As General.DataArea)
            _dataArea = value
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
            Return _dataType
        End Get
        Set(value As General.DataType)
            _dataType = value

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
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcColorTrue)>
    Public Property PLC_ColorTrue As Color
        Get
            Return _ColorTrue
        End Get
        Set(value As Color)
            _ColorTrue = value
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcColorFalse)>
    Public Property PLC_ColorFalse As Color
        Get
            Return _ColorFalse
        End Get
        Set(value As Color)
            _ColorFalse = value
        End Set
    End Property

    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcTipButtonType)>
    Public Property PLC_ButtonType As Boolean
        Get
            Return _buttonType
        End Get
        Set(value As Boolean)
            _buttonType = value
        End Set
    End Property

    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcTipCaptionType)>
    Public Overrides Property Text As String
        Get
            Return _Caption
        End Get
        Set(value As String)
            _Caption = value
        End Set
    End Property


#End Region

#Region "Control Events"
    Public Sub ButtonClick(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Click
        If Not sender.PLC_ButtonType Then

            If sender.PLC_Value Then
                sender.PLC_Value = False

            Else
                sender.PLC_Value = True
            End If
            pendingWrite = True
        End If
    End Sub
    Public Sub ButtonDown(ByVal sender As Object, ByVal e As EventArgs) Handles Me.MouseDown
        If sender.PLC_ButtonType Then
            sender.PLC_Value = True
            pendingWrite = True
        End If

    End Sub

    Public Sub ButtonUp(ByVal sender As Object, ByVal e As EventArgs) Handles Me.MouseUp
        If sender.PLC_ButtonType Then

            sender.PLC_Value = False
            pendingWrite = True
        End If

    End Sub

#End Region
#Region "Plc reading and writing"
    Public Sub UpdateValue(ByRef _PLC As PlcClient)
        'Reading if control is no pending and not write pending.
        If FirstExecution Or (Not controlFocused And Not pendingWrite) Then



            Select Case Me.PLC_DataArea
                Case DataArea.DB
                    Me.pLC_Value = TakeValue(_PLC.DBData(Me.PLC_DB), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)

                Case DataArea.INPUT
                    Me.pLC_Value = TakeValue(_PLC.InputData(0), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)

                Case DataArea.MARK
                    Me.pLC_Value = TakeValue(_PLC.MarksData(0), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)

                Case DataArea.OUTPUT
                    Me.pLC_Value = TakeValue(_PLC.OutputData(0), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)

                Case Else
            End Select

            'Updating the button color
            If CBool(Me.pLC_Value) Then
                Me.BackColor = _ColorTrue
            Else
                Me.BackColor = _colorFalse
            End If
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

            Case DataArea.INPUT
                WriteOnPlc(_Text, _PLC_Number, ViSnap7.S7AreaPE, _DataType, 0, _Byte, _Bit, _Length)

            Case DataArea.MARK
                WriteOnPlc(_Text, _PLC_Number, ViSnap7.S7AreaMK, _DataType, 0, _Byte, _Bit, _Length)

            Case DataArea.OUTPUT
                WriteOnPlc(_Text, _PLC_Number, ViSnap7.S7AreaPA, _DataType, 0, _Byte, _Bit, _Length)

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
        Dim txt As String
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





#Region "PLCButton Smart tags"

Public Class PLCButtonDesigner
    Inherits ControlDesigner

    Private _actionListCollection As DesignerActionListCollection

    Public Overrides ReadOnly Property ActionLists() As System.ComponentModel.Design.DesignerActionListCollection
        Get
            If _actionListCollection Is Nothing Then
                _actionListCollection = New DesignerActionListCollection()
                _actionListCollection.Add(New PLCButtonActionList(Me.Control))
            End If

            Return _actionListCollection
        End Get
    End Property
End Class

Friend Class PLCButtonActionList
    Inherits DesignerActionList

    Private ctr As VS7_Button
    Private designerActionSvc As DesignerActionUIService

    Public Sub New(ByVal component As IComponent)
        MyBase.New(component)

        ctr = DirectCast(component, VS7_Button)
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

    Public Property PLC_ColorTrue() As Color
        Get
            Return ctr.PLC_ColorTrue
        End Get
        Set(ByVal value As Color)
            GetPropertyByName(ctr, "PLC_ColorTrue").SetValue(ctr, value)

        End Set

    End Property
    Public Property PLC_ColorFalse() As Color
        Get
            Return ctr.PLC_ColorFalse
        End Get
        Set(ByVal value As Color)
            GetPropertyByName(ctr, "PLC_ColorFalse").SetValue(ctr, value)

        End Set
    End Property

    Public Property PLC_ButtonType() As Boolean
        Get
            Return ctr.PLC_ButtonType
        End Get
        Set(ByVal value As Boolean)
            GetPropertyByName(ctr, "PLC_ButtonType").SetValue(ctr, value)

        End Set
    End Property

    Public Property Text As String
        Get
            Return ctr.Text
        End Get
        Set(ByVal value As String)
            GetPropertyByName(ctr, "Text").SetValue(ctr, value)

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
        items.Add(New DesignerActionHeaderItem(KPlcLedCategory))
        items.Add(New DesignerActionHeaderItem(KPlcButtonCategory))

        'Add the properties
        items.Add(New DesignerActionPropertyItem("PLC_DataArea", KPlcValueTypeLabel, KPlcAdressingCategory, KPlcTipDataArea))
        items.Add(New DesignerActionPropertyItem("PLC_Number", KPlcNumberLabel, KPlcAdressingCategory, KPlcTipPlcNumber))
        If PLC_DataArea = DataArea.DB Then
            items.Add(New DesignerActionPropertyItem("PLC_DB", KPlcDBLabel, KPlcAdressingCategory, KPlcTipPlcDB))
        End If
        items.Add(New DesignerActionPropertyItem("PLC_Byte", KPlcByteLabel, KPlcAdressingCategory, KPlcTipPlcByte))

        items.Add(New DesignerActionPropertyItem("PLC_Bit", KPlcBitLabel, KPlcAdressingCategory, KPlcTipPlcBit))
        items.Add(New DesignerActionPropertyItem("PLC_ColorTrue", KPlcTrueValueLabel, KPlcLedCategory, KPlcTipTrueValue))
        items.Add(New DesignerActionPropertyItem("PLC_ColorFalse", KPlcFalseValueLabel, KPlcLedCategory, KPlcTipFalseValue))

        items.Add(New DesignerActionPropertyItem("Text", KPlcButtonCaption, KPlcButtonCategory, KPlcTipCaptionType))
        items.Add(New DesignerActionPropertyItem("PLC_ButtonType", KPlcButtonTypeLabel, KPlcButtonCategory, KPlcTipButtonType))

        'Return the ActionItemCollection
        Return items
    End Function
End Class

#End Region
