Module ViSnap7Setup
    ' Number of maximum PLC connected at the same time
    Public Const KMaxNumberOfPLC As Integer = 10
    'Connection should be True. Only for testing without PLC connections.
    Public Const KGlobalConnectionEnabled As Boolean = True

    'Time interval (miliseconds) between readings
    Public Const KReadingIntervalMiliseconds As Integer = 250
    'Just setup the PLC addresses
    Public Sub SetupPlc()
        'Setup PLC
        'Rack Slot for S7-1200/S7-1500 Rack 0, Slot 0
        'Rack Slot for S7-300 Rack 0, Slot 2
        'Rack Slot for S7-400 tipically Rack 0, Slot 3
        'Just uncomment or add as many as needed PLC connections (take in consideration the max number of PLC in KMaxNumberOfPLC
        plc(0) = New PlcClient("PLC0", "192.168.1.192", 0, 0)
        'plc(1) = New PlcClient("PLC1", "192.168.1.192", 0, 2)
        'plc(2) = New PlcClient("PLC0", "192.168.1.193", 0, 2)

    End Sub

    'Custom culture
    Public culture As Globalization.CultureInfo


    Public Sub CultureSelection()
        culture = New Globalization.CultureInfo("es-ES") ' English: "en-GB"
        REM Define decimal separator
        culture.NumberFormat.NumberDecimalSeparator = ","
        System.Threading.Thread.CurrentThread.CurrentCulture = culture
    End Sub



End Module
