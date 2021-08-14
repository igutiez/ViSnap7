Imports System.ComponentModel

Module General

    'Used for tasks that only should be executed at the loading of the main form.
    Public firstExecution As Boolean

    'Enable the controls updating
    Public udateControls As Boolean
    'Clients that connect to the PLC. See ViSnap7Setup
    Public plc(KMaxNumberOfPLC) As PlcClient

    'Used in Controls properties
    Public Enum DataArea
        MARK = 1
        DB = 2
        INPUT = 3
        OUTPUT = 4
    End Enum
    'Used in controls properties
    Public Enum DataType
        BOOL = 1
        UINT = 2
        SINT = 3
        INT = 4
        DINT = 5
        REAL = 6
        STR = 7
        CHR = 8
    End Enum

    'This starts the communication with PLC in the backgroud
    Public Sub LaunchCommunications()
        firstExecution = True
        BackgroudTasksLaunch.CyclicLoop.WorkerReportsProgress = True
        BackgroudTasksLaunch.CyclicLoop.RunWorkerAsync()
    End Sub


    ''' <summary>
    ''' Used to set values into Controls from Smart Tags
    ''' </summary>
    ''' <param name="Ctr">Control</param>
    ''' <param name="propName">Property Name</param>
    ''' <returns></returns>
    Public Function GetPropertyByName(ByVal Ctr As Object, ByVal propName As String) As PropertyDescriptor
        Dim prop As PropertyDescriptor = DirectCast(Nothing, PropertyDescriptor)
        prop = TypeDescriptor.GetProperties(Ctr)(propName)

        If prop Is Nothing Then
            Throw New ArgumentException("Invalid Property", propName)
        Else
            Return prop
        End If
    End Function



End Module
