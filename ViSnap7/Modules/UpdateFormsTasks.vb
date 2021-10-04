Module UpdateFormsTasks
    ''' <summary>
    ''' It is used for reading/writing the control. 
    ''' </summary>
    ''' <param name="enable">Enable or not control updating</param>
    Public Sub UpdateControls(ByVal enable As Boolean)
        Dim c As Integer
        If enable Then
            For c = 0 To totalPlcNumber - 1
                For Each ctr As Object In plc(c).controlsCollection
                    If plc(c).connected Then
                        ctr.UpdateControl(plc(c))
                    End If
                Next

            Next
        End If

    End Sub

    ''' <summary>
    ''' Submit the form created.
    ''' </summary>
    ''' <param name="FormNumber"></param>
    Public Sub SubmitForm(ByVal FormNumber As Integer)
        For c = 0 To totalPlcNumber - 1
            For Each ctr As Object In plc(c).controlsCollection
                If [Enum].IsDefined(GetType(Control_List.PlcCrtCanBeForms), ctr.GetType.Name) Then
                    If ctr.PLC_FormNumber <> 0 And ctr.PLC_FormNumber = FormNumber Then
                        ctr.pendingWrite = True
                    End If
                End If
            Next
        Next

    End Sub

    Public Sub UpdateForm(ByVal FormNumber As Integer)
        For c = 0 To totalPlcNumber - 1
            For Each ctr As Object In plc(c).controlsCollection
                If [Enum].IsDefined(GetType(Control_List.PlcCrtCanBeForms), ctr.GetType.Name) Then
                    If ctr.PLC_FormNumber <> 0 And ctr.PLC_FormNumber = FormNumber Then
                        ctr.updateForm = True
                        ctr.UpdateControl(plc(c))
                    End If
                End If
            Next
        Next

    End Sub

    Public Sub ClearForm(ByVal FormNumber As Integer)
        For c = 0 To totalPlcNumber - 1
            For Each ctr As Object In plc(c).controlsCollection
                If [Enum].IsDefined(GetType(Control_List.PlcCrtCanBeForms), ctr.GetType.Name) Then
                    If ctr.PLC_FormNumber <> 0 And ctr.PLC_FormNumber = FormNumber Then
                        ctr.ClearControl()
                    End If
                End If
            Next
        Next

    End Sub
End Module
