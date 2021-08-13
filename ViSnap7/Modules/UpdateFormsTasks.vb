Module UpdateFormsTasks
    Public Sub ReadControls(ByVal enable As Boolean)
        Dim c As Integer
        If enable Then

            For c = 0 To TotalPlcNumber - 1
                For Each ctr As Object In PLC(c).ControlsCollection
                    ctr.UpdateValue(PLC(0))
                Next

            Next
        End If

    End Sub

End Module
