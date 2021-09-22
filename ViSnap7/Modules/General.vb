Imports System.ComponentModel
''' <summary>
''' General module of communication
''' </summary>
Public Module General

    'Used for tasks that only should be executed at the loading of the main form.
    Public firstExecution As Boolean

    'Enable the controls updating
    Public udateControls As Boolean
    'Clients that connect to the PLC. See ViSnap7Setup
    Public plc(KMaxNumberOfPLC) As PlcClient

    ''' <summary>
    ''' Separator character between data in saved files
    ''' </summary>
    Public KColumSeparator As String = " ; "

    ''' <summary>
    ''' Type of data area: used in Controls properties
    ''' </summary>
    Public Enum DataArea
        MARK = 1
        DB = 2
        INPUT = 3
        OUTPUT = 4
    End Enum


    ''' <summary>
    ''' Variable type: used in controls properties
    ''' </summary>
    Public Enum DataType
        BOOL = 1
        USINT = 2
        SINT = 3
        INT = 4
        DINT = 5
        REAL = 6
        STR = 7
        CHR = 8
    End Enum

    'This starts the communication with PLC in the backgroud
    Public Sub LaunchCommunications()
        'Flag to execute some instructions at the begining
        firstExecution = True
        'Tasks to be performed recurrently
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

    Public Function GetTypeControls(Of T As Control)(
        parentContainer As Control, includeInheritedControls As Boolean) As List(Of T)

        If (parentContainer Is Nothing) Then Return Nothing

        Dim controls As New List(Of T)

        ' System.Type para el tipo de dato especificado.
        '
        Dim typeT As Type = GetType(T)

        For Each ctrl As Control In parentContainer.Controls

            If (includeInheritedControls) Then
                If (TypeOf ctrl Is T) Then _
                    controls.Add(DirectCast(ctrl, T))

            Else
                ' System.Type del control.
                '
                Dim typeControl As Type = ctrl.GetType()
                If (typeControl.Equals(typeT)) Then _
                    controls.Add(DirectCast(ctrl, T))

            End If

            controls.AddRange(GetTypeControls(Of T)(ctrl, includeInheritedControls))

        Next

        Return controls

    End Function

    Public Function WriteLogTextOnFile(ByVal folder As String, ByVal text As String) As Boolean
        Dim result As Boolean = False
        Dim filename As String = Now.Date.ToShortDateString.Replace("/", "-")
        Dim completeFileName As String = folder + "\" + filename + ".txt"


        Try
            Dim objWriter As New System.IO.StreamWriter(completeFileName, True)
            objWriter.Write(text)
            objWriter.Close()
            result = True
        Catch ex As Exception
            result = False
        End Try

        Return result
    End Function


End Module
