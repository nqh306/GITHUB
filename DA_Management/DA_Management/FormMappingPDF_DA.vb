Imports DevExpress.XtraGrid.Views
Imports DevExpress.XtraGrid.Views.Grid

Public Class FormMappingPDF_DA
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public link_app_config As String = appPath & "App_Config.txt"
    Public folder_path As String
    Public folder_path_save_file As String
    Public link_database_DA As String

    Sub refresh_link()
        folder_path = SQL_QUERY_TO_STRING(link_app_config, "SELECT Field_Value FROM Config WHERE Field_Name = 'Link_Folder_Path_DA'")
        If folder_path.Substring(folder_path.Length - 1, 1) <> "\" Then
            folder_path = folder_path & "\"
        End If
        folder_path_save_file = folder_path & "ARCHIVE"
        link_database_DA = folder_path & "Database_DA.txt"
    End Sub

    Private Sub FormMappingPDF_DA_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        refresh_link()


        'GridControl1.DataSource = DT
        Dim DT As DataTable = SQL_QUERY_TO_DATATABLE(link_database_DA, "SELECT DISTINCT File_Name FROM Main_DA")
        For Each Drr As DataRow In DT.Rows
            GridView1.AddNewRow()
        Next



        'For i = 0 To GridView1.RowCount - 1
        '    If My.Computer.FileSystem.FileExists(folder_path_save_file & "\" & GridView1.GetRowCellValue(i, "File_Name").ToString()) Then
        '        GridView1.SetRowCellValue(i, GridView1.Columns(1), "True")
        '    End If
        'Next



    End Sub

End Class