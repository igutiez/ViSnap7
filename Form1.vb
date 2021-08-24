Public Class Form1
    Public pepe As New VS7_RWVariable(Me, 0, DataArea.MARK, DataType.REAL, 0, 400, 0, 0)
    Public juan As New VS7_RWVariable(Me, 0, DataArea.DB, DataType.INT, 1, 0, 0, 0)
    Public registro As New VS7_Register(Me, 0, DataArea.DB, DataType.BOOL, 1, 46, 1, 0)
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        registro.PLC_Folder = "C:\log"
        registro.PLC_TypeTrigger = VS7_Register.TypeTrigger.PULSE_P

        'DO NOT CHANGE THIS LINE
        General.LaunchCommunications()


    End Sub


End Class
