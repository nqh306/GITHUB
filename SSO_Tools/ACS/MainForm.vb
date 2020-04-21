Imports System.ComponentModel
Imports System.Data.SQLite
Imports System.Globalization
Imports System.Reflection
Imports System.Threading
Imports DevExpress.XtraBars
Imports DevExpress.XtraEditors.Controls

Public Class MainForm

    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public local_app_config As String = appPath & "App_Config.txt"
    Public global_app_config As String
    Public link_folder_database As String
    Public link_database_SSO As String
    Dim CONNECTION_SSO As SQLiteConnection
    Dim CONNECTION_CONFIG As SQLiteConnection
    Private currentCulture As System.Globalization.CultureInfo

    Public Sub Taoketnoi_Config()
        CONNECTION_CONFIG = New SQLiteConnection("DataSource=" & global_app_config & ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;")
        CONNECTION_CONFIG.Open()
    End Sub

    Public Sub Taoketnoi_ACS()
        CONNECTION_SSO = New SQLiteConnection("DataSource=" & link_database_SSO & ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;")
        CONNECTION_SSO.Open()
    End Sub

    Public Sub Check_Database()
        If Not My.Computer.FileSystem.FileExists(local_app_config) Then
            SQLiteConnection.CreateFile(local_app_config)
            SetPasswordForNewDatabase(local_app_config)
        End If

        SQL_QUERY_FROM_FILE_NO_LOG(local_app_config, "CREATE TABLE IF NOT EXISTS Config(Field_Name VARCHAR Not NULL UNIQUE PRIMARY KEY, Field_Value_1 VARCHAR, Field_Value_2 VARCHAR, Field_Value_3 VARCHAR, Field_Value_4 VARCHAR, Field_Value_5 VARCHAR, Field_Value_6 VARCHAR, Field_Value_7 VARCHAR, Field_Value_8 VARCHAR, Field_Value_9 VARCHAR)")

        If SQL_FROMFILE_TO_INTEGER_NO_LOG(local_app_config, "Select COUNT(Field_Value_1) FROM Config WHERE [Field_Name] = 'Link_Global_Config'") = 0 Then
            SQL_QUERY_FROM_FILE_NO_LOG(local_app_config, "INSERT INTO Config([Field_Name], [Field_Value_1]) VALUES ('Link_Global_Config','C:\SSetup_w7\Application_Config_SSO.txt');")
        End If

        global_app_config = SQL_FROMFILE_TO_STRING_NO_LOG(local_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Link_Global_Config'")
        If Not My.Computer.FileSystem.FileExists(global_app_config) Then
            SQLiteConnection.CreateFile(global_app_config)
            SetPasswordForNewDatabase(global_app_config)
            Taoketnoi_Config()
        Else
            Taoketnoi_Config()
        End If

        SQL_QUERY_FROM_FILE_NO_LOG(global_app_config, "CREATE TABLE IF NOT EXISTS Config(Field_Name VARCHAR NOT NULL UNIQUE PRIMARY KEY, Field_Value_1 VARCHAR, Field_Value_2 VARCHAR, Field_Value_3 VARCHAR, Field_Value_4 VARCHAR, Field_Value_5 VARCHAR, Field_Value_6 VARCHAR, Field_Value_7 VARCHAR, Field_Value_8 VARCHAR, Field_Value_9 VARCHAR)")

        If SQL_FROMFILE_TO_INTEGER_NO_LOG(global_app_config, "SELECT COUNT(Field_Value_1) FROM Config WHERE [Field_Name] = 'Link_Folder_Database'") = 0 Then
            SQL_QUERY_FROM_FILE_NO_LOG(global_app_config, "INSERT INTO Config(Field_Name, Field_Value_1) VALUES ('Link_Folder_Database','C:\SSetup_w7\')")
        End If

        'get link_folder database
        link_folder_database = SQL_QUERY_TO_STRING(CONNECTION_CONFIG, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Link_Folder_Database'")
        If link_folder_database.Substring(link_folder_database.Length - 1, 1) <> "\" Then
            link_folder_database = link_folder_database & "\"
        End If

        link_database_SSO = link_folder_database & "Database_SSO.txt"

        If Not My.Computer.FileSystem.FileExists(link_database_SSO) Then
            SQLiteConnection.CreateFile(link_database_SSO)
            SetPasswordForNewDatabase(link_database_SSO)
        End If

        Taoketnoi_ACS()

        SQL_QUERY(CONNECTION_CONFIG, "CREATE TABLE IF NOT EXISTS DATABASE_USER(BANK_ID VARCHAR NOT NULL UNIQUE PRIMARY KEY, USER_MAKER BOOLEAN, USER_CHECKER BOOLEAN, USER_ADMIN BOOLEAN, STATUS BOOLEAN, Password_Str VARCHAR)")
        If SQL_QUERY_TO_INTEGER(CONNECTION_CONFIG, "SELECT COUNT(BANK_ID) FROM DATABASE_USER WHERE [BANK_ID] = 'Admin'") = 0 Then
            Dim Password_store_db As String = EncryptDecrypt.EncryptData("123")
            SQL_QUERY(CONNECTION_CONFIG, "INSERT INTO DATABASE_USER(BANK_ID, USER_MAKER, USER_CHECKER, USER_ADMIN, STATUS, Password_Str) VALUES ('Admin',1,1,1,1,'" & Password_store_db & "')")
        End If
    End Sub

    Sub Check_user_access()
        Dim form1 As New FormReconcilePaymentInstruction
        Dim form2 As New FormConfig
        Dim form3 As New FormUser
        Dim form4 As New FormChangePW
        Dim form5 As New FormLogin

        Dim frmCollection = System.Windows.Forms.Application.OpenForms
        For i As Int16 = 0I To frmCollection.Count - 1I
            If frmCollection.Item(i).Name = form1.Name Or frmCollection.Item(i).Name = form2.Name Or frmCollection.Item(i).Name = form3.Name Or frmCollection.Item(i).Name = form4.Name Or frmCollection.Item(i).Name = form5.Name Then
                frmCollection.Item(i).Close()
            End If
        Next i

        Dim user_name As String = BarEditItem_User.EditValue

        BarButtonItem_UserFunction.Enabled = False
        BarButtonItem_PaymentInstruction.Enabled = False
        BarButtonItem_Config.Enabled = False

        If SQL_QUERY_TO_BOOLEAN(CONNECTION_CONFIG, "SELECT USER_MAKER FROM DATABASE_USER WHERE Bank_ID = '" & user_name & "'") = True Then
            BarButtonItem_PaymentInstruction.Enabled = True
        End If

        If SQL_QUERY_TO_BOOLEAN(CONNECTION_CONFIG, "SELECT USER_CHECKER FROM DATABASE_USER WHERE Bank_ID = '" & user_name & "'") = True Then
            BarButtonItem_PaymentInstruction.Enabled = True
        End If

        If SQL_QUERY_TO_BOOLEAN(CONNECTION_CONFIG, "SELECT USER_ADMIN FROM DATABASE_USER WHERE Bank_ID = '" & user_name & "'") = True Then
            BarButtonItem_Config.Enabled = True
            BarButtonItem_UserFunction.Enabled = True
        End If

    End Sub

    Private Sub MainForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'load information application version
        StatusBar_Version.Caption = My.Application.Info.ProductName.ToString & " v" & Assembly.GetExecutingAssembly().GetName().Version.ToString()
        Check_Database() 'check and create database if not exists

        'check user access
        BarEditItem_User.EditWidth = 150
        BarEditItem_User.EditValue = Environment.UserName
        Check_user_access()

        Dim tmpCurrentCulture As System.Globalization.CultureInfo
        tmpCurrentCulture = New System.Globalization.CultureInfo("en-US")
        currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        tmpCurrentCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"
        System.Threading.Thread.CurrentThread.CurrentCulture = tmpCurrentCulture
    End Sub

    Private Sub BarButtonItem_Quit_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonItem_Quit.ItemClick
        Dim Response = MsgBox("Do you really want to exit application?", MessageBoxButtons.YesNo)
        If Response = vbYes Then
            Me.Close()
        End If
    End Sub

    Private Sub RepositoryItemButtonEdit_ChangeUser_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles RepositoryItemButtonEdit_ChangeUser.ButtonClick
        Check_user_access()
        Dim f As New FormLogin
        f.Show()
    End Sub

    Private Sub BarButtonItem_UserFunction_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonItem_UserFunction.ItemClick
        ShowForm(FormUser)
    End Sub

    Private Sub BarButtonItem_Advice_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonItem_PaymentInstruction.ItemClick
        Ribbon.Minimized = True
        ShowForm(FormReconcilePaymentInstruction)
    End Sub

    Private Sub BarButtonItem_Config_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonItem_Config.ItemClick
        ShowForm(FormConfig)
    End Sub

    Private Sub BarButtonItem_SQL_QUERY_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonItem_SQL_QUERY.ItemClick
        ShowForm(FormSQL_QUERY)
    End Sub

#Region "XuLy nhận lệnh"
    Public Delegate Sub process_data()
    Dim th_NhanDuLieu As Thread
    Public Sub XuLy()
        Try
            While True
                If check_password = True Then
                    Me.BeginInvoke(New process_data(AddressOf Check_user_access))
                    MsgBox("Completed", vbInformation, "Change User")
                End If
                check_password = False
                Thread.Sleep(300)
            End While
        Catch ex As Exception

        End Try

    End Sub

    Public Sub start_thearing()
        th_NhanDuLieu = New Thread(AddressOf XuLy)
        th_NhanDuLieu.IsBackground = True
        th_NhanDuLieu.Start()
    End Sub



#End Region


End Class