Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Drawing.Design
Imports System.Windows.Forms.Design




''' <summary>
''' PLC Combobox user control 
''' </summary>
<System.ComponentModel.Designer(GetType(PLCComboboxDesigner))>
Class VS7_ComboBox
    Inherits ComboBox
    Public pLC_Value As String
    Public controlFocused As Boolean
    Public pendingWrite As Boolean
    Public updateForm As Boolean
#Region "PLC Properties"
    Private _PLC As Integer
    Private _DataArea As General.DataArea = DataArea.DB
    Private _DB As Integer
    Private _Byte As Integer
    Private _Bit As Integer
    Private _DataType As General.DataType = DataType.INT
    Private _Length As Integer
    Private _txt As String
    Private _formActive As Boolean
    Private _formNumber As Integer
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
    Public PLC_DataType As General.DataType = DataType.INT


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


    Dim theCollection As New System.Collections.Specialized.StringCollection
    Public Property CustomItems As Collections.Specialized.StringCollection
        Get
            Return theCollection

        End Get
        Set(ByVal value As Collections.Specialized.StringCollection)
            theCollection = value
            Me.Items.Clear()
            For Each txt As String In value
                Me.Items.Add(txt)
            Next

        End Set
    End Property











#End Region
#Region "Control Events"
    Sub New()
        Me.AutoCompleteSource = AutoCompleteSource.CustomSource
        Me.DropDownStyle = ComboBoxStyle.DropDownList
    End Sub
    Private Sub IndexChanged(sender As Object, e As EventArgs) Handles Me.SelectedIndexChanged
        Me.pLC_Value = Me.SelectedIndex
        'If the control is not used in a form. 
        'In case of using in a control, pendingwrite will be set in submit form.
        If Not PLC_FormActive Then
            pendingWrite = True
        End If

    End Sub

    Private Sub ControlGotFocus(sender As Object, e As EventArgs) Handles Me.GotFocus
        controlFocused = True

    End Sub

    Private Sub ControlLostFocus(sender As Object, e As EventArgs) Handles Me.LostFocus
        controlFocused = False

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
                    Me.pLC_Value = TakeValue(_PLC.dbData(Me.PLC_DB), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)

                Case DataArea.INPUT
                    Me.pLC_Value = TakeValue(_PLC.inputData(0), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)

                Case DataArea.MARK
                    Me.pLC_Value = TakeValue(_PLC.marksData(0), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)

                Case DataArea.OUTPUT
                    Me.pLC_Value = TakeValue(_PLC.outputData(0), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)

                Case Else
            End Select
            If (Me.pLC_Value >= -1) And (Me.pLC_Value <= Me.Items.Count - 1) Then
                Me.SelectedIndex = Me.pLC_Value

            End If
        End If
        'Write in case of pendind write
        If pendingWrite Then
            If plc(PLC_Number).connected And KGlobalConnectionEnabled Then
                WriteValue(Me.pLC_Value - 1, Me.PLC_Number, Me.PLC_DataArea, Me.PLC_DataType, Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_Length)
            End If
            pendingWrite = False


        End If
    End Sub

    Public Sub updateValueFromForm(ByVal value As String)
        Me.pLC_Value = CInt(value)
        Me.SelectedIndex = Me.pLC_Value
    End Sub
    Public Sub ClearControl()
        Me.pLC_Value = -1
        Me.SelectedIndex = -1
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

            Case Else
        End Select

    End Sub

    Sub WriteOnPlc(_Text As String, _PLC_Number As Integer, _DataArea As Byte, _DataType As DataType, _DB As Integer, _Byte As Integer, _Bit As Integer, _Length As Integer)

        Dim Buffer(1) As Byte
        ViSnap7.S7.SetIntAt(Buffer, 0, _Text)
        plc(_PLC_Number).client.WriteArea(_DataArea, _DB, _Byte, 1, ViSnap7.S7Consts.S7WLInt, Buffer)
    End Sub

    Private Function TakeValue(_DBData As PlcClient.ByteData, _PLC_DB As Integer, _PLC_Byte As Integer, _PLC_Bit As Integer, _PLC_DataType As Integer, _PLC_Length As Integer) As String
        Dim txt As String = ""
        txt = ViSnap7.S7.GetIntAt(_DBData.data, _PLC_Byte)
        Return txt
    End Function

#End Region
End Class
#Region "PLCCombobox Smart tags"

Public Class PLCComboboxDesigner
    Inherits ControlDesigner

    Private _actionListCollection As DesignerActionListCollection

    Public Overrides ReadOnly Property ActionLists() As System.ComponentModel.Design.DesignerActionListCollection
        Get
            If _actionListCollection Is Nothing Then
                _actionListCollection = New DesignerActionListCollection()
                _actionListCollection.Add(New PLCComboboxActionList(Me.Control))
            End If

            Return _actionListCollection
        End Get
    End Property
End Class

Friend Class PLCComboboxActionList
    Inherits DesignerActionList

    Private ctr As VS7_Combobox
    Private designerActionSvc As DesignerActionUIService


    Public Sub New(ByVal component As IComponent)
        MyBase.New(component)

        ctr = DirectCast(component, VS7_Combobox)
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
    <Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design", GetType(UITypeEditor))>
    Public Property CustomItems As Collections.Specialized.StringCollection
        Get
            Return ctr.CustomItems
        End Get
        Set(ByVal value As Collections.Specialized.StringCollection)
            GetPropertyByName(ctr, "CustomItems").SetValue(ctr, value)
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
        items.Add(New DesignerActionHeaderItem(KPlcFormCategory))

        'Add the properties
        items.Add(New DesignerActionPropertyItem("PLC_UserLevel", KPlcSecLevel, KPlcAdressingCategory, KPlcSecLevel))
        items.Add(New DesignerActionPropertyItem("PLC_DataArea", KPlcValueTypeLabel, KPlcAdressingCategory, KPlcTipDataArea))
        items.Add(New DesignerActionPropertyItem("PLC_Number", KPlcNumberLabel, KPlcAdressingCategory, KPlcTipPlcNumber))
        If PLC_DataArea = DataArea.DB Then
            items.Add(New DesignerActionPropertyItem("PLC_DB", KPlcDBLabel, KPlcAdressingCategory, KPlcTipPlcDB))
        End If
        items.Add(New DesignerActionPropertyItem("PLC_Byte", KPlcByteLabel, KPlcAdressingCategory, KPlcTipPlcByte))

        items.Add(New DesignerActionPropertyItem("CustomItems", KPlcByteLabel, KPlcAdressingCategory, KPlcTipPlcByte))
        items.Add(New DesignerActionPropertyItem("PLC_FormActive", KPlcFormActive, KPlcFormCategory, KPlcTipPlcFormActive))
        If PLC_FormActive Then
            items.Add(New DesignerActionPropertyItem("PLC_FormNumber", KPlcFormNumber, KPlcFormCategory, KPlcTipPlcFormNumber))

        End If
        'Return the ActionItemCollection
        Return items
    End Function
End Class

#End Region
