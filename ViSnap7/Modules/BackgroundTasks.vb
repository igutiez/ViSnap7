Module BackgroundTasks
    Public openFormsLastIteration As Integer
    Public totalPlcNumber As Integer = 0
    Public UpdateNumberOfControlsActive As Boolean
    Public InibitUpdateControls As Boolean

    Private minReference As Integer = 9999
    Private maxReferece As Integer = -1
    Public Sub AccomodatePlcData()
        Dim i As Integer
        'It is just for execute at the loading of the main form.
        If firstExecution Then
            'Apply Custom Culture
            ViSnap7Setup.CultureSelection()
            'Adjust the desired PLC
            ViSnap7Setup.SetupPlc()

            'Check how many PLC are configured
            For c = 0 To plc.GetUpperBound(0)
                If Not IsNothing(plc(c)) Then
                    totalPlcNumber = totalPlcNumber + 1
                End If
            Next


        End If

        'Update controls when Open/close forms 
        'Only if number of openforms changes this is performed.
        'This can be set externally to force a new update
        If (My.Application.OpenForms.Count <> openFormsLastIteration) And (Not InibitUpdateControls) Then
            UpdateNumberOfControlsActive = True
        End If
        InibitUpdateControls = False

        If UpdateNumberOfControlsActive Then
            UpdateNumberOfControlsActive = False

            'Create the Clients (one per PLC)
            CreateClients()

            'Clear all controls and data from PLC
            For counter = 0 To totalPlcNumber - 1
                With plc(counter)

                    .controlsCollection.Clear()


                    If .dbData IsNot Nothing Then
                        For i = 0 To .dbData.GetUpperBound(0) - 1
                            ReDim .dbData(i).data(0)
                            .dbData(i).maxByte = maxReferece
                            .dbData(i).minByte = minReference
                        Next
                    End If

                    If .inputData IsNot Nothing Then
                        For i = 0 To .inputData.GetUpperBound(0) - 1
                            ReDim .inputData(i).data(0)
                            .inputData(i).maxByte = maxReferece
                            .inputData(i).minByte = minReference
                        Next
                    End If

                    If .outputData IsNot Nothing Then
                        For i = 0 To .outputData.GetUpperBound(0) - 1
                            ReDim .outputData(i).data(0)
                            .outputData(i).maxByte = maxReferece
                            .outputData(i).minByte = minReference
                        Next
                    End If

                    If .marksData IsNot Nothing Then
                        For i = 0 To .marksData.GetUpperBound(0) - 1
                            ReDim .marksData(i).data(0)
                            .marksData(i).maxByte = maxReferece
                            .marksData(i).minByte = minReference
                        Next
                    End If
                    'Delete all controls in collection
                    .controlsCollection.Clear()
                End With

            Next


            For Each frm As Form In My.Application.OpenForms

                Dim AllControls As List(Of Control)

                AllControls = GetTypeControls(Of Control)(frm, True)

                'Check if control belongs to the custom-controls list developed by the user

                For Each ctr As Object In AllControls

                    If [Enum].IsDefined(GetType(Control_List.PlcControlTypes), ctr.GetType.Name) Then
                        CheckDataToBeloaded(ctr)
                        'Add the controls
                        plc(ctr.PLC_Number).controlsCollection.Add(ctr)
                    End If

                Next

            Next
        End If
        'Save the number of openforms for checking next iteration
        openFormsLastIteration = My.Application.OpenForms.Count


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
                Case DataType.BOOL, DataType.CHR, DataType.SINT, DataType.USINT
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

                        plc(plcNum).outputData(plcDbNumber).maxByte = maxReferece
                        plc(plcNum).outputData(plcDbNumber).minByte = minReference  'byte not reachable

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

                        plc(plcNum).marksData(plcDbNumber).maxByte = maxReferece
                        plc(plcNum).marksData(plcDbNumber).minByte = minReference  'byte not reachable

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
