<ToolboxItem(False)>
Public Class VS7_Single_Alarm
    Inherits UserControl
    Public pLC_Value As String = "False"
    Public alarmStatus As Integer
    Public ack As Boolean
    Public alarmAlreadyExist As Boolean
    Public openColor As Color = Color.Red
    Public ackColor As Color = Color.Yellow
    Public closedColor As Color = Color.Transparent
    Public alreadyAck As Boolean = False
    Public LogFolder As String
    Public LogActivate As Boolean

    Enum StatusAlarmType
        ACTIVE
        CLOSED
        ACK
    End Enum

#Region "PLC Properties"
    Private _PLC As Integer
    Private _DataArea As DataArea = DataArea.DB
    Private _DB As Integer
    Private _Byte As Integer
    Private _Bit As Integer
    Private _DataType As DataType = DataType.INT
    Private _Length As Integer
    Private _txt As String
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
    Public Property PLC_DataType As DataType
        Get
            Return _DataType
        End Get
        Set(value As DataType)
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
    Sub New()

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

    End Sub
    Sub New(ByVal frm As Control, ByVal plcNumber As Integer, ByVal dataArea As General.DataArea, ByVal dataType As General.DataType, ByVal db_Number As Integer, ByVal numByte As Integer, ByVal numBit As Integer, ByVal length As Integer)

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

        _PLC = plcNumber
        _DataArea = dataArea
        _DataType = dataType
        _DB = db_Number
        _Byte = numByte
        _Bit = numBit
        _Length = length
        frm.Controls.Add(Me)
    End Sub
    Public Sub UpdateControl(ByRef _PLC As PlcClient)
        If Not firstExecution Then
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
        End If

        If CBool(Me.pLC_Value) And (Not firstExecution) Then
            'Alarma activa
            If Not Me.alarmAlreadyExist Then
                alarmStatus = 1
                Me.ClosedTime.Text = ""
                Me.AckTime.Text = ""
                Me.alarmAlreadyExist = True
                Me.Size = New Size(Me.Parent.Size.Width, Me.Size.Height)
                Me.OpenedTime.Text = Format(Now, "HH:mm:ss")
                Me.Status.Text = StatusAlarmType.ACTIVE.ToString
                Me.BackColor = openColor
                If LogActivate Then
                    Dim text As String = Me.OpenedTime.Text + vbTab + Me.AlarmNumber.Text + vbTab + Me.Status.Text + vbTab + Me.AlarmText.Text + vbCrLf
                    General.WriteLogTextOnFile(Me.LogFolder, text)
                End If
            Else
                alarmStatus = 2
                'Presente y activa
                If Me.ack Then
                    alarmStatus = 3
                    Me.Status.Text = StatusAlarmType.ACK.ToString
                    Me.BackColor = ackColor

                    If Not Me.alreadyAck Then
                        Me.alreadyAck = True
                        Me.AckTime.Text = Format(Now, "HH:mm:ss")
                        If Me.LogActivate Then
                            Dim text As String = Me.AckTime.Text + vbTab + Me.AlarmNumber.Text + vbTab + Me.Status.Text + vbTab + Me.AlarmText.Text + vbCrLf
                            General.WriteLogTextOnFile(Me.LogFolder, text)
                        End If

                    End If

                Else
                    Me.alreadyAck = False

                End If
            End If
        Else

            If Me.Ack Then
                'Alarma no activa
                alarmStatus = 0
                Me.Ack = False
            End If


            Me.Status.Text = StatusAlarmType.CLOSED.ToString
            Me.BackColor = closedColor
            If Me.alarmAlreadyExist Then
                Me.ClosedTime.Text = Format(Now, "HH:mm:ss")
                If Me.LogActivate Then
                    Dim text As String = Me.ClosedTime.Text + vbTab + Me.AlarmNumber.Text + vbTab + Me.Status.Text + vbTab + Me.AlarmText.Text + vbCrLf
                    General.WriteLogTextOnFile(Me.LogFolder, text)
                End If
            End If
            Me.alarmAlreadyExist = False
        End If




    End Sub
#End Region

#Region "Plc reading and writing"




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
            Case DataType.USINT
                txt = ViSnap7.S7.GetUIntAt(_DBData.data, _PLC_Byte)
            Case Else
                txt = ""
        End Select
        Return txt
    End Function







#End Region
End Class
