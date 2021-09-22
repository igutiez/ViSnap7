Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Windows.Forms.Design

''' <summary>
''' PLC Textbox user control 
''' </summary>
<System.ComponentModel.Designer(GetType(PLCTextBoxDesigner))>
Public Class VS7_Textbox
    Inherits TextBox
    Public pLC_Value As String = ""
    Public controlFocused As Boolean
    Public pendingWrite As Boolean
    Public updateForm As Boolean
    Public Enum ButtonType
        SET_TRUE = 0
        RESET_FALSE = 1
        SWITCH = 2
        BUTTON = 3
    End Enum
#Region "PLC Properties"
    Private _PLC As Integer
    Private _DataArea As DataArea = DataArea.DB
    Private _DB As Integer
    Private _Byte As Integer
    Private _Bit As Integer
    Private _DataType As DataType = DataType.INT
    Private _Length As Integer
    Private _txt As String
    Private _formNumber As Integer
    Private _formActive As Boolean
    Private _keyboard As Boolean = True

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
    Public Property PLC_DataArea As DataArea
        Get
            Return _DataArea
        End Get
        Set(value As DataArea)
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
    Public Property PLC_DataType As DataType
        Get
            Return _DataType
        End Get
        Set(value As DataType)
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
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcKeyboard)>
    Public Property PLC_keyboard As Boolean
        Get
            Return _keyboard
        End Get
        Set(value As Boolean)
            _keyboard = value
        End Set
    End Property
#End Region
#Region "Control Events"
    Public Sub New()
        Me.Text = ""
    End Sub
    Public Sub ControlLeave(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Leave
        'If the control is not used in a form. 
        'In case of using in a control, pendingwrite will be set in submit form.
        If Not PLC_FormActive Then
            pendingWrite = True
        End If

    End Sub
    Private Sub MadeClick(sender As Object, e As EventArgs) Handles Me.Click
        If Me.PLC_keyboard Then

            Select Case Me.PLC_DataType
                Case DataType.INT, DataType.DINT, DataType.REAL, DataType.USINT, DataType.SINT
                    Dim keyboard As New VS7_NumericKeyboard(Me)
                    keyboard.Show()
                    keyboard.Location = New Point(Me.Parent.Location.X + Me.Location.X + Me.Width, Me.Parent.Location.Y + Me.Location.Y)
                    keyboard.BringToFront()
                    Me.controlFocused = True
                Case DataType.STR, DataType.CHR
                    Dim keyboard As New VS7_AlphanumericKeyboard(Me)
                    keyboard.Show()
                    keyboard.Show()
                    keyboard.Location = New Point(Me.Parent.Location.X + Me.Location.X + Me.Width, Me.Parent.Location.Y + Me.Location.Y)
                    keyboard.BringToFront()


                Case Else

            End Select
        End If

    End Sub
    Public Sub ControlGotFocus(ByVal sender As Object, ByVal e As EventArgs) Handles Me.GotFocus
        Me.controlFocused = True

    End Sub
    Public Sub ControlLostFocus(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LostFocus
        Me.controlFocused = False
    End Sub
    Private Sub ControlIsCreated(sender As Object, e As EventArgs) Handles Me.HandleCreated
        Me.Text = ""
        Me.pLC_Value = Me.Text
    End Sub
    Public Sub FieldEvaluator(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles Me.KeyPress
        Dim separator As Char = culture.NumberFormat.NumberDecimalSeparator

        Select Case sender.PLC_Datatype
            Case DataType.BOOL
                If e.KeyChar = "0" Or e.KeyChar = "1" Or Char.IsControl(e.KeyChar) Then
                    e.Handled = False
                    If IsNumeric(sender.text) Then
                        If sender.text >= 1 Then
                            sender.PLC_Value = 1
                            sender.text = 1
                        Else
                            sender.PLC_Value = 0
                            sender.text = 0
                        End If
                    End If
                    If e.KeyChar = Chr(13) Then
                        sender.pendingWrite = True
                    End If

                    Exit Sub
                Else
                    e.Handled = True
                End If
            Case DataType.STR, DataType.CHR
                If e.KeyChar = Chr(13) Then
                    sender.pendingWrite = True
                End If

                Exit Sub
            Case DataType.INT
                If Char.IsDigit(e.KeyChar) Or e.KeyChar = "-" Or Char.IsControl(e.KeyChar) Then
                    e.Handled = False
                    If IsNumeric(sender.text) Then
                        sender.PLC_Value = sender.text
                    End If
                    If e.KeyChar = Chr(13) Then
                        sender.pendingWrite = True
                    End If

                    Exit Sub
                Else
                    e.Handled = True
                End If
            Case DataType.DINT

                If Char.IsDigit(e.KeyChar) Or e.KeyChar = "-" Or Char.IsControl(e.KeyChar) Then

                    e.Handled = False
                    If IsNumeric(sender.text) Then
                        sender.PLC_Value = sender.text
                    End If
                    If e.KeyChar = Chr(13) Then
                        sender.pendingWrite = True
                    End If

                    Exit Sub
                Else
                    e.Handled = True
                End If
            Case DataType.REAL
                If Char.IsDigit(e.KeyChar) Or e.KeyChar = "-" Or e.KeyChar = separator Or Char.IsControl(e.KeyChar) Then

                    e.Handled = False
                    If IsNumeric(sender.text) Then
                        sender.PLC_Value = CDbl(sender.text)
                    End If
                    If e.KeyChar = Chr(13) Then
                        sender.pendingWrite = True
                    End If


                    Exit Sub
                Else
                    e.Handled = True
                End If

            Case DataType.USINT
                If Char.IsDigit(e.KeyChar) Or e.KeyChar = "-" Or Char.IsControl(e.KeyChar) Or e.KeyChar = Chr(9) Then
                    e.Handled = False
                    If IsNumeric(sender.text) Then
                        sender.PLC_Value = sender.text
                    End If
                    If e.KeyChar = Chr(13) Then
                        sender.pendingWrite = True
                    End If
                    If e.KeyChar = Chr(9) Then
                        MsgBox("tab")
                    End If
                    Exit Sub
                Else
                    e.Handled = True
                End If

            Case Else
                e.Handled = True
        End Select



    End Sub
    Private Sub TextBox_Validated(sender As Object, e As EventArgs) Handles Me.Validated
        Me.pLC_Value = Me.Text
    End Sub
#End Region
#Region "Plc reading and writing"
    Public Sub UpdateControl(ByRef _PLC As PlcClient)
        Dim _valueOk As Boolean

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
        End If
        'Check the value
        _valueOk = True
        If pendingWrite Then


            Select Case Me.PLC_DataType
                Case DataType.USINT
                    If (Me.Text > Byte.MaxValue) Or (Me.Text < Byte.MinValue) Or (Not IsNumeric(Me.Text)) Then
                        _valueOk = False
                    End If
                Case DataType.SINT
                    If (Me.Text > SByte.MaxValue) Or (Me.Text < SByte.MinValue) Or (Not IsNumeric(Me.Text)) Then
                        _valueOk = False
                    End If
                Case DataType.INT
                    If (Me.Text > Int16.MaxValue) Or (Me.Text < Int16.MinValue) Or (Not IsNumeric(Me.Text)) Then
                        _valueOk = False
                    End If
                Case DataType.DINT
                    If (Me.Text > Int32.MaxValue) Or (Me.Text < Int32.MinValue) Or (Not IsNumeric(Me.Text)) Then
                        _valueOk = False
                    End If
                Case DataType.REAL
                    If (Me.Text > Double.MaxValue) Or (Me.Text < Double.MinValue) Or (Not IsNumeric(Me.Text)) Then
                        _valueOk = False
                    End If
                Case DataType.CHR
                    If (Me.Text > Char.MaxValue) Or (Me.Text < Char.MinValue) Then
                        _valueOk = False
                    End If

                Case Else

            End Select
        End If
        If Not _valueOk Then
            pendingWrite = False
            MsgBox(KErrorIntroducingValue)
        End If
        'Write in case of pendind write
        If pendingWrite Then
            If (Me.PLC_DataType = DataType.STR) Or (Me.PLC_DataType = DataType.CHR) Or IsNumeric(Me.Text) Then
                pendingWrite = False
                If plc(PLC_Number).connected And KGlobalConnectionEnabled Then
                    WriteValue(Me.Text, Me.PLC_Number, Me.PLC_DataArea, Me.PLC_DataType, Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_Length)
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

            Case DataType.SINT, DataType.USINT
                Dim Buffer(0) As Byte
                ViSnap7.S7.SetSIntAt(Buffer, 0, _Text)
                plc(_PLC_Number).client.WriteArea(_DataArea, _DB, _Byte, 1, ViSnap7.S7Consts.S7WLByte, Buffer)

            Case DataType.STR
                Dim Buffer(_Length + 1) As Byte
                ViSnap7.S7.SetStringAt(Buffer, 0, _Length, _Text)
                plc(_PLC_Number).client.DBWrite(_DB, _Byte, _Length + 2, Buffer)


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
            Case DataType.STR
                txt = ViSnap7.S7.GetStringAt(_DBData.data, _PLC_Byte)
            Case DataType.USINT
                txt = ViSnap7.S7.GetUSIntAt(_DBData.data, _PLC_Byte)
            Case Else
                txt = ""
        End Select
        Return txt
    End Function


#End Region






End Class





#Region "PLCTextbox Smart tags"

Public Class PLCTextBoxDesigner
    Inherits ControlDesigner

    Private _actionListCollection As DesignerActionListCollection

    Public Overrides ReadOnly Property ActionLists() As System.ComponentModel.Design.DesignerActionListCollection
        Get
            If _actionListCollection Is Nothing Then
                _actionListCollection = New DesignerActionListCollection()
                _actionListCollection.Add(New PLCTextBoxActionList(Me.Control))
            End If

            Return _actionListCollection
        End Get
    End Property
End Class

Friend Class PLCTextBoxActionList
    Inherits DesignerActionList

    Private ctr As VS7_Textbox
    Private designerActionSvc As DesignerActionUIService

    Public Sub New(ByVal component As IComponent)
        MyBase.New(component)

        ctr = DirectCast(component, VS7_Textbox)
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
    Public Property PLC_keyboard() As Boolean
        Get
            Return ctr.PLC_keyboard
        End Get
        Set(ByVal value As Boolean)
            GetPropertyByName(ctr, "PLC_keyboard").SetValue(ctr, value)
            designerActionSvc.Refresh(ctr)

        End Set
    End Property

#End Region

#Region " Methods to display in the Smart-Tag panel "

    Public Sub OnClear()
        ctr.Clear()
    End Sub

    Public Sub OnMakeSquare()
        If ctr.Multiline Then
            If ctr.Width >= ctr.Height Then
                ctr.Height = ctr.Width
            Else
                ctr.Width = ctr.Height
            End If

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
        items.Add(New DesignerActionHeaderItem(KPlcFormCategory))

        'Add the properties
        items.Add(New DesignerActionPropertyItem("PLC_DataArea", KPlcValueTypeLabel, KPlcAdressingCategory, KPLCTipDataArea))
        items.Add(New DesignerActionPropertyItem("PLC_DataType", KPlcValueTypeLabel, KPlcAdressingCategory, KPLCTipDataType))
        items.Add(New DesignerActionPropertyItem("PLC_Number", KPlcNumberLabel, KPlcAdressingCategory, KPLCTipPlcNumber))
        If PLC_DataArea = DataArea.DB Then
            items.Add(New DesignerActionPropertyItem("PLC_DB", KPlcDBLabel, KPlcAdressingCategory, KPLCTipPlcDB))
        End If
        items.Add(New DesignerActionPropertyItem("PLC_Byte", KPlcByteLabel, KPlcAdressingCategory, KPLCTipPlcByte))

        If PLC_DataType = DataType.BOOL Then
            items.Add(New DesignerActionPropertyItem("PLC_Bit", KPlcBitLabel, KPlcAdressingCategory, KPLCTipPlcBit))
        End If
        If PLC_DataType = DataType.STR Then
            items.Add(New DesignerActionPropertyItem("PLC_Length", KPlcLengthLabel, KPlcAdressingCategory, KPlcTipStrLength))
        End If
        items.Add(New DesignerActionPropertyItem("PLC_keyboard", KPlcKeyboard, KPlcFormCategory, KPlcKeyboard))

        items.Add(New DesignerActionPropertyItem("PLC_FormActive", KPlcFormActive, KPlcFormCategory, KPlcTipPlcFormActive))
        If PLC_FormActive Then
            items.Add(New DesignerActionPropertyItem("PLC_FormNumber", KPlcFormNumber, KPlcFormCategory, KPlcTipPlcFormNumber))

        End If

        'Return the ActionItemCollection
        Return items
    End Function
End Class

#End Region
