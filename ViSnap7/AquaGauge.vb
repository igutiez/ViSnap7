Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Drawing.Drawing2D

Namespace AquaControls
    ''' <summary>
    ''' Aqua Gauge Control - A Windows User Control.
    ''' Author  : Ambalavanar Thirugnanam
    ''' Date    : 24th August 2007
    ''' email   : ambalavanar.thiru@gmail.com
    ''' This is control is for free. You can use for any commercial or non-commercial purposes.
    ''' [Please do no remove this header when using this control in your application.]
    ''' </summary>
    Public Partial Class AquaGauge
        Inherits UserControl
#Region "Private Attributes"
        Private minValueField As Single
        Private maxValueField As Single
        Private threshold As Single
        Private currentValue As Single
        Private recommendedValueField As Single
        Private noOfDivisionsField As Integer
        Private noOfSubDivisionsField As Integer
        Private dialTextField As String
        Private dialColorField As Color = Color.Lavender
        Private glossinessAlpha As Single = 25
        Private oldWidth, oldHeight As Integer
        Private x, y, width, height As Integer
        Private fromAngle As Single = 135F
        Private toAngle As Single = 405F
        Private enableTransparentBackgroundField As Boolean
        Private requiresRedraw As Boolean
        Private backgroundImg As Image
        Private rectImg As Rectangle
#End Region

        Public Sub New()
            InitializeComponent()
            x = 5
            y = 5
            width = Width - 10
            height = Height - 10
            noOfDivisionsField = 10
            noOfSubDivisionsField = 3
            SetStyle(ControlStyles.SupportsTransparentBackColor, True)
            SetStyle(ControlStyles.ResizeRedraw, True)
            SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            SetStyle(ControlStyles.UserPaint, True)
            SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            BackColor = Color.Transparent
            AddHandler Resize, New EventHandler(AddressOf AquaGauge_Resize)
            requiresRedraw = True
        End Sub

#Region "Public Properties"
        ''' <summary>
        ''' Mininum value on the scale
        ''' </summary>
        <DefaultValue(0)>
        <Description("Mininum value on the scale")>
        Public Property MinValue As Single
            Get
                Return minValueField
            End Get
            Set(ByVal value As Single)

                If value < maxValueField Then
                    minValueField = value
                    If currentValue < minValueField Then currentValue = minValueField
                    If recommendedValueField < minValueField Then recommendedValueField = minValueField
                    requiresRedraw = True
                    Invalidate()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Maximum value on the scale
        ''' </summary>
        <DefaultValue(100)>
        <Description("Maximum value on the scale")>
        Public Property MaxValue As Single
            Get
                Return maxValueField
            End Get
            Set(ByVal value As Single)

                If value > minValueField Then
                    maxValueField = value
                    If currentValue > maxValueField Then currentValue = maxValueField
                    If recommendedValueField > maxValueField Then recommendedValueField = maxValueField
                    requiresRedraw = True
                    Invalidate()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or Sets the Threshold area from the Recommended Value. (1-99%)
        ''' </summary>
        <DefaultValue(25)>
        <Description("Gets or Sets the Threshold area from the Recommended Value. (1-99%)")>
        Public Property ThresholdPercent As Single
            Get
                Return threshold
            End Get
            Set(ByVal value As Single)

                If value > 0 AndAlso value < 100 Then
                    threshold = value
                    requiresRedraw = True
                    Invalidate()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Threshold value from which green area will be marked.
        ''' </summary>
        <DefaultValue(25)>
        <Description("Threshold value from which green area will be marked.")>
        Public Property RecommendedValue As Single
            Get
                Return recommendedValueField
            End Get
            Set(ByVal value As Single)

                If value > minValueField AndAlso value < maxValueField Then
                    recommendedValueField = value
                    requiresRedraw = True
                    Invalidate()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Value where the pointer will point to.
        ''' </summary>
        <DefaultValue(0)>
        <Description("Value where the pointer will point to.")>
        Public Property Value As Single
            Get
                Return currentValue
            End Get
            Set(ByVal value As Single)

                If value >= minValueField AndAlso value <= maxValueField Then
                    currentValue = value
                    Refresh()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Background color of the dial
        ''' </summary>
        <Description("Background color of the dial")>
        Public Property DialColor As Color
            Get
                Return dialColorField
            End Get
            Set(ByVal value As Color)
                dialColorField = value
                requiresRedraw = True
                Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Glossiness strength. Range: 0-100
        ''' </summary>
        <DefaultValue(72)>
        <Description("Glossiness strength. Range: 0-100")>
        Public Property Glossiness As Single
            Get
                Return glossinessAlpha * 100 / 220
            End Get
            Set(ByVal value As Single)
                Dim val = value
                If val > 100 Then value = 100
                If val < 0 Then value = 0
                glossinessAlpha = value * 220 / 100
                Refresh()
            End Set
        End Property

        ''' <summary>
        ''' Get or Sets the number of Divisions in the dial scale.
        ''' </summary>
        <DefaultValue(10)>
        <Description("Get or Sets the number of Divisions in the dial scale.")>
        Public Property NoOfDivisions As Integer
            Get
                Return noOfDivisionsField
            End Get
            Set(ByVal value As Integer)

                If value > 1 AndAlso value < 25 Then
                    noOfDivisionsField = value
                    requiresRedraw = True
                    Invalidate()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or Sets the number of Sub Divisions in the scale per Division.
        ''' </summary>
        <DefaultValue(3)>
        <Description("Gets or Sets the number of Sub Divisions in the scale per Division.")>
        Public Property NoOfSubDivisions As Integer
            Get
                Return noOfSubDivisionsField
            End Get
            Set(ByVal value As Integer)

                If value > 0 AndAlso value <= 10 Then
                    noOfSubDivisionsField = value
                    requiresRedraw = True
                    Invalidate()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or Sets the Text to be displayed in the dial
        ''' </summary>
        <Description("Gets or Sets the Text to be displayed in the dial")>
        Public Property DialText As String
            Get
                Return dialTextField
            End Get
            Set(ByVal value As String)
                dialTextField = value
                requiresRedraw = True
                Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Enables or Disables Transparent Background color.
        ''' Note: Enabling this will reduce the performance and may make the control flicker.
        ''' </summary>
        <DefaultValue(False)>
        <Description("Enables or Disables Transparent Background color. Note: Enabling this will reduce the performance and may make the control flicker.")>
        Public Property EnableTransparentBackground As Boolean
            Get
                Return enableTransparentBackgroundField
            End Get
            Set(ByVal value As Boolean)
                enableTransparentBackgroundField = value
                SetStyle(ControlStyles.OptimizedDoubleBuffer, Not enableTransparentBackgroundField)
                requiresRedraw = True
                Refresh()
            End Set
        End Property
#End Region

#Region "Overriden Control methods"
        ''' <summary>
        ''' Draws the pointer.
        ''' </summary>
        ''' <paramname="e"></param>
        Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
            width = Width - x * 2
            height = Height - y * 2
            DrawPointer(e.Graphics, width / 2 + x, height / 2 + y)
        End Sub

        ''' <summary>
        ''' Draws the dial background.
        ''' </summary>
        ''' <paramname="e"></param>
        Protected Overrides Sub OnPaintBackground(ByVal e As PaintEventArgs)
            If Not enableTransparentBackgroundField Then
                MyBase.OnPaintBackground(e)
            End If

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality
            e.Graphics.FillRectangle(New SolidBrush(Color.Transparent), New Rectangle(0, 0, MyBase.Width, MyBase.Height))

            If backgroundImg Is Nothing OrElse requiresRedraw Then
                backgroundImg = New Bitmap(Width, Height)
                Dim g = Graphics.FromImage(backgroundImg)
                g.SmoothingMode = SmoothingMode.HighQuality
                width = Width - x * 2
                height = Height - y * 2
                rectImg = New Rectangle(x, y, width, height)

                'Draw background color
                Dim backGroundBrush As Brush = New SolidBrush(Color.FromArgb(120, dialColorField))

                If enableTransparentBackgroundField AndAlso Parent IsNot Nothing Then
                    Dim gg As Single = width / 60
                    'g.FillEllipse(new SolidBrush(this.Parent.BackColor), -gg, -gg, this.Width+gg*2, this.Height+gg*2);
                End If

                g.FillEllipse(backGroundBrush, x, y, width, height)

                'Draw Rim
                Dim outlineBrush As SolidBrush = New SolidBrush(Color.FromArgb(100, Color.SlateGray))
                Dim outline As Pen = New Pen(outlineBrush, width * .03)
                g.DrawEllipse(outline, rectImg)
                Dim darkRim As Pen = New Pen(Color.SlateGray)
                g.DrawEllipse(darkRim, x, y, width, height)

                'Draw Callibration
                DrawCalibration(g, rectImg, width / 2 + x, height / 2 + y)

                'Draw Colored Rim
                Dim colorPen As Pen = New Pen(Color.FromArgb(190, Color.Gainsboro), Width / 40)
                Dim blackPen As Pen = New Pen(Color.FromArgb(250, Color.Black), Width / 200)
                Dim gap As Integer = Width * 0.03F
                Dim rectg As Rectangle = New Rectangle(rectImg.X + gap, rectImg.Y + gap, rectImg.Width - gap * 2, rectImg.Height - gap * 2)
                g.DrawArc(colorPen, rectg, 135, 270)

                'Draw Threshold
                colorPen = New Pen(Color.FromArgb(200, Color.LawnGreen), Width / 50)
                rectg = New Rectangle(rectImg.X + gap, rectImg.Y + gap, rectImg.Width - gap * 2, rectImg.Height - gap * 2)
                Dim val = MaxValue - MinValue
                val = 100 * (recommendedValueField - MinValue) / val
                val = (toAngle - fromAngle) * val / 100
                val += fromAngle
                Dim stAngle = val - 270 * threshold / 200
                If stAngle <= 135 Then stAngle = 135
                Dim sweepAngle = 270 * threshold / 100
                If stAngle + sweepAngle > 405 Then sweepAngle = 405 - stAngle
                g.DrawArc(colorPen, rectg, stAngle, sweepAngle)

                'Draw Digital Value
                Dim digiRect As RectangleF = New RectangleF(Width / 2F - width / 5F, height / 1.2F, width / 2.5F, Height / 9F)
                Dim digiFRect As RectangleF = New RectangleF(Width / 2 - width / 7, CInt(height / 1.18), width / 4, Height / 12)
                g.FillRectangle(New SolidBrush(Color.FromArgb(30, Color.Gray)), digiRect)
                DisplayNumber(g, currentValue, digiFRect)
                Dim textSize = g.MeasureString(dialTextField, Font)
                Dim digiFRectText As RectangleF = New RectangleF(Width / 2 - textSize.Width / 2, CInt(height / 1.5), textSize.Width, textSize.Height)
                g.DrawString(dialTextField, Font, New SolidBrush(ForeColor), digiFRectText)
                requiresRedraw = False
            End If

            e.Graphics.DrawImage(backgroundImg, rectImg)
        End Sub

        Protected Overrides ReadOnly Property CreateParams As CreateParams
            Get
                Dim cp = MyBase.CreateParams
                cp.ExStyle = cp.ExStyle Or &H20
                Return cp
            End Get
        End Property
#End Region

#Region "Private methods"
        ''' <summary>
        ''' Draws the Pointer.
        ''' </summary>
        ''' <paramname="gr"></param>
        ''' <paramname="cx"></param>
        ''' <paramname="cy"></param>
        Private Sub DrawPointer(ByVal gr As Graphics, ByVal cx As Integer, ByVal cy As Integer)
            Dim radius As Single = Width / 2 - Width * .12F
            Dim val = MaxValue - MinValue
            Dim img As Image = New Bitmap(Width, Height)
            Dim g = Graphics.FromImage(img)
            g.SmoothingMode = SmoothingMode.AntiAlias
            val = 100 * (currentValue - MinValue) / val
            val = (toAngle - fromAngle) * val / 100
            val += fromAngle
            Dim angle = GetRadian(val)
            Dim gradientAngle = angle
            Dim pts = New PointF(4) {}
            pts(0).X = CSng(cx + radius * Math.Cos(angle))
            pts(0).Y = CSng(cy + radius * Math.Sin(angle))
            pts(4).X = CSng(cx + radius * Math.Cos(angle - 0.02))
            pts(4).Y = CSng(cy + radius * Math.Sin(angle - 0.02))
            angle = GetRadian(val + 20)
            pts(1).X = CSng(cx + Width * .09F * Math.Cos(angle))
            pts(1).Y = CSng(cy + Width * .09F * Math.Sin(angle))
            pts(2).X = cx
            pts(2).Y = cy
            angle = GetRadian(val - 20)
            pts(3).X = CSng(cx + Width * .09F * Math.Cos(angle))
            pts(3).Y = CSng(cy + Width * .09F * Math.Sin(angle))
            Dim pointer As Brush = New SolidBrush(Color.Black)
            g.FillPolygon(pointer, pts)
            Dim shinePts = New PointF(2) {}
            angle = GetRadian(val)
            shinePts(0).X = CSng(cx + radius * Math.Cos(angle))
            shinePts(0).Y = CSng(cy + radius * Math.Sin(angle))
            angle = GetRadian(val + 20)
            shinePts(1).X = CSng(cx + Width * .09F * Math.Cos(angle))
            shinePts(1).Y = CSng(cy + Width * .09F * Math.Sin(angle))
            shinePts(2).X = cx
            shinePts(2).Y = cy
            Dim gpointer As LinearGradientBrush = New LinearGradientBrush(shinePts(0), shinePts(2), Color.SlateGray, Color.Black)
            g.FillPolygon(gpointer, shinePts)
            Dim rect As Rectangle = New Rectangle(x, y, width, height)
            DrawCenterPoint(g, rect, width / 2 + x, height / 2 + y)
            DrawGloss(g)
            gr.DrawImage(img, 0, 0)
        End Sub

        ''' <summary>
        ''' Draws the glossiness.
        ''' </summary>
        ''' <paramname="g"></param>
        Private Sub DrawGloss(ByVal g As Graphics)
            Dim glossRect As RectangleF = New RectangleF(x + CSng(width * 0.10), y + CSng(height * 0.07), width * 0.80, height * 0.7)
            Dim gradientBrush As LinearGradientBrush = New LinearGradientBrush(glossRect, Color.FromArgb(glossinessAlpha, Color.White), Color.Transparent, LinearGradientMode.Vertical)
            g.FillEllipse(gradientBrush, glossRect)

            'TODO: Gradient from bottom
            glossRect = New RectangleF(x + CSng(width * 0.25), y + CSng(height * 0.77), width * 0.50, height * 0.2)
            Dim gloss As Integer = glossinessAlpha / 3
            gradientBrush = New LinearGradientBrush(glossRect, Color.Transparent, Color.FromArgb(gloss, BackColor), LinearGradientMode.Vertical)
            g.FillEllipse(gradientBrush, glossRect)
        End Sub

        ''' <summary>
        ''' Draws the center point.
        ''' </summary>
        ''' <paramname="g"></param>
        ''' <paramname="rect"></param>
        ''' <paramname="cX"></param>
        ''' <paramname="cY"></param>
        Private Sub DrawCenterPoint(ByVal g As Graphics, ByVal rect As Rectangle, ByVal cX As Integer, ByVal cY As Integer)
            Dim shift As Single = MyBase.Width / 5
            Dim rectangle As RectangleF = New RectangleF(cX - shift / 2, cY - shift / 2, shift, shift)
            Dim brush As LinearGradientBrush = New LinearGradientBrush(rect, Color.Black, Color.FromArgb(100, dialColorField), LinearGradientMode.Vertical)
            g.FillEllipse(brush, rectangle)
            shift = MyBase.Width / 7
            rectangle = New RectangleF(cX - shift / 2, cY - shift / 2, shift, shift)
            brush = New LinearGradientBrush(rect, Color.SlateGray, Color.Black, LinearGradientMode.ForwardDiagonal)
            g.FillEllipse(brush, rectangle)
        End Sub

        ''' <summary>
        ''' Draws the Ruler
        ''' </summary>
        ''' <paramname="g"></param>
        ''' <paramname="rect"></param>
        ''' <paramname="cX"></param>
        ''' <paramname="cY"></param>
        Private Sub DrawCalibration(ByVal g As Graphics, ByVal rect As Rectangle, ByVal cX As Integer, ByVal cY As Integer)
            Dim noOfParts = noOfDivisionsField + 1
            Dim noOfIntermediates = noOfSubDivisionsField
            Dim currentAngle = GetRadian(fromAngle)
            Dim gap As Integer = Width * 0.01F
            Dim shift As Single = Width / 25
            Dim rectangle As Rectangle = New Rectangle(rect.Left + gap, rect.Top + gap, rect.Width - gap, rect.Height - gap)
            Dim x, y, x1, y1, tx, ty, radius As Single
            radius = rectangle.Width / 2 - gap * 5
            Dim totalAngle = toAngle - fromAngle
            Dim incr = GetRadian(totalAngle / ((noOfParts - 1) * (noOfIntermediates + 1)))
            Dim thickPen As Pen = New Pen(Color.Black, MyBase.Width / 50)
            Dim thinPen As Pen = New Pen(Color.Black, MyBase.Width / 100)
            Dim rulerValue = MinValue

            For i = 0 To noOfParts
                'Draw Thick Line
                x = CSng(cX + radius * Math.Cos(currentAngle))
                y = CSng(cY + radius * Math.Sin(currentAngle))
                x1 = CSng(cX + (radius - MyBase.Width / 20) * Math.Cos(currentAngle))
                y1 = CSng(cY + (radius - MyBase.Width / 20) * Math.Sin(currentAngle))
                g.DrawLine(thickPen, x, y, x1, y1)

                'Draw Strings
                Dim format As StringFormat = New StringFormat()
                tx = CSng(cX + (radius - MyBase.Width / 10) * Math.Cos(currentAngle))
                ty = CSng(cY - shift + (radius - MyBase.Width / 10) * Math.Sin(currentAngle))
                Dim stringPen As Brush = New SolidBrush(ForeColor)
                Dim strFormat As StringFormat = New StringFormat(StringFormatFlags.NoClip)
                strFormat.Alignment = StringAlignment.Center
                Dim f As Font = New Font(Font.FontFamily, Width / 23, Font.Style)
                g.DrawString(rulerValue.ToString() & "", f, stringPen, New PointF(tx, ty), strFormat)
                rulerValue += (MaxValue - MinValue) / (noOfParts - 1)
                rulerValue = CSng(Math.Round(rulerValue, 2))

                'currentAngle += incr;
                If i = noOfParts - 1 Then Exit For

                For j = 0 To noOfIntermediates
                    'Draw thin lines 
                    currentAngle += incr
                    x = CSng(cX + radius * Math.Cos(currentAngle))
                    y = CSng(cY + radius * Math.Sin(currentAngle))
                    x1 = CSng(cX + (radius - MyBase.Width / 50) * Math.Cos(currentAngle))
                    y1 = CSng(cY + (radius - MyBase.Width / 50) * Math.Sin(currentAngle))
                    g.DrawLine(thinPen, x, y, x1, y1)
                Next
            Next
        End Sub

        ''' <summary>
        ''' Converts the given degree to radian.
        ''' </summary>
        ''' <paramname="theta"></param>
        ''' <returns></returns>
        Public Function GetRadian(ByVal theta As Single) As Single
            Return theta * CSng(Math.PI) / 180F
        End Function

        ''' <summary>
        ''' Displays the given number in the 7-Segement format.
        ''' </summary>
        ''' <paramname="g"></param>
        ''' <paramname="number"></param>
        ''' <paramname="drect"></param>
        Private Sub DisplayNumber(ByVal g As Graphics, ByVal number As Single, ByVal drect As RectangleF)
            Try
                Dim num = number.ToString("000.00")
                num.PadLeft(3, "0"c)
                Dim shift As Single = 0

                If number < 0 Then
                    shift -= width / 17
                End If

                Dim drawDPS = False
                Dim chars As Char() = num.ToCharArray()

                For i = 0 To chars.Length - 1
                    Dim c = chars(i)

                    If i < chars.Length - 1 AndAlso chars(i + 1) = "."c Then
                        drawDPS = True
                    Else
                        drawDPS = False
                    End If

                    If c <> "."c Then
                        If c = "-"c Then
                            DrawDigit(g, -1, New PointF(drect.X + shift, drect.Y), drawDPS, drect.Height)
                        Else
                            DrawDigit(g, Short.Parse(c.ToString()), New PointF(drect.X + shift, drect.Y), drawDPS, drect.Height)
                        End If

                        shift += 15 * width / 250
                    Else
                        shift += 2 * width / 250
                    End If
                Next

            Catch __unusedException1__ As Exception
            End Try
        End Sub

        ''' <summary>
        ''' Draws a digit in 7-Segement format.
        ''' </summary>
        ''' <paramname="g"></param>
        ''' <paramname="number"></param>
        ''' <paramname="position"></param>
        ''' <paramname="dp"></param>
        ''' <paramname="height"></param>
        Private Sub DrawDigit(ByVal g As Graphics, ByVal number As Integer, ByVal position As PointF, ByVal dp As Boolean, ByVal height As Single)
            Dim width As Single
            width = 10F * height / 13
            Dim outline As Pen = New Pen(Color.FromArgb(40, dialColorField))
            Dim fillPen As Pen = New Pen(Color.Black)

#Region "Form Polygon Points"
            'Segment A
            Dim segmentA = New PointF(4) {}
            segmentA(0) = CSharpImpl.__Assign(segmentA(4), New PointF(position.X + GetX(2.8F, width), position.Y + GetY(1F, height)))
            segmentA(1) = New PointF(position.X + GetX(10, width), position.Y + GetY(1F, height))
            segmentA(2) = New PointF(position.X + GetX(8.8F, width), position.Y + GetY(2F, height))
            segmentA(3) = New PointF(position.X + GetX(3.8F, width), position.Y + GetY(2F, height))

            'Segment B
            Dim segmentB = New PointF(4) {}
            segmentB(0) = CSharpImpl.__Assign(segmentB(4), New PointF(position.X + GetX(10, width), position.Y + GetY(1.4F, height)))
            segmentB(1) = New PointF(position.X + GetX(9.3F, width), position.Y + GetY(6.8F, height))
            segmentB(2) = New PointF(position.X + GetX(8.4F, width), position.Y + GetY(6.4F, height))
            segmentB(3) = New PointF(position.X + GetX(9F, width), position.Y + GetY(2.2F, height))

            'Segment C
            Dim segmentC = New PointF(4) {}
            segmentC(0) = CSharpImpl.__Assign(segmentC(4), New PointF(position.X + GetX(9.2F, width), position.Y + GetY(7.2F, height)))
            segmentC(1) = New PointF(position.X + GetX(8.7F, width), position.Y + GetY(12.7F, height))
            segmentC(2) = New PointF(position.X + GetX(7.6F, width), position.Y + GetY(11.9F, height))
            segmentC(3) = New PointF(position.X + GetX(8.2F, width), position.Y + GetY(7.7F, height))

            'Segment D
            Dim segmentD = New PointF(4) {}
            segmentD(0) = CSharpImpl.__Assign(segmentD(4), New PointF(position.X + GetX(7.4F, width), position.Y + GetY(12.1F, height)))
            segmentD(1) = New PointF(position.X + GetX(8.4F, width), position.Y + GetY(13F, height))
            segmentD(2) = New PointF(position.X + GetX(1.3F, width), position.Y + GetY(13F, height))
            segmentD(3) = New PointF(position.X + GetX(2.2F, width), position.Y + GetY(12.1F, height))

            'Segment E
            Dim segmentE = New PointF(4) {}
            segmentE(0) = CSharpImpl.__Assign(segmentE(4), New PointF(position.X + GetX(2.2F, width), position.Y + GetY(11.8F, height)))
            segmentE(1) = New PointF(position.X + GetX(1F, width), position.Y + GetY(12.7F, height))
            segmentE(2) = New PointF(position.X + GetX(1.7F, width), position.Y + GetY(7.2F, height))
            segmentE(3) = New PointF(position.X + GetX(2.8F, width), position.Y + GetY(7.7F, height))

            'Segment F
            Dim segmentF = New PointF(4) {}
            segmentF(0) = CSharpImpl.__Assign(segmentF(4), New PointF(position.X + GetX(3F, width), position.Y + GetY(6.4F, height)))
            segmentF(1) = New PointF(position.X + GetX(1.8F, width), position.Y + GetY(6.8F, height))
            segmentF(2) = New PointF(position.X + GetX(2.6F, width), position.Y + GetY(1.3F, height))
            segmentF(3) = New PointF(position.X + GetX(3.6F, width), position.Y + GetY(2.2F, height))

            'Segment G
            Dim segmentG = New PointF(6) {}
            segmentG(0) = CSharpImpl.__Assign(segmentG(6), New PointF(position.X + GetX(2F, width), position.Y + GetY(7F, height)))
            segmentG(1) = New PointF(position.X + GetX(3.1F, width), position.Y + GetY(6.5F, height))
            segmentG(2) = New PointF(position.X + GetX(8.3F, width), position.Y + GetY(6.5F, height))
            segmentG(3) = New PointF(position.X + GetX(9F, width), position.Y + GetY(7F, height))
            segmentG(4) = New PointF(position.X + GetX(8.2F, width), position.Y + GetY(7.5F, height))
            segmentG(5) = New PointF(position.X + GetX(2.9F, width), position.Y + GetY(7.5F, height))

            'Segment DP
#End Region

#Region "Draw Segments Outline"
            g.FillPolygon(outline.Brush, segmentA)
            g.FillPolygon(outline.Brush, segmentB)
            g.FillPolygon(outline.Brush, segmentC)
            g.FillPolygon(outline.Brush, segmentD)
            g.FillPolygon(outline.Brush, segmentE)
            g.FillPolygon(outline.Brush, segmentF)
            g.FillPolygon(outline.Brush, segmentG)
#End Region

#Region "Fill Segments"
            'Fill SegmentA
            If IsNumberAvailable(number, 0, 2, 3, 5, 6, 7, 8, 9) Then
                g.FillPolygon(fillPen.Brush, segmentA)
            End If

            'Fill SegmentB
            If IsNumberAvailable(number, 0, 1, 2, 3, 4, 7, 8, 9) Then
                g.FillPolygon(fillPen.Brush, segmentB)
            End If

            'Fill SegmentC
            If IsNumberAvailable(number, 0, 1, 3, 4, 5, 6, 7, 8, 9) Then
                g.FillPolygon(fillPen.Brush, segmentC)
            End If

            'Fill SegmentD
            If IsNumberAvailable(number, 0, 2, 3, 5, 6, 8, 9) Then
                g.FillPolygon(fillPen.Brush, segmentD)
            End If

            'Fill SegmentE
            If IsNumberAvailable(number, 0, 2, 6, 8) Then
                g.FillPolygon(fillPen.Brush, segmentE)
            End If

            'Fill SegmentF
            If IsNumberAvailable(number, 0, 4, 5, 6, 7, 8, 9) Then
                g.FillPolygon(fillPen.Brush, segmentF)
            End If

            'Fill SegmentG
            If IsNumberAvailable(number, 2, 3, 4, 5, 6, 8, 9, -1) Then
                g.FillPolygon(fillPen.Brush, segmentG)
            End If
#End Region

            'Draw decimal point
            If dp Then
                g.FillEllipse(fillPen.Brush, New RectangleF(position.X + GetX(10F, width), position.Y + GetY(12F, height), width / 7, width / 7))
            End If
        End Sub

        ''' <summary>
        ''' Gets Relative X for the given width to draw digit
        ''' </summary>
        ''' <paramname="x"></param>
        ''' <paramname="width"></param>
        ''' <returns></returns>
        Private Function GetX(ByVal x As Single, ByVal width As Single) As Single
            Return x * width / 12
        End Function

        ''' <summary>
        ''' Gets relative Y for the given height to draw digit
        ''' </summary>
        ''' <paramname="y"></param>
        ''' <paramname="height"></param>
        ''' <returns></returns>
        Private Function GetY(ByVal y As Single, ByVal height As Single) As Single
            Return y * height / 15
        End Function

        ''' <summary>
        ''' Returns true if a given number is available in the given list.
        ''' </summary>
        ''' <paramname="number"></param>
        ''' <paramname="listOfNumbers"></param>
        ''' <returns></returns>
        Private Function IsNumberAvailable(ByVal number As Integer, ParamArray listOfNumbers As Integer()) As Boolean
            If listOfNumbers.Length > 0 Then
                For Each i In listOfNumbers
                    If i = number Then Return True
                Next
            End If

            Return False
        End Function

        ''' <summary>
        ''' Restricts the size to make sure the height and width are always same.
        ''' </summary>
        ''' <paramname="sender"></param>
        ''' <paramname="e"></param>
        Private Sub AquaGauge_Resize(ByVal sender As Object, ByVal e As EventArgs)
            If Width < 136 Then
                Width = 136
            End If

            If oldWidth <> Width Then
                Height = Width
                oldHeight = Width
            End If

            If oldHeight <> Height Then
                Width = Height
                oldWidth = Width
            End If
        End Sub

        Private Class CSharpImpl
            <Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
            Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
                target = value
                Return value
            End Function
        End Class
#End Region
    End Class
End Namespace
