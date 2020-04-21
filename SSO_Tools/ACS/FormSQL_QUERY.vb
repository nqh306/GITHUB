Imports System.Data.SQLite
Imports DevExpress.XtraEditors.Controls

Public Class FormSQL_QUERY
    Public CONNECTION_SSO As SQLiteConnection

    Public Sub Taoketnoi_SSO()
        CONNECTION_SSO = New SQLiteConnection("DataSource=" & tbLinkFile.Text & ";version=3;new=False;datetimeformat=CurrentCulture;Password=" & tbPassword.Text & ";")
        CONNECTION_SSO.Open()
    End Sub

    Private Sub FormSQL_QUERY_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub tbLinkFile_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles tbLinkFile.ButtonClick
        Try

            Dim fd As OpenFileDialog = New OpenFileDialog()

            fd.Title = "Select database"
            fd.InitialDirectory = "C:\"
            fd.Filter = "All File (*.*)|*.*"

            fd.FilterIndex = 1
            fd.RestoreDirectory = True

            If fd.ShowDialog() = DialogResult.OK Then
                If fd.FileName.Length > 0 Then
                    tbLinkFile.Text = fd.FileName
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message, vbCritical + vbOKOnly)
            WriteErrorLog(ex.ToString)
        End Try
    End Sub

    Private Sub btCreateConnection_Click(sender As Object, e As EventArgs) Handles btCreateConnection.Click
        Try

            If tbLinkFile.Text.Length = 0 Then
                Exit Sub
            End If

            Taoketnoi_SSO()
            Dim DT_list_table_in_database As DataTable = GET_ALL_TABLE_NAME_IN_DATABASE(CONNECTION_SSO)

            For Each Drr As DataRow In DT_list_table_in_database.Rows
                Dim Column_name As String = ""
                Dim DT_GET_COLUMN As DataTable = SQL_FROMFILE_TO_DATATABLE(tbLinkFile.Text, "SELECT * FROM " & Drr(0).ToString & " LIMIT 1")
                For i = 0 To DT_GET_COLUMN.Columns.Count - 1
                    If Column_name.Length = 0 Then
                        Column_name = "[" & DT_GET_COLUMN.Columns(i).ColumnName.ToString() & "]"
                    Else
                        Column_name = Column_name & ", [" & DT_GET_COLUMN.Columns(i).ColumnName.ToString() & "]"
                    End If
                Next
                tbNote.Text = tbNote.Text & Drr(0).ToString & Chr(13) & Chr(10) & Column_name & Chr(13) & Chr(10) & "__________________________" & Chr(13) & Chr(10)
            Next

        Catch ex As Exception
            MsgBox(ex.Message, vbCritical + vbOKOnly)
            WriteErrorLog(ex.ToString)
        End Try
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Try
            Dim DT As DataTable = SQL_FROMFILE_TO_DATATABLE(tbLinkFile.Text, tbStrSQL.Text)
            GridView1.Columns.Clear()
            GridControl1.DataSource = DT
            GridView1.BestFitColumns()

        Catch ex As Exception
            MsgBox(ex.Message, vbCritical + vbOKOnly)
            WriteErrorLog(ex.ToString)
        End Try
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        Try
            EXPORT_GRIDVIEW_TO_EXCEL(GridView1)
        Catch ex As Exception
            MsgBox(ex.Message, vbCritical + vbOKOnly)
            WriteErrorLog(ex.ToString)
        End Try
    End Sub
End Class