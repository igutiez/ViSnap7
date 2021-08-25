<ToolboxItem(False)>
Public Class VS7_Register
    Inherits Control
    Private _plc As Integer
    Private _dataArea As DataArea = DataArea.DB
    Private _db As Integer
    Private _byte As Integer
    Private _bit As Integer
    Private _dataType As DataType = DataType.INT
    Private _length As Integer
    Private _plcValuePrevious As String = ""
    Private _typeOfTrigger As TypeTrigger
    Private _referenceValue As String
    Private _folder As String

    Public plc_Value As String
    Public ListControls As New List(Of Control)

    Public Enum TypeTrigger
        EQUAL = 1
        LESS = 2
        GREATER = 3
        PULSE_P = 4
        PULSE_N = 5
    End Enum

    ''' <summary>
    ''' Variable when DataArea is DB and it is not a bit and not a string
    ''' </summary>
    ''' <param name="frm"></param>
    ''' <param name="plcNumber"></param>
    ''' <param name="dataType"></param>
    ''' <param name="db_Number"></param>
    ''' <param name="numByte"></param>

#Region "PLC Properties"

    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcNumberLabel)>
    Public Property PLC_Number As Integer
        Get
            Return _plc
        End Get
        Set(value As Integer)
            _plc = value
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcDataAreaLabel)>
    Public Property PLC_DataArea As DataArea
        Get
            Return _dataArea
        End Get
        Set(value As DataArea)
            _dataArea = value
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcDBLabel)>
    Public Property PLC_DB As Integer
        Get
            Return _db
        End Get
        Set(value As Integer)
            _db = value
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcByteLabel)>
    Public Property PLC_Byte As Integer
        Get
            Return _byte
        End Get
        Set(value As Integer)
            _byte = value
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcValueTypeLabel)>
    Public Property PLC_DataType As DataType
        Get
            Return _dataType
        End Get
        Set(value As DataType)
            _dataType = value

        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcBitLabel)>
    Public Property PLC_Bit As Integer
        Get
            Return _bit
        End Get
        Set(value As Integer)
            _bit = value
        End Set
    End Property
    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcLengthLabel)>
    Public Property PLC_Length As Integer
        Get
            Return _length
        End Get
        Set(value As Integer)
            _length = value
        End Set
    End Property


    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcTypeTriggerLabel)>
    Public Property PLC_TypeTrigger As TypeTrigger
        Get
            Return _typeOfTrigger
        End Get
        Set(value As TypeTrigger)
            _typeOfTrigger = value
        End Set
    End Property

    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KPlcReferenceValueLabel)>
    Public Property PLC_ReferenceValue As String
        Get
            Return _referenceValue
        End Get
        Set(value As String)
            _referenceValue = value
        End Set
    End Property

    <System.ComponentModel.Category(KPlcPropertiesCategory), System.ComponentModel.Description(KSelectFolder)>
    Public Property PLC_Folder As String
        Get
            Return _folder
        End Get
        Set(value As String)
            _folder = value
        End Set
    End Property

#End Region
#Region "Methods"
    Sub New(ByVal frm As Form, ByVal plcNumber As Integer, ByVal dataArea As General.DataArea, ByVal dataType As General.DataType, ByVal db_Number As Integer, ByVal numByte As Integer, ByVal numBit As Integer, ByVal length As Integer)
        _plc = plcNumber
        _dataArea = dataArea
        _dataType = dataType
        _db = db_Number
        _byte = numByte
        _bit = numBit
        _length = length
        frm.Controls.Add(Me)
    End Sub
    Public Sub ConfigureRegister(ByVal Folder As String, typeComparison As TypeTrigger, ByVal compareValue As Decimal)
        Me._folder = Folder
        Me._typeOfTrigger = typeComparison
        Me._referenceValue = compareValue
    End Sub
    Public Sub Add(ByVal ctr As Control)
        Me.ListControls.Add(ctr)
    End Sub
#End Region

#Region "Plc reading and writing"
    Public Sub UpdateControl(ByRef _PLC As PlcClient)
        Dim result As Boolean = False
        'Reading the  control value.
        Select Case Me.PLC_DataArea
            Case DataArea.DB
                Me.plc_Value = TakeValue(_PLC.dbData(Me.PLC_DB), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)
            Case DataArea.INPUT
                Me.plc_Value = TakeValue(_PLC.inputData(0), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)
            Case DataArea.MARK
                Me.plc_Value = TakeValue(_PLC.marksData(0), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)
            Case DataArea.OUTPUT
                Me.plc_Value = TakeValue(_PLC.outputData(0), Me.PLC_DB, Me.PLC_Byte, Me.PLC_Bit, Me.PLC_DataType, Me.PLC_Length)
            Case Else
        End Select

        If (Not firstExecution) And (_plcValuePrevious IsNot Nothing) Then

            Select Case Me.PLC_TypeTrigger
                Case TypeTrigger.EQUAL
                    result = ResultComparisonEqual(Me.plc_Value, Me.PLC_ReferenceValue, Me.PLC_DataType)
                Case TypeTrigger.GREATER
                    result = ResultComparisonGreaterThan(Me.plc_Value, Me.PLC_ReferenceValue, Me.PLC_DataType)
                Case TypeTrigger.LESS
                    result = ResultComparisonLessThan(Me.plc_Value, Me.PLC_ReferenceValue, Me.PLC_DataType)
                Case TypeTrigger.PULSE_P
                    result = (Not CBool(_plcValuePrevious)) And CBool(Me.plc_Value)
                Case TypeTrigger.PULSE_N
                    result = CBool(_plcValuePrevious) And (Not CBool(Me.plc_Value))

                Case Else
                    result = False

            End Select
            Static Dim Triggered As Boolean

            'Run only once when condition is satisfied
            If result And (Not Triggered) Then
                LogControls()

            End If
            Triggered = result
        End If




        'Save the data for next cycle
        _plcValuePrevious = plc_Value


    End Sub

    Sub LogControls()
        Dim fileName As String
        Dim _date As Date = Now()

        fileName = _date.Year.ToString + "-" + _date.Month.ToString + "-" + _date.Day.ToString


        Dim Abort As Boolean = False
        Dim path As String = ""
        Dim value As String
        Try
            If Not IO.Directory.Exists(Me.PLC_Folder) Then
                IO.Directory.CreateDirectory(Me.PLC_Folder)
            End If
        Catch ex As Exception
            Abort = True
        End Try

        If Not Abort Then
            path = Me.PLC_Folder & "\" & fileName & ".txt "
            Try
                If Not IO.File.Exists(path) Then

                    value = Now.ToLongTimeString
                    For Each ctr As Object In Me.ListControls

                        value = value & vbTab & ctr.Name

                    Next
                    Dim file As System.IO.StreamWriter
                    file = My.Computer.FileSystem.OpenTextFileWriter(path, False)
                    file.WriteLine(value)
                    file.Close()
                End If

            Catch ex As Exception
                Abort = True
            End Try

        End If
        If Not Abort Then
            value = Now.ToLongTimeString
            For Each ctr As Object In Me.ListControls

                value = value & vbTab & ctr.plc_Value
            Next

            Dim file As System.IO.StreamWriter
            file = My.Computer.FileSystem.OpenTextFileWriter(path, True)
            file.WriteLine(value)
            file.Close()


        End If
    End Sub
    Function ResultComparisonEqual(ByVal valueReal As String, ByVal valueSP As String, ByVal typeData As General.DataType) As Boolean
        Dim result As Boolean = False
        Select Case typeData
            Case DataType.BOOL
                Try
                    If CBool(valueReal) = CBool(valueSP) Then
                        result = True
                    End If
                Catch ex As Exception
                End Try

            Case DataType.UINT, DataType.SINT, DataType.INT, DataType.DINT, DataType.REAL
                Try
                    If CDec(valueReal) = CDec(valueSP) Then
                        result = True
                    End If
                Catch ex As Exception
                End Try

            Case DataType.STR, DataType.CHR
                If valueReal = valueSP Then
                    result = True
                End If

            Case Else

        End Select

        Return result
    End Function

    Function ResultComparisonGreaterThan(ByVal valueReal As String, ByVal valueSP As String, ByVal typeData As General.DataType) As Boolean
        Dim result As Boolean = False
        Select Case typeData
            Case DataType.BOOL
                Try
                    If CBool(valueReal) > CBool(valueSP) Then
                        result = True
                    End If
                Catch ex As Exception
                End Try

            Case DataType.UINT, DataType.SINT, DataType.INT, DataType.DINT, DataType.REAL
                Try
                    If CDec(valueReal) > CDec(valueSP) Then
                        result = True
                    End If
                Catch ex As Exception
                End Try

            Case DataType.STR, DataType.CHR
                If valueReal > valueSP Then
                    result = True
                End If

            Case Else

        End Select

        Return result
    End Function

    Function ResultComparisonLessThan(ByVal valueReal As String, ByVal valueSP As String, ByVal typeData As General.DataType) As Boolean
        Dim result As Boolean = False
        Select Case typeData
            Case DataType.BOOL
                Try
                    If CBool(valueReal) < CBool(valueSP) Then
                        result = True
                    End If
                Catch ex As Exception
                End Try

            Case DataType.UINT, DataType.SINT, DataType.INT, DataType.DINT, DataType.REAL
                Try
                    If CDec(valueReal) < CDec(valueSP) Then
                        result = True
                    End If
                Catch ex As Exception
                End Try

            Case DataType.STR, DataType.CHR
                If valueReal < valueSP Then
                    result = True
                End If

            Case Else

        End Select

        Return result
    End Function
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
