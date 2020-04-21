Partial Public Class Form1
    Inherits DevExpress.XtraEditors.XtraForm

    ''' <summary>
    ''' Required designer variable.
    ''' </summary>
    Private components As System.ComponentModel.IContainer = Nothing

    ''' <summary>
    ''' Clean up any resources being used.
    ''' </summary>
    ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso (components IsNot Nothing) Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

#Region "Windows Form Designer generated code"

    ''' <summary>
    ''' Required method for Designer support - do not modify
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim ColumnDefinition3 As DevExpress.XtraLayout.ColumnDefinition = New DevExpress.XtraLayout.ColumnDefinition()
        Dim RowDefinition4 As DevExpress.XtraLayout.RowDefinition = New DevExpress.XtraLayout.RowDefinition()
        Dim RowDefinition5 As DevExpress.XtraLayout.RowDefinition = New DevExpress.XtraLayout.RowDefinition()
        Dim RowDefinition6 As DevExpress.XtraLayout.RowDefinition = New DevExpress.XtraLayout.RowDefinition()
        Dim RowDefinition7 As DevExpress.XtraLayout.RowDefinition = New DevExpress.XtraLayout.RowDefinition()
        Dim RowDefinition8 As DevExpress.XtraLayout.RowDefinition = New DevExpress.XtraLayout.RowDefinition()
        Dim RowDefinition9 As DevExpress.XtraLayout.RowDefinition = New DevExpress.XtraLayout.RowDefinition()
        Dim RowDefinition10 As DevExpress.XtraLayout.RowDefinition = New DevExpress.XtraLayout.RowDefinition()
        Dim RowDefinition11 As DevExpress.XtraLayout.RowDefinition = New DevExpress.XtraLayout.RowDefinition()
        Dim ColumnDefinition1 As DevExpress.XtraLayout.ColumnDefinition = New DevExpress.XtraLayout.ColumnDefinition()
        Dim ColumnDefinition2 As DevExpress.XtraLayout.ColumnDefinition = New DevExpress.XtraLayout.ColumnDefinition()
        Dim RowDefinition1 As DevExpress.XtraLayout.RowDefinition = New DevExpress.XtraLayout.RowDefinition()
        Dim RowDefinition2 As DevExpress.XtraLayout.RowDefinition = New DevExpress.XtraLayout.RowDefinition()
        Dim RowDefinition3 As DevExpress.XtraLayout.RowDefinition = New DevExpress.XtraLayout.RowDefinition()
        Me.barManager1 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.bar1 = New DevExpress.XtraBars.Bar()
        Me.BarEditItem_VendorName = New DevExpress.XtraBars.BarEditItem()
        Me.RepositoryItemComboBox_VendorName = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.BarEditItem_DatabaseName = New DevExpress.XtraBars.BarEditItem()
        Me.RepositoryItemComboBox_DatabaseName = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.BarEditItem_Year = New DevExpress.XtraBars.BarEditItem()
        Me.RepositoryItemComboBox_Year = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.btLoadDatabase = New DevExpress.XtraBars.BarButtonItem()
        Me.bar3 = New DevExpress.XtraBars.Bar()
        Me.StatusBar1 = New DevExpress.XtraBars.BarStaticItem()
        Me.StatusBar2 = New DevExpress.XtraBars.BarStaticItem()
        Me.StatusBar3 = New DevExpress.XtraBars.BarStaticItem()
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
        Me.LayoutControl_Form = New DevExpress.XtraLayout.LayoutControl()
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl()
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.WebBrowser1 = New System.Windows.Forms.WebBrowser()
        Me.btUpdateForAllDatabase = New DevExpress.XtraEditors.SimpleButton()
        Me.btGetInformation = New DevExpress.XtraEditors.SimpleButton()
        Me.btLoad = New DevExpress.XtraEditors.SimpleButton()
        Me.ComboBox_FilterDatabase = New System.Windows.Forms.ComboBox()
        Me.tbStrSearch = New DevExpress.XtraEditors.SearchControl()
        Me.LayoutControlGroup_Form = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlGroup_Input = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem_tbStrSearch = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem_ComboBox_FilterDatabase = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem_btLoad = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem_btGetInformation = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem_btUpdateForAllDatabase = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem_WebBrowser1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem_GridControl1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.DefaultLookAndFeel1 = New DevExpress.LookAndFeel.DefaultLookAndFeel(Me.components)
        CType(Me.barManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemComboBox_VendorName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemComboBox_DatabaseName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemComboBox_Year, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControl_Form, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl_Form.SuspendLayout()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tbStrSearch.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup_Form, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup_Input, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem_tbStrSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem_ComboBox_FilterDatabase, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem_btLoad, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem_btGetInformation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem_btUpdateForAllDatabase, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem_WebBrowser1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem_GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'barManager1
        '
        Me.barManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.bar1, Me.bar3})
        Me.barManager1.DockControls.Add(Me.barDockControlTop)
        Me.barManager1.DockControls.Add(Me.barDockControlBottom)
        Me.barManager1.DockControls.Add(Me.barDockControlLeft)
        Me.barManager1.DockControls.Add(Me.barDockControlRight)
        Me.barManager1.Form = Me
        Me.barManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.BarEditItem_VendorName, Me.BarEditItem_DatabaseName, Me.BarEditItem_Year, Me.btLoadDatabase, Me.StatusBar1, Me.StatusBar2, Me.StatusBar3})
        Me.barManager1.MaxItemId = 11
        Me.barManager1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemComboBox_VendorName, Me.RepositoryItemComboBox_DatabaseName, Me.RepositoryItemComboBox_Year})
        Me.barManager1.StatusBar = Me.bar3
        '
        'bar1
        '
        Me.bar1.BarName = "Tools"
        Me.bar1.DockCol = 0
        Me.bar1.DockRow = 0
        Me.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
        Me.bar1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarEditItem_VendorName), New DevExpress.XtraBars.LinkPersistInfo(Me.BarEditItem_DatabaseName), New DevExpress.XtraBars.LinkPersistInfo(Me.BarEditItem_Year), New DevExpress.XtraBars.LinkPersistInfo(Me.btLoadDatabase)})
        Me.bar1.Text = "Tools"
        '
        'BarEditItem_VendorName
        '
        Me.BarEditItem_VendorName.Caption = "Choose Vendor"
        Me.BarEditItem_VendorName.Edit = Me.RepositoryItemComboBox_VendorName
        Me.BarEditItem_VendorName.Id = 2
        Me.BarEditItem_VendorName.Name = "BarEditItem_VendorName"
        '
        'RepositoryItemComboBox_VendorName
        '
        Me.RepositoryItemComboBox_VendorName.AutoHeight = False
        Me.RepositoryItemComboBox_VendorName.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemComboBox_VendorName.Name = "RepositoryItemComboBox_VendorName"
        '
        'BarEditItem_DatabaseName
        '
        Me.BarEditItem_DatabaseName.Caption = "Choose Database"
        Me.BarEditItem_DatabaseName.Edit = Me.RepositoryItemComboBox_DatabaseName
        Me.BarEditItem_DatabaseName.Id = 3
        Me.BarEditItem_DatabaseName.Name = "BarEditItem_DatabaseName"
        '
        'RepositoryItemComboBox_DatabaseName
        '
        Me.RepositoryItemComboBox_DatabaseName.AutoHeight = False
        Me.RepositoryItemComboBox_DatabaseName.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemComboBox_DatabaseName.Name = "RepositoryItemComboBox_DatabaseName"
        '
        'BarEditItem_Year
        '
        Me.BarEditItem_Year.Caption = "Choose Year"
        Me.BarEditItem_Year.Edit = Me.RepositoryItemComboBox_Year
        Me.BarEditItem_Year.Id = 5
        Me.BarEditItem_Year.Name = "BarEditItem_Year"
        '
        'RepositoryItemComboBox_Year
        '
        Me.RepositoryItemComboBox_Year.AutoHeight = False
        Me.RepositoryItemComboBox_Year.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemComboBox_Year.Name = "RepositoryItemComboBox_Year"
        '
        'btLoadDatabase
        '
        Me.btLoadDatabase.Caption = "Load Database"
        Me.btLoadDatabase.Id = 6
        Me.btLoadDatabase.Name = "btLoadDatabase"
        '
        'bar3
        '
        Me.bar3.BarName = "Status bar"
        Me.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
        Me.bar3.DockCol = 0
        Me.bar3.DockRow = 0
        Me.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
        Me.bar3.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.StatusBar1), New DevExpress.XtraBars.LinkPersistInfo(Me.StatusBar2), New DevExpress.XtraBars.LinkPersistInfo(Me.StatusBar3)})
        Me.bar3.OptionsBar.AllowQuickCustomization = False
        Me.bar3.OptionsBar.DrawDragBorder = False
        Me.bar3.OptionsBar.UseWholeRow = True
        Me.bar3.Text = "Status bar"
        '
        'StatusBar1
        '
        Me.StatusBar1.Caption = "BarStaticItem1"
        Me.StatusBar1.Id = 7
        Me.StatusBar1.Name = "StatusBar1"
        '
        'StatusBar2
        '
        Me.StatusBar2.Caption = "BarStaticItem2"
        Me.StatusBar2.Id = 8
        Me.StatusBar2.Name = "StatusBar2"
        '
        'StatusBar3
        '
        Me.StatusBar3.Caption = "BarStaticItem3"
        Me.StatusBar3.Id = 9
        Me.StatusBar3.Name = "StatusBar3"
        '
        'barDockControlTop
        '
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Manager = Me.barManager1
        Me.barDockControlTop.Size = New System.Drawing.Size(905, 27)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 454)
        Me.barDockControlBottom.Manager = Me.barManager1
        Me.barDockControlBottom.Size = New System.Drawing.Size(905, 27)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 27)
        Me.barDockControlLeft.Manager = Me.barManager1
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 427)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(905, 27)
        Me.barDockControlRight.Manager = Me.barManager1
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 427)
        '
        'LayoutControl_Form
        '
        Me.LayoutControl_Form.Controls.Add(Me.GridControl1)
        Me.LayoutControl_Form.Controls.Add(Me.WebBrowser1)
        Me.LayoutControl_Form.Controls.Add(Me.btUpdateForAllDatabase)
        Me.LayoutControl_Form.Controls.Add(Me.btGetInformation)
        Me.LayoutControl_Form.Controls.Add(Me.btLoad)
        Me.LayoutControl_Form.Controls.Add(Me.ComboBox_FilterDatabase)
        Me.LayoutControl_Form.Controls.Add(Me.tbStrSearch)
        Me.LayoutControl_Form.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl_Form.Location = New System.Drawing.Point(0, 27)
        Me.LayoutControl_Form.Name = "LayoutControl_Form"
        Me.LayoutControl_Form.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = New System.Drawing.Rectangle(461, 142, 450, 400)
        Me.LayoutControl_Form.Root = Me.LayoutControlGroup_Form
        Me.LayoutControl_Form.Size = New System.Drawing.Size(905, 427)
        Me.LayoutControl_Form.TabIndex = 4
        Me.LayoutControl_Form.Text = "LayoutControl1"
        '
        'GridControl1
        '
        Me.GridControl1.Location = New System.Drawing.Point(12, 130)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.MenuManager = Me.barManager1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.Size = New System.Drawing.Size(881, 140)
        Me.GridControl1.TabIndex = 13
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.[True]
        Me.GridView1.OptionsBehavior.Editable = False
        '
        'WebBrowser1
        '
        Me.WebBrowser1.Location = New System.Drawing.Point(12, 274)
        Me.WebBrowser1.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.Size = New System.Drawing.Size(881, 141)
        Me.WebBrowser1.TabIndex = 12
        '
        'btUpdateForAllDatabase
        '
        Me.btUpdateForAllDatabase.Location = New System.Drawing.Point(454, 93)
        Me.btUpdateForAllDatabase.Name = "btUpdateForAllDatabase"
        Me.btUpdateForAllDatabase.Size = New System.Drawing.Size(427, 22)
        Me.btUpdateForAllDatabase.StyleController = Me.LayoutControl_Form
        Me.btUpdateForAllDatabase.TabIndex = 11
        Me.btUpdateForAllDatabase.Text = "Update for all Database"
        '
        'btGetInformation
        '
        Me.btGetInformation.Location = New System.Drawing.Point(24, 93)
        Me.btGetInformation.Name = "btGetInformation"
        Me.btGetInformation.Size = New System.Drawing.Size(426, 22)
        Me.btGetInformation.StyleController = Me.LayoutControl_Form
        Me.btGetInformation.TabIndex = 7
        Me.btGetInformation.Text = "Get Mailing Status From Database of Vendor"
        '
        'btLoad
        '
        Me.btLoad.Location = New System.Drawing.Point(454, 67)
        Me.btLoad.Name = "btLoad"
        Me.btLoad.Size = New System.Drawing.Size(427, 22)
        Me.btLoad.StyleController = Me.LayoutControl_Form
        Me.btLoad.TabIndex = 6
        Me.btLoad.Text = "Load"
        '
        'ComboBox_FilterDatabase
        '
        Me.ComboBox_FilterDatabase.FormattingEnabled = True
        Me.ComboBox_FilterDatabase.Location = New System.Drawing.Point(24, 67)
        Me.ComboBox_FilterDatabase.Name = "ComboBox_FilterDatabase"
        Me.ComboBox_FilterDatabase.Size = New System.Drawing.Size(426, 21)
        Me.ComboBox_FilterDatabase.TabIndex = 5
        '
        'tbStrSearch
        '
        Me.tbStrSearch.Location = New System.Drawing.Point(24, 43)
        Me.tbStrSearch.MenuManager = Me.barManager1
        Me.tbStrSearch.Name = "tbStrSearch"
        Me.tbStrSearch.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Repository.ClearButton(), New DevExpress.XtraEditors.Repository.SearchButton()})
        Me.tbStrSearch.Size = New System.Drawing.Size(857, 20)
        Me.tbStrSearch.StyleController = Me.LayoutControl_Form
        Me.tbStrSearch.TabIndex = 4
        '
        'LayoutControlGroup_Form
        '
        Me.LayoutControlGroup_Form.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup_Form.GroupBordersVisible = False
        Me.LayoutControlGroup_Form.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup_Input, Me.LayoutControlItem_WebBrowser1, Me.LayoutControlItem_GridControl1})
        Me.LayoutControlGroup_Form.LayoutMode = DevExpress.XtraLayout.Utils.LayoutMode.Table
        Me.LayoutControlGroup_Form.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup_Form.Name = "Root"
        ColumnDefinition3.SizeType = System.Windows.Forms.SizeType.AutoSize
        ColumnDefinition3.Width = 885.0R
        Me.LayoutControlGroup_Form.OptionsTableLayoutGroup.ColumnDefinitions.AddRange(New DevExpress.XtraLayout.ColumnDefinition() {ColumnDefinition3})
        RowDefinition4.Height = 59.0R
        RowDefinition4.SizeType = System.Windows.Forms.SizeType.AutoSize
        RowDefinition5.Height = 59.0R
        RowDefinition5.SizeType = System.Windows.Forms.SizeType.AutoSize
        RowDefinition6.Height = 48.0R
        RowDefinition6.SizeType = System.Windows.Forms.SizeType.AutoSize
        RowDefinition7.Height = 48.0R
        RowDefinition7.SizeType = System.Windows.Forms.SizeType.AutoSize
        RowDefinition8.Height = 48.0R
        RowDefinition8.SizeType = System.Windows.Forms.SizeType.AutoSize
        RowDefinition9.Height = 48.0R
        RowDefinition9.SizeType = System.Windows.Forms.SizeType.AutoSize
        RowDefinition10.Height = 48.0R
        RowDefinition10.SizeType = System.Windows.Forms.SizeType.AutoSize
        RowDefinition11.Height = 49.0R
        RowDefinition11.SizeType = System.Windows.Forms.SizeType.AutoSize
        Me.LayoutControlGroup_Form.OptionsTableLayoutGroup.RowDefinitions.AddRange(New DevExpress.XtraLayout.RowDefinition() {RowDefinition4, RowDefinition5, RowDefinition6, RowDefinition7, RowDefinition8, RowDefinition9, RowDefinition10, RowDefinition11})
        Me.LayoutControlGroup_Form.Size = New System.Drawing.Size(905, 427)
        Me.LayoutControlGroup_Form.TextVisible = False
        '
        'LayoutControlGroup_Input
        '
        Me.LayoutControlGroup_Input.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem_tbStrSearch, Me.LayoutControlItem_ComboBox_FilterDatabase, Me.LayoutControlItem_btLoad, Me.LayoutControlItem_btGetInformation, Me.LayoutControlItem_btUpdateForAllDatabase})
        Me.LayoutControlGroup_Input.LayoutMode = DevExpress.XtraLayout.Utils.LayoutMode.Table
        Me.LayoutControlGroup_Input.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup_Input.Name = "LayoutControlGroup_Input"
        ColumnDefinition1.SizeType = System.Windows.Forms.SizeType.Percent
        ColumnDefinition1.Width = 50.0R
        ColumnDefinition2.SizeType = System.Windows.Forms.SizeType.Percent
        ColumnDefinition2.Width = 50.0R
        Me.LayoutControlGroup_Input.OptionsTableLayoutGroup.ColumnDefinitions.AddRange(New DevExpress.XtraLayout.ColumnDefinition() {ColumnDefinition1, ColumnDefinition2})
        RowDefinition1.Height = 24.0R
        RowDefinition1.SizeType = System.Windows.Forms.SizeType.AutoSize
        RowDefinition2.Height = 26.0R
        RowDefinition2.SizeType = System.Windows.Forms.SizeType.AutoSize
        RowDefinition3.Height = 26.0R
        RowDefinition3.SizeType = System.Windows.Forms.SizeType.AutoSize
        Me.LayoutControlGroup_Input.OptionsTableLayoutGroup.RowDefinitions.AddRange(New DevExpress.XtraLayout.RowDefinition() {RowDefinition1, RowDefinition2, RowDefinition3})
        Me.LayoutControlGroup_Input.OptionsTableLayoutItem.RowSpan = 2
        Me.LayoutControlGroup_Input.Size = New System.Drawing.Size(885, 118)
        Me.LayoutControlGroup_Input.Text = "Select Data:"
        '
        'LayoutControlItem_tbStrSearch
        '
        Me.LayoutControlItem_tbStrSearch.Control = Me.tbStrSearch
        Me.LayoutControlItem_tbStrSearch.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem_tbStrSearch.Name = "LayoutControlItem_tbStrSearch"
        Me.LayoutControlItem_tbStrSearch.OptionsTableLayoutItem.ColumnSpan = 2
        Me.LayoutControlItem_tbStrSearch.Size = New System.Drawing.Size(861, 24)
        Me.LayoutControlItem_tbStrSearch.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem_tbStrSearch.TextVisible = False
        '
        'LayoutControlItem_ComboBox_FilterDatabase
        '
        Me.LayoutControlItem_ComboBox_FilterDatabase.Control = Me.ComboBox_FilterDatabase
        Me.LayoutControlItem_ComboBox_FilterDatabase.Location = New System.Drawing.Point(0, 24)
        Me.LayoutControlItem_ComboBox_FilterDatabase.Name = "LayoutControlItem_ComboBox_FilterDatabase"
        Me.LayoutControlItem_ComboBox_FilterDatabase.OptionsTableLayoutItem.RowIndex = 1
        Me.LayoutControlItem_ComboBox_FilterDatabase.Size = New System.Drawing.Size(430, 26)
        Me.LayoutControlItem_ComboBox_FilterDatabase.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem_ComboBox_FilterDatabase.TextVisible = False
        '
        'LayoutControlItem_btLoad
        '
        Me.LayoutControlItem_btLoad.Control = Me.btLoad
        Me.LayoutControlItem_btLoad.Location = New System.Drawing.Point(430, 24)
        Me.LayoutControlItem_btLoad.Name = "LayoutControlItem_btLoad"
        Me.LayoutControlItem_btLoad.OptionsTableLayoutItem.ColumnIndex = 1
        Me.LayoutControlItem_btLoad.OptionsTableLayoutItem.RowIndex = 1
        Me.LayoutControlItem_btLoad.Size = New System.Drawing.Size(431, 26)
        Me.LayoutControlItem_btLoad.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem_btLoad.TextVisible = False
        '
        'LayoutControlItem_btGetInformation
        '
        Me.LayoutControlItem_btGetInformation.Control = Me.btGetInformation
        Me.LayoutControlItem_btGetInformation.Location = New System.Drawing.Point(0, 50)
        Me.LayoutControlItem_btGetInformation.Name = "LayoutControlItem_btGetInformation"
        Me.LayoutControlItem_btGetInformation.OptionsTableLayoutItem.RowIndex = 2
        Me.LayoutControlItem_btGetInformation.Size = New System.Drawing.Size(430, 26)
        Me.LayoutControlItem_btGetInformation.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem_btGetInformation.TextVisible = False
        '
        'LayoutControlItem_btUpdateForAllDatabase
        '
        Me.LayoutControlItem_btUpdateForAllDatabase.Control = Me.btUpdateForAllDatabase
        Me.LayoutControlItem_btUpdateForAllDatabase.Location = New System.Drawing.Point(430, 50)
        Me.LayoutControlItem_btUpdateForAllDatabase.Name = "LayoutControlItem_btUpdateForAllDatabase"
        Me.LayoutControlItem_btUpdateForAllDatabase.OptionsTableLayoutItem.ColumnIndex = 1
        Me.LayoutControlItem_btUpdateForAllDatabase.OptionsTableLayoutItem.RowIndex = 2
        Me.LayoutControlItem_btUpdateForAllDatabase.Size = New System.Drawing.Size(431, 26)
        Me.LayoutControlItem_btUpdateForAllDatabase.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem_btUpdateForAllDatabase.TextVisible = False
        '
        'LayoutControlItem_WebBrowser1
        '
        Me.LayoutControlItem_WebBrowser1.Control = Me.WebBrowser1
        Me.LayoutControlItem_WebBrowser1.Location = New System.Drawing.Point(0, 262)
        Me.LayoutControlItem_WebBrowser1.Name = "LayoutControlItem_WebBrowser1"
        Me.LayoutControlItem_WebBrowser1.OptionsTableLayoutItem.RowIndex = 5
        Me.LayoutControlItem_WebBrowser1.OptionsTableLayoutItem.RowSpan = 3
        Me.LayoutControlItem_WebBrowser1.Size = New System.Drawing.Size(885, 145)
        Me.LayoutControlItem_WebBrowser1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem_WebBrowser1.TextVisible = False
        '
        'LayoutControlItem_GridControl1
        '
        Me.LayoutControlItem_GridControl1.Control = Me.GridControl1
        Me.LayoutControlItem_GridControl1.Location = New System.Drawing.Point(0, 118)
        Me.LayoutControlItem_GridControl1.Name = "LayoutControlItem_GridControl1"
        Me.LayoutControlItem_GridControl1.OptionsTableLayoutItem.RowIndex = 2
        Me.LayoutControlItem_GridControl1.OptionsTableLayoutItem.RowSpan = 3
        Me.LayoutControlItem_GridControl1.Size = New System.Drawing.Size(885, 144)
        Me.LayoutControlItem_GridControl1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem_GridControl1.TextVisible = False
        '
        'DefaultLookAndFeel1
        '
        Me.DefaultLookAndFeel1.LookAndFeel.SkinName = "Office 2010 Blue"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(905, 481)
        Me.Controls.Add(Me.LayoutControl_Form)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.Name = "Form1"
        Me.Text = "Update Database [Vendor Management]"
        CType(Me.barManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemComboBox_VendorName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemComboBox_DatabaseName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemComboBox_Year, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControl_Form, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl_Form.ResumeLayout(False)
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tbStrSearch.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup_Form, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup_Input, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem_tbStrSearch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem_ComboBox_FilterDatabase, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem_btLoad, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem_btGetInformation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem_btUpdateForAllDatabase, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem_WebBrowser1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem_GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private WithEvents barManager1 As DevExpress.XtraBars.BarManager
    Private WithEvents bar1 As DevExpress.XtraBars.Bar
    Private WithEvents bar3 As DevExpress.XtraBars.Bar
    Private WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Private WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Private WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Private WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Friend WithEvents LayoutControl_Form As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents LayoutControlGroup_Form As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents BarEditItem_VendorName As DevExpress.XtraBars.BarEditItem
    Friend WithEvents RepositoryItemComboBox_VendorName As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents BarEditItem_DatabaseName As DevExpress.XtraBars.BarEditItem
    Friend WithEvents RepositoryItemComboBox_DatabaseName As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents BarEditItem_Year As DevExpress.XtraBars.BarEditItem
    Friend WithEvents RepositoryItemComboBox_Year As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents btLoadDatabase As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents tbStrSearch As DevExpress.XtraEditors.SearchControl
    Friend WithEvents btGetInformation As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btLoad As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents ComboBox_FilterDatabase As ComboBox
    Friend WithEvents StatusBar1 As DevExpress.XtraBars.BarStaticItem
    Friend WithEvents StatusBar2 As DevExpress.XtraBars.BarStaticItem
    Friend WithEvents StatusBar3 As DevExpress.XtraBars.BarStaticItem
    Friend WithEvents DefaultLookAndFeel1 As DevExpress.LookAndFeel.DefaultLookAndFeel
    Friend WithEvents LayoutControlGroup_Input As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem_tbStrSearch As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem_ComboBox_FilterDatabase As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem_btLoad As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem_btGetInformation As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents btUpdateForAllDatabase As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem_btUpdateForAllDatabase As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents WebBrowser1 As WebBrowser
    Friend WithEvents LayoutControlItem_WebBrowser1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem_GridControl1 As DevExpress.XtraLayout.LayoutControlItem
End Class
