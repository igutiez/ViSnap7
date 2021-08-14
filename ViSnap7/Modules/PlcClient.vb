'======================================================================================================================================
' PlcClient.vb Ver 1.0
' August 2021 - 

''' <summary>
''' PLCCLient is the class where you will store all data that are read from the PLC. Later, these data will be assigned to the controls.
''' </summary>
Public Class PlcClient
    ''' <summary>
    ''' Array data of bytes
    ''' </summary>
    Structure ByteData
        Public data As Byte() 'Raw data
        Public minByte As Integer 'Minimum-position byte with data
        Public maxByte As Integer 'Maximum-position byte with data
    End Structure
    Public connected As Boolean = False 'Client is connected or not
    Public client As ViSnap7.S7Client 'Client which connects to the PLC
    Public dbData() As ByteData 'Array of DB with data to be read
    Public inputData() As ByteData 'Array of Inputs (E) to be read
    Public outputData() As ByteData 'Array of Outputs (A) to be read
    Public marksData() As ByteData ' Array of marks (M) to be read
    Public ip As String
    Public rack As Integer
    Public slot As Integer
    Public plcName As String 'Just an identifier or PLC description
    Public controlsCollection As New List(Of Control)
    Sub New(_Name As String, _IP As String, _rack As Integer, _slot As Integer)
        Me.IP = _IP
        Me.plcName = _Name
        Me.Rack = _rack
        Me.Slot = _slot

        'Connect to the PLC if Connection enabled
        If KGlobalConnectionEnabled Then
            Connect()
        End If
    End Sub

    ''Connection to the PLC
    Public Function Connect() As Boolean

        Me.connected = ConnectToPLC(Me.ip, Me.rack, Me.slot)
        If Not Me.connected Then
            If MsgBox(KErrorConnectingToPLC1 & " " & Me.plcName & " " & KRequestRetry, MsgBoxStyle.OkCancel, KErrorConnectionTitle) = MsgBoxResult.Ok Then
                Connect()
            Else
                Return False
            End If
        End If
        Return True
    End Function

    Public Function ConnectToPLC(ByVal IP As String, ByVal Rack As Integer, ByVal Slot As Integer) As Boolean
        Dim result As Integer
        Me.Client = New ViSnap7.S7Client
        client.SetConnectionType(&H2)
        result = client.ConnectTo(IP, Rack, Slot)
        ConnectToPLC = client.Connected
    End Function

End Class
