Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Windows.Forms.Design

''' <summary>
''' PLC Led user control 
''' </summary>
<System.ComponentModel.Designer(GetType(PLCLedDesigner))>
Class VS7_Led
    Inherits Panel
    Public pLC_Value As Boolean = False
    Public controlFocused As Boolean
    Public pendingWrite As Boolean
    Friend Enum ShapeType
        Normal
        Circular
    End Enum
#Region "PLC Properties"
    Private _PLC As Integer
    Private _DataArea As General.DataArea = DataArea.DB
    Private _DB As Integer
    Private _Byte As Integer
    Private _Bit As Integer
    Private _DataType As General.DataType = DataType.BOOL
    Private _Length As Integer
    Private _txt As String
    Private _ColorTrue As Color = Color.FromKnownColor(KnownColor.Lime)
    Private _ColorFalse As Color = Color.FromKnownColor(KnownColor.Window)
    Private g As Graphics
    Private _shapeType As ShapeType
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

    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcShapeType)>
    Public Property PLC_ShapeType As ShapeType
        Get
            Return _shapeType
        End Get
        Set(value As ShapeType)
            _shapeType = value

            Circle()

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




#End Region
#Region "Control Events"
    Sub New()
        Me.BorderStyle = Windows.Forms.BorderStyle.FixedSingle
        Me.Size = New Point(25, 25)
    End Sub
#End Region
#Region "Control Procedures"
    Sub Circle()
        If Me.PLC_ShapeType = ShapeType.Circular Then

            Dim x = CSng(Me.Width - 2)
            Dim y = CSng(Me.Height - 2)
            Dim colorfondo As New SolidBrush(Color.Transparent)

            Me.BackColor = Color.Transparent
            Me.BorderStyle = BorderStyle.None


            g = Me.CreateGraphics

            g.DrawEllipse(Pens.Black, 1, 1, x, y)
            If Me.pLC_Value Then
                colorfondo.Color = _ColorTrue

            Else
                colorfondo.Color = _ColorFalse

            End If

            g.FillEllipse(colorfondo, 2, 2, x - 2, y - 2)

        Else
            Me.BorderStyle = BorderStyle.FixedSingle


            If Me.pLC_Value Then
                Me.BackColor = _ColorTrue

            Else
                Me.BackColor = _ColorFalse

            End If
        End If
    End Sub
    Sub Redimension(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Resize
        Circle()
    End Sub
    Private Sub InitializeComponent()
        Me.SuspendLayout()
        '
        'Leds
        '
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ResumeLayout(False)

    End Sub
    Sub PaintLed(sender As Object, e As EventArgs) Handles Me.Paint
        Circle()
    End Sub

    'Public Sub repaint(sender As Object, e As EventArgs) Handles Me.ParentChanged
    '    Circle()
    'End Sub


#End Region
#Region "Plc reading and writing"
    Public Sub UpdateControl(ByRef _PLC As PlcClient)


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
        If CBool(Me.pLC_Value) Then
            Me.BackColor = Me.PLC_ColorTrue
        Else
            Me.BackColor = Me.PLC_ColorFalse
        End If
        Circle()
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

#End Region
End Class
#Region "PLCLed Smart tags"

Public Class PLCLedDesigner
    Inherits ControlDesigner

    Private _actionListCollection As DesignerActionListCollection

    Public Overrides ReadOnly Property ActionLists() As System.ComponentModel.Design.DesignerActionListCollection
        Get
            If _actionListCollection Is Nothing Then
                _actionListCollection = New DesignerActionListCollection()
                _actionListCollection.Add(New PLCLedActionList(Me.Control))
            End If

            Return _actionListCollection
        End Get
    End Property
End Class

Friend Class PLCLedActionList
    Inherits DesignerActionList

    Private ctr As VS7_Led
    Private designerActionSvc As DesignerActionUIService

    Public Sub New(ByVal component As IComponent)
        MyBase.New(component)

        ctr = DirectCast(component, VS7_Led)
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
    Public Property PLC_ShapeType() As VS7_Led.ShapeType
        Get
            Return ctr.PLC_ShapeType
        End Get
        Set(ByVal value As VS7_Led.ShapeType)

            GetPropertyByName(ctr, "PLC_ShapeType").SetValue(ctr, value)

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
        If PLC_DataArea = DataArea.DB Then
            items.Add(New DesignerActionPropertyItem("PLC_DB", KPlcDBLabel, KPlcAdressingCategory, KPlcTipPlcDB))
        End If
        items.Add(New DesignerActionPropertyItem("PLC_Byte", KPlcByteLabel, KPlcAdressingCategory, KPlcTipPlcByte))

        items.Add(New DesignerActionPropertyItem("PLC_Bit", KPlcBitLabel, KPlcAdressingCategory, KPlcTipPlcBit))
        items.Add(New DesignerActionPropertyItem("PLC_ShapeType", KPlcShapetypeLabel, KPlcLedCategory, KPlcTipShapeType))
        items.Add(New DesignerActionPropertyItem("PLC_ColorTrue", KPlcTrueValueLabel, KPlcLedCategory, KPlcTipTrueValue))
        items.Add(New DesignerActionPropertyItem("PLC_ColorFalse", KPlcFalseValueLabel, KPlcLedCategory, KPlcTipFalseValue))

        'Return the ActionItemCollection
        Return items
    End Function
End Class

#End Region
