Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Windows.Forms.Design

''' <summary>
''' PLC Textbox user control 
''' </summary>
<System.ComponentModel.Designer(GetType(PLCTextBoxDesigner))>
Class VS7_Textbox
    Inherits TextBox
    Private _PLC As Integer
    Private _DataArea As DataArea = DataArea.DB
    Private _DB As Integer
    Private _Byte As Integer
    Private _Bit As Integer
    Private _DataType As DataType = DataType.INT
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





#End Region
#Region "Control Events"


    Public Sub ControlLeave(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Leave
        pendingWrite = True
    End Sub

    Public Sub ControlGotFocus(ByVal sender As Object, ByVal e As EventArgs) Handles Me.GotFocus
        Me.ControlFocused = True

    End Sub
    Public Sub ControlLostFocus(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LostFocus
        Me.ControlFocused = False
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

            Case DataType.UINT
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

                'Case DataType.USINT
                '    If Char.IsDigit(e.KeyChar) Or Char.IsControl(e.KeyChar) Then
                '        e.Handled = False
                '        If IsNumeric(sender.text) Then
                '            sender.PLC_Value = sender.text
                '        End If
                '        If e.KeyChar = Chr(13) Then
                '            sender.pendingWrite = True
                '        End If
                '        Exit Sub
                '    Else
                '        e.Handled = True
                '    End If
            Case Else
                e.Handled = True
        End Select



    End Sub

#End Region
#Region "Plc reading and writing"
    Public Sub UpdateValue(ByRef _PLC As PlcClient)

        'Reading if control is no pending and not write pending.
        If FirstExecution Or (Not controlFocused And Not pendingWrite) Then
            Select Case Me.PLC_DataArea
                Case DataArea.DB
                    Me.Text = TakeValue(_PLC.DBData(Me.PLC_DB), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)
                    Me.pLC_Value = Me.Text
                Case DataArea.INPUT
                    Me.Text = TakeValue(_PLC.InputData(0), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)
                    Me.pLC_Value = Me.Text
                Case DataArea.MARK
                    Me.Text = TakeValue(_PLC.MarksData(0), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)
                    Me.pLC_Value = Me.Text
                Case DataArea.OUTPUT
                    Me.Text = TakeValue(_PLC.OutputData(0), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)
                    Me.pLC_Value = Me.Text
                Case Else
            End Select
        End If
        'Write in case of pendind write
        If pendingWrite Then
            If (Me.PLC_DataType = DataType.STR) Or (Me.PLC_DataType = DataType.CHR) Or IsNumeric(Me.Text) Then
                pendingWrite = False
                If PLC(PLC_Number).Connected And KGlobalConnectionEnabled Then
                    WriteValue(Me.Text, Me.PLC_Number, Me.PLC_DataArea, Me.PLC_DataType, Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_Length)
                End If

            Else
                pendingWrite = False
                MsgBox(KErrorIntroducingValue)
            End If

        End If
    End Sub
    Private Sub WriteValue(_Text As String, _PLC_Number As Integer, _PLC_DataArea As DataArea, _DataType As DataType, _DB As Integer, _Byte As Integer, _Bit As Integer, _Length As Integer)
        Select Case Me.PLC_DataArea
            Case DataArea.DB
                WriteOnPlc(_Text, _PLC_Number, ViSnap7.S7AreaDB, _DataType, _DB, _Byte, _Bit, _Length)
                Me.PLC_Value = Me.Text
            Case DataArea.INPUT
                WriteOnPlc(_Text, _PLC_Number, ViSnap7.S7AreaPE, _DataType, 0, _Byte, _Bit, _Length)
                Me.PLC_Value = Me.Text
            Case DataArea.MARK
                WriteOnPlc(_Text, _PLC_Number, ViSnap7.S7AreaMK, _DataType, 0, _Byte, _Bit, _Length)
                Me.PLC_Value = Me.Text
            Case DataArea.OUTPUT
                WriteOnPlc(_Text, _PLC_Number, ViSnap7.S7AreaPA, _DataType, 0, _Byte, _Bit, _Length)
                Me.PLC_Value = Me.Text
            Case Else
        End Select

    End Sub

    Sub WriteOnPlc(_Text As String, _PLC_Number As Integer, _DataArea As Byte, _DataType As DataType, _DB As Integer, _Byte As Integer, _Bit As Integer, _Length As Integer)
        Select Case _DataType
            Case DataType.BOOL
                Dim Buffer(0) As Byte
                Buffer(0) = CByte(_Text)
                PLC(_PLC_Number).Client.WriteArea(_DataArea, _DB, _Byte * 8 + _Bit, 1, ViSnap7.S7Consts.S7WLBit, Buffer)

            Case DataType.CHR
                Dim Buffer(0) As Byte
                ViSnap7.S7.SetCharsAt(Buffer, 0, _Text)
                PLC(_PLC_Number).Client.WriteArea(_DataArea, _DB, _Byte, 1, ViSnap7.S7Consts.S7WLChar, Buffer)

            Case DataType.DINT
                Dim Buffer(3) As Byte
                ViSnap7.S7.SetDIntAt(Buffer, 0, _Text)
                PLC(_PLC_Number).Client.WriteArea(_DataArea, _DB, _Byte, 1, ViSnap7.S7Consts.S7WLDInt, Buffer)

            Case DataType.INT
                Dim Buffer(1) As Byte
                ViSnap7.S7.SetIntAt(Buffer, 0, _Text)
                PLC(_PLC_Number).Client.WriteArea(_DataArea, _DB, _Byte, 1, ViSnap7.S7Consts.S7WLInt, Buffer)

            Case DataType.REAL
                Dim Buffer(3) As Byte
                ViSnap7.S7.SetRealAt(Buffer, 0, _Text)
                PLC(_PLC_Number).Client.WriteArea(_DataArea, _DB, _Byte, 1, ViSnap7.S7Consts.S7WLReal, Buffer)

            Case DataType.SINT
                Dim Buffer(0) As Byte
                ViSnap7.S7.SetSIntAt(Buffer, 0, _Text)
                PLC(_PLC_Number).Client.WriteArea(_DataArea, _DB, _Byte, 1, ViSnap7.S7Consts.S7WLByte, Buffer)

            Case DataType.STR
                Dim Buffer(_Length + 1) As Byte
                ViSnap7.S7.SetStringAt(Buffer, 0, _Length, _Text)
                PLC(_PLC_Number).Client.DBWrite(_DB, _Byte, _Length + 2, Buffer)

            Case DataType.UINT

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

        'Return the ActionItemCollection
        Return items
    End Function
End Class

#End Region
