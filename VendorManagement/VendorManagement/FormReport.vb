Imports DevExpress.XtraBars
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraPrinting.Drawing
Imports DevExpress.XtraSplashScreen
Public Class FormReport
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public link_app_config As String = appPath & "Application_Config.db"
    Public link_application_config As String = Module_Letter_Management.SQL_QUERY_TO_STRING(link_app_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'Link_Application_Config'")
    Public Link_folder_database As String = ""
    Public link_database As String = ""
    Public table_name As String = ""

    Private Sub FormReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        On Error GoTo err_handle

        BarEditItem_FromDate.EditWidth = 150
        BarEditItem_FromDate.EditValue = Now
        BarEditItem_ToDate.EditWidth = 150
        BarEditItem_ToDate.EditValue = Now

        'Add value combobox Vendor
        BarEditItem_ChooseVendor.EditWidth = 200
        CType(BarEditItem_ChooseVendor.Edit, RepositoryItemComboBox).Items.Add("TASETCO EXPRESS")
        CType(BarEditItem_ChooseVendor.Edit, RepositoryItemComboBox).Items.Add("VIETTEL POST")
        BarEditItem_ChooseVendor.EditValue = "TASETCO EXPRESS"

        'add value year database
        For i = 2018 To CInt(Now.ToString("yyyy"))
            CType(BarEditItem_Year.Edit, RepositoryItemComboBox).Items.Add(i)
        Next
        BarEditItem_Year.EditWidth = 100
        BarEditItem_Year.EditValue = CInt(Now.ToString("yyyy"))

        Refresh_link_database()

        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub BarEditItem_ChooseVendor_EditValueChanged(sender As Object, e As EventArgs) Handles BarEditItem_ChooseVendor.EditValueChanged
        On Error GoTo err_handle

        'Add value database
        CType(BarEditItem_ChooseDatabase.Edit, RepositoryItemComboBox).Items.Clear()
        BarEditItem_ChooseDatabase.EditWidth = 200
        Dim DT As DataTable = Module_Letter_Management.SQL_QUERY_TO_DATATABLE(link_application_config, "SELECT DISTINCT Database_Name FROM LIST_DATABASE WHERE Vendor_Name = '" & BarEditItem_ChooseVendor.EditValue & "'")
        If DT.Rows.Count > 0 Then
            For i As Integer = 0 To DT.Rows.Count - 1
                CType(BarEditItem_ChooseDatabase.Edit, RepositoryItemComboBox).Items.Add(DT.Rows(i).Item(0).ToString)
            Next
            BarEditItem_ChooseDatabase.EditValue = DT.Rows(0).Item(0).ToString
        End If

        'Add value report name
        CType(BarEditItem_ChooseReport.Edit, RepositoryItemComboBox).Items.Clear()
        BarEditItem_ChooseReport.EditWidth = 250
        Dim DT_report_name As DataTable
        If BarEditItem_ChooseVendor.EditValue = "TASETCO EXPRESS" Then
            DT_report_name = Module_Letter_Management.SQL_QUERY_TO_DATATABLE(link_application_config, "SELECT DISTINCT Report_Name FROM LIST_DATABASE_REPORT WHERE Vendor = 'Both' OR Vendor = 'TASETCO'")
        Else
            DT_report_name = Module_Letter_Management.SQL_QUERY_TO_DATATABLE(link_application_config, "SELECT DISTINCT Report_Name FROM LIST_DATABASE_REPORT WHERE Vendor = 'Both' OR Vendor = 'VIETTEL'")
        End If
        If DT_report_name.Rows.Count > 0 Then
            For i As Integer = 0 To DT_report_name.Rows.Count - 1
                CType(BarEditItem_ChooseReport.Edit, RepositoryItemComboBox).Items.Add(DT_report_name.Rows(i).Item(0).ToString)
            Next
        End If

        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub Refresh_link_database()
        'Get link_folder_database
        Link_folder_database = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Folder_Path FROM LIST_DATABASE WHERE Vendor_NAME = '" & BarEditItem_ChooseVendor.EditValue.ToString & "' AND Database_Name = '" & BarEditItem_ChooseDatabase.EditValue.ToString & "'")
        If Link_folder_database.Substring(Link_folder_database.Length - 1, 1) <> "\" Then
            Link_folder_database = Link_folder_database & "\"
        End If

        'Setup link database


        If BarEditItem_ChooseVendor.EditValue.ToString = "TASETCO EXPRESS" Then
            link_database = Link_folder_database & "Database_Letter_Management.txt"
            table_name = "Tasetco_" & BarEditItem_ChooseDatabase.EditValue.ToString & "_" & BarEditItem_Year.EditValue.ToString
        Else
            link_database = Link_folder_database & "Database_Viettel.txt"
            table_name = "Viettel_" & BarEditItem_ChooseDatabase.EditValue.ToString & "_" & BarEditItem_Year.EditValue.ToString
        End If

        Main.Statusbar_item2.Caption = "Load database: '" & link_database & "' - Table: " & table_name
    End Sub

    Private Sub BarButtonItem_LoadDatabase_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonItem_LoadDatabase.ItemClick
        On Error GoTo err_handle

        Dim iret = MsgBox("Do you want to open Database: " & BarEditItem_ChooseDatabase.EditValue.ToString & "_" & BarEditItem_Year.EditValue.ToString, vbYesNo)
        If iret = vbYes Then
            Refresh_link_database()
        End If

        Exit Sub
err_handle:
        Error_handle()
    End Sub
    Public Sub InitBands(ByVal rep As XtraReport)
        ' Create bands
        Dim detail As New DetailBand()
        Dim pageHeader As New PageHeaderBand()
        Dim reportHeader As New ReportHeaderBand()
        Dim reportFooter As New ReportFooterBand()
        reportHeader.Height = 40
        detail.Height = 20
        reportFooter.Height = 380
        pageHeader.Height = 20
        ' Place the bands onto a report
        rep.Bands.AddRange(New DevExpress.XtraReports.UI.Band() {reportHeader, detail, pageHeader, reportFooter})
    End Sub
    Public Sub InitDetailsBasedonXRTable(strTitle As String, rep As XtraReport)
        Dim ds As DataSet = (CType(rep.DataSource, DataSet))
        Dim colCount As Integer = ds.Tables(0).Columns.Count
        Dim colWidth_temp As Integer = (rep.PageWidth - (rep.Margins.Left + rep.Margins.Right)) / colCount
        Dim total_minus As Integer = 0
        For j As Integer = 0 To colCount - 1
            If ds.Tables(0).Columns(j).Caption = "Client_Name" Then
                total_minus = total_minus + colWidth_temp * 2
            End If
            If ds.Tables(0).Columns(j).Caption = "Mailing_address" Then
                total_minus = total_minus + colWidth_temp * 3
            End If
        Next

        Dim from_date As String = Format(BarEditItem_FromDate.EditValue, "dd/MM/yyyy")
        Dim to_date As String = Format(BarEditItem_ToDate.EditValue, "dd/MM/yyyy")


        Dim colWidth As Integer = (rep.PageWidth - (total_minus + rep.Margins.Left + rep.Margins.Right)) / colCount
        rep.Margins = New System.Drawing.Printing.Margins(20, 20, 20, 20)
        Dim tieude As New XRLabel
        tieude.Text = strTitle & Chr(10) & Chr(10) & " From: " & from_date & "   -   To: " & to_date
        tieude.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        tieude.ForeColor = Color.Black
        tieude.Font = New Font("Tahoma", 20, FontStyle.Bold, GraphicsUnit.Pixel)
        tieude.Width = Convert.ToInt32(rep.PageWidth - 50)

        ' Create a table to represent headers
        Dim tableHeader As New XRTable()
        tableHeader.Height = 40
        tableHeader.BackColor = Color.Gray
        tableHeader.ForeColor = Color.White
        tableHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        tableHeader.Font = New Font("Tahoma", 12, FontStyle.Bold, GraphicsUnit.Pixel)
        tableHeader.Width = (rep.PageWidth - (rep.Margins.Left + rep.Margins.Right))
        tableHeader.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100.0F)
        Dim headerRow As New XRTableRow()
        headerRow.Width = tableHeader.Width
        tableHeader.Rows.Add(headerRow)
        tableHeader.BeginInit()
        ' Create a table to display data
        Dim tableDetail As New XRTable()
        tableDetail.Height = 20
        tableDetail.Width = (rep.PageWidth - (rep.Margins.Left + rep.Margins.Right))
        tableDetail.Font = New Font("Tahoma", 12, FontStyle.Regular, GraphicsUnit.Pixel)
        Dim detailRow As New XRTableRow()
        detailRow.Width = tableDetail.Width
        tableDetail.Rows.Add(detailRow)
        tableDetail.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100.0F)
        tableDetail.BeginInit()
        ' Create table cells, fill the header cells with text, bind the cells to data
        For i As Integer = 0 To colCount - 1
            Dim headerCell As New XRTableCell()
            headerCell.Text = ds.Tables(0).Columns(i).Caption
            Dim detailCell As New XRTableCell()
            detailCell.DataBindings.Add("Text", Nothing, ds.Tables(0).Columns(i).Caption)
            If i = 0 Then
                headerCell.Borders = DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom
                detailCell.Borders = DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom
            Else
                headerCell.Borders = DevExpress.XtraPrinting.BorderSide.All
                detailCell.Borders = DevExpress.XtraPrinting.BorderSide.All
            End If

            If headerCell.Text = "Client_Name" Then
                headerCell.Width = colWidth * 2
                detailCell.Width = colWidth * 2
            Else
                If headerCell.Text = "Mailing_address" Then
                    headerCell.Width = colWidth * 3
                    detailCell.Width = colWidth * 3
                Else
                    headerCell.Width = colWidth
                    detailCell.Width = colWidth
                End If
            End If

            detailCell.Borders = DevExpress.XtraPrinting.BorderSide.Bottom Or DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Right

            ' Place the cells into the corresponding tables
            headerRow.Cells.Add(headerCell)
            detailRow.Cells.Add(detailCell)
        Next i
        tableHeader.EndInit()
        tableDetail.EndInit()

        'add footer so luong thu
        Dim dt As DataTable
        dt = ds.Tables(0)

        Dim soluongthu As New XRLabel
        soluongthu.Text = "Total: " & dt.Rows.Count
        soluongthu.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft
        soluongthu.ForeColor = Color.Black
        soluongthu.Font = New Font("Tahoma", 12, FontStyle.Bold, GraphicsUnit.Pixel)
        soluongthu.Width = Convert.ToInt32(rep.PageWidth - 50)

        'add tableHeader_sent_receive
        Dim tableHeader_sent_receive As New XRTable()
        tableHeader_sent_receive.Height = 40
        tableHeader_sent_receive.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        tableHeader_sent_receive.Font = New Font("Tahoma", 12, FontStyle.Bold, GraphicsUnit.Pixel)
        tableHeader_sent_receive.Width = (rep.PageWidth - (rep.Margins.Left + rep.Margins.Right))
        tableHeader_sent_receive.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100.0F)
        Dim headerRow_sent_receive As New XRTableRow()
        tableHeader_sent_receive.Rows.Add(headerRow_sent_receive)
        headerRow_sent_receive.Width = tableHeader_sent_receive.Width

        tableHeader_sent_receive.BeginInit()

        Dim headerCell_sent_receive1 As New XRTableCell()
        headerCell_sent_receive1.Text = "Sent by"
        headerCell_sent_receive1.Width = (rep.PageWidth - (rep.Margins.Left + rep.Margins.Right)) / 2
        headerRow_sent_receive.Cells.Add(headerCell_sent_receive1)

        Dim headerCell_sent_receive2 As New XRTableCell()
        headerCell_sent_receive2.Text = "Received"
        headerCell_sent_receive2.Width = (rep.PageWidth - (rep.Margins.Left + rep.Margins.Right)) / 2
        headerRow_sent_receive.Cells.Add(headerCell_sent_receive2)

        tableHeader_sent_receive.EndInit()

        ' Place the table onto a report's Detail band
        rep.Bands(BandKind.ReportHeader).Controls.Add(tieude)
        rep.Bands(BandKind.PageHeader).Controls.Add(tableHeader)
        rep.Bands(BandKind.Detail).Controls.Add(tableDetail)
        rep.Bands(BandKind.ReportFooter).Controls.Add(soluongthu)

    End Sub
    Sub SetTextWatermark(ps As XtraReport, text As String)
        ' Create the text watermark.
        Dim textWatermark As New Watermark()

        ' Set watermark options.
        textWatermark.Text = text
        textWatermark.TextDirection = DirectionMode.ForwardDiagonal
        textWatermark.Font = New Font(textWatermark.Font.FontFamily, 40)
        textWatermark.ForeColor = Color.LightGray
        textWatermark.TextTransparency = 150
        textWatermark.ShowBehind = False
        textWatermark.PageRange = "1,3-5"

        ' Add the watermark to a document.
        ps.Watermark.CopyFrom(textWatermark)
    End Sub

    Public Sub SetPictureWatermark(ps As XtraReport)
        ' Create the picture watermark.
        Dim pictureWatermark As New Watermark()

        ' Set watermark options.
        pictureWatermark.Image = Bitmap.FromFile("logo.png")
        pictureWatermark.ImageAlign = ContentAlignment.MiddleCenter
        pictureWatermark.ImageTiling = False
        pictureWatermark.ImageViewMode = ImageViewMode.Zoom
        pictureWatermark.ImageTransparency = 150
        pictureWatermark.ShowBehind = True
        'pictureWatermark.PageRange = "2,4"

        ' Add the watermark to a document.
        ps.Watermark.CopyFrom(pictureWatermark)
    End Sub

    Private Sub btCreateReport_ItemClick(sender As Object, e As ItemClickEventArgs) Handles btCreateReport.ItemClick
        On Error GoTo err_handle

        SplashScreenManager.ShowForm(Me, GetType(loading), True, True, False)
        Dim SQL_String As String = SQL_QUERY_TO_STRING(link_application_config, "SELECT SQL_String FROM LIST_DATABASE_REPORT WHERE Report_Name = '" & BarEditItem_ChooseReport.EditValue.ToString & "'")
        Dim Title_Report As String = SQL_QUERY_TO_STRING(link_application_config, "SELECT Title_Report FROM LIST_DATABASE_REPORT WHERE Report_Name = '" & BarEditItem_ChooseReport.EditValue.ToString & "'")
        Dim Paper_Kind As String = SQL_QUERY_TO_STRING(link_application_config, "SELECT Paper_Kind FROM LIST_DATABASE_REPORT WHERE Report_Name = '" & BarEditItem_ChooseReport.EditValue.ToString & "'")
        Dim WaterMark_Text As String = SQL_QUERY_TO_STRING(link_application_config, "SELECT WaterMark_Text FROM LIST_DATABASE_REPORT WHERE Report_Name = '" & BarEditItem_ChooseReport.EditValue.ToString & "'")

        Dim from_date As String = "'" & Format(BarEditItem_FromDate.EditValue, "yyyyMMdd") & "'"
        Dim to_date As String = "'" & Format(BarEditItem_ToDate.EditValue, "yyyyMMdd") & "'"


        SQL_String = Replace(SQL_String, "{User_Defined_table_name}", table_name)
        SQL_String = Replace(SQL_String, "{User_Defined_from_date}", from_date)
        SQL_String = Replace(SQL_String, "{User_Defined_to_date}", to_date)
        SQL_String = Replace(SQL_String, "{dau_nhay}", "'")

        Dim rep As New XtraReport()

        If Paper_Kind = "Portrait" Then
            rep.PaperKind = System.Drawing.Printing.PaperKind.A4
        Else
            rep.PaperKind = System.Drawing.Printing.PaperKind.A4Rotated
        End If

        If WaterMark_Text.Length > 0 Then
            SetTextWatermark(rep, WaterMark_Text)
        End If

        rep.DataSource = SQL_QUERY_TO_DATASET(link_database, SQL_String)
        rep.DataMember = (CType(rep.DataSource, DataSet)).Tables(0).TableName
        InitBands(rep)
        InitDetailsBasedonXRTable(Title_Report, rep)

        rep.CreateDocument()
        DocumentViewer1.DocumentSource = rep

        SplashScreenManager.CloseForm(False)

        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub
End Class