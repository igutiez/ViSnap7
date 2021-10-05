Imports System.Windows.Forms.Design
<System.ComponentModel.Designer(GetType(IncreaseDecreaseDesigner))>
Public Class VS7_IncreaseDecrease

    Public Enum LocalDataArea
        MARK = 1
        DB = 2
    End Enum

#Region "PLC Properties"
    Public pendingWrite As Boolean
    Public pLC_Value As String
    Private _PLC As Integer
    Private _DataArea As LocalDataArea = LocalDataArea.MARK
    Private _DB As Integer
    Private _Byte As Integer
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
    Public Property PLC_DataArea As LocalDataArea
        Get
            Return _DataArea
        End Get
        Set(value As LocalDataArea)
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

    Public PLC_DataType As DataType = DataType.INT
    Public PLC_Bit As Integer = 0
    Public PLC_Length As Integer = 0

#End Region


#Region "Plc reading and writing"
    Public Sub UpdateControl(ByRef _PLC As PlcClient)
        If ActiveUserLevel < Me.PLC_UserLevel Then
            Me.Enabled = False
        Else
            Me.Enabled = True
        End If
        'Reading if control is no pending and not write pending.
        If (Not firstExecution And Not pendingWrite) Then

            Select Case Me.PLC_DataArea
                Case DataArea.DB
                    Me.pLC_Value = TakeValue(_PLC.dbData(Me.PLC_DB), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)
                Case DataArea.MARK
                    Me.pLC_Value = TakeValue(_PLC.marksData(0), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)
                Case Else
            End Select
            Me.Counter.Text = Me.pLC_Value
        End If
        'Write in case of pendind write
        If pendingWrite Then
            pendingWrite = False
            If plc(PLC_Number).connected And KGlobalConnectionEnabled Then
                WriteValue(Me.pLC_Value, Me.PLC_Number, Me.PLC_DataArea, Me.PLC_DataType, Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_Length)
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
            Case DataArea.MARK
                WriteOnPlc(_Text, _PLC_Number, ViSnap7.S7AreaMK, _DataType, 0, _Byte, _Bit, _Length)
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

    Private Sub Increase_Click(sender As Object, e As EventArgs) Handles Increase.Click
        If IsNumeric(Me.pLC_Value) Then
            Me.pLC_Value = CInt(Me.pLC_Value) + 1
            pendingWrite = True
        End If


    End Sub

    Private Sub Decrease_Click(sender As Object, e As EventArgs) Handles Decrease.Click
        If IsNumeric(Me.pLC_Value) Then
            Me.pLC_Value = CInt(Me.pLC_Value) - 1
            pendingWrite = True
        End If

    End Sub
#End Region
End Class

#Region "PLCTextbox Smart tags"

Public Class IncreaseDecreaseDesigner
    Inherits ControlDesigner

    Private _actionListCollection As DesignerActionListCollection

    Public Overrides ReadOnly Property ActionLists() As System.ComponentModel.Design.DesignerActionListCollection
        Get
            If _actionListCollection Is Nothing Then
                _actionListCollection = New DesignerActionListCollection()
                _actionListCollection.Add(New IncreaseDecreaseActionList(Me.Control))
            End If

            Return _actionListCollection
        End Get
    End Property
End Class

Friend Class IncreaseDecreaseActionList
    Inherits DesignerActionList

    Private ctr As VS7_IncreaseDecrease
    Private designerActionSvc As DesignerActionUIService

    Public Sub New(ByVal component As IComponent)
        MyBase.New(component)

        ctr = DirectCast(component, VS7_IncreaseDecrease)
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
    Public Property PLC_DataArea As VS7_IncreaseDecrease.LocalDataArea
        Get
            Return ctr.PLC_DataArea

        End Get
        Set(value As VS7_IncreaseDecrease.LocalDataArea)
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
        items.Add(New DesignerActionPropertyItem("PLC_UserLevel", KPlcSecLevel, KPlcAdressingCategory, KPlcSecLevel))
        items.Add(New DesignerActionPropertyItem("PLC_DataArea", KPlcValueTypeLabel, KPlcAdressingCategory, KPlcTipDataArea))
        items.Add(New DesignerActionPropertyItem("PLC_Number", KPlcNumberLabel, KPlcAdressingCategory, KPlcTipPlcNumber))
        If PLC_DataArea = VS7_IncreaseDecrease.LocalDataArea.DB Then
            items.Add(New DesignerActionPropertyItem("PLC_DB", KPlcDBLabel, KPlcAdressingCategory, KPlcTipPlcDB))
        End If
        items.Add(New DesignerActionPropertyItem("PLC_Byte", KPlcByteLabel, KPlcAdressingCategory, KPlcTipPlcByte))


        'Return the ActionItemCollection
        Return items
    End Function
End Class

#End Region
