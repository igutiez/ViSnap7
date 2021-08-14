Module UpdateFormsTasks
    ''' <summary>
    ''' It is used for reading/writing the control. 
    ''' </summary>
    ''' <param name="enable">Enable or not control updating</param>
    Public Sub ReadControls(ByVal enable As Boolean)
        Dim c As Integer
        If enable Then
            For c = 0 To totalPlcNumber - 1
                For Each ctr As Object In plc(c).controlsCollection
                    ctr.UpdateControl(plc(c))
                Next

            Next
        End If

    End Sub

End Module
