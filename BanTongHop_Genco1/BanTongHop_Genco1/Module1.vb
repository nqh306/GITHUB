Imports System.Data.SQLite

Module Module1
    Public EncryptDecrypt As New Simple3Des("0915330999")
    Public user_login As String = ""
    Public link_folder_database As String = ""
    Public global_config As String = "W:\App_BanTongHop\app_config.txt"
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory

    Public Sub WriteErrorLog(strErrorText As String)
        Dim strPath_Log As String = "W:\App_BanTongHop\"

        strPath_Log = strPath_Log & "LOG\Error\"

        If (Not System.IO.Directory.Exists(strPath_Log)) Then
            System.IO.Directory.CreateDirectory(strPath_Log)
        End If

        Dim strFileName As String = "errorLog_" & Environment.UserName & ".txt"
        System.IO.File.AppendAllText(strPath_Log & strFileName, (DateTime.Now.ToString() & Convert.ToString(" - ")) + strErrorText + vbCr + vbLf)
        WriteLog_Full((DateTime.Now.ToString() & Convert.ToString(" - ")) + strErrorText + vbCr + vbLf)
    End Sub

    Public Sub WriteLog_Full(strText As String)
        Try
            Dim strPath_Log As String = "W:\App_BanTongHop\"

            strPath_Log = strPath_Log & "LOG\Full\"
            Dim strFileName As String = "Log_ACS_" & Environment.UserName & "_" & Now.ToString("yyyyMMdd") & ".txt"

            If (Not System.IO.Directory.Exists(strPath_Log)) Then
                System.IO.Directory.CreateDirectory(strPath_Log)
            End If

            System.IO.File.AppendAllText(strPath_Log & strFileName, (DateTime.Now.ToString() & Convert.ToString(" - ")) + strText + vbLf + vbLf)

        Catch ex As Exception
            WriteErrorLog("Error in WriteErrorLog: " + ex.Message)
        End Try
    End Sub

    Public Function Connect_to_database() As Boolean
        Try
            Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_folder_database & ";version=3;new=False;datetimeformat=CurrentCulture;")
            MYCONNECTION.Open()
            MYCONNECTION.Close()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function SQL_QUERY_TO_BOOLEAN(link_database As String, sql_string As String) As Boolean
        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")

        Try
            MYCONNECTION.Open()
            Dim CMD As New SQLiteCommand
            CMD.CommandText = sql_string
            CMD.Connection = MYCONNECTION
            Dim RDR As SQLiteDataReader = CMD.ExecuteReader
            Dim DT As New DataTable
            DT.Load(RDR)
            RDR.Close()
            Dim result As Boolean
            If DT.Rows.Count = 0 Then
                result = False
            Else
                If DT.Rows(0).Item(0).ToString.Length = 0 Then
                    result = False
                Else
                    result = DT.Rows(0).Item(0)
                End If
            End If
            Return result
        Catch ex As Exception
            MsgBox(ex.Message, vbCritical)
            Return Nothing
        End Try
    End Function
    Public Sub SQL_QUERY(LINK_DATABASE As String, writelog As Boolean, sql_string As String)
        Dim TimerStart As DateTime = Now
        Dim showmsg As Boolean = False
        Dim err_msg As String = ""
        Dim MYCONNECTION As New SQLiteConnection

        Do
            Try
                MYCONNECTION = New SQLiteConnection("DataSource=" & LINK_DATABASE & ";version=3;new=False;datetimeformat=CurrentCulture;")
                MYCONNECTION.Open()

                Dim CMD As New SQLiteCommand
                CMD.CommandText = sql_string
                CMD.Connection = MYCONNECTION
                Dim RDR As SQLiteDataReader = CMD.ExecuteReader
                MYCONNECTION.Close()
                If writelog = True Then
                    WriteLog_Full("[SQL_QUERY] - " & sql_string & " - Status: Completed")
                End If
                Exit Do
            Catch ex As Exception
                If writelog = True Then
                    WriteLog_Full("[SQL_QUERY] - " & sql_string & " - Error Message: " & ex.Message)
                End If

                MYCONNECTION.Close()
                err_msg = ex.ToString

                If err_msg.Contains("used by another process") = True Then
                    showmsg = False
                Else
                    If err_msg.Contains("locked") = True Then
                        showmsg = False
                    Else
                        showmsg = True
                        Exit Do
                    End If
                End If

                Dim TimeSpent As System.TimeSpan = Now.Subtract(TimerStart)
                If TimeSpent.TotalSeconds > 60 Then
                    showmsg = True
                    Exit Do
                End If
            End Try
        Loop
        If showmsg = True Then
            MsgBox(err_msg, vbCritical)
        End If
    End Sub

    Public Function SQL_QUERY_TO_DATATABLE(link_database As String, sql_string As String) As DataTable
        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")
        Try
            MYCONNECTION.Open()

            Dim CMD As New SQLiteCommand
            CMD.CommandText = sql_string
            CMD.Connection = MYCONNECTION
            Dim RDR As SQLiteDataReader = CMD.ExecuteReader
            Dim DT As New DataTable
            DT.Load(RDR)
            RDR.Close()
            MYCONNECTION.Close()
            Return DT
        Catch ex As Exception
            MYCONNECTION.Close()
            Call MsgBox("Contact To Administrator !!!" & Chr(10) & Chr(10) & ex.Message, vbCritical)
            Return Nothing
        End Try
    End Function

    Public Function SQL_QUERY_TO_STRING(link_database As String, sql_string As String) As String
        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")

        Try
            MYCONNECTION.Open()
            Dim CMD As New SQLiteCommand
            CMD.CommandText = sql_string
            CMD.Connection = MYCONNECTION
            Dim RDR As SQLiteDataReader = CMD.ExecuteReader
            Dim DT As New DataTable
            Dim result As String = ""
            DT.Load(RDR)
            RDR.Close()
            MYCONNECTION.Close()

            If DT.Rows.Count > 0 Then
                result = DT.Rows(0).Item(0).ToString
            Else
                result = ""
            End If

            Return result
        Catch ex As Exception
            MYCONNECTION.Close()
            MsgBox(ex.Message, vbCritical)
            Return ""
        End Try
    End Function

    Public Function SQL_QUERY_TO_INTEGER(link_database As String, sql_string As String) As Integer
        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")

        Try
            MYCONNECTION.Open()
            Dim CMD As New SQLiteCommand
            CMD.CommandText = sql_string
            CMD.Connection = MYCONNECTION
            Dim RDR As SQLiteDataReader = CMD.ExecuteReader
            Dim DT As New DataTable
            DT.Load(RDR)
            RDR.Close()
            MYCONNECTION.Close()
            Dim result As Integer
            If DT.Rows.Count = 0 Then
                result = 0
            Else
                If DT.Rows(0).Item(0).ToString.Length = 0 Then
                    result = 0
                Else
                    result = CInt(DT.Rows(0).Item(0).ToString)
                End If
            End If
            MYCONNECTION.Close()

            Return result
        Catch ex As Exception
            MYCONNECTION.Close()
            MsgBox(ex.Message, vbCritical)
            Return Nothing
        End Try
    End Function

End Module
