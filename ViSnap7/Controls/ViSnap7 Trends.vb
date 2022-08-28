Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Windows.Forms.Design
<System.ComponentModel.Designer(GetType(TrendsDesigner))>
Public Class VS7_Trends

#Region "PLC Properties"
    Public pLC_Value As String
    Structure DataPoint
        Public X As Date
        Public Y As Double
    End Structure
    Public Enum NumSeries
        Serie1 = 0
        Serie2 = 1
        Serie3 = 2
        Serie4 = 3
        Serie5 = 4
        Serie6 = 5
        Serie7 = 6
        Serie8 = 7
        Serie9 = 8
        Serie10 = 9
    End Enum

    Private _PLC As Integer() = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    Private _DataArea As General.DataArea() = {DataArea.MARK, DataArea.MARK, DataArea.MARK, DataArea.MARK, DataArea.MARK, DataArea.MARK, DataArea.MARK, DataArea.MARK, DataArea.MARK, DataArea.MARK}
    Private _DB As Integer() = {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
    Private _Byte As Integer() = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    Private _Bit As Integer() = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    Private _DataType As LocalDataType() = {LocalDataType.INT, LocalDataType.INT, LocalDataType.INT, LocalDataType.INT, LocalDataType.INT, LocalDataType.INT, LocalDataType.INT, LocalDataType.INT, LocalDataType.INT, LocalDataType.INT}
    Private _Length As Integer() = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    Private _SerieActive As Boolean() = {True, False, False, False, False, False, False, False, False, False}
    Private m_PLCs_Enable As Boolean()

    Private _Color As Color() = {Color.Blue, Color.Red, Color.Blue, Color.Red, Color.Black, Color.Green, Color.Yellow, Color.Blue, Color.Red, Color.Blue}

    Private _ChartDashStyle As ChartDashStyle() = {ChartDashStyle.Solid, ChartDashStyle.Solid, ChartDashStyle.Solid, ChartDashStyle.Solid, ChartDashStyle.Solid, ChartDashStyle.Solid, ChartDashStyle.Solid, ChartDashStyle.Solid, ChartDashStyle.Solid, ChartDashStyle.Solid}
    Private _SerieName As String() = {"Nombre Serie1", "Nombre Serie2", "Nombre Serie3", "Nombre Serie4", "Nombre Serie5", "Nombre Serie6", "Nombre Serie7", "Nombre Serie8", "Nombre Serie9", "Nombre Serie10"}
    Private _TimeInterval As Integer = 1000
    Private _YAxis As String = "Y"
    Private _XAxis As String = "X"
    Private _YAxis2 As String = "Y2"
    Private _AxisType As AxisType() = {AxisType.Primary, AxisType.Primary, AxisType.Primary, AxisType.Primary, AxisType.Primary, AxisType.Primary, AxisType.Primary, AxisType.Primary, AxisType.Primary, AxisType.Primary}
    Private _AxisXStep As AxisXStep
    Public _SerieNumber As NumSeries

    Public Enum LocalDataType
        UINT = 2
        SINT = 3
        INT = 4
        DINT = 5
        REAL = 6
    End Enum
    Public Enum AxisXStep
        Auto = 0
        Step_1ud = 1
        Step_2uds = 2
        Step_3uds = 3
        Step_4uds = 4
        Step_5uds = 5
        Step_6uds = 6
        Step_7uds = 7
        Step_8uds = 8
        Step_9uds = 9
        Step_10uds = 10
    End Enum

    Private numberOfSeries As Integer = [Enum].GetValues(GetType(NumSeries)).Length - 1

    Public Values(numberOfSeries) As VS7_RWVariable

    Private _RegisterNumbers As Integer = 60
#Region "Serie Number"
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPLC_SerieNumber)>
    Public Property PLC_SerieNumber() As NumSeries
        Get
            Return _SerieNumber
        End Get
        Set(value As NumSeries)
            _SerieNumber = value
        End Set
    End Property
#End Region
#Region "Serie Color"
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPLC_SerieColor)>
    Public Property PLC_Color() As Color
        Get
            Return _Color(PLC_SerieNumber)
        End Get
        Set(value As Color)
            If _Color(PLC_SerieNumber) <> value Then
                _Color(PLC_SerieNumber) = value
                PLCs_Color = _Color
                Refresh()
            End If
        End Set
    End Property
    <System.ComponentModel.Browsable(False)>
    Public Property PLCs_Color() As Color()
        Get
            Return _Color
        End Get
        Set(value As Color())
            _Color = value
            Refresh()
        End Set
    End Property
#End Region
#Region "PLC Number"
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcNumberLabel)>
    Public Property PLC_Number() As Integer
        Get
            Return _PLC(PLC_SerieNumber)
        End Get
        Set(value As Integer)
            If _PLC(PLC_SerieNumber) <> value Then
                _PLC(PLC_SerieNumber) = value
                PLCs_Number = _PLC
            End If

        End Set
    End Property
    <System.ComponentModel.Browsable(False)>
    Public Property PLCs_Number() As Integer()
        Get
            Return _PLC
        End Get
        Set(value As Integer())
            _PLC = value
        End Set
    End Property
#End Region
#Region "PLC DataArea"
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcDataAreaLabel)>
    Public Property PLC_DataArea() As General.DataArea
        Get
            Return _DataArea(PLC_SerieNumber)
        End Get
        Set(value As General.DataArea)
            If _DataArea(PLC_SerieNumber) <> value Then
                _DataArea(PLC_SerieNumber) = value
                PLCs_DataArea = _DataArea
            End If

        End Set
    End Property
    <System.ComponentModel.Browsable(False)>
    Public Property PLCs_DataArea() As General.DataArea()
        Get
            Return _DataArea
        End Get
        Set(value As General.DataArea())
            _DataArea = value
        End Set
    End Property
#End Region
#Region "PLC DB"
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcDBLabel)>
    Public Property PLC_DB() As Integer
        Get
            Return _DB(PLC_SerieNumber)
        End Get
        Set(value As Integer)
            If _DB(PLC_SerieNumber) <> value Then
                _DB(PLC_SerieNumber) = value
                PLCs_DB = _DB
            End If

        End Set
    End Property
    <System.ComponentModel.Browsable(False)>
    Public Property PLCs_DB() As Integer()
        Get
            Return _DB
        End Get
        Set(value As Integer())
            _DB = value
        End Set
    End Property
#End Region
#Region "PLC Byte"

    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcByteLabel)>
    Public Property PLC_Byte() As Integer
        Get
            Return _Byte(PLC_SerieNumber)
        End Get
        Set(value As Integer)
            If _Byte(PLC_SerieNumber) <> value Then
                _Byte(PLC_SerieNumber) = value
                PLCs_Byte = _Byte
            End If

        End Set
    End Property
    <System.ComponentModel.Browsable(False)>
    Public Property PLCs_Byte() As Integer()
        Get
            Return _Byte
        End Get
        Set(value As Integer())
            _Byte = value
        End Set
    End Property
#End Region
#Region "Data Type"

    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcValueTypeLabel)>
    Public Property PLC_DataType() As LocalDataType
        Get
            Return _DataType(PLC_SerieNumber)
        End Get
        Set(value As LocalDataType)
            If _DataType(PLC_SerieNumber) <> value Then
                _DataType(PLC_SerieNumber) = value
                PLCs_DataType = _DataType
            End If


        End Set
    End Property

    <System.ComponentModel.Browsable(False)>
    Public Property PLCs_DataType() As LocalDataType()
        Get
            Return _DataType
        End Get
        Set(value As LocalDataType())
            _DataType = value

        End Set
    End Property
#End Region
#Region "PLC Bit"
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcBitLabel)>
    Public Property PLC_Bit() As Integer
        Get
            Return _Bit(PLC_SerieNumber)
        End Get
        Set(value As Integer)
            If _Bit(PLC_SerieNumber) <> value Then
                _Bit(PLC_SerieNumber) = value
                PLCs_Bit = _Bit
            End If

        End Set
    End Property
    <System.ComponentModel.Browsable(False)>
    Public Property PLCs_Bit() As Integer()
        Get
            Return _Bit
        End Get
        Set(value As Integer())
            _Bit = value
        End Set
    End Property
#End Region
#Region "PLC Length"
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcLengthLabel)>
    Public Property PLC_Length() As Integer
        Get
            Return _Length(PLC_SerieNumber)
        End Get
        Set(value As Integer)
            _Length(PLC_SerieNumber) = value
        End Set
    End Property
#End Region
#Region "Serie Name"
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPLC_SerieName)>
    Public Property PLC_SerieName() As String
        Get
            Return _SerieName(PLC_SerieNumber)
        End Get
        Set(value As String)
            If _SerieName(PLC_SerieNumber) <> value Then
                _SerieName(PLC_SerieNumber) = value
                PLCs_SerieName = _SerieName
            End If

        End Set
    End Property
    <System.ComponentModel.Browsable(False)>
    Public Property PLCs_SerieName() As String()
        Get
            Return _SerieName
        End Get
        Set(value As String())
            _SerieName = value
        End Set
    End Property
#End Region
#Region "Serie active"

    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPLC_SerieActive)>
    Public Property PLC_SerieActive() As Boolean
        Get
            Return _SerieActive(PLC_SerieNumber)
        End Get
        Set(value As Boolean)
            If _SerieActive(PLC_SerieNumber) <> value Then
                _SerieActive(PLC_SerieNumber) = value
                PLCs_SerieActive = _SerieActive
                Refresh()
            End If
        End Set

    End Property
    <System.ComponentModel.Browsable(False)>
    Public Property PLCs_SerieActive() As Boolean()
        Get
            Return _SerieActive
        End Get
        Set(value As Boolean())
            _SerieActive = value
            Refresh()
        End Set
    End Property
#End Region
#Region "Chart Style"
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPLC_ChartDashStyle)>
    Public Property PLC_ChartDashStyle() As ChartDashStyle
        Get
            Return _ChartDashStyle(PLC_SerieNumber)
        End Get
        Set(value As ChartDashStyle)
            If _ChartDashStyle(PLC_SerieNumber) <> value Then
                _ChartDashStyle(PLC_SerieNumber) = value
                PLCs_ChartDashStyle = _ChartDashStyle
                Refresh()
            End If

        End Set
    End Property
    <System.ComponentModel.Browsable(False)>
    Public Property PLCs_ChartDashStyle() As ChartDashStyle()
        Get
            Return _ChartDashStyle
        End Get
        Set(value As ChartDashStyle())
            _ChartDashStyle = value
            Refresh()
        End Set
    End Property
#End Region
#Region "Axis Type"
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPLC_SerieAxisType)>
    Public Property PLC_AxisType() As AxisType
        Get
            Return _AxisType(PLC_SerieNumber)
        End Get
        Set(value As AxisType)
            If _AxisType(PLC_SerieNumber) <> value Then
                _AxisType(PLC_SerieNumber) = value
                PLCs_AxisType = _AxisType
            End If

        End Set
    End Property
    <System.ComponentModel.Browsable(False)>
    Public Property PLCs_AxisType() As AxisType()
        Get
            Return _AxisType
        End Get
        Set(value As AxisType())
            _AxisType = value
        End Set
    End Property
#End Region

#Region "Axis x/y characteristics"
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPLC_RegisterNumbers)>
    Public Property PLC_RegisterNumbers() As Integer
        Get
            Return _RegisterNumbers
        End Get
        Set(value As Integer)
            _RegisterNumbers = value
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPLC_TimeInterval)>
    Public Property PLC_TimeInterval() As Integer
        Get
            Return _TimeInterval
        End Get
        Set(value As Integer)
            _TimeInterval = value
        End Set
    End Property

    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPLC_AxisXStep)>
    Public Property PLC_AxisXStep() As AxisXStep
        Get
            Return _AxisXStep
        End Get
        Set(value As AxisXStep)
            _AxisXStep = value
        End Set
    End Property

    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPLC_SerieYAxis)>
    Public Property PLC_YAxis() As String
        Get
            Return _YAxis
        End Get
        Set(value As String)
            _YAxis = value
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPLC_SerieYAxis2)>
    Public Property PLC_YAxis2() As String
        Get
            Return _YAxis2
        End Get
        Set(value As String)
            _YAxis2 = value
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPLC_SerieXAxis)>
    Public Property PLC_XAxis() As String
        Get
            Return _XAxis
        End Get
        Set(value As String)
            _XAxis = value
        End Set
    End Property

    Private MyArray(numberOfSeries, PLC_RegisterNumbers) As DataPoint
#End Region
#End Region
#Region "Plc reading and writing"
    Public Sub UpdateControl(ByRef _PlcClient As PlcClient)

        If firstExecution Then
            Me.BackColor = Me.Parent.BackColor
            Me.MyChart.BackColor = Me.BackColor
            MyChart.Dock = DockStyle.Fill
            For c = 0 To numberOfSeries
                If PLCs_SerieActive(c) Then
                    Values(c) = New VS7_RWVariable(Me, PLCs_Number(c), PLCs_DataArea(c), PLCs_DataType(c), PLCs_DB(c), PLCs_Byte(c), PLCs_Bit(c), 0)
                End If
            Next

            UpdateChart()
            ConfigureTrend()
            UpdateNumberOfControlsActive = True

        End If
        If Not UpdateNumberOfControlsActive Then
            Timer1.Start()
        End If
    End Sub
    Public Sub UpdateChart()
        ReDim MyArray(numberOfSeries, PLC_RegisterNumbers)
        Timer1.Interval = Me.PLC_TimeInterval
        With MyChart.ChartAreas(0)
            .AxisX.Title = Me.PLC_XAxis
            .AxisX2.Title = Me.PLC_XAxis
            .AxisY.Title = Me.PLC_YAxis
            .AxisY2.Title = Me.PLC_YAxis2
            .AxisY2.TextOrientation = TextOrientation.Rotated270
        End With
        For c = 0 To numberOfSeries
            If Me.PLCs_SerieActive(c) Then
                Try
                    MyChart.Series.Add(PLCs_SerieName(c))
                Catch ex As Exception
                End Try
                MyChart.Series(PLCs_SerieName(c)).YAxisType = PLCs_AxisType(c)
                MyChart.Series(PLCs_SerieName(c)).LabelBackColor = Color.Transparent
                MyChart.Series(PLCs_SerieName(c)).Points.Clear()
                For x As Integer = 0 To PLC_RegisterNumbers - 1
                    MyArray(c, x) = New DataPoint
                    MyArray(c, x).X = Now
                    MyArray(c, x).Y = 0 'Normally will be CDbl(Me.Values(c).pLC_Value)
                    MyChart.Series(0).Points.AddXY(Format(MyArray(c, x).X, "HH:mm:ss"), MyArray(c, x).Y)
                Next
            Else
                Try
                    MyChart.Series(PLCs_SerieName(c)).Points.Clear()
                Catch ex As Exception

                End Try

            End If
        Next

    End Sub



#End Region

#Region "Trends"
    Public Sub ConfigureTrend()
        'setup the chart
        For c = 0 To numberOfSeries
            If PLCs_SerieActive(c) Then
                ' draw the chart
                With MyChart.ChartAreas(0)
                    .AxisX.Title = PLC_XAxis
                    .AxisX.MajorGrid.LineColor = Color.LightBlue
                    .AxisX.Minimum = 0
                    .AxisX.Interval = PLC_AxisXStep
                    .AxisY.Title = PLC_YAxis
                    .AxisY.MajorGrid.LineColor = Color.LightGray
                    '.AxisY.Minimum = 0
                    .BackColor = Color.FloralWhite
                    .BackSecondaryColor = Color.White
                    .BackGradientStyle = GradientStyle.HorizontalCenter
                    .BorderColor = Color.Blue
                    .BorderDashStyle = ChartDashStyle.Solid
                    .BorderWidth = 1
                    .ShadowOffset = 2
                End With
                With MyChart.Series(PLCs_SerieName(c))
                    .ChartType = DataVisualization.Charting.SeriesChartType.Line
                    .BorderWidth = 1
                    .Color = _Color(c)
                    .BorderDashStyle = PLCs_ChartDashStyle(c)
                    .MarkerStyle = DataVisualization.Charting.MarkerStyle.Square
                    .MarkerSize = 4
                End With
            End If
        Next
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        For index = 0 To numberOfSeries

            If PLCs_SerieActive(index) Then
                For c = 0 To PLC_RegisterNumbers - 1
                    MyArray(index, c) = MyArray(index, c + 1)
                Next
                MyArray(index, PLC_RegisterNumbers) = New DataPoint
                MyArray(index, PLC_RegisterNumbers).X = Now
                MyArray(index, PLC_RegisterNumbers).Y = CDbl(Me.Values(index).pLC_Value)
                MyChart.Series(PLCs_SerieName(index)).Points.Clear()
                For x As Integer = 0 To PLC_RegisterNumbers - 1
                    MyChart.Series(PLCs_SerieName(index)).Points.AddXY(Format(MyArray(index, x).X, "HH:mm:ss"), MyArray(index, x).Y)
                Next
            Else
                Try
                    MyChart.Series(PLCs_SerieName(index)).Points.Clear()
                Catch ex As Exception

                End Try

            End If
        Next
        Timer1.Start()
    End Sub

    Private Sub VS7_Trends_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.CheckSerie1.Visible = PLCs_SerieActive(0)
        Me.CheckSerie2.Visible = PLCs_SerieActive(1)
        Me.CheckSerie3.Visible = PLCs_SerieActive(2)
        Me.CheckSerie4.Visible = PLCs_SerieActive(3)
        Me.CheckSerie5.Visible = PLCs_SerieActive(4)
        Me.CheckSerie6.Visible = PLCs_SerieActive(5)
        Me.CheckSerie7.Visible = PLCs_SerieActive(6)
        Me.CheckSerie8.Visible = PLCs_SerieActive(7)
        Me.CheckSerie9.Visible = PLCs_SerieActive(8)
        Me.CheckSerie10.Visible = PLCs_SerieActive(9)
        Me.CheckSerie1.Checked = PLCs_SerieActive(0)
        Me.CheckSerie2.Checked = PLCs_SerieActive(1)
        Me.CheckSerie3.Checked = PLCs_SerieActive(2)
        Me.CheckSerie4.Checked = PLCs_SerieActive(3)
        Me.CheckSerie5.Checked = PLCs_SerieActive(4)
        Me.CheckSerie6.Checked = PLCs_SerieActive(5)
        Me.CheckSerie7.Checked = PLCs_SerieActive(6)
        Me.CheckSerie8.Checked = PLCs_SerieActive(7)
        Me.CheckSerie9.Checked = PLCs_SerieActive(8)
        Me.CheckSerie10.Checked = PLCs_SerieActive(9)

    End Sub

    Private Sub CheckSerie1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckSerie1.CheckedChanged
        Me.PLCs_SerieActive(0) = CheckSerie1.Checked
    End Sub

    Private Sub CheckSerie2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckSerie2.CheckedChanged
        Me.PLCs_SerieActive(1) = CheckSerie2.Checked
    End Sub

    Private Sub CheckSerie3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckSerie3.CheckedChanged
        Me.PLCs_SerieActive(2) = CheckSerie3.Checked
    End Sub

    Private Sub CheckSerie4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckSerie4.CheckedChanged
        Me.PLCs_SerieActive(3) = CheckSerie4.Checked
    End Sub

    Private Sub CheckSerie5_CheckedChanged(sender As Object, e As EventArgs) Handles CheckSerie5.CheckedChanged
        Me.PLCs_SerieActive(4) = CheckSerie5.Checked
    End Sub

    Private Sub CheckSerie6_CheckedChanged(sender As Object, e As EventArgs) Handles CheckSerie6.CheckedChanged
        Me.PLCs_SerieActive(5) = CheckSerie6.Checked
    End Sub

    Private Sub CheckSerie7_CheckedChanged(sender As Object, e As EventArgs) Handles CheckSerie7.CheckedChanged
        Me.PLCs_SerieActive(6) = CheckSerie7.Checked
    End Sub

    Private Sub CheckSerie8_CheckedChanged(sender As Object, e As EventArgs) Handles CheckSerie8.CheckedChanged
        Me.PLCs_SerieActive(7) = CheckSerie8.Checked
    End Sub

    Private Sub CheckSerie9_CheckedChanged(sender As Object, e As EventArgs) Handles CheckSerie9.CheckedChanged
        Me.PLCs_SerieActive(8) = CheckSerie9.Checked
    End Sub

    Private Sub CheckSerie10_CheckedChanged(sender As Object, e As EventArgs) Handles CheckSerie10.CheckedChanged
        Me.PLCs_SerieActive(9) = CheckSerie10.Checked
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

    Public Sub New(ByRef component As IComponent)
        MyBase.New(component)

        ctr = DirectCast(component, VS7_Trends)
        designerActionSvc = CType(GetService(GetType(DesignerActionUIService)), DesignerActionUIService)
    End Sub

#Region " Properties to display in the Smart-Tag panel "

    Public Property PLC_DataArea() As General.DataArea
        Get
            Return ctr.PLC_DataArea

        End Get
        Set(value As General.DataArea)
            GetPropertyByName(ctr, "PLC_DataArea").SetValue(ctr, value)
            designerActionSvc.Refresh(ctr)

        End Set
    End Property

    Public Property PLC_DataType() As VS7_Trends.LocalDataType
        Get
            Return ctr.PLC_DataType

        End Get
        Set(value As VS7_Trends.LocalDataType)
            GetPropertyByName(ctr, "PLC_DataType").SetValue(ctr, value)

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

    Public Property PLC_SerieName() As String
        Get
            Return ctr.PLC_SerieName
        End Get
        Set(ByVal value As String)
            GetPropertyByName(ctr, "PLC_SerieName").SetValue(ctr, value)
            designerActionSvc.Refresh(ctr)

        End Set
    End Property
    Public Property PLC_AxisType() As AxisType
        Get
            Return ctr.PLC_AxisType
        End Get
        Set(ByVal value As AxisType)
            GetPropertyByName(ctr, "PLC_AxisType").SetValue(ctr, value)
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

    Public Property PLC_AxisXStep As VS7_Trends.AxisXStep
        Get
            Return ctr.PLC_AxisXStep
        End Get
        Set(value As VS7_Trends.AxisXStep)
            GetPropertyByName(ctr, "PLC_AxisXStep").SetValue(ctr, value)
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


    Public Property PLC_YAxis2 As String
        Get
            Return ctr.PLC_YAxis2
        End Get
        Set(ByVal value As String)
            GetPropertyByName(ctr, "PLC_YAxis2").SetValue(ctr, value)
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
    Public Property PLC_SerieActive() As Boolean
        Get
            Return ctr.PLC_SerieActive
        End Get
        Set(ByVal value As Boolean)
            GetPropertyByName(ctr, "PLC_SerieActive").SetValue(ctr, value)
            designerActionSvc.Refresh(ctr)
        End Set
    End Property

    Public Property PLC_ChartDashStyle() As ChartDashStyle
        Get
            Return ctr.PLC_ChartDashStyle
        End Get
        Set(ByVal value As ChartDashStyle)
            GetPropertyByName(ctr, "PLC_ChartDashStyle").SetValue(ctr, value)
            designerActionSvc.Refresh(ctr)

        End Set
    End Property
    Public Property PLC_Color() As Color
        Get
            Return ctr.PLC_Color
        End Get
        Set(ByVal value As Color)
            GetPropertyByName(ctr, "PLC_Color").SetValue(ctr, value)
            designerActionSvc.Refresh(ctr)

        End Set
    End Property

    Public Property PLC_SerieNumber() As VS7_Trends.NumSeries
        Get
            Return ctr.PLC_SerieNumber
        End Get
        Set(ByVal value As VS7_Trends.NumSeries)
            GetPropertyByName(ctr, "PLC_SerieNumber").SetValue(ctr, value)
            designerActionSvc.Refresh(ctr)

        End Set
    End Property

    Sub UpdateSerieNumber()
        GetPropertyByName(ctr, "PLC_SerieNumber").SetValue(ctr, PLC_SerieNumber)
        designerActionSvc.Refresh(ctr)
    End Sub
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
    Private Sub UpdateChart()
        ctr.MyChart.Dock = DockStyle.Fill
        ctr.UpdateChart()
        ctr.ConfigureTrend()

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
        items.Add(New DesignerActionPropertyItem("PLC_SerieNumber", "Serie", KPlcAdressingCategory, "Serie"))
        items.Add(New DesignerActionPropertyItem("PLC_SerieName", KPLC_SerieName, KPlcAdressingCategory, KPLC_SerieName))
        items.Add(New DesignerActionPropertyItem("PLC_SerieActive", "Active", KPlcAdressingCategory, "Active?"))
        items.Add(New DesignerActionPropertyItem("PLC_ChartDashStyle", "Style", KPlcAdressingCategory, "Style"))
        items.Add(New DesignerActionPropertyItem("PLC_AxisType", "Axis Type", KPlcAdressingCategory, "Style"))

        items.Add(New DesignerActionPropertyItem("PLC_Color", "Color", KPlcAdressingCategory, "color?"))

        items.Add(New DesignerActionPropertyItem("PLC_DataArea", KPlcValueTypeLabel, KPlcAdressingCategory, KPlcTipDataArea))
        items.Add(New DesignerActionPropertyItem("PLC_DataType", KPlcDataTypeLabel, KPlcAdressingCategory, KPlcTipDataType))
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
        items.Add(New DesignerActionPropertyItem("PLC_TimeInterval", KPlcPLC_TimeInterval, KChartCategory, KPlcPLC_TimeInterval))
        items.Add(New DesignerActionPropertyItem("PLC_XAxis", KPLC_XAxis, KChartCategory, KPLC_XAxis))
        items.Add(New DesignerActionPropertyItem("PLC_AxisXStep", KPLC_XAxisStep, KChartCategory, KPLC_XAxisStep))
        items.Add(New DesignerActionPropertyItem("PLC_YAxis", KPLC_YAxis, KChartCategory, KPLC_YAxis))
        items.Add(New DesignerActionPropertyItem("PLC_YAxis2", KPLC_YAxis2, KChartCategory, KPLC_YAxis2))

        items.Add(New DesignerActionPropertyItem("PLC_RegisterNumbers", KPLC_RegisterNumbers, KChartCategory, KPLC_RegisterNumbers))
        items.Add(New DesignerActionMethodItem(Me, "UpdateChart", "Chart Preview"))
        'Return the ActionItemCollection
        Return items
    End Function
End Class
#End Region
