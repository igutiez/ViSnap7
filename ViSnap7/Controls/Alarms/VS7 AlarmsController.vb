Imports System.IO
Imports System.Windows.Forms.Design
<System.ComponentModel.Designer(GetType(AlarmsControllerDesigner))>
Public Class VS7_AlarmsController
    Public pLC_Value As String
    Public Alarms() As VS7_Single_Alarm
#Region "PLC Properties"
    Private _PLC As Integer
    Private _DataArea As LocalDataArea = LocalDataArea.MARK
    Private _DB As Integer
    Private _Byte As Integer
    Private _Bit As Integer
    Private _Length As Integer = 1
    Private _txt As String
    Private arrText() As String
    Private _ColorActive As Color = Color.Red
    Private _ColorAck As Color = Color.Yellow
    Private _ColorClosed As Color = Color.Transparent
    Private _activeLog As Boolean
    Private _folderExplorer As String
    Private _AlarmFileName As String

    Public Enum LocalDataArea
        MARK = 1
        DB = 2
    End Enum
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KFileAlarm)>
    Public Property PLC_FileExplorer As String
        Get
            Return _AlarmFileName
        End Get
        Set(value As String)
            _AlarmFileName = value
            GenerateAlarms()
        End Set
    End Property

    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KFolderLog)>
    Public Property PLC_SaveFolder As String
        Get
            Return _folderExplorer
        End Get
        Set(value As String)
            _folderExplorer = value
        End Set
    End Property


    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KLogAlarmsActive)>
    Public Property PLC_ActivateLog As Boolean
        Get
            Return _activeLog
        End Get
        Set(value As Boolean)
            _activeLog = value
        End Set
    End Property

    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcColorActive)>
    Public Property PLC_ColorActiveAlarm As Color
        Get
            Return _ColorActive
        End Get
        Set(value As Color)
            _ColorActive = value
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcColorAck)>
    Public Property PLC_ColorAckAlarm As Color
        Get
            Return _ColorAck
        End Get
        Set(value As Color)
            _ColorAck = value
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcColorClosed)>
    Public Property PLC_ColorClosedAlarm As Color
        Get
            Return _ColorClosed
        End Get
        Set(value As Color)
            _ColorClosed = value
        End Set
    End Property
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
        Me.BorderStyle = BorderStyle.FixedSingle
    End Sub
#End Region

#Region "Plc reading and writing"
    Public Sub UpdateControl(ByRef _PLC As PlcClient)

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

            UpdateAlarms()
        End If

    End Sub

    Private Sub GenerateAlarms()
        ReDim Alarms(Me.PLC_Length * 16 - 1)
        ReDim arrText(Me.PLC_Length * 16 - 1)
        Me.Controls.Clear()
        InitializeComponent()
        Dim contador As Integer
        ReadAlarmsFile()
        For contador = 0 To Me.PLC_Length * 16 - 1
            If IsNothing(Alarms(contador)) Then
                Dim numByte = _Byte + contador \ 8
                Dim numBit = contador Mod 8
                Alarms(contador) = New VS7_Single_Alarm(Me, PLC_Number, PLC_DataArea, General.DataType.BOOL, PLC_DB, numByte, numBit, 0)
                Alarms(contador).Name = "Alarma_" & Format(contador, "###")
                Alarms(contador).Width = Me.Width
                Alarms(contador).Location = New Point(0, (contador + 1) * Alarms(contador).Height)
                Alarms(contador).AckTime.Text = ""
                Alarms(contador).OpenedTime.Text = ""
                Alarms(contador).ClosedTime.Text = ""
                Alarms(contador).closedColor = Me.PLC_ColorClosedAlarm
                Alarms(contador).ackColor = Me.PLC_ColorAckAlarm
                Alarms(contador).openColor = Me.PLC_ColorActiveAlarm
                Alarms(contador).LogFolder = Me.PLC_SaveFolder
                Alarms(contador).LogActivate = Me.PLC_ActivateLog
                Me.Controls.Add(Alarms(contador))
            End If

            Alarms(contador).AlarmNumber.Text = (contador)
            Alarms(contador).AlarmText.Text = arrText(Alarms(contador).AlarmNumber.Text)

        Next
        Me.Size = New System.Drawing.Size(Alarms(0).Width, (UBound(Alarms) + 0) * Alarms(0).Height)

    End Sub
    Private Sub ReadAlarmsFile()
        Dim objReader As StreamReader
        Dim sLine As String = ""
        Dim Counter As Integer = 0
        Try
            objReader = New StreamReader(Me.PLC_FileExplorer)
            Do
                sLine = objReader.ReadLine()
                If Not sLine Is Nothing Then
                    If sLine.Length > 3 Then
                        arrText(Counter) = Split(sLine, ":")(1)
                        If Split(sLine, ":")(0) <> Counter Then
                            MsgBox("El número de alarma no corresponde con su texto")
                        End If
                        Counter = Counter + 1
                    End If
                End If

            Loop Until sLine Is Nothing
            objReader.Close()
        Catch ex As Exception

        End Try


    End Sub
    Public Sub AckAlarms()
        Dim counter As Integer
        For counter = 0 To UBound(Alarms) - 1
            Alarms(counter).ack = True
        Next
    End Sub
    Public Sub UpdateAlarms()
        Dim Counter As Integer = 0
        Static FileReaded As Boolean
        For Each ctr As Control In Me.Controls
            If TypeOf (ctr) Is VS7_Single_Alarm Then
                With DirectCast(ctr, VS7_Single_Alarm)
                    .closedColor = Me.PLC_ColorClosedAlarm
                    .ackColor = Me.PLC_ColorAckAlarm
                    .openColor = Me.PLC_ColorActiveAlarm
                    .LogFolder = Me.PLC_SaveFolder
                    .LogActivate = Me.PLC_ActivateLog

                    If .alarmStatus > 0 Then
                        If Not FileReaded Then
                            ReadAlarmsFile()
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

    Public Property PLC_ColorActiveAlarm() As Color
        Get
            Return ctr.PLC_ColorActiveAlarm
        End Get
        Set(ByVal value As Color)
            GetPropertyByName(ctr, "PLC_ColorActiveAlarm").SetValue(ctr, value)

        End Set
    End Property
    Public Property PLC_ColorAckAlarm() As Color
        Get
            Return ctr.PLC_ColorAckAlarm
        End Get
        Set(ByVal value As Color)
            GetPropertyByName(ctr, "PLC_ColorAckAlarm").SetValue(ctr, value)

        End Set
    End Property
    Public Property PLC_ColorClosedAlarm() As Color
        Get
            Return ctr.PLC_ColorClosedAlarm
        End Get
        Set(ByVal value As Color)
            GetPropertyByName(ctr, "PLC_ColorClosedAlarm").SetValue(ctr, value)

        End Set
    End Property

    Public Property PLC_ActivateLog() As Boolean
        Get
            Return ctr.PLC_ActivateLog
        End Get
        Set(ByVal value As Boolean)
            GetPropertyByName(ctr, "PLC_ActivateLog").SetValue(ctr, value)
            designerActionSvc.Refresh(ctr)

        End Set
    End Property

    Public Property PLC_SaveFolder() As String
        Get
            Return ctr.PLC_SaveFolder
        End Get
        Set(ByVal value As String)

            GetPropertyByName(ctr, "PLC_SaveFolder").SetValue(ctr, value)

        End Set
    End Property

    Public Property PLC_FileExplorer() As String
        Get
            Return ctr.PLC_FileExplorer
        End Get
        Set(ByVal value As String)
            GetPropertyByName(ctr, "PLC_FileExplorer").SetValue(ctr, value)

        End Set
    End Property
#End Region

#Region " Methods to display in the Smart-Tag panel "

    Private Sub SelectFolder()
        Dim FolderSelector As New FolderBrowserDialog
        If FolderSelector.ShowDialog = DialogResult.OK Then
            GetPropertyByName(ctr, "PLC_SaveFolder").SetValue(ctr, FolderSelector.SelectedPath)
            designerActionSvc.Refresh(ctr)

        End If
    End Sub

    Private Sub SelectFile()
        Dim fileselector As New OpenFileDialog

        If fileselector.ShowDialog = DialogResult.OK Then
            GetPropertyByName(ctr, "PLC_FileExplorer").SetValue(ctr, fileselector.FileName)
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
        items.Add(New DesignerActionPropertyItem("PLC_DataArea", KPlcValueTypeLabel, KPlcAdressingCategory, KPlcTipDataArea))
        items.Add(New DesignerActionPropertyItem("PLC_Number", KPlcNumberLabel, KPlcAdressingCategory, KPlcTipPlcNumber))
        If PLC_DataArea = DataArea.DB Then
            items.Add(New DesignerActionPropertyItem("PLC_DB", KPlcDBLabel, KPlcAdressingCategory, KPlcTipPlcDB))
        End If
        items.Add(New DesignerActionPropertyItem("PLC_Byte", KPlcByteLabel, KPlcAdressingCategory, KPlcTipPlcByte))
        items.Add(New DesignerActionPropertyItem("PLC_Length", KPlcWordsLength, KPlcAdressingCategory, KPlcWordsLength))
        items.Add(New DesignerActionPropertyItem("PLC_FileExplorer", KPlcAlarmsFile, KPlcAdressingCategory, KPlcAlarmsFile))
        items.Add(New DesignerActionMethodItem(Me, "SelectFile", KPlcSelectAlarmsFile))
        items.Add(New DesignerActionPropertyItem("PLC_ColorActiveAlarm", KPlcColorActive, KPlcAdressingCategory, KPlcColorActive))
        items.Add(New DesignerActionPropertyItem("PLC_ColorAckAlarm", KPlcColorAck, KPlcAdressingCategory, KPlcColorAck))
        items.Add(New DesignerActionPropertyItem("PLC_ColorClosedAlarm", KPlcColorClosed, KPlcAdressingCategory, KPlcColorClosed))
        items.Add(New DesignerActionPropertyItem("PLC_ActivateLog", KLogAlarmsActive, KPlcAdressingCategory, KLogAlarmsActive))
        If ctr.PLC_ActivateLog Then
            items.Add(New DesignerActionPropertyItem("PLC_SaveFolder", KSaveFolder, KPlcFolderCategory, KFolderSaveTip))
            items.Add(New DesignerActionMethodItem(Me, "SelectFolder", KSelectFolder))
        End If


        'Return the ActionItemCollection
        Return items
    End Function
End Class

#End Region
