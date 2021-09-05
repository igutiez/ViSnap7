' Copyright (C) 2007 A.J.Bauer
'
'  This software is provided as-is, without any express or implied
'  warranty.  In no event will the authors be held liable for any damages
'  arising from the use of this software.

'  Permission is granted to anyone to use this software for any purpose,
'  including commercial applications, and to alter it and redistribute it
'  freely, subject to the following restrictions:

'  1. The origin of this software must not be misrepresented; you must not
'     claim that you wrote the original software. if you use this software
'     in a product, an acknowledgment in the product documentation would be
'     appreciated but is not required.
'  2. Altered source versions must be plainly marked as such, and must not be
'     misrepresented as being the original software.
'  3. This notice may not be removed or altered from any source distribution.

Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms.Design

Partial Public Class VS7_Gauge
    Inherits Control
#Region "enum, var, delegate, event"
    Public Enum NeedleColorEnum
        Gray = 0
        Red = 1
        Green = 2
        Blue = 3
        Yellow = 4
        Violet = 5
        Magenta = 6
    End Enum

    Private Const ZERO As Byte = 0
    Private Const NUMOFCAPS As Byte = 5
    Private Const NUMOFRANGES As Byte = 5
    Private fontBoundY1 As Single
    Private fontBoundY2 As Single
    Private gaugeBitmap As Bitmap
    Private drawGaugeBackground As Boolean = True
    Private m_value As Single
    Private m_valueIsInRange As Boolean() = {False, False, False, False, False}
    Private m_CapIdx As Byte = 1
    Private m_CapColor As Color() = {Color.Black, Color.Black, Color.Black, Color.Black, Color.Black}
    Private m_CapText As String() = {"", "", "", "", ""}
    Private m_CapPosition As Point() = {New Point(10, 10), New Point(10, 10), New Point(10, 10), New Point(10, 10), New Point(10, 10)}
    Private m_Center As Point = New Point(100, 100)
    Private m_MinValue As Single = -100
    Private m_MaxValue As Single = 400
    Private m_BaseArcColor As Color = Color.Gray
    Private m_BaseArcRadius As Integer = 80
    Private m_BaseArcStart As Integer = 135
    Private m_BaseArcSweep As Integer = 270
    Private m_BaseArcWidth As Integer = 2
    Private m_ScaleLinesInterColor As Color = Color.Black
    Private m_ScaleLinesInterInnerRadius As Integer = 73
    Private m_ScaleLinesInterOuterRadius As Integer = 80
    Private m_ScaleLinesInterWidth As Integer = 1
    Private m_ScaleLinesMinorNumOf As Integer = 9
    Private m_ScaleLinesMinorColor As Color = Color.Gray
    Private m_ScaleLinesMinorInnerRadius As Integer = 75
    Private m_ScaleLinesMinorOuterRadius As Integer = 80
    Private m_ScaleLinesMinorWidth As Integer = 1
    Private m_ScaleLinesMajorStepValue As Single = 50.0F
    Private m_ScaleLinesMajorColor As Color = Color.Black
    Private m_ScaleLinesMajorInnerRadius As Integer = 70
    Private m_ScaleLinesMajorOuterRadius As Integer = 80
    Private m_ScaleLinesMajorWidth As Integer = 2
    Private m_RangeIdx As Byte
    Private m_RangeEnabled As Boolean() = {True, True, False, False, False}
    Private m_RangeColor As Color() = {Color.LightGreen, Color.Red, Color.FromKnownColor(KnownColor.Control), Color.FromKnownColor(KnownColor.Control), Color.FromKnownColor(KnownColor.Control)}
    Private m_RangeStartValue As Single() = {-100.0F, 300.0F, 0.0F, 0.0F, 0.0F}
    Private m_RangeEndValue As Single() = {300.0F, 400.0F, 0.0F, 0.0F, 0.0F}
    Private m_RangeInnerRadius As Integer() = {70, 70, 70, 70, 70}
    Private m_RangeOuterRadius As Integer() = {80, 80, 80, 80, 80}
    Private m_ScaleNumbersRadius As Integer = 95
    Private m_ScaleNumbersColor As Color = Color.Black
    Private m_ScaleNumbersFormat As String
    Private m_ScaleNumbersStartScaleLine As Integer
    Private m_ScaleNumbersStepScaleLines As Integer = 1
    Private m_ScaleNumbersRotation As Integer = 0
    Private m_NeedleType As Integer = 0
    Private m_NeedleRadius As Integer = 80
    Private m_NeedleColor1 As NeedleColorEnum = NeedleColorEnum.Gray
    Private m_NeedleColor2 As Color = Color.DimGray
    Private m_NeedleWidth As Integer = 2

    Public Class ValueInRangeChangedEventArgs
        Inherits EventArgs

        Public valueInRange As Integer

        Public Sub New(ByVal valueInRange As Integer)
            Me.valueInRange = valueInRange
        End Sub
    End Class

    Public Delegate Sub ValueInRangeChangedDelegate(ByVal sender As Object, ByVal e As ValueInRangeChangedEventArgs)
    <Description("This event is raised if the value falls into a defined range.")>
    Public Event ValueInRangeChanged As ValueInRangeChangedDelegate
#End Region

#Region "hidden , overridden inherited properties"
    Public Overloads Property AllowDrop As Boolean
        Get
            Return False
        End Get
        Set(ByVal value As Boolean)
        End Set
    End Property

    Public Overloads Property AutoSize As Boolean
        Get
            Return False
        End Get
        Set(ByVal value As Boolean)
        End Set
    End Property

    Public Overloads Property ForeColor As Boolean
        Get
            Return False
        End Get
        Set(ByVal value As Boolean)
        End Set
    End Property

    Public Overloads Property ImeMode As Boolean
        Get
            Return False
        End Get
        Set(ByVal value As Boolean)
        End Set
    End Property

    Public Overrides Property BackColor As Color
        Get
            Return MyBase.BackColor
        End Get
        Set(ByVal value As Color)
            MyBase.BackColor = value
            drawGaugeBackground = True
            MyBase.Refresh()
        End Set
    End Property

    Public Overrides Property Font As Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As Font)
            MyBase.Font = value
            drawGaugeBackground = True
            MyBase.Refresh()
        End Set
    End Property

    Public Overrides Property BackgroundImageLayout As ImageLayout
        Get
            Return MyBase.BackgroundImageLayout
        End Get
        Set(ByVal value As ImageLayout)
            MyBase.BackgroundImageLayout = value
            drawGaugeBackground = True
            MyBase.Refresh()
        End Set
    End Property
#End Region

    Public Sub New()
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
    End Sub

#Region "properties"
    <Browsable(True), Category("AGauge"), Description("The value.")>
    Public Property Value As Single
        Get
            Return m_value
        End Get
        Set(ByVal value As Single)

            If m_value <> value Then
                m_value = Math.Min(Math.Max(value, m_MinValue), m_MaxValue)

                If DesignMode Then
                    drawGaugeBackground = True
                End If

                For counter = 0 To NUMOFRANGES - 1 - 1

                    If m_RangeStartValue(counter) <= m_value AndAlso m_value <= m_RangeEndValue(counter) AndAlso m_RangeEnabled(counter) Then
                        If Not m_valueIsInRange(counter) Then
                            RaiseEvent ValueInRangeChanged(Me, New ValueInRangeChangedEventArgs(counter))
                        End If
                    Else
                        m_valueIsInRange(counter) = False
                    End If
                Next

                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), RefreshProperties(RefreshProperties.All), Description("The caption index. set this to a value of 0 up to 4 to change the corresponding caption's properties.")>
    Public Property Cap_Idx As Byte
        Get
            Return m_CapIdx
        End Get
        Set(ByVal value As Byte)

            If m_CapIdx <> value AndAlso 0 <= value AndAlso value < 5 Then
                m_CapIdx = value
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The color of the caption text.")>
    Private Property CapColor As Color
        Get
            Return m_CapColor(m_CapIdx)
        End Get
        Set(ByVal value As Color)

            If m_CapColor(m_CapIdx) <> value Then
                m_CapColor(m_CapIdx) = value
                CapColors = m_CapColor
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(False)>
    Public Property CapColors As Color()
        Get
            Return m_CapColor
        End Get
        Set(ByVal value As Color())
            m_CapColor = value
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The text of the caption.")>
    Public Property CapText As String
        Get
            Return m_CapText(m_CapIdx)
        End Get
        Set(ByVal value As String)

            If Not Equals(m_CapText(m_CapIdx), value) Then
                m_CapText(m_CapIdx) = value
                CapsText = m_CapText
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(False)>
    Public Property CapsText As String()
        Get
            Return m_CapText
        End Get
        Set(ByVal value As String())

            For counter = 0 To 5 - 1
                m_CapText(counter) = value(counter)
            Next
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The position of the caption.")>
    Public Property CapPosition As Point
        Get
            Return m_CapPosition(m_CapIdx)
        End Get
        Set(ByVal value As Point)

            If m_CapPosition(m_CapIdx) <> value Then
                m_CapPosition(m_CapIdx) = value
                CapsPosition = m_CapPosition
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(False)>
    Public Property CapsPosition As Point()
        Get
            Return m_CapPosition
        End Get
        Set(ByVal value As Point())
            m_CapPosition = value
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The center of the gauge (in the control's client area).")>
    Public Property Center As Point
        Get
            Return m_Center
        End Get
        Set(ByVal value As Point)

            If m_Center <> value Then
                m_Center = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The minimum value to show on the scale.")>
    Public Property MinValue As Single
        Get
            Return m_MinValue
        End Get
        Set(ByVal value As Single)

            If m_MinValue <> value AndAlso value < m_MaxValue Then
                m_MinValue = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The maximum value to show on the scale.")>
    Public Property MaxValue As Single
        Get
            Return m_MaxValue
        End Get
        Set(ByVal value As Single)

            If m_MaxValue <> value AndAlso value > m_MinValue Then
                m_MaxValue = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The color of the base arc.")>
    Public Property BaseArcColor As Color
        Get
            Return m_BaseArcColor
        End Get
        Set(ByVal value As Color)

            If m_BaseArcColor <> value Then
                m_BaseArcColor = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The radius of the base arc.")>
    Public Property BaseArcRadius As Integer
        Get
            Return m_BaseArcRadius
        End Get
        Set(ByVal value As Integer)

            If m_BaseArcRadius <> value Then
                m_BaseArcRadius = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The start angle of the base arc.")>
    Public Property BaseArcStart As Integer
        Get
            Return m_BaseArcStart
        End Get
        Set(ByVal value As Integer)

            If m_BaseArcStart <> value Then
                m_BaseArcStart = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The sweep angle of the base arc.")>
    Public Property BaseArcSweep As Integer
        Get
            Return m_BaseArcSweep
        End Get
        Set(ByVal value As Integer)

            If m_BaseArcSweep <> value Then
                m_BaseArcSweep = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The width of the base arc.")>
    Public Property BaseArcWidth As Integer
        Get
            Return m_BaseArcWidth
        End Get
        Set(ByVal value As Integer)

            If m_BaseArcWidth <> value Then
                m_BaseArcWidth = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The color of the inter scale lines which are the middle scale lines for an uneven number of minor scale lines.")>
    Public Property ScaleLinesInterColor As Color
        Get
            Return m_ScaleLinesInterColor
        End Get
        Set(ByVal value As Color)

            If m_ScaleLinesInterColor <> value Then
                m_ScaleLinesInterColor = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The inner radius of the inter scale lines which are the middle scale lines for an uneven number of minor scale lines.")>
    Public Property ScaleLinesInterInnerRadius As Integer
        Get
            Return m_ScaleLinesInterInnerRadius
        End Get
        Set(ByVal value As Integer)

            If m_ScaleLinesInterInnerRadius <> value Then
                m_ScaleLinesInterInnerRadius = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The outer radius of the inter scale lines which are the middle scale lines for an uneven number of minor scale lines.")>
    Public Property ScaleLinesInterOuterRadius As Integer
        Get
            Return m_ScaleLinesInterOuterRadius
        End Get
        Set(ByVal value As Integer)

            If m_ScaleLinesInterOuterRadius <> value Then
                m_ScaleLinesInterOuterRadius = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The width of the inter scale lines which are the middle scale lines for an uneven number of minor scale lines.")>
    Public Property ScaleLinesInterWidth As Integer
        Get
            Return m_ScaleLinesInterWidth
        End Get
        Set(ByVal value As Integer)

            If m_ScaleLinesInterWidth <> value Then
                m_ScaleLinesInterWidth = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The number of minor scale lines.")>
    Public Property ScaleLinesMinorNumOf As Integer
        Get
            Return m_ScaleLinesMinorNumOf
        End Get
        Set(ByVal value As Integer)

            If m_ScaleLinesMinorNumOf <> value Then
                m_ScaleLinesMinorNumOf = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The color of the minor scale lines.")>
    Public Property ScaleLinesMinorColor As Color
        Get
            Return m_ScaleLinesMinorColor
        End Get
        Set(ByVal value As Color)

            If m_ScaleLinesMinorColor <> value Then
                m_ScaleLinesMinorColor = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The inner radius of the minor scale lines.")>
    Public Property ScaleLinesMinorInnerRadius As Integer
        Get
            Return m_ScaleLinesMinorInnerRadius
        End Get
        Set(ByVal value As Integer)

            If m_ScaleLinesMinorInnerRadius <> value Then
                m_ScaleLinesMinorInnerRadius = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The outer radius of the minor scale lines.")>
    Public Property ScaleLinesMinorOuterRadius As Integer
        Get
            Return m_ScaleLinesMinorOuterRadius
        End Get
        Set(ByVal value As Integer)

            If m_ScaleLinesMinorOuterRadius <> value Then
                m_ScaleLinesMinorOuterRadius = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The width of the minor scale lines.")>
    Public Property ScaleLinesMinorWidth As Integer
        Get
            Return m_ScaleLinesMinorWidth
        End Get
        Set(ByVal value As Integer)

            If m_ScaleLinesMinorWidth <> value Then
                m_ScaleLinesMinorWidth = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The step value of the major scale lines.")>
    Public Property ScaleLinesMajorStepValue As Single
        Get
            Return m_ScaleLinesMajorStepValue
        End Get
        Set(ByVal value As Single)

            If m_ScaleLinesMajorStepValue <> value AndAlso value > 0 Then
                m_ScaleLinesMajorStepValue = Math.Max(Math.Min(value, m_MaxValue), m_MinValue)
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The color of the major scale lines.")>
    Public Property ScaleLinesMajorColor As Color
        Get
            Return m_ScaleLinesMajorColor
        End Get
        Set(ByVal value As Color)

            If m_ScaleLinesMajorColor <> value Then
                m_ScaleLinesMajorColor = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The inner radius of the major scale lines.")>
    Public Property ScaleLinesMajorInnerRadius As Integer
        Get
            Return m_ScaleLinesMajorInnerRadius
        End Get
        Set(ByVal value As Integer)

            If m_ScaleLinesMajorInnerRadius <> value Then
                m_ScaleLinesMajorInnerRadius = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The outer radius of the major scale lines.")>
    Public Property ScaleLinesMajorOuterRadius As Integer
        Get
            Return m_ScaleLinesMajorOuterRadius
        End Get
        Set(ByVal value As Integer)

            If m_ScaleLinesMajorOuterRadius <> value Then
                m_ScaleLinesMajorOuterRadius = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The width of the major scale lines.")>
    Public Property ScaleLinesMajorWidth As Integer
        Get
            Return m_ScaleLinesMajorWidth
        End Get
        Set(ByVal value As Integer)

            If m_ScaleLinesMajorWidth <> value Then
                m_ScaleLinesMajorWidth = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), RefreshProperties(RefreshProperties.All), Description("The range index. set this to a value of 0 up to 4 to change the corresponding range's properties.")>
    Public Property Range_Idx As Byte
        Get
            Return m_RangeIdx
        End Get
        Set(ByVal value As Byte)

            If m_RangeIdx <> value AndAlso 0 <= value AndAlso value < NUMOFRANGES Then
                m_RangeIdx = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("Enables or disables the range selected by Range_Idx.")>
    Public Property RangeEnabled As Boolean
        Get
            Return m_RangeEnabled(m_RangeIdx)
        End Get
        Set(ByVal value As Boolean)

            If m_RangeEnabled(m_RangeIdx) <> value Then
                m_RangeEnabled(m_RangeIdx) = value
                RangesEnabled = m_RangeEnabled
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(False)>
    Public Property RangesEnabled As Boolean()
        Get
            Return m_RangeEnabled
        End Get
        Set(ByVal value As Boolean())
            m_RangeEnabled = value
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The color of the range.")>
    Public Property RangeColor As Color
        Get
            Return m_RangeColor(m_RangeIdx)
        End Get
        Set(ByVal value As Color)

            If m_RangeColor(m_RangeIdx) <> value Then
                m_RangeColor(m_RangeIdx) = value
                RangesColor = m_RangeColor
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(False)>
    Public Property RangesColor As Color()
        Get
            Return m_RangeColor
        End Get
        Set(ByVal value As Color())
            m_RangeColor = value
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The start value of the range, must be less than RangeEndValue.")>
    Public Property RangeStartValue As Single
        Get
            Return m_RangeStartValue(m_RangeIdx)
        End Get
        Set(ByVal value As Single)

            If m_RangeStartValue(m_RangeIdx) <> value AndAlso value < m_RangeEndValue(m_RangeIdx) Then
                m_RangeStartValue(m_RangeIdx) = value
                RangesStartValue = m_RangeStartValue
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(False)>
    Public Property RangesStartValue As Single()
        Get
            Return m_RangeStartValue
        End Get
        Set(ByVal value As Single())
            m_RangeStartValue = value
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The end value of the range. Must be greater than RangeStartValue.")>
    Public Property RangeEndValue As Single
        Get
            Return m_RangeEndValue(m_RangeIdx)
        End Get
        Set(ByVal value As Single)

            If m_RangeEndValue(m_RangeIdx) <> value AndAlso m_RangeStartValue(m_RangeIdx) < value Then
                m_RangeEndValue(m_RangeIdx) = value
                RangesEndValue = m_RangeEndValue
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(False)>
    Public Property RangesEndValue As Single()
        Get
            Return m_RangeEndValue
        End Get
        Set(ByVal value As Single())
            m_RangeEndValue = value
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The inner radius of the range.")>
    Public Property RangeInnerRadius As Integer
        Get
            Return m_RangeInnerRadius(m_RangeIdx)
        End Get
        Set(ByVal value As Integer)

            If m_RangeInnerRadius(m_RangeIdx) <> value Then
                m_RangeInnerRadius(m_RangeIdx) = value
                RangesInnerRadius = m_RangeInnerRadius
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(False)>
    Public Property RangesInnerRadius As Integer()
        Get
            Return m_RangeInnerRadius
        End Get
        Set(ByVal value As Integer())
            m_RangeInnerRadius = value
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The inner radius of the range.")>
    Public Property RangeOuterRadius As Integer
        Get
            Return m_RangeOuterRadius(m_RangeIdx)
        End Get
        Set(ByVal value As Integer)

            If m_RangeOuterRadius(m_RangeIdx) <> value Then
                m_RangeOuterRadius(m_RangeIdx) = value
                RangesOuterRadius = m_RangeOuterRadius
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(False)>
    Public Property RangesOuterRadius As Integer()
        Get
            Return m_RangeOuterRadius
        End Get
        Set(ByVal value As Integer())
            m_RangeOuterRadius = value
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The radius of the scale numbers.")>
    Public Property ScaleNumbersRadius As Integer
        Get
            Return m_ScaleNumbersRadius
        End Get
        Set(ByVal value As Integer)

            If m_ScaleNumbersRadius <> value Then
                m_ScaleNumbersRadius = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The color of the scale numbers.")>
    Public Property ScaleNumbersColor As Color
        Get
            Return m_ScaleNumbersColor
        End Get
        Set(ByVal value As Color)

            If m_ScaleNumbersColor <> value Then
                m_ScaleNumbersColor = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The format of the scale numbers.")>
    Public Property ScaleNumbersFormat As String
        Get
            Return m_ScaleNumbersFormat
        End Get
        Set(ByVal value As String)

            If Not Equals(m_ScaleNumbersFormat, value) Then
                m_ScaleNumbersFormat = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The number of the scale line to start writing numbers next to.")>
    Public Property ScaleNumbersStartScaleLine As Integer
        Get
            Return m_ScaleNumbersStartScaleLine
        End Get
        Set(ByVal value As Integer)

            If m_ScaleNumbersStartScaleLine <> value Then
                m_ScaleNumbersStartScaleLine = Math.Max(value, 1)
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The number of scale line steps for writing numbers.")>
    Public Property ScaleNumbersStepScaleLines As Integer
        Get
            Return m_ScaleNumbersStepScaleLines
        End Get
        Set(ByVal value As Integer)

            If m_ScaleNumbersStepScaleLines <> value Then
                m_ScaleNumbersStepScaleLines = Math.Max(value, 1)
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The angle relative to the tangent of the base arc at a scale line that is used to rotate numbers. set to 0 for no rotation or e.g. set to 90.")>
    Public Property ScaleNumbersRotation As Integer
        Get
            Return m_ScaleNumbersRotation
        End Get
        Set(ByVal value As Integer)

            If m_ScaleNumbersRotation <> value Then
                m_ScaleNumbersRotation = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The type of the needle, currently only type 0 and 1 are supported. Type 0 looks nicers but if you experience performance problems you might consider using type 1.")>
    Public Property NeedleType As Integer
        Get
            Return m_NeedleType
        End Get
        Set(ByVal value As Integer)

            If m_NeedleType <> value Then
                m_NeedleType = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The radius of the needle.")>
    Public Property NeedleRadius As Integer
        Get
            Return m_NeedleRadius
        End Get
        Set(ByVal value As Integer)

            If m_NeedleRadius <> value Then
                m_NeedleRadius = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The first color of the needle.")>
    Public Property NeedleColor1 As NeedleColorEnum
        Get
            Return m_NeedleColor1
        End Get
        Set(ByVal value As NeedleColorEnum)

            If m_NeedleColor1 <> value Then
                m_NeedleColor1 = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The second color of the needle.")>
    Public Property NeedleColor2 As Color
        Get
            Return m_NeedleColor2
        End Get
        Set(ByVal value As Color)

            If m_NeedleColor2 <> value Then
                m_NeedleColor2 = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property

    <Browsable(True), Category("AGauge"), Description("The width of the needle.")>
    Public Property NeedleWidth As Integer
        Get
            Return m_NeedleWidth
        End Get
        Set(ByVal value As Integer)

            If m_NeedleWidth <> value Then
                m_NeedleWidth = value
                drawGaugeBackground = True
                MyBase.Refresh()
            End If
        End Set
    End Property
#End Region

#Region "helper"
    Private Sub FindFontBounds()
        'find upper and lower bounds for numeric characters
        Dim c1 As Integer
        Dim c2 As Integer
        Dim boundfound As Boolean
        Dim b As System.Drawing.Bitmap
        Dim g As Graphics
        Dim backBrush As SolidBrush = New SolidBrush(Color.White)
        Dim foreBrush As SolidBrush = New SolidBrush(Color.Black)
        Dim boundingBox As SizeF
        b = New Bitmap(5, 5)
        g = Graphics.FromImage(b)
        boundingBox = g.MeasureString("0123456789", Font, -1, StringFormat.GenericTypographic)
        'b = New Bitmap(boundingBox.Width, boundingBox.Height)
        g = Graphics.FromImage(b)
        g.FillRectangle(backBrush, 0.0F, 0.0F, boundingBox.Width, boundingBox.Height)
        g.DrawString("0123456789", Font, foreBrush, 0.0F, 0.0F, StringFormat.GenericTypographic)
        fontBoundY1 = 0
        fontBoundY2 = 0
        c1 = 0
        boundfound = False

        While c1 < b.Height AndAlso Not boundfound
            c2 = 0

            While c2 < b.Width AndAlso Not boundfound

                If b.GetPixel(c2, c1) <> backBrush.Color Then
                    fontBoundY1 = c1
                    boundfound = True
                End If

                c2 += 1
            End While

            c1 += 1
        End While

        c1 = b.Height - 1
        boundfound = False

        While 0 < c1 AndAlso Not boundfound
            c2 = 0

            While c2 < b.Width AndAlso Not boundfound

                If b.GetPixel(c2, c1) <> backBrush.Color Then
                    fontBoundY2 = c1
                    boundfound = True
                End If

                c2 += 1
            End While

            c1 -= 1
        End While
    End Sub
#End Region

#Region "base member overrides"
    Protected Overrides Sub OnPaintBackground(ByVal pevent As PaintEventArgs)
    End Sub

    Protected Overrides Sub OnPaint(ByVal pe As PaintEventArgs)
        If Width < 10 OrElse Height < 10 Then
            Return
        End If

        If drawGaugeBackground Then
            drawGaugeBackground = False
            FindFontBounds()
            gaugeBitmap = New Bitmap(Width, Height, pe.Graphics)
            Dim ggr = Graphics.FromImage(gaugeBitmap)
            ggr.FillRectangle(New SolidBrush(BackColor), ClientRectangle)

            If MyBase.BackgroundImage IsNot Nothing Then
                Select Case BackgroundImageLayout
                    Case ImageLayout.Center
                        ggr.DrawImageUnscaled(MyBase.BackgroundImage, Width / 2 - MyBase.BackgroundImage.Width / 2, Height / 2 - MyBase.BackgroundImage.Height / 2)
                    Case ImageLayout.None
                        ggr.DrawImageUnscaled(MyBase.BackgroundImage, 0, 0)
                    Case ImageLayout.Stretch
                        ggr.DrawImage(MyBase.BackgroundImage, 0, 0, Width, Height)
                    Case ImageLayout.Tile
                        Dim pixelOffsetX = 0
                        Dim pixelOffsetY = 0

                        While pixelOffsetX < Width
                            pixelOffsetY = 0

                            While pixelOffsetY < Height
                                ggr.DrawImageUnscaled(MyBase.BackgroundImage, pixelOffsetX, pixelOffsetY)
                                pixelOffsetY += MyBase.BackgroundImage.Height
                            End While

                            pixelOffsetX += MyBase.BackgroundImage.Width
                        End While

                    Case ImageLayout.Zoom

                        If CSng(MyBase.BackgroundImage.Width / Width) < CSng(MyBase.BackgroundImage.Height / Height) Then
                            ggr.DrawImage(MyBase.BackgroundImage, 0, 0, Height, Height)
                        Else
                            ggr.DrawImage(MyBase.BackgroundImage, 0, 0, Width, Width)
                        End If
                End Select
            End If

            ggr.SmoothingMode = SmoothingMode.HighQuality
            ggr.PixelOffsetMode = PixelOffsetMode.HighQuality
            Dim gp As GraphicsPath = New GraphicsPath()
            Dim rangeStartAngle As Single
            Dim rangeSweepAngle As Single

            For counter = 0 To NUMOFRANGES - 1

                If m_RangeEndValue(counter) > m_RangeStartValue(counter) AndAlso m_RangeEnabled(counter) Then
                    rangeStartAngle = m_BaseArcStart + (m_RangeStartValue(counter) - m_MinValue) * m_BaseArcSweep / (m_MaxValue - m_MinValue)
                    rangeSweepAngle = (m_RangeEndValue(counter) - m_RangeStartValue(counter)) * m_BaseArcSweep / (m_MaxValue - m_MinValue)
                    gp.Reset()
                    gp.AddPie(New Rectangle(m_Center.X - m_RangeOuterRadius(counter), m_Center.Y - m_RangeOuterRadius(counter), 2 * m_RangeOuterRadius(counter), 2 * m_RangeOuterRadius(counter)), rangeStartAngle, rangeSweepAngle)
                    gp.Reverse()
                    gp.AddPie(New Rectangle(m_Center.X - m_RangeInnerRadius(counter), m_Center.Y - m_RangeInnerRadius(counter), 2 * m_RangeInnerRadius(counter), 2 * m_RangeInnerRadius(counter)), rangeStartAngle, rangeSweepAngle)
                    gp.Reverse()
                    ggr.SetClip(gp)
                    ggr.FillPie(New SolidBrush(m_RangeColor(counter)), New Rectangle(m_Center.X - m_RangeOuterRadius(counter), m_Center.Y - m_RangeOuterRadius(counter), 2 * m_RangeOuterRadius(counter), 2 * m_RangeOuterRadius(counter)), rangeStartAngle, rangeSweepAngle)
                End If
            Next

            ggr.SetClip(ClientRectangle)

            If m_BaseArcRadius > 0 Then
                ggr.DrawArc(New Pen(m_BaseArcColor, m_BaseArcWidth), New Rectangle(m_Center.X - m_BaseArcRadius, m_Center.Y - m_BaseArcRadius, 2 * m_BaseArcRadius, 2 * m_BaseArcRadius), m_BaseArcStart, m_BaseArcSweep)
            End If

            Dim valueText = ""
            Dim boundingBox As SizeF
            Dim countValue As Single = 0
            Dim counter1 = 0

            While countValue <= m_MaxValue - m_MinValue
                valueText = (m_MinValue + countValue).ToString(m_ScaleNumbersFormat)
                ggr.ResetTransform()
                boundingBox = ggr.MeasureString(valueText, Font, -1, StringFormat.GenericTypographic)
                gp.Reset()
                gp.AddEllipse(New Rectangle(m_Center.X - m_ScaleLinesMajorOuterRadius, m_Center.Y - m_ScaleLinesMajorOuterRadius, 2 * m_ScaleLinesMajorOuterRadius, 2 * m_ScaleLinesMajorOuterRadius))
                gp.Reverse()
                gp.AddEllipse(New Rectangle(m_Center.X - m_ScaleLinesMajorInnerRadius, m_Center.Y - m_ScaleLinesMajorInnerRadius, 2 * m_ScaleLinesMajorInnerRadius, 2 * m_ScaleLinesMajorInnerRadius))
                gp.Reverse()
                ggr.SetClip(gp)
                ggr.DrawLine(New Pen(m_ScaleLinesMajorColor, m_ScaleLinesMajorWidth), Center.X, Center.Y, CSng(Center.X + 2 * m_ScaleLinesMajorOuterRadius * Math.Cos((m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue)) * Math.PI / 180.0)), CSng(Center.Y + 2 * m_ScaleLinesMajorOuterRadius * Math.Sin((m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue)) * Math.PI / 180.0)))
                gp.Reset()
                gp.AddEllipse(New Rectangle(m_Center.X - m_ScaleLinesMinorOuterRadius, m_Center.Y - m_ScaleLinesMinorOuterRadius, 2 * m_ScaleLinesMinorOuterRadius, 2 * m_ScaleLinesMinorOuterRadius))
                gp.Reverse()
                gp.AddEllipse(New Rectangle(m_Center.X - m_ScaleLinesMinorInnerRadius, m_Center.Y - m_ScaleLinesMinorInnerRadius, 2 * m_ScaleLinesMinorInnerRadius, 2 * m_ScaleLinesMinorInnerRadius))
                gp.Reverse()
                ggr.SetClip(gp)

                If countValue < m_MaxValue - m_MinValue Then
                    For counter2 = 1 To m_ScaleLinesMinorNumOf

                        If m_ScaleLinesMinorNumOf Mod 2 = 1 AndAlso CInt(m_ScaleLinesMinorNumOf / 2) + 1 = counter2 Then
                            gp.Reset()
                            gp.AddEllipse(New Rectangle(m_Center.X - m_ScaleLinesInterOuterRadius, m_Center.Y - m_ScaleLinesInterOuterRadius, 2 * m_ScaleLinesInterOuterRadius, 2 * m_ScaleLinesInterOuterRadius))
                            gp.Reverse()
                            gp.AddEllipse(New Rectangle(m_Center.X - m_ScaleLinesInterInnerRadius, m_Center.Y - m_ScaleLinesInterInnerRadius, 2 * m_ScaleLinesInterInnerRadius, 2 * m_ScaleLinesInterInnerRadius))
                            gp.Reverse()
                            ggr.SetClip(gp)
                            ggr.DrawLine(New Pen(m_ScaleLinesInterColor, m_ScaleLinesInterWidth), Center.X, Center.Y, CSng(Center.X + 2 * m_ScaleLinesInterOuterRadius * Math.Cos((m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue) + counter2 * m_BaseArcSweep / ((m_MaxValue - m_MinValue) / m_ScaleLinesMajorStepValue * (m_ScaleLinesMinorNumOf + 1))) * Math.PI / 180.0)), CSng(Center.Y + 2 * m_ScaleLinesInterOuterRadius * Math.Sin((m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue) + counter2 * m_BaseArcSweep / ((m_MaxValue - m_MinValue) / m_ScaleLinesMajorStepValue * (m_ScaleLinesMinorNumOf + 1))) * Math.PI / 180.0)))
                            gp.Reset()
                            gp.AddEllipse(New Rectangle(m_Center.X - m_ScaleLinesMinorOuterRadius, m_Center.Y - m_ScaleLinesMinorOuterRadius, 2 * m_ScaleLinesMinorOuterRadius, 2 * m_ScaleLinesMinorOuterRadius))
                            gp.Reverse()
                            gp.AddEllipse(New Rectangle(m_Center.X - m_ScaleLinesMinorInnerRadius, m_Center.Y - m_ScaleLinesMinorInnerRadius, 2 * m_ScaleLinesMinorInnerRadius, 2 * m_ScaleLinesMinorInnerRadius))
                            gp.Reverse()
                            ggr.SetClip(gp)
                        Else
                            ggr.DrawLine(New Pen(m_ScaleLinesMinorColor, m_ScaleLinesMinorWidth), Center.X, Center.Y, CSng(Center.X + 2 * m_ScaleLinesMinorOuterRadius * Math.Cos((m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue) + counter2 * m_BaseArcSweep / ((m_MaxValue - m_MinValue) / m_ScaleLinesMajorStepValue * (m_ScaleLinesMinorNumOf + 1))) * Math.PI / 180.0)), CSng(Center.Y + 2 * m_ScaleLinesMinorOuterRadius * Math.Sin((m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue) + counter2 * m_BaseArcSweep / ((m_MaxValue - m_MinValue) / m_ScaleLinesMajorStepValue * (m_ScaleLinesMinorNumOf + 1))) * Math.PI / 180.0)))
                        End If
                    Next
                End If

                ggr.SetClip(ClientRectangle)

                If m_ScaleNumbersRotation <> 0 Then
                    ggr.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                    ggr.RotateTransform(90.0F + m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue))
                End If

                ggr.TranslateTransform(Center.X + m_ScaleNumbersRadius * Math.Cos((m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue)) * Math.PI / 180.0F), Center.Y + m_ScaleNumbersRadius * Math.Sin((m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue)) * Math.PI / 180.0F), MatrixOrder.Append)

                If counter1 >= ScaleNumbersStartScaleLine - 1 Then
                    ggr.DrawString(valueText, Font, New SolidBrush(m_ScaleNumbersColor), -boundingBox.Width / 2, -fontBoundY1 - (fontBoundY2 - fontBoundY1 + 1) / 2, StringFormat.GenericTypographic)
                End If

                countValue += m_ScaleLinesMajorStepValue
                counter1 += 1
            End While

            ggr.ResetTransform()
            ggr.SetClip(ClientRectangle)

            If m_ScaleNumbersRotation <> 0 Then
                ggr.TextRenderingHint = Drawing.Text.TextRenderingHint.SystemDefault
            End If

            For counter = 0 To NUMOFCAPS - 1

                If Not Equals(m_CapText(counter), "") Then
                    ggr.DrawString(m_CapText(counter), Font, New SolidBrush(m_CapColor(counter)), m_CapPosition(counter).X, m_CapPosition(counter).Y, StringFormat.GenericTypographic)
                End If
            Next
        End If

        pe.Graphics.DrawImageUnscaled(gaugeBitmap, 0, 0)
        pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias
        pe.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality
        Dim brushAngle As Single = CInt(m_BaseArcStart + (m_value - m_MinValue) * m_BaseArcSweep / (m_MaxValue - m_MinValue)) Mod 360
        Dim needleAngle = brushAngle * Math.PI / 180

        Select Case m_NeedleType
            Case 0
                Dim points = New PointF(2) {}
                Dim brush1 = Brushes.White
                Dim brush2 = Brushes.White
                Dim brush3 = Brushes.White
                Dim brush4 = Brushes.White
                Dim brushBucket = Brushes.White
                Dim subcol As Integer = ((brushAngle + 225) Mod 180) * 100 / 180
                Dim subcol2 As Integer = ((brushAngle + 135) Mod 180) * 100 / 180
                pe.Graphics.FillEllipse(New SolidBrush(m_NeedleColor2), Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6)

                Select Case m_NeedleColor1
                    Case NeedleColorEnum.Gray
                        brush1 = New SolidBrush(Color.FromArgb(80 + subcol, 80 + subcol, 80 + subcol))
                        brush2 = New SolidBrush(Color.FromArgb(180 - subcol, 180 - subcol, 180 - subcol))
                        brush3 = New SolidBrush(Color.FromArgb(80 + subcol2, 80 + subcol2, 80 + subcol2))
                        brush4 = New SolidBrush(Color.FromArgb(180 - subcol2, 180 - subcol2, 180 - subcol2))
                        pe.Graphics.DrawEllipse(Pens.Gray, Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6)
                    Case NeedleColorEnum.Red
                        brush1 = New SolidBrush(Color.FromArgb(145 + subcol, subcol, subcol))
                        brush2 = New SolidBrush(Color.FromArgb(245 - subcol, 100 - subcol, 100 - subcol))
                        brush3 = New SolidBrush(Color.FromArgb(145 + subcol2, subcol2, subcol2))
                        brush4 = New SolidBrush(Color.FromArgb(245 - subcol2, 100 - subcol2, 100 - subcol2))
                        pe.Graphics.DrawEllipse(Pens.Red, Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6)
                    Case NeedleColorEnum.Green
                        brush1 = New SolidBrush(Color.FromArgb(subcol, 145 + subcol, subcol))
                        brush2 = New SolidBrush(Color.FromArgb(100 - subcol, 245 - subcol, 100 - subcol))
                        brush3 = New SolidBrush(Color.FromArgb(subcol2, 145 + subcol2, subcol2))
                        brush4 = New SolidBrush(Color.FromArgb(100 - subcol2, 245 - subcol2, 100 - subcol2))
                        pe.Graphics.DrawEllipse(Pens.Green, Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6)
                    Case NeedleColorEnum.Blue
                        brush1 = New SolidBrush(Color.FromArgb(subcol, subcol, 145 + subcol))
                        brush2 = New SolidBrush(Color.FromArgb(100 - subcol, 100 - subcol, 245 - subcol))
                        brush3 = New SolidBrush(Color.FromArgb(subcol2, subcol2, 145 + subcol2))
                        brush4 = New SolidBrush(Color.FromArgb(100 - subcol2, 100 - subcol2, 245 - subcol2))
                        pe.Graphics.DrawEllipse(Pens.Blue, Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6)
                    Case NeedleColorEnum.Magenta
                        brush1 = New SolidBrush(Color.FromArgb(subcol, 145 + subcol, 145 + subcol))
                        brush2 = New SolidBrush(Color.FromArgb(100 - subcol, 245 - subcol, 245 - subcol))
                        brush3 = New SolidBrush(Color.FromArgb(subcol2, 145 + subcol2, 145 + subcol2))
                        brush4 = New SolidBrush(Color.FromArgb(100 - subcol2, 245 - subcol2, 245 - subcol2))
                        pe.Graphics.DrawEllipse(Pens.Magenta, Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6)
                    Case NeedleColorEnum.Violet
                        brush1 = New SolidBrush(Color.FromArgb(145 + subcol, subcol, 145 + subcol))
                        brush2 = New SolidBrush(Color.FromArgb(245 - subcol, 100 - subcol, 245 - subcol))
                        brush3 = New SolidBrush(Color.FromArgb(145 + subcol2, subcol2, 145 + subcol2))
                        brush4 = New SolidBrush(Color.FromArgb(245 - subcol2, 100 - subcol2, 245 - subcol2))
                        pe.Graphics.DrawEllipse(Pens.Violet, Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6)
                    Case NeedleColorEnum.Yellow
                        brush1 = New SolidBrush(Color.FromArgb(145 + subcol, 145 + subcol, subcol))
                        brush2 = New SolidBrush(Color.FromArgb(245 - subcol, 245 - subcol, 100 - subcol))
                        brush3 = New SolidBrush(Color.FromArgb(145 + subcol2, 145 + subcol2, subcol2))
                        brush4 = New SolidBrush(Color.FromArgb(245 - subcol2, 245 - subcol2, 100 - subcol2))
                        pe.Graphics.DrawEllipse(Pens.Violet, Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6)
                End Select

                If Math.Floor(CSng(((brushAngle + 225) Mod 360) / 180.0)) = 0 Then
                    brushBucket = brush1
                    brush1 = brush2
                    brush2 = brushBucket
                End If

                If Math.Floor(CSng(((brushAngle + 135) Mod 360) / 180.0)) = 0 Then
                    brush4 = brush3
                End If

                points(0).X = CSng(Center.X + m_NeedleRadius * Math.Cos(needleAngle))
                points(0).Y = CSng(Center.Y + m_NeedleRadius * Math.Sin(needleAngle))
                points(1).X = CSng(Center.X - m_NeedleRadius / 20 * Math.Cos(needleAngle))
                points(1).Y = CSng(Center.Y - m_NeedleRadius / 20 * Math.Sin(needleAngle))
                points(2).X = CSng(Center.X - m_NeedleRadius / 5 * Math.Cos(needleAngle) + m_NeedleWidth * 2 * Math.Cos(needleAngle + Math.PI / 2))
                points(2).Y = CSng(Center.Y - m_NeedleRadius / 5 * Math.Sin(needleAngle) + m_NeedleWidth * 2 * Math.Sin(needleAngle + Math.PI / 2))
                pe.Graphics.FillPolygon(brush1, points)
                points(2).X = CSng(Center.X - m_NeedleRadius / 5 * Math.Cos(needleAngle) + m_NeedleWidth * 2 * Math.Cos(needleAngle - Math.PI / 2))
                points(2).Y = CSng(Center.Y - m_NeedleRadius / 5 * Math.Sin(needleAngle) + m_NeedleWidth * 2 * Math.Sin(needleAngle - Math.PI / 2))
                pe.Graphics.FillPolygon(brush2, points)
                points(0).X = CSng(Center.X - (m_NeedleRadius / 20 - 1) * Math.Cos(needleAngle))
                points(0).Y = CSng(Center.Y - (m_NeedleRadius / 20 - 1) * Math.Sin(needleAngle))
                points(1).X = CSng(Center.X - m_NeedleRadius / 5 * Math.Cos(needleAngle) + m_NeedleWidth * 2 * Math.Cos(needleAngle + Math.PI / 2))
                points(1).Y = CSng(Center.Y - m_NeedleRadius / 5 * Math.Sin(needleAngle) + m_NeedleWidth * 2 * Math.Sin(needleAngle + Math.PI / 2))
                points(2).X = CSng(Center.X - m_NeedleRadius / 5 * Math.Cos(needleAngle) + m_NeedleWidth * 2 * Math.Cos(needleAngle - Math.PI / 2))
                points(2).Y = CSng(Center.Y - m_NeedleRadius / 5 * Math.Sin(needleAngle) + m_NeedleWidth * 2 * Math.Sin(needleAngle - Math.PI / 2))
                pe.Graphics.FillPolygon(brush4, points)
                points(0).X = CSng(Center.X - m_NeedleRadius / 20 * Math.Cos(needleAngle))
                points(0).Y = CSng(Center.Y - m_NeedleRadius / 20 * Math.Sin(needleAngle))
                points(1).X = CSng(Center.X + m_NeedleRadius * Math.Cos(needleAngle))
                points(1).Y = CSng(Center.Y + m_NeedleRadius * Math.Sin(needleAngle))
                pe.Graphics.DrawLine(New Pen(m_NeedleColor2), Center.X, Center.Y, points(0).X, points(0).Y)
                pe.Graphics.DrawLine(New Pen(m_NeedleColor2), Center.X, Center.Y, points(1).X, points(1).Y)
            Case 1
                Dim startPoint As Point = New Point(Center.X - m_NeedleRadius / 8 * Math.Cos(needleAngle), Center.Y - m_NeedleRadius / 8 * Math.Sin(needleAngle))
                Dim endPoint As Point = New Point(Center.X + m_NeedleRadius * Math.Cos(needleAngle), Center.Y + m_NeedleRadius * Math.Sin(needleAngle))
                pe.Graphics.FillEllipse(New SolidBrush(m_NeedleColor2), Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6)

                Select Case m_NeedleColor1
                    Case NeedleColorEnum.Gray
                        pe.Graphics.DrawLine(New Pen(Color.DarkGray, m_NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y)
                        pe.Graphics.DrawLine(New Pen(Color.DarkGray, m_NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y)
                    Case NeedleColorEnum.Red
                        pe.Graphics.DrawLine(New Pen(Color.Red, m_NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y)
                        pe.Graphics.DrawLine(New Pen(Color.Red, m_NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y)
                    Case NeedleColorEnum.Green
                        pe.Graphics.DrawLine(New Pen(Color.Green, m_NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y)
                        pe.Graphics.DrawLine(New Pen(Color.Green, m_NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y)
                    Case NeedleColorEnum.Blue
                        pe.Graphics.DrawLine(New Pen(Color.Blue, m_NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y)
                        pe.Graphics.DrawLine(New Pen(Color.Blue, m_NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y)
                    Case NeedleColorEnum.Magenta
                        pe.Graphics.DrawLine(New Pen(Color.Magenta, m_NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y)
                        pe.Graphics.DrawLine(New Pen(Color.Magenta, m_NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y)
                    Case NeedleColorEnum.Violet
                        pe.Graphics.DrawLine(New Pen(Color.Violet, m_NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y)
                        pe.Graphics.DrawLine(New Pen(Color.Violet, m_NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y)
                    Case NeedleColorEnum.Yellow
                        pe.Graphics.DrawLine(New Pen(Color.Yellow, m_NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y)
                        pe.Graphics.DrawLine(New Pen(Color.Yellow, m_NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y)
                End Select
        End Select
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        drawGaugeBackground = True
        MyBase.Refresh()
    End Sub
#End Region

End Class



<System.ComponentModel.Designer(GetType(GaugeDesigner))>
Partial Class VS7_Gauge
    Inherits Control
#Region "PLC Properties"
    Private _PLC As Integer
    Private _DataArea As General.DataArea = DataArea.DB
    Private _DB As Integer
    Private _Byte As Integer
    Private _Bit As Integer
    Private _DataType As General.DataType = DataType.INT
    Private _Length As Integer
    Private _txt As String
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
#Region "Control Events"



#End Region
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
            Case Else
        End Select
        Me.Value = Me.pLC_Value
        Me.Refresh()
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
#Region "Gauge Smart tags"

Public Class GaugeDesigner
    Inherits ControlDesigner

    Private _actionListCollection As DesignerActionListCollection

    Public Overrides ReadOnly Property ActionLists() As System.ComponentModel.Design.DesignerActionListCollection
        Get
            If _actionListCollection Is Nothing Then
                _actionListCollection = New DesignerActionListCollection()
                _actionListCollection.Add(New GaugeActionList(Me.Control))
            End If

            Return _actionListCollection
        End Get
    End Property
End Class

Friend Class GaugeActionList
    Inherits DesignerActionList

    Private ctr As VS7_Gauge
    Private designerActionSvc As DesignerActionUIService

    Public Sub New(ByVal component As IComponent)
        MyBase.New(component)

        ctr = DirectCast(component, VS7_Gauge)
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

        'Return the ActionItemCollection
        Return items
    End Function
End Class

#End Region
