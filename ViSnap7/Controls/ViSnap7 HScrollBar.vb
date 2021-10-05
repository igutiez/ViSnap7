Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Windows.Forms.Design
<System.ComponentModel.Designer(GetType(HScrollBarDesigner))>
Public Class VS7_HScrollBar
    Inherits HScrollBar


    Public pendingWrite As Boolean
    Public updateForm As Boolean

    Public _Maximum As Integer = 100
    Public _Minimum As Integer = 0
    Private _formActive As Boolean
    Private _formNumber As Integer
    Private controlFocused As Boolean
    Private _orientation As Orientation
    Private _readonly As Boolean
    Private _userLever As UserLevels = UserLevels.None

    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcUserLevel)>
    Public Property PLC_UserLevel As UserLevels
        Get
            Return _userLever
        End Get
        Set(value As UserLevels)
            _userLever = value
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcMaximumValue)>
    Public Property PLC_Maximum As Integer
        Get
            Return _Maximum
        End Get
        Set(value As Integer)
            _Maximum = value
            Me.Maximum = Me._Maximum
        End Set
    End Property


    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcReadOnly)>
    Public Property PLC_ReadOnly As Boolean
        Get
            Return _readonly
        End Get
        Set(value As Boolean)
            _readonly = value

            Me.Refresh()
        End Set
    End Property

    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcMaximumValue)>
    Public Property PLC_Minimum As Integer
        Get
            Return _Minimum
        End Get
        Set(value As Integer)
            _Minimum = value
            Me.Minimum = Me.PLC_Minimum
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcFormActive)>
    Public Property PLC_FormActive As Boolean
        Get
            Return _formActive
        End Get
        Set(value As Boolean)
            _formActive = value
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

#Region "Methods"
    Private Sub VS7_Slider_ValueChanged(sender As Object, e As EventArgs) Handles Me.ValueChanged
        If Not PLC_FormActive Then
            If Not _readonly Then
                Me.pLC_Value = Me.Value
                pendingWrite = True
            Else
                Me.Value = Me.pLC_Value
            End If
            Me.Update()
        Else
            Me.pLC_Value = Me.Value
        End If

    End Sub

    Public Sub ControlGotFocus(ByVal sender As Object, ByVal e As EventArgs) Handles Me.GotFocus
        Me.controlFocused = True

    End Sub
    Public Sub ControlLostFocus(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LostFocus
        Me.controlFocused = False
    End Sub
#End Region

#Region "Plc reading and writing"
    Public Sub UpdateControl(ByRef _PLC As PlcClient)
        If ActiveUserLevel < Me.PLC_UserLevel Then
            Me.Enabled = False
        Else
            Me.Enabled = True
        End If

        'Reading if control is no pending and not write pending.
        If (PLC_FormActive And updateForm) Or (Not PLC_FormActive And (firstExecution Or (Not controlFocused And Not pendingWrite))) Then
            updateForm = False

            Select Case Me.PLC_DataArea
                Case DataArea.DB
                    Me.Text = TakeValue(_PLC.dbData(Me.PLC_DB), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)
                    Me.pLC_Value = Me.Text
                Case DataArea.INPUT
                    Me.Text = TakeValue(_PLC.inputData(0), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)
                    Me.pLC_Value = Me.Text
                Case DataArea.MARK
                    Me.Text = TakeValue(_PLC.marksData(0), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)
                    Me.pLC_Value = Me.Text
                Case DataArea.OUTPUT
                    Me.Text = TakeValue(_PLC.outputData(0), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)
                    Me.pLC_Value = Me.Text
                Case Else
            End Select
            Try
                Me.Value = CInt(Me.pLC_Value)
            Catch ex As Exception

            End Try

        End If
        'Write in case of pendind write
        If pendingWrite Then
            If (Me.PLC_DataType = DataType.STR) Or (Me.PLC_DataType = DataType.CHR) Or IsNumeric(Me.Text) Then
                pendingWrite = False
                If plc(PLC_Number).connected And KGlobalConnectionEnabled Then
                    WriteValue(Me.pLC_Value, Me.PLC_Number, Me.PLC_DataArea, Me.PLC_DataType, Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_Length)
                End If

            Else
                pendingWrite = False
                MsgBox(KErrorIntroducingValue)
            End If

        End If
    End Sub

    Public Sub UpdateValueFromForm(ByVal value As String)
        Me.pLC_Value = value
        Me.Text = value
    End Sub

    Public Sub ClearControl()
        Me.pLC_Value = ""
        Me.Text = ""
    End Sub
    Private Sub WriteValue(_Text As String, _PLC_Number As Integer, _PLC_DataArea As DataArea, _DataType As DataType, _DB As Integer, _Byte As Integer, _Bit As Integer, _Length As Integer)
        Select Case Me.PLC_DataArea
            Case DataArea.DB
                WriteOnPlc(_Text, _PLC_Number, ViSnap7.S7AreaDB, _DataType, _DB, _Byte, _Bit, _Length)
                Me.pLC_Value = Me.Text
            Case DataArea.INPUT
                WriteOnPlc(_Text, _PLC_Number, ViSnap7.S7AreaPE, _DataType, 0, _Byte, _Bit, _Length)
                Me.pLC_Value = Me.Text
            Case DataArea.MARK
                WriteOnPlc(_Text, _PLC_Number, ViSnap7.S7AreaMK, _DataType, 0, _Byte, _Bit, _Length)
                Me.pLC_Value = Me.Text
            Case DataArea.OUTPUT
                WriteOnPlc(_Text, _PLC_Number, ViSnap7.S7AreaPA, _DataType, 0, _Byte, _Bit, _Length)
                Me.pLC_Value = Me.Text
            Case Else
        End Select

    End Sub

    Sub WriteOnPlc(_Text As String, _PLC_Number As Integer, _DataArea As Byte, _DataType As DataType, _DB As Integer, _Byte As Integer, _Bit As Integer, _Length As Integer)
        Select Case _DataType
            Case DataType.BOOL
                Dim Buffer(0) As Byte
                Buffer(0) = CByte(_Text)
                plc(_PLC_Number).client.WriteArea(_DataArea, _DB, _Byte * 8 + _Bit, 1, ViSnap7.S7Consts.S7WLBit, Buffer)

            Case DataType.CHR
                Dim Buffer(0) As Byte
                ViSnap7.S7.SetCharsAt(Buffer, 0, _Text)
                plc(_PLC_Number).client.WriteArea(_DataArea, _DB, _Byte, 1, ViSnap7.S7Consts.S7WLChar, Buffer)

            Case DataType.DINT
                Dim Buffer(3) As Byte
                ViSnap7.S7.SetDIntAt(Buffer, 0, _Text)
                plc(_PLC_Number).client.WriteArea(_DataArea, _DB, _Byte, 1, ViSnap7.S7Consts.S7WLDInt, Buffer)

            Case DataType.INT
                Dim Buffer(1) As Byte
                ViSnap7.S7.SetIntAt(Buffer, 0, _Text)
                plc(_PLC_Number).client.WriteArea(_DataArea, _DB, _Byte, 1, ViSnap7.S7Consts.S7WLInt, Buffer)

            Case DataType.REAL
                Dim Buffer(3) As Byte
                ViSnap7.S7.SetRealAt(Buffer, 0, _Text)
                plc(_PLC_Number).client.WriteArea(_DataArea, _DB, _Byte, 1, ViSnap7.S7Consts.S7WLReal, Buffer)

            Case DataType.USINT
                Dim Buffer(0) As Byte
                ViSnap7.S7.SetUSIntAt(Buffer, 0, _Text)
                plc(_PLC_Number).client.WriteArea(_DataArea, _DB, _Byte, 1, ViSnap7.S7Consts.S7WLByte, Buffer)

            Case DataType.STR
                Dim Buffer(_Length + 1) As Byte
                ViSnap7.S7.SetStringAt(Buffer, 0, _Length, _Text)
                plc(_PLC_Number).client.DBWrite(_DB, _Byte, _Length + 2, Buffer)

            Case DataType.SINT
                Dim Buffer(0) As Byte
                ViSnap7.S7.SetSIntAt(Buffer, 0, _Text)
                plc(_PLC_Number).client.WriteArea(_DataArea, _DB, _Byte, 1, ViSnap7.S7Consts.S7WLByte, Buffer)


            Case Else

        End Select
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

            Case DataType.USINT
                txt = ViSnap7.S7.GetUSIntAt(_DBData.data, _PLC_Byte)
            Case Else
                txt = ""
        End Select
        Return txt
    End Function
#End Region
End Class

Partial Public Class VS7_HScrollBar
    Inherits HScrollBar
#Region "PLC Properties"
    Public Enum LocalDatatype
        UINT = 2
        SINT = 3
        INT = 4
        DINT = 5
        REAL = 6
    End Enum
    Private _PLC As Integer
    Private _DataArea As General.DataArea = DataArea.DB
    Private _DB As Integer
    Private _Byte As Integer
    Private _Bit As Integer
    Private _DataType As LocalDatatype = LocalDatatype.INT
    Private _Length As Integer
    Public pLC_Value As String
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

End Class


Public Class HScrollBarDesigner
    Inherits ControlDesigner

    Private _actionListCollection As DesignerActionListCollection

    Public Overrides ReadOnly Property ActionLists() As System.ComponentModel.Design.DesignerActionListCollection
        Get
            If _actionListCollection Is Nothing Then
                _actionListCollection = New DesignerActionListCollection()
                _actionListCollection.Add(New HScrollActionList(Me.Control))
            End If

            Return _actionListCollection
        End Get
    End Property
End Class

Friend Class HScrollActionList
    Inherits DesignerActionList

    Private ctr As VS7_HScrollBar
    Private designerActionSvc As DesignerActionUIService

    Public Sub New(ByVal component As IComponent)
        MyBase.New(component)

        ctr = DirectCast(component, VS7_HScrollBar)
        designerActionSvc = CType(GetService(GetType(DesignerActionUIService)), DesignerActionUIService)
    End Sub

#Region " Properties to display in the Smart-Tag panel "
    Public Property PLC_UserLevel() As General.UserLevels
        Get
            Return ctr.PLC_UserLevel
        End Get
        Set(ByVal value As General.UserLevels)
            GetPropertyByName(ctr, "PLC_UserLevel").SetValue(ctr, value)
            designerActionSvc.Refresh(ctr)

        End Set
    End Property
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

    Public Property PLC_DataType As VS7_Slider.LocalDatatype
        Get
            Return ctr.PLC_DataType

        End Get
        Set(value As VS7_Slider.LocalDatatype)
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
    Public Property PLC_Maximum() As Integer
        Get
            Return ctr.PLC_Maximum
        End Get
        Set(ByVal value As Integer)
            GetPropertyByName(ctr, "PLC_Maximum").SetValue(ctr, value)
            designerActionSvc.Refresh(ctr)

        End Set
    End Property
    Public Property PLC_Minimum() As Integer
        Get
            Return ctr.PLC_Minimum
        End Get
        Set(ByVal value As Integer)
            GetPropertyByName(ctr, "PLC_Minimum").SetValue(ctr, value)
            designerActionSvc.Refresh(ctr)

        End Set
    End Property

    Public Property PLC_FormActive() As Boolean
        Get
            Return ctr.PLC_FormActive
        End Get
        Set(ByVal value As Boolean)
            GetPropertyByName(ctr, "PLC_FormActive").SetValue(ctr, value)
            designerActionSvc.Refresh(ctr)

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

    Public Property PLC_ReadOnly As Boolean
        Get
            Return ctr.PLC_ReadOnly
        End Get
        Set(ByVal value As Boolean)
            GetPropertyByName(ctr, "PLC_ReadOnly").SetValue(ctr, value)
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
        items.Add(New DesignerActionHeaderItem(KSliderCategory))

        'Add the properties
        items.Add(New DesignerActionPropertyItem("PLC_UserLevel", KPlcSecLevel, KPlcAdressingCategory, KPlcSecLevel))
        items.Add(New DesignerActionPropertyItem("PLC_DataArea", KPlcValueTypeLabel, KPlcAdressingCategory, KPlcTipDataArea))
        items.Add(New DesignerActionPropertyItem("PLC_DataType", KPlcValueTypeLabel, KPlcAdressingCategory, KPlcTipDataType))
        items.Add(New DesignerActionPropertyItem("PLC_Number", KPlcNumberLabel, KPlcAdressingCategory, KPlcTipPlcNumber))
        If PLC_DataArea = DataArea.DB Then
            items.Add(New DesignerActionPropertyItem("PLC_DB", KPlcDBLabel, KPlcAdressingCategory, KPlcTipPlcDB))
        End If
        items.Add(New DesignerActionPropertyItem("PLC_Byte", KPlcByteLabel, KPlcAdressingCategory, KPlcTipPlcByte))
        items.Add(New DesignerActionPropertyItem("PLC_Maximum", KPlcSliderMax, KSliderCategory, KPlcTipPLC_Maximum))
        items.Add(New DesignerActionPropertyItem("PLC_Minimum", KPlcSliderMin, KSliderCategory, KPlcTipPLC_Minimum))
        items.Add(New DesignerActionPropertyItem("PLC_ReadOnly", KPlcReadOnly, KSliderCategory, KPlcReadOnly))
        items.Add(New DesignerActionPropertyItem("PLC_FormActive", KPlcFormActive, KPlcFormCategory, KPlcTipPlcFormActive))
        If PLC_FormActive Then
            items.Add(New DesignerActionPropertyItem("PLC_FormNumber", KPlcFormNumber, KPlcFormCategory, KPlcTipPlcFormNumber))

        End If



        'Return the ActionItemCollection
        Return items
    End Function
End Class
