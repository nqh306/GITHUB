﻿Imports Microsoft.Office.Interop
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports System.Data.SQLite

Module Module1
    Public USERNAME As String = Environment.UserName
    Public link_folder_database As String = "D:\App_BanTongHop\database_bantonghop.txt"
    Public folder_backup As String = "D:\App_BanTongHop\BACKUP\"
    Public str_log_file As String = "D:\App_BanTongHop\LOG\IMPORT\import_log_" & Now.ToString("yyyyMMdd_hhmmss") & ".txt"

    Public Sub Console_WriteLine(strLogfile As String, strLog As String)
        Try
            Console.WriteLine(strLog)

            System.IO.File.AppendAllText(strLogfile, (DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") & Convert.ToString(" - ")) + strLog + vbLf + vbLf)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub Main()
        Console_WriteLine(str_log_file, "WELCOME TO ROBOTIC PROCESS APPLICATION [BAN TONG HOP - EVNGENCO1]")

        Dim currentCulture As System.Globalization.CultureInfo
        Dim tmpCurrentCulture As System.Globalization.CultureInfo
        tmpCurrentCulture = New System.Globalization.CultureInfo("en-US")
        currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        tmpCurrentCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"
        System.Threading.Thread.CurrentThread.CurrentCulture = tmpCurrentCulture

        If Connect_to_database() = True Then
            Console_WriteLine(str_log_file, "Connected to database...")
        Else
            Console_WriteLine(str_log_file, "Can't connect to database...")
            Exit Sub
        End If

        Dim ChromeDriver As ChromeDriver = Connect_To_eOffice()

        If ChromeDriver Is Nothing Then
            Console_WriteLine(str_log_file, "Can't connect to eOffice")
            Exit Sub
        Else
            Console_WriteLine(str_log_file, "Connected to eOffice")
        End If

        Dim str_body_email As String = ""
        Dim str_subject_email As String = ""

        'If LAY_THONG_TIN_VAN_BAN_PHAT_HANH(ChromeDriver) = False Then
        '    str_body_email = "Không thể lấy dữ liệu [VĂN BẢN PHÁT HÀNH] từ hệ thống eOffice - " & Now.ToString("dd/MM/yyyy hh:mm:ss")
        'Else
        '    If LAY_THONG_TIN_TO_TRINH(ChromeDriver) = False Then
        '        str_body_email = "Không thể lấy dữ liệu [XỬ LÝ TỜ TRÌNH] từ hệ thống eOffice - " & Now.ToString("dd/MM/yyyy hh:mm:ss")
        '    Else
        If LAYTHONGTIN_TOTRINH_DANGTHEODOI(ChromeDriver) = False Then
            str_body_email = "Không thể lấy dữ liệu [TỜ TRÌNH ĐANG THEO DÕI] từ hệ thống eOffice - " & Now.ToString("dd/MM/yyyy hh:mm:ss")
        Else
            str_body_email = "Lấy dữ liệu từ eOffice THÀNH CÔNG - " & Now.ToString("dd/MM/yyyy hh:mm:ss")
        End If
        'End If
        'End If

        ChromeDriver.Quit()

        Console_WriteLine(str_log_file, "Hoàn thành...")

        Dim OutlookApp As New Outlook.Application
        Dim olMail As Outlook.MailItem
        olMail = OutlookApp.CreateItem(0)
        With olMail
            .To = "huynq91@evngenco1.vn"
            .Subject = "[GetDatabase_eOffice] - Notification"
            .HTMLBody = str_body_email
            .Attachments.Add(str_log_file, Outlook.OlAttachmentType.olByValue)
            .Send()
        End With
        System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture
    End Sub

    Public Function Connect_to_database() As Boolean
        Try
            If Not My.Computer.FileSystem.FileExists(link_folder_database) Then
                SQLiteConnection.CreateFile(link_folder_database)
            Else
                If Not My.Computer.FileSystem.FileExists(folder_backup & "database_bantonghop_" & Now.ToString("yyyyMMdd") & ".txt") Then
                    My.Computer.FileSystem.CopyFile(link_folder_database, folder_backup & "database_bantonghop_" & Now.ToString("yyyyMMdd") & ".txt")
                End If
            End If

            SQL_QUERY(link_folder_database, False, "CREATE TABLE IF NOT EXISTS DATABASE_EOFFICE(CASEID VARCHAR NOT NULL, SOTOTRINH VARCHAR, NGAYTOTRINH VARCHAR, BANTRINH VARCHAR, NOIDUNGTRINH VARCHAR, SOVANBAN VARCHAR, SONGHIQUYET VARCHAR, NGAYNGHIQUYET VARCHAR, SOQUYETDINH_VANBAN VARCHAR, NGAYQUYETDINH_VANBAN VARCHAR, YKIEN_HDTV VARCHAR, NGAY_YKIEN_HDTV_GANNHAT VARCHAR, NGUOITHUCHIEN VARCHAR, NGAYTHUCHIEN VARCHAR, THOIGIANXULY INTEGER, GHICHU VARCHAR, LOG VARCHAR, STATUS VARCHAR, USER_CREATED VARCHAR, LAST_USER_MODIFIED VARCHAR, REMARKS VARCHAR, STATUS_DELETED VARCHAR)")

            SQL_QUERY(link_folder_database, False, "CREATE TABLE IF NOT EXISTS DATABASE_VANBAN_PHATHANH(CASEID VARCHAR NOT NULL, SOVANBAN VARCHAR, NGAYVANBAN DATE, TRICHYEU VARCHAR, NOINHAN VARCHAR, NGAYPHATHANH VARCHAR, NGUOIDANGKY VARCHAR, USER_CREATED VARCHAR, USER_MODIFIED VARCHAR, REMARK VARCHAR)")

            SQL_QUERY(link_folder_database, False, "CREATE TABLE IF NOT EXISTS DATABASE_TOTRINH_THEODOI(CASEID VARCHAR NOT NULL, SOTOTRINH VARCHAR, NGAYTOTRINH VARCHAR, BANTRINH VARCHAR, NOIDUNGTRINH VARCHAR, YKIEN_HDTV VARCHAR, NGAY_YKIEN_HDTV_GANNHAT VARCHAR, NGUOITHUCHIEN VARCHAR, GHICHU VARCHAR, STATUS VARCHAR, USER_CREATED VARCHAR, LAST_USER_MODIFIED VARCHAR, REMARKS VARCHAR)")

            Return True
        Catch ex As Exception
            Console_WriteLine(str_log_file, ex.Message)
            Return False
        End Try
    End Function

    Public Function Connect_To_eOffice() As ChromeDriver

        Try
            Dim ChromeDriver As ChromeDriver

            Dim service As ChromeDriverService = ChromeDriverService.CreateDefaultService()
            service.HideCommandPromptWindow = True
            Dim driver_option As ChromeOptions = New ChromeOptions
            driver_option.AddArgument("no-sandbox")
            driver_option.AddAdditionalCapability("useAutomationExtension", False)

            ChromeDriver = New ChromeDriver(service, driver_option, TimeSpan.FromMinutes(3))

            ChromeDriver.Navigate().GoToUrl("http://eoffice.evngenco1.vn/")

            Do

            Loop Until Selenium_Check_Element_Exist(ChromeDriver, By.Id("txtUserName")) = True


            ChromeDriver.FindElementById("txtUserName").SendKeys("huynq91")
            ChromeDriver.FindElementById("txtPassword").SendKeys("Admin@1")
            ChromeDriver.FindElementById("btclientLogin").Click()

            Return ChromeDriver
        Catch ex As Exception
            Console_WriteLine(str_log_file, ex.Message)
            Return Nothing
        End Try
    End Function

    Public Function LAY_THONG_TIN_VAN_BAN_PHAT_HANH(ChromeDriver As ChromeDriver) As Boolean
        Try
            ChromeDriver.Navigate().GoToUrl("https://eoffice.evngenco1.vn/vbdi_vanthu_phathanh.aspx")

            Dim js As IJavaScriptExecutor = TryCast(ChromeDriver, IJavaScriptExecutor)

            Dim check_load_all_record As String = Selenium_JavaScriptExecute_ToString(ChromeDriver, "var content =document.evaluate('/html/body/form/div[4]/div[2]/div/div[2]/div/div[7]/div/table/tbody/tr/td/table/tbody/tr/td/div[5]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML;return content;")

            js.ExecuteScript("var xPathRes = document.evaluate ('//*[@id=" & Chr(34) & "ctl00_cpmain_RadDropDownNoiPhatHanh" & Chr(34) & "]/span/span[2]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null);xPathRes.singleNodeValue.click();")
            js.ExecuteScript("var xPathRes = document.evaluate ('//*[@id=" & Chr(34) & "ctl00_cpmain_RadDropDownNoiPhatHanh_DropDown" & Chr(34) & "]/div/ul/li[6]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null);xPathRes.singleNodeValue.click();")

            Do

            Loop Until Selenium_JavaScriptExecute_ToString(ChromeDriver, "var content =document.evaluate('//*[@id=" & Chr(34) & "ctl00_cpmain_RadDropDownNoiPhatHanh" & Chr(34) & "]/span/span[1]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML;return content;") = "Ban Tổng hợp"


            js.ExecuteScript("var xPathRes = document.evaluate ('//*[@id=" & Chr(34) & "ctl00_cpmain_drdThangNam" & Chr(34) & "]/span/span[2]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null);xPathRes.singleNodeValue.click();")
            js.ExecuteScript("var xPathRes = document.evaluate ('//*[@id=" & Chr(34) & "ctl00_cpmain_drdThangNam_DropDown" & Chr(34) & "]/div/ul/li[1]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null);xPathRes.singleNodeValue.click();")

            Do

            Loop Until Selenium_JavaScriptExecute_ToString(ChromeDriver, "var content =document.evaluate('//*[@id=" & Chr(34) & "ctl00_cpmain_drdThangNam" & Chr(34) & "]/span/span[1]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML;return content;") = "Tất cả"

            Dim tongsorecord As String = ""
            Do
                Try
                    tongsorecord = Selenium_JavaScriptExecute_ToString(ChromeDriver, "var content =document.evaluate('/html/body/form/div[6]/div[2]/div/div[2]/div/div[7]/div/table/tbody/tr/td/table/tbody/tr/td/div[5]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML;return content;")
                Catch ex As Exception

                End Try

            Loop Until tongsorecord <> check_load_all_record And tongsorecord <> ""
            '/html/body/form/div[6]/div[2]/div/div[2]/div/div[7]/div/table/tbody/tr/td/table/tbody/tr/td/div[5]
            Dim tongsotrang As String = ""

            For i As Integer = 1 To Len(Replace(tongsorecord, " ", ""))
                If huynq_Substring(Replace(tongsorecord, " ", ""), i, 1) = "/" Then
                    For m = i + 1 To Len(Replace(tongsorecord, " ", ""))
                        If huynq_isnumeric(huynq_Substring(Replace(tongsorecord, " ", ""), m, 1)) = True Then
                            tongsotrang = tongsotrang & huynq_Substring(Replace(tongsorecord, " ", ""), m, 1)
                        Else
                            Exit For
                        End If
                    Next
                    Exit For
                End If
            Next

            For sotrang As Integer = 1 To CInt(tongsotrang)
                If sotrang > 1 Then
                    js.ExecuteScript("var xPathRes = document.evaluate ('/html/body/form/div[6]/div[2]/div/div[2]/div/div[7]/div/table/tbody/tr/td/table/tbody/tr/td/div[3]/input[1]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null);xPathRes.singleNodeValue.click();")
                    Do
                        '/html/body/form/div[4]/div[2]/div/div[2]/div/div[2]/div[6]/div[1]/div[2]/div/div/table/tbody/tr/td/table/tbody/tr/td/div[3]/input[1]

                    Loop Until Selenium_JavaScriptExecute_ToString(ChromeDriver, "var content = document.getElementsByClassName('rgCurrentPage')[0].children[0].innerHTML;;return content;") = Format(sotrang, "0")
                End If

                For i = 0 To 49
                    If Selenium_Check_Element_Exist(ChromeDriver, By.XPath("//*[@id='ctl00_cpmain_RadGrid_ctl00__" & i & "']/td[2]")) = True Then
                        Dim SOVANBAN As String = Selenium_Get_Text_Element(ChromeDriver, By.XPath("//*[@id='ctl00_cpmain_RadGrid_ctl00__" & i & "']/td[2]"))


                        If InStr(SOVANBAN, "/", CompareMethod.Binary) > 0 Then
                            Dim NGAYVANBAN As String = Selenium_Get_Text_Element(ChromeDriver, By.XPath("//*[@id='ctl00_cpmain_RadGrid_ctl00__" & i & "']/td[3]"))

                            'Dim NGAYVANBAN As Date = Format(DateTime.ParseExact(NGAYVANBAN2, "dd/MM/yyyy", Nothing), "dd/MM/yyyy")

                            If SQL_QUERY_TO_INTEGER(link_folder_database, "SELECT COUNT(*) FROM DATABASE_VANBAN_PHATHANH WHERE NGAYVANBAN = '" & NGAYVANBAN & "' AND [SOVANBAN] LIKE '%" & SOVANBAN & "%'") = 0 Then
                                Dim CASEID As String = "VB_" & Now.ToString("yyyyMMddhhmmss") & "_" & sotrang & "_" & i
                                Dim TRICHYEU As String = Selenium_Get_Text_Element(ChromeDriver, By.XPath("//*[@id='ctl00_cpmain_RadGrid_ctl00__" & i & "']/td[4]/div/div[2]"))
                                TRICHYEU = Replace(Replace(TRICHYEU, Chr(34), ""), "'", "")
                                Dim NOINHAN As String = Trim(Selenium_JavaScriptExecute_ToString(ChromeDriver, "var content =document.evaluate('//*[@id=" & Chr(34) & "ctl00_cpmain_RadGrid_ctl00__" & i & Chr(34) & "]/td[5]/div/a', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.title;return content;"))
                                NOINHAN = Replace(Replace(NOINHAN, Chr(34), ""), "'", "")
                                Dim NGAYPHATHANH As String = Selenium_Get_Text_Element(ChromeDriver, By.XPath("//*[@id='ctl00_cpmain_RadGrid_ctl00__" & i & "']/td[6]"))
                                NGAYPHATHANH = Replace(Replace(NGAYPHATHANH, Chr(34), ""), "'", "")
                                Dim NGUOIDANGKY As String = Selenium_Get_Text_Element(ChromeDriver, By.XPath("//*[@id='ctl00_cpmain_RadGrid_ctl00__" & i & "']/td[7]"))
                                NGUOIDANGKY = Replace(Replace(NGUOIDANGKY, Chr(34), ""), "'", "")

                                SQL_QUERY(link_folder_database, True, "INSERT INTO DATABASE_VANBAN_PHATHANH(CASEID, SOVANBAN, NGAYVANBAN, TRICHYEU, NOINHAN, NGAYPHATHANH, NGUOIDANGKY, USER_CREATED) " &
                                            "VALUES ('" & CASEID & "', '" & SOVANBAN & "', '" & NGAYVANBAN & "', '" & TRICHYEU & "', '" & NOINHAN & "', '" & NGAYPHATHANH & "', '" & NGUOIDANGKY & "', '" & Environment.UserName & "_" & Now.ToString("yyyy/MM/dd hh:mm:ss") & "')")
                            End If
                        End If
                    End If
                Next
            Next
            Console_WriteLine(str_log_file, "Lay thong tin van ban: Completed")
            Return True
        Catch ex As Exception
            Console_WriteLine(str_log_file, ex.Message)
            Return False
        End Try
    End Function

    Public Function LAY_THONG_TIN_TO_TRINH(ChromeDriver As ChromeDriver) As Boolean
        Try
            ChromeDriver.Navigate().GoToUrl("https://eoffice.evngenco1.vn/hdtv_smartbox.aspx?id=hdtv-vbde-ket")

            Do

            Loop Until Selenium_Check_Element_Exist(ChromeDriver, By.Id("ctl00_cpmain_ctl00_RadGrid_ctl00_ctl03_ctl01_PageSizeComboBox_Input")) = True

            Dim js As IJavaScriptExecutor = TryCast(ChromeDriver, IJavaScriptExecutor)
            js.ExecuteScript("document.getElementById('ctl00_cpmain_ctl00_RadGrid_ctl00_ctl03_ctl01_PageSizeComboBox_Input').setAttribute('value', '100')")

            For sotrang As Integer = 1 To 100
                If Selenium_Check_Element_Exist(ChromeDriver, By.XPath("//*[@id='ctl00_cpmain_ctl00_RadGrid_ctl00_Pager']/tbody/tr/td/table/tbody/tr/td/div[2]/a[" & sotrang & "]/span")) = True Then
                    If sotrang > 1 Then
                        js.ExecuteScript("var xPathRes = document.evaluate ('/html/body/form/div[4]/div[2]/div/div[2]/div/div[2]/div[6]/div[1]/div[2]/div/div/table/tbody/tr/td/table/tbody/tr/td/div[2]/a[" & sotrang & "]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null);xPathRes.singleNodeValue.click();")

                        Do

                        Loop Until Selenium_JavaScriptExecute_ToString(ChromeDriver, "var content = document.getElementsByClassName('rgCurrentPage')[0].children[0].innerHTML;;return content;") = Format(sotrang, "0")

                    End If

                    For i As Integer = 0 To 99
                        Dim ELEMENT_ID As String = "ctl00_cpmain_ctl00_RadGrid_ctl00__" & i

                        If Selenium_Check_Element_Exist(ChromeDriver, By.Id(ELEMENT_ID)) = True Then
                            Dim SOTOTRINH As String = Trim(Selenium_JavaScriptExecute_ToString(ChromeDriver, "var content = document.getElementById('ctl00_cpmain_ctl00_RadGrid_ctl00_ctl" & Format(4 + (i * 3), "00") & "_lbl_SOKYHIEU').innerHTML; return content;"))
                            Dim NGAYTOTRINH As Date = Format(DateTime.ParseExact(ChromeDriver.FindElementByXPath("//*[@id='" & ELEMENT_ID & "']/td[5]").Text, "dd/MM/yyyy", Nothing), "dd/MM/yyyy")

                            If SOTOTRINH.Length > 0 Then
                                If SQL_QUERY_TO_INTEGER(link_folder_database, "SELECT COUNT(*) FROM DATABASE_EOFFICE WHERE (STATUS_DELETED = '' OR STATUS_DELETED IS NULL OR STATUS_DELETED = 'Vĩnh viễn xóa, Không cần tự động lấy lại') AND NGAYTOTRINH = '" & NGAYTOTRINH & "' AND [SOTOTRINH] = '" & SOTOTRINH & "'") = 0 Then
                                    js.ExecuteScript("document.getElementById('" & ELEMENT_ID & "').click();")


                                    Dim element_check_click_success As String = "//*[@id='ctl00_cpmain_ctl00_RadGrid_ctl00_ctl" & Format(((i * 3) + 6), "00") & "_pnThongTinVanBan']/div[1]/table/tbody/tr/td[1]/div[1]/div/b"
                                    Do

                                    Loop Until Selenium_Check_Element_Exist(ChromeDriver, By.XPath(element_check_click_success))

                                    Dim CASEID As String = Now.ToString("yyyyMMddhhmmss") & "_" & sotrang & "_" & i

                                    Dim BANTRINH As String = ChromeDriver.FindElementByXPath("//*[@id='" & ELEMENT_ID & "']/td[2]/strong").Text
                                    Dim NOIDUNGTRINH As String = ChromeDriver.FindElementByXPath("//*[@id='" & ELEMENT_ID & "']/td[2]/div").Text

                                    Dim SOVANBAN As String = Trim(Selenium_JavaScriptExecute_ToString(ChromeDriver, "var content =document.evaluate('//*[@id=" & Chr(34) & "ctl00_cpmain_ctl00_RadGrid_ctl00_ctl" & Format(((i * 3) + 6), "00") & "_pnThongTinVanBan" & Chr(34) & "]/div[1]/table/tbody/tr/td[1]/div[1]/div/a', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML;return content;"))

                                    '//*[@id="ctl00_cpmain_ctl00_RadGrid_ctl00_ctl09_pnThongTinVanBan"]/div[1]/table/tbody/tr/td[1]/div[1]/div/a



                                    Dim SONGHIQUYET As String = ""
                                    Dim SOQUYETDINH_VANBAN As String = ""

                                    If InStr(SOVANBAN, "/NQ", CompareMethod.Binary) > 0 Then
                                        SONGHIQUYET = Trim(SOVANBAN)
                                    Else
                                        If InStr(SOVANBAN, "/", CompareMethod.Binary) > 0 And InStr(UCase(SOVANBAN), "V/V", CompareMethod.Binary) = 0 Then
                                            SOQUYETDINH_VANBAN = Trim(SOVANBAN)
                                        End If
                                    End If


                                    For j As Integer = 2 To 10
                                        Dim SOVANBAN_CON As String = Trim(Selenium_JavaScriptExecute_ToString(ChromeDriver, "var content =document.evaluate('//*[@id=" & Chr(34) & "ctl00_cpmain_ctl00_RadGrid_ctl00_ctl" & Format(((i * 3) + 6), "00") & "_pnThongTinVanBan" & Chr(34) & "]/div[1]/table/tbody/tr/td[1]/div[1]/div/a[" & j & "]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML;return content;"))
                                        If SOVANBAN_CON.Length > 0 Then
                                            If SOVANBAN.Length = 0 Then
                                                SOVANBAN = Trim(SOVANBAN_CON)
                                            Else
                                                SOVANBAN = SOVANBAN & ";" & Trim(SOVANBAN_CON)
                                            End If

                                            If InStr(SOVANBAN_CON, "/NQ", CompareMethod.Binary) > 0 Then
                                                If SONGHIQUYET.Length = 0 Then
                                                    SONGHIQUYET = Trim(SOVANBAN_CON)
                                                Else
                                                    SONGHIQUYET = SONGHIQUYET & ";" & Trim(SOVANBAN_CON)
                                                End If
                                            Else
                                                If InStr(SOVANBAN_CON, "/", CompareMethod.Binary) > 0 And InStr(UCase(SOVANBAN_CON), "V/V", CompareMethod.Binary) = 0 Then
                                                    If SOQUYETDINH_VANBAN.Length = 0 Then
                                                        SOQUYETDINH_VANBAN = SOVANBAN_CON
                                                    Else
                                                        SOQUYETDINH_VANBAN = SOQUYETDINH_VANBAN & ";" & SOVANBAN_CON
                                                    End If
                                                End If
                                            End If

                                        Else
                                            Exit For
                                        End If
                                    Next
                                    Dim NGAYNGHIQUYET As Date = Format(DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", Nothing), "dd/MM/yyyy")
                                    Dim NGAYQUYETDINH_VANBAN As Date = Format(DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", Nothing), "dd/MM/yyyy")
                                    Dim NGAYTHUCHIEN As Date = Format(DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", Nothing), "dd/MM/yyyy")

                                    If SONGHIQUYET.Length > 0 Then
                                        Dim DT_NGAYNGHIQUYET As DataTable = SQL_QUERY_TO_DATATABLE(link_folder_database, "SELECT NGAYVANBAN FROM DATABASE_VANBAN_PHATHANH WHERE SOVANBAN = '" & SONGHIQUYET & "'")
                                        If DT_NGAYNGHIQUYET.Rows.Count > 0 Then
                                            For Each DRR As DataRow In DT_NGAYNGHIQUYET.Rows

                                                If DRR("NGAYVANBAN") > NGAYNGHIQUYET Then
                                                    NGAYNGHIQUYET = Format(DRR("NGAYVANBAN"), "dd/MM/yyyy")
                                                End If
                                            Next
                                        End If
                                    End If

                                    If SOQUYETDINH_VANBAN.Length > 0 Then
                                        Dim DT_NGAYQUYETDINH As DataTable = SQL_QUERY_TO_DATATABLE(link_folder_database, "SELECT NGAYVANBAN FROM DATABASE_VANBAN_PHATHANH WHERE SOVANBAN = '" & SOQUYETDINH_VANBAN & "'")
                                        If DT_NGAYQUYETDINH.Rows.Count > 0 Then
                                            For Each DRR As DataRow In DT_NGAYQUYETDINH.Rows
                                                If DRR("NGAYVANBAN") > NGAYQUYETDINH_VANBAN Then
                                                    NGAYQUYETDINH_VANBAN = Format(DRR("NGAYVANBAN"), "dd/MM/yyyy")
                                                End If
                                            Next
                                        End If

                                    End If

                                    If NGAYNGHIQUYET > NGAYQUYETDINH_VANBAN Then
                                        NGAYTHUCHIEN = NGAYNGHIQUYET
                                    Else
                                        NGAYTHUCHIEN = NGAYQUYETDINH_VANBAN
                                    End If

                                    Dim THOIGIANXULY As Integer
                                    If NGAYTHUCHIEN = Format(DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", Nothing), "dd/MM/yyyy") Then
                                    Else
                                        THOIGIANXULY = CInt(NGAYTHUCHIEN.Subtract(NGAYTOTRINH).Days)
                                    End If

                                    Dim NGUOITHUCHIEN As String = ""
                                    Dim GHICHU As String = ""


                                    For j As Integer = 1 To 10
                                        NGUOITHUCHIEN = Trim(Selenium_JavaScriptExecute_ToString(ChromeDriver, "var content =document.evaluate('//*[@id=" & Chr(34) & "ctl00_cpmain_ctl00_RadGrid_ctl00_ctl" & Format(((i * 3) + 6), "00") & "_pnThongTinVanBan" & Chr(34) & "]/table/tbody/tr[" & j & "]/td[2]/div', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML;return content;"))

                                        Select Case NGUOITHUCHIEN
                                            Case "Hoàng Văn Long"
                                                GHICHU = Trim(Selenium_JavaScriptExecute_ToString(ChromeDriver, "var content =document.evaluate('//*[@id=" & Chr(34) & "ctl00_cpmain_ctl00_RadGrid_ctl00_ctl" & Format(((i * 3) + 6), "00") & "_pnThongTinVanBan" & Chr(34) & "]/table/tbody/tr[" & j & "]/td[3]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML;;return content;"))
                                                GHICHU = Replace(GHICHU, "&nbsp;", "")
                                                Exit For
                                            Case "Nguyễn Quang Huy"
                                                GHICHU = Trim(Selenium_JavaScriptExecute_ToString(ChromeDriver, "var content =document.evaluate('//*[@id=" & Chr(34) & "ctl00_cpmain_ctl00_RadGrid_ctl00_ctl" & Format(((i * 3) + 6), "00") & "_pnThongTinVanBan" & Chr(34) & "]/table/tbody/tr[" & j & "]/td[3]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML;;return content;"))
                                                GHICHU = Replace(GHICHU, "&nbsp;", "")
                                                Exit For
                                            Case "Hoàng Văn Đạt"
                                                GHICHU = Trim(Selenium_JavaScriptExecute_ToString(ChromeDriver, "var content =document.evaluate('//*[@id=" & Chr(34) & "ctl00_cpmain_ctl00_RadGrid_ctl00_ctl" & Format(((i * 3) + 6), "00") & "_pnThongTinVanBan" & Chr(34) & "]/table/tbody/tr[" & j & "]/td[3]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML;;return content;"))
                                                GHICHU = Replace(GHICHU, "&nbsp;", "")
                                                Exit For
                                            Case "Trương Thị Huyền Trang"
                                                GHICHU = Trim(Selenium_JavaScriptExecute_ToString(ChromeDriver, "var content =document.evaluate('//*[@id=" & Chr(34) & "ctl00_cpmain_ctl00_RadGrid_ctl00_ctl" & Format(((i * 3) + 6), "00") & "_pnThongTinVanBan" & Chr(34) & "]/table/tbody/tr[" & j & "]/td[3]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML;;return content;"))
                                                GHICHU = Replace(GHICHU, "&nbsp;", "")
                                                Exit For
                                            Case "Ngô Quốc Huy"
                                                GHICHU = Trim(Selenium_JavaScriptExecute_ToString(ChromeDriver, "var content =document.evaluate('//*[@id=" & Chr(34) & "ctl00_cpmain_ctl00_RadGrid_ctl00_ctl" & Format(((i * 3) + 6), "00") & "_pnThongTinVanBan" & Chr(34) & "]/table/tbody/tr[" & j & "]/td[3]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML;;return content;"))
                                                GHICHU = Replace(GHICHU, "&nbsp;", "")
                                                Exit For
                                        End Select
                                    Next

                                    If Not GHICHU Is Nothing Then
                                        If GHICHU.Length > 0 Then
                                            GHICHU = Edit_GhiChu_BanTongHop(GHICHU)
                                        End If
                                    End If

                                    Dim YKIEN_HDTV As String = ""
                                    Dim NGAY_YKIEN_HDTV_GANNHAT As Date = "01/01/1900"

                                    For stt_hdtv As Integer = 1 To 100
                                        If Selenium_Check_Element_Exist(ChromeDriver, By.XPath("//*[@id='" & ELEMENT_ID & "']/td[3]/table/tbody/tr[" & stt_hdtv & "]/td[1]")) = True Then
                                            Dim TVHDTV_NAME As String = ChromeDriver.FindElementByXPath("//*[@id='" & ELEMENT_ID & "']/td[3]/table/tbody/tr[" & stt_hdtv & "]/td[1]").Text
                                            Dim YKIEN_TVHDTV As String = Selenium_JavaScriptExecute_ToString(ChromeDriver, "var content =document.evaluate('/html/body/form/div[4]/div[2]/div/div[2]/div/div[2]/div[6]/div[1]/div[2]/div/div/div[2]/table/tbody/tr[" & Format(((i + 1) * 2) - 1, "0") & "]/td[3]/table/tbody/tr[" & stt_hdtv & "]/td[2]/p', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML;return content;")

                                            If YKIEN_HDTV.Length = 0 Then
                                                YKIEN_HDTV = "- " & TVHDTV_NAME & Chr(10) & YKIEN_TVHDTV
                                            Else
                                                YKIEN_HDTV = YKIEN_HDTV & Chr(10) & "- " & TVHDTV_NAME & Chr(10) & YKIEN_TVHDTV
                                            End If

                                            If YKIEN_TVHDTV.Length > 0 Then
                                                Dim NGAYYKIEN As String = ""

                                                For m As Integer = Len(YKIEN_TVHDTV) To 1 Step -1
                                                    If huynq_Substring(YKIEN_TVHDTV, m, Len(TVHDTV_NAME) + 1) = "[" & TVHDTV_NAME Then
                                                        For n As Integer = m To Len(YKIEN_TVHDTV)
                                                            If huynq_isnumeric(huynq_Substring(YKIEN_TVHDTV, n, 1)) = True Then
                                                                NGAYYKIEN = huynq_Substring(YKIEN_TVHDTV, n, 10)
                                                                Exit For
                                                            End If
                                                        Next
                                                    End If
                                                    If NGAYYKIEN.Length > 0 Then
                                                        If Format(DateTime.ParseExact(NGAYYKIEN, "dd/MM/yyyy", Nothing), "dd/MM/yyyy") > NGAY_YKIEN_HDTV_GANNHAT Then
                                                            NGAY_YKIEN_HDTV_GANNHAT = Format(DateTime.ParseExact(NGAYYKIEN, "dd/MM/yyyy", Nothing), "dd/MM/yyyy")
                                                        End If
                                                        Exit For
                                                    End If
                                                Next
                                            End If

                                            YKIEN_HDTV = Replace(YKIEN_HDTV, "<br>", " ")

                                        Else
                                            Exit For
                                        End If
                                    Next

                                    Dim STR_LOG As String = USERNAME & "_" & Now.ToString("yyyy/MM/dd hh:mm:ss") & ": Khởi tạo bản ghi"

                                    Dim STR_STATUS As String = ""
                                    Dim STR_REMARKS As String = ""

                                    Dim sovanban_check As String = SONGHIQUYET & SOQUYETDINH_VANBAN

                                    If sovanban_check.Length = 0 Then
                                        STR_STATUS = "Chờ giải trình"
                                        STR_REMARKS = "Không ban hành Nghị quyết/Văn bản"
                                    Else
                                        If THOIGIANXULY < 0 Then
                                            STR_STATUS = "Chờ giải trình"
                                            STR_REMARKS = "Thời gian xử lý không đạt yêu cầu"
                                        Else
                                            If THOIGIANXULY > 15 Then
                                                STR_STATUS = "Chờ giải trình"
                                                STR_REMARKS = "Thời gian xử lý không đạt yêu cầu"
                                            Else
                                                If (SONGHIQUYET.Length > 0 And NGAYNGHIQUYET = Format(DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", Nothing), "dd/MM/yyyy")) Or (SOQUYETDINH_VANBAN.Length > 0 And NGAYQUYETDINH_VANBAN = Format(DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", Nothing), "dd/MM/yyyy")) Then
                                                    STR_STATUS = "Chờ giải trình"
                                                    STR_REMARKS = "Không tìm thấy thông tin văn bản đã ban hành"
                                                Else
                                                    STR_STATUS = "Chờ xử lý"
                                                End If
                                            End If
                                        End If
                                    End If

                                    SOTOTRINH = Replace(Replace(SOTOTRINH, Chr(34), ""), "'", "")
                                    BANTRINH = Replace(Replace(BANTRINH, Chr(34), ""), "'", "")
                                    NOIDUNGTRINH = Replace(Replace(NOIDUNGTRINH, Chr(34), ""), "'", "")
                                    YKIEN_HDTV = Replace(Replace(YKIEN_HDTV, Chr(34), ""), "'", "")
                                    GHICHU = Replace(Replace(GHICHU, Chr(34), ""), "'", "")

                                    If NGAYNGHIQUYET = Format(DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", Nothing), "dd/MM/yyyy") Then
                                        If NGAYQUYETDINH_VANBAN = Format(DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", Nothing), "dd/MM/yyyy") Then
                                            SQL_QUERY(link_folder_database, True, "INSERT INTO DATABASE_EOFFICE(CASEID, SOTOTRINH, NGAYTOTRINH, BANTRINH, NOIDUNGTRINH, SOVANBAN, SONGHIQUYET, SOQUYETDINH_VANBAN, YKIEN_HDTV, NGAY_YKIEN_HDTV_GANNHAT, NGUOITHUCHIEN, GHICHU, LOG, STATUS, USER_CREATED, LAST_USER_MODIFIED, REMARKS) " &
                                                        "VALUES ('" & CASEID & "', '" & SOTOTRINH & "', '" & NGAYTOTRINH & "', '" & BANTRINH & "', '" & NOIDUNGTRINH & "', '" & SOVANBAN & "', '" & SONGHIQUYET & "', '" & SOQUYETDINH_VANBAN & "', '" & YKIEN_HDTV & "', '" & NGAY_YKIEN_HDTV_GANNHAT & "', '" & NGUOITHUCHIEN & "', '" & GHICHU & "', '" & STR_LOG & "', '" & STR_STATUS & "', '" & USERNAME & "_" & Now.ToString("yyyy/MM/dd hh:mm:ss") & "', '', '" & STR_REMARKS & "')")
                                        Else
                                            SQL_QUERY(link_folder_database, True, "INSERT INTO DATABASE_EOFFICE(CASEID, SOTOTRINH, NGAYTOTRINH, BANTRINH, NOIDUNGTRINH, SOVANBAN, SONGHIQUYET, SOQUYETDINH_VANBAN, NGAYQUYETDINH_VANBAN, YKIEN_HDTV, NGAY_YKIEN_HDTV_GANNHAT, NGUOITHUCHIEN, NGAYTHUCHIEN, THOIGIANXULY, GHICHU, LOG, STATUS, USER_CREATED, LAST_USER_MODIFIED, REMARKS) " &
                                                        "VALUES ('" & CASEID & "', '" & SOTOTRINH & "', '" & NGAYTOTRINH & "', '" & BANTRINH & "', '" & NOIDUNGTRINH & "', '" & SOVANBAN & "', '" & SONGHIQUYET & "', '" & SOQUYETDINH_VANBAN & "', '" & NGAYQUYETDINH_VANBAN & "', '" & YKIEN_HDTV & "', '" & NGAY_YKIEN_HDTV_GANNHAT & "', '" & NGUOITHUCHIEN & "', '" & NGAYTHUCHIEN & "', " & THOIGIANXULY & ", '" & GHICHU & "', '" & STR_LOG & "', '" & STR_STATUS & "', '" & USERNAME & "_" & Now.ToString("yyyy/MM/dd hh:mm:ss") & "', '', '" & STR_REMARKS & "')")
                                        End If
                                    Else
                                        If NGAYQUYETDINH_VANBAN = Format(DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", Nothing), "dd/MM/yyyy") Then
                                            SQL_QUERY(link_folder_database, True, "INSERT INTO DATABASE_EOFFICE(CASEID, SOTOTRINH, NGAYTOTRINH, BANTRINH, NOIDUNGTRINH, SOVANBAN, SONGHIQUYET, NGAYNGHIQUYET, SOQUYETDINH_VANBAN, YKIEN_HDTV, NGAY_YKIEN_HDTV_GANNHAT, NGUOITHUCHIEN, NGAYTHUCHIEN, THOIGIANXULY, GHICHU, LOG, STATUS, USER_CREATED, LAST_USER_MODIFIED, REMARKS) " &
                                                        "VALUES ('" & CASEID & "', '" & SOTOTRINH & "', '" & NGAYTOTRINH & "', '" & BANTRINH & "', '" & NOIDUNGTRINH & "', '" & SOVANBAN & "', '" & SONGHIQUYET & "', '" & NGAYNGHIQUYET & "', '" & SOQUYETDINH_VANBAN & "', '" & YKIEN_HDTV & "', '" & NGAY_YKIEN_HDTV_GANNHAT & "', '" & NGUOITHUCHIEN & "', '" & NGAYTHUCHIEN & "', " & THOIGIANXULY & ", '" & GHICHU & "', '" & STR_LOG & "', '" & STR_STATUS & "', '" & USERNAME & "_" & Now.ToString("yyyy/MM/dd hh:mm:ss") & "', '', '" & STR_REMARKS & "')")
                                        Else
                                            SQL_QUERY(link_folder_database, True, "INSERT INTO DATABASE_EOFFICE(CASEID, SOTOTRINH, NGAYTOTRINH, BANTRINH, NOIDUNGTRINH, SOVANBAN, SONGHIQUYET, NGAYNGHIQUYET, SOQUYETDINH_VANBAN, NGAYQUYETDINH_VANBAN, YKIEN_HDTV, NGAY_YKIEN_HDTV_GANNHAT, NGUOITHUCHIEN, NGAYTHUCHIEN, THOIGIANXULY, GHICHU, LOG, STATUS, USER_CREATED, LAST_USER_MODIFIED, REMARKS) " &
                                                        "VALUES ('" & CASEID & "', '" & SOTOTRINH & "', '" & NGAYTOTRINH & "', '" & BANTRINH & "', '" & NOIDUNGTRINH & "', '" & SOVANBAN & "', '" & SONGHIQUYET & "', '" & NGAYNGHIQUYET & "', '" & SOQUYETDINH_VANBAN & "', '" & NGAYQUYETDINH_VANBAN & "', '" & YKIEN_HDTV & "', '" & NGAY_YKIEN_HDTV_GANNHAT & "', '" & NGUOITHUCHIEN & "', '" & NGAYTHUCHIEN & "', " & THOIGIANXULY & ", '" & GHICHU & "', '" & STR_LOG & "', '" & STR_STATUS & "', '" & USERNAME & "_" & Now.ToString("yyyy/MM/dd hh:mm:ss") & "', '', '" & STR_REMARKS & "')")
                                        End If
                                    End If
                                End If
                            End If
                        Else
                            Exit For
                        End If
                    Next
                Else
                    Exit For
                End If
            Next
            Return True
        Catch ex As Exception
            Console_WriteLine(str_log_file, ex.Message)
            Return False
        End Try
    End Function

    Public Function LAYTHONGTIN_TOTRINH_DANGTHEODOI(ChromeDriver As ChromeDriver) As Boolean

        Try
            SQL_QUERY(link_folder_database, True, "DELETE FROM DATABASE_TOTRINH_THEODOI")
            SQL_QUERY(link_folder_database, False, "vacumm")
            Dim STR_URL As String = ""
            Dim STR_PERSON As String = ""
            For i = 1 To 5
                Select Case i
                    Case 1
                        STR_URL = "https://eoffice.evngenco1.vn/hdtv_smartbox.aspx?id=hdtv-vbde-td-252"
                        STR_PERSON = "Hoàng Văn Long"
                    Case 2
                        STR_URL = "https://eoffice.evngenco1.vn/hdtv_smartbox.aspx?id=hdtv-vbde-td-468"
                        STR_PERSON = "Nguyễn Quang Huy"
                    Case 3
                        STR_URL = "https://eoffice.evngenco1.vn/hdtv_smartbox.aspx?id=hdtv-vbde-td-425"
                        STR_PERSON = "Trương Thị Huyền Trang"
                    Case 4
                        STR_URL = "https://eoffice.evngenco1.vn/hdtv_smartbox.aspx?id=hdtv-vbde-td-456"
                        STR_PERSON = "Ngô Quốc Huy"
                    Case 5
                        STR_URL = "https://eoffice.evngenco1.vn/hdtv_smartbox.aspx?id=hdtv-vbde-td-442"
                        STR_PERSON = "Hoàng Văn Đạt"
                End Select

                ChromeDriver.Navigate().GoToUrl(STR_URL)

                Do
                    Dim check_load_page As String = UCase(Selenium_JavaScriptExecute_ToString(ChromeDriver, "var content =document.evaluate('//*[@id=" & Chr(34) & "breadcrumbs" & Chr(34) & "]/ul/li[3]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML;return content;"))
                    If check_load_page.Contains(UCase(STR_PERSON)) = True Then
                        Exit Do
                    End If
                Loop

                'Chưa lập phiếu
                ChromeDriver.FindElementById("cpmain_ctl00_lbtnChuaLapPhieu").Click()

                If Selenium_JavaScriptExecute_ToString(ChromeDriver, "var content =document.evaluate('//*[@id=" & Chr(34) & "ctl00_cpmain_ctl00_RadGrid_ctl00" & Chr(34) & "]/tbody/tr/td/div', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML;return content;") <> "No records to display." Then
                    For j = 0 To 99
                        Dim elementID As String = "ctl00_cpmain_ctl00_RadGrid_ctl00__" & j
                        If Selenium_Check_Element_Exist(ChromeDriver, By.Id(elementID)) = False Then
                            Exit For
                        Else
                            Dim CASEID As String = Now.ToString("yyyyMMddhhmmss") & "_" & i & "_" & j
                            Dim SOTOTRINH As String = ChromeDriver.FindElementById("ctl00_cpmain_ctl00_RadGrid_ctl00_ctl" & Format((j * 3) + 4, "00") & "_lbl_SOKYHIEU").Text
                            Dim NGAYTRINH As String = ChromeDriver.FindElementByXPath("//*[@id='ctl00_cpmain_ctl00_RadGrid_ctl00__" & j & "']/td[4]").Text
                            Dim BANTRINH As String = ChromeDriver.FindElementById("ctl00_cpmain_ctl00_RadGrid_ctl00_ctl" & Format((j * 3) + 4, "00") & "_lbl_c6").Text
                            Dim NOIDUNGTRINH As String = ChromeDriver.FindElementById(elementID).Text

                            Dim YKIEN_HDTV As String = ""
                            Dim NGAY_YKIEN_HDTV_GANNHAT As Date = "01/01/1900"

                            For stt_hdtv As Integer = 1 To 100
                                If Selenium_Check_Element_Exist(ChromeDriver, By.XPath("//*[@id='ctl00_cpmain_ctl00_RadGrid_ctl00__" & j & "']/td[2]/table/tbody/tr[" & stt_hdtv & "]/td[1]")) = True Then
                                    Dim TVHDTV_NAME As String = ChromeDriver.FindElementByXPath("//*[@id='ctl00_cpmain_ctl00_RadGrid_ctl00__" & j & "']/td[2]/table/tbody/tr[" & stt_hdtv & "]/td[1]").Text
                                    Dim YKIEN_TVHDTV As String = Selenium_JavaScriptExecute_ToString(ChromeDriver, "var content =document.evaluate('//*[@id=" & Chr(34) & "ctl00_cpmain_ctl00_RadGrid_ctl00__" & j & Chr(34) & "]/td[2]/table/tbody/tr[" & stt_hdtv & "]/td[2]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML;return content;")

                                    If YKIEN_HDTV.Length = 0 Then
                                        YKIEN_HDTV = "- " & TVHDTV_NAME & Chr(10) & YKIEN_TVHDTV
                                    Else
                                        YKIEN_HDTV = YKIEN_HDTV & Chr(10) & "- " & TVHDTV_NAME & Chr(10) & YKIEN_TVHDTV
                                    End If

                                    If YKIEN_TVHDTV.Length > 0 Then
                                        Dim NGAYYKIEN As String = ""

                                        For m As Integer = Len(YKIEN_TVHDTV) To 1 Step -1
                                            If huynq_Substring(YKIEN_TVHDTV, m, Len(TVHDTV_NAME) + 1) = "[" & TVHDTV_NAME Then
                                                For n As Integer = m To Len(YKIEN_TVHDTV)
                                                    If huynq_isnumeric(huynq_Substring(YKIEN_TVHDTV, n, 1)) = True Then
                                                        NGAYYKIEN = huynq_Substring(YKIEN_TVHDTV, n, 10)
                                                        Exit For
                                                    End If
                                                Next
                                            End If
                                            If NGAYYKIEN.Length > 0 Then
                                                If Format(DateTime.ParseExact(NGAYYKIEN, "dd/MM/yyyy", Nothing), "dd/MM/yyyy") > NGAY_YKIEN_HDTV_GANNHAT Then
                                                    NGAY_YKIEN_HDTV_GANNHAT = Format(DateTime.ParseExact(NGAYYKIEN, "dd/MM/yyyy", Nothing), "dd/MM/yyyy")
                                                End If
                                                Exit For
                                            End If
                                        Next
                                    End If

                                    YKIEN_HDTV = Replace(YKIEN_HDTV, "<br>", " ")

                                Else
                                    Exit For
                                End If
                            Next


                            SQL_QUERY(link_folder_database, True, "INSERT INTO DATABASE_TOTRINH_THEODOI(CASEID, SOTOTRINH, NGAYTOTRINH, BANTRINH, NOIDUNGTRINH, YKIEN_HDTV, NGAY_YKIEN_HDTV_GANNHAT, NGUOITHUCHIEN, STATUS, USER_CREATED) " &
                                                        "VALUES ('" & CASEID & "', '" & SOTOTRINH & "', '" & NGAYTRINH & "', '" & BANTRINH & "', '" & NOIDUNGTRINH & "', '" & YKIEN_HDTV & "', '" & NGAY_YKIEN_HDTV_GANNHAT & "', '" & STR_PERSON & "', 'Chưa lập phiếu', '" & USERNAME & "_" & Now.ToString("yyyy/MM/dd hh:mm:ss") & "')")


                        End If
                    Next
                End If


                'Đang theo dõi
                ChromeDriver.FindElementById("cpmain_ctl00_lbtnTheoDoi").Click()
                If Selenium_JavaScriptExecute_ToString(ChromeDriver, "var content =document.evaluate('//*[@id=" & Chr(34) & "ctl00_cpmain_ctl00_RadGrid_ctl00" & Chr(34) & "]/tbody/tr/td/div', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML;return content;") <> "No records to display." Then
                    For j = 0 To 99
                        Dim elementID As String = "ctl00_cpmain_ctl00_RadGrid_ctl00__" & j
                        If Selenium_Check_Element_Exist(ChromeDriver, By.Id(elementID)) = False Then
                            Exit For
                        Else
                            Dim CASEID As String = Now.ToString("yyyyMMddhhmmss") & "_" & i & "_" & j
                            Dim SOTOTRINH As String = ChromeDriver.FindElementById("ctl00_cpmain_ctl00_RadGrid_ctl00_ctl" & Format((j * 3) + 4, "00") & "_lbl_SOKYHIEU").Text
                            Dim NGAYTRINH As String = ChromeDriver.FindElementByXPath("//*[@id='ctl00_cpmain_ctl00_RadGrid_ctl00__" & j & "']/td[4]").Text
                            Dim BANTRINH As String = ChromeDriver.FindElementById("ctl00_cpmain_ctl00_RadGrid_ctl00_ctl" & Format((j * 3) + 4, "00") & "_lbl_c6").Text
                            Dim NOIDUNGTRINH As String = ChromeDriver.FindElementById(elementID).Text

                            Dim YKIEN_HDTV As String = ""
                            Dim NGAY_YKIEN_HDTV_GANNHAT As Date = "01/01/1900"

                            For stt_hdtv As Integer = 1 To 100
                                If Selenium_Check_Element_Exist(ChromeDriver, By.XPath("//*[@id='ctl00_cpmain_ctl00_RadGrid_ctl00__" & j & "']/td[2]/table/tbody/tr[" & stt_hdtv & "]/td[1]")) = True Then
                                    Dim TVHDTV_NAME As String = ChromeDriver.FindElementByXPath("//*[@id='ctl00_cpmain_ctl00_RadGrid_ctl00__" & j & "']/td[2]/table/tbody/tr[" & stt_hdtv & "]/td[1]").Text
                                    Dim YKIEN_TVHDTV As String = Selenium_JavaScriptExecute_ToString(ChromeDriver, "var content =document.evaluate('//*[@id=" & Chr(34) & "ctl00_cpmain_ctl00_RadGrid_ctl00__" & j & Chr(34) & "]/td[2]/table/tbody/tr[" & stt_hdtv & "]/td[2]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML;return content;")

                                    If YKIEN_HDTV.Length = 0 Then
                                        YKIEN_HDTV = "- " & TVHDTV_NAME & Chr(10) & YKIEN_TVHDTV
                                    Else
                                        YKIEN_HDTV = YKIEN_HDTV & Chr(10) & "- " & TVHDTV_NAME & Chr(10) & YKIEN_TVHDTV
                                    End If

                                    If YKIEN_TVHDTV.Length > 0 Then
                                        Dim NGAYYKIEN As String = ""

                                        For m As Integer = Len(YKIEN_TVHDTV) To 1 Step -1
                                            If huynq_Substring(YKIEN_TVHDTV, m, Len(TVHDTV_NAME) + 1) = "[" & TVHDTV_NAME Then
                                                For n As Integer = m To Len(YKIEN_TVHDTV)
                                                    If huynq_isnumeric(huynq_Substring(YKIEN_TVHDTV, n, 1)) = True Then
                                                        NGAYYKIEN = huynq_Substring(YKIEN_TVHDTV, n, 10)
                                                        Exit For
                                                    End If
                                                Next
                                            End If
                                            If NGAYYKIEN.Length > 0 Then
                                                If Format(DateTime.ParseExact(NGAYYKIEN, "dd/MM/yyyy", Nothing), "dd/MM/yyyy") > NGAY_YKIEN_HDTV_GANNHAT Then
                                                    NGAY_YKIEN_HDTV_GANNHAT = Format(DateTime.ParseExact(NGAYYKIEN, "dd/MM/yyyy", Nothing), "dd/MM/yyyy")
                                                End If
                                                Exit For
                                            End If
                                        Next
                                    End If

                                    YKIEN_HDTV = Replace(YKIEN_HDTV, "<p style=" & Chr(34) & "margin-top:0; margin-bottom:0;line-height: 15px; " & Chr(34) & "></p>", "")
                                    YKIEN_HDTV = Replace(YKIEN_HDTV, "<p style=" & Chr(34) & "margin-top:0; margin-bottom:0;line-height: 15px; " & Chr(34) & ">", "")
                                    YKIEN_HDTV = Replace(YKIEN_HDTV, "<br>", "")
                                    YKIEN_HDTV = Replace(YKIEN_HDTV, "</p>", "")
                                Else
                                    Exit For
                                End If
                            Next


                            SQL_QUERY(link_folder_database, True, "INSERT INTO DATABASE_TOTRINH_THEODOI(CASEID, SOTOTRINH, NGAYTOTRINH, BANTRINH, NOIDUNGTRINH, YKIEN_HDTV, NGAY_YKIEN_HDTV_GANNHAT, NGUOITHUCHIEN, STATUS, USER_CREATED) " &
                                                        "VALUES ('" & CASEID & "', '" & SOTOTRINH & "', '" & NGAYTRINH & "', '" & BANTRINH & "', '" & NOIDUNGTRINH & "', '" & YKIEN_HDTV & "', '" & NGAY_YKIEN_HDTV_GANNHAT & "', '" & STR_PERSON & "', 'Đang theo dõi', '" & USERNAME & "_" & Now.ToString("yyyy/MM/dd hh:mm:ss") & "')")


                        End If
                    Next
                End If
            Next

            Return True
        Catch ex As Exception
            Console_WriteLine(str_log_file, ex.Message)
            Return False
        End Try
    End Function


    Public Function SQL_QUERY_TO_DATE(link_database As String, sql_string As String) As DateTime
        Try
            Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")
            MYCONNECTION.Open()

            Dim CMD As New SQLiteCommand
            CMD.CommandText = sql_string
            CMD.Connection = MYCONNECTION
            Dim RDR As SQLiteDataReader = CMD.ExecuteReader
            Dim DT As New DataTable
            Dim result As DateTime
            DT.Load(RDR)
            RDR.Close()
            If DT.Rows.Count > 0 Then
                If DT.Rows(0).Item(0) <> "" Then
                    result = DT.Rows(0).Item(0)
                Else
                    result = Format(DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", Nothing), "dd/MM/yyyy")
                End If
            Else
                result = Format(DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", Nothing), "dd/MM/yyyy")
            End If
            Console_WriteLine(str_log_file, "[SQL_QUERY_TO_DATE] - '" & sql_string & "'" & Chr(10) & "Result: " & result)
            Return result
        Catch ex As Exception
            Console_WriteLine(str_log_file, "[SQL_QUERY_TO_DATE] - '" & sql_string & "'" & Chr(10) & "Error Message: " & ex.Message)
            Return Format(DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", Nothing), "dd/MM/yyyy")
        End Try
    End Function

    Public Function SQL_QUERY_TO_INTEGER(link_database As String, sql_string As String) As Integer
        Try
            Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")
            MYCONNECTION.Open()

            Dim CMD As New SQLiteCommand
            CMD.CommandText = sql_string
            CMD.Connection = MYCONNECTION
            Dim RDR As SQLiteDataReader = CMD.ExecuteReader
            Dim DT As New DataTable
            DT.Load(RDR)
            RDR.Close()
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
            Console_WriteLine(str_log_file, "[SQL_QUERY_TO_INTEGER] - '" & sql_string & "' - Result: " & result)
            Return result
        Catch ex As Exception
            Console_WriteLine(str_log_file, "[SQL_QUERY_TO_INTEGER] - '" & sql_string & "' - Error Message: " & ex.Message)
            Return Nothing
        End Try
    End Function
    Public Sub SQL_QUERY(LINK_DATABASE As String, write_log As Boolean, sql_string As String)
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
                If write_log = True Then
                    Console_WriteLine(str_log_file, "[SQL_QUERY] - '" & sql_string & "'")
                End If
                MYCONNECTION.Close()
                Exit Do
            Catch ex As Exception
                MYCONNECTION.Close()
                If write_log = True Then
                    Console_WriteLine(str_log_file, "[SQL_QUERY] - '" & sql_string & "'" & Chr(10) & "Error Message: " & ex.Message)
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
                If Format(TimeSpent.TotalSeconds, "0.00") > 60 Then
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
            Return Nothing
            Call MsgBox("Contact To Administrator !!!" & Chr(10) & Chr(10) & ex.Message, vbCritical)
        End Try
    End Function

    Function huynq_Substring(value As String, startindex As Integer, length As Integer) As String
        Try
            Return value.Substring(startindex, length)
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Function huynq_isnumeric(value As String) As Boolean
        Try
            If IsNumeric(CInt(value)) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function Selenium_Get_Text_Element(ChromeDriver As ChromeDriver, by As OpenQA.Selenium.By) As String
        Try
            Return ChromeDriver.FindElement(by).Text
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function Selenium_Check_Element_Exist(ChromeDriver As ChromeDriver, by As OpenQA.Selenium.By) As Boolean
        Try
            If ChromeDriver.FindElement(by).Displayed = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function IsElementPresent(by As OpenQA.Selenium.By, driver As ChromeDriver) As Boolean
        Try
            driver.FindElement(by)
            Return True
        Catch generatedExceptionName As NoSuchElementException
            Return False
        End Try
    End Function

    Public Function Selenium_JavaScriptExecute_ToString(ChromeDriver As ChromeDriver, Str_Script As String) As String
        Dim js As IJavaScriptExecutor = TryCast(ChromeDriver, IJavaScriptExecutor)
        Try
            Return js.ExecuteScript(Str_Script)
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Function Edit_GhiChu_BanTongHop(str As String) As String
        Dim final As String = ""

        If str.Length = 0 Then
            Return final
        End If

        Dim str_split As String = ""
        Dim str_list As New List(Of String)

        Do
            For i = 1 To Len(str)
                If huynq_Substring(str, i, 4) = "<br>" Then
                    str_split = huynq_Substring(str, 0, i)
                    If InStr(str_split, "<br><", CompareMethod.Binary) = 0 Then
                        str_list.Add(Replace(str_split, "<br>", "; "))
                    End If
                    str = Replace(str, str_split, "")
                    Exit For
                End If

                If i = Len(str) Then
                    If InStr(str, "<br><", CompareMethod.Binary) = 0 Then
                        str_list.Add(Replace(str, "<br>", "; "))
                    End If
                    Exit Do
                End If
            Next
        Loop

        For Each str_final As String In str_list
            final = final & str_final
        Next

        Return final
    End Function
End Module
