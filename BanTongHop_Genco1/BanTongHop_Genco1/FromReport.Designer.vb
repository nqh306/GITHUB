<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FromReport
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim ColumnDefinition1 As DevExpress.XtraLayout.ColumnDefinition = New DevExpress.XtraLayout.ColumnDefinition()
        Dim RowDefinition1 As DevExpress.XtraLayout.RowDefinition = New DevExpress.XtraLayout.RowDefinition()
        Dim RowDefinition2 As DevExpress.XtraLayout.RowDefinition = New DevExpress.XtraLayout.RowDefinition()
        Me.barManager1 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.bar2 = New DevExpress.XtraBars.Bar()
        Me.BarEditItem_Filter = New DevExpress.XtraBars.BarEditItem()
        Me.RepositoryItemComboBox1 = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.BarButtonItem_Refresh = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem_ExportToExcel = New DevExpress.XtraBars.BarButtonItem()
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.GridControl_DangTheoDoi = New DevExpress.XtraGrid.GridControl()
        Me.GridView_DangTheoDoi = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.GridControl_DaXuLy = New DevExpress.XtraGrid.GridControl()
        Me.GridView_DaXuLy = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlGroup2 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.BANTRINH = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.NGAYTOTRINH = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SOTOTRINH = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.NOIDUNGTRINH = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SONGHIQUYET = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.NGAYNGHIQUYET = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SOQUYETDINH_VANBAN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.NGAYQUYETDINH_VANBAN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.NGAY_YKIEN_HDTV_GANNHAT = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.NGUOITHUCHIEN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.THOIGIANXULY = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GHICHU = New DevExpress.XtraGrid.Columns.GridColumn()
        CType(Me.barManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemComboBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.GridControl_DangTheoDoi, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView_DangTheoDoi, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControl_DaXuLy, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView_DaXuLy, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'barManager1
        '
        Me.barManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.bar2})
        Me.barManager1.DockControls.Add(Me.barDockControlTop)
        Me.barManager1.DockControls.Add(Me.barDockControlBottom)
        Me.barManager1.DockControls.Add(Me.barDockControlLeft)
        Me.barManager1.DockControls.Add(Me.barDockControlRight)
        Me.barManager1.Form = Me
        Me.barManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.BarButtonItem_ExportToExcel, Me.BarButtonItem_Refresh, Me.BarEditItem_Filter})
        Me.barManager1.MainMenu = Me.bar2
        Me.barManager1.MaxItemId = 3
        Me.barManager1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemComboBox1})
        '
        'bar2
        '
        Me.bar2.BarName = "Main menu"
        Me.bar2.DockCol = 0
        Me.bar2.DockRow = 0
        Me.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
        Me.bar2.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarEditItem_Filter), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem_Refresh), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem_ExportToExcel)})
        Me.bar2.OptionsBar.MultiLine = True
        Me.bar2.OptionsBar.UseWholeRow = True
        Me.bar2.Text = "Main menu"
        '
        'BarEditItem_Filter
        '
        Me.BarEditItem_Filter.Caption = "BarEditItem1"
        Me.BarEditItem_Filter.Edit = Me.RepositoryItemComboBox1
        Me.BarEditItem_Filter.Id = 2
        Me.BarEditItem_Filter.Name = "BarEditItem_Filter"
        '
        'RepositoryItemComboBox1
        '
        Me.RepositoryItemComboBox1.AutoHeight = False
        Me.RepositoryItemComboBox1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemComboBox1.Name = "RepositoryItemComboBox1"
        '
        'BarButtonItem_Refresh
        '
        Me.BarButtonItem_Refresh.Caption = "Lấy Dữ Liệu"
        Me.BarButtonItem_Refresh.Id = 1
        Me.BarButtonItem_Refresh.Name = "BarButtonItem_Refresh"
        '
        'BarButtonItem_ExportToExcel
        '
        Me.BarButtonItem_ExportToExcel.Caption = "Xuất Excel"
        Me.BarButtonItem_ExportToExcel.Id = 0
        Me.BarButtonItem_ExportToExcel.Name = "BarButtonItem_ExportToExcel"
        '
        'barDockControlTop
        '
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Manager = Me.barManager1
        Me.barDockControlTop.Size = New System.Drawing.Size(933, 22)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 533)
        Me.barDockControlBottom.Manager = Me.barManager1
        Me.barDockControlBottom.Size = New System.Drawing.Size(933, 0)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 22)
        Me.barDockControlLeft.Manager = Me.barManager1
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 511)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(933, 22)
        Me.barDockControlRight.Manager = Me.barManager1
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 511)
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.GridControl_DangTheoDoi)
        Me.LayoutControl1.Controls.Add(Me.GridControl_DaXuLy)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 22)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(933, 511)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'GridControl_DangTheoDoi
        '
        Me.GridControl_DangTheoDoi.Location = New System.Drawing.Point(24, 288)
        Me.GridControl_DangTheoDoi.MainView = Me.GridView_DangTheoDoi
        Me.GridControl_DangTheoDoi.MenuManager = Me.barManager1
        Me.GridControl_DangTheoDoi.Name = "GridControl_DangTheoDoi"
        Me.GridControl_DangTheoDoi.Size = New System.Drawing.Size(885, 199)
        Me.GridControl_DangTheoDoi.TabIndex = 5
        Me.GridControl_DangTheoDoi.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView_DangTheoDoi})
        '
        'GridView_DangTheoDoi
        '
        Me.GridView_DangTheoDoi.GridControl = Me.GridControl_DangTheoDoi
        Me.GridView_DangTheoDoi.Name = "GridView_DangTheoDoi"
        Me.GridView_DangTheoDoi.OptionsBehavior.Editable = False
        Me.GridView_DangTheoDoi.OptionsView.ShowAutoFilterRow = True
        Me.GridView_DangTheoDoi.OptionsView.ShowGroupPanel = False
        '
        'GridControl_DaXuLy
        '
        Me.GridControl_DaXuLy.Location = New System.Drawing.Point(24, 43)
        Me.GridControl_DaXuLy.MainView = Me.GridView_DaXuLy
        Me.GridControl_DaXuLy.MenuManager = Me.barManager1
        Me.GridControl_DaXuLy.Name = "GridControl_DaXuLy"
        Me.GridControl_DaXuLy.Size = New System.Drawing.Size(885, 198)
        Me.GridControl_DaXuLy.TabIndex = 4
        Me.GridControl_DaXuLy.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView_DaXuLy})
        '
        'GridView_DaXuLy
        '
        Me.GridView_DaXuLy.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.BANTRINH, Me.NGAYTOTRINH, Me.SOTOTRINH, Me.NOIDUNGTRINH, Me.SONGHIQUYET, Me.NGAYNGHIQUYET, Me.SOQUYETDINH_VANBAN, Me.NGAYQUYETDINH_VANBAN, Me.NGAY_YKIEN_HDTV_GANNHAT, Me.NGUOITHUCHIEN, Me.THOIGIANXULY, Me.GHICHU})
        Me.GridView_DaXuLy.GridControl = Me.GridControl_DaXuLy
        Me.GridView_DaXuLy.Name = "GridView_DaXuLy"
        Me.GridView_DaXuLy.OptionsBehavior.Editable = False
        Me.GridView_DaXuLy.OptionsView.ShowAutoFilterRow = True
        Me.GridView_DaXuLy.OptionsView.ShowFooter = True
        Me.GridView_DaXuLy.OptionsView.ShowGroupPanel = False
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup1, Me.LayoutControlGroup2})
        Me.Root.LayoutMode = DevExpress.XtraLayout.Utils.LayoutMode.Table
        Me.Root.Name = "Root"
        ColumnDefinition1.SizeType = System.Windows.Forms.SizeType.AutoSize
        ColumnDefinition1.Width = 913.0R
        Me.Root.OptionsTableLayoutGroup.ColumnDefinitions.AddRange(New DevExpress.XtraLayout.ColumnDefinition() {ColumnDefinition1})
        RowDefinition1.Height = 50.0R
        RowDefinition1.SizeType = System.Windows.Forms.SizeType.Percent
        RowDefinition2.Height = 50.0R
        RowDefinition2.SizeType = System.Windows.Forms.SizeType.Percent
        Me.Root.OptionsTableLayoutGroup.RowDefinitions.AddRange(New DevExpress.XtraLayout.RowDefinition() {RowDefinition1, RowDefinition2})
        Me.Root.Size = New System.Drawing.Size(933, 511)
        Me.Root.TextVisible = False
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(913, 245)
        Me.LayoutControlGroup1.Text = "Tờ trình đã xử lý"
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GridControl_DaXuLy
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(889, 202)
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlGroup2
        '
        Me.LayoutControlGroup2.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem2})
        Me.LayoutControlGroup2.Location = New System.Drawing.Point(0, 245)
        Me.LayoutControlGroup2.Name = "LayoutControlGroup2"
        Me.LayoutControlGroup2.OptionsTableLayoutItem.RowIndex = 1
        Me.LayoutControlGroup2.Size = New System.Drawing.Size(913, 246)
        Me.LayoutControlGroup2.Text = "Tờ trình đang theo dõi"
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.GridControl_DangTheoDoi
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.OptionsTableLayoutItem.RowIndex = 1
        Me.LayoutControlItem2.Size = New System.Drawing.Size(889, 203)
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextVisible = False
        '
        'BANTRINH
        '
        Me.BANTRINH.Caption = "Ban Trình"
        Me.BANTRINH.FieldName = "BANTRINH"
        Me.BANTRINH.Name = "BANTRINH"
        Me.BANTRINH.Visible = True
        Me.BANTRINH.VisibleIndex = 0
        '
        'NGAYTOTRINH
        '
        Me.NGAYTOTRINH.Caption = "Ngày Tờ Trình"
        Me.NGAYTOTRINH.FieldName = "NGAYTOTRINH"
        Me.NGAYTOTRINH.Name = "NGAYTOTRINH"
        Me.NGAYTOTRINH.Visible = True
        Me.NGAYTOTRINH.VisibleIndex = 1
        '
        'SOTOTRINH
        '
        Me.SOTOTRINH.Caption = "Số Tờ Trình"
        Me.SOTOTRINH.FieldName = "SOTOTRINH"
        Me.SOTOTRINH.Name = "SOTOTRINH"
        Me.SOTOTRINH.Summary.AddRange(New DevExpress.XtraGrid.GridSummaryItem() {New DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "SOTOTRINH", "Count: {0}")})
        Me.SOTOTRINH.Visible = True
        Me.SOTOTRINH.VisibleIndex = 2
        '
        'NOIDUNGTRINH
        '
        Me.NOIDUNGTRINH.Caption = "Trích Yếu"
        Me.NOIDUNGTRINH.FieldName = "NOIDUNGTRINH"
        Me.NOIDUNGTRINH.Name = "NOIDUNGTRINH"
        Me.NOIDUNGTRINH.Visible = True
        Me.NOIDUNGTRINH.VisibleIndex = 3
        '
        'SONGHIQUYET
        '
        Me.SONGHIQUYET.Caption = "Số Nghị Quyết"
        Me.SONGHIQUYET.FieldName = "SONGHIQUYET"
        Me.SONGHIQUYET.Name = "SONGHIQUYET"
        Me.SONGHIQUYET.Visible = True
        Me.SONGHIQUYET.VisibleIndex = 4
        '
        'NGAYNGHIQUYET
        '
        Me.NGAYNGHIQUYET.Caption = "Ngày Nghị Quyết"
        Me.NGAYNGHIQUYET.FieldName = "NGAYNGHIQUYET"
        Me.NGAYNGHIQUYET.Name = "NGAYNGHIQUYET"
        Me.NGAYNGHIQUYET.Visible = True
        Me.NGAYNGHIQUYET.VisibleIndex = 5
        '
        'SOQUYETDINH_VANBAN
        '
        Me.SOQUYETDINH_VANBAN.Caption = "Số QĐ/VB"
        Me.SOQUYETDINH_VANBAN.FieldName = "SOQUYETDINH_VANBAN"
        Me.SOQUYETDINH_VANBAN.Name = "SOQUYETDINH_VANBAN"
        Me.SOQUYETDINH_VANBAN.Visible = True
        Me.SOQUYETDINH_VANBAN.VisibleIndex = 6
        '
        'NGAYQUYETDINH_VANBAN
        '
        Me.NGAYQUYETDINH_VANBAN.Caption = "Ngày QĐ/VB"
        Me.NGAYQUYETDINH_VANBAN.FieldName = "NGAYQUYETDINH_VANBAN"
        Me.NGAYQUYETDINH_VANBAN.Name = "NGAYQUYETDINH_VANBAN"
        Me.NGAYQUYETDINH_VANBAN.Visible = True
        Me.NGAYQUYETDINH_VANBAN.VisibleIndex = 7
        '
        'NGAY_YKIEN_HDTV_GANNHAT
        '
        Me.NGAY_YKIEN_HDTV_GANNHAT.Caption = "Ngày Ý Kiến HĐTV"
        Me.NGAY_YKIEN_HDTV_GANNHAT.FieldName = "NGAY_YKIEN_HDTV_GANNHAT"
        Me.NGAY_YKIEN_HDTV_GANNHAT.Name = "NGAY_YKIEN_HDTV_GANNHAT"
        Me.NGAY_YKIEN_HDTV_GANNHAT.Visible = True
        Me.NGAY_YKIEN_HDTV_GANNHAT.VisibleIndex = 8
        '
        'NGUOITHUCHIEN
        '
        Me.NGUOITHUCHIEN.Caption = "Người Thực Hiện"
        Me.NGUOITHUCHIEN.FieldName = "NGUOITHUCHIEN"
        Me.NGUOITHUCHIEN.Name = "NGUOITHUCHIEN"
        Me.NGUOITHUCHIEN.Visible = True
        Me.NGUOITHUCHIEN.VisibleIndex = 9
        '
        'THOIGIANXULY
        '
        Me.THOIGIANXULY.Caption = "Số ngày thực hiện"
        Me.THOIGIANXULY.FieldName = "THOIGIANXULY"
        Me.THOIGIANXULY.Name = "THOIGIANXULY"
        Me.THOIGIANXULY.Summary.AddRange(New DevExpress.XtraGrid.GridSummaryItem() {New DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, "THOIGIANXULY", "Avg={0:0.##}")})
        Me.THOIGIANXULY.Visible = True
        Me.THOIGIANXULY.VisibleIndex = 10
        '
        'GHICHU
        '
        Me.GHICHU.Caption = "Ghi Chú"
        Me.GHICHU.FieldName = "GHICHU"
        Me.GHICHU.Name = "GHICHU"
        Me.GHICHU.Visible = True
        Me.GHICHU.VisibleIndex = 11
        '
        'FromReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(933, 533)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.Name = "FromReport"
        Me.Text = "Báo Cáo"
        CType(Me.barManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemComboBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.GridControl_DangTheoDoi, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView_DangTheoDoi, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControl_DaXuLy, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView_DaXuLy, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend barManager1 As DevExpress.XtraBars.BarManager
    Friend bar2 As DevExpress.XtraBars.Bar
    Friend barDockControlTop As DevExpress.XtraBars.BarDockControl
    Friend barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Friend barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Friend barDockControlRight As DevExpress.XtraBars.BarDockControl
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents GridControl_DaXuLy As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView_DaXuLy As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents GridControl_DangTheoDoi As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView_DangTheoDoi As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BarButtonItem_ExportToExcel As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem_Refresh As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlGroup2 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents BarEditItem_Filter As DevExpress.XtraBars.BarEditItem
    Friend WithEvents RepositoryItemComboBox1 As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents BANTRINH As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents NGAYTOTRINH As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SOTOTRINH As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents NOIDUNGTRINH As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SONGHIQUYET As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents NGAYNGHIQUYET As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SOQUYETDINH_VANBAN As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents NGAYQUYETDINH_VANBAN As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents NGAY_YKIEN_HDTV_GANNHAT As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents NGUOITHUCHIEN As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents THOIGIANXULY As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GHICHU As DevExpress.XtraGrid.Columns.GridColumn
End Class
