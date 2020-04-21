
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraPrinting.Drawing
Imports System.Data.SQLite
Module Module_Report

    Dim con As New SQLiteConnection
    Public Sub Taoketnoi(link_database As String)
        Dim str As String = "DataSource=" & link_database & "; Version=3;"
        con.ConnectionString = str
        con.Open()
    End Sub
    Public Sub Dongketnoi()
        con.Close()
    End Sub
    Public Function LayDulieu(link_database As String, sql_string As String) As DataSet
        Taoketnoi(link_database)
        Dim CMD As New SQLiteCommand
        CMD.CommandText = sql_string
        CMD.Connection = con
        Dim RDR As SQLiteDataReader = CMD.ExecuteReader
        Dim DT As New DataTable
        DT.Load(RDR)
        RDR.Close()
        Dongketnoi()
        Dim DS As New DataSet
        DS.Tables.Add(DT)
        Return DS
    End Function

    'Private Sub btnCreateReport_Click(sender As Object, e As EventArgs) Handles btnCreateReport.Click
    'SplashScreenManager.ShowForm(Me, GetType(loading), True, True, False)
    '' Create XtraReport instance
    'Dim rep As New XtraReport()
    'If grpPagesetup.Text = 0 Then
    '    rep.PaperKind = System.Drawing.Printing.PaperKind.A4
    'Else
    '    rep.PaperKind = System.Drawing.Printing.PaperKind.A4Rotated
    'End If
    'If grpWaterMark.Text = 0 Then
    '    SetTextWatermark(rep)
    'Else
    '    SetPictureWatermark(rep)
    'End If

    'rep.DataSource = LayDulieu(txtQuery.Text)
    'rep.DataMember = (CType(rep.DataSource, DataSet)).Tables(0).TableName
    'InitBands(rep)
    'InitDetailsBasedonXRTable(rep)
    'rep.ShowRibbonPreviewDialog()
    'SplashScreenManager.CloseForm(False)
    'End Sub

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
        Dim colWidth As Integer = (rep.PageWidth - (rep.Margins.Left + rep.Margins.Right)) / colCount
        rep.Margins = New System.Drawing.Printing.Margins(20, 20, 20, 20)
        Dim tieude As New XRLabel
        tieude.Text = strTitle
        tieude.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        tieude.ForeColor = Color.Orange
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
            headerCell.Width = colWidth
            detailCell.Width = colWidth
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

        'add sent_by
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
        rep.Band.SubBands(BandKind.Detail).Controls.Add(soluongthu)
        rep.Bands(BandKind.ReportFooter).Controls.Add(tableHeader_sent_receive)

        'rep.Band.SubBands(BandKind.ReportFooter).Controls.Add(tableHeader_sent_receive)
    End Sub
    Sub SetTextWatermark(ps As XtraReport)
        ' Create the text watermark.
        Dim textWatermark As New Watermark()

        ' Set watermark options.
        textWatermark.Text = "Account Services Operations"
        textWatermark.TextDirection = DirectionMode.ForwardDiagonal
        textWatermark.Font = New Font(textWatermark.Font.FontFamily, 40)
        textWatermark.ForeColor = Color.DodgerBlue
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
End Module
