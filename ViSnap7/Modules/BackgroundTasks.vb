Module BackgroundTasks
    Public openFormsLastIteration As Integer
    Public totalPlcNumber As Integer = 0
    Public Sub AccomodatePlcData()
        Dim i As Integer
        'It is just for execute at the loading of the main form.
        If firstExecution Then

            ViSnap7Setup.CultureSelection()
            ViSnap7Setup.SetupPlc()
            'Check how many PLC are configured
            For c = 0 To plc.GetUpperBound(0)
                If Not IsNothing(plc(c)) Then
                    totalPlcNumber = totalPlcNumber + 1
                End If
            Next

            CreateClients()

        End If
        'Update controls when Open/close forms 
        If My.Application.OpenForms.Count <> OpenFormsLastIteration Then
            'Clear all controls and data from PLC

            For counter = 0 To TotalPlcNumber - 1
                With PLC(counter)

                    .ControlsCollection.Clear()


                    If .DBData IsNot Nothing Then
                        For i = 0 To .DBData.GetUpperBound(0) - 1
                            ReDim .DBData(i).Data(0)
                            .DBData(i).MaxByte = -1
                            .DBData(i).MinByte = 99999
                        Next
                    End If

                    If .InputData IsNot Nothing Then
                        For i = 0 To .InputData.GetUpperBound(0) - 1
                            ReDim .InputData(i).Data(0)
                            .InputData(i).MaxByte = -1
                            .InputData(i).MinByte = 99999
                        Next
                    End If

                    If .OutputData IsNot Nothing Then
                        For i = 0 To .OutputData.GetUpperBound(0) - 1
                            ReDim .OutputData(i).Data(0)
                            .OutputData(i).MaxByte = -1
                            .OutputData(i).MinByte = 99999
                        Next
                    End If

                    If .MarksData IsNot Nothing Then
                        For i = 0 To .MarksData.GetUpperBound(0) - 1
                            ReDim .MarksData(i).Data(0)
                            .MarksData(i).MaxByte = -1
                            .MarksData(i).MinByte = 99999
                        Next
                    End If
                End With

            Next
            For Each frm As Form In My.Application.OpenForms
                For Each ctr As Control In frm.Controls

                    'Check if control belongs to the custom-controls list developed by the user

                    If [Enum].IsDefined(GetType([PlcControlTypes]), ctr.GetType.Name) Then
                        CheckDataToBeloaded(ctr)
                    End If


                Next
            Next
        End If
        'Save the number of openforms for checking next iteration
        openFormsLastIteration = My.Application.OpenForms.Count
        'Delay the reading the desired interval
        Threading.Thread.Sleep(KReadingIntervalMiliseconds)

    End Sub

    Sub CreateClients()
        For contador = 0 To UBound(PLC) - 1
            If Not IsNothing(PLC(contador)) Then
                With PLC(contador)
                    ReDim .DBData(0)
                    ReDim .InputData(0)
                    ReDim .OutputData(0)
                    ReDim .MarksData(0)
                    .DBData.Initialize()
                    .InputData.Initialize()
                    .OutputData.Initialize()
                    .MarksData.Initialize()


                End With
            End If
        Next
    End Sub
    Sub CheckDataToBeloaded(ByVal ctr As Object)
        Dim plcNum As Integer
        Dim byteAmount As Integer
        Dim plcDataArea As DataArea
        Dim plcMinByte As Integer
        Dim plcMaxByte As Integer
        Dim plcDbNumber As Integer
        With ctr
            plcNum = .PLC_Number
            plcDataArea = .PLC_DataArea
            Select Case .PLC_DataType
                Case DataType.BOOL, DataType.CHR, DataType.SINT, DataType.UINT
                    byteAmount = 1
                Case DataType.INT
                    byteAmount = 2
                Case DataType.REAL, DataType.DINT
                    byteAmount = 4
                Case DataType.STR
                    byteAmount = .PLC_Length + 2


            End Select

            plcMinByte = .PLC_Byte
            plcMaxByte = .PLC_Byte + byteAmount - 1

            If .PLC_Dataarea = DataArea.DB Then
                plcDbNumber = .PLC_DB
            Else
                plcDbNumber = 0
            End If



            Select Case plcDataArea
                Case DataArea.DB
                    If plc(plcNum).dbData.GetUpperBound(0) < plcDbNumber Then
                        ReDim Preserve plc(plcNum).dbData(plcDbNumber)
                    End If
                    If plc(plcNum).dbData(plcDbNumber).data Is Nothing Then
                        plc(plcNum).dbData(plcDbNumber) = New PlcClient.ByteData
                        ReDim Preserve plc(plcNum).dbData(plcDbNumber).data(0)

                        plc(plcNum).dbData(plcDbNumber).maxByte = -1
                        plc(plcNum).dbData(plcDbNumber).minByte = 999999 'byte not reachable

                    End If
                    If plcMinByte < plc(plcNum).dbData(plcDbNumber).minByte Then
                        plc(plcNum).dbData(plcDbNumber).minByte = plcMinByte
                    End If

                    If plcMaxByte > plc(plcNum).dbData(plcDbNumber).maxByte Then
                        plc(plcNum).dbData(plcDbNumber).maxByte = plcMaxByte
                        ReDim Preserve plc(plcNum).dbData(plcDbNumber).data(plcMaxByte)
                    End If

                Case DataArea.INPUT
                    If plc(plcNum).inputData.GetUpperBound(0) < plcDbNumber Then
                        ReDim Preserve plc(plcNum).inputData(plcDbNumber)
                    End If
                    If plc(plcNum).inputData(plcDbNumber).data Is Nothing Then
                        plc(plcNum).inputData(plcDbNumber) = New PlcClient.ByteData
                        ReDim Preserve plc(plcNum).inputData(plcDbNumber).data(0)

                        plc(plcNum).inputData(plcDbNumber).maxByte = -1
                        plc(plcNum).inputData(plcDbNumber).minByte = 999999 'byte not reachable

                    End If
                    If plcMinByte < plc(plcNum).inputData(plcDbNumber).minByte Then
                        plc(plcNum).inputData(plcDbNumber).minByte = plcMinByte
                    End If

                    If plcMaxByte > plc(plcNum).inputData(plcDbNumber).maxByte Then
                        plc(plcNum).inputData(plcDbNumber).maxByte = plcMaxByte
                        ReDim Preserve plc(plcNum).inputData(plcDbNumber).data(plcMaxByte)
                    End If


                Case DataArea.OUTPUT
                    If plc(plcNum).outputData.GetUpperBound(0) < plcDbNumber Then
                        ReDim Preserve plc(plcNum).outputData(plcDbNumber)
                    End If
                    If plc(plcNum).outputData(plcDbNumber).data Is Nothing Then
                        plc(plcNum).outputData(plcDbNumber) = New PlcClient.ByteData
                        ReDim Preserve plc(plcNum).outputData(plcDbNumber).data(0)

                        plc(plcNum).outputData(plcDbNumber).maxByte = -1
                        plc(plcNum).outputData(plcDbNumber).minByte = 999999 'byte not reachable

                    End If
                    If plcMinByte < plc(plcNum).outputData(plcDbNumber).minByte Then
                        plc(plcNum).outputData(plcDbNumber).minByte = plcMinByte
                    End If

                    If plcMaxByte > plc(plcNum).outputData(plcDbNumber).maxByte Then
                        plc(plcNum).outputData(plcDbNumber).maxByte = plcMaxByte
                        ReDim Preserve plc(plcNum).outputData(plcDbNumber).data(plcMaxByte)
                    End If

                Case DataArea.MARK
                    If plc(plcNum).marksData.GetUpperBound(0) < plcDbNumber Then
                        ReDim Preserve plc(plcNum).marksData(plcDbNumber)
                    End If
                    If plc(plcNum).marksData(plcDbNumber).data Is Nothing Then
                        plc(plcNum).marksData(plcDbNumber) = New PlcClient.ByteData
                        ReDim Preserve plc(plcNum).marksData(plcDbNumber).data(0)

                        plc(plcNum).marksData(plcDbNumber).maxByte = -1
                        plc(plcNum).marksData(plcDbNumber).minByte = 999999 'byte not reachable

                    End If
                    If plcMinByte < plc(plcNum).marksData(plcDbNumber).minByte Then
                        plc(plcNum).marksData(plcDbNumber).minByte = plcMinByte
                    End If

                    If plcMaxByte > plc(plcNum).marksData(plcDbNumber).maxByte Then
                        plc(plcNum).marksData(plcDbNumber).maxByte = plcMaxByte
                        ReDim Preserve plc(plcNum).marksData(plcDbNumber).data(plcMaxByte)
                    End If


            End Select

        End With
        'Add the controls
        PLC(PlcNum).ControlsCollection.Add(ctr)


    End Sub


    Public Sub FillPlcData()
        Dim c As Integer
        Dim DbIndex As Integer

        For c = 0 To TotalPlcNumber - 1

            With PLC(c)
                If .Connected Then

                    'Read DB data
                    For DbIndex = 0 To .DBData.GetUpperBound(0)
                        If .DBData(DbIndex).Data IsNot Nothing And (DbIndex > 0) Then
                            FillDb(c, DbIndex, .DBData(DbIndex).MinByte, .DBData(DbIndex).MaxByte)
                        End If

                    Next

                    'Read Marks area
                    If .MarksData(0).Data IsNot Nothing Then
                        FillMarks(c, .MarksData(0).MinByte, .MarksData(0).MaxByte)
                    End If

                    'Read Inputs area
                    If .InputData(0).Data IsNot Nothing Then
                        FillInputs(c, .InputData(0).MinByte, .InputData(0).MaxByte)
                    End If

                    'Read Ouputs area
                    If .OutputData(0).Data IsNot Nothing Then
                        FillOutputs(c, .OutputData(0).MinByte, .OutputData(0).MaxByte)
                    End If
                End If

            End With

        Next
    End Sub

    Sub FillDb(ByVal PlcNumber As Integer, ByVal DbNumber As Integer, ByVal ByteMin As Integer, ByVal byteMax As Integer)
        Dim Amount As Integer = byteMax - ByteMin + 1
        Dim Buffer(Amount - 1) As Byte
        PLC(PlcNumber).Client.DBRead(DbNumber, ByteMin, Amount, Buffer)
        Array.Copy(Buffer, 0, PLC(PlcNumber).DBData(DbNumber).Data, ByteMin, Amount)

    End Sub
    Sub FillMarks(ByVal PlcNumber As Integer, ByVal ByteMin As Integer, ByVal byteMax As Integer)
        Dim Amount As Integer = byteMax - ByteMin + 1
        Dim Buffer(Amount - 1) As Byte
        PLC(PlcNumber).Client.MBRead(ByteMin, Amount, Buffer)
        Array.Copy(Buffer, 0, PLC(PlcNumber).MarksData(0).Data, ByteMin, Amount)

    End Sub

    Sub FillInputs(ByVal PlcNumber As Integer, ByVal ByteMin As Integer, ByVal byteMax As Integer)
        Dim Amount As Integer = byteMax - ByteMin + 1
        Dim Buffer(Amount - 1) As Byte
        PLC(PlcNumber).Client.EBRead(ByteMin, Amount, Buffer)
        Array.Copy(Buffer, 0, PLC(PlcNumber).InputData(0).Data, ByteMin, Amount)

    End Sub

    Sub FillOutputs(ByVal PlcNumber As Integer, ByVal ByteMin As Integer, ByVal byteMax As Integer)
        Dim Amount As Integer = byteMax - ByteMin + 1
        Dim Buffer(Amount - 1) As Byte
        PLC(PlcNumber).Client.ABRead(ByteMin, Amount, Buffer)
        Array.Copy(Buffer, 0, PLC(PlcNumber).OutputData(0).Data, ByteMin, Amount)

    End Sub


End Module
