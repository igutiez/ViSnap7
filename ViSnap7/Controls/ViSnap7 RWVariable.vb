<ToolboxItem(False)>
Public Class VS7_RWVariable
    Inherits Control

    Public pLC_Value As String
    Public controlFocused As Boolean
    Public pendingWrite As Boolean
    ''' <summary>
    ''' Variable when DataArea is DB and it is not a bit and not a string
    ''' </summary>
    ''' <param name="frm"></param>
    ''' <param name="plcNumber"></param>
    ''' <param name="dataType"></param>
    ''' <param name="db_Number"></param>
    ''' <param name="numByte"></param>


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
    Sub New(ByVal frm As Form, ByVal plcNumber As Integer, ByVal dataArea As General.DataArea, ByVal dataType As General.DataType, ByVal db_Number As Integer, ByVal numByte As Integer, ByVal numBit As Integer, ByVal length As Integer)
        _PLC = plcNumber
        _DataArea = dataArea
        _DataType = dataType
        _DB = db_Number
        _Byte = numByte
        _Bit = numBit
        _Length = length
        frm.Controls.Add(frm)
    End Sub
#End Region

#Region "Plc reading and writing"
    Public Sub UpdateControl(ByRef _PLC As PlcClient)

        'Reading if control is no pending and not write pending.
        If firstExecution Or (Not pendingWrite) Then
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
        'Write in case of pendind write
        If pendingWrite Then
            If (Me.PLC_DataType = DataType.STR) Or (Me.PLC_DataType = DataType.CHR) Or IsNumeric(Me.pLC_Value) Then
                pendingWrite = False
                If plc(PLC_Number).connected And KGlobalConnectionEnabled Then
                    WriteValue(Me.pLC_Value, Me.PLC_Number, Me.PLC_DataArea, Me.PLC_DataType, Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_Length)
                End If

            Else
                pendingWrite = False
                MsgBox(KErrorIntroducingValue)
            End If

        End If
    End Sub
    Private Sub WriteValue(_Text As String, _PLC_Number As Integer, _PLC_DataArea As DataArea, _DataType As DataType, _DB As Integer, _Byte As Integer, _Bit As Integer, _Length As Integer)
        Select Case Me.PLC_DataArea
            Case DataArea.DB
                WriteOnPlc(_Text, _PLC_Number, ViSnap7.S7AreaDB, _DataType, _DB, _Byte, _Bit, _Length)

            Case DataArea.INPUT
                WriteOnPlc(_Text, _PLC_Number, ViSnap7.S7AreaPE, _DataType, 0, _Byte, _Bit, _Length)

            Case DataArea.MARK
                WriteOnPlc(_Text, _PLC_Number, ViSnap7.S7AreaMK, _DataType, 0, _Byte, _Bit, _Length)

            Case DataArea.OUTPUT
                WriteOnPlc(_Text, _PLC_Number, ViSnap7.S7AreaPA, _DataType, 0, _Byte, _Bit, _Length)

            Case Else
        End Select

    End Sub

    Private Sub WriteOnPlc(_Text As String, _PLC_Number As Integer, _DataArea As Byte, _DataType As DataType, _DB As Integer, _Byte As Integer, _Bit As Integer, _Length As Integer)
        Select Case _DataType
            Case DataType.BOOL
                Dim Buffer(0) As Byte
                Buffer(0) = CByte(_Text)
                plc(_PLC_Number).client.WriteArea(_DataArea, _DB, _Byte * 8 + _Bit, 1, ViSnap7.S7Consts.S7WLBit, Buffer)

            Case DataType.CHR
                Dim Buffer(0) As Byte
                ViSnap7.S7.SetCharsAt(Buffer, 0, _Text)
                plc(_PLC_Number).client.WriteArea(_DataArea, _DB, _Byte, 1, ViSnap7.S7Consts.S7WLChar, Buffer)

            Case DataType.DINT
                Dim Buffer(3) As Byte
                ViSnap7.S7.SetDIntAt(Buffer, 0, _Text)
                plc(_PLC_Number).client.WriteArea(_DataArea, _DB, _Byte, 1, ViSnap7.S7Consts.S7WLDInt, Buffer)

            Case DataType.INT
                Dim Buffer(1) As Byte
                ViSnap7.S7.SetIntAt(Buffer, 0, _Text)
                plc(_PLC_Number).client.WriteArea(_DataArea, _DB, _Byte, 1, ViSnap7.S7Consts.S7WLInt, Buffer)

            Case DataType.REAL
                Dim Buffer(3) As Byte
                ViSnap7.S7.SetRealAt(Buffer, 0, _Text)
                plc(_PLC_Number).client.WriteArea(_DataArea, _DB, _Byte, 1, ViSnap7.S7Consts.S7WLReal, Buffer)

            Case DataType.SINT
                Dim Buffer(0) As Byte
                ViSnap7.S7.SetSIntAt(Buffer, 0, _Text)
                plc(_PLC_Number).client.WriteArea(_DataArea, _DB, _Byte, 1, ViSnap7.S7Consts.S7WLByte, Buffer)

            Case DataType.STR
                Dim Buffer(_Length + 1) As Byte
                ViSnap7.S7.SetStringAt(Buffer, 0, _Length, _Text)
                plc(_PLC_Number).client.DBWrite(_DB, _Byte, _Length + 2, Buffer)

            Case DataType.UINT

            Case Else

        End Select
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


    Public Sub WriteValue(ByVal value As String)
        Me.pLC_Value = value
        Me.pendingWrite = True
    End Sub
#End Region
End Class
