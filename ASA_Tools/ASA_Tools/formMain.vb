Imports System.Data.SQLite

Public Class formMain
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public local_app_config As String = appPath & "App_Config.txt"
    Public global_app_config As String
    Public link_folder_database As String
    Public link_database_ASA As String

    Sub Check_Database()
        If Not My.Computer.FileSystem.FileExists(local_app_config) Then
            SQLiteConnection.CreateFile(local_app_config)
        End If

        SQL_QUERY_FROM_FILE_NO_LOG(local_app_config, "CREATE TABLE IF NOT EXISTS Config(Field_Name VARCHAR Not NULL UNIQUE PRIMARY KEY, Field_Value_1 VARCHAR, Field_Value_2 VARCHAR, Field_Value_3 VARCHAR, Field_Value_4 VARCHAR, Field_Value_5 VARCHAR, Field_Value_6 VARCHAR, Field_Value_7 VARCHAR, Field_Value_8 VARCHAR, Field_Value_9 VARCHAR)")

        If SQL_FROMFILE_TO_INTEGER_NO_LOG(local_app_config, "Select COUNT(Field_Value_1) FROM Config WHERE [Field_Name] = 'Link_Global_Config'") = 0 Then
            SQL_QUERY_FROM_FILE_NO_LOG(local_app_config, "INSERT INTO Config([Field_Name], [Field_Value_1]) VALUES ('Link_Global_Config','C:\SSetup_w7\Application_Config_ASA.txt');")
        End If

        global_app_config = SQL_FROMFILE_TO_STRING_NO_LOG(local_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Link_Global_Config'")
        If Not My.Computer.FileSystem.FileExists(global_app_config) Then
            SQLiteConnection.CreateFile(global_app_config)
        End If

        SQL_QUERY_FROM_FILE_NO_LOG(global_app_config, "CREATE TABLE IF NOT EXISTS Config(Field_Name VARCHAR NOT NULL UNIQUE PRIMARY KEY, Field_Value_1 VARCHAR, Field_Value_2 VARCHAR, Field_Value_3 VARCHAR, Field_Value_4 VARCHAR, Field_Value_5 VARCHAR, Field_Value_6 VARCHAR, Field_Value_7 VARCHAR, Field_Value_8 VARCHAR, Field_Value_9 VARCHAR)")

        If SQL_FROMFILE_TO_INTEGER_NO_LOG(global_app_config, "SELECT COUNT(Field_Value_1) FROM Config WHERE [Field_Name] = 'Link_Folder_Database'") = 0 Then
            SQL_QUERY_FROM_FILE_NO_LOG(global_app_config, "INSERT INTO Config(Field_Name, Field_Value_1) VALUES ('Link_Folder_Database','C:\SSetup_w7\')")
        End If

        'get link_folder database
        link_folder_database = SQL_QUERY_TO_STRING(global_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Link_Folder_Database'")
        If link_folder_database.Substring(link_folder_database.Length - 1, 1) <> "\" Then
            link_folder_database = link_folder_database & "\"
        End If

        link_database_ASA = link_folder_database & "Database_ASA.txt"

        If Not My.Computer.FileSystem.FileExists(link_database_ASA) Then
            SQLiteConnection.CreateFile(link_database_ASA)
        End If

        If SQL_QUERY_TO_INTEGER(global_app_config, "SELECT COUNT(Field_Value_1) FROM Config WHERE [Field_Name] = 'Format_Source_CUSTCONT_1'") = 0 Then
            SQL_QUERY(global_app_config, "INSERT INTO Config(Field_Name, Field_Value_1) VALUES ('Format_Source_CUSTCONT_1','O:\Account Services_Advice&Statements\strDate_yyyy\strDate_MM\strDate_dd\CUSTCONT.csv.zip')")
        End If
        If SQL_QUERY_TO_INTEGER(global_app_config, "SELECT COUNT(Field_Value_1) FROM Config WHERE [Field_Name] = 'Format_Source_CUSTDTL_1'") = 0 Then
            SQL_QUERY(global_app_config, "INSERT INTO Config(Field_Name, Field_Value_1) VALUES ('Format_Source_CUSTDTL_1','O:\SR_CUSTDTL_FOB\strDate_yyyy\strDate_M\strDate_d\CUSTCONT.csv.zip')")
        End If
        If SQL_QUERY_TO_INTEGER(global_app_config, "SELECT COUNT(Field_Value_1) FROM Config WHERE [Field_Name] = 'Format_Source_Portfolio_1'") = 0 Then
            SQL_QUERY(global_app_config, "INSERT INTO Config(Field_Name, Field_Value_1) VALUES ('Format_Source_Portfolio_1','O:\SR_Portfolio\strDate_yyyy\strDate_M\strDate_d\CUSTCONT.csv.zip')")
        End If

    End Sub

    Private Sub BarButtonItem_SDM_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem_SDM.ItemClick
        ShowForm(formSDM)
        Ribbon.Minimized = True
    End Sub

    Private Sub formMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Check_Database()
    End Sub
End Class