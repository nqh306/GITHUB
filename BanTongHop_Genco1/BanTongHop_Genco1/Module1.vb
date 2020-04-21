Imports System.Data.SQLite

Module Module1
    Public Sub SQL_QUERY(LINK_DATABASE As String, write_log As Boolean, sql_string As String)
        Dim TimerStart As DateTime = Now
        Dim showmsg As Boolean = False
        Dim err_msg As String = ""
        Dim MYCONNECTION As SQLiteConnection

        Do
            Try
                MYCONNECTION = New SQLiteConnection("DataSource=" & LINK_DATABASE & ";version=3;new=False;datetimeformat=CurrentCulture;")
                MYCONNECTION.Open()

                Dim CMD As New SQLiteCommand
                CMD.CommandText = sql_string
                CMD.Connection = MYCONNECTION
                Dim RDR As SQLiteDataReader = CMD.ExecuteReader
                If write_log = True Then
                    Console.WriteLine("[SQL_QUERY] - '" & sql_string & "'")
                End If
                MYCONNECTION.Close()
                Exit Do
            Catch ex As Exception
                MYCONNECTION.Close()
                If write_log = True Then
                    Console.WriteLine("[SQL_QUERY] - '" & sql_string & "'" & Chr(10) & "Error Message: " & ex.Message)
                End If

                err_msg = ex.ToString

                If err_msg.Contains("used by another process") = True Then
                    showmsg = False
                Else
                    If err_msg.Contains("locked") = True Then
                        showmsg = False
                    Else
                        Exit Do
                        showmsg = True
                    End If
                End If

                Dim TimeSpent As System.TimeSpan = Now.Subtract(TimerStart)
                If TimeSpent.TotalSeconds > 60 Then
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
End Module
