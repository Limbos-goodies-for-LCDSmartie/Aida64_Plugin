Imports System.Collections.Generic
Imports System.IO
Imports System.Reflection
Imports System.Text
Imports Microsoft.Win32
Public Class LCDSmartie

    Public Function function1(ByVal param1 As String, ByVal param2 As String) As String
        If LCase(param1) = "about" Then
            Return "example: $dll (aida,1,TGPU1DIO,label)"
            Exit Function
        End If


        Dim Out As String
        Dim sAns As String
        Dim sErr As String = ""

        sAns = RegValue(RegistryHive.CurrentUser,
          "Software\FinalWire\AIDA64\SensorValues",
          param2 & "." & param1, sErr)
        If Trim(sAns) <> "" Then
            Out = sAns ' Debug.WriteLine("Value = " & sAns)
        Else
            Out = "x"
        End If


        Return Out

    End Function


    Public Function function2(ByVal param1 As String, ByVal param2 As String) As String
        If LCase(param1) = "about" Then
            Return "example: $dll (aida,2,TGPU1DIO,1)"
            Exit Function
        End If


        Dim Out As String
        Dim sAns As String
        Dim sErr As String = ""

        sAns = RegValue(RegistryHive.CurrentUser,
          "Software\FinalWire\AIDA64\SensorValues",
          "value." & param1, sErr)
        If Trim(sAns) <> "" Then
            Out = sAns ' Debug.WriteLine("Value = " & sAns)
        Else
            Out = "x"
        End If


        Return Out
    End Function

    Public Function function3(ByVal param1 As String, ByVal param2 As String) As String
        If LCase(param1) = "about" Then
            Return "example: $dll (aida,3,TGPU1DIO,1)"
            Exit Function
        End If

        Dim Out As String
        Dim sAns As String
        Dim sErr As String = ""

        sAns = RegValue(RegistryHive.CurrentUser,
          "Software\FinalWire\AIDA64\SensorValues",
          "label." & param1, sErr)
        If Trim(sAns) <> "" Then
            Out = sAns ' Debug.WriteLine("Value = " & sAns)
        Else
            Out = "x"
        End If


        Return Out
    End Function


    Public Function function19(ByVal param1 As String, ByVal param2 As String) As String
        Dim result = GetAllRegs()
        Return result
    End Function

    Public Function function20(ByVal param1 As String, ByVal param2 As String) As String
        If LCase(param1) = "about" Then
            Return "Creator Nikos Georgousis "
            Exit Function
        End If

        Return "Aida64 plugin 2.1 by Limbo "
    End Function




    Public Function GetMinRefreshInterval() As Integer
        Return 300 ' 300 ms (around 3 times a second)
    End Function


    Public Function RegValue(ByVal Hive As RegistryHive, ByVal Key As String, ByVal ValueName As String, Optional ByRef ErrInfo As String = "") As String
        Dim objParent As RegistryKey
        Dim objSubkey As RegistryKey
        Dim sAns As String
        Select Case Hive
            Case RegistryHive.ClassesRoot
                objParent = Registry.ClassesRoot
            Case RegistryHive.CurrentConfig
                objParent = Registry.CurrentConfig
            Case RegistryHive.CurrentUser
                objParent = Registry.CurrentUser
            Case RegistryHive.DynData
                objParent = Registry.DynData
            Case RegistryHive.LocalMachine
                objParent = Registry.LocalMachine
            Case RegistryHive.PerformanceData
                objParent = Registry.PerformanceData
            Case RegistryHive.Users
                objParent = Registry.Users

        End Select

        Try
            objSubkey = objParent.OpenSubKey(Key)
            'if can't be found, object is not initialized
            If Not objSubkey Is Nothing Then
                sAns = (objSubkey.GetValue(ValueName))
            End If

        Catch ex As Exception

            ErrInfo = ex.Message
        Finally

            'if no error but value is empty, populate errinfo
            If ErrInfo = "" And sAns = "" Then
                ErrInfo =
                   "No value found for requested registry key"
            End If
        End Try
        Return sAns
    End Function



    ' Function to check if content with the same name has already been written
    Private Function contentAlreadyWritten(name As String) As Boolean
        ' Implement your logic here to check if the content with the same name already exists in the file
        ' Return True if it exists, False otherwise
        ' You may need to read the existing file content and compare with 'name'
        ' For simplicity, I'm returning False here
        Return False
    End Function



    Public Function GetAllRegs()
        Dim dllPath As String = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
        Dim ListFile = Path.Combine(dllPath, "Aida64_Values.txt")
        If File.Exists(ListFile) Then
            Return "Aida64_Values.txt file exists on plugins folder"
        Else
            Try
                Dim file As System.IO.StreamWriter
                file = My.Computer.FileSystem.OpenTextFileWriter(ListFile, True)
                '   file.WriteLine("This is the text contents.")
                Dim keyPath As String = "Software\FinalWire\AIDA64\SensorValues"
                Dim instance As RegistryKey = Registry.CurrentUser.OpenSubKey(keyPath)

                If instance IsNot Nothing Then
                    ' ListBox1.Items.Clear()

                    For Each valueName As String In instance.GetValueNames()
                        Dim value As Object = instance.GetValue(valueName)

                        If TypeOf value Is String Then
                            file.WriteLine(valueName & " " & vbTab & vbTab & value.ToString())
                            'Dim Vname() = valueName.Split(".")
                            'If valueName.StartsWith("Label") Then
                            '    '   
                            '    Dim exampleString As FormattableString
                            '    exampleString = "Usage: $dll(aida,1," & Vname(1) & ",Label)"
                            '    file.WriteLine(exampleString)
                            '    '$dll(aida,1,TCPUDIO,Label) 

                            '    'Else valueName.StartsWith("Value")
                            '    '    Dim Vname() = valueName.Split(".")
                            '    '    Dim exampleString As FormattableString
                            '    '    exampleString = "Usage: $dll(aida,1," & Vname(1) & ",Value)"

                            '    '    file.WriteLine(exampleString)
                            '    '    file.WriteLine("____________________________________")
                            'End If
                        End If
                    Next

                    instance.Close()
                End If
                file.Close()
            Catch ex As Exception
            End Try

            Return "File saved..."


        End If



    End Function


    'Private Sub ListRegistryValues()
    '    Dim keyPath As String = "Software\FinalWire\AIDA64\SensorValues"
    '    Dim instance As RegistryKey = Registry.CurrentUser.OpenSubKey(keyPath)

    '    If instance IsNot Nothing Then
    '        ListBox1.Items.Clear()

    '        For Each valueName As String In instance.GetValueNames()
    '            Dim value As Object = instance.GetValue(valueName)

    '            If TypeOf value Is String Then
    '                ListBox1.Items.Add(valueName & " " & vbTab & value.ToString())
    '            End If
    '        Next

    '        instance.Close()
    '    End If
    'End Sub



    Public Function SmartieInfo()
        Return "Developer:Nikos Georgousis (Limbo)" & vbNewLine & "Version:2.1"
    End Function

    Public Function SmartieDemo()
        Dim demolist As New StringBuilder()
        demolist.AppendLine("AIDA64 plug-in")
        demolist.AppendLine("Displays info from AIDA64 ")
        demolist.AppendLine("Enable Registry Option on AIDA64 under external applications")
        demolist.AppendLine(" ")
        demolist.AppendLine("----Function 1----")
        demolist.AppendLine(">>> Returns from the registry value or label depending on second parameter <<<")
        demolist.AppendLine("Label of CPU Diode (temperature) TCPUDIO $dll(aida,1,TCPUDIO,Label)  ")
        demolist.AppendLine("Value of CPU Diode (temperature) TCPUDIO $dll(aida,1,TCPUDIO,Value)  ")
        demolist.AppendLine(" ")
        demolist.AppendLine("For a list of the available keywords on your system use the provided ")
        demolist.AppendLine("AIDA64 keys list utility ")
        demolist.AppendLine(" ")
        demolist.AppendLine("----Function 2----")
        demolist.AppendLine(">>> Returns the VALUE of the selected from the registry <<<")
        demolist.AppendLine("Value of CPU Utilization SCPUUTI $dll(aida,2,SCPUUTI,)  ")
        demolist.AppendLine("Value of System Uptime SUPTIME $dll(aida,2,SUPTIME,)  ")
        demolist.AppendLine(" ")
        demolist.AppendLine("----Function 3----")
        demolist.AppendLine(">>> Returns the LABEL of the selected key from the registry <<<")
        demolist.AppendLine("Label of CPU Utilization SCPUUTI $dll(aida,2,SCPUUTI,)  ")
        demolist.AppendLine("Label of System Uptime SUPTIME $dll(aida,2,SUPTIME,)  ")
        demolist.AppendLine("Label of CPU clock SCPUCLK $dll(aida,2,SCPUCLK,)  ")
        demolist.AppendLine("Label of System Mem speed SMEMSPEED $dll(aida,2,SMEMSPEED,)  ")
        demolist.AppendLine(" ")
        demolist.AppendLine("-------------------------------------------------------------------------")
        demolist.AppendLine("If x is returned then no value exists on the registry")
        demolist.AppendLine("Check if AIDA64 is running and if the registry option is enabled")
        demolist.AppendLine(" ")
        demolist.AppendLine("----Function 19----")
        demolist.AppendLine(">>> Saves a Aida64_Values.txt file on plug-ins folder with all the available values <<<")
        demolist.AppendLine("Run once: $dll(aida,19, , )")
        demolist.AppendLine("----function 20----")
        demolist.AppendLine(">>> Credits <<<")
        demolist.AppendLine("About $dll(aida,20,,)")
        demolist.AppendLine("-------------------------------------------------------------------------")

        demolist.AppendLine(" *** Visit ***")
        demolist.AppendLine("> Home page")
        demolist.AppendLine("https://lcdsmartie.sourceforge.net")
        demolist.AppendLine("> Forums")
        demolist.AppendLine("https://www.lcdsmartie.org")
        demolist.AppendLine("> Active development branch (latest version)")
        demolist.AppendLine("https://github.com/stokie-ant/lcdsmartie-laz")
        demolist.AppendLine("")



        Dim result As String = demolist.ToString()
        Return result

    End Function




End Class
