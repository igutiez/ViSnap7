' |==============================================================================|
' |  ViSnap7 V1.1.0                                                              | 
' |  Programacion Siemens                                                        |
' |  August 2021                                                                 |
' |==============================================================================|
' |  ViSnap7 is a fork of Sharp7 (developed and maintained by Davide Nardella).  |
' |  You can find the original project at http//snap7.sourceforge.net/).         |
' |  ViSnap7 has been transpated from Sharp7 with some changes                   |
' |  to make it works in VB.NET only for Windows Forms applications.             |
' |  The name is a mixure of Visual + Snap7                                      |
' |                                                                              |			
' |  ViSnap7 is free software: you can redistribute it and/or modify             |
' |  it under the terms of the Lesser GNU General Public License as published by |
' |  the Free Software Foundation, either version 3 of the License, or           |
' |  (at your option) any later version.                                         |
' |                                                                              |
' |  It means that you can distribute your commercial software which includes    |
' |  ViSnap7 without the requirement to distribute the source code of your       |
' |  application and without the requirement that your application be itself     |
' |  distributed under LGPL.                                                     |
' |                                                                              |
' |  ViSnap7 is distributed in the hope that it will be useful,                  |
' |  but WITHOUT ANY WARRANTY; without even the implied warranty of              |
' |  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the               |
' |  Lesser GNU General Public License for more details.                         |
' |                                                                              |
' |  You should have received a copy of the GNU General Public License and a     |
' |  copy of Lesser GNU General Public License along with VBS7.                  |
' |  If not, see  http://www.gnu.org/licenses/                                   |
' |==============================================================================|
' History:
'  1.1.0 2021/08/07 First Release based in Sharp7 1.1.0
' 
Imports System.IO
Imports System.Net.Sockets
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading

Namespace ViSnap7




#Region "[Sync Sockets Win32/Win64 Desktop Application]"

    Friend Class MsgSocket
        Private TCPSocket As Socket
        Private _ReadTimeout As Integer = 2000
        Private _WriteTimeout As Integer = 2000
        Private _ConnectTimeout As Integer = 1000
        Public LastError As Integer = 0

        Public Sub New()
        End Sub

        Protected Overrides Sub Finalize()
            Close()
        End Sub

        Public Sub Close()
            If TCPSocket IsNot Nothing Then
                TCPSocket.Dispose()
                TCPSocket = Nothing
            End If
        End Sub

        Private Sub CreateSocket()
            TCPSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            TCPSocket.NoDelay = True
        End Sub

        Private Sub TCPPing(ByVal Host As String, ByVal Port As Integer)
            ' To Ping the PLC an Asynchronous socket is used rather then an ICMP packet.
            ' This allows the use also across Internet and Firewalls (obviously the port must be opened)           
            LastError = 0
            Dim PingSocket As Socket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)


        End Sub

        Public Function Connect(ByVal Host As String, ByVal Port As Integer) As Integer
            LastError = 0

            If Not Connected Then
                TCPPing(Host, Port)

                If LastError = 0 Then
                    Try
                        CreateSocket()
                        TCPSocket.Connect(Host, Port)
                    Catch
                        LastError = errTCPConnectionFailed
                    End Try
                End If
            End If

            Return LastError
        End Function

        Private Function WaitForData(ByVal Size As Integer, ByVal Timeout As Integer) As Integer
            Dim Expired = False
            Dim SizeAvail As Integer
            Dim Elapsed = Environment.TickCount
            LastError = 0

            Try
                SizeAvail = TCPSocket.Available

                While SizeAvail < Size AndAlso Not Expired
                    Thread.Sleep(2)
                    SizeAvail = TCPSocket.Available
                    Expired = Environment.TickCount - Elapsed > Timeout
                    ' If timeout we clean the buffer
                    If Expired AndAlso SizeAvail > 0 Then
                        Try
                            Dim Flush = New Byte(SizeAvail - 1) {}
                            TCPSocket.Receive(Flush, 0, SizeAvail, SocketFlags.None)
                        Catch
                        End Try
                    End If
                End While

            Catch
                LastError = errTCPDataReceive
            End Try

            If Expired Then
                LastError = errTCPDataReceive
            End If

            Return LastError
        End Function

        Public Function Receive(ByVal Buffer As Byte(), ByVal Start As Integer, ByVal Size As Integer) As Integer
            Dim BytesRead = 0
            LastError = WaitForData(Size, _ReadTimeout)

            If LastError = 0 Then
                Try
                    BytesRead = TCPSocket.Receive(Buffer, Start, Size, SocketFlags.None)
                Catch
                    LastError = errTCPDataReceive
                End Try

                If BytesRead = 0 Then ' Connection Reset by the peer
                    LastError = errTCPDataReceive
                    Close()
                End If
            End If

            Return LastError
        End Function

        Public Function Send(ByVal Buffer As Byte(), ByVal Size As Integer) As Integer
            LastError = 0

            Try
                Dim BytesSent = TCPSocket.Send(Buffer, Size, SocketFlags.None)
            Catch
                LastError = errTCPDataSend
                Close()
            End Try

            Return LastError
        End Function

        Public ReadOnly Property Connected As Boolean
            Get
                Return TCPSocket IsNot Nothing AndAlso TCPSocket.Connected
            End Get
        End Property

        Public Property ReadTimeout As Integer
            Get
                Return _ReadTimeout
            End Get
            Set(ByVal value As Integer)
                _ReadTimeout = value
            End Set
        End Property

        Public Property WriteTimeout As Integer
            Get
                Return _WriteTimeout
            End Get
            Set(ByVal value As Integer)
                _WriteTimeout = value
            End Set
        End Property

        Public Property ConnectTimeout As Integer
            Get
                Return _ConnectTimeout
            End Get
            Set(ByVal value As Integer)
                _ConnectTimeout = value
            End Set
        End Property
    End Class
#End Region

    Public Module S7Consts
#Region "[Exported Consts]"
        ' Error codes
        '------------------------------------------------------------------------------
        '                                     ERRORS                 
        '------------------------------------------------------------------------------
        Public Const errTCPSocketCreation As Integer = &H1
        Public Const errTCPConnectionTimeout As Integer = &H2
        Public Const errTCPConnectionFailed As Integer = &H3
        Public Const errTCPReceiveTimeout As Integer = &H4
        Public Const errTCPDataReceive As Integer = &H5
        Public Const errTCPSendTimeout As Integer = &H6
        Public Const errTCPDataSend As Integer = &H7
        Public Const errTCPConnectionReset As Integer = &H8
        Public Const errTCPNotConnected As Integer = &H9
        Public Const errTCPUnreachableHost As Integer = &H2751
        Public Const errIsoConnect As Integer = &H10000 ' Connection error
        Public Const errIsoInvalidPDU As Integer = &H30000 ' Bad format
        Public Const errIsoInvalidDataSize As Integer = &H40000 ' Bad Datasize passed to send/recv : buffer is invalid
        Public Const errCliNegotiatingPDU As Integer = &H100000
        Public Const errCliInvalidParams As Integer = &H200000
        Public Const errCliJobPending As Integer = &H300000
        Public Const errCliTooManyItems As Integer = &H400000
        Public Const errCliInvalidWordLen As Integer = &H500000
        Public Const errCliPartialDataWritten As Integer = &H600000
        Public Const errCliSizeOverPDU As Integer = &H700000
        Public Const errCliInvalidPlcAnswer As Integer = &H800000
        Public Const errCliAddressOutOfRange As Integer = &H900000
        Public Const errCliInvalidTransportSize As Integer = &HA00000
        Public Const errCliWriteDataSizeMismatch As Integer = &HB00000
        Public Const errCliItemNotAvailable As Integer = &HC00000
        Public Const errCliInvalidValue As Integer = &HD00000
        Public Const errCliCannotStartPLC As Integer = &HE00000
        Public Const errCliAlreadyRun As Integer = &HF00000
        Public Const errCliCannotStopPLC As Integer = &H1000000
        Public Const errCliCannotCopyRamToRom As Integer = &H1100000
        Public Const errCliCannotCompress As Integer = &H1200000
        Public Const errCliAlreadyStop As Integer = &H1300000
        Public Const errCliFunNotAvailable As Integer = &H1400000
        Public Const errCliUploadSequenceFailed As Integer = &H1500000
        Public Const errCliInvalidDataSizeRecvd As Integer = &H1600000
        Public Const errCliInvalidBlockType As Integer = &H1700000
        Public Const errCliInvalidBlockNumber As Integer = &H1800000
        Public Const errCliInvalidBlockSize As Integer = &H1900000
        Public Const errCliNeedPassword As Integer = &H1D00000
        Public Const errCliInvalidPassword As Integer = &H1E00000
        Public Const errCliNoPasswordToSetOrClear As Integer = &H1F00000
        Public Const errCliJobTimeout As Integer = &H2000000
        Public Const errCliPartialDataRead As Integer = &H2100000
        Public Const errCliBufferTooSmall As Integer = &H2200000
        Public Const errCliFunctionRefused As Integer = &H2300000
        Public Const errCliDestroying As Integer = &H2400000
        Public Const errCliInvalidParamNumber As Integer = &H2500000
        Public Const errCliCannotChangeParam As Integer = &H2600000
        Public Const errCliFunctionNotImplemented As Integer = &H2700000
        '------------------------------------------------------------------------------
        '        PARAMS LIST FOR COMPATIBILITY WITH Snap7.net.cs           
        '------------------------------------------------------------------------------
        Public Const p_u16_LocalPort As Integer = 1  ' Not applicable here
        Public Const p_u16_RemotePort As Integer = 2
        Public Const p_i32_PingTimeout As Integer = 3
        Public Const p_i32_SendTimeout As Integer = 4
        Public Const p_i32_RecvTimeout As Integer = 5
        Public Const p_i32_WorkInterval As Integer = 6  ' Not applicable here
        Public Const p_u16_SrcRef As Integer = 7  ' Not applicable here
        Public Const p_u16_DstRef As Integer = 8  ' Not applicable here
        Public Const p_u16_SrcTSap As Integer = 9  ' Not applicable here
        Public Const p_i32_PDURequest As Integer = 10
        Public Const p_i32_MaxClients As Integer = 11 ' Not applicable here
        Public Const p_i32_BSendTimeout As Integer = 12 ' Not applicable here
        Public Const p_i32_BRecvTimeout As Integer = 13 ' Not applicable here
        Public Const p_u32_RecoveryTime As Integer = 14 ' Not applicable here
        Public Const p_u32_KeepAliveTime As Integer = 15 ' Not applicable here
        ' Area ID
        Public Const S7AreaPE As Byte = &H81
        Public Const S7AreaPA As Byte = &H82
        Public Const S7AreaMK As Byte = &H83
        Public Const S7AreaDB As Byte = &H84
        Public Const S7AreaCT As Byte = &H1C
        Public Const S7AreaTM As Byte = &H1D
        ' Word Length
        Public Const S7WLBit As Integer = &H1
        Public Const S7WLByte As Integer = &H2
        Public Const S7WLChar As Integer = &H3
        Public Const S7WLWord As Integer = &H4
        Public Const S7WLInt As Integer = &H5
        Public Const S7WLDWord As Integer = &H6
        Public Const S7WLDInt As Integer = &H7
        Public Const S7WLReal As Integer = &H8
        Public Const S7WLCounter As Integer = &H1C
        Public Const S7WLTimer As Integer = &H1D
        ' PLC Status
        Public Const S7CpuStatusUnknown As Integer = &H0
        Public Const S7CpuStatusRun As Integer = &H8
        Public Const S7CpuStatusStop As Integer = &H4

        <StructLayout(LayoutKind.Sequential, Pack:=1)>
        Public Structure S7Tag
            Public Area As Integer
            Public DBNumber As Integer
            Public Start As Integer
            Public Elements As Integer
            Public WordLen As Integer
        End Structure
#End Region
    End Module

    Public Class S7Timer
#Region "S7Timer"
        Private ptField As TimeSpan
        Private etField As TimeSpan
        Private input As Boolean = False
        Private qField As Boolean = False

        Public Sub New(ByVal buff As Byte(), ByVal position As Integer)
            If position + 12 < buff.Length Then
                Return
            Else
                SetTimer(New List(Of Byte)(buff).GetRange(position, 16).ToArray())
            End If
        End Sub

        Public Sub New(ByVal buff As Byte())
            SetTimer(buff)
        End Sub

        Private Sub SetTimer(ByVal buff As Byte())
            If buff.Length <> 12 Then
                ptField = New TimeSpan(0)
                etField = New TimeSpan(0)
            Else
                Dim resPT As Integer
                resPT = buff(0)
                resPT <<= 8
                resPT += buff(1)
                resPT <<= 8
                resPT += buff(2)
                resPT <<= 8
                resPT += buff(3)
                ptField = New TimeSpan(0, 0, 0, 0, resPT)
                Dim resET As Integer
                resET = buff(4)
                resET <<= 8
                resET += buff(5)
                resET <<= 8
                resET += buff(6)
                resET <<= 8
                resET += buff(7)
                etField = New TimeSpan(0, 0, 0, 0, resET)
                input = (buff(8) And &H1) = &H1
                qField = (buff(8) And &H2) = &H2
            End If
        End Sub

        Public ReadOnly Property PT As TimeSpan
            Get
                Return ptField
            End Get
        End Property

        Public ReadOnly Property ET As TimeSpan
            Get
                Return etField
            End Get
        End Property

        Public ReadOnly Property [IN] As Boolean
            Get
                Return input
            End Get
        End Property

        Public ReadOnly Property Q As Boolean
            Get
                Return qField
            End Get
        End Property
#End Region
    End Class
    Public Module S7
#Region "[Help Functions]"

        Private bias As Long = 621355968000000000 ' "decimicros" between 0001-01-01 00:00:00 and 1970-01-01 00:00:00

        Private Function BCDtoByte(ByVal B As Byte) As Integer
            Return (B >> 4) * 10 + (B And &HF)
        End Function

        Private Function ByteToBCD(ByVal Value As Integer) As Byte
            Return Value / 10 << 4 Or Value Mod 10
        End Function

        Private Function CopyFrom(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Size As Integer) As Byte()
            Dim Result = New Byte(Size - 1) {}
            Array.Copy(Buffer, Pos, Result, 0, Size)
            Return Result
        End Function

        Public Function DataSizeByte(ByVal WordLength As Integer) As Integer
            Select Case WordLength
                Case S7Consts.S7WLBit
                    Return 1  ' S7 sends 1 byte per bit
                Case S7Consts.S7WLByte
                    Return 1
                Case S7Consts.S7WLChar
                    Return 1
                Case S7Consts.S7WLWord
                    Return 2
                Case S7Consts.S7WLDWord
                    Return 4
                Case S7Consts.S7WLInt
                    Return 2
                Case S7Consts.S7WLDInt
                    Return 4
                Case S7Consts.S7WLReal
                    Return 4
                Case S7WLCounter
                    Return 2
                Case S7WLTimer
                    Return 2
                Case Else
                    Return 0
            End Select
        End Function

#Region "Get/Set the bit at Pos.Bit"
        Public Function GetBitAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Bit As Integer) As Boolean
            Dim Mask As Byte() = {&H1, &H2, &H4, &H8, &H10, &H20, &H40, &H80}
            If Bit < 0 Then Bit = 0
            If Bit > 7 Then Bit = 7
            Return (Buffer(Pos) And Mask(Bit)) <> 0
        End Function

        Public Sub SetBitAt(ByRef Buffer As Byte(), ByVal Pos As Integer, ByVal Bit As Integer, ByVal Value As Boolean)
            Dim Mask As Byte() = {&H1, &H2, &H4, &H8, &H10, &H20, &H40, &H80}
            If Bit < 0 Then Bit = 0
            If Bit > 7 Then Bit = 7

            If Value Then
                Buffer(Pos) = Buffer(Pos) Or Mask(Bit)
            Else
                Buffer(Pos) = Buffer(Pos) And Not Mask(Bit)
            End If
        End Sub
#End Region

#Region "Get/Set 8 bit signed value (S7 SInt) -128..127"
        Public Function GetSIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As Integer
            Dim Value As Integer = Buffer(Pos)

            If Value < 128 Then
                Return Value
            Else
                Return Value - 256
            End If
        End Function

        Public Sub SetSIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As Integer)
            If Value < -128 Then Value = -128
            If Value > 127 Then Value = 127
            Buffer(Pos) = CByte(Value)
        End Sub
#End Region







#Region "Get/Set 16 bit signed value (S7 int) -32768..32767"
        Public Function GetIntAt(Buffer As Byte(), Pos As Integer) As Integer
            Dim valor(1) As Byte
            valor(0) = Buffer(Pos + 1)
            valor(1) = Buffer(Pos)
            Return BitConverter.ToInt16(valor, 0)
            'Return CShort((Buffer(Pos) << 8) Or Buffer(Pos + 1))
        End Function
        Public Sub SetIntAt(Buffer As Byte(), Pos As Integer, Value As Int16)
            Dim Temporal() As Byte = BitConverter.GetBytes(Value)
            Buffer(Pos) = Temporal(1) 'CByte(Value >> 8)
            Buffer(Pos + 1) = Temporal(0) 'CByte(Value And &HFF)
        End Sub
#End Region

#Region "Get/Set 32 bit signed value (S7 DInt) -2147483648..2147483647"

        Public Function GetDIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As Integer
            Dim Result As Integer
            Result = Buffer(Pos)
            Result <<= 8
            Result += Buffer(Pos + 1)
            Result <<= 8
            Result += Buffer(Pos + 2)
            Result <<= 8
            Result += Buffer(Pos + 3)
            Return Result
        End Function
        Public Sub SetDIntAt(Buffer As Byte(), Pos As Integer, Value As Integer)
            Buffer(Pos + 3) = CByte(Value And &HFF)
            Buffer(Pos + 2) = CByte((Value >> 8) And &HFF)
            Buffer(Pos + 1) = CByte((Value >> 16) And &HFF)
            Buffer(Pos) = CByte((Value >> 24) And &HFF)
        End Sub
#End Region
#Region "Get/Set 64 bit signed value (S7 LInt) -9223372036854775808..9223372036854775807"
        Public Function GetLIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As Long
            Dim Result As Long
            Result = Buffer(Pos)
            Result <<= 8
            Result += Buffer(Pos + 1)
            Result <<= 8
            Result += Buffer(Pos + 2)
            Result <<= 8
            Result += Buffer(Pos + 3)
            Result <<= 8
            Result += Buffer(Pos + 4)
            Result <<= 8
            Result += Buffer(Pos + 5)
            Result <<= 8
            Result += Buffer(Pos + 6)
            Result <<= 8
            Result += Buffer(Pos + 7)
            Return Result
        End Function

        Public Sub SetLIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As Long)
            Buffer(Pos + 7) = CByte(Value And &HFF)
            Buffer(Pos + 6) = CByte(Value >> 8 And &HFF)
            Buffer(Pos + 5) = CByte(Value >> 16 And &HFF)
            Buffer(Pos + 4) = CByte(Value >> 24 And &HFF)
            Buffer(Pos + 3) = CByte(Value >> 32 And &HFF)
            Buffer(Pos + 2) = CByte(Value >> 40 And &HFF)
            Buffer(Pos + 1) = CByte(Value >> 48 And &HFF)
            Buffer(Pos) = CByte(Value >> 56 And &HFF)
        End Sub
#End Region

#Region "Get/Set 8 bit unsigned value (S7 USInt) 0..255"
        Public Function GetUSIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As Byte
            Return Buffer(Pos)
        End Function

        Public Sub SetUSIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As Byte)
            Buffer(Pos) = Value
        End Sub
#End Region

#Region "Get/Set 16 bit unsigned value (S7 UInt) 0..65535"
        Public Function GetUIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As UShort
            Return Buffer(Pos) << 8 Or Buffer(Pos + 1)
        End Function

        Public Sub SetUIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As UShort)
            Buffer(Pos) = CByte(Value >> 8)
            Buffer(Pos + 1) = CByte(Value And &HFF)
        End Sub
#End Region

#Region "Get/Set 32 bit unsigned value (S7 UDInt) 0..4294967296"
        Public Function GetUDIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As UInteger
            Dim Result As UInteger
            Result = Buffer(Pos)
            Result <<= 8
            Result = Result Or Buffer(Pos + 1)
            Result <<= 8
            Result = Result Or Buffer(Pos + 2)
            Result <<= 8
            Result = Result Or Buffer(Pos + 3)
            Return Result
        End Function

        Public Sub SetUDIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As UInteger)
            Buffer(Pos + 3) = CByte(Value And &HFF)
            Buffer(Pos + 2) = CByte(Value >> 8 And &HFF)
            Buffer(Pos + 1) = CByte(Value >> 16 And &HFF)
            Buffer(Pos) = CByte(Value >> 24 And &HFF)
        End Sub
#End Region

#Region "Get/Set 64 bit unsigned value (S7 ULint) 0..18446744073709551616"
        Public Function GetULIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As ULong
            Dim Result As ULong
            Result = Buffer(Pos)
            Result <<= 8
            Result = Result Or Buffer(Pos + 1)
            Result <<= 8
            Result = Result Or Buffer(Pos + 2)
            Result <<= 8
            Result = Result Or Buffer(Pos + 3)
            Result <<= 8
            Result = Result Or Buffer(Pos + 4)
            Result <<= 8
            Result = Result Or Buffer(Pos + 5)
            Result <<= 8
            Result = Result Or Buffer(Pos + 6)
            Result <<= 8
            Result = Result Or Buffer(Pos + 7)
            Return Result
        End Function

        Public Sub SetULintAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As ULong)
            Buffer(Pos + 7) = CByte(Value And &HFF)
            Buffer(Pos + 6) = CByte(Value >> 8 And &HFF)
            Buffer(Pos + 5) = CByte(Value >> 16 And &HFF)
            Buffer(Pos + 4) = CByte(Value >> 24 And &HFF)
            Buffer(Pos + 3) = CByte(Value >> 32 And &HFF)
            Buffer(Pos + 2) = CByte(Value >> 40 And &HFF)
            Buffer(Pos + 1) = CByte(Value >> 48 And &HFF)
            Buffer(Pos) = CByte(Value >> 56 And &HFF)
        End Sub
#End Region

#Region "Get/Set 8 bit word (S7 Byte) 16#00..16#FF"
        Public Function GetByteAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As Byte
            Return Buffer(Pos)
        End Function

        Public Sub SetByteAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As Byte)
            Buffer(Pos) = Value
        End Sub
#End Region

#Region "Get/Set 16 bit word (S7 Word) 16#0000..16#FFFF"
        Public Function GetWordAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As UShort
            Return GetUIntAt(Buffer, Pos)
        End Function

        Public Sub SetWordAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As UShort)
            SetUIntAt(Buffer, Pos, Value)
        End Sub
#End Region

#Region "Get/Set 32 bit word (S7 DWord) 16#00000000..16#FFFFFFFF"
        Public Function GetDWordAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As UInteger
            Return GetUDIntAt(Buffer, Pos)
        End Function

        Public Sub SetDWordAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As UInteger)
            SetUDIntAt(Buffer, Pos, Value)
        End Sub
#End Region

#Region "Get/Set 64 bit word (S7 LWord) 16#0000000000000000..16#FFFFFFFFFFFFFFFF"
        Public Function GetLWordAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As ULong
            Return GetULIntAt(Buffer, Pos)
        End Function

        Public Sub SetLWordAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As ULong)
            SetULintAt(Buffer, Pos, Value)
        End Sub
#End Region


#Region "Get/Set 32 bit floating point number (S7 Real) (Range of Single)"
        Public Function GetRealAt(Buffer As Byte(), Pos As Integer) As Single
            Dim Value As UInt32 = GetUDIntAt(Buffer, Pos)
            Dim bytes As Byte() = BitConverter.GetBytes(Value)
            Return BitConverter.ToSingle(bytes, 0)
        End Function
        Public Sub SetRealAt(Buffer As Byte(), Pos As Integer, Value As Single)
            Dim FloatArray As Byte() = BitConverter.GetBytes(Value)
            Buffer(Pos) = FloatArray(3)
            Buffer(Pos + 1) = FloatArray(2)
            Buffer(Pos + 2) = FloatArray(1)
            Buffer(Pos + 3) = FloatArray(0)
        End Sub
#End Region
#Region "Get/Set 64 bit floating point number (S7 LReal) (Range of Double)"
        Public Function GetLRealAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As Double
            Dim Value = GetULIntAt(Buffer, Pos)
            Dim bytes = BitConverter.GetBytes(Value)
            Return BitConverter.ToDouble(bytes, 0)
        End Function

        Public Sub SetLRealAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As Double)
            Dim FloatArray = BitConverter.GetBytes(Value)
            Buffer(Pos) = FloatArray(7)
            Buffer(Pos + 1) = FloatArray(6)
            Buffer(Pos + 2) = FloatArray(5)
            Buffer(Pos + 3) = FloatArray(4)
            Buffer(Pos + 4) = FloatArray(3)
            Buffer(Pos + 5) = FloatArray(2)
            Buffer(Pos + 6) = FloatArray(1)
            Buffer(Pos + 7) = FloatArray(0)
        End Sub
#End Region

#Region "Get/Set DateTime (S7 DATE_AND_TIME)"
        Public Function GetDateTimeAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As Date
            Dim Year, Month, Day, Hour, Min, Sec, MSec As Integer
            Year = BCDtoByte(Buffer(Pos))

            If Year < 90 Then
                Year += 2000
            Else
                Year += 1900
            End If

            Month = BCDtoByte(Buffer(Pos + 1))
            Day = BCDtoByte(Buffer(Pos + 2))
            Hour = BCDtoByte(Buffer(Pos + 3))
            Min = BCDtoByte(Buffer(Pos + 4))
            Sec = BCDtoByte(Buffer(Pos + 5))
            MSec = BCDtoByte(Buffer(Pos + 6)) * 10 + BCDtoByte(Buffer(Pos + 7)) / 10

            Try
                Return New DateTime(Year, Month, Day, Hour, Min, Sec, MSec)
            Catch __unusedArgumentOutOfRangeException1__ As ArgumentOutOfRangeException
                Return New DateTime(0)
            End Try
        End Function

        Public Sub SetDateTimeAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As Date)
            Dim Year = Value.Year
            Dim Month = Value.Month
            Dim Day = Value.Day
            Dim Hour = Value.Hour
            Dim Min = Value.Minute
            Dim Sec = Value.Second
            Dim Dow = Value.DayOfWeek + 1
            ' MSecH = First two digits of miliseconds 
            Dim MsecH As Integer = Value.Millisecond / 10
            ' MSecL = Last digit of miliseconds
            Dim MsecL = Value.Millisecond Mod 10
            If Year > 1999 Then Year -= 2000
            Buffer(Pos) = ByteToBCD(Year)
            Buffer(Pos + 1) = ByteToBCD(Month)
            Buffer(Pos + 2) = ByteToBCD(Day)
            Buffer(Pos + 3) = ByteToBCD(Hour)
            Buffer(Pos + 4) = ByteToBCD(Min)
            Buffer(Pos + 5) = ByteToBCD(Sec)
            Buffer(Pos + 6) = ByteToBCD(MsecH)
            Buffer(Pos + 7) = ByteToBCD(MsecL * 10 + Dow)
        End Sub
#End Region

#Region "Get/Set DATE (S7 DATE) "
        Public Function GetDateAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As Date
            Try
                Return New DateTime(1990, 1, 1).AddDays(GetIntAt(Buffer, Pos))
            Catch __unusedArgumentOutOfRangeException1__ As ArgumentOutOfRangeException
                Return New DateTime(0)
            End Try
        End Function

        Public Sub SetDateAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As Date)
            Call SetIntAt(Buffer, Pos, CShort((Value - New DateTime(1990, 1, 1)).Days))
        End Sub

#End Region

#Region "Get/Set TOD (S7 TIME_OF_DAY)"
        Public Function GetTODAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As Date
            Try
                Return New DateTime(0).AddMilliseconds(GetDIntAt(Buffer, Pos))
            Catch __unusedArgumentOutOfRangeException1__ As ArgumentOutOfRangeException
                Return New DateTime(0)
            End Try
        End Function

        Public Sub SetTODAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As Date)
            Dim Time = Value.TimeOfDay
            SetDIntAt(Buffer, Pos, Math.Round(Time.TotalMilliseconds))
        End Sub
#End Region

#Region "Get/Set LTOD (S7 1500 LONG TIME_OF_DAY)"
        Public Function GetLTODAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As Date
            ' .NET Tick = 100 ns, S71500 Tick = 1 ns
            Try
                Return New DateTime(Math.Abs(GetLIntAt(Buffer, Pos) / 100))
            Catch __unusedArgumentOutOfRangeException1__ As ArgumentOutOfRangeException
                Return New DateTime(0)
            End Try
        End Function

        Public Sub SetLTODAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As Date)
            Dim Time = Value.TimeOfDay
            SetLIntAt(Buffer, Pos, Time.Ticks * 100)
        End Sub
#End Region

#Region "GET/SET LDT (S7 1500 Long Date and Time)"
        Public Function GetLDTAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As Date
            Try
                Return New DateTime(GetLIntAt(Buffer, Pos) / 100 + bias)
            Catch __unusedArgumentOutOfRangeException1__ As ArgumentOutOfRangeException
                Return New DateTime(0)
            End Try
        End Function

        Public Sub SetLDTAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As Date)
            SetLIntAt(Buffer, Pos, (Value.Ticks - bias) * 100)
        End Sub
#End Region

#Region "Get/Set DTL (S71200/1500 Date and Time)"
        ' Thanks to Johan Cardoen for GetDTLAt
        Public Function GetDTLAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As Date
            Dim Year, Month, Day, Hour, Min, Sec, MSec As Integer
            Year = Buffer(Pos) * 256 + Buffer(Pos + 1)
            Month = Buffer(Pos + 2)
            Day = Buffer(Pos + 3)
            Hour = Buffer(Pos + 5)
            Min = Buffer(Pos + 6)
            Sec = Buffer(Pos + 7)
            MSec = GetUDIntAt(Buffer, Pos + 8) / 1000000

            Try
                Return New DateTime(Year, Month, Day, Hour, Min, Sec, MSec)
            Catch __unusedArgumentOutOfRangeException1__ As ArgumentOutOfRangeException
                Return New DateTime(0)
            End Try
        End Function

        Public Sub SetDTLAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As Date)
            Dim Year As Short = Value.Year
            Dim Month As Byte = Value.Month
            Dim Day As Byte = Value.Day
            Dim Hour As Byte = Value.Hour
            Dim Min As Byte = Value.Minute
            Dim Sec As Byte = Value.Second
            Dim Dow As Byte = Value.DayOfWeek + 1
            Dim NanoSecs = Value.Millisecond * 1000000
            Dim bytes_short = BitConverter.GetBytes(Year)
            Buffer(Pos) = bytes_short(1)
            Buffer(Pos + 1) = bytes_short(0)
            Buffer(Pos + 2) = Month
            Buffer(Pos + 3) = Day
            Buffer(Pos + 4) = Dow
            Buffer(Pos + 5) = Hour
            Buffer(Pos + 6) = Min
            Buffer(Pos + 7) = Sec
            SetDIntAt(Buffer, Pos + 8, NanoSecs)
        End Sub

#End Region

#Region "Get/Set String (S7 String)"
        ' Thanks to Pablo Agirre 
        Public Function GetStringAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As String
            Dim size As Integer = Buffer(Pos + 1)
            Return Encoding.UTF8.GetString(Buffer, Pos + 2, size)
        End Function

        Public Sub SetStringAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal MaxLen As Integer, ByVal Value As String)
            Dim size = Value.Length
            Buffer(Pos) = CByte(MaxLen)
            Buffer(Pos + 1) = CByte(size)
            Encoding.UTF8.GetBytes(Value, 0, size, Buffer, Pos + 2)
        End Sub

#End Region

#Region "Get/Set WString (S7-1500 String)"
        Public Function GetWStringAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As String
            'WString size = n characters + 2 Words (first for max length, second for real length)
            'Get the real length in Words
            Dim size As Integer = GetIntAt(Buffer, Pos + 2)
            'Extract string in UTF-16 unicode Big Endian.
            Return Encoding.BigEndianUnicode.GetString(Buffer, Pos + 4, size * 2)
        End Function

        Public Sub SetWStringAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal MaxCharNb As Integer, ByVal Value As String)
            'Get the length in words from number of characters
            Dim size = Value.Length
            'Set the Max length in Words 
            SetIntAt(Buffer, Pos, MaxCharNb)
            'Set the real length in words
            SetIntAt(Buffer, Pos + 2, size)
            'Set the UTF-16 unicode Big endian String (after max length and length)
            Encoding.BigEndianUnicode.GetBytes(Value, 0, size, Buffer, Pos + 4)
        End Sub

#End Region

#Region "Get/Set Array of char (S7 ARRAY OF CHARS)"
        Public Function GetCharsAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Size As Integer) As String
            Return Encoding.UTF8.GetString(Buffer, Pos, Size)
        End Function

        Public Sub SetCharsAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As String)
            Dim MaxLen = Buffer.Length - Pos
            ' Truncs the string if there's no room enough        
            If MaxLen > Value.Length Then MaxLen = Value.Length
            Encoding.UTF8.GetBytes(Value, 0, MaxLen, Buffer, Pos)
        End Sub

#End Region

#Region "Get/Set Array of WChar (S7-1500 ARRAY OF CHARS)"

        Public Function GetWCharsAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal SizeInCharNb As Integer) As String
            'Extract Unicode UTF-16 Big-Endian character from the buffer. To use with WChar Datatype.
            'Size to read is in byte. Be careful, 1 char = 2 bytes
            Return Encoding.BigEndianUnicode.GetString(Buffer, Pos, SizeInCharNb * 2)
        End Function

        Public Sub SetWCharsAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As String)
            'Compute Max length in char number
            Dim MaxLen As Integer = (Buffer.Length - Pos) / 2
            ' Truncs the string if there's no room enough        
            If MaxLen > Value.Length Then MaxLen = Value.Length
            Encoding.BigEndianUnicode.GetBytes(Value, 0, MaxLen, Buffer, Pos)
        End Sub

#End Region

#Region "Get/Set Counter"
        Public Function GetCounter(ByVal Value As UShort) As Integer
            Return BCDtoByte(Value) * 100 + BCDtoByte(Value >> 8)
        End Function

        Public Function GetCounterAt(ByVal Buffer As UShort(), ByVal Index As Integer) As Integer
            Return GetCounter(Buffer(Index))
        End Function

        Public Function ToCounter(ByVal Value As Integer) As UShort
            Return ByteToBCD(Value / 100) + (ByteToBCD(Value Mod 100) << 8)
        End Function

        Public Sub SetCounterAt(ByVal Buffer As UShort(), ByVal Pos As Integer, ByVal Value As Integer)
            Buffer(Pos) = ToCounter(Value)
        End Sub
#End Region

#Region "Get/Set Timer"

        Public Function GetS7TimerAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As S7Timer
            Return New S7Timer(New List(Of Byte)(Buffer).GetRange(Pos, 12).ToArray())
        End Function

        Public Sub SetS7TimespanAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As TimeSpan)
            SetDIntAt(Buffer, Pos, Value.TotalMilliseconds)
        End Sub

        Public Function GetS7TimespanAt(ByVal Buffer As Byte(), ByVal pos As Integer) As TimeSpan
            If Buffer.Length < pos + 4 Then
                Return New TimeSpan()
            End If

            Dim a As Integer
            a = Buffer(pos + 0)
            a <<= 8
            a += Buffer(pos + 1)
            a <<= 8
            a += Buffer(pos + 2)
            a <<= 8
            a += Buffer(pos + 3)
            Dim sp As TimeSpan = New TimeSpan(0, 0, 0, 0, a)
            Return sp
        End Function

        Public Function GetLTimeAt(ByVal Buffer As Byte(), ByVal pos As Integer) As TimeSpan
            'LTime size : 64 bits (8 octets)
            'Case if the buffer is too small
            If Buffer.Length < pos + 8 Then Return New TimeSpan()

            Try
                ' Extract and Convert number of nanoseconds to tick (1 tick = 100 nanoseconds)
                Return TimeSpan.FromTicks(GetLIntAt(Buffer, pos) / 100)
            Catch __unusedException1__ As Exception
                Return New TimeSpan()
            End Try
        End Function

        Public Sub SetLTimeAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As TimeSpan)
            SetLIntAt(Buffer, Pos, Value.Ticks * 100)
        End Sub

#End Region

#End Region
    End Module



    Public Class S7MultiVar
#Region "[MultiRead/Write Helper]"
        Private FClient As S7Client
        Private [Handles] As GCHandle() = New GCHandle(S7Client.MaxVars - 1) {}
        Private Count As Integer = 0
        Private Items As S7Client.S7DataItem() = New S7Client.S7DataItem(S7Client.MaxVars - 1) {}
        Public Results As Integer() = New Integer(S7Client.MaxVars - 1) {}

        Private Function AdjustWordLength(ByVal Area As Integer, ByRef WordLen As Integer, ByRef Amount As Integer, ByRef Start As Integer) As Boolean
            ' Calc Word size          
            Dim WordSize = S7.DataSizeByte(WordLen)
            If WordSize = 0 Then Return False
            If Area = S7AreaCT Then WordLen = S7WLCounter
            If Area = S7AreaTM Then WordLen = S7WLTimer

            If WordLen = S7Consts.S7WLBit Then
                Amount = 1  ' Only 1 bit can be transferred at time
            Else

                If WordLen <> S7WLCounter AndAlso WordLen <> S7WLTimer Then
                    Amount = Amount * WordSize
                    Start = Start * 8
                    WordLen = S7Consts.S7WLByte
                End If
            End If

            Return True
        End Function

        Public Sub New(ByVal Client As S7Client)
            FClient = Client

            For c = 0 To S7Client.MaxVars - 1
                Results(c) = errCliItemNotAvailable
            Next
        End Sub

        Protected Overrides Sub Finalize()
            Clear()
        End Sub

        Public Function Add(Of T)(ByVal Tag As S7Tag, ByRef Buffer As T(), ByVal Offset As Integer) As Boolean
            Return Add(Tag.Area, Tag.WordLen, Tag.DBNumber, Tag.Start, Tag.Elements, Buffer, Offset)
        End Function

        Public Function Add(Of T)(ByVal Tag As S7Tag, ByRef Buffer As T()) As Boolean
            Return Add(Tag.Area, Tag.WordLen, Tag.DBNumber, Tag.Start, Tag.Elements, Buffer)
        End Function

        Public Function Add(Of T)(ByVal Area As Integer, ByVal WordLen As Integer, ByVal DBNumber As Integer, ByVal Start As Integer, ByVal Amount As Integer, ByRef Buffer As T()) As Boolean
            Return Add(Area, WordLen, DBNumber, Start, Amount, Buffer, 0)
        End Function

        Public Function Add(Of T)(ByVal Area As Integer, ByVal WordLen As Integer, ByVal DBNumber As Integer, ByVal Start As Integer, ByVal Amount As Integer, ByRef Buffer As T(), ByVal Offset As Integer) As Boolean
            If Count < S7Client.MaxVars Then
                If AdjustWordLength(Area, WordLen, Amount, Start) Then
                    Items(Count).Area = Area
                    Items(Count).WordLen = WordLen
                    Items(Count).Result = errCliItemNotAvailable
                    Items(Count).DBNumber = DBNumber
                    Items(Count).Start = Start
                    Items(Count).Amount = Amount
                    Dim handle = GCHandle.Alloc(Buffer, GCHandleType.Pinned)
#If WINDOWS_UWP Or NETFX_CORE Then
					if (IntPtr.Size == 4)
						Items[Count].pData = (IntPtr)(handle.AddrOfPinnedObject().ToInt32() + Offset * Marshal.SizeOf<T>());
					else
						Items[Count].pData = (IntPtr)(handle.AddrOfPinnedObject().ToInt64() + Offset * Marshal.SizeOf<T>());
#Else
                    If IntPtr.Size = 4 Then
                        Items(Count).pData = CType((handle.AddrOfPinnedObject().ToInt32() + Offset * Marshal.SizeOf(GetType(T))), IntPtr)
                    Else
                        Items(Count).pData = CType((handle.AddrOfPinnedObject().ToInt64() + Offset * Marshal.SizeOf(GetType(T))), IntPtr)
                    End If
#End If
                    [Handles](Count) = handle
                    Count += 1
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        End Function

        Public Function Read() As Integer
            Dim FunctionResult As Integer
            Dim GlobalResult = errCliFunctionRefused

            Try

                If Count > 0 Then
                    FunctionResult = FClient.ReadMultiVars(Items, Count)

                    If FunctionResult = 0 Then
                        For c = 0 To S7Client.MaxVars - 1
                            Results(c) = Items(c).Result
                        Next
                    End If

                    GlobalResult = FunctionResult
                Else
                    GlobalResult = errCliFunctionRefused
                End If

            Finally
                Clear() ' handles are no more needed and MUST be freed
            End Try

            Return GlobalResult
        End Function

        Public Function Write() As Integer
            Dim FunctionResult As Integer
            Dim GlobalResult = errCliFunctionRefused

            Try

                If Count > 0 Then
                    FunctionResult = FClient.WriteMultiVars(Items, Count)

                    If FunctionResult = 0 Then
                        For c = 0 To S7Client.MaxVars - 1
                            Results(c) = Items(c).Result
                        Next
                    End If

                    GlobalResult = FunctionResult
                Else
                    GlobalResult = errCliFunctionRefused
                End If

            Finally
                Clear() ' handles are no more needed and MUST be freed
            End Try

            Return GlobalResult
        End Function

        Public Sub Clear()
            For c = 0 To Count - 1
                If Not ([Handles](c)) = Nothing Then [Handles](c).Free()
            Next

            Count = 0
        End Sub
#End Region
    End Class


    Public Class S7Client
#Region "[Constants and TypeDefs]"

        ' Block type
        Public Const Block_OB As Integer = &H38
        Public Const Block_DB As Integer = &H41
        Public Const Block_SDB As Integer = &H42
        Public Const Block_FC As Integer = &H43
        Public Const Block_SFC As Integer = &H44
        Public Const Block_FB As Integer = &H45
        Public Const Block_SFB As Integer = &H46

        ' Sub Block Type 
        Public Const SubBlk_OB As Byte = &H8
        Public Const SubBlk_DB As Byte = &HA
        Public Const SubBlk_SDB As Byte = &HB
        Public Const SubBlk_FC As Byte = &HC
        Public Const SubBlk_SFC As Byte = &HD
        Public Const SubBlk_FB As Byte = &HE
        Public Const SubBlk_SFB As Byte = &HF

        ' Block languages
        Public Const BlockLangAWL As Byte = &H1
        Public Const BlockLangKOP As Byte = &H2
        Public Const BlockLangFUP As Byte = &H3
        Public Const BlockLangSCL As Byte = &H4
        Public Const BlockLangDB As Byte = &H5
        Public Const BlockLangGRAPH As Byte = &H6

        ' Max number of vars (multiread/write)
        Public Shared ReadOnly MaxVars As Integer = 20

        ' Result transport size
        Const TS_ResBit As Byte = &H3
        Const TS_ResByte As Byte = &H4
        Const TS_ResInt As Byte = &H5
        Const TS_ResReal As Byte = &H7
        Const TS_ResOctet As Byte = &H9
        Const Code7Ok As UShort = &H0
        Const Code7AddressOutOfRange As UShort = &H5
        Const Code7InvalidTransportSize As UShort = &H6
        Const Code7WriteDataSizeMismatch As UShort = &H7
        Const Code7ResItemNotAvailable As UShort = &HA
        Const Code7ResItemNotAvailable1 As UShort = &HD209
        Const Code7InvalidValue As UShort = &HDC01
        Const Code7NeedPassword As UShort = &HD241
        Const Code7InvalidPassword As UShort = &HD602
        Const Code7NoPasswordToClear As UShort = &HD604
        Const Code7NoPasswordToSet As UShort = &HD605
        Const Code7FunNotAvailable As UShort = &H8104
        Const Code7DataOverPDU As UShort = &H8500

        ' Client Connection Type
        Public Shared ReadOnly CONNTYPE_PG As UShort = &H1  ' Connect to the PLC as a PG
        Public Shared ReadOnly CONNTYPE_OP As UShort = &H2  ' Connect to the PLC as an OP
        Public Shared ReadOnly CONNTYPE_BASIC As UShort = &H3  ' Basic connection 
        Public _LastError As Integer = 0

        Public Structure S7DataItem
            Public Area As Integer
            Public WordLen As Integer
            Public Result As Integer
            Public DBNumber As Integer
            Public Start As Integer
            Public Amount As Integer
            Public pData As IntPtr
        End Structure

        ' Order Code + Version
        Public Structure S7OrderCode
            Public Code As String ' such as "6ES7 151-8AB01-0AB0"
            Public V1 As Byte     ' Version 1st digit
            Public V2 As Byte     ' Version 2nd digit
            Public V3 As Byte     ' Version 3th digit
        End Structure

        ' CPU Info
        Public Structure S7CpuInfo
            Public ModuleTypeName As String
            Public SerialNumber As String
            Public ASName As String
            Public Copyright As String
            Public ModuleName As String
        End Structure

        Public Structure S7CpInfo
            Public MaxPduLength As Integer
            Public MaxConnections As Integer
            Public MaxMpiRate As Integer
            Public MaxBusRate As Integer
        End Structure

        ' Block List
        <StructLayout(LayoutKind.Sequential, Pack:=1)>
        Public Structure S7BlocksList
            Public OBCount As Integer
            Public FBCount As Integer
            Public FCCount As Integer
            Public SFBCount As Integer
            Public SFCCount As Integer
            Public DBCount As Integer
            Public SDBCount As Integer
        End Structure

        ' Managed Block Info
        Public Structure S7BlockInfo
            Public BlkType As Integer
            Public BlkNumber As Integer
            Public BlkLang As Integer
            Public BlkFlags As Integer
            Public MC7Size As Integer  ' The real size in bytes
            Public LoadSize As Integer
            Public LocalData As Integer
            Public SBBLength As Integer
            Public CheckSum As Integer
            Public Version As Integer
            ' Chars info
            Public CodeDate As String
            Public IntfDate As String
            Public Author As String
            Public Family As String
            Public Header As String
        End Structure

        ' See §33.1 of "System Software for S7-300/400 System and Standard Functions"
        ' and see SFC51 description too
        <StructLayout(LayoutKind.Sequential, Pack:=1)>
        Public Structure SZL_HEADER
            Public LENTHDR As UShort
            Public N_DR As UShort
        End Structure

        <StructLayout(LayoutKind.Sequential, Pack:=1)>
        Public Structure S7SZL
            Public Header As SZL_HEADER
            <MarshalAs(UnmanagedType.ByValArray)>
            Public Data As Byte()
        End Structure

        ' SZL List of available SZL IDs : same as SZL but List items are big-endian adjusted
        <StructLayout(LayoutKind.Sequential, Pack:=1)>
        Public Structure S7SZLList
            Public Header As SZL_HEADER
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=&H2000 - 2)>
            Public Data As UShort()
        End Structure

        ' S7 Protection
        ' See §33.19 of "System Software for S7-300/400 System and Standard Functions"
        Public Structure S7Protection
            Public sch_schal As UShort
            Public sch_par As UShort
            Public sch_rel As UShort
            Public bart_sch As UShort
            Public anl_sch As UShort
        End Structure

#End Region

#Region "[S7 Telegrams]"

        ' ISO Connection Request telegram (contains also ISO Header and COTP Header)
        ' TPKT (RFC1006 Header)
        ' COTP (ISO 8073 Header)
        Private ISO_CR As Byte() = {&H3, &H0, &H0, &H16, &H11, &HE0, &H0, &H0, &H0, &H1, &H0, &HC0, &H1, &HA, &HC1, &H2, &H1, &H0, &HC2, &H2, &H1, &H2} ' RFC 1006 ID (3) 
        ' Reserved, always 0
        ' High part of packet lenght (entire frame, payload and TPDU included)
        ' Low part of packet lenght (entire frame, payload and TPDU included)
        ' PDU Size Length
        ' CR - Connection Request ID
        ' Dst Reference HI
        ' Dst Reference LO
        ' Src Reference HI
        ' Src Reference LO
        ' Class + Options Flags
        ' PDU Max Length ID
        ' PDU Max Length HI
        ' PDU Max Length LO
        ' Src TSAP Identifier
        ' Src TSAP Length (2 bytes)
        ' Src TSAP HI (will be overwritten)
        ' Src TSAP LO (will be overwritten)
        ' Dst TSAP Identifier
        ' Dst TSAP Length (2 bytes)
        ' Dst TSAP HI (will be overwritten)
        ' Dst TSAP LO (will be overwritten)

        ' TPKT + ISO COTP Header (Connection Oriented Transport Protocol)
        Private TPKT_ISO As Byte() = {&H3, &H0, &H0, &H1F, &H2, &HF0, &H80} ' 7 bytes
        ' Telegram Length (Data Size + 31 or 35)
        ' COTP (see above for info)

        ' S7 PDU Negotiation Telegram (contains also ISO Header and COTP Header)
        Private S7_PN As Byte() = {&H3, &H0, &H0, &H19, &H2, &HF0, &H80, &H32, &H1, &H0, &H0, &H4, &H0, &H0, &H8, &H0, &H0, &HF0, &H0, &H0, &H1, &H0, &H1, &H0, &H1E} ' TPKT + COTP (see above for info)
        ' PDU Length Requested = HI-LO Here Default 480 bytes

        ' S7 Read/Write Request Header (contains also ISO Header and COTP Header)
        ' WR area
        Private S7_RW As Byte() = {&H3, &H0, &H0, &H1F, &H2, &HF0, &H80, &H32, &H1, &H0, &H0, &H5, &H0, &H0, &HE, &H0, &H0, &H4, &H1, &H12, &HA, &H10, S7Consts.S7WLByte, &H0, &H0, &H0, &H0, &H84, &H0, &H0, &H0, &H0, &H4, &H0, &H0} ' 31-35 bytes
        ' Telegram Length (Data Size + 31 or 35)
        ' COTP (see above for info)
        ' S7 Protocol ID 
        ' Job Type
        ' Redundancy identification
        ' PDU Reference
        ' Parameters Length
        ' Data Length = Size(bytes) + 4      
        ' Function 4 Read Var, 5 Write Var  
        ' Items count
        ' Var spec.
        ' Length of remaining bytes
        ' Syntax ID 
        ' Transport Size idx=22                       
        ' Num Elements                          
        ' DB Number (if any, else 0)            
        ' Area Type                            
        ' Area Offset                     
        ' Reserved 
        ' Transport size
        ' Data Length * 8 (if not bit or timer or counter) 
        Public Size_RD As Integer = 31 ' Header Size when Reading 
        Public Size_WR As Integer = 35 ' Header Size when Writing

        ' S7 Variable MultiRead Header
        Private S7_MRD_HEADER As Byte() = {&H3, &H0, &H0, &H1F, &H2, &HF0, &H80, &H32, &H1, &H0, &H0, &H5, &H0, &H0, &HE, &H0, &H0, &H4, &H1}       ' Telegram Length 
        ' COTP (see above for info)
        ' S7 Protocol ID 
        ' Job Type
        ' Redundancy identification
        ' PDU Reference
        ' Parameters Length
        ' Data Length = Size(bytes) + 4      
        ' Function 4 Read Var, 5 Write Var  
        ' Items count (idx 18)

        ' S7 Variable MultiRead Item
        Private S7_MRD_ITEM As Byte() = {&H12, &HA, &H10, S7Consts.S7WLByte, &H0, &H0, &H0, &H0, &H84, &H0, &H0, &H0}            ' Var spec.
        ' Length of remaining bytes
        ' Syntax ID 
        ' Transport Size idx=3                   
        ' Num Elements                          
        ' DB Number (if any, else 0)            
        ' Area Type                            
        ' Area Offset                     

        ' S7 Variable MultiWrite Header
        Private S7_MWR_HEADER As Byte() = {&H3, &H0, &H0, &H1F, &H2, &HF0, &H80, &H32, &H1, &H0, &H0, &H5, &H0, &H0, &HE, &H0, &H0, &H5, &H1}       ' Telegram Length 
        ' COTP (see above for info)
        ' S7 Protocol ID 
        ' Job Type
        ' Redundancy identification
        ' PDU Reference
        ' Parameters Length (idx 13)
        ' Data Length = Size(bytes) + 4 (idx 15)     
        ' Function 5 Write Var  
        ' Items count (idx 18)

        ' S7 Variable MultiWrite Item (Param)
        Private S7_MWR_PARAM As Byte() = {&H12, &HA, &H10, S7Consts.S7WLByte, &H0, &H0, &H0, &H0, &H84, &H0, &H0, &H0}            ' Var spec.
        ' Length of remaining bytes
        ' Syntax ID 
        ' Transport Size idx=3                      
        ' Num Elements                          
        ' DB Number (if any, else 0)            
        ' Area Type                            
        ' Area Offset                     

        ' SZL First telegram request   
        Private S7_SZL_FIRST As Byte() = {&H3, &H0, &H0, &H21, &H2, &HF0, &H80, &H32, &H7, &H0, &H0, &H5, &H0, &H0, &H8, &H0, &H8, &H0, &H1, &H12, &H4, &H11, &H44, &H1, &H0, &HFF, &H9, &H0, &H4, &H0, &H0, &H0, &H0} ' Sequence out
        ' ID (29)
        ' Index (31)

        ' SZL Next telegram request 
        Private S7_SZL_NEXT As Byte() = {&H3, &H0, &H0, &H21, &H2, &HF0, &H80, &H32, &H7, &H0, &H0, &H6, &H0, &H0, &HC, &H0, &H4, &H0, &H1, &H12, &H8, &H12, &H44, &H1, &H1, &H0, &H0, &H0, &H0, &HA, &H0, &H0, &H0} ' Sequence

        ' Get Date/Time request
        Private S7_GET_DT As Byte() = {&H3, &H0, &H0, &H1D, &H2, &HF0, &H80, &H32, &H7, &H0, &H0, &H38, &H0, &H0, &H8, &H0, &H4, &H0, &H1, &H12, &H4, &H11, &H47, &H1, &H0, &HA, &H0, &H0, &H0}

        ' Set Date/Time command
        Private S7_SET_DT As Byte() = {&H3, &H0, &H0, &H27, &H2, &HF0, &H80, &H32, &H7, &H0, &H0, &H89, &H3, &H0, &H8, &H0, &HE, &H0, &H1, &H12, &H4, &H11, &H47, &H2, &H0, &HFF, &H9, &H0, &HA, &H0, &H19, &H13, &H12, &H6, &H17, &H37, &H13, &H0, &H1} ' Hi part of Year (idx=30)
        ' Lo part of Year
        ' Month
        ' Day
        ' Hour
        ' Min
        ' Sec
        ' ms + Day of week   

        ' S7 Set Session Password 
        ' 8 Char Encoded Password
        Private S7_SET_PWD As Byte() = {&H3, &H0, &H0, &H25, &H2, &HF0, &H80, &H32, &H7, &H0, &H0, &H27, &H0, &H0, &H8, &H0, &HC, &H0, &H1, &H12, &H4, &H11, &H45, &H1, &H0, &HFF, &H9, &H0, &H8, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0}

        ' S7 Clear Session Password 
        Private S7_CLR_PWD As Byte() = {&H3, &H0, &H0, &H1D, &H2, &HF0, &H80, &H32, &H7, &H0, &H0, &H29, &H0, &H0, &H8, &H0, &H4, &H0, &H1, &H12, &H4, &H11, &H45, &H2, &H0, &HA, &H0, &H0, &H0}

        ' S7 STOP request
        Private S7_STOP As Byte() = {&H3, &H0, &H0, &H21, &H2, &HF0, &H80, &H32, &H1, &H0, &H0, &HE, &H0, &H0, &H10, &H0, &H0, &H29, &H0, &H0, &H0, &H0, &H0, &H9, &H50, &H5F, &H50, &H52, &H4F, &H47, &H52, &H41, &H4D}

        ' S7 HOT Start request
        Private S7_HOT_START As Byte() = {&H3, &H0, &H0, &H25, &H2, &HF0, &H80, &H32, &H1, &H0, &H0, &HC, &H0, &H0, &H14, &H0, &H0, &H28, &H0, &H0, &H0, &H0, &H0, &H0, &HFD, &H0, &H0, &H9, &H50, &H5F, &H50, &H52, &H4F, &H47, &H52, &H41, &H4D}

        ' S7 COLD Start request
        Private S7_COLD_START As Byte() = {&H3, &H0, &H0, &H27, &H2, &HF0, &H80, &H32, &H1, &H0, &H0, &HF, &H0, &H0, &H16, &H0, &H0, &H28, &H0, &H0, &H0, &H0, &H0, &H0, &HFD, &H0, &H2, &H43, &H20, &H9, &H50, &H5F, &H50, &H52, &H4F, &H47, &H52, &H41, &H4D}
        Const pduStart As Byte = &H28   ' CPU start
        Const pduStop As Byte = &H29   ' CPU stop
        Const pduAlreadyStarted As Byte = &H2   ' CPU already in run mode
        Const pduAlreadyStopped As Byte = &H7   ' CPU already in stop mode

        ' S7 Get PLC Status 
        Private S7_GET_STAT As Byte() = {&H3, &H0, &H0, &H21, &H2, &HF0, &H80, &H32, &H7, &H0, &H0, &H2C, &H0, &H0, &H8, &H0, &H8, &H0, &H1, &H12, &H4, &H11, &H44, &H1, &H0, &HFF, &H9, &H0, &H4, &H4, &H24, &H0, &H0}

        ' S7 Get Block Info Request Header (contains also ISO Header and COTP Header)
        Private S7_BI As Byte() = {&H3, &H0, &H0, &H25, &H2, &HF0, &H80, &H32, &H7, &H0, &H0, &H5, &H0, &H0, &H8, &H0, &HC, &H0, &H1, &H12, &H4, &H11, &H43, &H3, &H0, &HFF, &H9, &H0, &H8, &H30, &H41, &H30, &H30, &H30, &H30, &H30, &H41} ' Block Type
        ' ASCII Block Number

        ' S7 List Blocks Request Header
        Private S7_LIST_BLOCKS As Byte() = {&H3, &H0, &H0, &H1D, &H2, &HF0, &H80, &H32, &H7, &H0, &H0, &H0, &H0, &H0, &H8, &H0, &H4, &H0, &H1, &H12, &H4, &H11, &H43, &H1, &H0, &HA, &H0, &H0, &H0} ' 0x43 0x01 = ListBlocks

        ' S7 List Blocks Of Type Request Header
        Private S7_LIST_BLOCKS_OF_TYPE As Byte() = {&H3, &H0, &H0, &H1F, &H2, &HF0, &H80, &H32, &H7, &H0, &H0, &H0, &H0, &H0, &H8, &H0, &H6, &H0, &H1, &H12, &H4, &H11, &H43, &H2, &H0} ' 0x43 0x02 = ListBlocksOfType
        ' ... append ReqData

#End Region

#Region "[Internals]"

        ' Defaults
        Public ISOTCP As Integer = 102 ' ISOTCP Port
        Public MinPduSize As Integer = 16
        Public MinPduSizeToRequest As Integer = 240
        Public MaxPduSizeToRequest As Integer = 960
        Public DefaultTimeout As Integer = 2000
        Public IsoHSize As Integer = 7 ' TPKT+COTP Header Size

        ' Properties
        Private _PDULength As Integer = 0
        Private _PduSizeRequested As Integer = 480
        Private _PLCPort As Integer = ISOTCP
        Private _RecvTimeout As Integer = DefaultTimeout
        Private _SendTimeout As Integer = DefaultTimeout
        Private _ConnTimeout As Integer = DefaultTimeout

        ' Privates
        Private IPAddress As String
        Private LocalTSAP_HI As Byte
        Private LocalTSAP_LO As Byte
        Private RemoteTSAP_HI As Byte
        Private RemoteTSAP_LO As Byte
        Private LastPDUType As Byte
        Private ConnType As UShort = CONNTYPE_PG
        Private PDU As Byte() = New Byte(2047) {}
        Private Socket As MsgSocket = Nothing
        Private Time_ms As Integer = 0
        Private cntword As UShort = 0

        Private Sub CreateSocket()
            Try
                Socket = New MsgSocket()
                Socket.ConnectTimeout = _ConnTimeout
                Socket.ReadTimeout = _RecvTimeout
                Socket.WriteTimeout = _SendTimeout
            Catch
            End Try
        End Sub

        Private Function TCPConnect() As Integer
            If _LastError = 0 Then
                Try
                    _LastError = Socket.Connect(IPAddress, _PLCPort)
                Catch
                    _LastError = errTCPConnectionFailed
                End Try
            End If

            Return _LastError
        End Function

        Private Sub RecvPacket(ByVal Buffer As Byte(), ByVal Start As Integer, ByVal Size As Integer)
            If Connected Then
                _LastError = Socket.Receive(Buffer, Start, Size)
            Else
                _LastError = errTCPNotConnected
            End If
        End Sub

        Private Sub SendPacket(ByVal Buffer As Byte(), ByVal Len As Integer)
            _LastError = Socket.Send(Buffer, Len)
        End Sub

        Private Sub SendPacket(ByVal Buffer As Byte())
            If Connected Then
                SendPacket(Buffer, Buffer.Length)
            Else
                _LastError = errTCPNotConnected
            End If
        End Sub

        Private Function RecvIsoPacket() As Integer
            Dim Done = False
            Dim Size = 0

            While _LastError = 0 AndAlso Not Done
                ' Get TPKT (4 bytes)
                RecvPacket(PDU, 0, 4)

                If _LastError = 0 Then
                    Size = S7.GetWordAt(PDU, 2)
                    ' Check 0 bytes Data Packet (only TPKT+COTP = 7 bytes)
                    If Size = IsoHSize Then
                        RecvPacket(PDU, 4, 3) ' Skip remaining 3 bytes and Done is still false
                    Else

                        If Size > _PduSizeRequested + IsoHSize OrElse Size < MinPduSize Then
                            _LastError = errIsoInvalidPDU
                        Else
                            Done = True
                        End If ' a valid Length !=7 && >16 && <247
                    End If
                End If
            End While

            If _LastError = 0 Then
                RecvPacket(PDU, 4, 3) ' Skip remaining 3 COTP bytes
                LastPDUType = PDU(5)   ' Stores PDU Type, we need it 
                ' Receives the S7 Payload          
                RecvPacket(PDU, 7, Size - IsoHSize)
            End If

            If _LastError = 0 Then
                Return Size
            Else
                Return 0
            End If
        End Function

        Private Function ISOConnect() As Integer
            Dim Size As Integer
            ISO_CR(16) = LocalTSAP_HI
            ISO_CR(17) = LocalTSAP_LO
            ISO_CR(20) = RemoteTSAP_HI
            ISO_CR(21) = RemoteTSAP_LO

            ' Sends the connection request telegram      
            SendPacket(ISO_CR)

            If _LastError = 0 Then
                ' Gets the reply (if any)
                Size = RecvIsoPacket()

                If _LastError = 0 Then
                    If Size = 22 Then
                        If LastPDUType <> CByte(&HD0) Then _LastError = errIsoConnect ' 0xD0 = CC Connection confirm
                    Else
                        _LastError = errIsoInvalidPDU
                    End If
                End If
            End If

            Return _LastError
        End Function

        Private Function NegotiatePduLength() As Integer
            Dim Length As Integer
            ' Set PDU Size Requested
            S7.SetWordAt(S7_PN, 23, _PduSizeRequested)
            ' Sends the connection request telegram
            SendPacket(S7_PN)

            If _LastError = 0 Then
                Length = RecvIsoPacket()

                If _LastError = 0 Then
                    ' check S7 Error
                    If Length = 27 AndAlso PDU(17) = 0 AndAlso PDU(18) = 0 Then  ' 20 = size of Negotiate Answer
                        ' Get PDU Size Negotiated
                        _PDULength = S7.GetWordAt(PDU, 25)
                        If _PDULength <= 0 Then _LastError = errCliNegotiatingPDU
                    Else
                        _LastError = errCliNegotiatingPDU
                    End If
                End If
            End If

            Return _LastError
        End Function

        Private Function CpuError(ByVal [Error] As UShort) As Integer
            Select Case [Error]
                Case 0
                    Return 0
                Case Code7AddressOutOfRange
                    Return errCliAddressOutOfRange
                Case Code7InvalidTransportSize
                    Return errCliInvalidTransportSize
                Case Code7WriteDataSizeMismatch
                    Return errCliWriteDataSizeMismatch
                Case Code7ResItemNotAvailable, Code7ResItemNotAvailable1
                    Return errCliItemNotAvailable
                Case Code7DataOverPDU
                    Return errCliSizeOverPDU
                Case Code7InvalidValue
                    Return errCliInvalidValue
                Case Code7FunNotAvailable
                    Return errCliFunNotAvailable
                Case Code7NeedPassword
                    Return errCliNeedPassword
                Case Code7InvalidPassword
                    Return errCliInvalidPassword
                Case Code7NoPasswordToSet, Code7NoPasswordToClear
                    Return errCliNoPasswordToSetOrClear
                Case Else
                    Return errCliFunctionRefused
            End Select
        End Function

        Private Function GetNextWord() As UShort
            Return Math.Min(Interlocked.Increment((cntword)), cntword - 1)
        End Function

#End Region

#Region "[Class Control]"

        Public Sub New()
            CreateSocket()
        End Sub

        Protected Overrides Sub Finalize()
            Disconnect()
        End Sub

        Public Function Connect() As Integer
            _LastError = 0
            Time_ms = 0
            Dim Elapsed = Environment.TickCount

            If Not Connected Then
                TCPConnect() ' First stage : TCP Connection

                If _LastError = 0 Then
                    ISOConnect() ' Second stage : ISOTCP (ISO 8073) Connection

                    If _LastError = 0 Then
                        _LastError = NegotiatePduLength() ' Third stage : S7 PDU negotiation
                    End If
                End If
            End If

            If _LastError <> 0 Then
                Disconnect()
            Else
                Time_ms = Environment.TickCount - Elapsed
            End If

            Return _LastError
        End Function

        Public Function ConnectTo(ByVal Address As String, ByVal Rack As Integer, ByVal Slot As Integer) As Integer
            Dim RemoteTSAP As UShort = (ConnType << 8) + Rack * &H20 + Slot
            SetConnectionParams(Address, &H100, RemoteTSAP)
            Return Connect()
        End Function

        Public Function SetConnectionParams(ByVal Address As String, ByVal LocalTSAP As UShort, ByVal RemoteTSAP As UShort) As Integer
            Dim LocTSAP = LocalTSAP And &HFFFF
            Dim RemTSAP = RemoteTSAP And &HFFFF
            IPAddress = Address
            LocalTSAP_HI = CByte(LocTSAP >> 8)
            LocalTSAP_LO = CByte(LocTSAP And &HFF)
            RemoteTSAP_HI = CByte(RemTSAP >> 8)
            RemoteTSAP_LO = CByte(RemTSAP And &HFF)
            Return 0
        End Function

        Public Function SetConnectionType(ByVal ConnectionType As UShort) As Integer
            ConnType = ConnectionType
            Return 0
        End Function

        Public Function Disconnect() As Integer
            Socket.Close()
            Return 0
        End Function

        Public Function GetParam(ByVal ParamNumber As Integer, ByRef Value As Integer) As Integer
            Dim Result = 0

            Select Case ParamNumber
                Case p_u16_RemotePort
                    Value = PLCPort
                    Exit Select
                Case p_i32_PingTimeout
                    Value = ConnTimeout
                    Exit Select
                Case p_i32_SendTimeout
                    Value = SendTimeout
                    Exit Select
                Case p_i32_RecvTimeout
                    Value = RecvTimeout
                    Exit Select
                Case p_i32_PDURequest
                    Value = PduSizeRequested
                    Exit Select
                Case Else
                    Result = errCliInvalidParamNumber
                    Exit Select
            End Select

            Return Result
        End Function

        ' Set Properties for compatibility with Snap7.net.cs
        Public Function SetParam(ByVal ParamNumber As Integer, ByRef Value As Integer) As Integer
            Dim Result = 0

            Select Case ParamNumber
                Case p_u16_RemotePort
                    PLCPort = Value
                    Exit Select
                Case p_i32_PingTimeout
                    ConnTimeout = Value
                    Exit Select
                Case p_i32_SendTimeout
                    SendTimeout = Value
                    Exit Select
                Case p_i32_RecvTimeout
                    RecvTimeout = Value
                    Exit Select
                Case p_i32_PDURequest
                    PduSizeRequested = Value
                    Exit Select
                Case Else
                    Result = errCliInvalidParamNumber
                    Exit Select
            End Select

            Return Result
        End Function

        Public Delegate Sub S7CliCompletion(ByVal usrPtr As IntPtr, ByVal opCode As Integer, ByVal opResult As Integer)

        Public Function SetAsCallBack(ByVal Completion As S7CliCompletion, ByVal usrPtr As IntPtr) As Integer
            Return errCliFunctionNotImplemented
        End Function

#End Region

#Region "[Data I/O main functions]"

        Public Function ReadArea(ByVal Area As Integer, ByVal DBNumber As Integer, ByVal Start As Integer, ByVal Amount As Integer, ByVal WordLen As Integer, ByVal Buffer As Byte()) As Integer
            Dim BytesRead = 0
            Return ReadArea(Area, DBNumber, Start, Amount, WordLen, Buffer, BytesRead)
        End Function

        Public Function ReadArea(ByVal Area As Integer, ByVal DBNumber As Integer, ByVal Start As Integer, ByVal Amount As Integer, ByVal WordLen As Integer, ByVal Buffer As Byte(), ByRef BytesRead As Integer) As Integer
            Dim Address As Integer
            Dim NumElements As Integer
            Dim MaxElements As Integer
            Dim TotElements As Integer
            Dim SizeRequested As Integer
            Dim Length As Integer
            Dim Offset = 0
            Dim WordSize = 1
            _LastError = 0
            Time_ms = 0
            Dim Elapsed = Environment.TickCount
            ' Some adjustment
            If Area = S7AreaCT Then WordLen = S7WLCounter
            If Area = S7AreaTM Then WordLen = S7WLTimer

            ' Calc Word size          
            WordSize = S7.DataSizeByte(WordLen)
            If WordSize = 0 Then Return errCliInvalidWordLen

            If WordLen = S7Consts.S7WLBit Then
                Amount = 1  ' Only 1 bit can be transferred at time
            Else

                If WordLen <> S7WLCounter AndAlso WordLen <> S7WLTimer Then
                    Amount = Amount * WordSize
                    WordSize = 1
                    WordLen = S7Consts.S7WLByte
                End If
            End If

            MaxElements = (_PDULength - 18) / WordSize ' 18 = Reply telegram header
            TotElements = Amount

            While TotElements > 0 AndAlso _LastError = 0
                NumElements = TotElements
                If NumElements > MaxElements Then NumElements = MaxElements
                SizeRequested = NumElements * WordSize

                ' Setup the telegram
                Array.Copy(S7_RW, 0, PDU, 0, Size_RD)
                ' Set DB Number
                PDU(27) = CByte(Area)
                ' Set Area
                If Area = S7AreaDB Then S7.SetWordAt(PDU, 25, DBNumber)

                ' Adjusts Start and word length
                If WordLen = S7Consts.S7WLBit OrElse WordLen = S7WLCounter OrElse WordLen = S7WLTimer Then
                    Address = Start
                    PDU(22) = CByte(WordLen)
                Else
                    Address = Start << 3
                End If

                ' Num elements
                S7.SetWordAt(PDU, 23, NumElements)

                ' Address into the PLC (only 3 bytes)           
                PDU(30) = CByte(Address And &HFF)
                Address = Address >> 8
                PDU(29) = CByte(Address And &HFF)
                Address = Address >> 8
                PDU(28) = CByte(Address And &HFF)
                SendPacket(PDU, Size_RD)

                If _LastError = 0 Then
                    Length = RecvIsoPacket()

                    If _LastError = 0 Then
                        If Length < 25 Then
                            _LastError = errIsoInvalidDataSize
                        Else

                            If PDU(21) <> &HFF Then
                                _LastError = CpuError(PDU(21))
                            Else
                                Array.Copy(PDU, 25, Buffer, Offset, SizeRequested)
                                Offset += SizeRequested
                            End If
                        End If
                    End If
                End If

                TotElements -= NumElements
                Start += NumElements * WordSize
            End While

            If _LastError = 0 Then
                BytesRead = Offset
                Time_ms = Environment.TickCount - Elapsed
            Else
                BytesRead = 0
            End If

            Return _LastError
        End Function

        Public Function WriteArea(ByVal Area As Integer, ByVal DBNumber As Integer, ByVal Start As Integer, ByVal Amount As Integer, ByVal WordLen As Integer, ByVal Buffer As Byte()) As Integer
            Dim BytesWritten = 0
            Return WriteArea(Area, DBNumber, Start, Amount, WordLen, Buffer, BytesWritten)
        End Function

        Public Function WriteArea(ByVal Area As Integer, ByVal DBNumber As Integer, ByVal Start As Integer, ByVal Amount As Integer, ByVal WordLen As Integer, ByVal Buffer As Byte(), ByRef BytesWritten As Integer) As Integer
            Dim Address As Integer
            Dim NumElements As Integer
            Dim MaxElements As Integer
            Dim TotElements As Integer
            Dim DataSize As Integer
            Dim IsoSize As Integer
            Dim Length As Integer
            Dim Offset = 0
            Dim WordSize = 1
            _LastError = 0
            Time_ms = 0
            Dim Elapsed = Environment.TickCount
            ' Some adjustment
            If Area = S7AreaCT Then WordLen = S7WLCounter
            If Area = S7AreaTM Then WordLen = S7WLTimer

            ' Calc Word size          
            WordSize = S7.DataSizeByte(WordLen)
            If WordSize = 0 Then Return errCliInvalidWordLen

            If WordLen = S7Consts.S7WLBit Then ' Only 1 bit can be transferred at time
                Amount = 1
            Else

                If WordLen <> S7WLCounter AndAlso WordLen <> S7WLTimer Then
                    Amount = Amount * WordSize
                    WordSize = 1
                    WordLen = S7Consts.S7WLByte
                End If
            End If

            MaxElements = (_PDULength - 35) / WordSize ' 35 = Reply telegram header
            TotElements = Amount

            While TotElements > 0 AndAlso _LastError = 0
                NumElements = TotElements
                If NumElements > MaxElements Then NumElements = MaxElements
                DataSize = NumElements * WordSize
                IsoSize = Size_WR + DataSize

                ' Setup the telegram
                Array.Copy(S7_RW, 0, PDU, 0, Size_WR)
                ' Whole telegram Size
                S7.SetWordAt(PDU, 2, IsoSize)
                ' Data Length
                Length = DataSize + 4
                S7.SetWordAt(PDU, 15, Length)
                ' Function
                PDU(17) = &H5
                ' Set DB Number
                PDU(27) = CByte(Area)
                If Area = S7AreaDB Then S7.SetWordAt(PDU, 25, DBNumber)


                ' Adjusts Start and word length
                If WordLen = S7Consts.S7WLBit OrElse WordLen = S7WLCounter OrElse WordLen = S7WLTimer Then
                    Address = Start
                    Length = DataSize
                    PDU(22) = CByte(WordLen)
                Else
                    Address = Start << 3
                    Length = DataSize << 3
                End If

                ' Num elements
                S7.SetWordAt(PDU, 23, NumElements)
                ' Address into the PLC
                PDU(30) = CByte(Address And &HFF)
                Address = Address >> 8
                PDU(29) = CByte(Address And &HFF)
                Address = Address >> 8
                PDU(28) = CByte(Address And &HFF)

                ' Transport Size
                Select Case WordLen
                    Case S7Consts.S7WLBit
                        PDU(32) = TS_ResBit
                    Case S7WLCounter, S7WLTimer
                        PDU(32) = TS_ResOctet
                    Case Else
                        PDU(32) = TS_ResByte ' byte/word/dword etc.
                End Select
                ' Length
                S7.SetWordAt(PDU, 33, Length)

                ' Copies the Data
                Array.Copy(Buffer, Offset, PDU, 35, DataSize)
                SendPacket(PDU, IsoSize)

                If _LastError = 0 Then
                    Length = RecvIsoPacket()

                    If _LastError = 0 Then
                        If Length = 22 Then
                            If PDU(21) <> CByte(&HFF) Then _LastError = CpuError(PDU(21))
                        Else
                            _LastError = errIsoInvalidPDU
                        End If
                    End If
                End If

                Offset += DataSize
                TotElements -= NumElements
                Start += NumElements * WordSize
            End While

            If _LastError = 0 Then
                BytesWritten = Offset
                Time_ms = Environment.TickCount - Elapsed
            Else
                BytesWritten = 0
            End If

            Return _LastError
        End Function

        Public Function ReadMultiVars(ByVal Items As S7DataItem(), ByVal ItemsCount As Integer) As Integer
            Dim Offset As Integer
            Dim Length As Integer
            Dim ItemSize As Integer
            Dim S7Item = New Byte(11) {}
            Dim S7ItemRead = New Byte(1023) {}
            _LastError = 0
            Time_ms = 0
            Dim Elapsed = Environment.TickCount

            ' Checks items
            If ItemsCount > MaxVars Then Return errCliTooManyItems

            ' Fills Header
            Array.Copy(S7_MRD_HEADER, 0, PDU, 0, S7_MRD_HEADER.Length)
            S7.SetWordAt(PDU, 13, ItemsCount * S7Item.Length + 2)
            PDU(18) = CByte(ItemsCount)
            ' Fills the Items
            Offset = 19

            For c = 0 To ItemsCount - 1
                Array.Copy(S7_MRD_ITEM, S7Item, S7Item.Length)
                S7Item(3) = CByte(Items(c).WordLen)
                S7.SetWordAt(S7Item, 4, Items(c).Amount)
                If Items(c).Area = S7AreaDB Then S7.SetWordAt(S7Item, 6, Items(c).DBNumber)
                S7Item(8) = CByte(Items(c).Area)

                ' Address into the PLC
                Dim Address = Items(c).Start
                S7Item(11) = CByte(Address And &HFF)
                Address = Address >> 8
                S7Item(10) = CByte(Address And &HFF)
                Address = Address >> 8
                S7Item(9) = CByte(Address And &HFF)
                Array.Copy(S7Item, 0, PDU, Offset, S7Item.Length)
                Offset += S7Item.Length
            Next

            If Offset > _PDULength Then Return errCliSizeOverPDU
            S7.SetWordAt(PDU, 2, Offset) ' Whole size
            SendPacket(PDU, Offset)
            If _LastError <> 0 Then Return _LastError
            ' Get Answer
            Length = RecvIsoPacket()
            If _LastError <> 0 Then Return _LastError
            ' Check ISO Length
            If Length < 22 Then
                _LastError = errIsoInvalidPDU ' PDU too Small
                Return _LastError
            End If
            ' Check Global Operation Result
            _LastError = CpuError(S7.GetWordAt(PDU, 17))
            If _LastError <> 0 Then Return _LastError
            ' Get true ItemsCount
            Dim ItemsRead As Integer = S7.GetByteAt(PDU, 20)

            If ItemsRead <> ItemsCount OrElse ItemsRead > MaxVars Then
                _LastError = errCliInvalidPlcAnswer
                Return _LastError
            End If
            ' Get Data
            Offset = 21

            For c = 0 To ItemsCount - 1
                ' Get the Item
                Array.Copy(PDU, Offset, S7ItemRead, 0, Length - Offset)

                If S7ItemRead(0) = &HFF Then
                    ItemSize = S7.GetWordAt(S7ItemRead, 2)
                    If S7ItemRead(1) <> TS_ResOctet AndAlso S7ItemRead(1) <> TS_ResReal AndAlso S7ItemRead(1) <> TS_ResBit Then ItemSize = ItemSize >> 3
                    Marshal.Copy(S7ItemRead, 4, Items(c).pData, ItemSize)
                    Items(c).Result = 0
                    If ItemSize Mod 2 <> 0 Then ItemSize += 1 ' Odd size are rounded
                    Offset = Offset + 4 + ItemSize
                Else
                    Items(c).Result = CpuError(S7ItemRead(0))
                    Offset += 4 ' Skip the Item header                           
                End If
            Next

            Time_ms = Environment.TickCount - Elapsed
            Return _LastError
        End Function

        Public Function WriteMultiVars(ByVal Items As S7DataItem(), ByVal ItemsCount As Integer) As Integer
            Dim Offset As Integer
            Dim ParLength As Integer
            Dim DataLength As Integer
            Dim ItemDataSize As Integer
            Dim S7ParItem = New Byte(S7_MWR_PARAM.Length - 1) {}
            Dim S7DataItem = New Byte(1023) {}
            _LastError = 0
            Time_ms = 0
            Dim Elapsed = Environment.TickCount

            ' Checks items
            If ItemsCount > MaxVars Then Return errCliTooManyItems
            ' Fills Header
            Array.Copy(S7_MWR_HEADER, 0, PDU, 0, S7_MWR_HEADER.Length)
            ParLength = ItemsCount * S7_MWR_PARAM.Length + 2
            S7.SetWordAt(PDU, 13, ParLength)
            PDU(18) = CByte(ItemsCount)
            ' Fills Params
            Offset = S7_MWR_HEADER.Length

            For c = 0 To ItemsCount - 1
                Array.Copy(S7_MWR_PARAM, 0, S7ParItem, 0, S7_MWR_PARAM.Length)
                S7ParItem(3) = CByte(Items(c).WordLen)
                S7ParItem(8) = CByte(Items(c).Area)
                S7.SetWordAt(S7ParItem, 4, Items(c).Amount)
                S7.SetWordAt(S7ParItem, 6, Items(c).DBNumber)
                ' Address into the PLC
                Dim Address = Items(c).Start
                S7ParItem(11) = CByte(Address And &HFF)
                Address = Address >> 8
                S7ParItem(10) = CByte(Address And &HFF)
                Address = Address >> 8
                S7ParItem(9) = CByte(Address And &HFF)
                Array.Copy(S7ParItem, 0, PDU, Offset, S7ParItem.Length)
                Offset += S7_MWR_PARAM.Length
            Next
            ' Fills Data
            DataLength = 0

            For c = 0 To ItemsCount - 1
                S7DataItem(0) = &H0

                Select Case Items(c).WordLen
                    Case S7Consts.S7WLBit
                        S7DataItem(1) = TS_ResBit
                    Case S7WLCounter, S7WLTimer
                        S7DataItem(1) = TS_ResOctet
                    Case Else
                        S7DataItem(1) = TS_ResByte ' byte/word/dword etc.
                End Select

                If Items(c).WordLen = S7WLTimer OrElse Items(c).WordLen = S7WLCounter Then
                    ItemDataSize = Items(c).Amount * 2
                Else
                    ItemDataSize = Items(c).Amount
                End If

                If S7DataItem(1) <> TS_ResOctet AndAlso S7DataItem(1) <> TS_ResBit Then
                    S7.SetWordAt(S7DataItem, 2, ItemDataSize * 8)
                Else
                    S7.SetWordAt(S7DataItem, 2, ItemDataSize)
                End If

                Marshal.Copy(Items(c).pData, S7DataItem, 4, ItemDataSize)

                If ItemDataSize Mod 2 <> 0 Then
                    S7DataItem(ItemDataSize + 4) = &H0
                    ItemDataSize += 1
                End If

                Array.Copy(S7DataItem, 0, PDU, Offset, ItemDataSize + 4)
                Offset = Offset + ItemDataSize + 4
                DataLength = DataLength + ItemDataSize + 4
            Next

            ' Checks the size
            If Offset > _PDULength Then Return errCliSizeOverPDU
            S7.SetWordAt(PDU, 2, Offset) ' Whole size
            S7.SetWordAt(PDU, 15, DataLength) ' Whole size
            SendPacket(PDU, Offset)
            RecvIsoPacket()

            If _LastError = 0 Then
                ' Check Global Operation Result
                _LastError = CpuError(S7.GetWordAt(PDU, 17))
                If _LastError <> 0 Then Return _LastError
                ' Get true ItemsCount
                Dim ItemsWritten As Integer = S7.GetByteAt(PDU, 20)

                If ItemsWritten <> ItemsCount OrElse ItemsWritten > MaxVars Then
                    _LastError = errCliInvalidPlcAnswer
                    Return _LastError
                End If

                For c = 0 To ItemsCount - 1

                    If PDU(c + 21) = &HFF Then
                        Items(c).Result = 0
                    Else
                        Items(c).Result = CpuError(PDU(c + 21))
                    End If
                Next

                Time_ms = Environment.TickCount - Elapsed
            End If

            Return _LastError
        End Function

#End Region

#Region "[Data I/O lean functions]"

        Public Function DBRead(ByVal DBNumber As Integer, ByVal Start As Integer, ByVal Size As Integer, ByVal Buffer As Byte()) As Integer
            Return ReadArea(S7AreaDB, DBNumber, Start, Size, S7Consts.S7WLByte, Buffer)
        End Function

        Public Function DBWrite(ByVal DBNumber As Integer, ByVal Start As Integer, ByVal Size As Integer, ByVal Buffer As Byte()) As Integer
            Return WriteArea(S7AreaDB, DBNumber, Start, Size, S7Consts.S7WLByte, Buffer)
        End Function

        Public Function MBRead(ByVal Start As Integer, ByVal Size As Integer, ByVal Buffer As Byte()) As Integer
            Return ReadArea(S7AreaMK, 0, Start, Size, S7Consts.S7WLByte, Buffer)
        End Function

        Public Function MBWrite(ByVal Start As Integer, ByVal Size As Integer, ByVal Buffer As Byte()) As Integer
            Return WriteArea(S7AreaMK, 0, Start, Size, S7Consts.S7WLByte, Buffer)
        End Function

        Public Function EBRead(ByVal Start As Integer, ByVal Size As Integer, ByVal Buffer As Byte()) As Integer
            Return ReadArea(S7AreaPE, 0, Start, Size, S7Consts.S7WLByte, Buffer)
        End Function

        Public Function EBWrite(ByVal Start As Integer, ByVal Size As Integer, ByVal Buffer As Byte()) As Integer
            Return WriteArea(S7AreaPE, 0, Start, Size, S7Consts.S7WLByte, Buffer)
        End Function

        Public Function ABRead(ByVal Start As Integer, ByVal Size As Integer, ByVal Buffer As Byte()) As Integer
            Return ReadArea(S7AreaPA, 0, Start, Size, S7Consts.S7WLByte, Buffer)
        End Function

        Public Function ABWrite(ByVal Start As Integer, ByVal Size As Integer, ByVal Buffer As Byte()) As Integer
            Return WriteArea(S7AreaPA, 0, Start, Size, S7Consts.S7WLByte, Buffer)
        End Function

        Public Function TMRead(ByVal Start As Integer, ByVal Amount As Integer, ByVal Buffer As UShort()) As Integer
            Dim sBuffer = New Byte(Amount * 2 - 1) {}
            Dim Result = ReadArea(S7AreaTM, 0, Start, Amount, S7WLTimer, sBuffer)

            If Result = 0 Then
                For c = 0 To Amount - 1
                    Buffer(c) = (sBuffer(c * 2 + 1) << 8) + sBuffer(c * 2)
                Next
            End If

            Return Result
        End Function

        Public Function TMWrite(ByVal Start As Integer, ByVal Amount As Integer, ByVal Buffer As UShort()) As Integer
            Dim sBuffer = New Byte(Amount * 2 - 1) {}

            For c = 0 To Amount - 1
                sBuffer(c * 2 + 1) = CByte((Buffer(c) And &HFF00) >> 8)
                sBuffer(c * 2) = CByte(Buffer(c) And &HFF)
            Next

            Return WriteArea(S7AreaTM, 0, Start, Amount, S7WLTimer, sBuffer)
        End Function

        Public Function CTRead(ByVal Start As Integer, ByVal Amount As Integer, ByVal Buffer As UShort()) As Integer
            Dim sBuffer = New Byte(Amount * 2 - 1) {}
            Dim Result = ReadArea(S7AreaCT, 0, Start, Amount, S7WLCounter, sBuffer)

            If Result = 0 Then
                For c = 0 To Amount - 1
                    Buffer(c) = (sBuffer(c * 2 + 1) << 8) + sBuffer(c * 2)
                Next
            End If

            Return Result
        End Function

        Public Function CTWrite(ByVal Start As Integer, ByVal Amount As Integer, ByVal Buffer As UShort()) As Integer
            Dim sBuffer = New Byte(Amount * 2 - 1) {}

            For c = 0 To Amount - 1
                sBuffer(c * 2 + 1) = CByte((Buffer(c) And &HFF00) >> 8)
                sBuffer(c * 2) = CByte(Buffer(c) And &HFF)
            Next

            Return WriteArea(S7AreaCT, 0, Start, Amount, S7WLCounter, sBuffer)
        End Function

#End Region

#Region "[Directory functions]"

        Public Function ListBlocks(ByRef List As S7BlocksList) As Integer
            _LastError = 0
            Time_ms = 0
            Dim Elapsed = Environment.TickCount
            Dim Sequence As UShort = GetNextWord()
            Array.Copy(S7_LIST_BLOCKS, 0, PDU, 0, S7_LIST_BLOCKS.Length)
            PDU(&HB) = CByte(Sequence And &HFF)
            PDU(&HC) = CByte(Sequence >> 8)
            SendPacket(PDU, S7_LIST_BLOCKS.Length)
            If _LastError <> 0 Then Return _LastError
            Dim Length As Integer = RecvIsoPacket()

            If Length <= 32 Then ' the minimum expected
                _LastError = errIsoInvalidPDU
                Return _LastError
            End If

            Dim Result = S7.GetWordAt(PDU, 27)

            If Result <> 0 Then
                _LastError = CpuError(Result)
                Return _LastError
            End If

            List = Nothing
            Dim BlocksSize As Integer = S7.GetWordAt(PDU, 31)

            If Length <= 32 + BlocksSize Then
                _LastError = errIsoInvalidPDU
                Return _LastError
            End If

            Dim BlocksCount = BlocksSize >> 2

            For blockNum = 0 To BlocksCount - 1
                Dim Count As Integer = S7.GetWordAt(PDU, (blockNum << 2) + 35)

                Select Case S7.GetByteAt(PDU, (blockNum << 2) + 34) 'BlockType
                    Case Block_OB
                        List.OBCount = Count
                    Case Block_DB
                        List.DBCount = Count
                    Case Block_SDB
                        List.SDBCount = Count
                    Case Block_FC
                        List.FCCount = Count
                    Case Block_SFC
                        List.SFCCount = Count
                    Case Block_FB
                        List.FBCount = Count
                    Case Block_SFB
                        'Unknown block type. Ignore
                        List.SFBCount = Count
                    Case Else
                End Select
            Next

            Time_ms = Environment.TickCount - Elapsed
            Return _LastError ' 0
        End Function

        Private Function SiemensTimestamp(ByVal EncodedDate As Long) As String
            Dim DT As Date = New DateTime(1984, 1, 1).AddSeconds(EncodedDate * 86400)
#If WINDOWS_UWP Or NETFX_CORE Or CORE_CLR Then
			return DT.ToString(System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern);
#Else
            Return DT.ToShortDateString()
            '  return DT.ToString();
#End If
        End Function

        Public Function GetAgBlockInfo(ByVal BlockType As Integer, ByVal BlockNum As Integer, ByRef Info As S7BlockInfo) As Integer
            _LastError = 0
            Time_ms = 0
            Dim Elapsed = Environment.TickCount
            S7_BI(30) = CByte(BlockType)
            ' Block Number
            S7_BI(31) = CByte(BlockNum / 10000 + &H30)
            BlockNum = BlockNum Mod 10000
            S7_BI(32) = CByte(BlockNum / 1000 + &H30)
            BlockNum = BlockNum Mod 1000
            S7_BI(33) = CByte(BlockNum / 100 + &H30)
            BlockNum = BlockNum Mod 100
            S7_BI(34) = CByte(BlockNum / 10 + &H30)
            BlockNum = BlockNum Mod 10
            S7_BI(35) = CByte(BlockNum / 1 + &H30)
            SendPacket(S7_BI)

            If _LastError = 0 Then
                Dim Length As Integer = RecvIsoPacket()

                If Length > 32 Then ' the minimum expected
                    Dim Result = S7.GetWordAt(PDU, 27)

                    If Result = 0 Then
                        Info.BlkFlags = PDU(42)
                        Info.BlkLang = PDU(43)
                        Info.BlkType = PDU(44)
                        Info.BlkNumber = S7.GetWordAt(PDU, 45)
                        Info.LoadSize = S7.GetDIntAt(PDU, 47)
                        Info.CodeDate = SiemensTimestamp(S7.GetWordAt(PDU, 59))
                        Info.IntfDate = SiemensTimestamp(S7.GetWordAt(PDU, 65))
                        Info.SBBLength = S7.GetWordAt(PDU, 67)
                        Info.LocalData = S7.GetWordAt(PDU, 71)
                        Info.MC7Size = S7.GetWordAt(PDU, 73)
                        Info.Author = S7.GetCharsAt(CType(PDU, Byte()), CInt(75), CInt(8)).Trim(New Char() {Microsoft.VisualBasic.ChrW(0)})
                        Info.Family = S7.GetCharsAt(CType(PDU, Byte()), CInt(83), CInt(8)).Trim(New Char() {Microsoft.VisualBasic.ChrW(0)})
                        Info.Header = S7.GetCharsAt(CType(PDU, Byte()), CInt(91), CInt(8)).Trim(New Char() {Microsoft.VisualBasic.ChrW(0)})
                        Info.Version = PDU(99)
                        Info.CheckSum = S7.GetWordAt(PDU, 101)
                    Else
                        _LastError = CpuError(Result)
                    End If
                Else
                    _LastError = errIsoInvalidPDU
                End If
            End If

            If _LastError = 0 Then Time_ms = Environment.TickCount - Elapsed
            Return _LastError
        End Function

        Public Function GetPgBlockInfo(ByRef Info As S7BlockInfo, ByVal Buffer As Byte(), ByVal Size As Integer) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function ListBlocksOfType(ByVal BlockType As Integer, ByVal List As UShort(), ByRef ItemsCount As Integer) As Integer
            Dim First = True
            Dim Done = False
            Dim In_Seq As Byte = 0
            Dim Count = 0 'Block 1...n
            Dim PduLength As Integer
            Dim Elapsed = Environment.TickCount

            'Consequent packets have a different ReqData
            Dim ReqData = New Byte() {&HFF, &H9, &H0, &H2, &H30, BlockType}
            Dim ReqDataContinue = New Byte() {&H0, &H0, &H0, &H0, &HA, &H0, &H0, &H0}
            _LastError = 0
            Time_ms = 0

            Do
                PduLength = S7_LIST_BLOCKS_OF_TYPE.Length + ReqData.Length
                Dim Sequence As UShort = GetNextWord()
                Array.Copy(S7_LIST_BLOCKS_OF_TYPE, 0, PDU, 0, S7_LIST_BLOCKS_OF_TYPE.Length)
                S7.SetWordAt(PDU, &H2, PduLength)
                PDU(&HB) = CByte(Sequence And &HFF)
                PDU(&HC) = CByte(Sequence >> 8)

                If Not First Then
                    S7.SetWordAt(PDU, &HD, 12) 'ParLen
                    S7.SetWordAt(PDU, &HF, 4) 'DataLen
                    PDU(&H14) = 8 'PLen
                    PDU(&H15) = &H12 'Uk
                End If

                PDU(&H17) = &H2
                PDU(&H18) = In_Seq
                Array.Copy(ReqData, 0, PDU, &H19, ReqData.Length)
                SendPacket(PDU, PduLength)
                If _LastError <> 0 Then Return _LastError
                PduLength = RecvIsoPacket()
                If _LastError <> 0 Then Return _LastError

                If PduLength <= 32 Then ' the minimum expected
                    _LastError = errIsoInvalidPDU
                    Return _LastError
                End If

                Dim Result = S7.GetWordAt(PDU, &H1B)

                If Result <> 0 Then
                    _LastError = CpuError(Result)
                    Return _LastError
                End If

                If PDU(&H1D) <> &HFF Then
                    _LastError = errCliItemNotAvailable
                    Return _LastError
                End If

                Done = PDU(&H1A) = 0
                In_Seq = PDU(&H18)
                Dim CThis As Integer = S7.GetWordAt(PDU, &H1F) >> 2 'Amount of blocks in this message

                For c = 0 To CThis - 1

                    If Count >= ItemsCount Then 'RoomError
                        _LastError = errCliPartialDataRead
                        Return _LastError
                    End If

                    List(Math.Min(Interlocked.Increment(Count), Count - 1)) = S7.GetWordAt(PDU, &H21 + 4 * c)
                    Done = Done Or Count = &H8000 'but why?
                Next

                If First Then
                    ReqData = ReqDataContinue
                    First = False
                End If
            Loop While _LastError = 0 AndAlso Not Done

            If _LastError = 0 Then ItemsCount = Count
            Time_ms = Environment.TickCount - Elapsed
            Return _LastError ' 0
        End Function

#End Region

#Region "[Blocks functions]"

        Public Function Upload(ByVal BlockType As Integer, ByVal BlockNum As Integer, ByVal UsrData As Byte(), ByRef Size As Integer) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function FullUpload(ByVal BlockType As Integer, ByVal BlockNum As Integer, ByVal UsrData As Byte(), ByRef Size As Integer) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function Download(ByVal BlockNum As Integer, ByVal UsrData As Byte(), ByVal Size As Integer) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function Delete(ByVal BlockType As Integer, ByVal BlockNum As Integer) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function DBGet(ByVal DBNumber As Integer, ByVal UsrData As Byte(), ByRef Size As Integer) As Integer
            Dim BI As S7BlockInfo = New S7BlockInfo()
            Dim Elapsed = Environment.TickCount
            Time_ms = 0
            _LastError = GetAgBlockInfo(Block_DB, DBNumber, BI)

            If _LastError = 0 Then
                Dim DBSize = BI.MC7Size

                If DBSize <= UsrData.Length Then
                    Size = DBSize
                    _LastError = DBRead(DBNumber, 0, DBSize, UsrData)
                    If _LastError = 0 Then Size = DBSize
                Else
                    _LastError = errCliBufferTooSmall
                End If
            End If

            If _LastError = 0 Then Time_ms = Environment.TickCount - Elapsed
            Return _LastError
        End Function

        Public Function DBFill(ByVal DBNumber As Integer, ByVal FillChar As Integer) As Integer
            Dim BI As S7BlockInfo = New S7BlockInfo()
            Dim Elapsed = Environment.TickCount
            Time_ms = 0
            _LastError = GetAgBlockInfo(Block_DB, DBNumber, BI)

            If _LastError = 0 Then
                Dim Buffer = New Byte(BI.MC7Size - 1) {}

                For c = 0 To BI.MC7Size - 1
                    Buffer(c) = CByte(FillChar)
                Next

                _LastError = DBWrite(DBNumber, 0, BI.MC7Size, Buffer)
            End If

            If _LastError = 0 Then Time_ms = Environment.TickCount - Elapsed
            Return _LastError
        End Function

#End Region

#Region "[Date/Time functions]"

        Public Function GetPlcDateTime(ByRef DT As Date) As Integer
            Dim Length As Integer
            _LastError = 0
            Time_ms = 0
            Dim Elapsed = Environment.TickCount
            SendPacket(S7_GET_DT)

            If _LastError = 0 Then
                Length = RecvIsoPacket()

                If Length > 30 Then ' the minimum expected
                    If S7.GetWordAt(PDU, 27) = 0 AndAlso PDU(29) = &HFF Then
                        DT = S7.GetDateTimeAt(PDU, 35)
                    Else
                        _LastError = errCliInvalidPlcAnswer
                    End If
                Else
                    _LastError = errIsoInvalidPDU
                End If
            End If

            If _LastError = 0 Then Time_ms = Environment.TickCount - Elapsed
            Return _LastError
        End Function

        Public Function SetPlcDateTime(ByVal DT As Date) As Integer
            Dim Length As Integer
            _LastError = 0
            Time_ms = 0
            Dim Elapsed = Environment.TickCount
            S7.SetDateTimeAt(S7_SET_DT, 31, DT)
            SendPacket(S7_SET_DT)

            If _LastError = 0 Then
                Length = RecvIsoPacket()

                If Length > 30 Then ' the minimum expected
                    If S7.GetWordAt(PDU, 27) <> 0 Then _LastError = errCliInvalidPlcAnswer
                Else
                    _LastError = errIsoInvalidPDU
                End If
            End If

            If _LastError = 0 Then Time_ms = Environment.TickCount - Elapsed
            Return _LastError
        End Function

        Public Function SetPlcSystemDateTime() As Integer
            Return SetPlcDateTime(Date.Now)
        End Function

#End Region

#Region "[System Info functions]"

        Public Function GetOrderCode(ByRef Info As S7OrderCode) As Integer
            Dim SZL As S7SZL = New S7SZL()
            Dim Size = 1024
            SZL.Data = New Byte(Size - 1) {}
            Dim Elapsed = Environment.TickCount
            _LastError = ReadSZL(&H11, &H0, SZL, Size)

            If _LastError = 0 Then
                Info.Code = S7.GetCharsAt(SZL.Data, 2, 20)
                Info.V1 = SZL.Data(Size - 3)
                Info.V2 = SZL.Data(Size - 2)
                Info.V3 = SZL.Data(Size - 1)
            End If

            If _LastError = 0 Then Time_ms = Environment.TickCount - Elapsed
            Return _LastError
        End Function

        Public Function GetCpuInfo(ByRef Info As S7CpuInfo) As Integer
            Dim SZL As S7SZL = New S7SZL()
            Dim Size = 1024
            SZL.Data = New Byte(Size - 1) {}
            Dim Elapsed = Environment.TickCount
            _LastError = ReadSZL(&H1C, &H0, SZL, Size)

            If _LastError = 0 Then
                Info.ModuleTypeName = S7.GetCharsAt(SZL.Data, 172, 32)
                Info.SerialNumber = S7.GetCharsAt(SZL.Data, 138, 24)
                Info.ASName = S7.GetCharsAt(SZL.Data, 2, 24)
                Info.Copyright = S7.GetCharsAt(SZL.Data, 104, 26)
                Info.ModuleName = S7.GetCharsAt(SZL.Data, 36, 24)
            End If

            If _LastError = 0 Then Time_ms = Environment.TickCount - Elapsed
            Return _LastError
        End Function

        Public Function GetCpInfo(ByRef Info As S7CpInfo) As Integer
            Dim SZL As S7SZL = New S7SZL()
            Dim Size = 1024
            SZL.Data = New Byte(Size - 1) {}
            Dim Elapsed = Environment.TickCount
            _LastError = ReadSZL(&H131, &H1, SZL, Size)

            If _LastError = 0 Then
                Info.MaxPduLength = S7.GetIntAt(PDU, 2)
                Info.MaxConnections = S7.GetIntAt(PDU, 4)
                Info.MaxMpiRate = S7.GetDIntAt(PDU, 6)
                Info.MaxBusRate = S7.GetDIntAt(PDU, 10)
            End If

            If _LastError = 0 Then Time_ms = Environment.TickCount - Elapsed
            Return _LastError
        End Function

        Public Function ReadSZL(ByVal ID As Integer, ByVal Index As Integer, ByRef SZL As S7SZL, ByRef Size As Integer) As Integer
            Dim Length As Integer
            Dim DataSZL As Integer
            Dim Offset = 0
            Dim Done = False
            Dim First = True
            Dim Seq_in As Byte = &H0
            Dim Seq_out As UShort = &H0
            _LastError = 0
            Time_ms = 0
            Dim Elapsed = Environment.TickCount
            SZL.Header.LENTHDR = 0

            Do

                If First Then
                    S7.SetWordAt(S7_SZL_FIRST, 11, Interlocked.Increment((Seq_out)))
                    S7.SetWordAt(S7_SZL_FIRST, 29, ID)
                    S7.SetWordAt(S7_SZL_FIRST, 31, Index)
                    SendPacket(S7_SZL_FIRST)
                Else
                    S7.SetWordAt(S7_SZL_NEXT, 11, Interlocked.Increment((Seq_out)))
                    PDU(24) = Seq_in
                    SendPacket(S7_SZL_NEXT)
                End If

                If _LastError <> 0 Then Return _LastError
                Length = RecvIsoPacket()

                If _LastError = 0 Then
                    If First Then
                        If Length > 32 Then ' the minimum expected
                            If S7.GetWordAt(PDU, 27) = 0 AndAlso PDU(29) = CByte(&HFF) Then
                                ' Gets Amount of this slice
                                DataSZL = S7.GetWordAt(PDU, 31) - 8 ' Skips extra params (ID, Index ...)
                                Done = PDU(26) = &H0
                                Seq_in = PDU(24) ' Slice sequence
                                SZL.Header.LENTHDR = S7.GetWordAt(PDU, 37)
                                SZL.Header.N_DR = S7.GetWordAt(PDU, 39)
                                Array.Copy(PDU, 41, SZL.Data, Offset, DataSZL)
                                '                                SZL.Copy(PDU, 41, Offset, DataSZL);
                                Offset += DataSZL
                                SZL.Header.LENTHDR += SZL.Header.LENTHDR
                            Else
                                _LastError = errCliInvalidPlcAnswer
                            End If
                        Else
                            _LastError = errIsoInvalidPDU
                        End If
                    Else

                        If Length > 32 Then ' the minimum expected
                            If S7.GetWordAt(PDU, 27) = 0 AndAlso PDU(29) = CByte(&HFF) Then
                                ' Gets Amount of this slice
                                DataSZL = S7.GetWordAt(PDU, 31)
                                Done = PDU(26) = &H0
                                Seq_in = PDU(24) ' Slice sequence
                                Array.Copy(PDU, 37, SZL.Data, Offset, DataSZL)
                                Offset += DataSZL
                                SZL.Header.LENTHDR += SZL.Header.LENTHDR
                            Else
                                _LastError = errCliInvalidPlcAnswer
                            End If
                        Else
                            _LastError = errIsoInvalidPDU
                        End If
                    End If
                End If

                First = False
            Loop While Not Done AndAlso _LastError = 0

            If _LastError = 0 Then
                Size = SZL.Header.LENTHDR
                Time_ms = Environment.TickCount - Elapsed
            End If

            Return _LastError
        End Function

        Public Function ReadSZLList(ByRef List As S7SZLList, ByRef ItemsCount As Integer) As Integer
            Return errCliFunctionNotImplemented
        End Function

#End Region

#Region "[Control functions]"

        Public Function PlcHotStart() As Integer
            _LastError = 0
            Dim Elapsed = Environment.TickCount
            SendPacket(S7_HOT_START)

            If _LastError = 0 Then
                Dim Length As Integer = RecvIsoPacket()

                If Length > 18 Then ' 18 is the minimum expected
                    If PDU(19) <> pduStart Then
                        _LastError = errCliCannotStartPLC
                    Else

                        If PDU(20) = pduAlreadyStarted Then
                            _LastError = errCliAlreadyRun
                        Else
                            _LastError = errCliCannotStartPLC
                        End If
                    End If
                Else
                    _LastError = errIsoInvalidPDU
                End If
            End If

            If _LastError = 0 Then Time_ms = Environment.TickCount - Elapsed
            Return _LastError
        End Function

        Public Function PlcColdStart() As Integer
            _LastError = 0
            Dim Elapsed = Environment.TickCount
            SendPacket(S7_COLD_START)

            If _LastError = 0 Then
                Dim Length As Integer = RecvIsoPacket()

                If Length > 18 Then ' 18 is the minimum expected
                    If PDU(19) <> pduStart Then
                        _LastError = errCliCannotStartPLC
                    Else

                        If PDU(20) = pduAlreadyStarted Then
                            _LastError = errCliAlreadyRun
                        Else
                            _LastError = errCliCannotStartPLC
                        End If
                    End If
                Else
                    _LastError = errIsoInvalidPDU
                End If
            End If

            If _LastError = 0 Then Time_ms = Environment.TickCount - Elapsed
            Return _LastError
        End Function

        Public Function PlcStop() As Integer
            _LastError = 0
            Dim Elapsed = Environment.TickCount
            SendPacket(S7_STOP)

            If _LastError = 0 Then
                Dim Length As Integer = RecvIsoPacket()

                If Length > 18 Then ' 18 is the minimum expected
                    If PDU(19) <> pduStop Then
                        _LastError = errCliCannotStopPLC
                    Else

                        If PDU(20) = pduAlreadyStopped Then
                            _LastError = errCliAlreadyStop
                        Else
                            _LastError = errCliCannotStopPLC
                        End If
                    End If
                Else
                    _LastError = errIsoInvalidPDU
                End If
            End If

            If _LastError = 0 Then Time_ms = Environment.TickCount - Elapsed
            Return _LastError
        End Function

        Public Function PlcCopyRamToRom(ByVal Timeout As UInteger) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function PlcCompress(ByVal Timeout As UInteger) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function PlcGetStatus(ByRef Status As Integer) As Integer
            _LastError = 0
            Dim Elapsed = Environment.TickCount
            SendPacket(S7_GET_STAT)

            If _LastError = 0 Then
                Dim Length As Integer = RecvIsoPacket()

                If Length > 30 Then ' the minimum expected
                    Dim Result = S7.GetWordAt(PDU, 27)

                    If Result = 0 Then
                        Select Case PDU(44)
                            Case S7CpuStatusUnknown, S7CpuStatusRun, S7CpuStatusStop
                                Status = PDU(44)
                                Exit Select
                            Case Else
                                ' Since RUN status is always 0x08 for all CPUs and CPs, STOP status
                                ' sometime can be coded as 0x03 (especially for old cpu...)
                                Status = S7CpuStatusStop
                                Exit Select
                        End Select
                    Else
                        _LastError = CpuError(Result)
                    End If
                Else
                    _LastError = errIsoInvalidPDU
                End If
            End If

            If _LastError = 0 Then Time_ms = Environment.TickCount - Elapsed
            Return _LastError
        End Function

#End Region

#Region "[Security functions]"
        Public Function SetSessionPassword(ByVal Password As String) As Integer
            Dim pwd As Byte() = {&H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20}
            Dim Length As Integer
            _LastError = 0
            Dim Elapsed = Environment.TickCount
            ' Encodes the Password
            S7.SetCharsAt(pwd, 0, Password)
            pwd(0) = CByte(pwd(0) Xor &H55)
            pwd(1) = CByte(pwd(1) Xor &H55)

            For c = 2 To 8 - 1
                pwd(c) = CByte(pwd(c) Xor &H55 Xor pwd(c - 2))
            Next

            Array.Copy(pwd, 0, S7_SET_PWD, 29, 8)
            ' Sends the telegrem
            SendPacket(S7_SET_PWD)

            If _LastError = 0 Then
                Length = RecvIsoPacket()

                If Length > 32 Then ' the minimum expected
                    Dim Result = S7.GetWordAt(PDU, 27)
                    If Result <> 0 Then _LastError = CpuError(Result)
                Else
                    _LastError = errIsoInvalidPDU
                End If
            End If

            If _LastError = 0 Then Time_ms = Environment.TickCount - Elapsed
            Return _LastError
        End Function

        Public Function ClearSessionPassword() As Integer
            Dim Length As Integer
            _LastError = 0
            Dim Elapsed = Environment.TickCount
            SendPacket(S7_CLR_PWD)

            If _LastError = 0 Then
                Length = RecvIsoPacket()

                If Length > 30 Then ' the minimum expected
                    Dim Result = S7.GetWordAt(PDU, 27)
                    If Result <> 0 Then _LastError = CpuError(Result)
                Else
                    _LastError = errIsoInvalidPDU
                End If
            End If

            Return _LastError
        End Function

        Public Function GetProtection(ByRef Protection As S7Protection) As Integer
            Dim SZL As S7SZL = New S7SZL()
            Dim Size = 256
            SZL.Data = New Byte(Size - 1) {}
            _LastError = ReadSZL(&H232, &H4, SZL, Size)

            If _LastError = 0 Then
                Protection.sch_schal = S7.GetWordAt(SZL.Data, 2)
                Protection.sch_par = S7.GetWordAt(SZL.Data, 4)
                Protection.sch_rel = S7.GetWordAt(SZL.Data, 6)
                Protection.bart_sch = S7.GetWordAt(SZL.Data, 8)
                Protection.anl_sch = S7.GetWordAt(SZL.Data, 10)
            End If

            Return _LastError
        End Function
#End Region

#Region "[Low Level]"

        Public Function IsoExchangeBuffer(ByVal Buffer As Byte(), ByRef Size As Integer) As Integer
            _LastError = 0
            Time_ms = 0
            Dim Elapsed = Environment.TickCount
            Array.Copy(TPKT_ISO, 0, PDU, 0, TPKT_ISO.Length)
            S7.SetWordAt(PDU, 2, Size + TPKT_ISO.Length)

            Try
                Array.Copy(Buffer, 0, PDU, TPKT_ISO.Length, Size)
            Catch
                Return errIsoInvalidPDU
            End Try

            SendPacket(PDU, TPKT_ISO.Length + Size)

            If _LastError = 0 Then
                Dim Length As Integer = RecvIsoPacket()

                If _LastError = 0 Then
                    Array.Copy(PDU, TPKT_ISO.Length, Buffer, 0, Length - TPKT_ISO.Length)
                    Size = Length - TPKT_ISO.Length
                End If
            End If

            If _LastError = 0 Then
                Time_ms = Environment.TickCount - Elapsed
            Else
                Size = 0
            End If

            Return _LastError
        End Function

#End Region

#Region "[Async functions (not implemented)]"

        Public Function AsReadArea(ByVal Area As Integer, ByVal DBNumber As Integer, ByVal Start As Integer, ByVal Amount As Integer, ByVal WordLen As Integer, ByVal Buffer As Byte()) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function AsWriteArea(ByVal Area As Integer, ByVal DBNumber As Integer, ByVal Start As Integer, ByVal Amount As Integer, ByVal WordLen As Integer, ByVal Buffer As Byte()) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function AsDBRead(ByVal DBNumber As Integer, ByVal Start As Integer, ByVal Size As Integer, ByVal Buffer As Byte()) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function AsDBWrite(ByVal DBNumber As Integer, ByVal Start As Integer, ByVal Size As Integer, ByVal Buffer As Byte()) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function AsMBRead(ByVal Start As Integer, ByVal Size As Integer, ByVal Buffer As Byte()) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function AsMBWrite(ByVal Start As Integer, ByVal Size As Integer, ByVal Buffer As Byte()) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function AsEBRead(ByVal Start As Integer, ByVal Size As Integer, ByVal Buffer As Byte()) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function AsEBWrite(ByVal Start As Integer, ByVal Size As Integer, ByVal Buffer As Byte()) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function AsABRead(ByVal Start As Integer, ByVal Size As Integer, ByVal Buffer As Byte()) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function AsABWrite(ByVal Start As Integer, ByVal Size As Integer, ByVal Buffer As Byte()) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function AsTMRead(ByVal Start As Integer, ByVal Amount As Integer, ByVal Buffer As UShort()) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function AsTMWrite(ByVal Start As Integer, ByVal Amount As Integer, ByVal Buffer As UShort()) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function AsCTRead(ByVal Start As Integer, ByVal Amount As Integer, ByVal Buffer As UShort()) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function AsCTWrite(ByVal Start As Integer, ByVal Amount As Integer, ByVal Buffer As UShort()) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function AsListBlocksOfType(ByVal BlockType As Integer, ByVal List As UShort()) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function AsReadSZL(ByVal ID As Integer, ByVal Index As Integer, ByRef Data As S7SZL, ByRef Size As Integer) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function AsReadSZLList(ByRef List As S7SZLList, ByRef ItemsCount As Integer) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function AsUpload(ByVal BlockType As Integer, ByVal BlockNum As Integer, ByVal UsrData As Byte(), ByRef Size As Integer) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function AsFullUpload(ByVal BlockType As Integer, ByVal BlockNum As Integer, ByVal UsrData As Byte(), ByRef Size As Integer) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function ASDownload(ByVal BlockNum As Integer, ByVal UsrData As Byte(), ByVal Size As Integer) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function AsPlcCopyRamToRom(ByVal Timeout As UInteger) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function AsPlcCompress(ByVal Timeout As UInteger) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function AsDBGet(ByVal DBNumber As Integer, ByVal UsrData As Byte(), ByRef Size As Integer) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function AsDBFill(ByVal DBNumber As Integer, ByVal FillChar As Integer) As Integer
            Return errCliFunctionNotImplemented
        End Function

        Public Function CheckAsCompletion(ByRef opResult As Integer) As Boolean
            opResult = 0
            Return False
        End Function

        Public Function WaitAsCompletion(ByVal Timeout As Integer) As Integer
            Return errCliFunctionNotImplemented
        End Function

#End Region

#Region "[Info Functions / Properties]"

        Public Function ErrorText(ByVal [Error] As Integer) As String
            Select Case [Error]
                Case 0
                    Return "OK"
                Case errTCPSocketCreation
                    Return "SYS : Error creating the Socket"
                Case errTCPConnectionTimeout
                    Return "TCP : Connection Timeout"
                Case errTCPConnectionFailed
                    Return "TCP : Connection Error"
                Case errTCPReceiveTimeout
                    Return "TCP : Data receive Timeout"
                Case errTCPDataReceive
                    Return "TCP : Error receiving Data"
                Case errTCPSendTimeout
                    Return "TCP : Data send Timeout"
                Case errTCPDataSend
                    Return "TCP : Error sending Data"
                Case errTCPConnectionReset
                    Return "TCP : Connection reset by the Peer"
                Case errTCPNotConnected
                    Return "CLI : Client not connected"
                Case errTCPUnreachableHost
                    Return "TCP : Unreachable host"
                Case errIsoConnect
                    Return "ISO : Connection Error"
                Case errIsoInvalidPDU
                    Return "ISO : Invalid PDU received"
                Case errIsoInvalidDataSize
                    Return "ISO : Invalid Buffer passed to Send/Receive"
                Case errCliNegotiatingPDU
                    Return "CLI : Error in PDU negotiation"
                Case errCliInvalidParams
                    Return "CLI : invalid param(s) supplied"
                Case errCliJobPending
                    Return "CLI : Job pending"
                Case errCliTooManyItems
                    Return "CLI : too may items (>20) in multi read/write"
                Case errCliInvalidWordLen
                    Return "CLI : invalid WordLength"
                Case errCliPartialDataWritten
                    Return "CLI : Partial data written"
                Case errCliSizeOverPDU
                    Return "CPU : total data exceeds the PDU size"
                Case errCliInvalidPlcAnswer
                    Return "CLI : invalid CPU answer"
                Case errCliAddressOutOfRange
                    Return "CPU : Address out of range"
                Case errCliInvalidTransportSize
                    Return "CPU : Invalid Transport size"
                Case errCliWriteDataSizeMismatch
                    Return "CPU : Data size mismatch"
                Case errCliItemNotAvailable
                    Return "CPU : Item not available"
                Case errCliInvalidValue
                    Return "CPU : Invalid value supplied"
                Case errCliCannotStartPLC
                    Return "CPU : Cannot start PLC"
                Case errCliAlreadyRun
                    Return "CPU : PLC already RUN"
                Case errCliCannotStopPLC
                    Return "CPU : Cannot stop PLC"
                Case errCliCannotCopyRamToRom
                    Return "CPU : Cannot copy RAM to ROM"
                Case errCliCannotCompress
                    Return "CPU : Cannot compress"
                Case errCliAlreadyStop
                    Return "CPU : PLC already STOP"
                Case errCliFunNotAvailable
                    Return "CPU : Function not available"
                Case errCliUploadSequenceFailed
                    Return "CPU : Upload sequence failed"
                Case errCliInvalidDataSizeRecvd
                    Return "CLI : Invalid data size received"
                Case errCliInvalidBlockType
                    Return "CLI : Invalid block type"
                Case errCliInvalidBlockNumber
                    Return "CLI : Invalid block number"
                Case errCliInvalidBlockSize
                    Return "CLI : Invalid block size"
                Case errCliNeedPassword
                    Return "CPU : Function not authorized for current protection level"
                Case errCliInvalidPassword
                    Return "CPU : Invalid password"
                Case errCliNoPasswordToSetOrClear
                    Return "CPU : No password to set or clear"
                Case errCliJobTimeout
                    Return "CLI : Job Timeout"
                Case errCliFunctionRefused
                    Return "CLI : function refused by CPU (Unknown error)"
                Case errCliPartialDataRead
                    Return "CLI : Partial data read"
                Case errCliBufferTooSmall
                    Return "CLI : The buffer supplied is too small to accomplish the operation"
                Case errCliDestroying
                    Return "CLI : Cannot perform (destroying)"
                Case errCliInvalidParamNumber
                    Return "CLI : Invalid Param Number"
                Case errCliCannotChangeParam
                    Return "CLI : Cannot change this param now"
                Case errCliFunctionNotImplemented
                    Return "CLI : Function not implemented"
                Case Else
                    Return "CLI : Unknown error (0x" & Convert.ToString([Error], 16) & ")"
            End Select
        End Function

        Public Function LastError() As Integer
            Return _LastError
        End Function

        Public Function RequestedPduLength() As Integer
            Return _PduSizeRequested
        End Function

        Public Function NegotiatedPduLength() As Integer
            Return _PDULength
        End Function

        Public Function ExecTime() As Integer
            Return Time_ms
        End Function

        Public ReadOnly Property ExecutionTime As Integer
            Get
                Return Time_ms
            End Get
        End Property

        Public ReadOnly Property PduSizeNegotiated As Integer
            Get
                Return _PDULength
            End Get
        End Property

        Public Property PduSizeRequested As Integer
            Get
                Return _PduSizeRequested
            End Get
            Set(ByVal value As Integer)
                If value < MinPduSizeToRequest Then value = MinPduSizeToRequest
                If value > MaxPduSizeToRequest Then value = MaxPduSizeToRequest
                _PduSizeRequested = value
            End Set
        End Property

        Public Property PLCPort As Integer
            Get
                Return _PLCPort
            End Get
            Set(ByVal value As Integer)
                _PLCPort = value
            End Set
        End Property

        Public Property ConnTimeout As Integer
            Get
                Return _ConnTimeout
            End Get
            Set(ByVal value As Integer)
                _ConnTimeout = value
            End Set
        End Property

        Public Property RecvTimeout As Integer
            Get
                Return _RecvTimeout
            End Get
            Set(ByVal value As Integer)
                _RecvTimeout = value
            End Set
        End Property

        Public Property SendTimeout As Integer
            Get
                Return _SendTimeout
            End Get
            Set(ByVal value As Integer)
                _SendTimeout = value
            End Set
        End Property

        Public ReadOnly Property Connected As Boolean
            Get
                Return Socket IsNot Nothing AndAlso Socket.Connected
            End Get
        End Property
#End Region

#Region "forcejob"

        Public Class S7Forces
            Public Forces As List(Of ForceJob)
        End Class

        Friend Function GetActiveForces(ByVal forces As List(Of ForceJob), ByVal forceframe As Byte()) As Integer


            ' sending second package only if there are force jobs active 
            SendPacket(forceframe)
            Dim length = RecvIsoPacket()

            Select Case WordFromByteArr(PDU, 27)
                Case &H0

                    ' creating byte [] with length of useful data (first 67 bytes aren't useful data )
                    Dim forceData = New Byte(length - 67 - 1) {}
                    ' copy pdu to other byte[] and remove the unused data 
                    Array.Copy(PDU, 67, forceData, 0, length - 67)
                    ' check array transition definition > value's 
                    Dim splitDefData = New Byte() {&H0, &H9, &H0}
                    Dim Splitposition = 0
                    Dim x = 0

                    While x < forceData.Length - 3
                        ' checking when the definitions go to data (the data starts with split definition data and the amount of bytes before should always be a plural of 6)
                        If forceData(x) = splitDefData(0) AndAlso forceData(x + 1) = splitDefData(1) AndAlso forceData(x + 2) = splitDefData(2) AndAlso x Mod 6 = 0 Then
                            Splitposition = x
                            Exit While
                        End If

                        x = x + 6
                    End While
                    ' calculating amount of forces 
                    Dim amountForces As Integer = Splitposition / 6
                    ' setting first byte from data
                    Dim dataposition = Splitposition

                    For x = 0 To amountForces - 1
                        Dim force As ForceJob
                        force = New ForceJob
                        With force
                            ' bit value
                            .BitAdress = forceData(1 + 6 * x)
                            ' byte value
                            .ByteAdress = forceData(4 + 6 * x) * 256 + forceData(5 + 6 * x)
                        End With

                        ' foce identity
                        Select Case forceData(0 + 6 * x)
                            Case &H0
                                force.ForceType = "M"
                            Case &H1
                                force.ForceType = "MB"
                                force.BitAdress = Nothing
                            Case &H2
                                force.ForceType = "MW"
                                force.BitAdress = Nothing
                            Case &H3
                                force.ForceType = "MD"
                                force.BitAdress = Nothing
                            Case &H10
                                force.ForceType = "I"
                            Case &H11
                                force.ForceType = "IB"
                                force.BitAdress = Nothing
                            Case &H12
                                force.ForceType = "IW"
                                force.BitAdress = Nothing
                            Case &H13
                                force.ForceType = "ID"
                                force.BitAdress = Nothing
                            Case &H20
                                force.ForceType = "Q"
                            Case &H21
                                force.ForceType = "QB"
                                force.BitAdress = Nothing
                            Case &H22
                                force.ForceType = "QW"
                                force.BitAdress = Nothing
                            Case &H23
                                force.ForceType = "QD"

                                ' if you get this code You can add it in the list above.
                                force.BitAdress = Nothing
                            Case Else
                                force.ForceType = forceData(0 + 6 * x).ToString() & " unknown"
                        End Select

                        ' setting force value depending on the data length
                        Select Case forceData(dataposition + 3)' Data length from force
                            Case &H1
                                force.ForceValue = forceData(dataposition + 4)
                            Case &H2
                                force.ForceValue = WordFromByteArr(forceData, dataposition + 4)
                            Case &H4
                                force.ForceValue = DoubleFromByteArr(forceData, dataposition + 4)
                            Case Else
                        End Select

                        ' calculating when the next force start 

                        Dim nextForce = &H4 + forceData(dataposition + 3)

                        If nextForce < 6 Then
                            nextForce = 6
                        End If

                        dataposition += nextForce
                        ' adding force to list 
                        forces.Add(force)
                    Next

                Case Else
                    _LastError = errTCPDataReceive
            End Select

            Return _LastError
        End Function

        Public Function GetForceValues300(ByRef forces As S7Forces) As Integer
            _LastError = 0
            Dim Elapsed = Environment.TickCount
            Dim forcedValues As List(Of ForceJob) = New List(Of ForceJob)()
            SendPacket(S7_FORCE_VAL1)
            Dim Length = RecvIsoPacket()

            ' when response is 45 there are no force jobs active or no correct response from plc

            Select Case WordFromByteArr(PDU, 27)
                Case &H0 ' no error code

                    If WordFromByteArr(PDU, 31) >= 16 Then
                        _LastError = GetActiveForces(forcedValues, S7_FORCE_VAL300)
                    End If

                Case Else
                    _LastError = errTCPDataReceive
            End Select

            forces.Forces = forcedValues
            Time_ms = Environment.TickCount - Elapsed
            Return _LastError
        End Function

        Public Function GetForceValues400(ByRef forces As S7Forces) As Integer
            _LastError = 0
            Dim Elapsed = Environment.TickCount
            Dim forcedValues As List(Of ForceJob) = New List(Of ForceJob)()
            SendPacket(S7_FORCE_VAL1)
            Dim Length = RecvIsoPacket()

            ' when response is 45 there are no force jobs active or no correct response from PLC

            Select Case WordFromByteArr(PDU, 27)
                Case &H0

                    If WordFromByteArr(PDU, 31) >= 12 Then
                        _LastError = GetActiveForces(forcedValues, S7_FORCE_VAL400)
                    End If

                Case Else
                    _LastError = errTCPDataReceive
            End Select

            forces.Forces = forcedValues
            Time_ms = Environment.TickCount - Elapsed
            Return _LastError
        End Function

        Public Function WordFromByteArr(ByVal data As Byte(), ByVal position As Integer) As Integer
            Dim result = Convert.ToInt32((data(position) << 8) + data(position + 1))
            Return result
        End Function

        Public Function DoubleFromByteArr(ByVal data As Byte(), ByVal position As Integer) As Integer
            Dim result = Convert.ToInt32((data(position) << 24) + (data(position + 1) << 16) + (data(position + 2) << 8) + data(position + 3))
            Return result
        End Function


        ' S7 Get Force Values frame 1
        Private S7_FORCE_VAL1 As Byte() = {&H3, &H0, &H0, &H3D, &H2, &HF0, &H80, &H32, &H7, &H0, &H0, &H7, &H0, &H0, &HC, &H0, &H20, &H0, &H1, &H12, &H8, &H12, &H41, &H10, &H0, &H0, &H0, &H0, &H0, &HFF, &H9, &H0, &H1C, &H0, &H14, &H0, &H4, &H0, &H0, &H0, &H0, &H0, &H1, &H0, &H0, &H0, &H1, &H0, &H1, &H0, &H1, &H0, &H1, &H0, &H1, &H0, &H0, &H0, &H0, &H0, &H0}

        ' S7 Get Force Values frame 2 (300 series )
        Private S7_FORCE_VAL300 As Byte() = {&H3, &H0, &H0, &H3B, &H2, &HF0, &H80, &H32, &H7, &H0, &H0, &HC, &H0, &H0, &HC, &H0, &H1E, &H0, &H1, &H12, &H8, &H12, &H41, &H11, &H0, &H0, &H0, &H0, &H0, &HFF, &H9, &H0, &H1A, &H0, &H14, &H0, &H2, &H0, &H0, &H0, &H0, &H0, &H1, &H0, &H0, &H0, &H1, &H0, &H1, &H0, &H1, &H0, &H1, &H0, &H1, &H0, &H0, &H9, &H3}

        ' S7 Get Force Values frame 2 (400 series )
        Private S7_FORCE_VAL400 As Byte() = {&H3, &H0, &H0, &H3B, &H2, &HF0, &H80, &H32, &H7, &H0, &H0, &HC, &H0, &H0, &HC, &H0, &H1E, &H0, &H1, &H12, &H8, &H12, &H41, &H11, &H0, &H0, &H0, &H0, &H0, &HFF, &H9, &H0, &H1A, &H0, &H14, &H0, &H2, &H0, &H0, &H0, &H0, &H0, &H1, &H0, &H0, &H0, &H1, &H0, &H1, &H0, &H1, &H0, &H1, &H0, &H1, &H0, &H0, &H9, &H5}

        Public Class ForceJob
            Public ReadOnly Property FullAdress As String
                Get

                    If BitAdress Is Nothing Then
                        Return $"{ForceType} {ByteAdress}"
                    Else
                        Return $"{ForceType} {ByteAdress}.{BitAdress}"
                    End If
                End Get
            End Property

            Public Property ForceValue As Integer
            Public Property ForceType As String
            Public Property ByteAdress As Integer
            Public Property BitAdress As Integer?
            Public Property Symbol As String
            Public Property Comment As String
        End Class
#End Region

#Region "CommentedForce"
        Public Class CommentForces
            ' Only symbol table's with.seq extension
            Public Function AddForceComments(ByVal filepath As String, ByVal actualForces As List(Of ForceJob)) As List(Of ForceJob)
                If Equals(Path.GetExtension(filepath).ToLower(), ".seq") Then
                    Dim SymbolTableDataText = ReadSymbolTable(filepath)

                    If SymbolTableDataText.Length >= 1 Then
                        Dim SymbolTableDataList = ConvertDataArrToList(SymbolTableDataText)
                        Dim CommentedForceList = AddCommentToForce(actualForces, SymbolTableDataList)
                        Return CommentedForceList
                    End If
                End If

                Return ErrorSymbTableProces(actualForces)
            End Function

            Private Function AddCommentToForce(ByVal forceringen As List(Of ForceJob), ByVal symbolTable As List(Of SymbolTableRecord)) As List(Of ForceJob)
                Dim commentedforces As List(Of ForceJob) = New List(Of ForceJob)()

                For Each force In forceringen
                    Dim found = symbolTable.Where(Function(s) Equals(s.Address, force.FullAdress)).FirstOrDefault()
                    Dim commentedforce As ForceJob = New ForceJob()
                    commentedforce = force

                    If found IsNot Nothing Then
                        commentedforce.Symbol = found.Symbol
                        commentedforce.Comment = found.Comment
                    Else
                        commentedforce.Symbol = "NOT SET"
                        commentedforce.Comment = "not in variable table"
                    End If

                    commentedforces.Add(commentedforce)
                Next

                Return commentedforces
            End Function

            Private Function ConvertDataArrToList(ByVal text As String()) As List(Of SymbolTableRecord)
                Dim Symbollist As List(Of SymbolTableRecord) = New List(Of SymbolTableRecord)()

                For Each line In text
                    Dim temp As SymbolTableRecord = New SymbolTableRecord()
                    Dim splited = New String(9) {}
                    splited = line.Split(Microsoft.VisualBasic.Strings.ChrW(9))
                    temp.Address = splited(1)
                    temp.Symbol = splited(2)
                    temp.Comment = splited(3)
                    Symbollist.Add(temp)
                Next

                Return Symbollist
            End Function

            Private Function ReadSymbolTable(ByVal Filepath As String) As String()
                Dim lines = File.ReadAllLines(Filepath)
                Return lines
            End Function

            Private Function ErrorSymbTableProces(ByVal actualForces As List(Of ForceJob)) As List(Of ForceJob)
                Dim errorForceTable = actualForces

                For Each forcerecord In errorForceTable
                    forcerecord.Comment = "Force Table could not be processed"
                    forcerecord.Symbol = "ERROR"
                Next

                Return errorForceTable
            End Function
        End Class

        Public Class SymbolTableRecord
            Public Property Symbol As String
            Public Property Address As String
            Public Property Comment As String
        End Class


#End Region

#Region "Sinumerik Client Functions"

#Region "S7DriveES Client Functions"
        ' The following functions were only tested with Sinumerik 840D Solution Line (no Power Line support)
        ' Connection to Sinumerik-Drive Main CU: use slot number 9
        ' Connection to Sinumerik-Drive NX-Extensions: slot number usually starts with 13 (check via starter for individual configuration)
#Region "[S7 DriveES Telegrams]"
        ' S7 DriveES Read/Write Request Header (contains also ISO Header and COTP Header)
        ' WR area
        Private S7_DrvRW As Byte() = {&H3, &H0, &H0, &H1F, &H2, &HF0, &H80, &H32, &H1, &H0, &H0, &H5, &H0, &H0, &HE, &H0, &H0, &H4, &H1, &H12, &HA, &HA2, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H4, &H0, &H0} ' 31-35 bytes
        ' Telegram Length (Data Size + 31 or 35)
        ' COTP (see above for info)
        ' S7 Protocol ID 
        ' Job Type
        ' Redundancy identification
        ' PDU Reference
        ' Parameters Length
        ' Data Length = Size(bytes) + 4      
        ' Function 4 Read Var, 5 Write Var  
        ' Items count
        ' Var spec.
        ' Length of remaining bytes
        ' Syntax ID 
        ' Empty --> Parameter Type                       
        ' Empty --> Number of Rows                          
        ' Empty --> Number of DriveObject          
        ' Empty --> Parameter Number                           
        ' Empty --> Parameter Index                     
        ' Reserved 
        ' Transport size
        ' Data Length * 8 (if not bit or timer or counter) 

        ' S7 Drv Variable MultiRead Header
        Private S7Drv_MRD_HEADER As Byte() = {&H3, &H0, &H0, &H1F, &H2, &HF0, &H80, &H32, &H1, &H0, &H0, &H5, &H0, &H0, &HE, &H0, &H0, &H4, &H1}       ' Telegram Length (Data Size + 31 or 35)
        ' COTP (see above for info)
        ' S7 Protocol ID 
        ' Job Type
        ' Redundancy identification
        ' PDU Reference
        ' Parameters Length
        ' Data Length = Size(bytes) + 4      
        ' Function 4 Read Var, 5 Write Var  
        ' Items count

        ' S7 Drv Variable MultiRead Item
        Private S7Drv_MRD_ITEM As Byte() = {&H12, &HA, &HA2, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0}            ' Var spec.
        ' Length of remaining bytes
        ' Syntax ID 
        ' Empty --> Parameter Type                       
        ' Empty --> Number of Rows                          
        ' Empty --> Number of DriveObject          
        ' Empty --> Parameter Number                           
        ' Empty --> Parameter Index                    

        ' S7 Drv Variable MultiWrite Header
        Private S7Drv_MWR_HEADER As Byte() = {&H3, &H0, &H0, &H1F, &H2, &HF0, &H80, &H32, &H1, &H0, &H0, &H5, &H0, &H0, &HE, &H0, &H0, &H5, &H1}       ' Telegram Length (Data Size + 31 or 35)
        ' COTP (see above for info)
        ' S7 Protocol ID 
        ' Job Type
        ' Redundancy identification
        ' PDU Reference
        ' Parameters Length
        ' Data Length = Size(bytes) + 4      
        ' Function 4 Read Var, 5 Write Var  
        ' Items count

        ' S7 Drv Variable MultiWrite Item
        Private S7Drv_MWR_PARAM As Byte() = {&H12, &HA, &HA2, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0}            ' Var spec.
        ' Length of remaining bytes
        ' Syntax ID 
        ' Empty --> Parameter Type                       
        ' Empty --> Number of Rows                          
        ' Empty --> Number of DriveObject          
        ' Empty --> Parameter Number                           
        ' Empty --> Parameter Index                    
#End Region


        ''' <summary>
        ''' Data I/O main function: Read Drive Area
        ''' Function reads one drive parameter and defined amount of indizes of this parameter
        ''' </summary>
        Public Function ReadDrvArea(ByVal DONumber As Integer, ByVal ParameterNumber As Integer, ByVal Start As Integer, ByVal Amount As Integer, ByVal WordLen As Integer, ByVal Buffer As Byte()) As Integer
            Dim BytesRead = 0
            Return ReadDrvArea(DONumber, ParameterNumber, Start, Amount, WordLen, Buffer, BytesRead)
        End Function

        Public Function ReadDrvArea(ByVal DONumber As Integer, ByVal ParameterNumber As Integer, ByVal Start As Integer, ByVal Amount As Integer, ByVal WordLen As Integer, ByVal Buffer As Byte(), ByRef BytesRead As Integer) As Integer
            ' Variables
            Dim NumElements As Integer
            Dim MaxElements As Integer
            Dim TotElements As Integer
            Dim SizeRequested As Integer
            Dim Length As Integer
            Dim Offset = 0
            Dim WordSize = 1
            _LastError = 0
            Time_ms = 0
            Dim Elapsed = Environment.TickCount
            ' Calc Word size          
            WordSize = DrvDataSizeByte(WordLen)
            If WordSize = 0 Then Return errCliInvalidWordLen
            MaxElements = (_PDULength - 18) / WordSize ' 18 = Reply telegram header
            TotElements = Amount

            While TotElements > 0 AndAlso _LastError = 0
                NumElements = TotElements
                If NumElements > MaxElements Then NumElements = MaxElements
                SizeRequested = NumElements * WordSize

                '$7+: Setup the telegram - New Implementation for Drive Parameters
                Array.Copy(S7_DrvRW, 0, PDU, 0, Size_RD)
                'set DriveParameters
                S7.SetWordAt(PDU, 23, NumElements)
                S7.SetWordAt(PDU, 25, DONumber)
                S7.SetWordAt(PDU, 27, ParameterNumber)
                S7.SetWordAt(PDU, 29, Start)
                PDU(22) = CByte(WordLen)
                SendPacket(PDU, Size_RD)

                If _LastError = 0 Then
                    Length = RecvIsoPacket()

                    If _LastError = 0 Then
                        If Length < 25 Then
                            _LastError = errIsoInvalidDataSize
                        Else

                            If PDU(21) <> &HFF Then
                                _LastError = CpuError(PDU(21))
                            Else
                                Array.Copy(PDU, 25, Buffer, Offset, SizeRequested)
                                Offset += SizeRequested
                            End If
                        End If
                    End If
                End If

                TotElements -= NumElements
                Start += NumElements
            End While

            If _LastError = 0 Then
                BytesRead = Offset
                Time_ms = Environment.TickCount - Elapsed
            Else
                BytesRead = 0
            End If

            Return _LastError
        End Function

        ''' <summary>
        ''' Data I/O main function: Read Multiple Drive Values
        ''' </summary>
        Public Function ReadMultiDrvVars(ByVal Items As S7DrvDataItem(), ByVal ItemsCount As Integer) As Integer
            Dim Offset As Integer
            Dim Length As Integer
            Dim ItemSize As Integer
            Dim S7DrvItem = New Byte(11) {}
            Dim S7DrvItemRead = New Byte(1023) {}
            _LastError = 0
            Time_ms = 0
            Dim Elapsed = Environment.TickCount

            ' Checks items
            If ItemsCount > MaxVars Then Return errCliTooManyItems

            ' Fills Header
            Array.Copy(S7Drv_MRD_HEADER, 0, PDU, 0, S7Drv_MRD_HEADER.Length)
            S7.SetWordAt(PDU, 13, ItemsCount * S7DrvItem.Length + 2)
            PDU(18) = CByte(ItemsCount)
            ' Fills the Items
            Offset = 19

            For c = 0 To ItemsCount - 1
                Array.Copy(S7Drv_MRD_ITEM, S7DrvItem, S7DrvItem.Length)
                S7DrvItem(3) = CByte(Items(c).WordLen)
                S7.SetWordAt(S7DrvItem, 4, Items(c).Amount)
                S7.SetWordAt(S7DrvItem, 6, Items(c).DONumber)
                S7.SetWordAt(S7DrvItem, 8, Items(c).ParameterNumber)
                S7.SetWordAt(S7DrvItem, 10, Items(c).Start)
                Array.Copy(S7DrvItem, 0, PDU, Offset, S7DrvItem.Length)
                Offset += S7DrvItem.Length
            Next

            If Offset > _PDULength Then Return errCliSizeOverPDU
            S7.SetWordAt(PDU, 2, Offset) ' Whole size
            SendPacket(PDU, Offset)
            If _LastError <> 0 Then Return _LastError
            ' Get Answer
            Length = RecvIsoPacket()
            If _LastError <> 0 Then Return _LastError
            ' Check ISO Length
            If Length < 22 Then
                _LastError = errIsoInvalidPDU ' PDU too Small
                Return _LastError
            End If
            ' Check Global Operation Result
            _LastError = CpuError(S7.GetWordAt(PDU, 17))
            If _LastError <> 0 Then Return _LastError
            ' Get true ItemsCount
            Dim ItemsRead As Integer = S7.GetByteAt(PDU, 20)

            If ItemsRead <> ItemsCount OrElse ItemsRead > MaxVars Then
                _LastError = errCliInvalidPlcAnswer
                Return _LastError
            End If
            ' Get Data
            Offset = 21

            For c = 0 To ItemsCount - 1
                ' Get the Item
                Array.Copy(PDU, Offset, S7DrvItemRead, 0, Length - Offset)

                If S7DrvItemRead(0) = &HFF Then
                    ItemSize = S7.GetWordAt(S7DrvItemRead, 2)
                    If S7DrvItemRead(1) <> TS_ResOctet AndAlso S7DrvItemRead(1) <> TS_ResReal AndAlso S7DrvItemRead(1) <> TS_ResBit Then ItemSize = ItemSize >> 3
                    Marshal.Copy(S7DrvItemRead, 4, Items(c).pData, ItemSize)
                    Items(c).Result = 0
                    If ItemSize Mod 2 <> 0 Then ItemSize += 1 ' Odd size are rounded
                    Offset = Offset + 4 + ItemSize
                Else
                    Items(c).Result = CpuError(S7DrvItemRead(0))
                    Offset += 4 ' Skip the Item header                           
                End If
            Next

            Time_ms = Environment.TickCount - Elapsed
            Return _LastError
        End Function

        ''' <summary>
        ''' Data I/O main function: Write Multiple Drive Values
        ''' </summary>
        ''' <returns></returns>
        Public Function WriteMultiDrvVars(ByVal Items As S7DrvDataItem(), ByVal ItemsCount As Integer) As Integer
            Dim Offset As Integer
            Dim ParLength As Integer
            Dim DataLength As Integer
            Dim ItemDataSize = 4 'default
            Dim S7DrvParItem = New Byte(S7Drv_MWR_PARAM.Length - 1) {}
            Dim S7DrvDataItem = New Byte(1023) {}
            _LastError = 0
            Time_ms = 0
            Dim Elapsed = Environment.TickCount

            ' Checks items
            If ItemsCount > MaxVars Then Return errCliTooManyItems
            ' Fills Header
            Array.Copy(S7Drv_MWR_HEADER, 0, PDU, 0, S7Drv_MWR_HEADER.Length)
            ParLength = ItemsCount * S7Drv_MWR_PARAM.Length + 2
            S7.SetWordAt(PDU, 13, ParLength)
            PDU(18) = CByte(ItemsCount)
            ' Fills Params
            Offset = S7Drv_MWR_HEADER.Length

            For c = 0 To ItemsCount - 1
                Array.Copy(S7Drv_MWR_PARAM, 0, S7DrvParItem, 0, S7Drv_MWR_PARAM.Length)
                S7DrvParItem(3) = CByte(Items(c).WordLen)
                S7.SetWordAt(S7DrvParItem, 4, Items(c).Amount)
                S7.SetWordAt(S7DrvParItem, 6, Items(c).DONumber)
                S7.SetWordAt(S7DrvParItem, 8, Items(c).ParameterNumber)
                S7.SetWordAt(S7DrvParItem, 10, Items(c).Start)
                Array.Copy(S7DrvParItem, 0, PDU, Offset, S7DrvParItem.Length)
                Offset += S7Drv_MWR_PARAM.Length
            Next
            ' Fills Data
            DataLength = 0

            For c = 0 To ItemsCount - 1
                S7DrvDataItem(0) = &H0

                Select Case Items(c).WordLen
                    Case S7DrvConsts.S7WLReal
                        S7DrvDataItem(1) = TS_ResReal      ' Real
                        ItemDataSize = 4
                    Case S7DrvConsts.S7WLDWord            ' DWord
                        S7DrvDataItem(1) = TS_ResByte
                        ItemDataSize = 4
                    Case S7DrvConsts.S7WLDInt            ' DWord
                        S7DrvDataItem(1) = TS_ResByte
                        ItemDataSize = 4
                    Case Else
                        S7DrvDataItem(1) = TS_ResByte     ' byte/word/int etc.
                        ItemDataSize = 2
                End Select

                If S7DrvDataItem(1) <> TS_ResOctet AndAlso S7DrvDataItem(1) <> TS_ResBit AndAlso S7DrvDataItem(1) <> TS_ResReal Then
                    S7.SetWordAt(S7DrvDataItem, 2, ItemDataSize * 8)
                Else
                    S7.SetWordAt(S7DrvDataItem, 2, ItemDataSize)
                End If

                Marshal.Copy(Items(c).pData, S7DrvDataItem, 4, ItemDataSize)

                If ItemDataSize Mod 2 <> 0 Then
                    S7DrvDataItem(ItemDataSize + 4) = &H0
                    ItemDataSize += 1
                End If

                Array.Copy(S7DrvDataItem, 0, PDU, Offset, ItemDataSize + 4)
                Offset = Offset + ItemDataSize + 4
                DataLength = DataLength + ItemDataSize + 4
            Next

            ' Checks the size
            If Offset > _PDULength Then Return errCliSizeOverPDU
            S7.SetWordAt(PDU, 2, Offset) ' Whole size
            S7.SetWordAt(PDU, 15, DataLength) ' Whole size
            SendPacket(PDU, Offset)
            RecvIsoPacket()

            If _LastError = 0 Then
                ' Check Global Operation Result
                _LastError = CpuError(S7.GetWordAt(PDU, 17))
                If _LastError <> 0 Then Return _LastError
                ' Get true ItemsCount
                Dim ItemsWritten As Integer = S7.GetByteAt(PDU, 20)

                If ItemsWritten <> ItemsCount OrElse ItemsWritten > MaxVars Then
                    _LastError = errCliInvalidPlcAnswer
                    Return _LastError
                End If

                For c = 0 To ItemsCount - 1

                    If PDU(c + 21) = &HFF Then
                        Items(c).Result = 0
                    Else
                        Items(c).Result = CpuError(PDU(c + 21))
                    End If
                Next

                Time_ms = Environment.TickCount - Elapsed
            End If

            Return _LastError
        End Function


        ' S7 Drive Data Item
        Public Structure S7DrvDataItem
            Public DONumber As Integer
            Public WordLen As Integer
            Public Result As Integer
            Public ParameterNumber As Integer
            Public Start As Integer
            Public Amount As Integer
            Public pData As IntPtr
        End Structure

        ' S7 Drive Connection
        Public Function DrvConnectTo(ByVal Address As String) As Integer
            Dim RemoteTSAP As UShort = (ConnType << 8) + 0 * &H20 + 9
            ' testen
            SetConnectionParams(Address, &H100, RemoteTSAP)
            Return Connect()
        End Function
        ' S7 Drive Connection with Slot
        Public Function DrvConnectTo(ByVal Address As String, ByVal Slot As Integer) As Integer
            Dim RemoteTSAP As UShort = (ConnType << 8) + 0 * &H20 + Slot
            ' testen
            SetConnectionParams(Address, &H100, RemoteTSAP)
            Return Connect()
        End Function
        ' S7 Drive Connection with Rack & Slot
        Public Function DrvConnectTo(ByVal Address As String, ByVal Rack As Integer, ByVal Slot As Integer) As Integer
            Dim RemoteTSAP As UShort = (ConnType << 8) + Rack * &H20 + Slot
            ' testen
            SetConnectionParams(Address, &H100, RemoteTSAP)
            Return Connect()
        End Function


#End Region

#Region "S7Nck Client Functions"
        ' Connection to Sinumerik NC: use slot number 3
#Region "[S7 NckTelegrams]"
        ' Size NCK-Protocoll not equal to S7-Any-Protocoll
        Public Size_NckRD As Integer = 29 ' Header Size when Reading 
        Public Size_NckWR As Integer = 33 ' Header Size when Writing

        ' S7 NCK Read/Write Request Header (contains also ISO Header and COTP Header)
        ' WR area
        Private S7_NckRW As Byte() = {&H3, &H0, &H0, &H1D, &H2, &HF0, &H80, &H32, &H1, &H0, &H0, &H5, &H0, &H0, &HC, &H0, &H0, &H4, &H1, &H12, &H8, &H82, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H4, &H0, &H0} ' 31-35 bytes
        ' Telegram Length (Data Size + 29 or 33)
        ' COTP (see above for info)
        ' S7 Protocol ID 
        ' Job Type
        ' Redundancy identification
        ' PDU Reference
        ' Parameters Length
        ' Data Length = Size(bytes) + 4      
        ' Function 4 Read Var, 5 Write Var  
        ' Items count
        ' Var spec.
        ' Length of remaining bytes
        ' Syntax ID 
        ' Empty --> NCK Area and Unit                     
        ' Empty --> Parameter Number                          
        ' Empty --> Parameter Index          
        ' Empty --> NCK Module (See NCVar-Selector for help)                           
        ' Empty --> Number of Rows                     
        ' Reserved 
        ' Transport size
        ' Data Length * 8 (if not bit or timer or counter) 

        ' S7 Nck Variable MultiRead Header
        Private S7Nck_MRD_HEADER As Byte() = {&H3, &H0, &H0, &H1D, &H2, &HF0, &H80, &H32, &H1, &H0, &H0, &H5, &H0, &H0, &HC, &H0, &H0, &H4, &H1}       ' Telegram Length (Data Size + 29 or 33)
        ' COTP (see above for info)
        ' S7 Protocol ID 
        ' Job Type
        ' Redundancy identification
        ' PDU Reference
        ' Parameters Length
        ' Data Length = Size(bytes) + 4      
        ' Function 4 Read Var, 5 Write Var  
        ' Items count

        ' S7 Nck Variable MultiRead Item
        Private S7Nck_MRD_ITEM As Byte() = {&H12, &H8, &H82, &H0, &H0, &H0, &H0, &H0, &H0, &H0}            ' Var spec.
        ' Length of remaining bytes
        ' Syntax ID 
        ' Empty --> NCK Area and Unit                     
        ' Empty --> Parameter Number                          
        ' Empty --> Parameter Index          
        ' Empty --> NCK Module (See NCVar-Selector for help)                           
        ' Empty --> Number of Rows                     

        ' S7 Nck Variable MultiWrite Header
        Private S7Nck_MWR_HEADER As Byte() = {&H3, &H0, &H0, &H1D, &H2, &HF0, &H80, &H32, &H1, &H0, &H0, &H5, &H0, &H0, &HC, &H0, &H0, &H5, &H1}       ' Telegram Length (Data Size + 29 or 33)
        ' COTP (see above for info)
        ' S7 Protocol ID 
        ' Job Type
        ' Redundancy identification
        ' PDU Reference
        ' Parameters Length
        ' Data Length = Size(bytes) + 4      
        ' Function 4 Read Var, 5 Write Var  
        ' Items count

        ' S7 Nck Variable MultiWrite Item
        Private S7Nck_MWR_PARAM As Byte() = {&H12, &H8, &H82, &H0, &H0, &H0, &H0, &H0, &H0, &H0}            ' Var spec.
        ' Length of remaining bytes
        ' Syntax ID 
        ' Empty --> NCK Area and Unit                     
        ' Empty --> Parameter Number                          
        ' Empty --> Parameter Index          
        ' Empty --> NCK Module (See NCVar-Selector for help)                           
        ' Empty --> Number of Rows                     
#End Region

        ''' <summary>
        ''' Data I/O main function: Read Nck Area
        ''' Function reads one Nck parameter and defined amount of indizes of this parameter
        ''' </summary>

        Public Function ReadNckArea(ByVal NckArea As Integer, ByVal NckUnit As Integer, ByVal NckModule As Integer, ByVal ParameterNumber As Integer, ByVal Start As Integer, ByVal Amount As Integer, ByVal WordLen As Integer, ByVal Buffer As Byte()) As Integer
            Dim BytesRead = 0
            Return ReadNckArea(NckArea, NckUnit, NckModule, ParameterNumber, Start, Amount, WordLen, Buffer, BytesRead)
        End Function

        Public Function ReadNckArea(ByVal NckArea As Integer, ByVal NckUnit As Integer, ByVal NckModule As Integer, ByVal ParameterNumber As Integer, ByVal Start As Integer, ByVal Amount As Integer, ByVal WordLen As Integer, ByVal Buffer As Byte(), ByRef BytesRead As Integer) As Integer
            ' Variables
            Dim NumElements As Integer
            Dim MaxElements As Integer
            Dim TotElements As Integer
            Dim SizeRequested As Integer
            Dim Length As Integer
            Dim Offset = 0
            Dim WordSize = 1
            _LastError = 0
            Time_ms = 0
            Dim Elapsed = Environment.TickCount
            ' Calc Word size 
            'New Definition used: NCKDataSizeByte         
            WordSize = NckDataSizeByte(WordLen)
            If WordSize = 0 Then Return errCliInvalidWordLen
            MaxElements = (_PDULength - 18) / WordSize ' 18 = Reply telegram header
            TotElements = Amount

            While TotElements > 0 AndAlso _LastError = 0
                NumElements = TotElements
                If NumElements > MaxElements Then NumElements = MaxElements
                SizeRequested = NumElements * WordSize
                'Setup the telegram - New Implementation for NCK Parameters
                Array.Copy(S7_NckRW, 0, PDU, 0, Size_NckRD)
                'set NckParameters        
                NckArea = NckArea << 4
                PDU(22) = CByte(NckArea + NckUnit)
                S7.SetWordAt(PDU, 23, ParameterNumber)
                S7.SetWordAt(PDU, 25, Start)
                PDU(27) = CByte(NckModule)
                PDU(28) = CByte(NumElements)
                SendPacket(PDU, Size_NckRD)

                If _LastError = 0 Then
                    Length = RecvIsoPacket()

                    If _LastError = 0 Then
                        If Length < 25 Then
                            _LastError = errIsoInvalidDataSize
                        Else

                            If PDU(21) <> &HFF Then
                                _LastError = CpuError(PDU(21))
                            Else
                                Array.Copy(PDU, 25, Buffer, Offset, SizeRequested)
                                Offset += SizeRequested
                            End If
                        End If
                    End If
                End If

                TotElements -= NumElements
                Start += NumElements
            End While

            If _LastError = 0 Then
                BytesRead = Offset
                Time_ms = Environment.TickCount - Elapsed
            Else
                BytesRead = 0
            End If

            Return _LastError
        End Function

        ''' <summary>
        ''' Data I/O main function: Read Multiple Nck Values
        ''' </summary>
        ''' <returns></returns>
        Public Function ReadMultiNckVars(ByVal Items As S7NckDataItem(), ByVal ItemsCount As Integer) As Integer
            Dim Offset As Integer
            Dim Length As Integer
            Dim ItemSize As Integer
            Dim S7NckItem = New Byte(9) {}
            Dim S7NckItemRead = New Byte(1023) {}
            _LastError = 0
            Time_ms = 0
            Dim Elapsed = Environment.TickCount

            ' Checks items
            If ItemsCount > MaxVars Then Return errCliTooManyItems

            ' Fills Header
            Array.Copy(S7Nck_MRD_HEADER, 0, PDU, 0, S7Nck_MRD_HEADER.Length)
            S7.SetWordAt(PDU, 13, ItemsCount * S7NckItem.Length + 2)
            PDU(18) = CByte(ItemsCount)
            ' Fills the Items
            Offset = 19

            For c = 0 To ItemsCount - 1
                Array.Copy(S7Nck_MRD_ITEM, S7NckItem, S7NckItem.Length)
                Dim NckArea = Items(c).NckArea << 4
                S7NckItem(3) = CByte(NckArea + Items(c).NckUnit)
                S7.SetWordAt(S7NckItem, 4, Items(c).ParameterNumber)
                S7.SetWordAt(S7NckItem, 6, Items(c).Start)
                S7.SetByteAt(S7NckItem, 8, Items(c).NckModule)
                S7.SetByteAt(S7NckItem, 9, Items(c).Amount)
                Array.Copy(S7NckItem, 0, PDU, Offset, S7NckItem.Length)
                Offset += S7NckItem.Length
            Next

            If Offset > _PDULength Then Return errCliSizeOverPDU
            S7.SetWordAt(PDU, 2, Offset) ' Whole size
            SendPacket(PDU, Offset)
            If _LastError <> 0 Then Return _LastError
            ' Get Answer
            Length = RecvIsoPacket()
            If _LastError <> 0 Then Return _LastError
            ' Check ISO Length
            If Length < 22 Then
                _LastError = errIsoInvalidPDU ' PDU too Small
                Return _LastError
            End If
            ' Check Global Operation Result
            _LastError = CpuError(S7.GetWordAt(PDU, 17))
            If _LastError <> 0 Then Return _LastError
            ' Get true ItemsCount
            Dim ItemsRead As Integer = S7.GetByteAt(PDU, 20)

            If ItemsRead <> ItemsCount OrElse ItemsRead > MaxVars Then
                _LastError = errCliInvalidPlcAnswer
                Return _LastError
            End If
            ' Get Data
            Offset = 21

            For c = 0 To ItemsCount - 1
                ' Get the Item
                Array.Copy(PDU, Offset, S7NckItemRead, 0, Length - Offset)

                If S7NckItemRead(0) = &HFF Then
                    ItemSize = S7.GetWordAt(S7NckItemRead, 2)
                    If S7NckItemRead(1) <> TS_ResOctet AndAlso S7NckItemRead(1) <> TS_ResReal AndAlso S7NckItemRead(1) <> TS_ResBit Then ItemSize = ItemSize >> 3
                    Marshal.Copy(S7NckItemRead, 4, Items(c).pData, ItemSize)
                    Items(c).Result = 0
                    If ItemSize Mod 2 <> 0 Then ItemSize += 1 ' Odd size are rounded
                    Offset = Offset + 4 + ItemSize
                Else
                    Items(c).Result = CpuError(S7NckItemRead(0))
                    Offset += 4 ' Skip the Item header                           
                End If
            Next

            Time_ms = Environment.TickCount - Elapsed
            Return _LastError
        End Function

        Public Function WriteMultiNckVars(ByVal Items As S7NckDataItem(), ByVal ItemsCount As Integer) As Integer
            Dim Offset As Integer
            Dim ParLength As Integer
            Dim DataLength As Integer
            Dim ItemDataSize As Integer
            Dim S7NckParItem = New Byte(S7Nck_MWR_PARAM.Length - 1) {}
            Dim S7NckDataItem = New Byte(1023) {}
            _LastError = 0
            Time_ms = 0
            Dim Elapsed = Environment.TickCount

            ' Checks items
            If ItemsCount > MaxVars Then Return errCliTooManyItems
            ' Fills Header
            Array.Copy(S7Nck_MWR_HEADER, 0, PDU, 0, S7Nck_MWR_HEADER.Length)
            ParLength = ItemsCount * S7Nck_MWR_PARAM.Length + 2
            S7.SetWordAt(PDU, 13, ParLength)
            PDU(18) = CByte(ItemsCount)
            ' Fills Params
            Offset = S7Nck_MWR_HEADER.Length

            For c = 0 To ItemsCount - 1
                ' Set Parameters
                Array.Copy(S7Nck_MWR_PARAM, 0, S7NckParItem, 0, S7Nck_MWR_PARAM.Length)
                Dim NckArea = Items(c).NckArea << 4
                S7NckParItem(3) = CByte(NckArea + Items(c).NckUnit)
                S7.SetWordAt(S7NckParItem, 4, Items(c).ParameterNumber)
                S7.SetWordAt(S7NckParItem, 6, Items(c).Start)
                S7.SetByteAt(S7NckParItem, 8, Items(c).NckModule)
                S7.SetByteAt(S7NckParItem, 9, Items(c).Amount)
                Array.Copy(S7NckParItem, 0, PDU, Offset, S7NckParItem.Length)
                Offset += S7Nck_MWR_PARAM.Length
            Next
            ' Fills Data
            DataLength = 0

            For c = 0 To ItemsCount - 1
                S7NckDataItem(0) = &H0
                ' All Nck-Parameters are written as octet-string
                S7NckDataItem(1) = TS_ResOctet

                If Items(c).WordLen = S7NckConsts.S7WLBit OrElse Items(c).WordLen = S7Consts.S7WLByte Then
                    ItemDataSize = 1
                ElseIf Items(c).WordLen = S7WLDouble Then
                    ItemDataSize = 8
                ElseIf Items(c).WordLen = S7WLString Then
                    ItemDataSize = 16
                Else
                    ItemDataSize = 4
                End If

                If S7NckDataItem(1) <> TS_ResOctet AndAlso S7NckDataItem(1) <> TS_ResBit AndAlso S7NckDataItem(1) <> TS_ResReal Then
                    S7.SetWordAt(S7NckDataItem, 2, ItemDataSize * 8)
                Else
                    S7.SetWordAt(S7NckDataItem, 2, ItemDataSize)
                End If

                Marshal.Copy(Items(c).pData, S7NckDataItem, 4, ItemDataSize)

                If ItemDataSize Mod 2 <> 0 Then
                    S7NckDataItem(ItemDataSize + 4) = &H0
                    ItemDataSize += 1
                End If

                Array.Copy(S7NckDataItem, 0, PDU, Offset, ItemDataSize + 4)
                Offset = Offset + ItemDataSize + 4
                DataLength = DataLength + ItemDataSize + 4
            Next




            ' Checks the size
            If Offset > _PDULength Then Return errCliSizeOverPDU
            S7.SetWordAt(PDU, 2, Offset) ' Whole size
            S7.SetWordAt(PDU, 15, DataLength) ' Whole size
            SendPacket(PDU, Offset)
            RecvIsoPacket()

            If _LastError = 0 Then
                ' Check Global Operation Result
                _LastError = CpuError(S7.GetWordAt(PDU, 17))
                If _LastError <> 0 Then Return _LastError
                ' Get true ItemsCount
                Dim ItemsWritten As Integer = S7.GetByteAt(PDU, 20)

                If ItemsWritten <> ItemsCount OrElse ItemsWritten > MaxVars Then
                    _LastError = errCliInvalidPlcAnswer
                    Return _LastError
                End If

                For c = 0 To ItemsCount - 1

                    If PDU(c + 21) = &HFF Then
                        Items(c).Result = 0
                    Else
                        Items(c).Result = CpuError(PDU(c + 21))
                    End If
                Next

                Time_ms = Environment.TickCount - Elapsed
            End If

            Return _LastError
        End Function

        ' S7 Nck Data Item
        Public Structure S7NckDataItem
            Public NckArea As Integer
            Public NckUnit As Integer
            Public NckModule As Integer
            Public WordLen As Integer
            Public Result As Integer
            Public ParameterNumber As Integer
            Public Start As Integer
            Public Amount As Integer
            Public pData As IntPtr
        End Structure

        ' S7 Nck Connection
        Public Function NckConnectTo(ByVal Address As String) As Integer
            Dim RemoteTSAP As UShort = (ConnType << 8) + 0 * &H20 + 3
            ' testen
            SetConnectionParams(Address, &H100, RemoteTSAP)
            Return Connect()
        End Function
        ' S7 Nck Connection with Rack
        Public Function NckConnectTo(ByVal Address As String, ByVal Rack As Integer) As Integer
            Dim RemoteTSAP As UShort = (ConnType << 8) + Rack * &H20 + 3
            ' testen
            SetConnectionParams(Address, &H100, RemoteTSAP)
            Return Connect()
        End Function

#End Region

#End Region
    End Class


#Region "[S7 Sinumerik]"

#Region "[S7 DriveES]"
#Region "[S7 Drive MultiVar]"
    ' S7 DriveES MultiRead and MultiWrite
    Public Class S7DrvMultiVar
#Region "[MultiRead/Write Helper]"
        Private FClient As S7Client
        Private [Handles] As GCHandle() = New GCHandle(S7Client.MaxVars - 1) {}
        Private Count As Integer = 0
        Private DrvItems As S7Client.S7DrvDataItem() = New S7Client.S7DrvDataItem(S7Client.MaxVars - 1) {}
        Public Results As Integer() = New Integer(S7Client.MaxVars - 1) {}
        ' Adapt WordLength
        Private Function AdjustWordLength(ByVal Area As Integer, ByRef WordLen As Integer, ByRef Amount As Integer, ByRef Start As Integer) As Boolean
            ' Calc Word size          
            Dim WordSize = S7.DataSizeByte(WordLen)
            If WordSize = 0 Then Return False
            Return True
        End Function

        Public Sub New(ByVal Client As S7Client)
            FClient = Client

            For c = 0 To S7Client.MaxVars - 1
                Results(c) = errCliItemNotAvailable
            Next
        End Sub

        Protected Overrides Sub Finalize()
            Clear()
        End Sub

        ' Add Drive Variables
        Public Function DrvAdd(Of T)(ByVal Tag As S7DrvTag, ByRef Buffer As T(), ByVal Offset As Integer) As Boolean
            Return DrvAdd(Tag.DONumber, Tag.ParameterNumber, Tag.WordLen, Tag.Start, Tag.Elements, Buffer, Offset)
        End Function

        Public Function DrvAdd(Of T)(ByVal Tag As S7DrvTag, ByRef Buffer As T()) As Boolean
            Return DrvAdd(Tag.DONumber, Tag.ParameterNumber, Tag.WordLen, Tag.Start, Tag.Elements, Buffer)
        End Function

        Public Function DrvAdd(Of T)(ByVal DONumber As Integer, ByVal ParameterNumber As Integer, ByVal WordLen As Integer, ByVal Start As Integer, ByRef Buffer As T()) As Boolean
            Dim Amount = 1
            Return DrvAdd(DONumber, ParameterNumber, WordLen, Start, Amount, Buffer, 0)
        End Function

        Public Function DrvAdd(Of T)(ByVal DONumber As Integer, ByVal ParameterNumber As Integer, ByVal WordLen As Integer, ByVal Start As Integer, ByVal Amount As Integer, ByRef Buffer As T()) As Boolean
            Return DrvAdd(DONumber, ParameterNumber, WordLen, Start, Amount, Buffer, 0)
        End Function

        Public Function DrvAdd(Of T)(ByVal DONumber As Integer, ByVal ParameterNumber As Integer, ByVal WordLen As Integer, ByVal Start As Integer, ByRef Buffer As T(), ByVal Offset As Integer) As Boolean
            Dim Amount = 1
            Return DrvAdd(DONumber, ParameterNumber, WordLen, Start, Amount, Buffer, Offset)
        End Function

        Public Function DrvAdd(Of T)(ByVal DONumber As Integer, ByVal ParameterNumber As Integer, ByVal WordLen As Integer, ByVal Start As Integer, ByVal Amount As Integer, ByRef Buffer As T(), ByVal Offset As Integer) As Boolean
            If Count < S7Client.MaxVars Then
                'Syntax-ID for DriveES-Communication
                Dim DrvSynID = 162

                If AdjustWordLength(DrvSynID, WordLen, Amount, Start) Then
                    DrvItems(Count).DONumber = DONumber
                    DrvItems(Count).WordLen = WordLen
                    DrvItems(Count).Result = errCliItemNotAvailable
                    DrvItems(Count).ParameterNumber = ParameterNumber
                    DrvItems(Count).Start = Start
                    DrvItems(Count).Amount = Amount
                    Dim handle = GCHandle.Alloc(Buffer, GCHandleType.Pinned)
#If WINDOWS_UWP Or NETFX_CORE Then
                    if (IntPtr.Size == 4)
                        DrvItems[Count].pData = (IntPtr)(handle.AddrOfPinnedObject().ToInt32() + DataOffset * Marshal.SizeOf<T>());
                    else
                        DrvItems[Count].pData = (IntPtr)(handle.AddrOfPinnedObject().ToInt64() + DataOffset * Marshal.SizeOf<T>());
#Else
                    If IntPtr.Size = 4 Then
                        DrvItems(Count).pData = CType((handle.AddrOfPinnedObject().ToInt32() + Offset * Marshal.SizeOf(GetType(T))), IntPtr)
                    Else
                        DrvItems(Count).pData = CType((handle.AddrOfPinnedObject().ToInt64() + Offset * Marshal.SizeOf(GetType(T))), IntPtr)
                    End If
#End If
                    [Handles](Count) = handle
                    Count += 1
                    Offset = WordLen
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        End Function

        ' Read Drive Parameter
        Public Function ReadDrv() As Integer
            Dim FunctionResult As Integer
            Dim GlobalResult = errCliFunctionRefused

            Try

                If Count > 0 Then
                    FunctionResult = FClient.ReadMultiDrvVars(DrvItems, Count)

                    If FunctionResult = 0 Then
                        For c = 0 To S7Client.MaxVars - 1
                            Results(c) = DrvItems(c).Result
                        Next
                    End If

                    GlobalResult = FunctionResult
                Else
                    GlobalResult = errCliFunctionRefused
                End If

            Finally
                Clear() ' handles are no more needed and MUST be freed
            End Try

            Return GlobalResult
        End Function

        ' Write Drive Parameter
        Public Function WriteDrv() As Integer
            Dim FunctionResult As Integer
            Dim GlobalResult = errCliFunctionRefused

            Try

                If Count > 0 Then
                    FunctionResult = FClient.WriteMultiDrvVars(DrvItems, Count)

                    If FunctionResult = 0 Then
                        For c = 0 To S7Client.MaxVars - 1
                            Results(c) = DrvItems(c).Result
                        Next
                    End If

                    GlobalResult = FunctionResult
                Else
                    GlobalResult = errCliFunctionRefused
                End If

            Finally
                Clear() ' handles are no more needed and MUST be freed
            End Try

            Return GlobalResult
        End Function

        Public Sub Clear()
            For c = 0 To Count - 1
                If Not ([Handles](c)) = Nothing Then [Handles](c).Free()
            Next

            Count = 0
        End Sub
#End Region
    End Class
#End Region

#Region "[S7 Drive Constants]"
    ' S7 DriveES Constants
    Public Module S7DrvConsts

        ' Word Length
        Public Const S7WLByte As Integer = &H2
        Public Const S7WLWord As Integer = &H4
        Public Const S7WLInt As Integer = &H5
        Public Const S7WLDWord As Integer = &H6
        Public Const S7WLDInt As Integer = &H7
        Public Const S7WLReal As Integer = &H8

        'S7 DriveEs Tag
        Public Structure S7DrvTag
            Public DONumber As Integer
            Public ParameterNumber As Integer
            Public Start As Integer
            Public Elements As Integer
            Public WordLen As Integer
        End Structure
    End Module
#End Region

    '$CS: Help Funktionen
#Region "[S7 Drv  Help Functions]"
    Public Module S7Drv
        Private bias As Long = 621355968000000000 ' "decimicros" between 0001-01-01 00:00:00 and 1970-01-01 00:00:00

        Private Function BCDtoByte(ByVal B As Byte) As Integer
            Return (B >> 4) * 10 + (B And &HF)
        End Function

        Private Function ByteToBCD(ByVal Value As Integer) As Byte
            Return Value / 10 << 4 Or Value Mod 10
        End Function

        Private Function CopyFrom(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Size As Integer) As Byte()
            Dim Result = New Byte(Size - 1) {}
            Array.Copy(Buffer, Pos, Result, 0, Size)
            Return Result
        End Function

        'S7 DriveES Constants
        Public Function DrvDataSizeByte(ByVal WordLength As Integer) As Integer
            Select Case WordLength
                Case S7DrvConsts.S7WLByte
                    Return 1
                Case S7DrvConsts.S7WLWord
                    Return 2
                Case S7DrvConsts.S7WLDWord
                    Return 4
                Case S7DrvConsts.S7WLInt
                    Return 2
                Case S7DrvConsts.S7WLDInt
                    Return 4
                Case S7DrvConsts.S7WLReal
                    Return 4
                Case Else
                    Return 0
            End Select
        End Function

#Region "Get/Set 8 bit signed value (S7 SInt) -128..127"
        Public Function GetSIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As Integer
            Dim Value As Integer = Buffer(Pos)

            If Value < 128 Then
                Return Value
            Else
                Return Value - 256
            End If
        End Function

        Public Sub SetSIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As Integer)
            If Value < -128 Then Value = -128
            If Value > 127 Then Value = 127
            Buffer(Pos) = CByte(Value)
        End Sub
#End Region

#Region "Get/Set 16 bit signed value (S7 int) -32768..32767"
        Public Function GetIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As Short
            Return Buffer(Pos) << 8 Or Buffer(Pos + 1)
        End Function

        Public Sub SetIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As Short)
            Buffer(Pos) = CByte(Value >> 8)
            Buffer(Pos + 1) = CByte(Value And &HFF)
        End Sub
#End Region

#Region "Get/Set 32 bit signed value (S7 DInt) -2147483648..2147483647"
        Public Function GetDIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As Integer
            Dim Result As Integer
            Result = Buffer(Pos)
            Result <<= 8
            Result += Buffer(Pos + 1)
            Result <<= 8
            Result += Buffer(Pos + 2)
            Result <<= 8
            Result += Buffer(Pos + 3)
            Return Result
        End Function

        Public Sub SetDIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As Integer)
            Buffer(Pos + 3) = CByte(Value And &HFF)
            Buffer(Pos + 2) = CByte(Value >> 8 And &HFF)
            Buffer(Pos + 1) = CByte(Value >> 16 And &HFF)
            Buffer(Pos) = CByte(Value >> 24 And &HFF)
        End Sub
#End Region

#Region "Get/Set 8 bit unsigned value (S7 USInt) 0..255"
        Public Function GetUSIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As Byte
            Return Buffer(Pos)
        End Function

        Public Sub SetUSIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As Byte)
            Buffer(Pos) = Value
        End Sub
#End Region

#Region "Get/Set 16 bit unsigned value (S7 UInt) 0..65535"
        Public Function GetUIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As UShort
            Return Buffer(Pos) << 8 Or Buffer(Pos + 1)
        End Function

        Public Sub SetUIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As UShort)
            Buffer(Pos) = CByte(Value >> 8)
            Buffer(Pos + 1) = CByte(Value And &HFF)
        End Sub
#End Region

#Region "Get/Set 32 bit unsigned value (S7 UDInt) 0..4294967296"
        Public Function GetUDIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As UInteger
            Dim Result As UInteger
            Result = Buffer(Pos)
            Result <<= 8
            Result = Result Or Buffer(Pos + 1)
            Result <<= 8
            Result = Result Or Buffer(Pos + 2)
            Result <<= 8
            Result = Result Or Buffer(Pos + 3)
            Return Result
        End Function

        Public Sub SetUDIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As UInteger)
            Buffer(Pos + 3) = CByte(Value And &HFF)
            Buffer(Pos + 2) = CByte(Value >> 8 And &HFF)
            Buffer(Pos + 1) = CByte(Value >> 16 And &HFF)
            Buffer(Pos) = CByte(Value >> 24 And &HFF)
        End Sub
#End Region

#Region "Get/Set 8 bit word (S7 Byte) 16#00..16#FF"
        Public Function GetByteAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As Byte
            Return Buffer(Pos)
        End Function

        Public Sub SetByteAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As Byte)
            Buffer(Pos) = Value
        End Sub
#End Region

#Region "Get/Set 16 bit word (S7 Word) 16#0000..16#FFFF"
        Public Function GetWordAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As UShort
            Return GetUIntAt(Buffer, Pos)
        End Function

        Public Sub SetWordAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As UShort)
            SetUIntAt(Buffer, Pos, Value)
        End Sub
#End Region

#Region "Get/Set 32 bit word (S7 DWord) 16#00000000..16#FFFFFFFF"
        Public Function GetDWordAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As UInteger
            Return GetUDIntAt(Buffer, Pos)
        End Function

        Public Sub SetDWordAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As UInteger)
            SetUDIntAt(Buffer, Pos, Value)
        End Sub
#End Region

#Region "Get/Set 32 bit floating point number (S7 Real) (Range of Single)"
        Public Function GetRealAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As Single
            Dim Value = GetUDIntAt(Buffer, Pos)
            Dim bytes = BitConverter.GetBytes(Value)
            Return BitConverter.ToSingle(bytes, 0)
        End Function

        Public Sub SetRealAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As Single)
            Dim FloatArray = BitConverter.GetBytes(Value)
            Buffer(Pos) = FloatArray(3)
            Buffer(Pos + 1) = FloatArray(2)
            Buffer(Pos + 2) = FloatArray(1)
            Buffer(Pos + 3) = FloatArray(0)
        End Sub
#End Region

    End Module
#End Region

#End Region

#Region "[S7 Nck]"
#Region "[S7 Nck MultiVar]"
    ' S7 Nck MultiRead and MultiWrite
    Public Class S7NckMultiVar
#Region "[MultiRead/Write Helper]"
        Private FClient As S7Client
        Private [Handles] As GCHandle() = New GCHandle(S7Client.MaxVars - 1) {}
        Private Count As Integer = 0
        Private NckItems As S7Client.S7NckDataItem() = New S7Client.S7NckDataItem(S7Client.MaxVars - 1) {}
        Public Results As Integer() = New Integer(S7Client.MaxVars - 1) {}
        ' Adapt WordLength
        Private Function AdjustWordLength(ByVal Area As Integer, ByRef WordLen As Integer, ByRef Amount As Integer, ByRef Start As Integer) As Boolean
            ' Calc Word size          
            Dim WordSize = NckDataSizeByte(WordLen)
            If WordSize = 0 Then Return False
            If WordLen = S7NckConsts.S7WLBit Then Amount = 1  ' Only 1 bit can be transferred at time
            Return True
        End Function

        Public Sub New(ByVal Client As S7Client)
            FClient = Client

            For c = 0 To S7Client.MaxVars - 1
                Results(c) = errCliItemNotAvailable
            Next
        End Sub

        Protected Overrides Sub Finalize()
            Clear()
        End Sub

        ' Add Nck Variables
        Public Function NckAdd(Of T)(ByVal Tag As S7NckTag, ByRef Buffer As T(), ByVal Offset As Integer) As Boolean
            Return NckAdd(Tag.NckArea, Tag.NckUnit, Tag.NckModule, Tag.ParameterNumber, Tag.WordLen, Tag.Start, Tag.Elements, Buffer, Offset)
        End Function

        Public Function NckAdd(Of T)(ByVal Tag As S7NckTag, ByRef Buffer As T()) As Boolean
            Return NckAdd(Tag.NckArea, Tag.NckUnit, Tag.NckModule, Tag.ParameterNumber, Tag.WordLen, Tag.Start, Tag.Elements, Buffer)
        End Function

        Public Function NckAdd(Of T)(ByVal NckArea As Integer, ByVal NckUnit As Integer, ByVal NckModule As Integer, ByVal ParameterNumber As Integer, ByVal WordLen As Integer, ByVal Start As Integer, ByRef Buffer As T()) As Boolean
            Dim Amount = 1
            Return NckAdd(NckArea, NckUnit, NckModule, ParameterNumber, WordLen, Start, Amount, Buffer)
        End Function

        Public Function NckAdd(Of T)(ByVal NckArea As Integer, ByVal NckUnit As Integer, ByVal NckModule As Integer, ByVal ParameterNumber As Integer, ByVal WordLen As Integer, ByVal Start As Integer, ByVal Amount As Integer, ByRef Buffer As T()) As Boolean
            Return NckAdd(NckArea, NckUnit, NckModule, ParameterNumber, WordLen, Start, Amount, Buffer, 0)
        End Function

        Public Function NckAdd(Of T)(ByVal NckArea As Integer, ByVal NckUnit As Integer, ByVal NckModule As Integer, ByVal ParameterNumber As Integer, ByVal WordLen As Integer, ByVal Start As Integer, ByVal Amount As Integer, ByRef Buffer As T(), ByVal Offset As Integer) As Boolean
            If Count < S7Client.MaxVars Then
                'Syntax-ID for Nck-Communication
                Dim NckSynID = 130

                If AdjustWordLength(NckSynID, WordLen, Amount, Start) Then
                    NckItems(Count).NckArea = NckArea
                    NckItems(Count).WordLen = WordLen
                    NckItems(Count).Result = errCliItemNotAvailable
                    NckItems(Count).ParameterNumber = ParameterNumber
                    NckItems(Count).Start = Start
                    NckItems(Count).Amount = Amount
                    NckItems(Count).NckUnit = NckUnit
                    NckItems(Count).NckModule = NckModule
                    Dim handle = GCHandle.Alloc(Buffer, GCHandleType.Pinned)
#If WINDOWS_UWP Or NETFX_CORE Then
                    if (IntPtr.Size == 4)
                        NckItems[Count].pData = (IntPtr)(handle.AddrOfPinnedObject().ToInt32() + Offset * Marshal.SizeOf<T>());
                    else
                        NckItems[Count].pData = (IntPtr)(handle.AddrOfPinnedObject().ToInt64() + Offset * Marshal.SizeOf<T>());
#Else
                    If IntPtr.Size = 4 Then
                        NckItems(Count).pData = CType((handle.AddrOfPinnedObject().ToInt32() + Offset * Marshal.SizeOf(GetType(T))), IntPtr)
                    Else
                        NckItems(Count).pData = CType((handle.AddrOfPinnedObject().ToInt64() + Offset * Marshal.SizeOf(GetType(T))), IntPtr)
                    End If
#End If
                    [Handles](Count) = handle
                    Count += 1
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        End Function

        'Read Nck Parameter
        Public Function ReadNck() As Integer
            Dim FunctionResult As Integer
            Dim GlobalResult = errCliFunctionRefused

            Try

                If Count > 0 Then
                    FunctionResult = FClient.ReadMultiNckVars(NckItems, Count)

                    If FunctionResult = 0 Then
                        For c = 0 To S7Client.MaxVars - 1
                            Results(c) = NckItems(c).Result
                        Next
                    End If

                    GlobalResult = FunctionResult
                Else
                    GlobalResult = errCliFunctionRefused
                End If

            Finally
                Clear() ' handles are no more needed and MUST be freed
            End Try

            Return GlobalResult
        End Function

        ' Write Nck Parameter
        Public Function WriteNck() As Integer
            Dim FunctionResult As Integer
            Dim GlobalResult = errCliFunctionRefused

            Try

                If Count > 0 Then
                    FunctionResult = FClient.WriteMultiNckVars(NckItems, Count)

                    If FunctionResult = 0 Then
                        For c = 0 To S7Client.MaxVars - 1
                            Results(c) = NckItems(c).Result
                        Next
                    End If

                    GlobalResult = FunctionResult
                Else
                    GlobalResult = errCliFunctionRefused
                End If

            Finally
                Clear() ' handles are no more needed and MUST be freed
            End Try

            Return GlobalResult
        End Function

        Public Sub Clear()
            For c = 0 To Count - 1
                If Not ([Handles](c)) = Nothing Then [Handles](c).Free()
            Next

            Count = 0
        End Sub
#End Region
    End Class
#End Region

#Region "[S7 Nck Constants]"
    ' Nck Constants
    Public Module S7NckConsts

        ' Word Length
        Public Const S7WLBit As Integer = &H1
        Public Const S7WLByte As Integer = &H2
        Public Const S7WLChar As Integer = &H3
        Public Const S7WLWord As Integer = &H4
        Public Const S7WLInt As Integer = &H5
        Public Const S7WLDWord As Integer = &H6
        Public Const S7WLDInt As Integer = &H7
        Public Const S7WLDouble As Integer = &H1A
        Public Const S7WLString As Integer = &H1B


        'S7 Nck Tag
        Public Structure S7NckTag
            Public NckArea As Integer
            Public NckUnit As Integer
            Public NckModule As Integer
            Public ParameterNumber As Integer
            Public Start As Integer
            Public Elements As Integer
            Public WordLen As Integer
        End Structure
    End Module
#End Region

    '$CS: Help Funktionen sind zu überarbeiten
#Region "[S7 Nck  Help Functions]"

    Public Module S7Nck
        Private bias As Long = 621355968000000000 ' "decimicros" between 0001-01-01 00:00:00 and 1970-01-01 00:00:00

        Private Function BCDtoByte(ByVal B As Byte) As Integer
            Return (B >> 4) * 10 + (B And &HF)
        End Function

        Private Function ByteToBCD(ByVal Value As Integer) As Byte
            Return Value / 10 << 4 Or Value Mod 10
        End Function

        Private Function CopyFrom(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Size As Integer) As Byte()
            Dim Result = New Byte(Size - 1) {}
            Array.Copy(Buffer, Pos, Result, 0, Size)
            Return Result
        End Function

        Private Function OctRev(ByVal bytes As Byte(), ByVal Size As Integer) As Byte()
            Dim reverse = New Byte(Size - 1) {}
            Dim j = 0

            For i = Size - 1 To 0 Step -1
                reverse(j) = bytes(i)
                j += 1
            Next

            Return reverse
        End Function

        'S7 Nck Constants
        Public Function NckDataSizeByte(ByVal WordLength As Integer) As Integer
            Select Case WordLength
                Case S7NckConsts.S7WLBit
                    Return 1  ' S7 sends 1 byte per bit
                Case S7NckConsts.S7WLByte
                    Return 1
                Case S7NckConsts.S7WLChar
                    Return 1
                Case S7NckConsts.S7WLWord
                    Return 2
                Case S7NckConsts.S7WLDWord
                    Return 4
                Case S7NckConsts.S7WLInt
                    Return 2
                Case S7NckConsts.S7WLDInt
                    Return 4
                Case S7WLDouble
                    Return 8
                Case S7WLString
                    Return 16
                Case Else
                    Return 0
            End Select
        End Function

#Region "Get/Set the bit at Pos.Bit"
        Public Function GetBitAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Bit As Integer) As Boolean
            Dim Mask As Byte() = {&H1, &H2, &H4, &H8, &H10, &H20, &H40, &H80}
            If Bit < 0 Then Bit = 0
            If Bit > 7 Then Bit = 7
            Return (Buffer(Pos) And Mask(Bit)) <> 0
        End Function

        Public Sub SetBitAt(ByRef Buffer As Byte(), ByVal Pos As Integer, ByVal Bit As Integer, ByVal Value As Boolean)
            Dim Mask As Byte() = {&H1, &H2, &H4, &H8, &H10, &H20, &H40, &H80}
            If Bit < 0 Then Bit = 0
            If Bit > 7 Then Bit = 7

            If Value Then
                Buffer(Pos) = Buffer(Pos) Or Mask(Bit)
            Else
                Buffer(Pos) = Buffer(Pos) And Not Mask(Bit)
            End If
        End Sub
#End Region

#Region "Get/Set 8 bit signed value (S7 SInt) -128..127"
        Public Function GetSIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As Integer
            Dim Value As Integer = Buffer(Pos)

            If Value < 128 Then
                Return Value
            Else
                Return Value - 256
            End If
        End Function

        Public Sub SetSIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As Integer)
            If Value < -128 Then Value = -128
            If Value > 127 Then Value = 127
            Buffer(Pos) = CByte(Value)
        End Sub
#End Region

#Region "Get/Set 16 bit signed value (S7 int) -32768..32767"
        Public Function GetIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As Short
            Return Buffer(Pos + 1) << 8 Or Buffer(Pos)
        End Function

        Public Sub SetIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As Short)
            Buffer(Pos + 1) = CByte(Value >> 8)
            Buffer(Pos) = CByte(Value And &HFF)
        End Sub
#End Region

#Region "Get/Set 32 bit signed value (S7 DInt) -2147483648..2147483647"
        Public Function GetDIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As Integer
            Dim Result As Integer
            Result = Buffer(Pos + 3)
            Result <<= 8
            Result += Buffer(Pos + 2)
            Result <<= 8
            Result += Buffer(Pos + 1)
            Result <<= 8
            Result += Buffer(Pos)
            Return Result
        End Function

        Public Sub SetDIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As Integer)
            Buffer(Pos) = CByte(Value And &HFF)
            Buffer(Pos + 1) = CByte(Value >> 8 And &HFF)
            Buffer(Pos + 2) = CByte(Value >> 16 And &HFF)
            Buffer(Pos + 3) = CByte(Value >> 24 And &HFF)
        End Sub
#End Region

#Region "Get/Set 64 bit signed value (S7 LInt) -9223372036854775808..9223372036854775807"
        Public Function GetLIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As Long
            Dim Result As Long
            Result = Buffer(Pos + 7)
            Result <<= 8
            Result += Buffer(Pos + 6)
            Result <<= 8
            Result += Buffer(Pos + 5)
            Result <<= 8
            Result += Buffer(Pos + 4)
            Result <<= 8
            Result += Buffer(Pos + 3)
            Result <<= 8
            Result += Buffer(Pos + 2)
            Result <<= 8
            Result += Buffer(Pos + 1)
            Result <<= 8
            Result += Buffer(Pos)
            Return Result
        End Function

        Public Sub SetLIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As Long)
            Buffer(Pos) = CByte(Value And &HFF)
            Buffer(Pos + 1) = CByte(Value >> 8 And &HFF)
            Buffer(Pos + 2) = CByte(Value >> 16 And &HFF)
            Buffer(Pos + 3) = CByte(Value >> 24 And &HFF)
            Buffer(Pos + 4) = CByte(Value >> 32 And &HFF)
            Buffer(Pos + 5) = CByte(Value >> 40 And &HFF)
            Buffer(Pos + 6) = CByte(Value >> 48 And &HFF)
            Buffer(Pos + 7) = CByte(Value >> 56 And &HFF)
        End Sub
#End Region

#Region "Get/Set 8 bit unsigned value (S7 USInt) 0..255"
        Public Function GetUSIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As Byte
            Return Buffer(Pos)
        End Function

        Public Sub SetUSIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As Byte)
            Buffer(Pos) = Value
        End Sub
#End Region

#Region "Get/Set 16 bit unsigned value (S7 UInt) 0..65535"
        Public Function GetUIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As UShort
            Return Buffer(Pos + 1) << 8 Or Buffer(Pos)
        End Function

        Public Sub SetUIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As UShort)
            Buffer(Pos + 1) = CByte(Value >> 8)
            Buffer(Pos) = CByte(Value And &HFF)
        End Sub
#End Region

#Region "Get/Set 32 bit unsigned value (S7 UDInt) 0..4294967296"
        Public Function GetUDIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As UInteger
            Dim Result As UInteger
            Result = Buffer(Pos + 3)
            Result <<= 8
            Result = Result Or Buffer(Pos + 2)
            Result <<= 8
            Result = Result Or Buffer(Pos + 1)
            Result <<= 8
            Result = Result Or Buffer(Pos)
            Return Result
        End Function

        Public Sub SetUDIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As UInteger)
            Buffer(Pos) = CByte(Value And &HFF)
            Buffer(Pos + 1) = CByte(Value >> 8 And &HFF)
            Buffer(Pos + 2) = CByte(Value >> 16 And &HFF)
            Buffer(Pos + 3) = CByte(Value >> 24 And &HFF)
        End Sub
#End Region

#Region "Get/Set 64 bit unsigned value (S7 ULint) 0..18446744073709551616"
        Public Function GetULIntAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As ULong
            Dim Result As ULong
            Result = Buffer(Pos + 7)
            Result <<= 8
            Result = Result Or Buffer(Pos + 6)
            Result <<= 8
            Result = Result Or Buffer(Pos + 5)
            Result <<= 8
            Result = Result Or Buffer(Pos + 4)
            Result <<= 8
            Result = Result Or Buffer(Pos + 3)
            Result <<= 8
            Result = Result Or Buffer(Pos + 2)
            Result <<= 8
            Result = Result Or Buffer(Pos + 1)
            Result <<= 8
            Result = Result Or Buffer(Pos)
            Return Result
        End Function

        Public Sub SetULintAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As ULong)
            Buffer(Pos + 7) = CByte(Value And &HFF)
            Buffer(Pos + 6) = CByte(Value >> 8 And &HFF)
            Buffer(Pos + 5) = CByte(Value >> 16 And &HFF)
            Buffer(Pos + 4) = CByte(Value >> 24 And &HFF)
            Buffer(Pos + 3) = CByte(Value >> 32 And &HFF)
            Buffer(Pos + 2) = CByte(Value >> 40 And &HFF)
            Buffer(Pos + 1) = CByte(Value >> 48 And &HFF)
            Buffer(Pos) = CByte(Value >> 56 And &HFF)
        End Sub
#End Region

#Region "Get/Set 8 bit word (S7 Byte) 16#00..16#FF"
        Public Function GetByteAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As Byte
            Return Buffer(Pos)
        End Function

        Public Sub SetByteAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As Byte)
            Buffer(Pos) = Value
        End Sub
#End Region

#Region "Get/Set 16 bit word (S7 Word) 16#0000..16#FFFF"
        Public Function GetWordAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As UShort
            Return GetUIntAt(Buffer, Pos)
        End Function

        Public Sub SetWordAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As UShort)
            SetUIntAt(Buffer, Pos, Value)
        End Sub
#End Region

#Region "Get/Set 32 bit word (S7 DWord) 16#00000000..16#FFFFFFFF"
        Public Function GetDWordAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As UInteger
            Return GetUDIntAt(Buffer, Pos)
        End Function

        Public Sub SetDWordAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As UInteger)
            SetUDIntAt(Buffer, Pos, Value)
        End Sub
#End Region

#Region "Get/Set 64 bit word (S7 LWord) 16#0000000000000000..16#FFFFFFFFFFFFFFFF"
        Public Function GetLWordAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As ULong
            Return GetULIntAt(Buffer, Pos)
        End Function

        Public Sub SetLWordAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As ULong)
            SetULintAt(Buffer, Pos, Value)
        End Sub
#End Region

#Region "Get/Set 64 bit floating point number (S7 LReal) (Range of Double)"
        Public Function GetLRealAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As Double
            Dim Value = GetULIntAt(Buffer, Pos)
            Dim bytes = BitConverter.GetBytes(Value)
            Return BitConverter.ToDouble(bytes, 0)
        End Function

        Public Sub SetLRealAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As Double)
            Dim FloatArray = BitConverter.GetBytes(Value)
            Buffer(Pos + 7) = FloatArray(7)
            Buffer(Pos + 6) = FloatArray(6)
            Buffer(Pos + 5) = FloatArray(5)
            Buffer(Pos + 4) = FloatArray(4)
            Buffer(Pos + 3) = FloatArray(3)
            Buffer(Pos + 2) = FloatArray(2)
            Buffer(Pos + 1) = FloatArray(1)
            Buffer(Pos) = FloatArray(0)
        End Sub
#End Region

#Region "Get/Set String (Nck Octet String)"
        Public Function GetStringAt(ByVal Buffer As Byte(), ByVal Pos As Integer) As String
            Dim size = 16
            Return Encoding.UTF8.GetString(Buffer, Pos, size)
        End Function

        Public Sub SetStringAt(ByVal Buffer As Byte(), ByVal Pos As Integer, ByVal Value As String)
            Dim size = Value.Length
            Encoding.UTF8.GetBytes(Value, 0, size, Buffer, Pos)
        End Sub

#End Region




    End Module
#End Region

#End Region

#End Region







End Namespace





