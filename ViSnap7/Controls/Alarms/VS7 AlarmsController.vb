Imports System.IO
Imports System.Windows.Forms.Design
<System.ComponentModel.Designer(GetType(AlarmsControllerDesigner))>
Public Class VS7_AlarmsController

#Region "PLC Properties"
    Private _PLC As Integer
    Private _DataArea As LocalDataArea = LocalDataArea.MARK
    Private _DB As Integer
    Private _Byte As Integer
    Private _Bit As Integer
    Private _Length As Integer = 1
    Private _txt As String
    Public pLC_Value As String
    Public Alarms() As VS7_Single_Alarm
    Private arrText As New ArrayList()
    Private _AlarmFileName As String = "C:\p1\alarmas.txt"

    Public Enum LocalDataArea
        MARK = 1
        DB = 2
    End Enum


    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcNumberLabel)>
    Public Property PLC_Number As Integer
        Get
            Return _PLC
        End Get
        Set(value As Integer)
            _PLC = value
            GenerateAlarms()
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcDataAreaLabel)>
    Public Property PLC_DataArea As General.DataArea
        Get
            Return _DataArea
        End Get
        Set(value As General.DataArea)
            _DataArea = value
            GenerateAlarms()
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcDBLabel)>
    Public Property PLC_DB As Integer
        Get
            Return _DB
        End Get
        Set(value As Integer)
            _DB = value
            GenerateAlarms()
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcByteLabel)>
    Public Property PLC_Byte As Integer
        Get
            Return _Byte
        End Get
        Set(value As Integer)
            _Byte = value
            GenerateAlarms()
        End Set
    End Property
    Public PLC_DataType As General.DataType = General.DataType.INT

    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcBitLabel)>
    Public Property PLC_Bit As Integer
        Get
            Return _Bit
        End Get
        Set(value As Integer)
            _Bit = value
            GenerateAlarms()
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcLengthLabel)>
    Public Property PLC_Length As Integer
        Get
            Return _Length
        End Get
        Set(value As Integer)
            _Length = value
            GenerateAlarms()
        End Set
    End Property





#End Region

#Region "Methods"
    Sub New()
        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
        Me.VerticalScroll.Enabled = True
        Me.VerticalScroll.Visible = True
    End Sub
#End Region

#Region "Plc reading and writing"
    Public Sub UpdateControl(ByRef _PLC As PlcClient)
        Dim bit As Integer

        If Not firstExecution Then
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

        If firstExecution Then

        End If
        UpdateAlarms()
    End Sub

    Private Sub GenerateAlarms()
        ReDim Alarms(Me.PLC_Length * 16 - 1)
        Me.Controls.Clear()
        InitializeComponent()
        Dim contador As Integer
        For contador = 0 To Me.PLC_Length * 16 - 1

            If IsNothing(Alarms(contador)) Then
                Dim numByte = _Byte + contador \ 8
                Dim numBit = contador Mod 8
                Alarms(contador) = New VS7_Single_Alarm(Me, PLC_Number, PLC_DataArea, General.DataType.BOOL, PLC_DB, numByte, numBit, 0)
                Alarms(contador).Name = "Alarma_" & Format(contador, "###")
                Alarms(contador).Location = New Point(0, (contador + 1) * Alarms(contador).Height)
                Alarms(contador).AckTime.Text = ""
                Alarms(contador).OpenedTime.Text = ""
                Alarms(contador).ClosedTime.Text = ""

                Me.Controls.Add(Alarms(contador))
            End If

            Alarms(contador).AlarmNumber.Text = (contador)

        Next

        Me.Size = New System.Drawing.Size(Alarms(0).Width, (UBound(Alarms) + 0) * Alarms(0).Height)

    End Sub
    Private Sub ReadAlarmsFile()
        Dim objReader As New StreamReader(Me._AlarmFileName)
        Dim sLine As String = ""
        Dim Counter As Integer = 0

        Do
            sLine = objReader.ReadLine()
            If Not sLine Is Nothing Then
                If sLine.Length > 3 Then
                    arrText.Add(Split(sLine, ":")(1))

                    If Split(sLine, ":")(0) <> Counter Then
                        MsgBox("El número de alarma no corresponde con su texto")
                    End If
                    Counter = Counter + 1
                End If
            End If

        Loop Until sLine Is Nothing
        objReader.Close()


    End Sub
    Public Sub AckAlarms()
        Dim counter As Integer

        For counter = 0 To UBound(Alarms) - 1
            Alarms(counter).Ack = True
        Next
    End Sub
    Public Sub UpdateAlarms()
        Dim Counter As Integer = 0
        Static FileReaded As Boolean

        For Each ctr As Control In Me.Controls
            If TypeOf (ctr) Is VS7_Single_Alarm Then
                With DirectCast(ctr, VS7_Single_Alarm)
                    If .alarmStatus > 0 Then
                        If Not FileReaded Then
                            ReadAlarmsFile()
                            Try

                            Catch ex As Exception
                                MsgBox("" & ex.Message)
                            End Try

                            FileReaded = True
                        End If
                        .AlarmText.Text = arrText(.AlarmNumber.Text)
                        .Visible = True
                        .Location = New Point(.Location.X, (Counter + 1) * .Height)

                        Counter = Counter + 1
                    Else

                        .Visible = False
                        .Update()
                    End If
                End With
            End If
            Me.Update()

        Next

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

#Region "Label Smart tags"

Public Class AlarmsControllerDesigner
    Inherits ControlDesigner

    Private _actionListCollection As DesignerActionListCollection

    Public Overrides ReadOnly Property ActionLists() As System.ComponentModel.Design.DesignerActionListCollection
        Get
            If _actionListCollection Is Nothing Then
                _actionListCollection = New DesignerActionListCollection()
                _actionListCollection.Add(New AlarmsControllerActionList(Me.Control))
            End If

            Return _actionListCollection
        End Get
    End Property
End Class

Friend Class AlarmsControllerActionList
    Inherits DesignerActionList

    Private ctr As VS7_AlarmsController
    Private designerActionSvc As DesignerActionUIService

    Public Sub New(ByVal component As IComponent)
        MyBase.New(component)

        ctr = DirectCast(component, VS7_AlarmsController)
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
        items.Add(New DesignerActionPropertyItem("PLC_Number", KPlcNumberLabel, KPlcAdressingCategory, KPlcTipPlcNumber))
        If PLC_DataArea = DataArea.DB Then
            items.Add(New DesignerActionPropertyItem("PLC_DB", KPlcDBLabel, KPlcAdressingCategory, KPlcTipPlcDB))
        End If
        items.Add(New DesignerActionPropertyItem("PLC_Byte", KPlcByteLabel, KPlcAdressingCategory, KPlcTipPlcByte))

        items.Add(New DesignerActionPropertyItem("PLC_Length", KPlcWordsLength, KPlcAdressingCategory, KPlcWordsLength))


        'Return the ActionItemCollection
        Return items
    End Function
End Class

#End Region
