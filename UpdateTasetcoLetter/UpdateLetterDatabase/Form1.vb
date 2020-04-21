Imports DevExpress.XtraBars
Imports DevExpress.XtraEditors.Repository
Imports OpenQA.Selenium.Chrome

Partial Public Class Form1
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public link_app_config As String = appPath & "Application_Config.db"
    Public link_application_config As String = Module_Letter_Management.SQL_QUERY_TO_STRING(link_app_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'Link_Application_Config'")
    Public Link_folder_database As String
    Public link_database As String
    Public table_name As String
    Private Property pageready As Boolean = False

    Shared Sub New()
        DevExpress.UserSkins.BonusSkins.Register()
        DevExpress.Skins.SkinManager.EnableFormSkins()
    End Sub
    Public Sub New()
        InitializeComponent()
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StatusBar1.Caption = ""
        StatusBar3.Caption = ""
        StatusBar2.Caption = ""

        'Add value for combobox StrFilter
        ComboBox_FilterDatabase.Items.Add("")
        ComboBox_FilterDatabase.Items.Add("All")
        ComboBox_FilterDatabase.Items.Add("Incomplete")
        ComboBox_FilterDatabase.Items.Add("Completed")
        ComboBox_FilterDatabase.Items.Add("Other")

        'add value vendor name
        BarEditItem_VendorName.EditWidth = 150
        BarEditItem_DatabaseName.EditWidth = 150
        BarEditItem_Year.EditWidth = 100

        Dim DT As DataTable = Module_Letter_Management.SQL_QUERY_TO_DATATABLE(link_application_config, "SELECT DISTINCT Vendor_Name FROM LIST_DATABASE")
        If DT.Rows.Count > 0 Then
            For i As Integer = 0 To DT.Rows.Count - 1
                CType(BarEditItem_VendorName.Edit, RepositoryItemComboBox).Items.Add(DT.Rows(i).Item(0).ToString)
            Next
            BarEditItem_VendorName.EditValue = DT.Rows(0).Item(0).ToString
        End If

        btLoadDatabase.PerformClick()

        WebBrowser1.ScriptErrorsSuppressed = True
    End Sub
    Private Sub BarEditItem_VendorName_EditValueChanged(sender As Object, e As EventArgs) Handles BarEditItem_VendorName.EditValueChanged
        CType(BarEditItem_DatabaseName.Edit, RepositoryItemComboBox).Items.Clear()
        CType(BarEditItem_Year.Edit, RepositoryItemComboBox).Items.Clear()

        'Add value database
        Dim DT As DataTable = Module_Letter_Management.SQL_QUERY_TO_DATATABLE(link_application_config, "SELECT DISTINCT Database_Name FROM LIST_DATABASE WHERE Vendor_Name = '" & BarEditItem_VendorName.EditValue.ToString & "'")
        If DT.Rows.Count > 0 Then
            For i As Integer = 0 To DT.Rows.Count - 1
                CType(BarEditItem_DatabaseName.Edit, RepositoryItemComboBox).Items.Add(DT.Rows(i).Item(0).ToString)
            Next
            BarEditItem_DatabaseName.EditValue = DT.Rows(0).Item(0).ToString
        End If

        'add value year database
        For i = 2018 To CInt(Now.ToString("yyyy"))
            CType(BarEditItem_Year.Edit, RepositoryItemComboBox).Items.Add(i)
        Next
        BarEditItem_Year.EditValue = CInt(Now.ToString("yyyy"))
    End Sub
    Private Sub Refresh_link_database()
        'On Error GoTo err_handle
        Link_folder_database = ""
        link_database = ""
        table_name = ""

        'Get link_folder_database
        Link_folder_database = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Folder_Path FROM LIST_DATABASE WHERE Vendor_NAME = '" & BarEditItem_VendorName.EditValue.ToString & "' AND Database_Name = '" & BarEditItem_DatabaseName.EditValue.ToString & "'")
        If Link_folder_database.Substring(Link_folder_database.Length - 1, 1) <> "\" Then
            Link_folder_database = Link_folder_database & "\"
        End If

        'Setup link database
        If BarEditItem_VendorName.EditValue.ToString = "TASETCO EXPRESS" Then
            link_database = Link_folder_database & "Database_Letter_Management.txt"
            table_name = "Tasetco_" & BarEditItem_DatabaseName.EditValue.ToString & "_" & BarEditItem_Year.EditValue.ToString
        Else
            link_database = Link_folder_database & "Database_Viettel.txt"
            table_name = "Viettel_" & BarEditItem_DatabaseName.EditValue.ToString & "_" & BarEditItem_Year.EditValue.ToString
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btLoadDatabase_ItemClick(sender As Object, e As ItemClickEventArgs) Handles btLoadDatabase.ItemClick
        On Error GoTo err_handle

        StatusBar1.Caption = ""
        StatusBar3.Caption = ""
        StatusBar2.Caption = ""

        Refresh_link_database()
        ComboBox_FilterDatabase.Text = "Incomplete"
        btLoad.PerformClick()

        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub
    Private Sub btGetInformation_Click(sender As Object, e As EventArgs) Handles btGetInformation.Click
        Dim iRet = MsgBox("Do you want to get mailing status from Database of Vendor?", vbYesNo, "Vendor Management - Update Database")
        If iRet = vbNo Then
            Exit Sub
        Else
            Update_information_from_vendor()
        End If
    End Sub

    Public Function verify_element(driver As ChromeDriver, ByVal elementName As String) As Boolean
        Try
            Dim isElementDisplayed As Boolean = driver.FindElementById(elementName).Displayed
            Return True
        Catch
            Return False
        End Try
    End Function

    Private Sub Update_information_from_vendor()
        On Error GoTo err_handle
        Dim str_update As String = ""

        If GridView1.RowCount = 0 Then Exit Sub

        StatusBar1.Caption = ""
        StatusBar3.Caption = ""
        StatusBar2.Caption = ""

        'Kiem tra xem co user nao dang download information for database hay khong
        Dim User_Updating_Database As String = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT User_Updating_Database FROM LIST_DATABASE WHERE Vendor_Name = '" & BarEditItem_VendorName.EditValue.ToString & "' AND Database_Name = '" & BarEditItem_DatabaseName.EditValue.ToString & "'")
        Dim Year_Updating_Database As String = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Year_Updating_Database FROM LIST_DATABASE WHERE Vendor_Name = '" & BarEditItem_VendorName.EditValue.ToString & "' AND Database_Name = '" & BarEditItem_DatabaseName.EditValue.ToString & "'")
        If User_Updating_Database.Length > 0 Then
            If Year_Updating_Database.Length > 0 Then
                If InStr(Year_Updating_Database, BarEditItem_Year.EditValue.ToString, vbBinaryCompare) > 0 Then
                    MsgBox("User " & User_Updating_Database & " still updating database. Please try again later", vbInformation, "Vendor Management - Update Database")
                    Exit Sub
                End If
            End If
        End If

        'Neu khong co user nao dang download thi update user hien tai vao database
        If Year_Updating_Database.Length > 0 Then
            Year_Updating_Database = Year_Updating_Database & ";" & BarEditItem_Year.EditValue.ToString
        Else
            Year_Updating_Database = BarEditItem_Year.EditValue.ToString
        End If
        If User_Updating_Database.Length > 0 Then
            User_Updating_Database = User_Updating_Database & ";" & Environ("username")
        Else
            User_Updating_Database = Environ("username")
        End If
        str_update = "UPDATE LIST_DATABASE" &
                            " SET [User_Updating_Database] = '" & User_Updating_Database & "'," &
                            " [Year_Updating_Database] = '" & Year_Updating_Database & "'" &
                            " WHERE Vendor_Name = '" & BarEditItem_VendorName.EditValue.ToString & "' AND [Database_Name] = '" & BarEditItem_DatabaseName.EditValue.ToString & "'"
        Module_Letter_Management.SQL_QUERY(link_application_config, str_update)

        StatusBar1.Caption = "Connecting to database..."

        If BarEditItem_VendorName.EditValue.ToString = "TASETCO EXPRESS" Then
            StatusBar1.Caption = "Connecting to database of vendor..."
            Wait(1000)
            Dim service As ChromeDriverService = ChromeDriverService.CreateDefaultService()
            service.HideCommandPromptWindow = True
            Dim driver_option As ChromeOptions = New ChromeOptions
            driver_option.AddArgument("no-sandbox")
            driver_option.AddAdditionalCapability("useAutomationExtension", False)

            Dim ChromeDriver As ChromeDriver = New ChromeDriver(service, driver_option, TimeSpan.FromMinutes(3))

            ChromeDriver.Navigate().GoToUrl("http://www.google.com.vn")

            Do While InStr(1, ChromeDriver.Title, "Google", vbBinaryCompare) = 0
                Application.DoEvents()
            Loop

            For i = 0 To GridView1.RowCount - 1
                Dim rows_process As Integer = i
                StatusBar1.Caption = "[" & i + 1 & "/" & GridView1.RowCount & "]"
                Dim bill_number_to_search As String = GridView1.GetRowCellValue(i, "Bill_Number").ToString()
                Dim url_link As String = "http://www.tansonnhat.vn/tra-cuu-van-đon.aspx?tracking=" & bill_number_to_search
                ChromeDriver.Navigate().GoToUrl(url_link)
                StatusBar2.Caption = ChromeDriver.Url.ToString

                If verify_element(ChromeDriver, "ctl00_plcMain_lblShipmentNumber") = True Then
                    If ChromeDriver.FindElementById("ctl00_plcMain_lblShipmentNumber").Text = bill_number_to_search Then
                        'ChromeDriver.Manage.Window.Size = New Size(750, 1000)
                        'If (Not System.IO.Directory.Exists(Link_folder_database & "Screenshot")) Then
                        '    System.IO.Directory.CreateDirectory(Link_folder_database & "Screenshot")
                        'End If
                        'ChromeDriver.GetScreenshot.SaveAsFile(Link_folder_database & "Screenshot\" & bill_number_to_search & ".jpg", OpenQA.Selenium.ScreenshotImageFormat.Jpeg)

                        Dim Tasetco_PO = ChromeDriver.FindElementById("ctl00_plcMain_lblPoHubForm")
                        Dim Tasetco_TP = ChromeDriver.FindElementById("ctl00_plcMain_lblProvinceName")
                        Dim Tasetco_CustomerNameRC = ChromeDriver.FindElementById("ctl00_plcMain_lblReciverName")
                        Dim Tasetco_Address_RC = ChromeDriver.FindElementById("ctl00_plcMain_lblShippingAddress")
                        Dim Tasetco_Status = ChromeDriver.FindElementById("ctl00_plcMain_lblShipmentStatusName")
                        Dim Tasetco_Ac_Cosignee = ChromeDriver.FindElementById("ctl00_plcMain_lblRealRecipientName")
                        Dim Tasetco_Date_Received = ChromeDriver.FindElementById("ctl00_plcMain_lblEndDeliveryTime")

                        If Len(Tasetco_PO.Text) > 0 Then
                            GridView1.SetRowCellValue(i, "Tasetco_PO", Tasetco_PO.Text)
                        End If
                        If Len(Tasetco_TP.Text) > 0 Then
                            GridView1.SetRowCellValue(i, "Tasetco_TP", Tasetco_TP.Text)
                        End If
                        If Len(Tasetco_CustomerNameRC.Text) > 0 Then
                            GridView1.SetRowCellValue(i, "Tasetco_Cus_Name_RC", Tasetco_CustomerNameRC.Text)
                        End If
                        If Len(Tasetco_Address_RC.Text) > 0 Then
                            GridView1.SetRowCellValue(i, "Tasetco_Address_RC", Tasetco_Address_RC.Text)
                        End If
                        If Len(Tasetco_Status.Text) > 0 Then
                            GridView1.SetRowCellValue(i, "Tasetco_Status", Tasetco_Status.Text)
                        End If
                        If Len(Tasetco_Ac_Cosignee.Text) > 0 Then
                            GridView1.SetRowCellValue(i, "Tasetco_Ac_Cosignee", Tasetco_Ac_Cosignee.Text)
                        End If
                        If Len(Tasetco_Date_Received.Text) > 0 Then
                            GridView1.SetRowCellValue(i, "Tasetco_Date_Received", Tasetco_Date_Received.Text)
                        End If

                        If GridView1.GetRowCellValue(i, "Tasetco_Status").ToString() = "Giao hàng thành công" Then
                            GridView1.SetRowCellValue(i, "Final_Result", "Completed")
                        Else
                            GridView1.SetRowCellValue(i, "Final_Result", "Incomplete")
                        End If
                        GridView1.SetRowCellValue(i, "Tasetco_Date_Get_Data", "SYSTEM_" & Now().ToString("dd/MM/yyyy hh:mm:ss tt"))

                        str_update = "UPDATE " & table_name &
                             " SET [Tasetco_PO] = '" & GridView1.GetRowCellValue(i, "Tasetco_PO").ToString() & "'," &
                                " [Tasetco_TP] = '" & GridView1.GetRowCellValue(i, "Tasetco_TP").ToString() & "'," &
                                " [Tasetco_Cus_Name_RC] = '" & GridView1.GetRowCellValue(i, "Tasetco_Cus_Name_RC").ToString() & "'," &
                                " [Tasetco_Address_RC] = '" & GridView1.GetRowCellValue(i, "Tasetco_Address_RC").ToString() & "'," &
                                " [Tasetco_Status] = '" & GridView1.GetRowCellValue(i, "Tasetco_Status").ToString() & "'," &
                                " [Tasetco_Ac_Cosignee] = '" & GridView1.GetRowCellValue(i, "Tasetco_Ac_Cosignee").ToString() & "'," &
                                " [Tasetco_Date_Received] = '" & GridView1.GetRowCellValue(i, "Tasetco_Date_Received").ToString() & "'," &
                                " [Tasetco_Date_Get_Data] = '" & GridView1.GetRowCellValue(i, "Tasetco_Date_Get_Data").ToString() & "'," &
                                " [Final_Result] = '" & GridView1.GetRowCellValue(i, "Final_Result").ToString() & "'" &
                             " WHERE [Bill_Number] = '" & bill_number_to_search & "'"
                        Module_Letter_Management.SQL_QUERY(link_database, str_update)

                    End If
                Else
                    GridView1.SetRowCellValue(i, "Tasetco_Status", "Not found")
                    GridView1.SetRowCellValue(i, "Tasetco_Date_Get_Data", "SYSTEM_" & Now().ToString("dd/MM/yyyy hh:mm:ss tt"))
                    str_update = "UPDATE " & table_name &
                    " SET [Tasetco_Status] = '" & GridView1.GetRowCellValue(i, "Tasetco_Status").ToString() & "'" & "," &
                    " [Tasetco_Date_Get_Data] = '" & GridView1.GetRowCellValue(i, "Tasetco_Date_Get_Data").ToString() & "'" &
                    " WHERE [Bill_Number] = '" & bill_number_to_search & "'"
                    Module_Letter_Management.SQL_QUERY(link_database, str_update)
                End If
            Next
            ChromeDriver.Quit()
        End If

        If BarEditItem_VendorName.EditValue.ToString = "VIETTEL POST" Then
            StatusBar1.Caption = "Connecting to database of vendor..."
            Wait(1000)
            Dim service As ChromeDriverService = ChromeDriverService.CreateDefaultService()
            service.HideCommandPromptWindow = True
            Dim driver_option As ChromeOptions = New ChromeOptions
            driver_option.AddArgument("no-sandbox")
            driver_option.AddAdditionalCapability("useAutomationExtension", False)

            Dim ChromeDriver As ChromeDriver = New ChromeDriver(service, driver_option, TimeSpan.FromMinutes(3))

            ChromeDriver.Navigate().GoToUrl("http://www.google.com.vn")

            Do While InStr(1, ChromeDriver.Title, "Google", vbBinaryCompare) = 0
                Application.DoEvents()
            Loop

            Dim url_link As String = "https://viettelpost.com.vn/tra-cuu-don-hang"
            ChromeDriver.Navigate().GoToUrl(url_link)

            For i = 0 To GridView1.RowCount - 1
                Dim bill_number_to_search As String = GridView1.GetRowCellValue(i, "Bill_Number").ToString()
                ChromeDriver.FindElementById("orders").SendKeys(bill_number_to_search)
                ChromeDriver.FindElementById("btn_trackorders__form").Click()

                Dim status_found As Boolean = True

                If InStr(1, ChromeDriver.FindElementByClassName("service__name").Text, "Không tìm thấy vận đơn nào phù hợp với quý khách", vbBinaryCompare) > 0 Then
                    status_found = False
                    GridView1.SetRowCellValue(i, "Viettel_Status", "Not found")
                End If

                If status_found = True Then
                    GridView1.SetRowCellValue(i, "Viettel_Size", ChromeDriver.FindElementByClassName("order__weight").Text)
                    GridView1.SetRowCellValue(i, "Viettel_Service", ChromeDriver.FindElementByClassName("order__service").Text)
                    GridView1.SetRowCellValue(i, "Viettel_Status", ChromeDriver.FindElementByClassName("order__status").Text)

                    For Each element As OpenQA.Selenium.IWebElement In ChromeDriver.FindElementsByTagName("tr")
                        If element.GetAttribute("className") = "order__info" Then
                            For Each element2 As OpenQA.Selenium.IWebElement In element.FindElements(OpenQA.Selenium.By.TagName("td"))
                                If element2.GetAttribute("className") = "order__weight" Then
                                    GridView1.SetRowCellValue(i, "Viettel_Size", element2.Text)
                                End If
                                If element2.GetAttribute("className") = "order__service" Then
                                    GridView1.SetRowCellValue(i, "Viettel_Service", element2.Text)
                                End If
                                If element2.GetAttribute("className") = "order__status" Then
                                    GridView1.SetRowCellValue(i, "Viettel_Status", element2.Text)
                                End If
                            Next
                        End If

                        If element.GetAttribute("className") = "order__detail" Then
                            For Each Element2 As OpenQA.Selenium.IWebElement In element.FindElements(OpenQA.Selenium.By.TagName("ul"))
                                If Element2.GetAttribute("className") = "trackorders__activity clearfix" Then
                                    For Each Element3 As OpenQA.Selenium.IWebElement In element.FindElements(OpenQA.Selenium.By.TagName("li"))
                                        For Each Element4 As OpenQA.Selenium.IWebElement In element.FindElements(OpenQA.Selenium.By.TagName("time"))
                                            If Element4.GetAttribute("className") = "datetime" Then
                                                If GridView1.GetRowCellValue(i, "Viettel_Details_Status").ToString().Length = 0 Then
                                                    GridView1.SetRowCellValue(i, "Viettel_Details_Status", Element4.Text)
                                                Else
                                                    GridView1.SetRowCellValue(i, "Viettel_Details_Status", GridView1.GetRowCellValue(i, "Viettel_Details_Status").ToString() & Chr(10) & Element4.Text)
                                                End If
                                                For Each Element5 As OpenQA.Selenium.IWebElement In element.FindElements(OpenQA.Selenium.By.TagName("span"))
                                                    If Element5.GetAttribute("className") = "message" Then
                                                        GridView1.SetRowCellValue(i, "Viettel_Details_Status", GridView1.GetRowCellValue(i, "Viettel_Details_Status").ToString() & " " & Element5.Text)
                                                    End If
                                                Next
                                            End If
                                        Next
                                    Next
                                End If
                            Next
                        End If
                    Next
                End If
                If GridView1.GetRowCellValue(i, "Viettel_Status").ToString() = "Phát thành công" Then
                    GridView1.SetRowCellValue(i, "Final_Result", "Completed")
                End If
                GridView1.SetRowCellValue(i, "Viettel_Date_Get_Data", "SYSTEM_" & Now().ToString("dd/MM/yyyy hh:mm:ss tt"))
                str_update = "UPDATE " & table_name &
                                 " SET [Viettel_Size] = '" & GridView1.GetRowCellValue(i, "Viettel_Size").ToString() & "'," &
                                    " [Viettel_Service] = '" & GridView1.GetRowCellValue(i, "Viettel_Service").ToString() & "'," &
                                    " [Viettel_Status] = '" & GridView1.GetRowCellValue(i, "Viettel_Status").ToString() & "'," &
                                    " [Viettel_DateReceived] = '" & GridView1.GetRowCellValue(i, "Viettel_DateReceived").ToString() & "'," &
                                    " [Viettel_Details_Status] = '" & GridView1.GetRowCellValue(i, "Viettel_Details_Status").ToString() & "'," &
                                    " [Viettel_Date_Get_Data] = '" & Environ("username") & "_" & Now().ToString("dd/MM/yyyy hh:mm:ss tt") & "'," &
                                    " [Final_Result] = '" & GridView1.GetRowCellValue(i, "Final_Result").ToString() & "'" &
                                 " WHERE [Bill_Number] = '" & bill_number_to_search & "'"
                Module_Letter_Management.SQL_QUERY(link_database, str_update)
            Next
        End If
        delete_record_user_update_database()
        StatusBar1.Caption = "[Get Mailing Status] Completed !!!"
        StatusBar2.Caption = ""
        StatusBar3.Caption = ""
        Exit Sub
err_handle:
        StatusBar1.Caption = ""
        StatusBar3.Caption = ""
        StatusBar2.Caption = ""
        delete_record_user_update_database()
        Module_Letter_Management.Error_handle()
    End Sub

    '    Private Sub Update_information_from_vendor()
    '        On Error GoTo err_handle
    '        Dim str_update As String = ""

    '        If GridView1.RowCount = 0 Then Exit Sub

    '        StatusBar1.Caption = ""
    '        StatusBar3.Caption = ""
    '        StatusBar2.Caption = ""

    '        'Kiem tra xem co user nao dang download information for database hay khong
    '        Dim User_Updating_Database As String = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT User_Updating_Database FROM LIST_DATABASE WHERE Vendor_Name = '" & BarEditItem_VendorName.EditValue.ToString & "' AND Database_Name = '" & BarEditItem_DatabaseName.EditValue.ToString & "'")
    '        Dim Year_Updating_Database As String = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Year_Updating_Database FROM LIST_DATABASE WHERE Vendor_Name = '" & BarEditItem_VendorName.EditValue.ToString & "' AND Database_Name = '" & BarEditItem_DatabaseName.EditValue.ToString & "'")
    '        If User_Updating_Database.Length > 0 Then
    '            If Year_Updating_Database.Length > 0 Then
    '                If InStr(Year_Updating_Database, BarEditItem_Year.EditValue.ToString, vbBinaryCompare) > 0 Then
    '                    MsgBox("User " & User_Updating_Database & " still updating database. Please try again later", vbInformation, "Vendor Management - Update Database")
    '                    Exit Sub
    '                End If
    '            End If
    '        End If

    '        'Neu khong co user nao dang download thi update user hien tai vao database
    '        If Year_Updating_Database.Length > 0 Then
    '            Year_Updating_Database = Year_Updating_Database & ";" & BarEditItem_Year.EditValue.ToString
    '        Else
    '            Year_Updating_Database = BarEditItem_Year.EditValue.ToString
    '        End If
    '        If User_Updating_Database.Length > 0 Then
    '            User_Updating_Database = User_Updating_Database & ";" & Environ("username")
    '        Else
    '            User_Updating_Database = Environ("username")
    '        End If
    '        str_update = "UPDATE LIST_DATABASE" &
    '                            " SET [User_Updating_Database] = '" & User_Updating_Database & "'," &
    '                            " [Year_Updating_Database] = '" & Year_Updating_Database & "'" &
    '                            " WHERE Vendor_Name = '" & BarEditItem_VendorName.EditValue.ToString & "' AND [Database_Name] = '" & BarEditItem_DatabaseName.EditValue.ToString & "'"
    '        Module_Letter_Management.SQL_QUERY(link_application_config, str_update)

    '        StatusBar1.Caption = "Connecting to database..."

    '        If BarEditItem_VendorName.EditValue.ToString = "TASETCO EXPRESS" Then
    '            StatusBar1.Caption = "Connecting to database of vendor..."
    '            Wait(1000)
    '            Dim service As ChromeDriverService = ChromeDriverService.CreateDefaultService()
    '            service.HideCommandPromptWindow = True
    '            Dim driver_option As ChromeOptions = New ChromeOptions
    '            driver_option.AddArgument("no-sandbox")
    '            driver_option.AddAdditionalCapability("useAutomationExtension", False)

    '            Dim ChromeDriver As ChromeDriver = New ChromeDriver(service, driver_option, TimeSpan.FromMinutes(3))

    '            ChromeDriver.Navigate().GoToUrl("http://www.google.com.vn")

    '            Do While InStr(1, ChromeDriver.Title, "Google", vbBinaryCompare) = 0
    '                Application.DoEvents()
    '            Loop

    '            For i = 0 To GridView1.RowCount - 1
    '                Dim rows_process As Integer = i
    '                StatusBar1.Caption = "[" & i + 1 & "/" & GridView1.RowCount & "]"
    '                Dim bill_number_to_search As String = GridView1.GetRowCellValue(i, "Bill_Number").ToString()
    '                Dim url_link As String = "http://www.tansonnhat.vn/tra-cuu-van-đon.aspx?tracking=" & bill_number_to_search
    '                ChromeDriver.Navigate().GoToUrl(url_link)
    '                StatusBar2.Caption = ChromeDriver.Url.ToString

    '                If verify_element(ChromeDriver, "BIllCode") = True Then
    '                    If ChromeDriver.FindElementById("BIllCode").Text = bill_number_to_search Then
    '                        ChromeDriver.Manage.Window.Size = New Size(750, 1000)
    '                        If (Not System.IO.Directory.Exists(Link_folder_database & "Screenshot")) Then
    '                            System.IO.Directory.CreateDirectory(Link_folder_database & "Screenshot")
    '                        End If
    '                        ChromeDriver.GetScreenshot.SaveAsFile(Link_folder_database & "Screenshot\" & bill_number_to_search & ".jpg", OpenQA.Selenium.ScreenshotImageFormat.Jpeg)

    '                        Dim Tasetco_PO = ChromeDriver.FindElementById("PO")
    '                        Dim Tasetco_TP = ChromeDriver.FindElementById("TP")
    '                        Dim Tasetco_CustomerNameRC = ChromeDriver.FindElementById("CustomerNameRC")
    '                        Dim Tasetco_Address_RC = ChromeDriver.FindElementById("AddressRC")
    '                        Dim Tasetco_Status = ChromeDriver.FindElementById("Status")
    '                        Dim Tasetco_Ac_Cosignee = ChromeDriver.FindElementById("AcConsignee")
    '                        Dim Tasetco_Date_Received = ChromeDriver.FindElementById("EndDeliveryTime")

    '                        If Len(Tasetco_PO.Text) > 0 Then
    '                            GridView1.SetRowCellValue(i, "Tasetco_PO", Tasetco_PO.Text)
    '                        End If
    '                        If Len(Tasetco_TP.Text) > 0 Then
    '                            GridView1.SetRowCellValue(i, "Tasetco_TP", Tasetco_TP.Text)
    '                        End If
    '                        If Len(Tasetco_CustomerNameRC.Text) > 0 Then
    '                            GridView1.SetRowCellValue(i, "Tasetco_Cus_Name_RC", Tasetco_CustomerNameRC.Text)
    '                        End If
    '                        If Len(Tasetco_Address_RC.Text) > 0 Then
    '                            GridView1.SetRowCellValue(i, "Tasetco_Address_RC", Tasetco_Address_RC.Text)
    '                        End If
    '                        If Len(Tasetco_Status.Text) > 0 Then
    '                            GridView1.SetRowCellValue(i, "Tasetco_Status", Tasetco_Status.Text)
    '                        End If
    '                        If Len(Tasetco_Ac_Cosignee.Text) > 0 Then
    '                            GridView1.SetRowCellValue(i, "Tasetco_Ac_Cosignee", Tasetco_Ac_Cosignee.Text)
    '                        End If
    '                        If Len(Tasetco_Date_Received.Text) > 0 Then
    '                            GridView1.SetRowCellValue(i, "Tasetco_Date_Received", Tasetco_Date_Received.Text)
    '                        End If

    '                        If GridView1.GetRowCellValue(i, "Tasetco_Status").ToString() = "Giao hàng thành công" Then
    '                            GridView1.SetRowCellValue(i, "Final_Result", "Completed")
    '                        Else
    '                            GridView1.SetRowCellValue(i, "Final_Result", "Incomplete")
    '                        End If
    '                        GridView1.SetRowCellValue(i, "Tasetco_Date_Get_Data", "SYSTEM_" & Now().ToString("dd/MM/yyyy hh:mm:ss tt"))

    '                        str_update = "UPDATE " & table_name &
    '                             " SET [Tasetco_PO] = '" & GridView1.GetRowCellValue(i, "Tasetco_PO").ToString() & "'," &
    '                                " [Tasetco_TP] = '" & GridView1.GetRowCellValue(i, "Tasetco_TP").ToString() & "'," &
    '                                " [Tasetco_Cus_Name_RC] = '" & GridView1.GetRowCellValue(i, "Tasetco_Cus_Name_RC").ToString() & "'," &
    '                                " [Tasetco_Address_RC] = '" & GridView1.GetRowCellValue(i, "Tasetco_Address_RC").ToString() & "'," &
    '                                " [Tasetco_Status] = '" & GridView1.GetRowCellValue(i, "Tasetco_Status").ToString() & "'," &
    '                                " [Tasetco_Ac_Cosignee] = '" & GridView1.GetRowCellValue(i, "Tasetco_Ac_Cosignee").ToString() & "'," &
    '                                " [Tasetco_Date_Received] = '" & GridView1.GetRowCellValue(i, "Tasetco_Date_Received").ToString() & "'," &
    '                                " [Tasetco_Date_Get_Data] = '" & GridView1.GetRowCellValue(i, "Tasetco_Date_Get_Data").ToString() & "'," &
    '                                " [Final_Result] = '" & GridView1.GetRowCellValue(i, "Final_Result").ToString() & "'" &
    '                             " WHERE [Bill_Number] = '" & bill_number_to_search & "'"
    '                        Module_Letter_Management.SQL_QUERY(link_database, str_update)

    '                    End If
    '                Else
    '                    GridView1.SetRowCellValue(i, "Tasetco_Status", "Not found")
    '                    GridView1.SetRowCellValue(i, "Tasetco_Date_Get_Data", "SYSTEM_" & Now().ToString("dd/MM/yyyy hh:mm:ss tt"))
    '                    str_update = "UPDATE " & table_name &
    '                    " SET [Tasetco_Status] = '" & GridView1.GetRowCellValue(i, "Tasetco_Status").ToString() & "'" & "," &
    '                    " [Tasetco_Date_Get_Data] = '" & GridView1.GetRowCellValue(i, "Tasetco_Date_Get_Data").ToString() & "'" &
    '                    " WHERE [Bill_Number] = '" & bill_number_to_search & "'"
    '                    Module_Letter_Management.SQL_QUERY(link_database, str_update)
    '                End If
    '            Next
    '            ChromeDriver.Quit()
    '        End If

    '        If BarEditItem_VendorName.EditValue.ToString = "VIETTEL POST" Then
    '            WebBrowser1.Navigate("https://viettelpost.com.vn/")
    '            WaitForPageLoad()
    '            Do
    '                Application.DoEvents()
    '                StatusBar1.Caption = "Wait for load database of Viettel..."
    '            Loop Until WebBrowser1.DocumentTitle = "Trang Chủ - Bưu Chính Viettel"
    '            WebBrowser1.Navigate("https://viettelpost.com.vn/tra-cuu-don-hang")
    '            WaitForPageLoad()
    '            For i = 0 To GridView1.RowCount - 1
    '                Dim bill_number_to_search As String = GridView1.GetRowCellValue(i, "Bill_Number").ToString()
    '                StatusBar1.Caption = "[" & i + 1 & "/" & GridView1.RowCount & "] - Processing bill number " & bill_number_to_search
    '                Dim AllElementes As HtmlElementCollection = WebBrowser1.Document.All
    '                For Each webpageelement As HtmlElement In AllElementes
    '                    If webpageelement.GetAttribute("name") = "orders" Then
    '                        webpageelement.SetAttribute("value", bill_number_to_search)
    '                    End If
    '                Next

    '                WebBrowser1.Document.GetElementById("btn_trackorders__form").InvokeMember("click")

    '                'For Each webpageelement As HtmlElement In AllElementes
    '                '    If webpageelement.GetAttribute("className") = "button__inner" Then
    '                '        webpageelement.Focus()
    '                '        webpageelement.InvokeMember("click")
    '                '    End If
    '                'Next
    '                WaitForPageLoad()
    '                Dim status_found As Boolean = True
    '                For Each Element As HtmlElement In WebBrowser1.Document.GetElementsByTagName("strong")
    '                    If Element.GetAttribute("className") = "service__name" Then
    '                        If InStr(1, Element.InnerText, "Không tìm thấy vận đơn nào phù hợp với quý khách", vbBinaryCompare) > 0 Then
    '                            status_found = False
    '                            GridView1.SetRowCellValue(i, "Viettel_Status", "Not found")
    '                        End If
    '                    End If
    '                Next
    '                If status_found = True Then
    '                    For Each Element As HtmlElement In WebBrowser1.Document.GetElementsByTagName("tr")
    '                        If Element.GetAttribute("className") = "order__info" Then
    '                            For Each OptionElement As HtmlElement In Element.GetElementsByTagName("td")
    '                                If OptionElement.GetAttribute("className") = "order__weight" Then
    '                                    GridView1.SetRowCellValue(i, "Viettel_Size", OptionElement.InnerText)
    '                                End If
    '                                If OptionElement.GetAttribute("className") = "order__service" Then
    '                                    GridView1.SetRowCellValue(i, "Viettel_Service", OptionElement.InnerText)
    '                                End If
    '                                If OptionElement.GetAttribute("className") = "order__status" Then
    '                                    GridView1.SetRowCellValue(i, "Viettel_Status", OptionElement.InnerText)
    '                                End If
    '                            Next
    '                        End If

    '                        If Element.GetAttribute("className") = "order__detail" Then
    '                            For Each Element2 As HtmlElement In Element.GetElementsByTagName("ul")
    '                                If Element2.GetAttribute("className") = "trackorders__activity clearfix" Then
    '                                    For Each Element3 As HtmlElement In Element2.GetElementsByTagName("li")
    '                                        For Each Element4 As HtmlElement In Element3.GetElementsByTagName("time")
    '                                            If Element4.GetAttribute("className") = "datetime" Then
    '                                                If GridView1.GetRowCellValue(i, "Viettel_Details_Status").ToString().Length = 0 Then
    '                                                    GridView1.SetRowCellValue(i, "Viettel_Details_Status", Element4.InnerText)
    '                                                Else
    '                                                    GridView1.SetRowCellValue(i, "Viettel_Details_Status", GridView1.GetRowCellValue(i, "Viettel_Details_Status").ToString() & Chr(10) & Element4.InnerText)
    '                                                End If
    '                                                For Each Element5 As HtmlElement In Element3.GetElementsByTagName("span")
    '                                                    If Element5.GetAttribute("className") = "message" Then
    '                                                        GridView1.SetRowCellValue(i, "Viettel_Details_Status", GridView1.GetRowCellValue(i, "Viettel_Details_Status").ToString() & " " & Element5.InnerText)
    '                                                    End If
    '                                                Next
    '                                            End If
    '                                        Next
    '                                    Next
    '                                End If
    '                            Next
    '                        End If
    '                    Next
    '                    If GridView1.GetRowCellValue(i, "Viettel_Status").ToString() = "Phát thành công" Then
    '                        GridView1.SetRowCellValue(i, "Final_Result", "Completed")
    '                    End If
    '                End If
    '                GridView1.SetRowCellValue(i, "Viettel_Date_Get_Data", "SYSTEM_" & Now().ToString("dd/MM/yyyy hh:mm:ss tt"))
    '                str_update = "UPDATE " & table_name &
    '                             " SET [Viettel_Size] = '" & GridView1.GetRowCellValue(i, "Viettel_Size").ToString() & "'," &
    '                                " [Viettel_Service] = '" & GridView1.GetRowCellValue(i, "Viettel_Service").ToString() & "'," &
    '                                " [Viettel_Status] = '" & GridView1.GetRowCellValue(i, "Viettel_Status").ToString() & "'," &
    '                                " [Viettel_DateReceived] = '" & GridView1.GetRowCellValue(i, "Viettel_DateReceived").ToString() & "'," &
    '                                " [Viettel_Details_Status] = '" & GridView1.GetRowCellValue(i, "Viettel_Details_Status").ToString() & "'," &
    '                                " [Viettel_Date_Get_Data] = '" & Environ("username") & "_" & Now().ToString("dd/MM/yyyy hh:mm:ss tt") & "'," &
    '                                " [Final_Result] = '" & GridView1.GetRowCellValue(i, "Final_Result").ToString() & "'" &
    '                             " WHERE [Bill_Number] = '" & bill_number_to_search & "'"
    '                Module_Letter_Management.SQL_QUERY(link_database, str_update)
    '            Next
    '        End If
    '        delete_record_user_update_database()
    '        StatusBar1.Caption = "[Get Mailing Status] Completed !!!"
    '        StatusBar2.Caption = ""
    '        StatusBar3.Caption = ""
    '        Exit Sub
    'err_handle:
    '        StatusBar1.Caption = ""
    '        StatusBar3.Caption = ""
    '        StatusBar2.Caption = ""
    '        delete_record_user_update_database()
    '        Module_Letter_Management.Error_handle()
    '    End Sub
    Private Sub btLoad_Click(sender As Object, e As EventArgs) Handles btLoad.Click
        On Error GoTo err_handle
        If ComboBox_FilterDatabase.Text.Length = 0 Then Exit Sub
        Module_Letter_Management.LOAD_DATABASE_TASETCO_TO_GRIDVIEW(link_database, table_name, ComboBox_FilterDatabase.Text, "", GridControl1, GridView1)

        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub
    Private Sub tbStrSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles tbStrSearch.KeyDown
        On Error GoTo err_handle

        If e.KeyCode = System.Windows.Forms.Keys.Return Then

            ComboBox_FilterDatabase.Text = ""

            Dim SQL_Str_Search As String = ""

            Dim DT As DataTable = Module_Letter_Management.SQL_QUERY_TO_DATATABLE(link_database, "SELECT * FROM " & table_name & " LIMIT 1")
            For i = 0 To DT.Columns.Count - 1
                If SQL_Str_Search.Length = 0 Then
                    SQL_Str_Search = "[" & DT.Columns(i).ColumnName.ToString() & "] Like '%" & tbStrSearch.Text & "%' "
                Else
                    SQL_Str_Search = SQL_Str_Search & " OR [" & DT.Columns(i).ColumnName.ToString() & "] LIKE '%" & tbStrSearch.Text & "%' "
                End If
            Next
            DT.Clear()
            DT = Module_Letter_Management.SQL_QUERY_TO_DATATABLE(link_database, "SELECT * FROM " & table_name & " WHERE " & SQL_Str_Search)

            GridControl1.DataSource = DT
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Sub delete_record_user_update_database()
        Dim User_Updating_Database As String = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT User_Updating_Database FROM LIST_DATABASE WHERE Vendor_Name = '" & BarEditItem_VendorName.EditValue.ToString & "' AND Database_Name = '" & BarEditItem_DatabaseName.EditValue.ToString & "'")
        Dim Year_Updating_Database As String = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Year_Updating_Database FROM LIST_DATABASE WHERE Vendor_Name = '" & BarEditItem_VendorName.EditValue.ToString & "' AND Database_Name = '" & BarEditItem_DatabaseName.EditValue.ToString & "'")

        Year_Updating_Database = Replace(Year_Updating_Database, BarEditItem_Year.EditValue.ToString & ";", "")

        Year_Updating_Database = Replace(Year_Updating_Database, ";" & BarEditItem_Year.EditValue.ToString, "")

        Year_Updating_Database = Replace(Year_Updating_Database, BarEditItem_Year.EditValue.ToString, "")

        User_Updating_Database = Replace(User_Updating_Database, Environ("username") & ";", "")

        User_Updating_Database = Replace(User_Updating_Database, ";" & Environ("username"), "")

        User_Updating_Database = Replace(User_Updating_Database, Environ("username"), "")

        Dim str_update As String = "UPDATE LIST_DATABASE" &
                                     " SET [User_Updating_Database] = '" & User_Updating_Database & "'," &
                                        " [Year_Updating_Database] = '" & Year_Updating_Database & "'" &
                                     " WHERE Vendor_Name = '" & BarEditItem_VendorName.EditValue.ToString & "' AND [Database_Name] = '" & BarEditItem_DatabaseName.EditValue.ToString & "'"
        Module_Letter_Management.SQL_QUERY(link_application_config, str_update)
    End Sub

#Region "Page Loading Functions"
    Private Sub WaitForPageLoad()
        AddHandler WebBrowser1.DocumentCompleted, New WebBrowserDocumentCompletedEventHandler(AddressOf PageWaiter)
        While Not pageready
            Application.DoEvents()
        End While
        pageready = False
    End Sub

    Private Sub PageWaiter(ByVal sender As Object, ByVal e As WebBrowserDocumentCompletedEventArgs)
        If WebBrowser1.ReadyState = WebBrowserReadyState.Complete Then
            pageready = True
            RemoveHandler WebBrowser1.DocumentCompleted, New WebBrowserDocumentCompletedEventHandler(AddressOf PageWaiter)
        End If
    End Sub

#End Region
End Class