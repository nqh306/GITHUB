Imports System.IO
Imports System.Data.SQLite

Public Class formSDM
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public local_app_config As String = appPath & "App_Config.txt"
    Public global_app_config As String = SQL_FROMFILE_TO_STRING_NO_LOG(local_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Link_Global_Config'")

    Private Sub formSDM_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SplitContainerControl1.SplitterPosition = 330
    End Sub

    Private Sub cb_DOB_CheckedChanged(sender As Object, e As EventArgs) Handles cb_DOB.CheckedChanged
        If cb_DOB.Checked = True Then
            tbDOB_LITS.Enabled = True
        Else
            tbDOB_LITS.Enabled = False
        End If
    End Sub

    Private Sub cb_MailingAddress_CheckedChanged(sender As Object, e As EventArgs) Handles cb_MailingAddress.CheckedChanged
        If cb_MailingAddress.Checked = True Then
            tbMailAdd_LITS1.Enabled = True
            tbMailAdd_LITS2.Enabled = True
            tbMailAdd_LITS3.Enabled = True
            tbMailAdd_LITS_City.Enabled = True
            tbMailAdd_LITS_Country.Enabled = True
        Else
            tbMailAdd_LITS1.Enabled = False
            tbMailAdd_LITS2.Enabled = False
            tbMailAdd_LITS3.Enabled = False
            tbMailAdd_LITS_City.Enabled = False
            tbMailAdd_LITS_Country.Enabled = False
        End If
    End Sub

    Private Sub cb_ResidentAdd_CheckedChanged(sender As Object, e As EventArgs) Handles cb_ResidentAdd.CheckedChanged
        If cb_ResidentAdd.Checked = True Then
            tbResAdd_LITS1.Enabled = True
            tbResAdd_LITS2.Enabled = True
            tbResAdd_LITS3.Enabled = True
            tbResAdd_LITS_City.Enabled = True
            tbResAdd_LITS_Country.Enabled = True
        Else
            tbResAdd_LITS1.Enabled = False
            tbResAdd_LITS2.Enabled = False
            tbResAdd_LITS3.Enabled = False
            tbResAdd_LITS_City.Enabled = False
            tbResAdd_LITS_Country.Enabled = False
        End If
    End Sub

    Private Sub cbCurrentPosition_CheckedChanged(sender As Object, e As EventArgs) Handles cbCurrentPosition.CheckedChanged
        If cbCurrentPosition.Checked = True Then
            tbCurrentPosition_LITS.Enabled = True
        Else
            tbCurrentPosition_LITS.Enabled = False
        End If
    End Sub

    Private Sub cbEBCode_CheckedChanged(sender As Object, e As EventArgs) Handles cbEBCode.CheckedChanged
        If cbEBCode.Checked = True Then
            ComboBox_EBCode.Enabled = True
        Else
            ComboBox_EBCode.Enabled = False
        End If
    End Sub

    Private Sub SplitContainerControl1_DoubleClick(sender As Object, e As EventArgs) Handles SplitContainerControl1.DoubleClick
        SplitContainerControl1.SplitterPosition = 330
    End Sub

    Private Sub btImportLITS_Click(sender As Object, e As EventArgs) Handles btImportLITS.Click

    End Sub

    Function Get_path_latest_Source_eBBs(Format_Source As String) As String
        Dim result As String = ""
        For i As Integer = 0 To 15
            Dim TxnDate As DateTime = Now()
            TxnDate = TxnDate.AddDays(-i)
            Dim strDate_yyyy As String = Format(TxnDate, "yyyy")
            Dim strDate_MM As String = Format(TxnDate, "MM")
            Dim strDate_M As Integer = CInt(strDate_MM)
            Dim strDate_dd As String = Format(TxnDate, "dd")
            Dim strDate_d As String = CInt(strDate_dd)
            Dim strDate_MMM As String = Format(TxnDate, "MMM")

            Dim file_source As String
            For j As Integer = 1 To SQL_FROMFILE_COUNT_ROW(global_app_config, "SELECT Field_Value_1 FROM Config WHERE [Field_Name] LIKE '%" & Format_Source & "%'")
                file_source = SQL_QUERY_TO_STRING(global_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = '" & Format_Source & j & "'")
                file_source = Replace(file_source, "strDate_yyyy", strDate_yyyy)
                file_source = Replace(file_source, "strDate_MMM", strDate_MMM)
                file_source = Replace(file_source, "strDate_MM", strDate_MM)
                file_source = Replace(file_source, "strDate_M", strDate_M)
                file_source = Replace(file_source, "strDate_dd", strDate_dd)
                file_source = Replace(file_source, "strDate_d", strDate_d)
                If My.Computer.FileSystem.FileExists(file_source) Then
                    result = file_source
                    Exit For
                End If
            Next
        Next
        WriteLog_Full("[Get latest CUSTCONT] - [" & Format_Source & "] " & result)
        Return result
    End Function

    Public Function IMPORT_CUSTCONT_REPORT(LINK_CUSTCONT As String, LINK_TEMP_DATABASE As String, TABLE_NAME As String) As Boolean
        Try
            Dim LINK_CUSTCONT2 As String = ""
            If Path.GetExtension(LINK_CUSTCONT) = "zip" Then
                LINK_CUSTCONT2 = Unzip_To_File_Path(LINK_CUSTCONT, appPath)
                LINK_CUSTCONT = LINK_CUSTCONT2
            End If

            If Path.GetExtension(LINK_CUSTCONT) = "csv" Then
                Dim csvFilePath As String = System.IO.Path.GetDirectoryName(LINK_CUSTCONT)
                Dim csvFileName As String = System.IO.Path.GetFileName(LINK_CUSTCONT)
                Dim sql_string As String = "SELECT DISTINCT [RELATIONSHIP NO] AS REL_NO, [FULL NAME] AS FULLNAME, [CONTACT TYPE CODE] AS CONTACT_TYPE_CODE, [CONTACT], [ATTENTION PARTY] AS ATTENTION" &
                    " FROM [" & csvFileName & "]" &
                    " WHERE [SEGMENTCODE] NOT IN ('03','15','08','40','44','53','54') AND [CONTACT TYPE CODE] IN ('EMO','EMR','MOB','TOF','PPN')"

                ' Create the SQLite database
                If Not My.Computer.FileSystem.FileExists(LINK_TEMP_DATABASE) Then
                    SQLiteConnection.CreateFile(LINK_TEMP_DATABASE)
                End If

                SQL_QUERY(LINK_TEMP_DATABASE, "CREATE TABLE IF NOT EXISTS " & TABLE_NAME & "(REL_NO VARCHAR, FULLNAME VARCHAR, CONTACT_TYPE_CODE VARCHAR, CONTACT VARCHAR, ATTENTION VARCHAR)")
                ' SQLITE_BULK_COPY(SQL_CSV_TO_DATATABLE(csvFilePath, csvFileName, sql_string), LINK_TEMP_DATABASE, TABLE_NAME)

                If My.Computer.FileSystem.FileExists(LINK_CUSTCONT2) Then
                    Kill(LINK_CUSTCONT2)
                End If

                Return True
            Else
                WriteErrorLog("Is not CSV file - " & LINK_CUSTCONT)
                Return False
            End If

        Catch ex As Exception
            WriteErrorLog(String.Format("Error: {0}", ex.Message))
            Return False
        End Try
    End Function

    Sub Import_CUSTDTL_Portfolio_CUSTCONT_Individual()
        Dim Link_CUSTDTL As String = Get_path_latest_Source_eBBs("Format_Source_CUSTDTL_")
        Dim Link_Portfolio As String = Get_path_latest_Source_eBBs("Format_Source_Portfolio_")
        Dim Link_CUSTCONT As String = Get_path_latest_Source_eBBs("Format_Source_CUSTCONT_")


    End Sub
End Class