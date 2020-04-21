Imports System.Data.OleDb
Imports System.Data.SQLite
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraSplashScreen
Imports System.IO
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports Microsoft.Office.Interop
Imports System.Globalization
Imports System.Threading

Public Class FormReconcilePaymentInstruction
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public local_app_config As String = appPath & "App_Config.txt"
    Public global_app_config As String = SQL_FROMFILE_TO_STRING(local_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Link_Global_Config'")
    Public link_folder_database As String
    Public link_database_SSO As String
    Public CONNECTION_SSO As SQLiteConnection
    Public CONNECTION_CONFIG As SQLiteConnection

    Sub Taoketnoi_Config()
        CONNECTION_CONFIG = New SQLiteConnection("DataSource=" & global_app_config & ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;")
        CONNECTION_CONFIG.Open()
    End Sub

    Sub Taoketnoi_SSO()
        CONNECTION_SSO = New SQLiteConnection("DataSource=" & link_database_SSO & ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;")
        CONNECTION_SSO.Open()
    End Sub

    Private Sub FormReconcilePaymentInstruction_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Check_Database()
        WindowsFormsSettings.AllowAutoFilterConditionChange = DevExpress.Utils.DefaultBoolean.False

        ImportVSD_GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT * FROM FIXED_DAILY_VSD_REPORT WHERE STR_DATE = '" & Now.ToString("dd/MM/yyyy") & "'")
        ImportVSD_GridView1.BestFitColumns()
        ImportVSD_GridView1.ExpandAllGroups()

        Processing_cbByDate.EditValue = Now

        Processing_cbByStatus.Items.Clear()
        Processing_cbByStatus.Items.Add("All")
        Processing_cbByStatus.Items.Add("Completed")
        Processing_cbByStatus.Items.Add("Incomplete")
        Processing_cbByStatus.Text = "All"

        Processing_GridView_TaskList.BestFitColumns()

        ListView1.View = View.Details
        ListView1.CheckBoxes = True
        ListView1.Columns.Add("", 50, HorizontalAlignment.Left)
        ListView1.Columns.Add("No", 50, HorizontalAlignment.Left)
        ListView1.Columns.Add("File Name", 100, HorizontalAlignment.Left)
        ListView1.Columns.Add("Modified Date", 100, HorizontalAlignment.Left)
        ListView1.Columns.Add("Status", 100, HorizontalAlignment.Left)
        ListView1.Columns.Add("User Imported", 100, HorizontalAlignment.Left)
        ListView1.Columns.Add("File Path", 100, HorizontalAlignment.Left)

        Processing_rbtSelectAll.PerformClick()

        DataMaintenance_cbTableName.Items.Add("FIXED_DAILY_VSD_REPORT")
        DataMaintenance_cbTableName.Items.Add("PAYMENT_INSTRUCTION_REPORT")
        DataMaintenance_cbTableName.Items.Add("INTERNAL_TRANSFER")
        DataMaintenance_cbTableName.Items.Add("Redemption Control Report")
    End Sub

    Public Sub Check_Database()
        'check and create database global_app_config if not exists
        If Not My.Computer.FileSystem.FileExists(global_app_config) Then
            SQLiteConnection.CreateFile(global_app_config)
            SetPasswordForNewDatabase(global_app_config)
            Taoketnoi_Config()
        Else
            Taoketnoi_Config()
        End If

        SQL_QUERY(CONNECTION_CONFIG, "CREATE TABLE IF NOT EXISTS Config(Field_Name VARCHAR NOT NULL UNIQUE PRIMARY KEY, Field_Value_1 VARCHAR, Field_Value_2 VARCHAR, Field_Value_3 VARCHAR, Field_Value_4 VARCHAR, Field_Value_5 VARCHAR, Field_Value_6 VARCHAR, Field_Value_7 VARCHAR, Field_Value_8 VARCHAR, Field_Value_9 VARCHAR)")
        SQL_QUERY(CONNECTION_CONFIG, "CREATE TABLE IF NOT EXISTS LIST_CASH_ACCOUNT(CASE_ID VARCHAR NOT NULL UNIQUE PRIMARY KEY, SYMBOL VARCHAR, CASH_ACCOUNT VARCHAR, VALUE_DATE_ADD_DAYS NUMERIC, DEADLINE_ADD_DAYS NUMERIC, BEN_CHARGE_FOR_LOW_VALUE NUMERIC, BEN_CHARGE_FOR_HIGH_VALUE NUMERIC)")

        'get link_folder database
        link_folder_database = SQL_QUERY_TO_STRING(CONNECTION_CONFIG, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Link_Folder_Database'")
        If link_folder_database.Substring(link_folder_database.Length - 1, 1) <> "\" Then
            link_folder_database = link_folder_database & "\"
        End If

        link_database_SSO = link_folder_database & "Database_SSO.txt"
        If Not My.Computer.FileSystem.FileExists(link_database_SSO) Then
            SQLiteConnection.CreateFile(link_database_SSO)
            SetPasswordForNewDatabase(link_database_SSO)
            Taoketnoi_SSO()
        Else
            Taoketnoi_SSO()
        End If

        If SQL_FROMFILE_TO_INTEGER_NO_LOG(global_app_config, "SELECT COUNT(Field_Value_1) FROM Config WHERE [Field_Name] = 'Code_Setup_Search_VSD_Report_Link_Folder'") = 0 Then
            SQL_QUERY_FROM_FILE_NO_LOG(global_app_config, "INSERT INTO Config(Field_Name, Field_Value_1) VALUES ('Code_Setup_Search_VSD_Report_Link_Folder','C:\SSetup_w7\')")
        End If

        If SQL_FROMFILE_TO_INTEGER_NO_LOG(global_app_config, "SELECT COUNT(Field_Value_1) FROM Config WHERE [Field_Name] = 'Code_Setup_Search_VSD_Report_Format'") = 0 Then
            SQL_QUERY_FROM_FILE_NO_LOG(global_app_config, "INSERT INTO Config(Field_Name, Field_Value_1) VALUES ('Code_Setup_Search_VSD_Report_Format','*.csv')")
        End If

        If SQL_FROMFILE_TO_INTEGER_NO_LOG(global_app_config, "SELECT COUNT(Field_Value_1) FROM Config WHERE [Field_Name] = 'Code_Setup_Search_VSD_Report_Option_AllDirectories'") = 0 Then
            SQL_QUERY_FROM_FILE_NO_LOG(global_app_config, "INSERT INTO Config(Field_Name, Field_Value_1) VALUES ('Code_Setup_Search_VSD_Report_Option_AllDirectories','NO')")
        End If

        If SQL_QUERY_TO_INTEGER(CONNECTION_CONFIG, "SELECT COUNT(Field_Value_1) FROM Config WHERE [Field_Name] = 'Format_Source_VSD_Report'") = 0 Then
            SQL_QUERY(CONNECTION_CONFIG, "INSERT INTO Config(Field_Name, Field_Value_1) VALUES ('Format_Source_VSD_Report','Link_Folder_Source_VSD_Report\strDate_yyyy\strDate_MM\strDate_dd\')")
        End If

        If SQL_QUERY_TO_INTEGER(CONNECTION_CONFIG, "SELECT COUNT(Field_Value_1) FROM Config WHERE [Field_Name] = 'Email_Payment_Instruction_Subject'") = 0 Then
            SQL_QUERY(CONNECTION_CONFIG, "INSERT INTO Config(Field_Name, Field_Value_1) VALUES ('Email_Payment_Instruction_Subject','')")
        End If

        If SQL_QUERY_TO_INTEGER(CONNECTION_CONFIG, "SELECT COUNT(Field_Value_1) FROM Config WHERE [Field_Name] = 'Email_Payment_Instruction_To'") = 0 Then
            SQL_QUERY(CONNECTION_CONFIG, "INSERT INTO Config(Field_Name, Field_Value_1) VALUES ('Email_Payment_Instruction_To','')")
        End If

        If SQL_QUERY_TO_INTEGER(CONNECTION_CONFIG, "SELECT COUNT(Field_Value_1) FROM Config WHERE [Field_Name] = 'Email_Payment_Instruction_CC'") = 0 Then
            SQL_QUERY(CONNECTION_CONFIG, "INSERT INTO Config(Field_Name, Field_Value_1) VALUES ('Email_Payment_Instruction_CC','')")
        End If

        If SQL_QUERY_TO_INTEGER(CONNECTION_CONFIG, "SELECT COUNT(Field_Value_1) FROM Config WHERE [Field_Name] = 'Email_Payment_Instruction_BCC'") = 0 Then
            SQL_QUERY(CONNECTION_CONFIG, "INSERT INTO Config(Field_Name, Field_Value_1) VALUES ('Email_Payment_Instruction_BCC','')")
        End If

        If SQL_QUERY_TO_INTEGER(CONNECTION_CONFIG, "SELECT COUNT(Field_Value_1) FROM Config WHERE [Field_Name] = 'Delete_Temp_Send_Email'") = 0 Then
            SQL_QUERY(CONNECTION_CONFIG, "INSERT INTO Config(Field_Name, Field_Value_1) VALUES ('Delete_Temp_Send_Email','YES')")
        End If

        If SQL_QUERY_TO_INTEGER(CONNECTION_CONFIG, "SELECT COUNT(Field_Value_1) FROM Config WHERE [Field_Name] = 'Refresh_Database_after_delete'") = 0 Then
            SQL_QUERY(CONNECTION_CONFIG, "INSERT INTO Config(Field_Name, Field_Value_1) VALUES ('Refresh_Database_after_delete','YES')")
        End If

        If SQL_QUERY_TO_INTEGER(CONNECTION_CONFIG, "SELECT COUNT(Field_Value_1) FROM Config WHERE [Field_Name] = 'Template_Email_Payment_Intruction_Release_All'") = 0 Then
            SQL_QUERY(CONNECTION_CONFIG, "INSERT INTO Config(Field_Name, Field_Value_1) VALUES ('Template_Email_Payment_Intruction_Release_All','appPath\Template\Template_Send_Email_Release_All.docx')")
        End If

        If SQL_QUERY_TO_INTEGER(CONNECTION_CONFIG, "SELECT COUNT(Field_Value_1) FROM Config WHERE [Field_Name] = 'Template_Email_Payment_Intruction_Hold_All'") = 0 Then
            SQL_QUERY(CONNECTION_CONFIG, "INSERT INTO Config(Field_Name, Field_Value_1) VALUES ('Template_Email_Payment_Intruction_Hold_All','appPath\Template\Template_Send_Email_Hold_All.docx')")
        End If

        If SQL_QUERY_TO_INTEGER(CONNECTION_CONFIG, "SELECT COUNT(Field_Value_1) FROM Config WHERE [Field_Name] = 'Template_Email_Payment_Intruction_Cancel_All'") = 0 Then
            SQL_QUERY(CONNECTION_CONFIG, "INSERT INTO Config(Field_Name, Field_Value_1) VALUES ('Template_Email_Payment_Intruction_Cancel_All','appPath\Template\Template_Send_Email_Cancel_All.docx')")
        End If

        If SQL_QUERY_TO_INTEGER(CONNECTION_CONFIG, "SELECT COUNT(Field_Value_1) FROM Config WHERE [Field_Name] = 'Template_Email_Payment_Intruction_Release_Hold_Cancel'") = 0 Then
            SQL_QUERY(CONNECTION_CONFIG, "INSERT INTO Config(Field_Name, Field_Value_1) VALUES ('Template_Email_Payment_Intruction_Release_Hold_Cancel','appPath\Template\Template_Send_Email_Release_Hold_Cancel.docx')")
        End If

        SQL_QUERY(CONNECTION_SSO, "CREATE TABLE IF NOT EXISTS LOG_IMPORT_VSD_REPORT(STR_DATE VARCHAR, FILE_PATH VARCHAR, STATUS VARCHAR, REMARK VARCHAR, USER_IMPORTED VARCHAR, NOTE VARCHAR)")
        SQL_QUERY(CONNECTION_SSO, "CREATE TABLE IF NOT EXISTS FIXED_DAILY_VSD_REPORT(CASE_ID VARCHAR NOT NULL UNIQUE PRIMARY KEY, STR_DATE VARCHAR, REF_NO VARCHAR, FULLNAME VARCHAR, MBNAME VARCHAR, ACTYPE VARCHAR, DBCODE VARCHAR, ORDERQTTY VARCHAR, MATCHQTTY VARCHAR, MATCHAMT VARCHAR, FEEAMC VARCHAR, FEEDXX VARCHAR, FEEFUND VARCHAR, TOTALFEE VARCHAR, TAXAMT VARCHAR, AMT NUMERIC, CUSTODYCD VARCHAR, BANKACNAME VARCHAR, CITYBANK VARCHAR, BANKACC VARCHAR, NOTE VARCHAR, TRADINGID VARCHAR, NAV VARCHAR, TOTALNAV VARCHAR, TXDATE VARCHAR, SYMBOL VARCHAR, NAME VARCHAR, FEENAME VARCHAR, EXECNAME VARCHAR, FEEID VARCHAR, SRTYPE VARCHAR, FILE_PATH VARCHAR, CASH_ACCOUNT VARCHAR, AMOUNT_PAYMENT NUMERIC, TRADE_DATE DATE, VALUE_DATE DATE, DEADLINE_PER_FUND DATE, FULLNAME_UNSIGN VARCHAR, BANKACNAME_UNSIGN VARCHAR, CITYBANK_UNSIGN VARCHAR, USER_CREATED VARCHAR, CASE_STATUS VARCHAR, USER_MODIFIED VARCHAR, BATCH_PAYMENT_INSTRUCTION VARCHAR, USER_NOTE VARCHAR)")
        SQL_QUERY(CONNECTION_SSO, "CREATE TABLE IF NOT EXISTS PAYMENT_INSTRUCTION_REPORT(CASE_ID VARCHAR NOT NULL UNIQUE PRIMARY KEY, REF_NO VARCHAR, VSD_REF_NO VARCHAR, CUSTOMER_ID VARCHAR, CUSTOMER_NAME VARCHAR, BATCH VARCHAR, SUB_BATCH VARCHAR, PMT_REF VARCHAR, PMT_STATUS VARCHAR, PMT_TYPE VARCHAR, DEBIT_AC_NO VARCHAR, DEBIT_AMOUNT NUMERIC, PAYMENT_CCY VARCHAR, PAYEE_AMOUNT NUMERIC, PROCESS_DATE DATETIME, PAYEE_NAME VARCHAR, ADDRESS1 VARCHAR, ADDRESS2 VARCHAR, ADDRESS3 VARCHAR, BEN_ACC VARCHAR, BEN_BANK_CODE VARCHAR, PAYMENT_DETAILS_1 VARCHAR, PAYMENT_DETAILS_2 VARCHAR, LOCAL_CHRG VARCHAR, BANK_NAME VARCHAR, TT_BENEFICIARY_BANK_DETAILS VARCHAR, USER_CREATED VARCHAR, DATE_CREATED VARCHAR, BATCH_NAME VARCHAR, BATCH_STATUS VARCHAR, VSD_FULLNAME VARCHAR, VSD_BANKACNAME_CITYBANK VARCHAR, VSD_CASH_ACCOUNT VARCHAR, VSD_VALUE_DATE DATETIME, VSD_AMOUNT_PAYMENT NUMERIC, CHECK_NAME BOOLEAN, CHECK_BANK_NAME BOOLEAN, CHECK_CASH_ACC BOOLEAN, CHECK_VALUE_DATE BOOLEAN, CHECK_AMOUNT BOOLEAN, CHECK_BEN_CHARGE BOOLEAN, CHECK_BEN_CHARGE_FEE BOOLEAN, FINAL_CHECK BOOLEAN, CASE_STATUS VARCHAR, USER_MODIFIED VARCHAR, USER_NOTE VARCHAR)")
        SQL_QUERY(CONNECTION_SSO, "CREATE TABLE IF NOT EXISTS INTERNAL_TRANSFER(CASE_ID VARCHAR NOT NULL UNIQUE PRIMARY KEY, SYMBOL VARCHAR, CASH_ACCOUNT VARCHAR, VALUE_DATE DATE, TOTAL_AMOUNT NUMERIC, PAYMENT_CASE_ID VARCHAR, PAYMENT_AMOUNT NUMERIC, PAYMENT_STATUS VARCHAR, STATUS VARCHAR, USER_NOTE VARCHAR, USER_CREATED VARCHAR, USER_MODIFIED VARCHAR)")
    End Sub

#Region "VSD_REPORT"

    Private Sub ImportVSD_GridView1_CellValueChanged(sender As Object, e As CellValueChangedEventArgs) Handles ImportVSD_GridView1.CellValueChanged
        Dim view As GridView = sender
        If view Is Nothing Then
            Exit Sub
        End If
        If e.Column.GetTextCaption <> "USER_NOTE" Then
            If e.Column.GetTextCaption <> "BATCH_PAYMENT_INSTRUCTION" Then
                If e.Column.GetTextCaption <> "CASE_STATUS" Then
                    Exit Sub
                End If
            End If
        End If

        Dim iRet = MsgBox("Do you want to save this changes?", vbQuestion + vbYesNo, "SSO")
        If iRet = vbYes Then
            Dim USER_MODIFIED As String = UCase(Environment.UserName) & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss")
            ImportVSD_GridView1.SetFocusedRowCellValue("USER_MODIFIED", USER_MODIFIED)
            SQL_QUERY(CONNECTION_SSO, "UPDATE FIXED_DAILY_VSD_REPORT SET CASE_STATUS = '" & ImportVSD_GridView1.GetFocusedRowCellDisplayText("CASE_STATUS").ToString & "', USER_MODIFIED = '" & USER_MODIFIED & "', USER_NOTE = '" & ImportVSD_GridView1.GetFocusedRowCellDisplayText("USER_NOTE").ToString & "', BATCH_PAYMENT_INSTRUCTION = '" & ImportVSD_GridView1.GetFocusedRowCellDisplayText("BATCH_PAYMENT_INSTRUCTION").ToString & "' WHERE CASE_ID = '" & ImportVSD_GridView1.GetFocusedRowCellDisplayText("CASE_ID").ToString & "'")
        End If

    End Sub

    Private Sub GridView_Internal_Transfer_CellValueChanged(sender As Object, e As CellValueChangedEventArgs) Handles GridView_Internal_Transfer.CellValueChanged
        Dim view As GridView = sender
        If view Is Nothing Then
            Exit Sub
        End If
        If e.Column.GetTextCaption <> "USER_NOTE" Then
            If e.Column.GetTextCaption <> "PAYMENT_CASE_ID" Then
                If e.Column.GetTextCaption <> "STATUS" Then
                    If e.Column.GetTextCaption <> "PAYMENT_AMOUNT" Then
                        Exit Sub
                    End If
                End If
            End If
        End If

        Dim PAYMENT_CASE_ID As String = GridView_Internal_Transfer.GetFocusedRowCellValue("PAYMENT_CASE_ID")
        Dim PAYMENT_CASE_ID_DATABASE As String = SQL_QUERY_TO_STRING(CONNECTION_SSO, "SELECT DISTINCT PAYMENT_CASE_ID FROM INTERNAL_TRANSFER WHERE CASE_ID = '" & GridView_Internal_Transfer.GetFocusedRowCellDisplayText("CASE_ID").ToString & "'")

        If e.Column.GetTextCaption = "PAYMENT_CASE_ID" Then
            If PAYMENT_CASE_ID <> PAYMENT_CASE_ID_DATABASE Then
                If PAYMENT_CASE_ID.Length = 0 Then
                    GridView_Internal_Transfer.SetFocusedRowCellValue("PAYMENT_AMOUNT", "")
                Else
                    Dim COUNT As Integer = SQL_QUERY_TO_INTEGER(CONNECTION_SSO, "SELECT COUNT(CASE_ID) FROM PAYMENT_INSTRUCTION_REPORT WHERE CASE_ID = '" & PAYMENT_CASE_ID & "'")
                    If COUNT = 0 Then
                        GridView_Internal_Transfer.SetFocusedRowCellValue("PAYMENT_CASE_ID", PAYMENT_CASE_ID_DATABASE)
                        MsgBox("Not found payment instruction with Case_ID: " & PAYMENT_CASE_ID, vbCritical + vbOKOnly, "SSO")
                        Exit Sub
                    Else
                        GridView_Internal_Transfer.SetFocusedRowCellValue("PAYMENT_AMOUNT", SQL_QUERY_TO_LONG(CONNECTION_SSO, "SELECT DISTINCT DEBIT_AMOUNT FROM PAYMENT_INSTRUCTION_REPORT WHERE CASE_ID = '" & PAYMENT_CASE_ID & "'"))
                        GridView_Internal_Transfer.SetFocusedRowCellValue("PAYMENT_STATUS", SQL_QUERY_TO_LONG(CONNECTION_SSO, "SELECT DISTINCT CASE_STATUS FROM PAYMENT_INSTRUCTION_REPORT WHERE CASE_ID = '" & PAYMENT_CASE_ID & "'"))
                    End If
                End If
            Else
                Exit Sub
            End If
        End If

        Dim iRet = MsgBox("Do you want to save this changes?", vbQuestion + vbYesNo, "SSO")
        If iRet = vbYes Then

            GridView_Internal_Transfer.SetFocusedRowCellValue("USER_MODIFIED", UCase(Environment.UserName) & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss"))
            SQL_QUERY(CONNECTION_SSO, "UPDATE INTERNAL_TRANSFER SET PAYMENT_STATUS = '" & GridView_Internal_Transfer.GetFocusedRowCellValue("PAYMENT_STATUS") & "', PAYMENT_CASE_ID = '" & GridView_Internal_Transfer.GetFocusedRowCellValue("PAYMENT_CASE_ID") & "', PAYMENT_AMOUNT = '" & GridView_Internal_Transfer.GetFocusedRowCellValue("PAYMENT_AMOUNT") & "', STATUS = '" & GridView_Internal_Transfer.GetFocusedRowCellValue("STATUS") & "', USER_NOTE = '" & GridView_Internal_Transfer.GetFocusedRowCellValue("USER_NOTE") & "', USER_MODIFIED = '" & GridView_Internal_Transfer.GetFocusedRowCellValue("USER_MODIFIED") & "' WHERE CASE_ID = '" & GridView_Internal_Transfer.GetFocusedRowCellDisplayText("CASE_ID").ToString & "'")
        Else
            GridControl_Internal_Transfer.DataSource = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT * FROM INTERNAL_TRANSFER ORDER BY VALUE_DATE DESC")
            GridView_Internal_Transfer.BestFitColumns()
        End If
    End Sub

    Public Sub CHANGE_DESCRIPTION_WAIT_FROM(DESCRIPTION As String)
        SplashScreenManager.Default.SetWaitFormDescription(DESCRIPTION)
        WriteLog_Full(DESCRIPTION)
    End Sub

    Private Sub ImportVSD_btImport_Click(sender As Object, e As EventArgs) Handles ImportVSD_btImport.Click
        If ListView1.Items.Count = 0 Then
            MsgBox("No link source file", vbCritical + vbOKOnly, "SSO")
            Exit Sub
        End If

        For i = 0 To ListView1.Items.Count - 1
            If ListView1.Items(i).Checked = True Then
                If IsWorkbookAlreadyOpen(ListView1.Items(i).SubItems(6).Text) = True Then
                    MsgBox("The file is already open." & Chr(10) & Chr(10) & ListView1.Items(i).SubItems(6).Text, vbCritical + vbOKOnly)
                    Exit Sub
                End If
            End If
        Next

        Try
            SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)

            If (Not System.IO.Directory.Exists(link_folder_database & "TEMP")) Then
                System.IO.Directory.CreateDirectory(link_folder_database & "TEMP")
            End If

            Dim LINK_TEMP_DATABASE As String = link_folder_database & "TEMP\TEMP_" & Environment.UserName & "_" & Now.ToString("ddMMyyyyhhmmss") & ".txt"
            ' Create the SQLite database
            If Not My.Computer.FileSystem.FileExists(LINK_TEMP_DATABASE) Then
                SQLiteConnection.CreateFile(LINK_TEMP_DATABASE)
            End If

            SQL_QUERY_FROM_FILE(LINK_TEMP_DATABASE, "CREATE TABLE IF NOT EXISTS TEMP_DAILY_VSD_REPORT(Column0 VARCHAR, Column1 VARCHAR, Column2 VARCHAR, Column3 VARCHAR, Column4 VARCHAR, Column5 VARCHAR, Column6 VARCHAR, Column7 VARCHAR, Column8 VARCHAR, Column9 VARCHAR, Column10 VARCHAR, Column11 VARCHAR, Column12 VARCHAR, Column13 VARCHAR, Column14 VARCHAR, Column15 VARCHAR, Column16 VARCHAR, Column17 VARCHAR, Column18 VARCHAR, Column19 VARCHAR, Column20 VARCHAR, Column21 VARCHAR, Column22 VARCHAR, Column23 VARCHAR, Column24 VARCHAR, Column25 VARCHAR, Column26 VARCHAR, Column27 VARCHAR, Column28 VARCHAR)")
            SQL_QUERY_FROM_FILE(LINK_TEMP_DATABASE, "CREATE TABLE IF NOT EXISTS DAILY_VSD_REPORT(ID_NO INTEGER PRIMARY KEY AUTOINCREMENT, FILE_PATH VARCHAR, STR_DATE VARCHAR, FULLNAME VARCHAR, MBNAME VARCHAR, ACTYPE VARCHAR, DBCODE VARCHAR, ORDERQTTY VARCHAR, MATCHQTTY VARCHAR, MATCHAMT VARCHAR, FEEAMC VARCHAR, FEEDXX VARCHAR, FEEFUND VARCHAR, TOTALFEE VARCHAR, TAXAMT VARCHAR, AMT NUMERIC, CUSTODYCD VARCHAR, BANKACNAME VARCHAR, CITYBANK VARCHAR, BANKACC VARCHAR, NOTE VARCHAR, TRADINGID VARCHAR, NAV VARCHAR, TOTALNAV VARCHAR, TXDATE VARCHAR, SYMBOL VARCHAR, NAME VARCHAR, FEENAME VARCHAR, EXECNAME VARCHAR, FEEID VARCHAR, SRTYPE VARCHAR)")
            SQL_QUERY_FROM_FILE(LINK_TEMP_DATABASE, "CREATE TABLE IF NOT EXISTS FIXED_DAILY_VSD_REPORT(CASE_ID VARCHAR, STR_DATE VARCHAR, REF_NO VARCHAR, FULLNAME VARCHAR, MBNAME VARCHAR, ACTYPE VARCHAR, DBCODE VARCHAR, ORDERQTTY VARCHAR, MATCHQTTY VARCHAR, MATCHAMT VARCHAR, FEEAMC VARCHAR, FEEDXX VARCHAR, FEEFUND VARCHAR, TOTALFEE VARCHAR, TAXAMT VARCHAR, AMT NUMERIC, CUSTODYCD VARCHAR, BANKACNAME VARCHAR, CITYBANK VARCHAR, BANKACC VARCHAR, NOTE VARCHAR, TRADINGID VARCHAR, NAV VARCHAR, TOTALNAV VARCHAR, TXDATE VARCHAR, SYMBOL VARCHAR, NAME VARCHAR, FEENAME VARCHAR, EXECNAME VARCHAR, FEEID VARCHAR, SRTYPE VARCHAR, FILE_PATH VARCHAR, CASH_ACCOUNT VARCHAR, AMOUNT_PAYMENT NUMERIC, TRADE_DATE DATE, VALUE_DATE DATE, DEADLINE_PER_FUND DATE, FULLNAME_UNSIGN VARCHAR, BANKACNAME_UNSIGN VARCHAR, CITYBANK_UNSIGN VARCHAR, USER_CREATED VARCHAR, CASE_STATUS VARCHAR, USER_MODIFIED VARCHAR, BATCH_PAYMENT_INSTRUCTION VARCHAR)")

            For i = 0 To ListView1.Items.Count - 1
                If ListView1.Items(i).Checked = True Then
                    If ListView1.Items(i).SubItems(4).Text <> "Pending" Then
                        ListView1.Items(i).Checked = False
                        Dim LINK_VSD As String = ListView1.Items(i).SubItems(6).Text
                        If SQL_QUERY_TO_INTEGER(CONNECTION_SSO, "SELECT COUNT(FILE_PATH) FROM FIXED_DAILY_VSD_REPORT WHERE FILE_PATH = '" & LINK_VSD & "'") = 0 Then
                            Import_VSD_REPORT_IN_CSV_TO_DATABASE(LINK_TEMP_DATABASE, LINK_VSD)
                        Else
                            SplashScreenManager.CloseForm(False)
                            Dim iRet = MsgBox("This source file already imported. Do you want to overwrite?" & Chr(10) & Chr(10) & LINK_VSD, vbQuestion + vbYesNo, "SSO")
                            If iRet = vbYes Then
                                SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
                                SQL_QUERY(CONNECTION_SSO, "DELETE FROM FIXED_DAILY_VSD_REPORT WHERE FILE_PATH = '" & LINK_VSD & "'")
                                Import_VSD_REPORT_IN_CSV_TO_DATABASE(LINK_TEMP_DATABASE, LINK_VSD)
                            End If
                        End If
                    End If
                End If
            Next

            Dim USER_CREATED As String = UCase(Environment.UserName) & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss")
            Dim DT_FIXED_VSD As DataTable = SQL_FROMFILE_TO_DATATABLE(LINK_TEMP_DATABASE, "SELECT '" & Environment.UserName & "'||'" & Now.ToString("yyyyMMddhhmmsstt") & "'||ID_NO AS CASE_ID, FILE_PATH, STR_DATE, BANKACC||AMT AS REF_NO, FULLNAME, MBNAME, ACTYPE, DBCODE, ORDERQTTY, MATCHQTTY, MATCHAMT, FEEAMC, FEEDXX, FEEFUND, TOTALFEE, TAXAMT, AMT, CUSTODYCD, BANKACNAME, CITYBANK, BANKACC, NOTE, TRADINGID, NAV, TOTALNAV, TXDATE, SYMBOL, NAME, FEENAME, EXECNAME, FEEID, SRTYPE, AMT AS AMOUNT_PAYMENT, '" & USER_CREATED & "' AS USER_CREATED, 'Incomplete' AS CASE_STATUS FROM DAILY_VSD_REPORT")
            SQLITE_BULK_COPY(DT_FIXED_VSD, LINK_TEMP_DATABASE, "FIXED_DAILY_VSD_REPORT")

            Dim DT_FILEPATH As DataTable = SQL_FROMFILE_TO_DATATABLE(LINK_TEMP_DATABASE, "SELECT DISTINCT FILE_PATH FROM FIXED_DAILY_VSD_REPORT")
            For Each DRR As DataRow In DT_FILEPATH.Rows
                Dim TimerStart As DateTime = Now
                Dim RESULT As String = IMPORT_VSD_REPORT(LINK_TEMP_DATABASE, DRR("FILE_PATH").ToString)
                Dim TimeSpent As System.TimeSpan = Now.Subtract(TimerStart)

                If RESULT = "DONE" Then
                    SQL_QUERY(CONNECTION_SSO, "UPDATE LOG_IMPORT_VSD_REPORT SET STATUS = 'Successful', USER_IMPORTED = '" & UCase(Environment.UserName) & "_" & Now.ToString("yyyy/MM/dd hh:mm:ss") & "', NOTE = '" & "in " & Format(TimeSpent.TotalSeconds, "0.00") & " seconds" & "' WHERE FILE_PATH = '" & DRR("FILE_PATH").ToString & "' AND USER_IMPORTED IS NULL")
                Else
                    SQL_QUERY(CONNECTION_SSO, "UPDATE LOG_IMPORT_VSD_REPORT SET REMARK = '" & RESULT & "', STATUS = 'Fail', USER_IMPORTED = '" & UCase(Environment.UserName) & "_" & Now.ToString("yyyy/MM/dd hh:mm:ss") & "', NOTE = '" & "in " & Format(TimeSpent.TotalSeconds, "0.00") & " seconds" & "' WHERE FILE_PATH = '" & DRR("FILE_PATH").ToString & "' AND USER_IMPORTED IS NULL")
                End If
            Next

            SplashScreenManager.CloseForm(False)

            MsgBox("Completed!", vbInformation)
        Catch ex As Exception
            WriteErrorLog(String.Format("Error:  {0}", ex.Message))
            SplashScreenManager.CloseForm(False)
            MsgBox(String.Format("Error: {0}", ex.Message), vbCritical + vbOKOnly, "SSO")
        End Try

    End Sub

    Public Sub Import_VSD_REPORT_IN_CSV_TO_DATABASE(LINK_TEMP_DATABASE As String, LINK_FILE_VSD As String)
        Try
            If LINK_FILE_VSD.Substring(LINK_FILE_VSD.Length - 3, 3) = "csv" Then
                SQLITE_BULK_COPY(CSV_TO_DATATABLE_USING_TextFieldParser(LINK_FILE_VSD), LINK_TEMP_DATABASE, "TEMP_DAILY_VSD_REPORT")
                Dim DT As DataTable = SQL_FROMFILE_TO_DATATABLE(LINK_TEMP_DATABASE, "SELECT '" & Format(ImportVSD_tbDate.EditValue, "dd/MM/yyyy") & "' AS STR_DATE, '" & LINK_FILE_VSD & "' AS FILE_PATH, Column0 AS FULLNAME, Column1 AS MBNAME, Column2 AS ACTYPE, Column3 AS DBCODE, Column4 AS ORDERQTTY, Column5 AS MATCHQTTY, Column6 AS MATCHAMT, Column7 AS FEEAMC, Column8 AS FEEDXX, Column9 AS FEEFUND, Column10 AS TOTALFEE, Column11 AS TAXAMT, Column12 AS AMT, Column13 AS CUSTODYCD, Column14 AS BANKACNAME, Column15 AS CITYBANK, Column16 AS BANKACC, Column17 AS NOTE, Column18 AS TRADINGID, Column19 AS NAV, Column20 AS TOTALNAV, Column21 AS TXDATE, Column22 AS SYMBOL, Column23 AS NAME, Column24 AS FEENAME, Column25 AS EXECNAME, Column26 AS FEEID, Column27 AS SRTYPE FROM TEMP_DAILY_VSD_REPORT WHERE Column0 <> 'FULLNAME'")
                If DT.Rows.Count > 0 Then
                    SQLITE_BULK_COPY(DT, LINK_TEMP_DATABASE, "DAILY_VSD_REPORT")
                    SQL_QUERY(CONNECTION_SSO, "INSERT INTO LOG_IMPORT_VSD_REPORT(STR_DATE, FILE_PATH, STATUS) VALUES ('" & Format(ImportVSD_tbDate.EditValue, "dd/MM/yyyy") & "', '" & LINK_FILE_VSD & "','Pending')")
                    SQL_QUERY_FROM_FILE(LINK_TEMP_DATABASE, "DELETE FROM TEMP_DAILY_VSD_REPORT")
                Else
                    SQL_QUERY(CONNECTION_SSO, "INSERT INTO LOG_IMPORT_VSD_REPORT(STR_DATE, FILE_PATH, STATUS, REMARK, USER_IMPORTED) VALUES ('" & Format(ImportVSD_tbDate.EditValue, "dd/MM/yyyy") & "', '" & LINK_FILE_VSD & "','Successful', 'No data', '" & UCase(Environment.UserName) & "_" & Now.ToString("yyyy/MM/dd hh:mm:ss") & "')")
                End If
            Else
                SQL_QUERY(CONNECTION_SSO, "INSERT INTO LOG_IMPORT_VSD_REPORT(STR_DATE, FILE_PATH, STATUS, REMARK, USER_IMPORTED) VALUES ('" & Format(ImportVSD_tbDate.EditValue, "dd/MM/yyyy") & "', '" & LINK_FILE_VSD & "','Fail','It's not .csv file', '" & UCase(Environment.UserName) & "_" & Now.ToString("yyyy/MM/dd hh:mm:ss") & "')")
            End If
        Catch ex As Exception
            WriteErrorLog(String.Format("Error:  {0}", ex.Message))
            SQL_QUERY(CONNECTION_SSO, "INSERT INTO LOG_IMPORT_VSD_REPORT(STR_DATE, FILE_PATH, STATUS, REMARK, USER_IMPORTED) VALUES ('" & Format(ImportVSD_tbDate.EditValue, "dd/MM/yyyy") & "', '" & LINK_FILE_VSD & "','Fail','" & ex.Message & "', '" & UCase(Environment.UserName) & "_" & Now.ToString("yyyy/MM/dd hh:mm:ss") & "')")
        End Try
    End Sub

    Public Function IMPORT_VSD_REPORT(LINK_TEMP_DATABASE As String, FILE_PATH As String) As String
        Try
            CHANGE_DESCRIPTION_WAIT_FROM("[IMPORT VSD REPORT] - " & FILE_PATH)
            Dim DT As DataTable = SQL_FROMFILE_TO_DATATABLE(LINK_TEMP_DATABASE, "SELECT * FROM FIXED_DAILY_VSD_REPORT WHERE FILE_PATH = '" & FILE_PATH & "'")
            For Each Drr As DataRow In DT.Rows
                Dim CASE_ID As String = Drr("CASE_ID").ToString
                Dim TRADE_DATE As DateTime = CONVERT_STRING_TO_DATE(Drr("TXDATE").ToString, "dd/MM/yyyy", "dd/MM/yyyy")
                Dim VALUE_DATE As DateTime = Format(DateAddWeekDaysOnly(CONVERT_STRING_TO_DATE(Drr("TXDATE").ToString, "dd/MM/yyyy", "dd/MM/yyyy"), SQL_QUERY_TO_INTEGER(CONNECTION_CONFIG, "SELECT DISTINCT VALUE_DATE_ADD_DAYS FROM LIST_CASH_ACCOUNT WHERE SYMBOL = '" & Drr("SYMBOL").ToString & "'")), "dd/MM/yyyy")
                Dim DEADLINE_DATE As DateTime = Format(DateAddWeekDaysOnly(CONVERT_STRING_TO_DATE(Drr("TXDATE").ToString, "dd/MM/yyyy", "dd/MM/yyyy"), SQL_QUERY_TO_INTEGER(CONNECTION_CONFIG, "SELECT DISTINCT DEADLINE_ADD_DAYS FROM LIST_CASH_ACCOUNT WHERE SYMBOL = '" & Drr("SYMBOL").ToString & "'")), "dd/MM/yyyy")
                Dim FULL_NAME_UNSIGN As String = ConvertToUnSign(Drr("FULLNAME").ToString)
                SQL_QUERY_FROM_FILE(LINK_TEMP_DATABASE, "UPDATE FIXED_DAILY_VSD_REPORT SET DEADLINE_PER_FUND = '" & DEADLINE_DATE & "', VALUE_DATE = '" & VALUE_DATE & "', TRADE_DATE = '" & TRADE_DATE & "', FULLNAME_UNSIGN = '" & FULL_NAME_UNSIGN & "', BANKACNAME_UNSIGN = '" & ConvertToUnSign(Drr("BANKACNAME").ToString) & "', CITYBANK_UNSIGN = '" & ConvertToUnSign(Drr("CITYBANK").ToString) & "', CASH_ACCOUNT = '" & SQL_QUERY_TO_STRING(CONNECTION_CONFIG, "SELECT DISTINCT CASH_ACCOUNT FROM LIST_CASH_ACCOUNT WHERE SYMBOL = '" & Drr("SYMBOL").ToString & "'") & "' WHERE CASE_ID = '" & CASE_ID & "'")
            Next

            SQLITE_BULK_COPY(SQL_FROMFILE_TO_DATATABLE(LINK_TEMP_DATABASE, "SELECT * FROM FIXED_DAILY_VSD_REPORT WHERE FILE_PATH = '" & FILE_PATH & "'"), link_database_SSO, "FIXED_DAILY_VSD_REPORT")

            DT = SQL_FROMFILE_TO_DATATABLE(LINK_TEMP_DATABASE, "SELECT DISTINCT SYMBOL, VALUE_DATE FROM FIXED_DAILY_VSD_REPORT WHERE FILE_PATH = '" & FILE_PATH & "'")
            Dim i As Integer = 0
            For Each DRR As DataRow In DT.Rows
                i = i + 1
                Dim CASE_ID As String = Environment.UserName & Now.ToString("ddMMyyyyhhmmsstt") & i
                Dim SYMBOL As String = DRR("SYMBOL").ToString
                Dim VALUE_DATE As DateTime = Format(DRR("VALUE_DATE"), "dd/MM/yyyy")
                Dim COUNT As Integer = SQL_QUERY_TO_INTEGER(CONNECTION_SSO, "SELECT COUNT(CASE_ID) FROM INTERNAL_TRANSFER WHERE SYMBOL = '" & SYMBOL & "' AND VALUE_DATE LIKE '%" & VALUE_DATE & "%'")
                If COUNT = 0 Then
                    SQLITE_BULK_COPY(SQL_FROMFILE_TO_DATATABLE(LINK_TEMP_DATABASE, "SELECT CASH_ACCOUNT, 'Incomplete' AS STATUS, '" & UCase(Environment.UserName) & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss") & "' AS USER_CREATED, '" & CASE_ID & "' AS CASE_ID, SYMBOL, VALUE_DATE, SUM(AMOUNT_PAYMENT) AS TOTAL_AMOUNT FROM FIXED_DAILY_VSD_REPORT WHERE SYMBOL = '" & SYMBOL & "' AND VALUE_DATE = '" & VALUE_DATE & "' GROUP BY SYMBOL"), link_database_SSO, "INTERNAL_TRANSFER")
                End If
            Next

            Dim DT_CHANGE_CASH_ACC As DataTable = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT DISTINCT SYMBOL, CASH_ACCOUNT FROM INTERNAL_TRANSFER")
            For Each DRR As DataRow In DT_CHANGE_CASH_ACC.Rows
                Dim SYMBOL As String = DRR("SYMBOL").ToString
                Dim CASH_ACCOUNT As String = GET_ONLY_NUMERIC_FROM_STRING(SQL_QUERY_TO_STRING(CONNECTION_CONFIG, "SELECT DISTINCT CASH_ACCOUNT FROM LIST_CASH_ACCOUNT WHERE SYMBOL = '" & SYMBOL & "'"))
                SQL_QUERY(CONNECTION_SSO, "UPDATE INTERNAL_TRANSFER SET CASH_ACCOUNT = '" & CASH_ACCOUNT & "' WHERE SYMBOL = '" & SYMBOL & "'")
            Next

            Return "DONE"
        Catch ex As Exception
            WriteErrorLog(String.Format("Error: {0}", Replace(ex.Message, "'", "")))
            Return ex.Message
        End Try
    End Function

    Private Sub ImportVSD_tbLinkFile_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles ImportVSD_tbLinkFile.ButtonClick
        Try

            Dim dialog As New FolderBrowserDialog()
            dialog.RootFolder = Environment.SpecialFolder.Desktop
            dialog.SelectedPath = "C:\"
            dialog.Description = "Select Folder VSD Report"
            If dialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
                ImportVSD_tbLinkFile.Text = dialog.SelectedPath
            End If

        Catch ex As Exception
            MsgBox(ex.Message, vbCritical + vbOKOnly)
            WriteErrorLog(ex.ToString)
        End Try
    End Sub

    Private Sub ImportVSD_btExportToExcel_Click(sender As Object, e As EventArgs) Handles ImportVSD_btExportToExcel.Click
        Try
            Dim page_index As Integer = TabPane2.SelectedPageIndex
            If TabPane2.SelectedPageIndex = 0 Then
                If ImportVSD_GridView1.RowCount = 0 Then
                    MsgBox("No data", vbCritical + vbOKOnly, "SSO")
                    Exit Sub
                End If
                EXPORT_GRIDVIEW_TO_EXCEL(ImportVSD_GridView1)
            Else
                If TabPane2.SelectedPageIndex = 1 Then
                    If GridView_Internal_Transfer.RowCount = 0 Then
                        MsgBox("No data", vbCritical + vbOKOnly, "SSO")
                        Exit Sub
                    End If
                    EXPORT_GRIDVIEW_TO_EXCEL(GridView_Internal_Transfer)
                Else
                    If TabPane2.SelectedPageIndex = 2 Then
                        If GridView_MaintenanceVSD.RowCount = 0 Then
                            MsgBox("No data", vbCritical + vbOKOnly, "SSO")
                            Exit Sub
                        End If
                        EXPORT_GRIDVIEW_TO_EXCEL(GridView_MaintenanceVSD)
                    Else
                        If TabPane2.SelectedPageIndex = 3 Then
                            If ImportVSD_GridView_ImportLog.RowCount = 0 Then
                                MsgBox("No data", vbCritical + vbOKOnly, "SSO")
                                Exit Sub
                            End If
                            EXPORT_GRIDVIEW_TO_EXCEL(ImportVSD_GridView_ImportLog)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, vbCritical + vbOKOnly)
            WriteErrorLog(ex.ToString)
        End Try
    End Sub

    Private Sub ImportVSD_tbLinkFile_EditValueChanged(sender As Object, e As EventArgs) Handles ImportVSD_tbLinkFile.EditValueChanged
        Load_Source_File_VSD_To_ListView(ImportVSD_tbLinkFile.Text)
    End Sub

    Public Sub Load_Source_File_VSD_To_ListView(LINK_FOLDER As String)
        Try
            If LINK_FOLDER.Substring(LINK_FOLDER.Length - 1, 1) <> "\" Then
                LINK_FOLDER = LINK_FOLDER & "\"
            End If

            If Directory.Exists(LINK_FOLDER) Then
                ListView1.Items.Clear()

                Dim Code_Setup_Search_VSD_Report_Format As String = SQL_QUERY_TO_STRING(CONNECTION_CONFIG, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Code_Setup_Search_VSD_Report_Format'")
                Dim Code_Setup_Search_VSD_Report_Option_AllDirectories As String = SQL_QUERY_TO_STRING(CONNECTION_CONFIG, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Code_Setup_Search_VSD_Report_Option_AllDirectories'")


                Dim files As String()
                Select Case UCase(Code_Setup_Search_VSD_Report_Option_AllDirectories)
                    Case "YES"
                        files = IO.Directory.GetFiles(LINK_FOLDER, Code_Setup_Search_VSD_Report_Format, SearchOption.AllDirectories)
                    Case Else
                        files = IO.Directory.GetFiles(LINK_FOLDER, Code_Setup_Search_VSD_Report_Format, SearchOption.TopDirectoryOnly)
                End Select

                Dim i As Integer = 0
                For Each file As String In files
                    i = i + 1
                    Dim USER_IMPORTED As String = SQL_QUERY_TO_STRING(CONNECTION_SSO, "SELECT USER_IMPORTED FROM LOG_IMPORT_VSD_REPORT WHERE FILE_PATH = '" & file & "' ORDER BY USER_IMPORTED DESC")
                    Dim STATUS As String = SQL_QUERY_TO_STRING(CONNECTION_SSO, "SELECT STATUS FROM LOG_IMPORT_VSD_REPORT WHERE FILE_PATH = '" & file & "' ORDER BY USER_IMPORTED DESC")

                    ListView1.Items.Add("")
                    ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(i)
                    ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Path.GetFileName(file))
                    ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Format(IO.File.GetLastWriteTime(file), "dd/MM/yyyy hh:mm:ss tt"))
                    ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(STATUS)
                    ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(USER_IMPORTED)
                    ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(file)

                    If i > 100 Then
                        Exit For
                    End If
                Next
                LISTVIEW_AUTO_RESIZE_COLUMNS_WIDTH(ListView1)

                For Each item As ListViewItem In Me.ListView1.Items
                    If item.SubItems(4).Text <> "Successful" Then
                        item.Checked = True
                    End If
                Next

            End If
        Catch ex As Exception
            WriteErrorLog("Error in WriteErrorLog: " + ex.ToString)
        End Try
    End Sub

    Private Sub ImportVSD_tbDate_EditValueChanged(sender As Object, e As EventArgs) Handles ImportVSD_tbDate.EditValueChanged
        Try
            If ImportVSD_tbDate.EditValue Is Nothing Then
                Exit Sub
            End If

            ImportVSD_tbLinkFile.Text = ""

            Dim TxnDate As DateTime = ImportVSD_tbDate.EditValue
            Dim strDate_yy As String = Format(TxnDate, "yy")
            Dim strDate_yyyy As String = Format(TxnDate, "yyyy")
            Dim strDate_MM As String = Format(TxnDate, "MM")
            Dim strDate_M As Integer = CInt(strDate_MM)
            Dim strDate_dd As String = Format(TxnDate, "dd")
            Dim strDate_d As String = CInt(strDate_dd)
            Dim strDate_MMM As String = Format(TxnDate, "MMM")

            Dim file_source As String = SQL_FROMFILE_TO_STRING(global_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Format_Source_VSD_Report'")
            file_source = Replace(file_source, "strDate_yyyy", strDate_yyyy)
            file_source = Replace(file_source, "strDate_yy", strDate_yy)
            file_source = Replace(file_source, "strDate_MMM", strDate_MMM)
            file_source = Replace(file_source, "strDate_MM", strDate_MM)
            file_source = Replace(file_source, "strDate_M", strDate_M)
            file_source = Replace(file_source, "strDate_dd", strDate_dd)
            file_source = Replace(file_source, "strDate_d", strDate_d)

            If Directory.Exists(file_source) Then
                ImportVSD_tbLinkFile.Text = file_source
            Else
                ImportVSD_tbLinkFile.Text = ""
                MsgBox("Not found path: " & Chr(10) & Chr(10) & file_source, vbCritical + vbOKOnly, "SSO")
            End If
        Catch ex As Exception
            WriteErrorLog("Error in WriteErrorLog: " + ex.ToString)
            MsgBox(ex.Message, vbCritical + vbOKOnly, "SSO")
        End Try
    End Sub

    Private Sub TabPane2_SelectedPageIndexChanged(sender As Object, e As EventArgs) Handles TabPane2.SelectedPageIndexChanged
        Try
            If TabPane2.SelectedPage Is TabNavigationPage_ImportLog Then
                ImportVSD_GridControl_ImportLog.DataSource = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT * FROM LOG_IMPORT_VSD_REPORT ORDER BY CAST(SUBSTR(STR_DATE, 7, 4) AS INT) DESC, CAST(SUBSTR(STR_DATE, 4, 2) AS INT) DESC, CAST(SUBSTR(STR_DATE, 1, 2) AS INT) DESC")
                ImportVSD_GridView_ImportLog.BestFitColumns()
            Else
                If TabPane2.SelectedPage Is TabNavigationPage_Internal_Transfer Then
                    Dim SQL_STRING As String = "SELECT DISTINCT CASE_ID, SYMBOL, CASH_ACCOUNT, VALUE_DATE, TOTAL_AMOUNT, PAYMENT_CASE_ID, PAYMENT_AMOUNT, PAYMENT_STATUS, STATUS, USER_NOTE, USER_MODIFIED," &
                                                " CASE WHEN TOTAL_AMOUNT - CURRENT_TOTAL_AMOUNT = 0 THEN '' ELSE CASE WHEN CURRENT_TOTAL_AMOUNT IS NULL THEN 0 ELSE CURRENT_TOTAL_AMOUNT END END AS CURRENT_TOTAL_AMOUNT" &
                                                " FROM (" &
                                                " SELECT A.*, B.CURRENT_TOTAL_AMOUNT FROM (SELECT * FROM INTERNAL_TRANSFER) A" &
                                                " LEFT JOIN (SELECT DISTINCT SYMBOL, VALUE_DATE, SUM(AMOUNT_PAYMENT) AS CURRENT_TOTAL_AMOUNT FROM FIXED_DAILY_VSD_REPORT GROUP BY SYMBOL, VALUE_DATE) B ON A.SYMBOL = B.SYMBOL AND A.VALUE_DATE = B.VALUE_DATE" &
                                                ") ORDER BY CAST(SUBSTR(VALUE_DATE, 7, 4) AS INT) DESC, CAST(SUBSTR(VALUE_DATE, 4, 2) AS INT) DESC, CAST(SUBSTR(VALUE_DATE, 1, 2) AS INT) DESC"


                    GridControl_Internal_Transfer.DataSource = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, SQL_STRING)
                    GridView_Internal_Transfer.BestFitColumns()

                    'For i As Integer = 0 To GridView_Internal_Transfer.RowCount - 1
                    '    Dim SYMBOL As String = GridView_Internal_Transfer.GetRowCellValue(i, "SYMBOL")
                    '    Dim VALUE_DATE As String = GridView_Internal_Transfer.GetRowCellValue(i, "VALUE_DATE")
                    '    Dim CaseID As String = GridView_Internal_Transfer.GetRowCellValue(i, "CASE_ID")

                    '    Dim CURRENT_TOTAL_AMOUNT As Long = SQL_QUERY_TO_LONG(CONNECTION_SSO, "SELECT SUM(AMOUNT_PAYMENT) FROM FIXED_DAILY_VSD_REPORT WHERE SYMBOL = '" & SYMBOL & "' AND VALUE_DATE LIKE '%" & VALUE_DATE & "%' GROUP BY SYMBOL")

                    '    'SQL_QUERY(CONNECTION_SSO, "UPDATE INTERNAL_TRANSFER SET TOTAL_AMOUNT = " & CURRENT_TOTAL_AMOUNT & " WHERE CASE_ID = '" & CaseID & "'")
                    '    If GridView_Internal_Transfer.GetRowCellValue(i, "TOTAL_AMOUNT") <> CURRENT_TOTAL_AMOUNT Then
                    '        GridView_Internal_Transfer.SetRowCellValue(i, GridView_Internal_Transfer.Columns("CURRENT_TOTAL_AMOUNT"), CURRENT_TOTAL_AMOUNT)
                    '    End If
                    'Next

                    'GridControl_Internal_Transfer.DataSource = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT * FROM INTERNAL_TRANSFER ORDER BY VALUE_DATE DESC")
                Else
                    If TabPane2.SelectedPage Is TabNavigationPage_Maintenance_VSD Then
                        GridControl_MaintenanceVSD.DataSource = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT DISTINCT STR_DATE, FILE_PATH, USER_CREATED FROM FIXED_DAILY_VSD_REPORT ORDER BY CAST(SUBSTR(STR_DATE, 7, 4) AS INT) DESC, CAST(SUBSTR(STR_DATE, 4, 2) AS INT) DESC, CAST(SUBSTR(STR_DATE, 1, 2) AS INT) DESC")
                        GridView_MaintenanceVSD.BestFitColumns()
                    End If
                End If
            End If
        Catch ex As Exception
            WriteErrorLog("Error in WriteErrorLog: " + ex.ToString)
            MsgBox(ex.Message, vbCritical + vbOKOnly, "SSO")
        End Try
    End Sub

    Private Sub InternalTransfer_btChange_CURRENT_TOTAL_AMOUNT_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles InternalTransfer_btChange_CURRENT_TOTAL_AMOUNT.ButtonClick
        Try
            Dim CURRENT_TOTAL_AMOUNT As Long = CLng(GridView_Internal_Transfer.GetFocusedRowCellValue("CURRENT_TOTAL_AMOUNT"))
            If CURRENT_TOTAL_AMOUNT = 0 Or CURRENT_TOTAL_AMOUNT > 0 Then
                Dim iRet = MsgBox("Do you want update new TOTAL_AMOUNT to database?", vbQuestion + vbYesNo, "SSO")
                If iRet = vbYes Then
                    SQL_QUERY(CONNECTION_SSO, "UPDATE INTERNAL_TRANSFER SET USER_MODIFIED = '" & UCase(Environment.UserName) & "_" & Now.ToString("yyyy/MM/dd hh:mm:ss") & "', TOTAL_AMOUNT = " & CURRENT_TOTAL_AMOUNT & " WHERE CASE_ID = '" & GridView_Internal_Transfer.GetFocusedRowCellValue("CASE_ID") & "'")

                    Dim SQL_STRING As String = "SELECT DISTINCT CASE_ID, SYMBOL, CASH_ACCOUNT, VALUE_DATE, TOTAL_AMOUNT, PAYMENT_CASE_ID, PAYMENT_AMOUNT, PAYMENT_STATUS, STATUS, USER_NOTE, USER_MODIFIED," &
                                                " CASE WHEN TOTAL_AMOUNT - CURRENT_TOTAL_AMOUNT = 0 THEN '' ELSE CASE WHEN CURRENT_TOTAL_AMOUNT IS NULL THEN 0 ELSE CURRENT_TOTAL_AMOUNT END END AS CURRENT_TOTAL_AMOUNT" &
                                                " FROM (" &
                                                " SELECT A.*, B.CURRENT_TOTAL_AMOUNT FROM (SELECT * FROM INTERNAL_TRANSFER) A" &
                                                " LEFT JOIN (SELECT DISTINCT SYMBOL, VALUE_DATE, SUM(AMOUNT_PAYMENT) AS CURRENT_TOTAL_AMOUNT FROM FIXED_DAILY_VSD_REPORT GROUP BY SYMBOL, VALUE_DATE) B ON A.SYMBOL = B.SYMBOL AND A.VALUE_DATE = B.VALUE_DATE" &
                                                ") ORDER BY CAST(SUBSTR(VALUE_DATE, 7, 4) AS INT) DESC, CAST(SUBSTR(VALUE_DATE, 4, 2) AS INT) DESC, CAST(SUBSTR(VALUE_DATE, 1, 2) AS INT) DESC"


                    GridControl_Internal_Transfer.DataSource = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, SQL_STRING)

                    'GridControl_Internal_Transfer.DataSource = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT * FROM INTERNAL_TRANSFER ORDER BY VALUE_DATE DESC")
                    'GridView_Internal_Transfer.BestFitColumns()

                    'For i As Integer = 0 To GridView_Internal_Transfer.RowCount - 1
                    '    Dim SYMBOL As String = GridView_Internal_Transfer.GetRowCellValue(i, "SYMBOL")
                    '    Dim VALUE_DATE As String = GridView_Internal_Transfer.GetRowCellValue(i, "VALUE_DATE")
                    '    CURRENT_TOTAL_AMOUNT = SQL_QUERY_TO_LONG(CONNECTION_SSO, "SELECT SUM(AMOUNT_PAYMENT) FROM FIXED_DAILY_VSD_REPORT WHERE SYMBOL = '" & SYMBOL & "' AND VALUE_DATE LIKE '%" & VALUE_DATE & "%' GROUP BY SYMBOL")
                    '    If GridView_Internal_Transfer.GetRowCellValue(i, "TOTAL_AMOUNT") <> CURRENT_TOTAL_AMOUNT Then
                    '        GridView_Internal_Transfer.SetRowCellValue(i, "CURRENT_TOTAL_AMOUNT", CURRENT_TOTAL_AMOUNT)
                    '    End If
                    'Next
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Maintenance_VSD_btDelete_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles Maintenance_VSD_btDelete.ButtonClick
        Dim STR_DATE As String = GridView_MaintenanceVSD.GetFocusedRowCellDisplayText("STR_DATE").ToString
        Dim FILE_PATH As String = GridView_MaintenanceVSD.GetFocusedRowCellDisplayText("FILE_PATH").ToString

        Try
            Dim iRet = MsgBox("Do you want to delete VSD report: '" & FILE_PATH & "'?", vbYesNo + vbQuestion, "SSO")
            If iRet = vbYes Then
                SQL_QUERY(CONNECTION_SSO, "DELETE FROM FIXED_DAILY_VSD_REPORT WHERE FILE_PATH = '" & FILE_PATH & "'")
                GridControl_MaintenanceVSD.DataSource = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT DISTINCT STR_DATE, FILE_PATH, USER_CREATED FROM FIXED_DAILY_VSD_REPORT ORDER BY CAST(SUBSTR(STR_DATE, 7, 4) AS INT) DESC, CAST(SUBSTR(STR_DATE, 4, 2) AS INT) DESC, CAST(SUBSTR(STR_DATE, 1, 2) AS INT) DESC")
                SQL_QUERY(CONNECTION_SSO, "INSERT INTO LOG_IMPORT_VSD_REPORT(STR_DATE, FILE_PATH, STATUS, USER_IMPORTED) VALUES ('" & STR_DATE & "', '" & FILE_PATH & "','DELETED','" & UCase(Environment.UserName) & "_" & Now.ToString("yyyy/MM/dd hh:mm:ss") & "')")
            End If
        Catch ex As Exception
            WriteErrorLog("[DELETE VSD DATA] - " & STR_DATE & vbLf & " - File Path: " & FILE_PATH & vbLf & " - Error Message: " & ex.ToString)
            MsgBox(ex.Message, vbCritical)
        End Try
    End Sub

    Private Sub DropDownButton_RefreshAll_Click(sender As Object, e As EventArgs) Handles DropDownButton_RefreshAll.Click
        Try
            ImportVSD_GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT * FROM FIXED_DAILY_VSD_REPORT ORDER BY CAST(SUBSTR(STR_DATE, 7, 4) AS INT) DESC, CAST(SUBSTR(STR_DATE, 4, 2) AS INT) DESC, CAST(SUBSTR(STR_DATE, 1, 2) AS INT) DESC")
            ImportVSD_GridView1.BestFitColumns()
            ImportVSD_GridView1.ExpandAllGroups()
        Catch ex As Exception
            MsgBox(ex.Message, vbCritical + vbOKOnly)
            WriteErrorLog(ex.ToString)
        End Try
    End Sub

    Private Sub BarButtonItem_GetAllCaseCompleted_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem_GetAllCaseCompleted.ItemClick
        Try
            ImportVSD_GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT * FROM FIXED_DAILY_VSD_REPORT WHERE CASE_STATUS = 'Completed' ORDER BY CAST(SUBSTR(STR_DATE, 7, 4) AS INT) DESC, CAST(SUBSTR(STR_DATE, 4, 2) AS INT) DESC, CAST(SUBSTR(STR_DATE, 1, 2) AS INT) DESC")
            ImportVSD_GridView1.BestFitColumns()
            ImportVSD_GridView1.ExpandAllGroups()
        Catch ex As Exception
            MsgBox(ex.Message, vbCritical + vbOKOnly)
            WriteErrorLog(ex.ToString)
        End Try
    End Sub

    Private Sub BarButtonItem_GetAllCaseIncomplete_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem_GetAllCaseIncomplete.ItemClick
        Try
            ImportVSD_GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT * FROM FIXED_DAILY_VSD_REPORT WHERE CASE_STATUS = 'Incomplete' ORDER BY CAST(SUBSTR(STR_DATE, 7, 4) AS INT) DESC, CAST(SUBSTR(STR_DATE, 4, 2) AS INT) DESC, CAST(SUBSTR(STR_DATE, 1, 2) AS INT) DESC")
            ImportVSD_GridView1.BestFitColumns()
            ImportVSD_GridView1.ExpandAllGroups()
        Catch ex As Exception
            MsgBox(ex.Message, vbCritical + vbOKOnly)
            WriteErrorLog(ex.ToString)
        End Try
    End Sub

#End Region

#Region "Processing"

    Private Sub Processing_GridView_Queue1_CellValueChanged(sender As Object, e As CellValueChangedEventArgs) Handles Processing_GridView_Queue1.CellValueChanged
        Dim view As GridView = sender
        If view Is Nothing Then
            Exit Sub
        End If

        If e.Column.GetTextCaption <> "USER_NOTE" Then
            If e.Column.GetTextCaption <> "ACTION" Then
                Exit Sub
            End If
        End If

        If e.Column.GetTextCaption <> "ACTION" Then
            Dim iRet = MsgBox("Do you want to save this changes?", vbQuestion + vbYesNo, "SSO")
            If iRet = vbYes Then
                Dim USER_MODIFIED As String = UCase(Environment.UserName) & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss")
                Processing_GridView_Queue1.SetFocusedRowCellValue("USER_MODIFIED", USER_MODIFIED)
                SQL_QUERY(CONNECTION_SSO, "UPDATE PAYMENT_INSTRUCTION_REPORT SET USER_MODIFIED = '" & USER_MODIFIED & "', USER_NOTE = '" & Processing_GridView_Queue1.GetFocusedRowCellDisplayText("USER_NOTE").ToString & "' WHERE CASE_ID = '" & Processing_GridView_Queue1.GetFocusedRowCellDisplayText("CASE_ID").ToString & "'")
            End If
        End If
    End Sub

    Private Sub Processing_tbLinkFile_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles Processing_tbLinkFile.ButtonClick
        Dim fd As OpenFileDialog = New OpenFileDialog()

        fd.Title = "Select report Payment instruction"
        fd.RestoreDirectory = True
        fd.Filter = "Excel Files|*.xls;*.xlsx"

        fd.FilterIndex = 1
        fd.RestoreDirectory = True

        If fd.ShowDialog() = DialogResult.OK Then
            If fd.FileName.Length > 0 Then
                Processing_tbLinkFile.Text = fd.FileName
                Dim filePath As String = fd.FileName

                If IsWorkbookAlreadyOpen(filePath) = True Then
                    MsgBox("The file is already open." & Chr(10) & Chr(10) & filePath, vbCritical + vbOKOnly)
                    Exit Sub
                End If


                Dim connString As String = String.Empty
                If filePath.EndsWith(".xlsx") Then
                    connString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=No'", filePath)
                Else
                    connString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=No'", filePath)
                End If

                Dim connExcel As New OleDbConnection(connString)
                Dim cmdExcel As New OleDbCommand()
                Dim oda As New OleDbDataAdapter()

                Try
                    cmdExcel.Connection = connExcel
                    connExcel.Open()
                    Processing_cbSheetName.DataSource = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
                    Processing_cbSheetName.DisplayMember = "TABLE_NAME"
                    Processing_cbSheetName.ValueMember = "TABLE_NAME"
                    connExcel.Close()
                Catch ex As Exception
                    MsgBox(ex.Message, vbCritical + vbOKOnly)
                    WriteErrorLog(ex.ToString)
                Finally
                    If Not IsNothing(connExcel) Then : connExcel.Close() : End If
                End Try
            End If
        End If
    End Sub

    Private Sub Processing_btImport_Click(sender As Object, e As EventArgs) Handles Processing_btImport.Click
        Dim TimerStart As DateTime = Now
        Try
            SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)

            Dim result As String = IMPORT_PAYMENT_INSTRUCTION_REPORT(Processing_tbLinkFile.Text)

            If result = "Done" Then
                SplashScreenManager.CloseForm(False)
                Dim TimeSpent As System.TimeSpan = Now.Subtract(TimerStart)
                MsgBox("Completed!" & Chr(10) & "in " & Format(TimeSpent.TotalSeconds, "0.00") & " seconds", vbInformation + vbOKOnly, "SSO")
                'Processing_rbtSelectAll.Checked = True
            Else
                SplashScreenManager.CloseForm(False)
                MsgBox(result, vbCritical + vbOKOnly, "SSO")
            End If

        Catch ex As Exception
            WriteErrorLog(String.Format("Error: {0}", ex.ToString))
            SplashScreenManager.CloseForm(False)
            MsgBox(String.Format("Error: {0}", ex.Message), vbCritical + vbOKOnly, "SSO")
        End Try
    End Sub

    Public Function IMPORT_PAYMENT_INSTRUCTION_REPORT(LINK_PAYMENT_INSTRUCTION As String) As String
        Try
            If (Not System.IO.Directory.Exists(link_folder_database & "TEMP")) Then
                System.IO.Directory.CreateDirectory(link_folder_database & "TEMP")
            End If

            Dim LINK_TEMP_DATABASE = link_folder_database & "TEMP\TEMP_" & System.IO.Path.GetFileNameWithoutExtension(LINK_PAYMENT_INSTRUCTION) & "_" & Now.ToString("ddMMyyyyhhmmss") & ".txt"

            ' Create the SQLite database
            If Not My.Computer.FileSystem.FileExists(LINK_TEMP_DATABASE) Then
                SQLiteConnection.CreateFile(LINK_TEMP_DATABASE)
            End If

            CHANGE_DESCRIPTION_WAIT_FROM("Importing excel file to database - " & LINK_PAYMENT_INSTRUCTION)

            Dim BATCH_NAME As String = System.IO.Path.GetFileNameWithoutExtension(LINK_PAYMENT_INSTRUCTION) & "_" & Environment.UserName & "_" & Now.ToString("yyyyMMddhhmmsstt")

            SQL_QUERY_FROM_FILE(LINK_TEMP_DATABASE, "CREATE TABLE IF NOT EXISTS TEMP_PAYMENT_INSTRUCTION_REPORT(ID_NO INTEGER PRIMARY KEY AUTOINCREMENT, BATCH_NAME VARCHAR, CUSTOMER_ID VARCHAR, CUSTOMER_NAME VARCHAR, BATCH VARCHAR, SUB_BATCH VARCHAR, PMT_REF VARCHAR, PMT_STATUS VARCHAR, PMT_TYPE VARCHAR, DEBIT_AC_NO VARCHAR, DEBIT_AMOUNT VARCHAR, PAYMENT_CCY VARCHAR, PAYEE_AMOUNT VARCHAR, PROCESS_DATE VARCHAR, PAYEE_NAME VARCHAR, ADDRESS1 VARCHAR, ADDRESS2 VARCHAR, ADDRESS3 VARCHAR, BEN_ACC VARCHAR, BEN_BANK_CODE VARCHAR, PAYMENT_DETAILS_1 VARCHAR, PAYMENT_DETAILS_2 VARCHAR, LOCAL_CHRG VARCHAR, BANK_NAME VARCHAR, TT_BENEFICIARY_BANK_DETAILS VARCHAR, USER_CREATED VARCHAR, DATE_CREATED VARCHAR)")
            SQL_QUERY_FROM_FILE(LINK_TEMP_DATABASE, "CREATE TABLE IF NOT EXISTS PAYMENT_INSTRUCTION_REPORT_FINAL(CASE_ID VARCHAR, REF_NO VARCHAR, VSD_REF_NO VARCHAR, CUSTOMER_ID VARCHAR, CUSTOMER_NAME VARCHAR, BATCH VARCHAR, SUB_BATCH VARCHAR, PMT_REF VARCHAR, PMT_STATUS VARCHAR, PMT_TYPE VARCHAR, DEBIT_AC_NO VARCHAR, DEBIT_AMOUNT CURRENCY, PAYMENT_CCY VARCHAR, PAYEE_AMOUNT CURRENCY, PROCESS_DATE DATETIME, PAYEE_NAME VARCHAR, ADDRESS1 VARCHAR, ADDRESS2 VARCHAR, ADDRESS3 VARCHAR, BEN_ACC VARCHAR, BEN_BANK_CODE VARCHAR, PAYMENT_DETAILS_1 VARCHAR, PAYMENT_DETAILS_2 VARCHAR, LOCAL_CHRG VARCHAR, BANK_NAME VARCHAR, TT_BENEFICIARY_BANK_DETAILS VARCHAR, USER_CREATED VARCHAR, DATE_CREATED VARCHAR, BATCH_NAME VARCHAR, BATCH_STATUS VARCHAR, VSD_FULLNAME VARCHAR, VSD_BANKACNAME_CITYBANK VARCHAR, VSD_CASH_ACCOUNT VARCHAR, VSD_VALUE_DATE DATETIME, VSD_AMOUNT_PAYMENT CURRENCY, CHECK_NAME BOOLEAN, CHECK_BANK_NAME BOOLEAN, CHECK_CASH_ACC BOOLEAN, CHECK_VALUE_DATE BOOLEAN, CHECK_AMOUNT BOOLEAN, CHECK_BEN_CHARGE BOOLEAN, CHECK_BEN_CHARGE_FEE BOOLEAN, FINAL_CHECK BOOLEAN, CASE_STATUS VARCHAR, USER_NOTE VARCHAR)")
            SQL_QUERY_FROM_FILE(LINK_TEMP_DATABASE, "CREATE TABLE IF NOT EXISTS PAYMENT_INSTRUCTION_REPORT(CASE_ID VARCHAR, REF_NO VARCHAR, VSD_REF_NO VARCHAR, CUSTOMER_ID VARCHAR, CUSTOMER_NAME VARCHAR, BATCH VARCHAR, SUB_BATCH VARCHAR, PMT_REF VARCHAR, PMT_STATUS VARCHAR, PMT_TYPE VARCHAR, DEBIT_AC_NO VARCHAR, DEBIT_AMOUNT CURRENCY, PAYMENT_CCY VARCHAR, PAYEE_AMOUNT CURRENCY, PROCESS_DATE DATETIME, PAYEE_NAME VARCHAR, ADDRESS1 VARCHAR, ADDRESS2 VARCHAR, ADDRESS3 VARCHAR, BEN_ACC VARCHAR, BEN_BANK_CODE VARCHAR, PAYMENT_DETAILS_1 VARCHAR, PAYMENT_DETAILS_2 VARCHAR, LOCAL_CHRG VARCHAR, BANK_NAME VARCHAR, TT_BENEFICIARY_BANK_DETAILS VARCHAR, USER_CREATED VARCHAR, DATE_CREATED VARCHAR, BATCH_NAME VARCHAR, BATCH_STATUS VARCHAR, VSD_FULLNAME VARCHAR, VSD_BANKACNAME_CITYBANK VARCHAR, VSD_CASH_ACCOUNT VARCHAR, VSD_VALUE_DATE DATETIME, VSD_AMOUNT_PAYMENT CURRENCY, CHECK_NAME BOOLEAN, CHECK_BANK_NAME BOOLEAN, CHECK_CASH_ACC BOOLEAN, CHECK_VALUE_DATE BOOLEAN, CHECK_AMOUNT BOOLEAN, CHECK_BEN_CHARGE BOOLEAN, CHECK_BEN_CHARGE_FEE BOOLEAN, FINAL_CHECK BOOLEAN, CASE_STATUS VARCHAR, USER_NOTE VARCHAR)")
            SQL_QUERY_FROM_FILE(LINK_TEMP_DATABASE, "CREATE TABLE IF NOT EXISTS PAYMENT_INSTRUCTION_REPORT2(CASE_ID VARCHAR, REF_NO VARCHAR, VSD_REF_NO VARCHAR, CUSTOMER_ID VARCHAR, CUSTOMER_NAME VARCHAR, BATCH VARCHAR, SUB_BATCH VARCHAR, PMT_REF VARCHAR, PMT_STATUS VARCHAR, PMT_TYPE VARCHAR, DEBIT_AC_NO VARCHAR, DEBIT_AMOUNT CURRENCY, PAYMENT_CCY VARCHAR, PAYEE_AMOUNT CURRENCY, PROCESS_DATE DATETIME, PAYEE_NAME VARCHAR, ADDRESS1 VARCHAR, ADDRESS2 VARCHAR, ADDRESS3 VARCHAR, BEN_ACC VARCHAR, BEN_BANK_CODE VARCHAR, PAYMENT_DETAILS_1 VARCHAR, PAYMENT_DETAILS_2 VARCHAR, LOCAL_CHRG VARCHAR, BANK_NAME VARCHAR, TT_BENEFICIARY_BANK_DETAILS VARCHAR, USER_CREATED VARCHAR, DATE_CREATED VARCHAR, BATCH_NAME VARCHAR, BATCH_STATUS VARCHAR, VSD_FULLNAME VARCHAR, VSD_BANKACNAME_CITYBANK VARCHAR, VSD_CASH_ACCOUNT VARCHAR, VSD_VALUE_DATE DATETIME, VSD_AMOUNT_PAYMENT CURRENCY, CHECK_NAME BOOLEAN, CHECK_BANK_NAME BOOLEAN, CHECK_CASH_ACC BOOLEAN, CHECK_VALUE_DATE BOOLEAN, CHECK_AMOUNT BOOLEAN, CHECK_BEN_CHARGE BOOLEAN, CHECK_BEN_CHARGE_FEE BOOLEAN, FINAL_CHECK BOOLEAN, CASE_STATUS VARCHAR, USER_NOTE VARCHAR)")
            SQL_QUERY_FROM_FILE(LINK_TEMP_DATABASE, "CREATE TABLE IF NOT EXISTS PAYMENT_INSTRUCTION_REPORT3(BEN_CHARGE_FOR_LOW_VALUE NUMERIC, BEN_CHARGE_FOR_HIGH_VALUE NUMERIC, CASE_ID VARCHAR, REF_NO VARCHAR, VSD_REF_NO VARCHAR, CUSTOMER_ID VARCHAR, CUSTOMER_NAME VARCHAR, BATCH VARCHAR, SUB_BATCH VARCHAR, PMT_REF VARCHAR, PMT_STATUS VARCHAR, PMT_TYPE VARCHAR, DEBIT_AC_NO VARCHAR, DEBIT_AMOUNT CURRENCY, PAYMENT_CCY VARCHAR, PAYEE_AMOUNT CURRENCY, PROCESS_DATE DATETIME, PAYEE_NAME VARCHAR, ADDRESS1 VARCHAR, ADDRESS2 VARCHAR, ADDRESS3 VARCHAR, BEN_ACC VARCHAR, BEN_BANK_CODE VARCHAR, PAYMENT_DETAILS_1 VARCHAR, PAYMENT_DETAILS_2 VARCHAR, LOCAL_CHRG VARCHAR, BANK_NAME VARCHAR, TT_BENEFICIARY_BANK_DETAILS VARCHAR, USER_CREATED VARCHAR, DATE_CREATED VARCHAR, BATCH_NAME VARCHAR, BATCH_STATUS VARCHAR, VSD_FULLNAME VARCHAR, VSD_BANKACNAME_CITYBANK VARCHAR, VSD_CASH_ACCOUNT VARCHAR, VSD_VALUE_DATE DATETIME, VSD_AMOUNT_PAYMENT CURRENCY, CHECK_NAME BOOLEAN, CHECK_BANK_NAME BOOLEAN, CHECK_CASH_ACC BOOLEAN, CHECK_VALUE_DATE BOOLEAN, CHECK_AMOUNT BOOLEAN, CHECK_BEN_CHARGE BOOLEAN, CHECK_BEN_CHARGE_FEE BOOLEAN, FINAL_CHECK BOOLEAN, CASE_STATUS VARCHAR, USER_NOTE VARCHAR)")
            SQL_QUERY_FROM_FILE(LINK_TEMP_DATABASE, "CREATE TABLE IF NOT EXISTS FIXED_DAILY_VSD_REPORT(CASE_ID VARCHAR NOT NULL UNIQUE PRIMARY KEY, STR_DATE VARCHAR, REF_NO VARCHAR, FULLNAME VARCHAR, MBNAME VARCHAR, ACTYPE VARCHAR, DBCODE VARCHAR, ORDERQTTY VARCHAR, MATCHQTTY VARCHAR, MATCHAMT VARCHAR, FEEAMC VARCHAR, FEEDXX VARCHAR, FEEFUND VARCHAR, TOTALFEE VARCHAR, TAXAMT VARCHAR, AMT NUMERIC, CUSTODYCD VARCHAR, BANKACNAME VARCHAR, CITYBANK VARCHAR, BANKACC VARCHAR, NOTE VARCHAR, TRADINGID VARCHAR, NAV VARCHAR, TOTALNAV VARCHAR, TXDATE VARCHAR, SYMBOL VARCHAR, NAME VARCHAR, FEENAME VARCHAR, EXECNAME VARCHAR, FEEID VARCHAR, SRTYPE VARCHAR, FILE_PATH VARCHAR, CASH_ACCOUNT VARCHAR, AMOUNT_PAYMENT NUMERIC, TRADE_DATE DATE, VALUE_DATE DATE, DEADLINE_PER_FUND DATE, FULLNAME_UNSIGN VARCHAR, BANKACNAME_UNSIGN VARCHAR, CITYBANK_UNSIGN VARCHAR, USER_CREATED VARCHAR, CASE_STATUS VARCHAR, USER_MODIFIED VARCHAR, BATCH_PAYMENT_INSTRUCTION VARCHAR, USER_NOTE VARCHAR)")
            SQL_QUERY_FROM_FILE(LINK_TEMP_DATABASE, "CREATE TABLE IF NOT EXISTS LIST_CASH_ACCOUNT(CASE_ID VARCHAR, SYMBOL VARCHAR, CASH_ACCOUNT VARCHAR, VALUE_DATE_ADD_DAYS NUMERIC, DEADLINE_ADD_DAYS NUMERIC, BEN_CHARGE_FOR_LOW_VALUE NUMERIC, BEN_CHARGE_FOR_HIGH_VALUE NUMERIC)")

            Dim SQL_STRING_EXCEL As String = "SELECT '" & BATCH_NAME & "' AS BATCH_NAME, [Customer ID] AS CUSTOMER_ID, [Customer Name] AS CUSTOMER_NAME, [Batch] AS BATCH, [Sub-batch] AS SUB_BATCH, [Pmt Ref] AS PMT_REF, [Status] AS PMT_STATUS, [Pmt Type] AS PMT_TYPE, [Debit Acct No] AS DEBIT_AC_NO, [Debit Amount] AS DEBIT_AMOUNT, [Payment Cur] AS PAYMENT_CCY, [Payee Amount] AS PAYEE_AMOUNT, [Process date] AS PROCESS_DATE, [Payee Name] AS PAYEE_NAME, [Address 1] AS ADDRESS1, [Address 2] AS ADDRESS2, [Address 3] AS ADDRESS3, [Ben Account] AS BEN_ACC, [Ben Bank code] AS BEN_BANK_CODE, [Payment Details 1] AS PAYMENT_DETAILS_1, [Payment Details 2] AS PAYMENT_DETAILS_2, [Local Chrg] AS LOCAL_CHRG, [Bank name] AS BANK_NAME, [TT Beneficiary Bank Details] AS TT_BENEFICIARY_BANK_DETAILS, '" & Environment.UserName & "' AS USER_CREATED, '" & Now.ToString("dd/MM/yyyy hh:mm:ss tt") & "' AS DATE_CREATED FROM [" & Processing_cbSheetName.Text & "]"

            'import full data from excel file
            SQLITE_BULK_COPY(SQL_EXCEL_TO_DATATABLE(Processing_tbLinkFile.Text, SQL_STRING_EXCEL), LINK_TEMP_DATABASE, "TEMP_PAYMENT_INSTRUCTION_REPORT")

            'check txn đã tồn tại trong database hay chưa
            SQL_QUERY_FROM_FILE(LINK_TEMP_DATABASE, "CREATE TABLE IF NOT EXISTS DATABASE_PAYMENT_INSTRUCTION_REPORT(CASE_ID VARCHAR NOT NULL UNIQUE PRIMARY KEY, REF_NO VARCHAR, VSD_REF_NO VARCHAR, CUSTOMER_ID VARCHAR, CUSTOMER_NAME VARCHAR, BATCH VARCHAR, SUB_BATCH VARCHAR, PMT_REF VARCHAR, PMT_STATUS VARCHAR, PMT_TYPE VARCHAR, DEBIT_AC_NO VARCHAR, DEBIT_AMOUNT NUMERIC, PAYMENT_CCY VARCHAR, PAYEE_AMOUNT NUMERIC, PROCESS_DATE DATETIME, PAYEE_NAME VARCHAR, ADDRESS1 VARCHAR, ADDRESS2 VARCHAR, ADDRESS3 VARCHAR, BEN_ACC VARCHAR, BEN_BANK_CODE VARCHAR, PAYMENT_DETAILS_1 VARCHAR, PAYMENT_DETAILS_2 VARCHAR, LOCAL_CHRG VARCHAR, BANK_NAME VARCHAR, TT_BENEFICIARY_BANK_DETAILS VARCHAR, USER_CREATED VARCHAR, DATE_CREATED VARCHAR, BATCH_NAME VARCHAR, BATCH_STATUS VARCHAR, VSD_FULLNAME VARCHAR, VSD_BANKACNAME_CITYBANK VARCHAR, VSD_CASH_ACCOUNT VARCHAR, VSD_VALUE_DATE DATETIME, VSD_AMOUNT_PAYMENT NUMERIC, CHECK_NAME BOOLEAN, CHECK_BANK_NAME BOOLEAN, CHECK_CASH_ACC BOOLEAN, CHECK_VALUE_DATE BOOLEAN, CHECK_AMOUNT BOOLEAN, CHECK_BEN_CHARGE BOOLEAN, CHECK_BEN_CHARGE_FEE BOOLEAN, FINAL_CHECK BOOLEAN, CASE_STATUS VARCHAR, USER_MODIFIED VARCHAR, USER_NOTE VARCHAR)")
            SQLITE_BULK_COPY(SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT * FROM PAYMENT_INSTRUCTION_REPORT"), LINK_TEMP_DATABASE, "DATABASE_PAYMENT_INSTRUCTION_REPORT")

            Dim data_not_yet_imported As DataTable = SQL_FROMFILE_TO_DATATABLE(LINK_TEMP_DATABASE, "SELECT '" & Environment.UserName & "'||'" & Now.ToString("yyyyMMddhhmmsstt") & "'||ID_NO AS CASE_ID, BEN_ACC||DEBIT_AMOUNT AS REF_NO, CUSTOMER_ID, CUSTOMER_NAME, BATCH, SUB_BATCH, PMT_REF, PMT_STATUS, PMT_TYPE, DEBIT_AC_NO, CAST(DEBIT_AMOUNT AS CURRENCY) AS DEBIT_AMOUNT, PAYMENT_CCY, CAST(PAYEE_AMOUNT AS CURRENCY) AS PAYEE_AMOUNT, PROCESS_DATE, PAYEE_NAME, ADDRESS1, ADDRESS2, ADDRESS3, BEN_ACC, BEN_BANK_CODE, PAYMENT_DETAILS_1, PAYMENT_DETAILS_2, LOCAL_CHRG, BANK_NAME, TT_BENEFICIARY_BANK_DETAILS, USER_CREATED, DATE_CREATED, BATCH_NAME, 'Incomplete' AS CASE_STATUS, 'Incomplete' AS BATCH_STATUS FROM TEMP_PAYMENT_INSTRUCTION_REPORT WHERE CUSTOMER_ID||PMT_REF NOT IN (SELECT DISTINCT CUSTOMER_ID||PMT_REF FROM DATABASE_PAYMENT_INSTRUCTION_REPORT)")
            Dim data_already_imported As DataTable = SQL_FROMFILE_TO_DATATABLE(LINK_TEMP_DATABASE, "SELECT * FROM TEMP_PAYMENT_INSTRUCTION_REPORT WHERE CUSTOMER_ID||PMT_REF IN (SELECT DISTINCT CUSTOMER_ID||PMT_REF FROM DATABASE_PAYMENT_INSTRUCTION_REPORT)")

            If data_not_yet_imported.Rows.Count = 0 Then
                If data_already_imported.Rows.Count > 0 Then
                    DataTableToExcel(data_already_imported)
                End If
                Return "This data already imported!"
            End If

            SQLITE_BULK_COPY(data_not_yet_imported, LINK_TEMP_DATABASE, "PAYMENT_INSTRUCTION_REPORT")

            SQLITE_BULK_COPY(SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT * FROM FIXED_DAILY_VSD_REPORT WHERE (CASE_STATUS <> 'Completed' AND CASE_STATUS <> 'CANCEL')"), LINK_TEMP_DATABASE, "FIXED_DAILY_VSD_REPORT")

            SQLITE_BULK_COPY(SQL_QUERY_TO_DATATABLE(CONNECTION_CONFIG, "SELECT * FROM LIST_CASH_ACCOUNT"), LINK_TEMP_DATABASE, "LIST_CASH_ACCOUNT")

            Dim SQL_STRING As String = "SELECT DISTINCT A.CASE_ID, A.REF_NO, B.REF_NO AS VSD_REF_NO, A.CUSTOMER_ID, A.CUSTOMER_NAME, A.BATCH, A.SUB_BATCH, A.PMT_REF, A.PMT_STATUS, A.PMT_TYPE, A.DEBIT_AC_NO, A.DEBIT_AMOUNT, A.PAYMENT_CCY, A.PAYEE_AMOUNT, A.PROCESS_DATE, A.PAYEE_NAME, A.ADDRESS1, A.ADDRESS2, A.ADDRESS3, A.BEN_ACC, A.BEN_BANK_CODE, A.PAYMENT_DETAILS_1, A.PAYMENT_DETAILS_2, A.LOCAL_CHRG, A.BANK_NAME, A.TT_BENEFICIARY_BANK_DETAILS, A.USER_CREATED, A.DATE_CREATED, A.BATCH_NAME, A.BATCH_STATUS, B.FULLNAME_UNSIGN AS VSD_FULLNAME, CASE WHEN B.CITYBANK_UNSIGN IS NULL THEN B.BANKACNAME_UNSIGN ELSE B.BANKACNAME_UNSIGN||','||B.CITYBANK_UNSIGN END AS VSD_BANKACNAME_CITYBANK, B.CASH_ACCOUNT AS VSD_CASH_ACCOUNT, B.VALUE_DATE AS VSD_VALUE_DATE, B.AMOUNT_PAYMENT AS VSD_AMOUNT_PAYMENT, CASE UPPER(B.FULLNAME_UNSIGN) WHEN UPPER(A.PAYEE_NAME) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||UPPER(A.ADDRESS1) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||' '||UPPER(A.ADDRESS1) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||UPPER(A.ADDRESS1)||UPPER(A.ADDRESS2) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||UPPER(A.ADDRESS1)||' '||UPPER(A.ADDRESS2) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||' '||UPPER(A.ADDRESS1)||UPPER(A.ADDRESS2) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||' '||UPPER(A.ADDRESS1)||' '||UPPER(A.ADDRESS2) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||UPPER(A.ADDRESS1)||UPPER(A.ADDRESS2)||UPPER(A.ADDRESS3) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||UPPER(A.ADDRESS1)||UPPER(A.ADDRESS2)||' '||UPPER(A.ADDRESS3) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||UPPER(A.ADDRESS1)||' '||UPPER(A.ADDRESS2)||UPPER(A.ADDRESS3) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||UPPER(A.ADDRESS1)||' '||UPPER(A.ADDRESS2)||' '||UPPER(A.ADDRESS3) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||' '||UPPER(A.ADDRESS1)||UPPER(A.ADDRESS2)||UPPER(A.ADDRESS3) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||' '||UPPER(A.ADDRESS1)||UPPER(A.ADDRESS2)||' '||UPPER(A.ADDRESS3) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||' '||UPPER(A.ADDRESS1)||' '||UPPER(A.ADDRESS2)||UPPER(A.ADDRESS3) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||' '||UPPER(A.ADDRESS1)||' '||UPPER(A.ADDRESS2)||UPPER(A.ADDRESS3) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||' '||UPPER(A.ADDRESS1)||' '||UPPER(A.ADDRESS2)||' '||UPPER(A.ADDRESS3) THEN TRUE ELSE FALSE END AS CHECK_NAME, A.CHECK_BANK_NAME, CASE WHEN B.CASH_ACCOUNT = A.DEBIT_AC_NO THEN TRUE ELSE FALSE END AS CHECK_CASH_ACC, CASE WHEN A.PROCESS_DATE = B.VALUE_DATE THEN TRUE ELSE FALSE END AS CHECK_VALUE_DATE, CASE WHEN B.AMOUNT_PAYMENT = A.DEBIT_AMOUNT THEN TRUE ELSE FALSE END AS CHECK_AMOUNT, CASE WHEN A.PMT_TYPE = 'BT' THEN TRUE ELSE CASE WHEN A.DEBIT_AMOUNT > A.PAYEE_AMOUNT THEN CASE WHEN A.DEBIT_AC_NO <> A.BEN_ACC THEN TRUE ELSE FALSE END ELSE FALSE END END AS CHECK_BEN_CHARGE, FALSE AS CHECK_BEN_CHARGE_FEE, FINAL_CHECK, A.CASE_STATUS FROM PAYMENT_INSTRUCTION_REPORT A" &
                                        " INNER JOIN FIXED_DAILY_VSD_REPORT B ON A.REF_NO = B.REF_NO" &
                                        " UNION ALL" &
                                        " SELECT DISTINCT A.CASE_ID, A.REF_NO, '' AS VSD_REF_NO, A.CUSTOMER_ID, A.CUSTOMER_NAME, A.BATCH, A.SUB_BATCH, A.PMT_REF, A.PMT_STATUS, A.PMT_TYPE, A.DEBIT_AC_NO, A.DEBIT_AMOUNT, A.PAYMENT_CCY, A.PAYEE_AMOUNT, A.PROCESS_DATE, A.PAYEE_NAME, A.ADDRESS1, A.ADDRESS2, A.ADDRESS3, A.BEN_ACC, A.BEN_BANK_CODE, A.PAYMENT_DETAILS_1, A.PAYMENT_DETAILS_2, A.LOCAL_CHRG, A.BANK_NAME, A.TT_BENEFICIARY_BANK_DETAILS, A.USER_CREATED, A.DATE_CREATED, A.BATCH_NAME, A.BATCH_STATUS, VSD_FULLNAME, VSD_BANKACNAME_CITYBANK, VSD_CASH_ACCOUNT, VSD_VALUE_DATE, VSD_AMOUNT_PAYMENT, FALSE AS CHECK_NAME, FALSE AS CHECK_BANK_NAME, FALSE AS CHECK_CASH_ACC, FALSE AS CHECK_VALUE_DATE, FALSE AS CHECK_AMOUNT, CASE WHEN A.PMT_TYPE = 'BT' THEN TRUE ELSE CASE WHEN A.DEBIT_AMOUNT > A.PAYEE_AMOUNT THEN CASE WHEN A.DEBIT_AC_NO <> A.BEN_ACC THEN TRUE ELSE FALSE END ELSE FALSE END END AS CHECK_BEN_CHARGE, CASE WHEN A.PMT_TYPE = 'BT' THEN CASE WHEN A.DEBIT_AMOUNT = A.PAYEE_AMOUNT THEN TRUE ELSE FALSE END ELSE FALSE END AS CHECK_BEN_CHARGE_FEE, FINAL_CHECK, A.CASE_STATUS FROM PAYMENT_INSTRUCTION_REPORT A" &
                                        " LEFT OUTER JOIN FIXED_DAILY_VSD_REPORT B ON A.REF_NO = B.REF_NO WHERE B.REF_NO IS NULL"

            SQLITE_BULK_COPY(SQL_FROMFILE_TO_DATATABLE(LINK_TEMP_DATABASE, SQL_STRING), LINK_TEMP_DATABASE, "PAYMENT_INSTRUCTION_REPORT2")

            SQL_STRING = "SELECT DISTINCT B.BEN_CHARGE_FOR_LOW_VALUE, B.BEN_CHARGE_FOR_HIGH_VALUE, A.CASE_ID, A.REF_NO, A.VSD_REF_NO, A.CUSTOMER_ID, A.CUSTOMER_NAME, A.BATCH, A.SUB_BATCH, A.PMT_REF, A.PMT_STATUS, A.PMT_TYPE, A.DEBIT_AC_NO, A.DEBIT_AMOUNT, A.PAYMENT_CCY, A.PAYEE_AMOUNT, A.PROCESS_DATE, A.PAYEE_NAME, A.ADDRESS1, A.ADDRESS2, A.ADDRESS3, A.BEN_ACC, A.BEN_BANK_CODE, A.PAYMENT_DETAILS_1, A.PAYMENT_DETAILS_2, A.LOCAL_CHRG, A.BANK_NAME, A.TT_BENEFICIARY_BANK_DETAILS, A.USER_CREATED, A.DATE_CREATED, A.BATCH_NAME, A.BATCH_STATUS, A.VSD_FULLNAME, A.VSD_BANKACNAME_CITYBANK, A.VSD_CASH_ACCOUNT, A.VSD_VALUE_DATE, A.VSD_AMOUNT_PAYMENT, A.CHECK_NAME, CASE WHEN UPPER(SUBSTR(A.VSD_BANKACNAME_CITYBANK,5,LENGTH(VSD_BANKACNAME_CITYBANK)-4)) = UPPER(A.TT_BENEFICIARY_BANK_DETAILS) THEN TRUE ELSE CASE WHEN UPPER(A.TT_BENEFICIARY_BANK_DETAILS) = UPPER(A.VSD_BANKACNAME_CITYBANK) THEN TRUE ELSE FALSE END END AS CHECK_BANK_NAME, A.CHECK_CASH_ACC, A.CHECK_VALUE_DATE, A.CHECK_AMOUNT, A.CHECK_BEN_CHARGE, CASE WHEN A.DEBIT_AMOUNT < 500000000 THEN CASE WHEN B.BEN_CHARGE_FOR_LOW_VALUE = A.DEBIT_AMOUNT - A.PAYEE_AMOUNT THEN TRUE ELSE FALSE END ELSE CASE WHEN B.BEN_CHARGE_FOR_HIGH_VALUE = A.DEBIT_AMOUNT - A.PAYEE_AMOUNT THEN TRUE ELSE FALSE END END AS CHECK_BEN_CHARGE_FEE, A.FINAL_CHECK, A.CASE_STATUS, A.USER_NOTE FROM PAYMENT_INSTRUCTION_REPORT2 A" &
                            " INNER JOIN LIST_CASH_ACCOUNT B ON A.VSD_CASH_ACCOUNT = B.CASH_ACCOUNT" &
                            " UNION ALL" &
                            " SELECT DISTINCT BEN_CHARGE_FOR_LOW_VALUE, BEN_CHARGE_FOR_HIGH_VALUE, A.CASE_ID, A.REF_NO, A.VSD_REF_NO, A.CUSTOMER_ID, A.CUSTOMER_NAME, A.BATCH, A.SUB_BATCH, A.PMT_REF, A.PMT_STATUS, A.PMT_TYPE, A.DEBIT_AC_NO, A.DEBIT_AMOUNT, A.PAYMENT_CCY, A.PAYEE_AMOUNT, A.PROCESS_DATE, A.PAYEE_NAME, A.ADDRESS1, A.ADDRESS2, A.ADDRESS3, A.BEN_ACC, A.BEN_BANK_CODE, A.PAYMENT_DETAILS_1, A.PAYMENT_DETAILS_2, A.LOCAL_CHRG, A.BANK_NAME, A.TT_BENEFICIARY_BANK_DETAILS, A.USER_CREATED, A.DATE_CREATED, A.BATCH_NAME, A.BATCH_STATUS, A.VSD_FULLNAME, A.VSD_BANKACNAME_CITYBANK, A.VSD_CASH_ACCOUNT, A.VSD_VALUE_DATE, A.VSD_AMOUNT_PAYMENT, A.CHECK_NAME, FALSE AS CHECK_BANK_NAME, A.CHECK_CASH_ACC, A.CHECK_VALUE_DATE, A.CHECK_AMOUNT, A.CHECK_BEN_CHARGE, CASE WHEN A.PMT_TYPE = 'BT' THEN CASE WHEN A.DEBIT_AMOUNT = A.PAYEE_AMOUNT THEN TRUE ELSE FALSE END ELSE FALSE END AS CHECK_BEN_CHARGE_FEE, A.FINAL_CHECK, A.CASE_STATUS, A.USER_NOTE FROM PAYMENT_INSTRUCTION_REPORT2 A" &
                            " LEFT OUTER JOIN LIST_CASH_ACCOUNT B ON A.VSD_CASH_ACCOUNT = B.CASH_ACCOUNT WHERE B.CASH_ACCOUNT IS NULL"

            SQLITE_BULK_COPY(SQL_FROMFILE_TO_DATATABLE(LINK_TEMP_DATABASE, SQL_STRING), LINK_TEMP_DATABASE, "PAYMENT_INSTRUCTION_REPORT3")

            Dim DT As DataTable = SQL_FROMFILE_TO_DATATABLE(LINK_TEMP_DATABASE, "SELECT * FROM PAYMENT_INSTRUCTION_REPORT3 WHERE PMT_TYPE <> 'BT' AND (CHECK_BANK_NAME = FALSE OR CHECK_AMOUNT = FALSE)")

            Dim I As Integer = 0
            Dim TOTAL_ROWS As Integer = DT.Rows.Count
            For Each Drr As DataRow In DT.Rows
                I = I + 1
                CHANGE_DESCRIPTION_WAIT_FROM("[" & I & "/" & TOTAL_ROWS & "] - Additional Checking: " & Drr("PAYEE_NAME").ToString)

                Dim CHECK_BANK_NAME As Boolean = Drr("CHECK_BANK_NAME")
                Dim CHECK_AMOUNT As Boolean = Drr("CHECK_AMOUNT")

                If CHECK_BANK_NAME = False Then
                    Dim VSD_BANKACNAME_CITYBANK As String = CUT_BANK_NAME_INITIAL(Drr("VSD_BANKACNAME_CITYBANK").ToString)
                    If Drr("TT_BENEFICIARY_BANK_DETAILS").ToString = VSD_BANKACNAME_CITYBANK Then
                        CHECK_BANK_NAME = True
                    End If
                End If

                Dim VSD_REF_NO As String = ""
                If CHECK_AMOUNT = False Then
                    Dim TOTAL_AMT_VSD_REPORT As Long = SQL_QUERY_TO_LONG(CONNECTION_SSO, "SELECT SUM(AMOUNT_PAYMENT) FROM FIXED_DAILY_VSD_REPORT WHERE (CASE_STATUS <> 'Completed' AND CASE_STATUS <> 'CANCEL') AND CASH_ACCOUNT = '" & Drr("DEBIT_AC_NO").ToString & "' AND BANKACC = '" & GET_ONLY_NUMERIC_FROM_STRING(Drr("BEN_ACC").ToString) & "'")
                    If CLng(Drr("DEBIT_AMOUNT").ToString) = TOTAL_AMT_VSD_REPORT Then
                        CHECK_AMOUNT = True
                        VSD_REF_NO = SQL_QUERY_TO_STRING(CONNECTION_SSO, "SELECT GROUP_CONCAT(REF_NO,';') FROM FIXED_DAILY_VSD_REPORT WHERE (CASE_STATUS <> 'Completed' AND CASE_STATUS <> 'CANCEL') AND CASH_ACCOUNT = '" & Drr("DEBIT_AC_NO").ToString & "' AND BANKACC = '" & GET_ONLY_NUMERIC_FROM_STRING(Drr("BEN_ACC").ToString) & "'")
                    End If
                End If

                If CHECK_AMOUNT = True Then
                    SQL_QUERY_FROM_FILE(LINK_TEMP_DATABASE, "UPDATE PAYMENT_INSTRUCTION_REPORT3 SET VSD_REF_NO = '" & VSD_REF_NO & "', CHECK_AMOUNT = " & CHECK_AMOUNT & " WHERE CASE_ID = '" & Drr("CASE_ID").ToString & "'")

                    Dim DT_CHECK As DataTable = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT DISTINCT REF_NO FROM FIXED_DAILY_VSD_REPORT WHERE (CASE_STATUS <> 'Completed' AND CASE_STATUS <> 'CANCEL') AND CASH_ACCOUNT = '" & Drr("DEBIT_AC_NO").ToString & "' AND BANKACC = '" & GET_ONLY_NUMERIC_FROM_STRING(Drr("BEN_ACC").ToString) & "'")
                    Dim CHECK_NAME As Boolean = False
                    Dim CHECK_CASH_ACCOUNT As Boolean = False
                    Dim CHECK_VALUE_DATE As Boolean = False

                    For Each DRR_CHECK As DataRow In DT_CHECK.Rows
                        If Drr("PAYEE_NAME").ToString() = SQL_QUERY_TO_STRING(CONNECTION_SSO, "SELECT DISTINCT FULLNAME_UNSIGN FROM FIXED_DAILY_VSD_REPORT WHERE REF_NO = '" & DRR_CHECK("REF_NO").ToString & "'") Then
                            CHECK_NAME = True
                        Else
                            CHECK_NAME = False
                        End If

                        If Drr("DEBIT_AC_NO").ToString = SQL_QUERY_TO_STRING(CONNECTION_SSO, "SELECT DISTINCT CASH_ACCOUNT FROM FIXED_DAILY_VSD_REPORT WHERE REF_NO = '" & DRR_CHECK("REF_NO").ToString & "'") Then
                            CHECK_CASH_ACCOUNT = True
                        Else
                            CHECK_CASH_ACCOUNT = False
                        End If

                        If Drr("PROCESS_DATE").ToString = SQL_QUERY_TO_STRING(CONNECTION_SSO, "SELECT DISTINCT VALUE_DATE FROM FIXED_DAILY_VSD_REPORT WHERE REF_NO = '" & DRR_CHECK("REF_NO").ToString & "'") Then
                            CHECK_VALUE_DATE = True
                        Else
                            CHECK_VALUE_DATE = False
                        End If
                    Next

                    If CHECK_NAME = True Then
                        SQL_QUERY_FROM_FILE(LINK_TEMP_DATABASE, "UPDATE PAYMENT_INSTRUCTION_REPORT3 SET CHECK_NAME = TRUE WHERE CASE_ID = '" & Drr("CASE_ID").ToString & "'")
                    End If

                    If CHECK_CASH_ACCOUNT = True Then
                        SQL_QUERY_FROM_FILE(LINK_TEMP_DATABASE, "UPDATE PAYMENT_INSTRUCTION_REPORT3 SET CHECK_CASH_ACC = TRUE WHERE CASE_ID = '" & Drr("CASE_ID").ToString & "'")
                    End If

                    If CHECK_VALUE_DATE = True Then
                        SQL_QUERY_FROM_FILE(LINK_TEMP_DATABASE, "UPDATE PAYMENT_INSTRUCTION_REPORT3 SET CHECK_VALUE_DATE = TRUE WHERE CASE_ID = '" & Drr("CASE_ID").ToString & "'")
                    End If
                End If

                If CHECK_BANK_NAME = True Then
                    SQL_QUERY_FROM_FILE(LINK_TEMP_DATABASE, "UPDATE PAYMENT_INSTRUCTION_REPORT3 SET VSD_REF_NO = '" & VSD_REF_NO & "', CHECK_BANK_NAME = " & CHECK_BANK_NAME & " WHERE CASE_ID = '" & Drr("CASE_ID").ToString & "'")
                End If
            Next

            DT = SQL_FROMFILE_TO_DATATABLE(LINK_TEMP_DATABASE, "SELECT * FROM PAYMENT_INSTRUCTION_REPORT3 WHERE PMT_TYPE = 'BT'")
            If DT.Rows.Count > 0 Then
                I = 0
                TOTAL_ROWS = DT.Rows.Count

                For Each DRR As DataRow In DT.Rows
                    I = I + 1
                    CHANGE_DESCRIPTION_WAIT_FROM("[" & I & "/" & TOTAL_ROWS & "] - CHECK INTERNAL TRANSFER: " & DRR("PAYEE_NAME").ToString)

                    Dim DT_CHECK_INTERNAL_PAYMENT As DataTable = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT * FROM INTERNAL_TRANSFER WHERE STATUS = 'Incomplete' AND CASH_ACCOUNT = '" & GET_ONLY_NUMERIC_FROM_STRING(DRR("BEN_ACC").ToString) & "'")
                    For Each Drr_CHECK As DataRow In DT_CHECK_INTERNAL_PAYMENT.Rows
                        Dim CASE_ID_CHECK_INTERNAL_PAYMENT As String = Drr_CHECK("CASE_ID").ToString
                        Dim PAYMENT_CASE_ID As String = Drr_CHECK("PAYMENT_CASE_ID").ToString
                        Dim TOTAL_AMOUNT As Long = CLng(Drr_CHECK("TOTAL_AMOUNT").ToString)
                        Dim PAYMENT_AMOUNT As Long
                        If Drr_CHECK("PAYMENT_AMOUNT").ToString = "" Then
                            PAYMENT_AMOUNT = 0
                        Else
                            PAYMENT_AMOUNT = CLng(Drr_CHECK("PAYMENT_AMOUNT").ToString)
                        End If
                        WriteLog_Full("PAYMENT_AMOUNT = " & PAYMENT_AMOUNT)
                        If TOTAL_AMOUNT <> PAYMENT_AMOUNT Then
                            If PAYMENT_CASE_ID.Length = 0 Then
                                If CLng(DRR("DEBIT_AMOUNT").ToString) > TOTAL_AMOUNT Or CLng(DRR("DEBIT_AMOUNT").ToString) = TOTAL_AMOUNT Then
                                    SQL_QUERY(CONNECTION_SSO, "UPDATE INTERNAL_TRANSFER SET PAYMENT_CASE_ID = '" & DRR("CASE_ID").ToString & "', PAYMENT_AMOUNT = '" & DRR("DEBIT_AMOUNT").ToString & "' WHERE CASE_ID = '" & CASE_ID_CHECK_INTERNAL_PAYMENT & "'")
                                End If
                            End If
                        End If
                    Next
                Next
            End If


            SQL_QUERY_FROM_FILE(LINK_TEMP_DATABASE, "DELETE FROM PAYMENT_INSTRUCTION_REPORT")
            SQLITE_BULK_COPY(SQL_FROMFILE_TO_DATATABLE(LINK_TEMP_DATABASE, "SELECT CASE_ID, REF_NO, VSD_REF_NO, CUSTOMER_ID, CUSTOMER_NAME, BATCH, SUB_BATCH, PMT_REF, PMT_STATUS, PMT_TYPE, DEBIT_AC_NO, DEBIT_AMOUNT, PAYMENT_CCY, PAYEE_AMOUNT, PROCESS_DATE, PAYEE_NAME, ADDRESS1, ADDRESS2, ADDRESS3, BEN_ACC, BEN_BANK_CODE, PAYMENT_DETAILS_1, PAYMENT_DETAILS_2, LOCAL_CHRG, BANK_NAME, TT_BENEFICIARY_BANK_DETAILS, USER_CREATED, DATE_CREATED, BATCH_NAME, BATCH_STATUS, VSD_FULLNAME, VSD_BANKACNAME_CITYBANK, VSD_CASH_ACCOUNT, VSD_VALUE_DATE, VSD_AMOUNT_PAYMENT, CHECK_NAME, CHECK_BANK_NAME, CHECK_CASH_ACC, CHECK_VALUE_DATE, CHECK_AMOUNT, CHECK_BEN_CHARGE, CHECK_BEN_CHARGE_FEE, CASE WHEN CHECK_NAME != 1 AND CHECK_BANK_NAME IS TRUE AND CHECK_CASH_ACC != 1 AND CHECK_VALUE_DATE != 1 AND CHECK_VALUE_DATE != 1 AND CHECK_AMOUNT != 1 AND CHECK_BEN_CHARGE != 1 AND CHECK_BEN_CHARGE_FEE IS TRUE THEN TRUE ELSE FALSE END AS FINAL_CHECK, CASE_STATUS, USER_NOTE FROM PAYMENT_INSTRUCTION_REPORT3"), LINK_TEMP_DATABASE, "PAYMENT_INSTRUCTION_REPORT")

            SQLITE_BULK_COPY(SQL_FROMFILE_TO_DATATABLE(LINK_TEMP_DATABASE, "SELECT DISTINCT * FROM PAYMENT_INSTRUCTION_REPORT"), link_database_SSO, "PAYMENT_INSTRUCTION_REPORT")

            If data_already_imported.Rows.Count > 0 Then
                DataTableToExcel(data_already_imported)
            End If

            Return "DONE"

        Catch ex As Exception
            WriteErrorLog(String.Format("Error: {0}", ex.ToString))
            Return ex.Message
        End Try
    End Function

    Function CUT_BANK_NAME_INITIAL(BANK_NAME As String) As String
        Dim RESULT As String = BANK_NAME
        For i As Integer = 0 To BANK_NAME.Length - 1
            If BANK_NAME.Substring(i, 1).ToString = "-" Then
                RESULT = BANK_NAME.Substring(i + 1, BANK_NAME.Length - 1 - i)
            End If
        Next
        Return RESULT
    End Function

    Private Sub Processing_rbtSelectAll_Click(sender As Object, e As EventArgs) Handles Processing_rbtSelectAll.Click
        Try
            If Processing_rbtSelectAll.Checked = True Then
                LayoutControlItem_Processing_cbByDate.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                LayoutControlItem_Processing_cbByStatus.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                LayoutControlItem_Processing_cbFilter_byUser.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                LayoutControlItem_Processing_tbSearchByText.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                LayoutControlItem_Processing_btSearch.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never

                Processing_GridControl_TaskList.DataSource = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT DISTINCT BATCH_NAME, USER_CREATED, DATE_CREATED, BATCH_STATUS FROM PAYMENT_INSTRUCTION_REPORT ORDER BY CAST(SUBSTR(DATE_CREATED, 7, 4) AS INT) DESC, CAST(SUBSTR(DATE_CREATED, 4, 2) AS INT) DESC, CAST(SUBSTR(DATE_CREATED, 1, 2) AS INT) DESC, BATCH_NAME DESC")
            End If
        Catch ex As Exception
            MsgBox(ex.Message, vbCritical + vbOKOnly)
            WriteErrorLog(ex.ToString)
        End Try
    End Sub

    Private Sub Processing_rbtSelectAll_CheckedChanged(sender As Object, e As EventArgs) Handles Processing_rbtSelectAll.CheckedChanged
        Try
            If Processing_rbtSelectAll.Checked = True Then
                LayoutControlItem_Processing_cbByDate.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                LayoutControlItem_Processing_cbByStatus.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                LayoutControlItem_Processing_cbFilter_byUser.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                LayoutControlItem_Processing_tbSearchByText.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                LayoutControlItem_Processing_btSearch.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never

                Processing_GridControl_TaskList.DataSource = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT DISTINCT BATCH_NAME, USER_CREATED, DATE_CREATED, BATCH_STATUS FROM PAYMENT_INSTRUCTION_REPORT ORDER BY CAST(SUBSTR(DATE_CREATED, 7, 4) AS INT) DESC, CAST(SUBSTR(DATE_CREATED, 4, 2) AS INT) DESC, CAST(SUBSTR(DATE_CREATED, 1, 2) AS INT) DESC, BATCH_NAME ASC")
            End If
        Catch ex As Exception
            MsgBox(ex.Message, vbCritical + vbOKOnly)
            WriteErrorLog(ex.ToString)
        End Try
    End Sub

    Private Sub Processing_rbtFilter_Click(sender As Object, e As EventArgs) Handles Processing_rbtFilter.Click
        Try

            If Processing_rbtFilter.Checked = True Then
                LayoutControlItem_Processing_cbByDate.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                LayoutControlItem_Processing_cbByStatus.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                LayoutControlItem_Processing_cbFilter_byUser.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                LayoutControlItem_Processing_tbSearchByText.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                LayoutControlItem_Processing_btSearch.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always

                Processing_cbFilter_byUser.Items.Clear()
                Processing_cbFilter_byUser.Items.Add("All")
                Processing_cbFilter_byUser.Text = "All"

                Dim dt As DataTable = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT DISTINCT USER_CREATED FROM PAYMENT_INSTRUCTION_REPORT")
                For Each Drr As DataRow In dt.Rows
                    If Len(Drr("USER_CREATED").ToString) > 0 Then
                        Processing_cbFilter_byUser.Items.Add(Drr("USER_CREATED").ToString)
                    End If
                Next
            End If

        Catch ex As Exception
            MsgBox(ex.Message, vbCritical + vbOKOnly)
            WriteErrorLog(ex.ToString)
        End Try
    End Sub

    Private Sub Processing_rbtFilter_CheckedChanged(sender As Object, e As EventArgs) Handles Processing_rbtFilter.CheckedChanged
        Try

            If Processing_rbtFilter.Checked = True Then
                LayoutControlItem_Processing_cbByDate.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                LayoutControlItem_Processing_cbByStatus.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                LayoutControlItem_Processing_cbFilter_byUser.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                LayoutControlItem_Processing_tbSearchByText.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                LayoutControlItem_Processing_btSearch.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always

                Processing_cbFilter_byUser.Items.Clear()
                Processing_cbFilter_byUser.Items.Add("All")
                Processing_cbFilter_byUser.Text = "All"

                Dim dt As DataTable = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT DISTINCT USER_CREATED FROM PAYMENT_INSTRUCTION_REPORT")
                For Each Drr As DataRow In dt.Rows
                    If Len(Drr("USER_CREATED").ToString) > 0 Then
                        Processing_cbFilter_byUser.Items.Add(Drr("USER_CREATED").ToString)
                    End If
                Next
            End If

        Catch ex As Exception
            MsgBox(ex.Message, vbCritical + vbOKOnly)
            WriteErrorLog(ex.ToString)
        End Try
    End Sub

    Private Sub Processing_rbtSearchByText_CheckedChanged(sender As Object, e As EventArgs) Handles Processing_rbtSearchByText.CheckedChanged
        Try
            If Processing_rbtSearchByText.Checked = True Then
                LayoutControlItem_Processing_cbByDate.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                LayoutControlItem_Processing_cbByStatus.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                LayoutControlItem_Processing_cbFilter_byUser.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                LayoutControlItem_Processing_tbSearchByText.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                LayoutControlItem_Processing_btSearch.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            End If
        Catch ex As Exception
            MsgBox(ex.Message, vbCritical + vbOKOnly)
            WriteErrorLog(ex.ToString)
        End Try
    End Sub

    Private Sub Processing_btExportToExcel_Click(sender As Object, e As EventArgs) Handles Processing_btExportToExcel.Click
        Try
            If Processing_GridView_Queue1.RowCount = 0 Then
                MsgBox("No data", vbCritical + vbOKOnly, "SSO")
                Exit Sub
            End If
            EXPORT_GRIDVIEW_TO_EXCEL(Processing_GridView_Queue1)
            MsgBox("Completed!", vbInformation)
        Catch ex As Exception
            MsgBox(ex.Message, vbCritical + vbOKOnly)
            WriteErrorLog(ex.ToString)
        End Try
    End Sub

    Private Sub Processing_TaskList_btShowBatch1_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles Processing_TaskList_btShowBatch1.ButtonClick
        Try
            Dim BATCH_NAME As String = Processing_GridView_TaskList.GetFocusedRowCellDisplayText("BATCH_NAME").ToString
            Processing_GridControl_Queue.DataSource = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT 'RELEASE' AS ACTION, * FROM PAYMENT_INSTRUCTION_REPORT WHERE BATCH_NAME = '" & BATCH_NAME & "'")
        Catch ex As Exception
            MsgBox(ex.Message, vbCritical + vbOKOnly)
            WriteErrorLog(ex.ToString)
        End Try
    End Sub

    Private Sub Processing_TaskList_btDeleteBatch_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles Processing_TaskList_btDeleteBatch.ButtonClick
        Try

            Dim BATCH_NAME As String = Processing_GridView_TaskList.GetFocusedRowCellDisplayText("BATCH_NAME").ToString

            Dim COUNT As Integer = SQL_QUERY_TO_INTEGER(CONNECTION_SSO, "SELECT COUNT(BATCH_PAYMENT_INSTRUCTION) FROM FIXED_DAILY_VSD_REPORT WHERE BATCH_PAYMENT_INSTRUCTION = '" & BATCH_NAME & "'")

            If COUNT > 0 Then
                MsgBox("Can't delete this batch" & Chr(10) & "This batch had been assigned to VSD report", vbCritical + vbOKOnly, "SSO")
                Exit Sub
            End If

            Dim iRet = MsgBox("Do you want to delete batch: " & BATCH_NAME & "?", vbYesNo + vbQuestion, "SSO")
            If iRet = vbYes Then
                SQL_QUERY(CONNECTION_SSO, "DELETE FROM PAYMENT_INSTRUCTION_REPORT WHERE BATCH_NAME = '" & BATCH_NAME & "'")
                SQL_QUERY(CONNECTION_SSO, "vacuum")
                Processing_GridView_TaskList.DeleteSelectedRows()
                Processing_GridControl_Queue.DataSource = Nothing

                MsgBox("Completed", vbInformation + vbOKOnly, "SSO")
            End If

        Catch ex As Exception
            MsgBox(ex.Message, vbCritical + vbOKOnly)
            WriteErrorLog(ex.ToString)
        End Try
    End Sub

    Private Sub Processing_btComplete_Click(sender As Object, e As EventArgs) Handles Processing_btComplete.Click
        Try
            SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
            Dim BATCH_NAME As String = Processing_GridView_Queue1.GetRowCellValue(0, "BATCH_NAME")
            Dim OutlookApp As New Outlook.Application
            Dim olMail As Outlook.MailItem
            olMail = OutlookApp.CreateItem(0)

            'CREATE TABLE FROM GRIDVIEW
            Dim dt_gridview As DataTable = GridView_To_Datatable(Processing_GridView_Queue1)
            Dim list_col As String = ""
            For col_num As Integer = 0 To dt_gridview.Columns.Count - 1
                If list_col.Length = 0 Then
                    list_col = dt_gridview.Columns(col_num).ColumnName.ToString() & " VARCHAR"
                Else
                    list_col = list_col & ", " & dt_gridview.Columns(col_num).ColumnName.ToString() & " VARCHAR"
                End If
            Next

            Dim user_created As String = Environment.UserName & "_" & Now.ToString("yyyyMMddhhmmss")
            Dim TABLE_ALL_LIST_CONFIRM As String = "ALL_LIST_CONFIRM_" & user_created

            SQL_QUERY(CONNECTION_SSO, "CREATE TABLE If Not EXISTS " & TABLE_ALL_LIST_CONFIRM & "(" & list_col & ")")
            SQLITE_BULK_COPY(dt_gridview, link_database_SSO, TABLE_ALL_LIST_CONFIRM)

            Dim DT_RELEASE As DataTable = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT CUSTOMER_ID, CUSTOMER_NAME, BATCH, SUB_BATCH, PMT_REF, PMT_STATUS, PMT_TYPE, DEBIT_AC_NO, DEBIT_AMOUNT, PAYMENT_CCY, PAYEE_AMOUNT, PROCESS_DATE, PAYEE_NAME, ADDRESS1, ADDRESS2, ADDRESS3, BEN_ACC, BEN_BANK_CODE, PAYMENT_DETAILS_1, PAYMENT_DETAILS_2, LOCAL_CHRG, BANK_NAME, TT_BENEFICIARY_BANK_DETAILS FROM PAYMENT_INSTRUCTION_REPORT WHERE CASE_ID IN (SELECT DISTINCT CASE_ID FROM ALL_LIST_CONFIRM WHERE ACTION = 'RELEASE') AND BATCH_NAME = '" & BATCH_NAME & "'")
            Dim DT_HOLD As DataTable = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT CUSTOMER_ID, CUSTOMER_NAME, BATCH, SUB_BATCH, PMT_REF, PMT_STATUS, PMT_TYPE, DEBIT_AC_NO, DEBIT_AMOUNT, PAYMENT_CCY, PAYEE_AMOUNT, PROCESS_DATE, PAYEE_NAME, ADDRESS1, ADDRESS2, ADDRESS3, BEN_ACC, BEN_BANK_CODE, PAYMENT_DETAILS_1, PAYMENT_DETAILS_2, LOCAL_CHRG, BANK_NAME, TT_BENEFICIARY_BANK_DETAILS FROM PAYMENT_INSTRUCTION_REPORT WHERE CASE_ID IN (SELECT DISTINCT CASE_ID FROM ALL_LIST_CONFIRM WHERE ACTION = 'HOLD') AND BATCH_NAME = '" & BATCH_NAME & "'")
            Dim DT_CANCEL As DataTable = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT CUSTOMER_ID, CUSTOMER_NAME, BATCH, SUB_BATCH, PMT_REF, PMT_STATUS, PMT_TYPE, DEBIT_AC_NO, DEBIT_AMOUNT, PAYMENT_CCY, PAYEE_AMOUNT, PROCESS_DATE, PAYEE_NAME, ADDRESS1, ADDRESS2, ADDRESS3, BEN_ACC, BEN_BANK_CODE, PAYMENT_DETAILS_1, PAYMENT_DETAILS_2, LOCAL_CHRG, BANK_NAME, TT_BENEFICIARY_BANK_DETAILS FROM PAYMENT_INSTRUCTION_REPORT WHERE CASE_ID IN (SELECT DISTINCT CASE_ID FROM ALL_LIST_CONFIRM WHERE ACTION = 'CANCEL') AND BATCH_NAME = '" & BATCH_NAME & "'")

            'UPDATE BATCH TO DATABASE VSD
            Dim TABLE_LIST_UPDATE_VSD As String = "LIST_RELEASE_" & user_created
            SQL_QUERY(CONNECTION_SSO, "CREATE TABLE IF NOT EXISTS " & TABLE_LIST_UPDATE_VSD & "(CASE_ID VARCHAR, CASE_STATUS VARCHAR, REF_NO VARCHAR, VSD_REF_NO VARCHAR, ACTION VARCHAR, COUNT_VSD NUMERIC)")
            Dim SQL_STRING As String = "SELECT DISTINCT A.CASE_ID, A.CASE_STATUS, A.REF_NO, A.VSD_REF_NO, A.ACTION, CASE WHEN B.COUNT_VSD IS NULL THEN 0 ELSE B.COUNT_VSD END AS COUNT_VSD " &
                                            "FROM (SELECT * FROM ALL_LIST_CONFIRM) A " &
                                            "LEFT JOIN (SELECT REF_NO, COUNT(REF_NO) AS COUNT_VSD FROM FIXED_DAILY_VSD_REPORT WHERE CASE_STATUS NOT IN ('Completed', 'CANCEL') GROUP BY REF_NO) B ON A.VSD_REF_NO = B.REF_NO"
            SQLITE_BULK_COPY(SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, SQL_STRING), link_database_SSO, TABLE_LIST_UPDATE_VSD)

            If DT_RELEASE.Rows.Count > 0 Then
                'COUNT_VSD = 1
                SQL_QUERY(CONNECTION_SSO, "UPDATE FIXED_DAILY_VSD_REPORT SET BATCH_PAYMENT_INSTRUCTION = '" & BATCH_NAME & "', CASE_STATUS = 'Completed', USER_MODIFIED = '" & user_created & "' WHERE CASE_STATUS NOT IN ('Completed', 'CANCEL') AND REF_NO IN (SELECT DISTINCT VSD_REF_NO FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'RELEASE' AND COUNT_VSD = 1)")
                SQL_QUERY(CONNECTION_SSO, "UPDATE PAYMENT_INSTRUCTION_REPORT SET CASE_STATUS = 'Completed', USER_MODIFIED = '" & user_created & "' WHERE CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'RELEASE' AND COUNT_VSD = 1)")
                SQL_QUERY(CONNECTION_SSO, "UPDATE INTERNAL_TRANSFER SET PAYMENT_STATUS = 'Completed' WHERE PAYMENT_CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'RELEASE' AND COUNT_VSD = 1)")

                'COUNT_VSD > 1
                SQL_QUERY(CONNECTION_SSO, "UPDATE PAYMENT_INSTRUCTION_REPORT SET USER_NOTE = 'Please manual update in VSD report', CASE_STATUS = 'Completed', USER_MODIFIED = '" & user_created & "' WHERE CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'RELEASE' AND COUNT_VSD > 1)")
                SQL_QUERY(CONNECTION_SSO, "UPDATE INTERNAL_TRANSFER SET PAYMENT_STATUS = 'Completed' WHERE PAYMENT_CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'RELEASE' AND COUNT_VSD > 1)")

                'COUNT_VSD = 0 AND VSD_REF_NO IS NULL
                SQL_QUERY(CONNECTION_SSO, "UPDATE PAYMENT_INSTRUCTION_REPORT SET USER_NOTE = 'Not found in VSD report', CASE_STATUS = 'Completed', USER_MODIFIED = '" & user_created & "' WHERE CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'RELEASE' AND COUNT_VSD = 0 AND VSD_REF_NO IS NULL)")
                SQL_QUERY(CONNECTION_SSO, "UPDATE INTERNAL_TRANSFER SET PAYMENT_STATUS = 'Completed' WHERE PAYMENT_CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'RELEASE' AND COUNT_VSD = 0 AND VSD_REF_NO IS NULL)")
            End If

            If DT_HOLD.Rows.Count > 0 Then
                'COUNT_VSD = 1
                SQL_QUERY(CONNECTION_SSO, "UPDATE FIXED_DAILY_VSD_REPORT SET BATCH_PAYMENT_INSTRUCTION = '" & BATCH_NAME & "', CASE_STATUS = 'HOLD', USER_MODIFIED = '" & user_created & "' WHERE CASE_STATUS NOT IN ('Completed', 'CANCEL') AND REF_NO IN (SELECT DISTINCT VSD_REF_NO FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'HOLD' AND COUNT_VSD = 1)")
                SQL_QUERY(CONNECTION_SSO, "UPDATE PAYMENT_INSTRUCTION_REPORT SET CASE_STATUS = 'HOLD', USER_MODIFIED = '" & user_created & "' WHERE CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'HOLD' AND COUNT_VSD = 1)")
                SQL_QUERY(CONNECTION_SSO, "UPDATE INTERNAL_TRANSFER SET PAYMENT_STATUS = 'HOLD' WHERE PAYMENT_CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'HOLD' AND COUNT_VSD = 1)")

                'COUNT_VSD > 1
                SQL_QUERY(CONNECTION_SSO, "UPDATE PAYMENT_INSTRUCTION_REPORT SET USER_NOTE = 'Please manual update in VSD report', CASE_STATUS = 'HOLD', USER_MODIFIED = '" & user_created & "' WHERE CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'HOLD' AND COUNT_VSD > 1)")
                SQL_QUERY(CONNECTION_SSO, "UPDATE INTERNAL_TRANSFER SET PAYMENT_STATUS = 'HOLD' WHERE PAYMENT_CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'HOLD' AND COUNT_VSD > 1)")

                'COUNT_VSD = 0 AND VSD_REF_NO IS NULL
                SQL_QUERY(CONNECTION_SSO, "UPDATE PAYMENT_INSTRUCTION_REPORT SET USER_NOTE = 'Not found in VSD report', CASE_STATUS = 'HOLD', USER_MODIFIED = '" & user_created & "' WHERE CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'HOLD' AND COUNT_VSD = 0 AND VSD_REF_NO IS NULL)")
                SQL_QUERY(CONNECTION_SSO, "UPDATE INTERNAL_TRANSFER SET PAYMENT_STATUS = 'HOLD' WHERE PAYMENT_CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'HOLD' AND COUNT_VSD = 0 AND VSD_REF_NO IS NULL)")
            End If

            If DT_CANCEL.Rows.Count > 0 Then
                'COUNT_VSD = 1
                SQL_QUERY(CONNECTION_SSO, "UPDATE FIXED_DAILY_VSD_REPORT SET BATCH_PAYMENT_INSTRUCTION = '" & BATCH_NAME & "', CASE_STATUS = 'CANCEL', USER_MODIFIED = '" & user_created & "' WHERE CASE_STATUS NOT IN ('Completed', 'CANCEL') AND REF_NO IN (SELECT DISTINCT VSD_REF_NO FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'CANCEL' AND COUNT_VSD = 1)")
                SQL_QUERY(CONNECTION_SSO, "UPDATE PAYMENT_INSTRUCTION_REPORT SET CASE_STATUS = 'CANCEL', USER_MODIFIED = '" & user_created & "' WHERE CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'CANCEL' AND COUNT_VSD = 1)")
                SQL_QUERY(CONNECTION_SSO, "UPDATE INTERNAL_TRANSFER SET PAYMENT_STATUS = 'CANCEL' WHERE PAYMENT_CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'CANCEL' AND COUNT_VSD = 1)")

                'COUNT_VSD > 1
                SQL_QUERY(CONNECTION_SSO, "UPDATE PAYMENT_INSTRUCTION_REPORT SET USER_NOTE = 'Please manual update in VSD report', CASE_STATUS = 'CANCEL', USER_MODIFIED = '" & user_created & "' WHERE CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'CANCEL' AND COUNT_VSD > 1)")
                SQL_QUERY(CONNECTION_SSO, "UPDATE INTERNAL_TRANSFER SET PAYMENT_STATUS = 'CANCEL' WHERE PAYMENT_CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'CANCEL' AND COUNT_VSD > 1)")

                'COUNT_VSD = 0 AND VSD_REF_NO IS NULL
                SQL_QUERY(CONNECTION_SSO, "UPDATE PAYMENT_INSTRUCTION_REPORT SET USER_NOTE = 'Not found in VSD report', CASE_STATUS = 'CANCEL', USER_MODIFIED = '" & user_created & "' WHERE CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'CANCEL' AND COUNT_VSD = 0 AND VSD_REF_NO IS NULL)")
                SQL_QUERY(CONNECTION_SSO, "UPDATE INTERNAL_TRANSFER SET PAYMENT_STATUS = 'CANCEL' WHERE PAYMENT_CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'CANCEL' AND COUNT_VSD = 0 AND VSD_REF_NO IS NULL)")
            End If

            Dim DT_LIST_CASE_NOT_YET_MAPPING As DataTable = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT * FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND COUNT_VSD = 0 AND VSD_REF_NO IS NOT NULL")
            If DT_LIST_CASE_NOT_YET_MAPPING.Rows.Count > 0 Then
                For Each DRR As DataRow In DT_LIST_CASE_NOT_YET_MAPPING.Rows
                    Dim CASE_ID As String = DRR("CASE_ID").ToString
                    Dim USER_NOTE As String = ""
                    Dim LIST_VSD_REF_NO As List(Of String) = SPLIT_TEXT_TO_LIST_BY_DELIMITOR(DRR("VSD_REF_NO").ToString, ";")

                    For j As Integer = 0 To LIST_VSD_REF_NO.Count - 1
                        Dim VSD_REF_NO As String = LIST_VSD_REF_NO.Item(j)
                        Dim count_ref_no As Integer = SQL_QUERY_TO_INTEGER(CONNECTION_SSO, "SELECT COUNT(REF_NO) FROM FIXED_DAILY_VSD_REPORT WHERE CASE_STATUS NOT IN ('Completed', 'CANCEL') AND REF_NO = '" & VSD_REF_NO & "'")
                        If count_ref_no > 1 Then
                            If USER_NOTE.Length = 0 Then
                                USER_NOTE = "Please manual update in VSD report"
                            Else
                                USER_NOTE = USER_NOTE & "; " & "Please manual update in VSD report"
                            End If
                        Else
                            If count_ref_no = 0 Then
                                If USER_NOTE.Length = 0 Then
                                    USER_NOTE = "Not found in VSD report"
                                Else
                                    USER_NOTE = USER_NOTE & "; " & "Not found in VSD report"
                                End If
                            Else
                                SQL_QUERY(CONNECTION_SSO, "UPDATE FIXED_DAILY_VSD_REPORT SET BATCH_PAYMENT_INSTRUCTION = '" & BATCH_NAME & "', CASE_STATUS = 'Completed', USER_MODIFIED = '" & UCase(Environment.UserName) & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss") & "' WHERE CASE_STATUS <> 'Completed' AND REF_NO = '" & VSD_REF_NO & "'")
                            End If
                        End If
                    Next
                    SQL_QUERY(CONNECTION_SSO, "UPDATE INTERNAL_TRANSFER SET PAYMENT_STATUS = 'Completed' WHERE PAYMENT_CASE_ID = '" & CASE_ID & "'")
                    SQL_QUERY(CONNECTION_SSO, "UPDATE PAYMENT_INSTRUCTION_REPORT SET USER_NOTE = '" & USER_NOTE & "', CASE_STATUS = 'Completed', USER_MODIFIED = '" & UCase(Environment.UserName) & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss") & "' WHERE CASE_ID = '" & CASE_ID & "'")
                Next
            End If

            Processing_GridControl_Queue.DataSource = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT * FROM PAYMENT_INSTRUCTION_REPORT WHERE BATCH_NAME = '" & BATCH_NAME & "'")
            GridControl_DataMaintenance.DataSource = Nothing
            GridView_DataMaintenance.Columns.Clear()

            If SQL_FROMFILE_TO_STRING(global_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Delete_Temp_Send_Email'") = "YES" Then
                SQL_QUERY(CONNECTION_SSO, "DROP TABLE " & TABLE_LIST_UPDATE_VSD)
                SQL_QUERY(CONNECTION_SSO, "DROP TABLE " & TABLE_ALL_LIST_CONFIRM)
                If SQL_FROMFILE_TO_STRING(global_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Refresh_Database_after_delete'") = "YES" Then
                    SQL_QUERY(CONNECTION_SSO, "vacuum")
                End If
            End If

            SplashScreenManager.CloseForm(False)
        Catch ex As Exception
            WriteErrorLog(String.Format("Error: {0}", ex.ToString))
            SplashScreenManager.CloseForm(False)
            MsgBox(String.Format("Error: {0}", ex.Message), vbCritical + vbOKOnly, "SSO")
        End Try
    End Sub

    Private Sub Processing_btManualMapping_Click(sender As Object, e As EventArgs) Handles Processing_btManualMapping.Click
        Try
            Dim TimerStart As DateTime = Now
            SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
            WaitForm1.Width = 500

            If Processing_GridView_Queue1.RowCount = 0 Then
                Exit Sub
            End If

            Dim BATCH_NAME As String = Processing_GridView_Queue1.GetRowCellValue(0, "BATCH_NAME")
            Dim DT_BATCH As DataTable = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT CASE_ID, REF_NO, CUSTOMER_ID, CUSTOMER_NAME, BATCH, SUB_BATCH, PMT_REF, PMT_STATUS, PMT_TYPE, DEBIT_AC_NO, DEBIT_AMOUNT, PAYMENT_CCY, PAYEE_AMOUNT, PROCESS_DATE, PAYEE_NAME, ADDRESS1, ADDRESS2, ADDRESS3, BEN_ACC, BEN_BANK_CODE, PAYMENT_DETAILS_1, PAYMENT_DETAILS_2, LOCAL_CHRG, BANK_NAME, TT_BENEFICIARY_BANK_DETAILS, USER_NOTE FROM PAYMENT_INSTRUCTION_REPORT WHERE BATCH_NAME = '" & BATCH_NAME & "'")

            If (Not System.IO.Directory.Exists(link_folder_database & "TEMP")) Then
                System.IO.Directory.CreateDirectory(link_folder_database & "TEMP")
            End If

            Dim LINK_TEMP_DATABASE = link_folder_database & "TEMP\TEMP_MANUAL_MAPPING_" & BATCH_NAME & "_" & Now.ToString("ddMMyyyyhhmmss") & ".txt"

            ' Create the SQLite database
            If Not My.Computer.FileSystem.FileExists(LINK_TEMP_DATABASE) Then
                SQLiteConnection.CreateFile(LINK_TEMP_DATABASE)
            End If

            SQL_QUERY_FROM_FILE(LINK_TEMP_DATABASE, "CREATE TABLE IF NOT EXISTS LIST_CASH_ACCOUNT(CASE_ID VARCHAR, SYMBOL VARCHAR, CASH_ACCOUNT VARCHAR, VALUE_DATE_ADD_DAYS NUMERIC, DEADLINE_ADD_DAYS NUMERIC, BEN_CHARGE_FOR_LOW_VALUE NUMERIC, BEN_CHARGE_FOR_HIGH_VALUE NUMERIC)")
            SQL_QUERY_FROM_FILE(LINK_TEMP_DATABASE, "CREATE TABLE IF NOT EXISTS FIXED_DAILY_VSD_REPORT(CASE_ID VARCHAR NOT NULL UNIQUE PRIMARY KEY, STR_DATE VARCHAR, REF_NO VARCHAR, FULLNAME VARCHAR, MBNAME VARCHAR, ACTYPE VARCHAR, DBCODE VARCHAR, ORDERQTTY VARCHAR, MATCHQTTY VARCHAR, MATCHAMT VARCHAR, FEEAMC VARCHAR, FEEDXX VARCHAR, FEEFUND VARCHAR, TOTALFEE VARCHAR, TAXAMT VARCHAR, AMT NUMERIC, CUSTODYCD VARCHAR, BANKACNAME VARCHAR, CITYBANK VARCHAR, BANKACC VARCHAR, NOTE VARCHAR, TRADINGID VARCHAR, NAV VARCHAR, TOTALNAV VARCHAR, TXDATE VARCHAR, SYMBOL VARCHAR, NAME VARCHAR, FEENAME VARCHAR, EXECNAME VARCHAR, FEEID VARCHAR, SRTYPE VARCHAR, FILE_PATH VARCHAR, CASH_ACCOUNT VARCHAR, AMOUNT_PAYMENT NUMERIC, TRADE_DATE DATE, VALUE_DATE DATE, DEADLINE_PER_FUND DATE, FULLNAME_UNSIGN VARCHAR, BANKACNAME_UNSIGN VARCHAR, CITYBANK_UNSIGN VARCHAR, USER_CREATED VARCHAR, CASE_STATUS VARCHAR, USER_MODIFIED VARCHAR, BATCH_PAYMENT_INSTRUCTION VARCHAR, USER_NOTE VARCHAR)")
            SQL_QUERY_FROM_FILE(LINK_TEMP_DATABASE, "CREATE TABLE IF NOT EXISTS PAYMENT_INSTRUCTION_REPORT(CASE_ID VARCHAR, REF_NO VARCHAR, VSD_REF_NO VARCHAR, CUSTOMER_ID VARCHAR, CUSTOMER_NAME VARCHAR, BATCH VARCHAR, SUB_BATCH VARCHAR, PMT_REF VARCHAR, PMT_STATUS VARCHAR, PMT_TYPE VARCHAR, DEBIT_AC_NO VARCHAR, DEBIT_AMOUNT CURRENCY, PAYMENT_CCY VARCHAR, PAYEE_AMOUNT CURRENCY, PROCESS_DATE DATETIME, PAYEE_NAME VARCHAR, ADDRESS1 VARCHAR, ADDRESS2 VARCHAR, ADDRESS3 VARCHAR, BEN_ACC VARCHAR, BEN_BANK_CODE VARCHAR, PAYMENT_DETAILS_1 VARCHAR, PAYMENT_DETAILS_2 VARCHAR, LOCAL_CHRG VARCHAR, BANK_NAME VARCHAR, TT_BENEFICIARY_BANK_DETAILS VARCHAR, USER_CREATED VARCHAR, DATE_CREATED VARCHAR, BATCH_NAME VARCHAR, BATCH_STATUS VARCHAR, VSD_FULLNAME VARCHAR, VSD_BANKACNAME_CITYBANK VARCHAR, VSD_CASH_ACCOUNT VARCHAR, VSD_VALUE_DATE DATETIME, VSD_AMOUNT_PAYMENT CURRENCY, CHECK_NAME BOOLEAN, CHECK_BANK_NAME BOOLEAN, CHECK_CASH_ACC BOOLEAN, CHECK_VALUE_DATE BOOLEAN, CHECK_AMOUNT BOOLEAN, CHECK_BEN_CHARGE BOOLEAN, CHECK_BEN_CHARGE_FEE BOOLEAN, FINAL_CHECK BOOLEAN, CASE_STATUS VARCHAR, USER_NOTE VARCHAR)")
            SQL_QUERY_FROM_FILE(LINK_TEMP_DATABASE, "CREATE TABLE IF NOT EXISTS PAYMENT_INSTRUCTION_REPORT2(CASE_ID VARCHAR, REF_NO VARCHAR, VSD_REF_NO VARCHAR, CUSTOMER_ID VARCHAR, CUSTOMER_NAME VARCHAR, BATCH VARCHAR, SUB_BATCH VARCHAR, PMT_REF VARCHAR, PMT_STATUS VARCHAR, PMT_TYPE VARCHAR, DEBIT_AC_NO VARCHAR, DEBIT_AMOUNT CURRENCY, PAYMENT_CCY VARCHAR, PAYEE_AMOUNT CURRENCY, PROCESS_DATE DATETIME, PAYEE_NAME VARCHAR, ADDRESS1 VARCHAR, ADDRESS2 VARCHAR, ADDRESS3 VARCHAR, BEN_ACC VARCHAR, BEN_BANK_CODE VARCHAR, PAYMENT_DETAILS_1 VARCHAR, PAYMENT_DETAILS_2 VARCHAR, LOCAL_CHRG VARCHAR, BANK_NAME VARCHAR, TT_BENEFICIARY_BANK_DETAILS VARCHAR, USER_CREATED VARCHAR, DATE_CREATED VARCHAR, BATCH_NAME VARCHAR, BATCH_STATUS VARCHAR, VSD_FULLNAME VARCHAR, VSD_BANKACNAME_CITYBANK VARCHAR, VSD_CASH_ACCOUNT VARCHAR, VSD_VALUE_DATE DATETIME, VSD_AMOUNT_PAYMENT CURRENCY, CHECK_NAME BOOLEAN, CHECK_BANK_NAME BOOLEAN, CHECK_CASH_ACC BOOLEAN, CHECK_VALUE_DATE BOOLEAN, CHECK_AMOUNT BOOLEAN, CHECK_BEN_CHARGE BOOLEAN, CHECK_BEN_CHARGE_FEE BOOLEAN, FINAL_CHECK BOOLEAN, CASE_STATUS VARCHAR, USER_NOTE VARCHAR)")
            SQL_QUERY_FROM_FILE(LINK_TEMP_DATABASE, "CREATE TABLE IF NOT EXISTS PAYMENT_INSTRUCTION_REPORT3(BEN_CHARGE_FOR_LOW_VALUE NUMERIC, BEN_CHARGE_FOR_HIGH_VALUE NUMERIC, CASE_ID VARCHAR, REF_NO VARCHAR, VSD_REF_NO VARCHAR, CUSTOMER_ID VARCHAR, CUSTOMER_NAME VARCHAR, BATCH VARCHAR, SUB_BATCH VARCHAR, PMT_REF VARCHAR, PMT_STATUS VARCHAR, PMT_TYPE VARCHAR, DEBIT_AC_NO VARCHAR, DEBIT_AMOUNT CURRENCY, PAYMENT_CCY VARCHAR, PAYEE_AMOUNT CURRENCY, PROCESS_DATE DATETIME, PAYEE_NAME VARCHAR, ADDRESS1 VARCHAR, ADDRESS2 VARCHAR, ADDRESS3 VARCHAR, BEN_ACC VARCHAR, BEN_BANK_CODE VARCHAR, PAYMENT_DETAILS_1 VARCHAR, PAYMENT_DETAILS_2 VARCHAR, LOCAL_CHRG VARCHAR, BANK_NAME VARCHAR, TT_BENEFICIARY_BANK_DETAILS VARCHAR, USER_CREATED VARCHAR, DATE_CREATED VARCHAR, BATCH_NAME VARCHAR, BATCH_STATUS VARCHAR, VSD_FULLNAME VARCHAR, VSD_BANKACNAME_CITYBANK VARCHAR, VSD_CASH_ACCOUNT VARCHAR, VSD_VALUE_DATE DATETIME, VSD_AMOUNT_PAYMENT CURRENCY, CHECK_NAME BOOLEAN, CHECK_BANK_NAME BOOLEAN, CHECK_CASH_ACC BOOLEAN, CHECK_VALUE_DATE BOOLEAN, CHECK_AMOUNT BOOLEAN, CHECK_BEN_CHARGE BOOLEAN, CHECK_BEN_CHARGE_FEE BOOLEAN, FINAL_CHECK BOOLEAN, CASE_STATUS VARCHAR, USER_NOTE VARCHAR)")

            SQLITE_BULK_COPY(DT_BATCH, LINK_TEMP_DATABASE, "PAYMENT_INSTRUCTION_REPORT")
            SQLITE_BULK_COPY(SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT * FROM FIXED_DAILY_VSD_REPORT WHERE (CASE_STATUS <> 'Completed' AND CASE_STATUS <> 'CANCEL')"), LINK_TEMP_DATABASE, "FIXED_DAILY_VSD_REPORT")
            SQLITE_BULK_COPY(SQL_QUERY_TO_DATATABLE(CONNECTION_CONFIG, "SELECT * FROM LIST_CASH_ACCOUNT"), LINK_TEMP_DATABASE, "LIST_CASH_ACCOUNT")

            Dim SQL_STRING As String = "SELECT DISTINCT A.CASE_ID, A.REF_NO, B.REF_NO AS VSD_REF_NO, A.CUSTOMER_ID, A.CUSTOMER_NAME, A.BATCH, A.SUB_BATCH, A.PMT_REF, A.PMT_STATUS, A.PMT_TYPE, A.DEBIT_AC_NO, A.DEBIT_AMOUNT, A.PAYMENT_CCY, A.PAYEE_AMOUNT, A.PROCESS_DATE, A.PAYEE_NAME, A.ADDRESS1, A.ADDRESS2, A.ADDRESS3, A.BEN_ACC, A.BEN_BANK_CODE, A.PAYMENT_DETAILS_1, A.PAYMENT_DETAILS_2, A.LOCAL_CHRG, A.BANK_NAME, A.TT_BENEFICIARY_BANK_DETAILS, A.USER_CREATED, A.DATE_CREATED, A.BATCH_NAME, A.BATCH_STATUS, B.FULLNAME_UNSIGN AS VSD_FULLNAME, CASE WHEN B.CITYBANK_UNSIGN IS NULL THEN B.BANKACNAME_UNSIGN ELSE B.BANKACNAME_UNSIGN||','||B.CITYBANK_UNSIGN END AS VSD_BANKACNAME_CITYBANK, B.CASH_ACCOUNT AS VSD_CASH_ACCOUNT, B.VALUE_DATE AS VSD_VALUE_DATE, B.AMOUNT_PAYMENT AS VSD_AMOUNT_PAYMENT, CASE UPPER(B.FULLNAME_UNSIGN) WHEN UPPER(A.PAYEE_NAME) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||UPPER(A.ADDRESS1) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||' '||UPPER(A.ADDRESS1) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||UPPER(A.ADDRESS1)||UPPER(A.ADDRESS2) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||UPPER(A.ADDRESS1)||' '||UPPER(A.ADDRESS2) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||' '||UPPER(A.ADDRESS1)||UPPER(A.ADDRESS2) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||' '||UPPER(A.ADDRESS1)||' '||UPPER(A.ADDRESS2) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||UPPER(A.ADDRESS1)||UPPER(A.ADDRESS2)||UPPER(A.ADDRESS3) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||UPPER(A.ADDRESS1)||UPPER(A.ADDRESS2)||' '||UPPER(A.ADDRESS3) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||UPPER(A.ADDRESS1)||' '||UPPER(A.ADDRESS2)||UPPER(A.ADDRESS3) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||UPPER(A.ADDRESS1)||' '||UPPER(A.ADDRESS2)||' '||UPPER(A.ADDRESS3) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||' '||UPPER(A.ADDRESS1)||UPPER(A.ADDRESS2)||UPPER(A.ADDRESS3) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||' '||UPPER(A.ADDRESS1)||UPPER(A.ADDRESS2)||' '||UPPER(A.ADDRESS3) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||' '||UPPER(A.ADDRESS1)||' '||UPPER(A.ADDRESS2)||UPPER(A.ADDRESS3) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||' '||UPPER(A.ADDRESS1)||' '||UPPER(A.ADDRESS2)||UPPER(A.ADDRESS3) THEN TRUE WHEN UPPER(A.PAYEE_NAME)||' '||UPPER(A.ADDRESS1)||' '||UPPER(A.ADDRESS2)||' '||UPPER(A.ADDRESS3) THEN TRUE ELSE FALSE END AS CHECK_NAME, A.CHECK_BANK_NAME, CASE WHEN B.CASH_ACCOUNT = A.DEBIT_AC_NO THEN TRUE ELSE FALSE END AS CHECK_CASH_ACC, CASE WHEN A.PROCESS_DATE = B.VALUE_DATE THEN TRUE ELSE FALSE END AS CHECK_VALUE_DATE, CASE WHEN B.AMOUNT_PAYMENT = A.DEBIT_AMOUNT THEN TRUE ELSE FALSE END AS CHECK_AMOUNT, CASE WHEN A.PMT_TYPE = 'BT' THEN TRUE ELSE CASE WHEN A.DEBIT_AMOUNT > A.PAYEE_AMOUNT THEN CASE WHEN A.DEBIT_AC_NO <> A.BEN_ACC THEN TRUE ELSE FALSE END ELSE FALSE END END AS CHECK_BEN_CHARGE, FALSE AS CHECK_BEN_CHARGE_FEE, FINAL_CHECK, A.CASE_STATUS FROM PAYMENT_INSTRUCTION_REPORT A" &
                                        " INNER JOIN FIXED_DAILY_VSD_REPORT B ON A.REF_NO = B.REF_NO" &
                                        " UNION ALL" &
                                        " SELECT DISTINCT A.CASE_ID, A.REF_NO, '' AS VSD_REF_NO, A.CUSTOMER_ID, A.CUSTOMER_NAME, A.BATCH, A.SUB_BATCH, A.PMT_REF, A.PMT_STATUS, A.PMT_TYPE, A.DEBIT_AC_NO, A.DEBIT_AMOUNT, A.PAYMENT_CCY, A.PAYEE_AMOUNT, A.PROCESS_DATE, A.PAYEE_NAME, A.ADDRESS1, A.ADDRESS2, A.ADDRESS3, A.BEN_ACC, A.BEN_BANK_CODE, A.PAYMENT_DETAILS_1, A.PAYMENT_DETAILS_2, A.LOCAL_CHRG, A.BANK_NAME, A.TT_BENEFICIARY_BANK_DETAILS, A.USER_CREATED, A.DATE_CREATED, A.BATCH_NAME, A.BATCH_STATUS, VSD_FULLNAME, VSD_BANKACNAME_CITYBANK, VSD_CASH_ACCOUNT, VSD_VALUE_DATE, VSD_AMOUNT_PAYMENT, FALSE AS CHECK_NAME, FALSE AS CHECK_BANK_NAME, FALSE AS CHECK_CASH_ACC, FALSE AS CHECK_VALUE_DATE, FALSE AS CHECK_AMOUNT, CASE WHEN A.PMT_TYPE = 'BT' THEN TRUE ELSE CASE WHEN A.DEBIT_AMOUNT > A.PAYEE_AMOUNT THEN CASE WHEN A.DEBIT_AC_NO <> A.BEN_ACC THEN TRUE ELSE FALSE END ELSE FALSE END END AS CHECK_BEN_CHARGE, CASE WHEN A.PMT_TYPE = 'BT' THEN CASE WHEN A.DEBIT_AMOUNT = A.PAYEE_AMOUNT THEN TRUE ELSE FALSE END ELSE FALSE END AS CHECK_BEN_CHARGE_FEE, FINAL_CHECK, A.CASE_STATUS FROM PAYMENT_INSTRUCTION_REPORT A" &
                                        " LEFT OUTER JOIN FIXED_DAILY_VSD_REPORT B ON A.REF_NO = B.REF_NO WHERE B.REF_NO IS NULL"

            SQLITE_BULK_COPY(SQL_FROMFILE_TO_DATATABLE(LINK_TEMP_DATABASE, SQL_STRING), LINK_TEMP_DATABASE, "PAYMENT_INSTRUCTION_REPORT2")

            SQL_STRING = "SELECT DISTINCT B.BEN_CHARGE_FOR_LOW_VALUE, B.BEN_CHARGE_FOR_HIGH_VALUE, A.CASE_ID, A.REF_NO, A.VSD_REF_NO, A.CUSTOMER_ID, A.CUSTOMER_NAME, A.BATCH, A.SUB_BATCH, A.PMT_REF, A.PMT_STATUS, A.PMT_TYPE, A.DEBIT_AC_NO, A.DEBIT_AMOUNT, A.PAYMENT_CCY, A.PAYEE_AMOUNT, A.PROCESS_DATE, A.PAYEE_NAME, A.ADDRESS1, A.ADDRESS2, A.ADDRESS3, A.BEN_ACC, A.BEN_BANK_CODE, A.PAYMENT_DETAILS_1, A.PAYMENT_DETAILS_2, A.LOCAL_CHRG, A.BANK_NAME, A.TT_BENEFICIARY_BANK_DETAILS, A.USER_CREATED, A.DATE_CREATED, A.BATCH_NAME, A.BATCH_STATUS, A.VSD_FULLNAME, A.VSD_BANKACNAME_CITYBANK, A.VSD_CASH_ACCOUNT, A.VSD_VALUE_DATE, A.VSD_AMOUNT_PAYMENT, A.CHECK_NAME, CASE WHEN UPPER(SUBSTR(A.VSD_BANKACNAME_CITYBANK,5,LENGTH(VSD_BANKACNAME_CITYBANK)-4)) = UPPER(A.TT_BENEFICIARY_BANK_DETAILS) THEN TRUE ELSE CASE WHEN UPPER(A.TT_BENEFICIARY_BANK_DETAILS) = UPPER(A.VSD_BANKACNAME_CITYBANK) THEN TRUE ELSE FALSE END END AS CHECK_BANK_NAME, A.CHECK_CASH_ACC, A.CHECK_VALUE_DATE, A.CHECK_AMOUNT, A.CHECK_BEN_CHARGE, CASE WHEN A.DEBIT_AMOUNT < 500000000 THEN CASE WHEN B.BEN_CHARGE_FOR_LOW_VALUE = A.DEBIT_AMOUNT - A.PAYEE_AMOUNT THEN TRUE ELSE FALSE END ELSE CASE WHEN B.BEN_CHARGE_FOR_HIGH_VALUE = A.DEBIT_AMOUNT - A.PAYEE_AMOUNT THEN TRUE ELSE FALSE END END AS CHECK_BEN_CHARGE_FEE, A.FINAL_CHECK, A.CASE_STATUS, A.USER_NOTE FROM PAYMENT_INSTRUCTION_REPORT2 A" &
                            " INNER JOIN LIST_CASH_ACCOUNT B ON A.VSD_CASH_ACCOUNT = B.CASH_ACCOUNT" &
                            " UNION ALL" &
                            " SELECT DISTINCT BEN_CHARGE_FOR_LOW_VALUE, BEN_CHARGE_FOR_HIGH_VALUE, A.CASE_ID, A.REF_NO, A.VSD_REF_NO, A.CUSTOMER_ID, A.CUSTOMER_NAME, A.BATCH, A.SUB_BATCH, A.PMT_REF, A.PMT_STATUS, A.PMT_TYPE, A.DEBIT_AC_NO, A.DEBIT_AMOUNT, A.PAYMENT_CCY, A.PAYEE_AMOUNT, A.PROCESS_DATE, A.PAYEE_NAME, A.ADDRESS1, A.ADDRESS2, A.ADDRESS3, A.BEN_ACC, A.BEN_BANK_CODE, A.PAYMENT_DETAILS_1, A.PAYMENT_DETAILS_2, A.LOCAL_CHRG, A.BANK_NAME, A.TT_BENEFICIARY_BANK_DETAILS, A.USER_CREATED, A.DATE_CREATED, A.BATCH_NAME, A.BATCH_STATUS, A.VSD_FULLNAME, A.VSD_BANKACNAME_CITYBANK, A.VSD_CASH_ACCOUNT, A.VSD_VALUE_DATE, A.VSD_AMOUNT_PAYMENT, A.CHECK_NAME, FALSE AS CHECK_BANK_NAME, A.CHECK_CASH_ACC, A.CHECK_VALUE_DATE, A.CHECK_AMOUNT, A.CHECK_BEN_CHARGE, CASE WHEN A.PMT_TYPE = 'BT' THEN CASE WHEN A.DEBIT_AMOUNT = A.PAYEE_AMOUNT THEN TRUE ELSE FALSE END ELSE FALSE END AS CHECK_BEN_CHARGE_FEE, A.FINAL_CHECK, A.CASE_STATUS, A.USER_NOTE FROM PAYMENT_INSTRUCTION_REPORT2 A" &
                            " LEFT OUTER JOIN LIST_CASH_ACCOUNT B ON A.VSD_CASH_ACCOUNT = B.CASH_ACCOUNT WHERE B.CASH_ACCOUNT IS NULL"

            SQLITE_BULK_COPY(SQL_FROMFILE_TO_DATATABLE(LINK_TEMP_DATABASE, SQL_STRING), LINK_TEMP_DATABASE, "PAYMENT_INSTRUCTION_REPORT3")

            Dim DT As DataTable = SQL_FROMFILE_TO_DATATABLE(LINK_TEMP_DATABASE, "SELECT * FROM PAYMENT_INSTRUCTION_REPORT3 WHERE PMT_TYPE <> 'BT' AND (CHECK_BANK_NAME = FALSE OR CHECK_AMOUNT = FALSE)")

            Dim I As Integer = 0
            Dim TOTAL_ROWS As Integer = DT.Rows.Count
            For Each Drr As DataRow In DT.Rows
                I = I + 1
                CHANGE_DESCRIPTION_WAIT_FROM("[" & I & "/" & TOTAL_ROWS & "] - Additional Checking: " & Drr("PAYEE_NAME").ToString)

                Dim CHECK_BANK_NAME As Boolean = Drr("CHECK_BANK_NAME")
                Dim CHECK_AMOUNT As Boolean = Drr("CHECK_AMOUNT")

                If CHECK_BANK_NAME = False Then
                    Dim VSD_BANKACNAME_CITYBANK As String = CUT_BANK_NAME_INITIAL(Drr("VSD_BANKACNAME_CITYBANK").ToString)
                    If Drr("TT_BENEFICIARY_BANK_DETAILS").ToString = VSD_BANKACNAME_CITYBANK Then
                        CHECK_BANK_NAME = True
                    End If
                End If

                Dim VSD_REF_NO As String = ""
                If CHECK_AMOUNT = False Then
                    Dim TOTAL_AMT_VSD_REPORT As Long = SQL_QUERY_TO_LONG(CONNECTION_SSO, "SELECT SUM(AMOUNT_PAYMENT) FROM FIXED_DAILY_VSD_REPORT WHERE (CASE_STATUS <> 'Completed' AND CASE_STATUS <> 'CANCEL') AND CASH_ACCOUNT = '" & Drr("DEBIT_AC_NO").ToString & "' AND BANKACC = '" & GET_ONLY_NUMERIC_FROM_STRING(Drr("BEN_ACC").ToString) & "'")
                    If CLng(Drr("DEBIT_AMOUNT").ToString) = TOTAL_AMT_VSD_REPORT Then
                        CHECK_AMOUNT = True
                        VSD_REF_NO = SQL_QUERY_TO_STRING(CONNECTION_SSO, "SELECT GROUP_CONCAT(REF_NO,';') FROM FIXED_DAILY_VSD_REPORT WHERE (CASE_STATUS <> 'Completed' AND CASE_STATUS <> 'CANCEL') AND CASH_ACCOUNT = '" & Drr("DEBIT_AC_NO").ToString & "' AND BANKACC = '" & GET_ONLY_NUMERIC_FROM_STRING(Drr("BEN_ACC").ToString) & "'")
                    End If
                End If

                If CHECK_AMOUNT <> Drr("CHECK_AMOUNT") Or CHECK_BANK_NAME <> Drr("CHECK_BANK_NAME") Then
                    SQL_QUERY_FROM_FILE(LINK_TEMP_DATABASE, "UPDATE PAYMENT_INSTRUCTION_REPORT SET VSD_REF_NO = '" & VSD_REF_NO & "', CHECK_AMOUNT = '" & CHECK_AMOUNT & "', CHECK_BANK_NAME = '" & CHECK_BANK_NAME & "' WHERE CASE_ID = '" & Drr("CASE_ID").ToString & "'")
                End If
            Next

            DT = SQL_FROMFILE_TO_DATATABLE(LINK_TEMP_DATABASE, "SELECT * FROM PAYMENT_INSTRUCTION_REPORT3 WHERE PMT_TYPE = 'BT'")
            If DT.Rows.Count > 0 Then
                I = 0
                TOTAL_ROWS = DT.Rows.Count

                For Each DRR As DataRow In DT.Rows
                    I = I + 1
                    CHANGE_DESCRIPTION_WAIT_FROM("[" & I & "/" & TOTAL_ROWS & "] - CHECK INTERNAL TRANSFER: " & DRR("PAYEE_NAME").ToString)

                    Dim DT_CHECK_INTERNAL_PAYMENT As DataTable = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT * FROM INTERNAL_TRANSFER WHERE STATUS = 'Incomplete' AND CASH_ACCOUNT = '" & GET_ONLY_NUMERIC_FROM_STRING(DRR("BEN_ACC").ToString) & "'")
                    For Each Drr_CHECK As DataRow In DT_CHECK_INTERNAL_PAYMENT.Rows
                        Dim CASE_ID_CHECK_INTERNAL_PAYMENT As String = Drr_CHECK("CASE_ID").ToString
                        Dim PAYMENT_CASE_ID As String = Drr_CHECK("PAYMENT_CASE_ID").ToString
                        Dim TOTAL_AMOUNT As Long = CLng(Drr_CHECK("TOTAL_AMOUNT").ToString)
                        Dim PAYMENT_AMOUNT As Long
                        If Drr_CHECK("PAYMENT_AMOUNT").ToString = "" Then
                            PAYMENT_AMOUNT = 0
                        Else
                            PAYMENT_AMOUNT = CLng(Drr_CHECK("PAYMENT_AMOUNT").ToString)
                        End If
                        WriteLog_Full("PAYMENT_AMOUNT = " & PAYMENT_AMOUNT)
                        If TOTAL_AMOUNT <> PAYMENT_AMOUNT Then
                            If PAYMENT_CASE_ID.Length = 0 Then
                                If CLng(DRR("DEBIT_AMOUNT").ToString) > TOTAL_AMOUNT Or CLng(DRR("DEBIT_AMOUNT").ToString) = TOTAL_AMOUNT Then
                                    SQL_QUERY(CONNECTION_SSO, "UPDATE INTERNAL_TRANSFER SET PAYMENT_CASE_ID = '" & DRR("CASE_ID").ToString & "', PAYMENT_AMOUNT = '" & DRR("DEBIT_AMOUNT").ToString & "' WHERE CASE_ID = '" & CASE_ID_CHECK_INTERNAL_PAYMENT & "'")
                                End If
                            End If
                        End If
                    Next
                Next
            End If

            SQL_QUERY_FROM_FILE(LINK_TEMP_DATABASE, "DELETE FROM PAYMENT_INSTRUCTION_REPORT")
            SQLITE_BULK_COPY(SQL_FROMFILE_TO_DATATABLE(LINK_TEMP_DATABASE, "SELECT CASE_ID, REF_NO, VSD_REF_NO, CUSTOMER_ID, CUSTOMER_NAME, BATCH, SUB_BATCH, PMT_REF, PMT_STATUS, PMT_TYPE, DEBIT_AC_NO, DEBIT_AMOUNT, PAYMENT_CCY, PAYEE_AMOUNT, PROCESS_DATE, PAYEE_NAME, ADDRESS1, ADDRESS2, ADDRESS3, BEN_ACC, BEN_BANK_CODE, PAYMENT_DETAILS_1, PAYMENT_DETAILS_2, LOCAL_CHRG, BANK_NAME, TT_BENEFICIARY_BANK_DETAILS, USER_CREATED, DATE_CREATED, BATCH_NAME, BATCH_STATUS, VSD_FULLNAME, VSD_BANKACNAME_CITYBANK, VSD_CASH_ACCOUNT, VSD_VALUE_DATE, VSD_AMOUNT_PAYMENT, CHECK_NAME, CHECK_BANK_NAME, CHECK_CASH_ACC, CHECK_VALUE_DATE, CHECK_AMOUNT, CHECK_BEN_CHARGE, CHECK_BEN_CHARGE_FEE, CASE WHEN CHECK_NAME != 1 AND CHECK_BANK_NAME IS TRUE AND CHECK_CASH_ACC != 1 AND CHECK_VALUE_DATE != 1 AND CHECK_VALUE_DATE != 1 AND CHECK_AMOUNT != 1 AND CHECK_BEN_CHARGE != 1 AND CHECK_BEN_CHARGE_FEE IS TRUE THEN TRUE ELSE FALSE END AS FINAL_CHECK, CASE_STATUS, USER_NOTE FROM PAYMENT_INSTRUCTION_REPORT3"), LINK_TEMP_DATABASE, "PAYMENT_INSTRUCTION_REPORT")

            SQL_QUERY(CONNECTION_SSO, "DELETE FROM PAYMENT_INSTRUCTION_REPORT WHERE BATCH_NAME = '" & BATCH_NAME & "'")
            SQLITE_BULK_COPY(SQL_FROMFILE_TO_DATATABLE(LINK_TEMP_DATABASE, "SELECT DISTINCT * FROM PAYMENT_INSTRUCTION_REPORT"), link_database_SSO, "PAYMENT_INSTRUCTION_REPORT")

            Processing_GridControl_Queue.DataSource = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT * FROM PAYMENT_INSTRUCTION_REPORT WHERE BATCH_NAME = '" & BATCH_NAME & "'")
            Processing_GridView_Queue1.BestFitColumns()
            SplashScreenManager.CloseForm(False)
            Dim TimeSpent As System.TimeSpan = Now.Subtract(TimerStart)
            MsgBox("Completed!" & Chr(10) & "in " & Format(TimeSpent.TotalSeconds, "0.00") & " seconds", vbInformation + vbOKOnly, "SSO")

        Catch ex As Exception
            WriteErrorLog(String.Format("Error: {0}", ex.ToString))
            SplashScreenManager.CloseForm(False)
            MsgBox(String.Format("Error: {0}", ex.Message), vbCritical + vbOKOnly, "SSO")
        End Try
    End Sub

    Private Sub Processing_btSearch_Click(sender As Object, e As EventArgs) Handles Processing_btSearch.Click
        Dim str_WHERE_SQL As String = ""

        If Processing_rbtFilter.Checked = True Then
            If Processing_cbFilter_byUser.Text <> "All" Then
                If str_WHERE_SQL.Length = 0 Then
                    str_WHERE_SQL = "USER_CREATED LIKE '" & Processing_cbFilter_byUser.Text & "'"
                Else
                    str_WHERE_SQL = str_WHERE_SQL & " AND USER_CREATED LIKE '%" & Processing_cbFilter_byUser.Text & "%'"
                End If
            End If

            If Processing_cbByStatus.Text <> "All" Then
                If str_WHERE_SQL.Length = 0 Then
                    str_WHERE_SQL = "BATCH_STATUS = '" & Processing_cbByStatus.Text & "'"
                Else
                    str_WHERE_SQL = str_WHERE_SQL & " AND BATCH_STATUS = '" & Processing_cbByStatus.Text & "'"
                End If
            End If

            If Processing_cbByDate.EditValue <> Nothing Then
                Dim date_filter As String = Format(Processing_cbByDate.EditValue, "dd/MM/yyyy")
                If str_WHERE_SQL.Length = 0 Then
                    str_WHERE_SQL = "DATE_CREATED LIKE '" & date_filter & "'"
                Else
                    str_WHERE_SQL = str_WHERE_SQL & " AND DATE_CREATED LIKE '%" & date_filter & "%'"
                End If
            End If

            If str_WHERE_SQL.Length = 0 Then
                Processing_GridControl_TaskList.DataSource = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT DISTINCT BATCH_NAME, USER_CREATED, DATE_CREATED, BATCH_STATUS FROM PAYMENT_INSTRUCTION_REPORT ORDER BY CAST(SUBSTR(DATE_CREATED, 7, 4) AS INT) DESC, CAST(SUBSTR(DATE_CREATED, 4, 2) AS INT) DESC, CAST(SUBSTR(DATE_CREATED, 1, 2) AS INT) DESC, BATCH_NAME ASC")
            Else
                Processing_GridControl_TaskList.DataSource = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT DISTINCT BATCH_NAME, USER_CREATED, DATE_CREATED, BATCH_STATUS FROM PAYMENT_INSTRUCTION_REPORT WHERE " & str_WHERE_SQL & "ORDER BY CAST(SUBSTR(DATE_CREATED, 7, 4) AS INT) DESC, CAST(SUBSTR(DATE_CREATED, 4, 2) AS INT) DESC, CAST(SUBSTR(DATE_CREATED, 1, 2) AS INT) DESC, BATCH_NAME ASC")
            End If

            Processing_GridView_TaskList.BestFitColumns()
        End If

        If Processing_rbtSearchByText.Checked = True Then
            If Len(Processing_tbSearchByText.Text) = 0 Then
                Exit Sub
            End If

            Dim DT_Col As DataTable = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT * FROM PAYMENT_INSTRUCTION_REPORT LIMIT 1")
            Dim SQL_Str_Search As String = ""
            For i = 0 To DT_Col.Columns.Count - 1
                If SQL_Str_Search.Length = 0 Then
                    SQL_Str_Search = "[" & DT_Col.Columns(i).ColumnName.ToString() & "] LIKE '%" & Processing_tbSearchByText.Text & "%'"
                Else
                    SQL_Str_Search = SQL_Str_Search & " OR [" & DT_Col.Columns(i).ColumnName.ToString() & "] LIKE '%" & Processing_tbSearchByText.Text & "%'"
                End If
            Next

            str_WHERE_SQL = "(" & SQL_Str_Search & ") "

            Processing_GridControl_TaskList.DataSource = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT DISTINCT BATCH_NAME, USER_CREATED, DATE_CREATED, BATCH_STATUS FROM PAYMENT_INSTRUCTION_REPORT WHERE " & str_WHERE_SQL & "ORDER BY CAST(SUBSTR(DATE_CREATED, 7, 4) AS INT) DESC, CAST(SUBSTR(DATE_CREATED, 4, 2) AS INT) DESC, CAST(SUBSTR(DATE_CREATED, 1, 2) AS INT) DESC, BATCH_NAME ASC")
            Processing_GridView_TaskList.BestFitColumns()
        End If

    End Sub

    Private Sub Processing_btSendEmail_Click(sender As Object, e As EventArgs) Handles Processing_btSendEmail.Click
        Try
            SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
            Dim BATCH_NAME As String = Processing_GridView_Queue1.GetRowCellValue(0, "BATCH_NAME")
            Dim OutlookApp As New Outlook.Application
            Dim olMail As Outlook.MailItem
            olMail = OutlookApp.CreateItem(0)

            'CREATE TABLE FROM GRIDVIEW
            Dim dt_gridview As DataTable = GridView_To_Datatable(Processing_GridView_Queue1)
            Dim list_col As String = ""
            For col_num As Integer = 0 To dt_gridview.Columns.Count - 1
                If list_col.Length = 0 Then
                    list_col = dt_gridview.Columns(col_num).ColumnName.ToString() & " VARCHAR"
                Else
                    list_col = list_col & ", " & dt_gridview.Columns(col_num).ColumnName.ToString() & " VARCHAR"
                End If
            Next

            Dim user_created As String = Environment.UserName & "_" & Now.ToString("yyyyMMddhhmmss")
            Dim TABLE_ALL_LIST_CONFIRM As String = "ALL_LIST_CONFIRM_" & user_created

            SQL_QUERY(CONNECTION_SSO, "CREATE TABLE If Not EXISTS " & TABLE_ALL_LIST_CONFIRM & "(" & list_col & ")")
            SQLITE_BULK_COPY(dt_gridview, link_database_SSO, TABLE_ALL_LIST_CONFIRM)

            'SEND EMAIL
            Dim DT_RELEASE As DataTable = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT CUSTOMER_ID, CUSTOMER_NAME, BATCH, SUB_BATCH, PMT_REF, PMT_STATUS, PMT_TYPE, DEBIT_AC_NO, DEBIT_AMOUNT, PAYMENT_CCY, PAYEE_AMOUNT, PROCESS_DATE, PAYEE_NAME, ADDRESS1, ADDRESS2, ADDRESS3, BEN_ACC, BEN_BANK_CODE, PAYMENT_DETAILS_1, PAYMENT_DETAILS_2, LOCAL_CHRG, BANK_NAME, TT_BENEFICIARY_BANK_DETAILS FROM PAYMENT_INSTRUCTION_REPORT WHERE CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_ALL_LIST_CONFIRM & " WHERE ACTION = 'RELEASE') AND BATCH_NAME = '" & BATCH_NAME & "'")
            Dim DT_HOLD As DataTable = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT CUSTOMER_ID, CUSTOMER_NAME, BATCH, SUB_BATCH, PMT_REF, PMT_STATUS, PMT_TYPE, DEBIT_AC_NO, DEBIT_AMOUNT, PAYMENT_CCY, PAYEE_AMOUNT, PROCESS_DATE, PAYEE_NAME, ADDRESS1, ADDRESS2, ADDRESS3, BEN_ACC, BEN_BANK_CODE, PAYMENT_DETAILS_1, PAYMENT_DETAILS_2, LOCAL_CHRG, BANK_NAME, TT_BENEFICIARY_BANK_DETAILS FROM PAYMENT_INSTRUCTION_REPORT WHERE CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_ALL_LIST_CONFIRM & " WHERE ACTION = 'HOLD') AND BATCH_NAME = '" & BATCH_NAME & "'")
            Dim DT_CANCEL As DataTable = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT CUSTOMER_ID, CUSTOMER_NAME, BATCH, SUB_BATCH, PMT_REF, PMT_STATUS, PMT_TYPE, DEBIT_AC_NO, DEBIT_AMOUNT, PAYMENT_CCY, PAYEE_AMOUNT, PROCESS_DATE, PAYEE_NAME, ADDRESS1, ADDRESS2, ADDRESS3, BEN_ACC, BEN_BANK_CODE, PAYMENT_DETAILS_1, PAYMENT_DETAILS_2, LOCAL_CHRG, BANK_NAME, TT_BENEFICIARY_BANK_DETAILS FROM PAYMENT_INSTRUCTION_REPORT WHERE CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_ALL_LIST_CONFIRM & " WHERE ACTION = 'CANCEL') AND BATCH_NAME = '" & BATCH_NAME & "'")

            Dim doctext_confirmall As String = ""

            If DT_HOLD.Rows.Count = dt_gridview.Rows.Count Then
                'HOLD ALL
                doctext_confirmall = getDocText(Replace(SQL_FROMFILE_TO_STRING(global_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Template_Email_Payment_Intruction_Hold_All'"), "appPath\", appPath))
                GridView_DataMaintenance.Columns.Clear()
                GridControl_DataMaintenance.DataSource = DT_HOLD
                doctext_confirmall = doctext_confirmall.Replace("{REPORT_HOLD}", ExportGridToHtml(GridView_DataMaintenance))
            Else
                If DT_RELEASE.Rows.Count = dt_gridview.Rows.Count Then
                    'RELEASE ALL
                    doctext_confirmall = getDocText(Replace(SQL_FROMFILE_TO_STRING(global_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Template_Email_Payment_Intruction_Release_All'"), "appPath\", appPath))
                    GridView_DataMaintenance.Columns.Clear()
                    GridControl_DataMaintenance.DataSource = DT_RELEASE
                    doctext_confirmall = doctext_confirmall.Replace("{REPORT_COMPLETE}", ExportGridToHtml(GridView_DataMaintenance))
                Else
                    If DT_CANCEL.Rows.Count = dt_gridview.Rows.Count Then
                        'CANCEL ALL
                        doctext_confirmall = getDocText(Replace(SQL_FROMFILE_TO_STRING(global_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Template_Email_Payment_Intruction_Cancel_All'"), "appPath\", appPath))
                        GridView_DataMaintenance.Columns.Clear()
                        GridControl_DataMaintenance.DataSource = DT_RELEASE
                        doctext_confirmall = doctext_confirmall.Replace("{REPORT_COMPLETE}", ExportGridToHtml(GridView_DataMaintenance))
                    Else
                        doctext_confirmall = getDocText(Replace(SQL_FROMFILE_TO_STRING(global_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Template_Email_Payment_Intruction_Release_Hold_Cancel'"), "appPath\", appPath))
                        GridView_DataMaintenance.Columns.Clear()
                        GridControl_DataMaintenance.DataSource = DT_RELEASE
                        doctext_confirmall = doctext_confirmall.Replace("{REPORT_COMPLETE}", ExportGridToHtml(GridView_DataMaintenance))

                        GridView_DataMaintenance.Columns.Clear()
                        GridControl_DataMaintenance.DataSource = DT_HOLD
                        doctext_confirmall = doctext_confirmall.Replace("{REPORT_HOLD}", ExportGridToHtml(GridView_DataMaintenance))

                        GridView_DataMaintenance.Columns.Clear()
                        GridControl_DataMaintenance.DataSource = DT_CANCEL
                        doctext_confirmall = doctext_confirmall.Replace("{REPORT_CANCEL}", ExportGridToHtml(GridView_DataMaintenance))
                    End If
                End If
            End If

            With olMail
                .To = SQL_FROMFILE_TO_STRING(global_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Email_Payment_Instruction_To'")
                .CC = SQL_FROMFILE_TO_STRING(global_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Email_Payment_Instruction_CC'")
                .BCC = SQL_FROMFILE_TO_STRING(global_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Email_Payment_Instruction_BCC'")
                .Subject = SQL_FROMFILE_TO_STRING(global_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Email_Payment_Instruction_Subject'")
                .HTMLBody = doctext_confirmall
                .Display()
            End With

            GridView_DataMaintenance.Columns.Clear()

            'UPDATE BATCH TO DATABASE VSD
            Dim TABLE_LIST_UPDATE_VSD As String = "LIST_RELEASE_" & user_created
            SQL_QUERY(CONNECTION_SSO, "CREATE TABLE IF NOT EXISTS " & TABLE_LIST_UPDATE_VSD & "(CASE_ID VARCHAR, CASE_STATUS VARCHAR, REF_NO VARCHAR, VSD_REF_NO VARCHAR, ACTION VARCHAR, COUNT_VSD NUMERIC)")
            Dim SQL_STRING As String = "SELECT DISTINCT A.CASE_ID, A.CASE_STATUS, A.REF_NO, A.VSD_REF_NO, A.ACTION, CASE WHEN B.COUNT_VSD IS NULL THEN 0 ELSE B.COUNT_VSD END AS COUNT_VSD " &
                                            "FROM (SELECT * FROM " & TABLE_ALL_LIST_CONFIRM & ") A " &
                                            "LEFT JOIN (SELECT REF_NO, COUNT(REF_NO) AS COUNT_VSD FROM FIXED_DAILY_VSD_REPORT WHERE CASE_STATUS NOT IN ('Completed', 'CANCEL') GROUP BY REF_NO) B ON A.VSD_REF_NO = B.REF_NO"
            SQLITE_BULK_COPY(SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, SQL_STRING), link_database_SSO, TABLE_LIST_UPDATE_VSD)

            If DT_RELEASE.Rows.Count > 0 Then
                'COUNT_VSD = 1
                SQL_QUERY(CONNECTION_SSO, "UPDATE FIXED_DAILY_VSD_REPORT SET BATCH_PAYMENT_INSTRUCTION = '" & BATCH_NAME & "', CASE_STATUS = 'Completed', USER_MODIFIED = '" & user_created & "' WHERE CASE_STATUS NOT IN ('Completed', 'CANCEL') AND REF_NO IN (SELECT DISTINCT VSD_REF_NO FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'RELEASE' AND COUNT_VSD = 1)")
                SQL_QUERY(CONNECTION_SSO, "UPDATE PAYMENT_INSTRUCTION_REPORT SET CASE_STATUS = 'Completed', USER_MODIFIED = '" & user_created & "' WHERE CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'RELEASE' AND COUNT_VSD = 1)")
                SQL_QUERY(CONNECTION_SSO, "UPDATE INTERNAL_TRANSFER SET PAYMENT_STATUS = 'Completed' WHERE PAYMENT_CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'RELEASE' AND COUNT_VSD = 1)")

                'COUNT_VSD > 1
                SQL_QUERY(CONNECTION_SSO, "UPDATE PAYMENT_INSTRUCTION_REPORT SET USER_NOTE = 'Please manual update in VSD report', CASE_STATUS = 'Completed', USER_MODIFIED = '" & user_created & "' WHERE CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'RELEASE' AND COUNT_VSD > 1)")
                SQL_QUERY(CONNECTION_SSO, "UPDATE INTERNAL_TRANSFER SET PAYMENT_STATUS = 'Completed' WHERE PAYMENT_CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'RELEASE' AND COUNT_VSD > 1)")

                'COUNT_VSD = 0 AND VSD_REF_NO IS NULL
                SQL_QUERY(CONNECTION_SSO, "UPDATE PAYMENT_INSTRUCTION_REPORT SET USER_NOTE = 'Not found in VSD report', CASE_STATUS = 'Completed', USER_MODIFIED = '" & user_created & "' WHERE CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'RELEASE' AND COUNT_VSD = 0 AND VSD_REF_NO IS NULL)")
                SQL_QUERY(CONNECTION_SSO, "UPDATE INTERNAL_TRANSFER SET PAYMENT_STATUS = 'Completed' WHERE PAYMENT_CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'RELEASE' AND COUNT_VSD = 0 AND VSD_REF_NO IS NULL)")
            End If

            If DT_HOLD.Rows.Count > 0 Then
                'COUNT_VSD = 1
                SQL_QUERY(CONNECTION_SSO, "UPDATE FIXED_DAILY_VSD_REPORT SET BATCH_PAYMENT_INSTRUCTION = '" & BATCH_NAME & "', CASE_STATUS = 'HOLD', USER_MODIFIED = '" & user_created & "' WHERE CASE_STATUS NOT IN ('Completed', 'CANCEL') AND REF_NO IN (SELECT DISTINCT VSD_REF_NO FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'HOLD' AND COUNT_VSD = 1)")
                SQL_QUERY(CONNECTION_SSO, "UPDATE PAYMENT_INSTRUCTION_REPORT SET CASE_STATUS = 'HOLD', USER_MODIFIED = '" & user_created & "' WHERE CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'HOLD' AND COUNT_VSD = 1)")
                SQL_QUERY(CONNECTION_SSO, "UPDATE INTERNAL_TRANSFER SET PAYMENT_STATUS = 'HOLD' WHERE PAYMENT_CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'HOLD' AND COUNT_VSD = 1)")

                'COUNT_VSD > 1
                SQL_QUERY(CONNECTION_SSO, "UPDATE PAYMENT_INSTRUCTION_REPORT SET USER_NOTE = 'Please manual update in VSD report', CASE_STATUS = 'HOLD', USER_MODIFIED = '" & user_created & "' WHERE CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'HOLD' AND COUNT_VSD > 1)")
                SQL_QUERY(CONNECTION_SSO, "UPDATE INTERNAL_TRANSFER SET PAYMENT_STATUS = 'HOLD' WHERE PAYMENT_CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'HOLD' AND COUNT_VSD > 1)")

                'COUNT_VSD = 0 AND VSD_REF_NO IS NULL
                SQL_QUERY(CONNECTION_SSO, "UPDATE PAYMENT_INSTRUCTION_REPORT SET USER_NOTE = 'Not found in VSD report', CASE_STATUS = 'HOLD', USER_MODIFIED = '" & user_created & "' WHERE CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'HOLD' AND COUNT_VSD = 0 AND VSD_REF_NO IS NULL)")
                SQL_QUERY(CONNECTION_SSO, "UPDATE INTERNAL_TRANSFER SET PAYMENT_STATUS = 'HOLD' WHERE PAYMENT_CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'HOLD' AND COUNT_VSD = 0 AND VSD_REF_NO IS NULL)")
            End If

            If DT_CANCEL.Rows.Count > 0 Then
                'COUNT_VSD = 1
                SQL_QUERY(CONNECTION_SSO, "UPDATE FIXED_DAILY_VSD_REPORT SET BATCH_PAYMENT_INSTRUCTION = '" & BATCH_NAME & "', CASE_STATUS = 'CANCEL', USER_MODIFIED = '" & user_created & "' WHERE CASE_STATUS NOT IN ('Completed', 'CANCEL') AND REF_NO IN (SELECT DISTINCT VSD_REF_NO FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'CANCEL' AND COUNT_VSD = 1)")
                SQL_QUERY(CONNECTION_SSO, "UPDATE PAYMENT_INSTRUCTION_REPORT SET CASE_STATUS = 'CANCEL', USER_MODIFIED = '" & user_created & "' WHERE CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'CANCEL' AND COUNT_VSD = 1)")
                SQL_QUERY(CONNECTION_SSO, "UPDATE INTERNAL_TRANSFER SET PAYMENT_STATUS = 'CANCEL' WHERE PAYMENT_CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'CANCEL' AND COUNT_VSD = 1)")

                'COUNT_VSD > 1
                SQL_QUERY(CONNECTION_SSO, "UPDATE PAYMENT_INSTRUCTION_REPORT SET USER_NOTE = 'Please manual update in VSD report', CASE_STATUS = 'CANCEL', USER_MODIFIED = '" & user_created & "' WHERE CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'CANCEL' AND COUNT_VSD > 1)")
                SQL_QUERY(CONNECTION_SSO, "UPDATE INTERNAL_TRANSFER SET PAYMENT_STATUS = 'CANCEL' WHERE PAYMENT_CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'CANCEL' AND COUNT_VSD > 1)")

                'COUNT_VSD = 0 AND VSD_REF_NO IS NULL
                SQL_QUERY(CONNECTION_SSO, "UPDATE PAYMENT_INSTRUCTION_REPORT SET USER_NOTE = 'Not found in VSD report', CASE_STATUS = 'CANCEL', USER_MODIFIED = '" & user_created & "' WHERE CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'CANCEL' AND COUNT_VSD = 0 AND VSD_REF_NO IS NULL)")
                SQL_QUERY(CONNECTION_SSO, "UPDATE INTERNAL_TRANSFER SET PAYMENT_STATUS = 'CANCEL' WHERE PAYMENT_CASE_ID IN (SELECT DISTINCT CASE_ID FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND ACTION = 'CANCEL' AND COUNT_VSD = 0 AND VSD_REF_NO IS NULL)")
            End If

            Dim DT_LIST_CASE_NOT_YET_MAPPING As DataTable = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT * FROM " & TABLE_LIST_UPDATE_VSD & " WHERE CASE_STATUS <> 'Completed' AND COUNT_VSD = 0 AND VSD_REF_NO IS NOT NULL")
            If DT_LIST_CASE_NOT_YET_MAPPING.Rows.Count > 0 Then
                For Each DRR As DataRow In DT_LIST_CASE_NOT_YET_MAPPING.Rows
                    Dim CASE_ID As String = DRR("CASE_ID").ToString
                    Dim USER_NOTE As String = ""
                    Dim LIST_VSD_REF_NO As List(Of String) = SPLIT_TEXT_TO_LIST_BY_DELIMITOR(DRR("VSD_REF_NO").ToString, ";")

                    For j As Integer = 0 To LIST_VSD_REF_NO.Count - 1
                        Dim VSD_REF_NO As String = LIST_VSD_REF_NO.Item(j)
                        Dim count_ref_no As Integer = SQL_QUERY_TO_INTEGER(CONNECTION_SSO, "SELECT COUNT(REF_NO) FROM FIXED_DAILY_VSD_REPORT WHERE CASE_STATUS NOT IN ('Completed', 'CANCEL') AND REF_NO = '" & VSD_REF_NO & "'")
                        If count_ref_no > 1 Then
                            If USER_NOTE.Length = 0 Then
                                USER_NOTE = "Please manual update in VSD report"
                            Else
                                USER_NOTE = USER_NOTE & "; " & "Please manual update in VSD report"
                            End If
                        Else
                            If count_ref_no = 0 Then
                                If USER_NOTE.Length = 0 Then
                                    USER_NOTE = "Not found in VSD report"
                                Else
                                    USER_NOTE = USER_NOTE & "; " & "Not found in VSD report"
                                End If
                            Else
                                SQL_QUERY(CONNECTION_SSO, "UPDATE FIXED_DAILY_VSD_REPORT SET BATCH_PAYMENT_INSTRUCTION = '" & BATCH_NAME & "', CASE_STATUS = 'Completed', USER_MODIFIED = '" & UCase(Environment.UserName) & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss") & "' WHERE CASE_STATUS <> 'Completed' AND REF_NO = '" & VSD_REF_NO & "'")
                            End If
                        End If
                    Next
                    SQL_QUERY(CONNECTION_SSO, "UPDATE INTERNAL_TRANSFER SET PAYMENT_STATUS = 'Completed' WHERE PAYMENT_CASE_ID = '" & CASE_ID & "'")
                    SQL_QUERY(CONNECTION_SSO, "UPDATE PAYMENT_INSTRUCTION_REPORT SET USER_NOTE = '" & USER_NOTE & "', CASE_STATUS = 'Completed', USER_MODIFIED = '" & UCase(Environment.UserName) & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss") & "' WHERE CASE_ID = '" & CASE_ID & "'")
                Next
            End If

            Processing_GridControl_Queue.DataSource = SQL_QUERY_TO_DATATABLE(CONNECTION_SSO, "SELECT * FROM PAYMENT_INSTRUCTION_REPORT WHERE BATCH_NAME = '" & BATCH_NAME & "'")
            GridControl_DataMaintenance.DataSource = Nothing
            GridView_DataMaintenance.Columns.Clear()

            If SQL_FROMFILE_TO_STRING(global_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Delete_Temp_Send_Email'") = "YES" Then
                SQL_QUERY(CONNECTION_SSO, "DROP TABLE " & TABLE_LIST_UPDATE_VSD)
                SQL_QUERY(CONNECTION_SSO, "DROP TABLE " & TABLE_ALL_LIST_CONFIRM)
                If SQL_FROMFILE_TO_STRING(global_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Refresh_Database_after_delete'") = "YES" Then
                    SQL_QUERY(CONNECTION_SSO, "vacuum")
                End If
            End If
            SplashScreenManager.CloseForm(False)

            MsgBox("Completed", vbInformation + vbOKOnly)
        Catch ex As Exception
            WriteErrorLog(String.Format("Error: {0}", ex.ToString))
            SplashScreenManager.CloseForm(False)
            MsgBox(String.Format("Error: {0}", ex.Message), vbCritical + vbOKOnly, "SSO")
        End Try

    End Sub

#End Region

#Region "DATA_MAINTENANCE"

    Private Sub DataMaintenance_cbTableName_TextChanged(sender As Object, e As EventArgs) Handles DataMaintenance_cbTableName.TextChanged
        If DataMaintenance_cbTableName.Text = "Redemption Control Report" Then
            Dim SQL_STRING As String = "SELECT A.SYMBOL, A.VALUE_DATE, A.TOTAL_AMOUNT, B.FULLNAME, B.CUSTODYCD, B.BANKACNAME, B.CITYBANK, B.BANKACC, B.CASH_ACCOUNT" &
                                        ", B.AMOUNT_PAYMENT, C.PAYEE_AMOUNT, B.TRADE_DATE, B.DEADLINE_PER_FUND, B.CASE_STATUS, B.USER_MODIFIED, B.USER_NOTE" &
                                        " FROM (SELECT * FROM INTERNAL_TRANSFER WHERE STATUS = 'Incomplete') A" &
                                        " LEFT JOIN (SELECT * FROM FIXED_DAILY_VSD_REPORT) B ON A.SYMBOL = B.SYMBOL AND A.VALUE_DATE = B.VALUE_DATE" &
                                        " LEFT JOIN (SELECT * FROM PAYMENT_INSTRUCTION_REPORT) C ON B.BATCH_PAYMENT_INSTRUCTION = C.BATCH_NAME AND B.REF_NO = C.VSD_REF_NO"
            GridView_DataMaintenance.Columns.Clear()
            GridControl_DataMaintenance.DataSource = SQL_FROMFILE_TO_DATATABLE(link_database_SSO, SQL_STRING)
            GridView_DataMaintenance.BestFitColumns()
        Else
            GridView_DataMaintenance.Columns.Clear()
            GridControl_DataMaintenance.DataSource = SQL_FROMFILE_TO_DATATABLE(link_database_SSO, "SELECT * FROM " & DataMaintenance_cbTableName.Text)
            GridView_DataMaintenance.BestFitColumns()
        End If
    End Sub

    Private Sub GridView_DataMaintenance_CellValueChanged(sender As Object, e As CellValueChangedEventArgs) Handles GridView_DataMaintenance.CellValueChanged
        If DataMaintenance_cbTableName.Text = "Redemption Control Report" Then
            Exit Sub
        End If

        Dim view As GridView = sender
        If view Is Nothing Then
            Exit Sub
        End If

        Dim Column_NAME As String = e.Column.GetTextCaption()
        If Column_NAME = "USER_CREATED" Or Column_NAME = "USER_MODIFIED" Then
            Exit Sub
        End If

        Dim iRet = MsgBox("Do you want to save this changes?", vbQuestion + vbYesNo, "SSO")
        If iRet = vbYes Then
            Dim USER_MODIFIED As String = UCase(Environment.UserName) & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss")
            SQL_QUERY(CONNECTION_SSO, "UPDATE " & DataMaintenance_cbTableName.Text & " SET USER_MODIFIED = '" & USER_MODIFIED & "', " & Column_NAME & " = '" & GridView_DataMaintenance.GetFocusedRowCellDisplayText(Column_NAME).ToString & "' WHERE CASE_ID = '" & GridView_DataMaintenance.GetFocusedRowCellDisplayText("CASE_ID").ToString & "'")
        End If
    End Sub

    Private Sub DataMaintenance_btExportToExcel_Click(sender As Object, e As EventArgs) Handles DataMaintenance_btExportToExcel.Click
        Try
            If GridView_DataMaintenance.RowCount = 0 Then
                Exit Sub
            End If
            EXPORT_GRIDVIEW_TO_EXCEL(GridView_DataMaintenance)
        Catch ex As Exception
            MsgBox(ex.Message, vbCritical + vbOKOnly)
            WriteErrorLog(ex.ToString)
        End Try
    End Sub

#End Region
End Class