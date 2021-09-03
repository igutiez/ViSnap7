Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Windows.Forms.Design
<System.ComponentModel.Designer(GetType(TrendsDesigner))>
Public Class VS7_Trends
#Region "PLC Properties"
    Public pLC_Value As String

    Private _PLC As Integer
    Private _DataArea As General.DataArea = DataArea.MARK
    Private _DB As Integer = 1
    Private _Byte As Integer = 0
    Private _Bit As Integer
    Private _DataType As General.DataType = DataType.INT
    Private _Length As Integer
    Private _txt As String
    Private _SerieName As String = "Example"
    Private _TimeInterval As Integer = 1000
    Private _YAxis As String = "Y"
    Private _XAxis As String = "X"
    Private Enum NumSeries
        Serie0
        Serie1
        Serie2
        Serie3
        Serie4
    End Enum
    Private _RegisterNumbers As Integer = 60

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

    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPLC_SerieName)>
    Public Property PLC_SerieName As String
        Get
            Return _SerieName
        End Get
        Set(value As String)
            _SerieName = value
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPLC_RegisterNumbers)>
    Public Property PLC_RegisterNumbers As Integer
        Get
            Return _RegisterNumbers
        End Get
        Set(value As Integer)
            _RegisterNumbers = value
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPLC_TimeInterval)>
    Public Property PLC_TimeInterval As Integer
        Get
            Return _TimeInterval
        End Get
        Set(value As Integer)
            _TimeInterval = value
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPLC_SerieYAxis)>
    Public Property PLC_YAxis As String
        Get
            Return _YAxis
        End Get
        Set(value As String)
            _YAxis = value
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPLC_SerieXAxis)>
    Public Property PLC_XAxis As String
        Get
            Return _XAxis
        End Get
        Set(value As String)
            _XAxis = value
        End Set
    End Property

#End Region

    Structure DataPoint
        Public X As Date
        Public Y As Double
    End Structure
    Private MyArray(PLC_RegisterNumbers) As DataPoint


    Public Sub New()

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().


    End Sub


#Region "Plc reading and writing"
    Public Sub UpdateControl(ByRef _PLC As PlcClient)


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
        End Select
        If firstExecution Then
            ReDim MyArray(PLC_RegisterNumbers)
            Trend()
            Timer1.Interval = Me.PLC_TimeInterval
            With MyChart.ChartAreas(0)
                .AxisX.Title = Me.PLC_XAxis
                .AxisY.Title = Me.PLC_YAxis
            End With
            Timer1.Start()
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

#End Region

#Region "Trends"
    Sub Trend()
        Text = "Sample Chart"
        MyChart.Dock = DockStyle.Fill

        'setup the chart
        ' MyChart.ChartAreas.Add(0)

        With MyChart.ChartAreas(0)
            .AxisX.Title = "X"
            .AxisX.MajorGrid.LineColor = Color.LightBlue
            .AxisX.Minimum = 0
            .AxisX.Interval = 2
            .AxisY.Title = "Y"
            .AxisY.MajorGrid.LineColor = Color.LightGray
            .AxisY.Minimum = 0
            .BackColor = Color.FloralWhite
            .BackSecondaryColor = Color.White
            .BackGradientStyle = GradientStyle.HorizontalCenter
            .BorderColor = Color.Blue
            .BorderDashStyle = ChartDashStyle.Solid
            .BorderWidth = 1
            .ShadowOffset = 2
        End With

        ' draw the chart
        MyChart.Series.Clear()
        MyChart.Series.Add(PLC_SerieName)
        With MyChart.Series(0)
            .ChartType = DataVisualization.Charting.SeriesChartType.Line
            .BorderWidth = 1
            .Color = Color.Red
            .BorderDashStyle = ChartDashStyle.Dash
            .MarkerStyle = DataVisualization.Charting.MarkerStyle.Square
            .MarkerSize = 4
        End With
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        For c = 0 To PLC_RegisterNumbers - 1
            MyArray(c) = MyArray(c + 1)
        Next
        MyArray(PLC_RegisterNumbers) = New DataPoint
        MyArray(PLC_RegisterNumbers).X = Now
        MyArray(PLC_RegisterNumbers).Y = CDbl(Me.pLC_Value)
        MyChart.Series(0).Points.Clear()

        For x As Integer = 0 To MyArray.Length - 1
            MyChart.Series(0).Points.AddXY(Format(MyArray(x).X, "HH:mm:ss"), MyArray(x).Y)
        Next
        Timer1.Start()
    End Sub

    Private Sub VS7_Trends_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub





#End Region

End Class

#Region "Smart Tag"

Public Class TrendsDesigner
    Inherits ControlDesigner

    Private _actionListCollection As DesignerActionListCollection

    Public Overrides ReadOnly Property ActionLists() As System.ComponentModel.Design.DesignerActionListCollection
        Get
            If _actionListCollection Is Nothing Then
                _actionListCollection = New DesignerActionListCollection()
                _actionListCollection.Add(New TrendsActionList(Me.Control))
            End If

            Return _actionListCollection
        End Get
    End Property
End Class
Friend Class TrendsActionList
    Inherits DesignerActionList

    Private ctr As VS7_Trends
    Private designerActionSvc As DesignerActionUIService

    Public Sub New(ByVal component As IComponent)
        MyBase.New(component)

        ctr = DirectCast(component, VS7_Trends)
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
    Public Property PLC_SerieName As String
        Get
            Return ctr.PLC_SerieName
        End Get
        Set(ByVal value As String)
            GetPropertyByName(ctr, "PLC_SerieName").SetValue(ctr, value)
            designerActionSvc.Refresh(ctr)

        End Set
    End Property

    Public Property PLC_XAxis As String
        Get
            Return ctr.PLC_XAxis
        End Get
        Set(ByVal value As String)
            GetPropertyByName(ctr, "PLC_XAxis").SetValue(ctr, value)
            designerActionSvc.Refresh(ctr)

        End Set
    End Property
    Public Property PLC_YAxis As String
        Get
            Return ctr.PLC_YAxis
        End Get
        Set(ByVal value As String)
            GetPropertyByName(ctr, "PLC_YAxis").SetValue(ctr, value)
            designerActionSvc.Refresh(ctr)

        End Set
    End Property
    Public Property PLC_TimeInterval As Integer
        Get
            Return ctr.PLC_TimeInterval
        End Get
        Set(ByVal value As Integer)
            GetPropertyByName(ctr, "PLC_TimeInterval").SetValue(ctr, value)
            designerActionSvc.Refresh(ctr)

        End Set
    End Property
    Public Property PLC_RegisterNumbers As Integer
        Get
            Return ctr.PLC_RegisterNumbers
        End Get
        Set(ByVal value As Integer)
            GetPropertyByName(ctr, "PLC_RegisterNumbers").SetValue(ctr, value)
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
        items.Add(New DesignerActionHeaderItem(KChartCategory))

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
        items.Add(New DesignerActionPropertyItem("PLC_SerieName", KPLC_SerieName, KChartCategory, KPLC_SerieName))
        items.Add(New DesignerActionPropertyItem("PLC_TimeInterval", KPlcPLC_TimeInterval, KChartCategory, KPlcPLC_TimeInterval))
        items.Add(New DesignerActionPropertyItem("PLC_XAxis", KPLC_XAxis, KChartCategory, KPLC_XAxis))
        items.Add(New DesignerActionPropertyItem("PLC_YAxis", KPLC_YAxis, KChartCategory, KPLC_YAxis))
        items.Add(New DesignerActionPropertyItem("PLC_RegisterNumbers", KPLC_RegisterNumbers, KChartCategory, KPLC_RegisterNumbers))

        'Return the ActionItemCollection
        Return items
    End Function
End Class
#End Region
