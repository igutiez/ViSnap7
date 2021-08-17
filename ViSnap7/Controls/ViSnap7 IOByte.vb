Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Windows.Forms.Design



<System.ComponentModel.Designer(GetType(PLCIOByteDesigner))>
Public Class VS7_IOByte

    ''' <summary>
    ''' We uses a partial enum of General.TypeArea taking only Input and output
    ''' </summary>
    Public Enum LocalDataArea
        INPUT = 3
        OUTPUT = 4
    End Enum

    Private _PLC As Integer
    Private _DataArea As LocalDataArea = LocalDataArea.INPUT
    Private _DB As Integer
    Private _Byte As Integer
    Private _Bit As Integer
    Private _ColorTrue As Color = Color.FromKnownColor(KnownColor.Lime)
    Private _ColorFalse As Color = Color.FromKnownColor(KnownColor.Window)

    Public pLC_Value As String
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
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcValueTypeLabel)>
    Public PLC_DataType As General.DataType = DataType.SINT

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
    Public PLC_Length As Integer = 1




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




#End Region


    Public Sub UpdateControl(ByRef _PLC As PlcClient)


        Select Case Me.PLC_DataArea

            Case DataArea.INPUT
                Me.pLC_Value = TakeValue(_PLC.inputData(0), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)


            Case DataArea.OUTPUT
                Me.pLC_Value = TakeValue(_PLC.outputData(0), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)

            Case Else
        End Select

        'Covert the byte into bits
        Dim myBitArray As New BitArray(BitConverter.GetBytes(CInt(Me.pLC_Value)))

        'Each position of the array is evaluated to be shown 

        If myBitArray(0) Then
            Me.Bit0.BackColor = Me.PLC_ColorTrue
        Else
            Me.Bit0.BackColor = Me.PLC_ColorFalse
        End If

        If myBitArray(1) Then
            Me.Bit1.BackColor = Me.PLC_ColorTrue
        Else
            Me.Bit1.BackColor = Me.PLC_ColorFalse
        End If

        If myBitArray(2) Then
            Me.Bit2.BackColor = Me.PLC_ColorTrue
        Else
            Me.Bit2.BackColor = Me.PLC_ColorFalse
        End If

        If myBitArray(3) Then
            Me.Bit3.BackColor = Me.PLC_ColorTrue
        Else
            Me.Bit3.BackColor = Me.PLC_ColorFalse
        End If

        If myBitArray(4) Then
            Me.Bit4.BackColor = Me.PLC_ColorTrue
        Else
            Me.Bit4.BackColor = Me.PLC_ColorFalse
        End If

        If myBitArray(5) Then
            Me.Bit5.BackColor = Me.PLC_ColorTrue
        Else
            Me.Bit5.BackColor = Me.PLC_ColorFalse
        End If

        If myBitArray(6) Then
            Me.Bit6.BackColor = Me.PLC_ColorTrue
        Else
            Me.Bit6.BackColor = Me.PLC_ColorFalse
        End If

        If myBitArray(7) Then
            Me.Bit7.BackColor = Me.PLC_ColorTrue
        Else
            Me.Bit7.BackColor = Me.PLC_ColorFalse
        End If


    End Sub

    Private Function TakeValue(_DBData As PlcClient.ByteData, _PLC_DB As Integer, _PLC_Byte As Integer, _PLC_Bit As Integer, _PLC_DataType As Integer, _PLC_Length As Integer) As String

        Return ViSnap7.S7.GetSIntAt(_DBData.data, _PLC_Byte)
    End Function



End Class


#Region "PLCIOByte Smart tags"

Public Class PLCIOByteDesigner
    Inherits ControlDesigner

    Private _actionListCollection As DesignerActionListCollection

    Public Overrides ReadOnly Property ActionLists() As System.ComponentModel.Design.DesignerActionListCollection
        Get
            If _actionListCollection Is Nothing Then
                _actionListCollection = New DesignerActionListCollection()
                _actionListCollection.Add(New PLCIOByteActionList(Me.Control))
            End If

            Return _actionListCollection
        End Get
    End Property
End Class

Friend Class PLCIOByteActionList
    Inherits DesignerActionList

    Private ctr As VS7_IOByte
    Private designerActionSvc As DesignerActionUIService

    Public Sub New(ByVal component As IComponent)
        MyBase.New(component)

        ctr = DirectCast(component, VS7_IOByte)
        designerActionSvc = CType(GetService(GetType(DesignerActionUIService)), DesignerActionUIService)
    End Sub

#Region " Properties to display in the Smart-Tag panel "

    Public Property PLC_DataArea As VS7_IOByte.LocalDataArea
        Get
            Return ctr.PLC_DataArea

        End Get
        Set(value As VS7_IOByte.LocalDataArea)
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

        'Add the properties
        items.Add(New DesignerActionPropertyItem("PLC_DataArea", KPlcValueTypeLabel, KPlcAdressingCategory, KPlcTipDataArea))
        items.Add(New DesignerActionPropertyItem("PLC_Number", KPlcNumberLabel, KPlcAdressingCategory, KPlcTipPlcNumber))
        items.Add(New DesignerActionPropertyItem("PLC_Byte", KPlcByteLabel, KPlcAdressingCategory, KPlcTipPlcByte))
        items.Add(New DesignerActionPropertyItem("PLC_ColorTrue", KPlcTrueValueLabel, KPlcLedCategory, KPlcTipTrueValue))
        items.Add(New DesignerActionPropertyItem("PLC_ColorFalse", KPlcFalseValueLabel, KPlcLedCategory, KPlcTipFalseValue))

        'Return the ActionItemCollection
        Return items
    End Function
End Class

#End Region
