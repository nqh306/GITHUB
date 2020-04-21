<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Config
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
        Dim ColumnDefinition1 As DevExpress.XtraLayout.ColumnDefinition = New DevExpress.XtraLayout.ColumnDefinition()
        Dim ColumnDefinition2 As DevExpress.XtraLayout.ColumnDefinition = New DevExpress.XtraLayout.ColumnDefinition()
        Dim ColumnDefinition3 As DevExpress.XtraLayout.ColumnDefinition = New DevExpress.XtraLayout.ColumnDefinition()
        Dim RowDefinition1 As DevExpress.XtraLayout.RowDefinition = New DevExpress.XtraLayout.RowDefinition()
        Dim RowDefinition2 As DevExpress.XtraLayout.RowDefinition = New DevExpress.XtraLayout.RowDefinition()
        Dim RowDefinition3 As DevExpress.XtraLayout.RowDefinition = New DevExpress.XtraLayout.RowDefinition()
        Dim RowDefinition4 As DevExpress.XtraLayout.RowDefinition = New DevExpress.XtraLayout.RowDefinition()
        Dim RowDefinition5 As DevExpress.XtraLayout.RowDefinition = New DevExpress.XtraLayout.RowDefinition()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Config))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.btChangeGlobalConfig = New DevExpress.XtraEditors.SimpleButton()
        Me.tbLinkGlobalConfig = New DevExpress.XtraEditors.ButtonEdit()
        Me.tbDatabaseUser = New DevExpress.XtraEditors.ButtonEdit()
        Me.btChangeDatabaseUser = New DevExpress.XtraEditors.SimpleButton()
        Me.btChange = New DevExpress.XtraEditors.SimpleButton()
        Me.tbFolderPath = New DevExpress.XtraEditors.ButtonEdit()
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem_tbFolderPath = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem_btChange = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem_btChangeDatabaseUser = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem_tbDatabaseUser = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem_tbLinkGlobalConfig = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem_btChangeGlobalConfig = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.tbLinkGlobalConfig.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tbDatabaseUser.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tbFolderPath.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem_tbFolderPath, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem_btChange, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem_btChangeDatabaseUser, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem_tbDatabaseUser, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem_tbLinkGlobalConfig, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem_btChangeGlobalConfig, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.btChangeGlobalConfig)
        Me.LayoutControl1.Controls.Add(Me.tbLinkGlobalConfig)
        Me.LayoutControl1.Controls.Add(Me.tbDatabaseUser)
        Me.LayoutControl1.Controls.Add(Me.btChangeDatabaseUser)
        Me.LayoutControl1.Controls.Add(Me.btChange)
        Me.LayoutControl1.Controls.Add(Me.tbFolderPath)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        Me.LayoutControl1.Size = New System.Drawing.Size(709, 442)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'btChangeGlobalConfig
        '
        Me.btChangeGlobalConfig.Location = New System.Drawing.Point(471, 64)
        Me.btChangeGlobalConfig.Name = "btChangeGlobalConfig"
        Me.btChangeGlobalConfig.Size = New System.Drawing.Size(226, 22)
        Me.btChangeGlobalConfig.StyleController = Me.LayoutControl1
        Me.btChangeGlobalConfig.TabIndex = 10
        Me.btChangeGlobalConfig.Text = "Change Link Global Config"
        '
        'tbLinkGlobalConfig
        '
        Me.tbLinkGlobalConfig.Location = New System.Drawing.Point(86, 64)
        Me.tbLinkGlobalConfig.Name = "tbLinkGlobalConfig"
        Me.tbLinkGlobalConfig.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.tbLinkGlobalConfig.Size = New System.Drawing.Size(381, 20)
        Me.tbLinkGlobalConfig.StyleController = Me.LayoutControl1
        Me.tbLinkGlobalConfig.TabIndex = 9
        '
        'tbDatabaseUser
        '
        Me.tbDatabaseUser.Location = New System.Drawing.Point(86, 38)
        Me.tbDatabaseUser.Name = "tbDatabaseUser"
        Me.tbDatabaseUser.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.tbDatabaseUser.Size = New System.Drawing.Size(381, 20)
        Me.tbDatabaseUser.StyleController = Me.LayoutControl1
        Me.tbDatabaseUser.TabIndex = 8
        '
        'btChangeDatabaseUser
        '
        Me.btChangeDatabaseUser.Location = New System.Drawing.Point(471, 38)
        Me.btChangeDatabaseUser.Name = "btChangeDatabaseUser"
        Me.btChangeDatabaseUser.Size = New System.Drawing.Size(226, 22)
        Me.btChangeDatabaseUser.StyleController = Me.LayoutControl1
        Me.btChangeDatabaseUser.TabIndex = 7
        Me.btChangeDatabaseUser.Text = "Change Database User"
        '
        'btChange
        '
        Me.btChange.Location = New System.Drawing.Point(471, 12)
        Me.btChange.Name = "btChange"
        Me.btChange.Size = New System.Drawing.Size(226, 22)
        Me.btChange.StyleController = Me.LayoutControl1
        Me.btChange.TabIndex = 5
        Me.btChange.Text = "Change Folder Path"
        '
        'tbFolderPath
        '
        Me.tbFolderPath.Location = New System.Drawing.Point(86, 12)
        Me.tbFolderPath.Name = "tbFolderPath"
        Me.tbFolderPath.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.tbFolderPath.Size = New System.Drawing.Size(381, 20)
        Me.tbFolderPath.StyleController = Me.LayoutControl1
        Me.tbFolderPath.TabIndex = 4
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup1.GroupBordersVisible = False
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem_tbFolderPath, Me.LayoutControlItem_btChange, Me.LayoutControlItem_btChangeDatabaseUser, Me.LayoutControlItem_tbDatabaseUser, Me.LayoutControlItem_tbLinkGlobalConfig, Me.LayoutControlItem_btChangeGlobalConfig})
        Me.LayoutControlGroup1.LayoutMode = DevExpress.XtraLayout.Utils.LayoutMode.Table
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        ColumnDefinition1.SizeType = System.Windows.Forms.SizeType.Percent
        ColumnDefinition1.Width = 33.333333333333336R
        ColumnDefinition2.SizeType = System.Windows.Forms.SizeType.Percent
        ColumnDefinition2.Width = 33.333333333333336R
        ColumnDefinition3.SizeType = System.Windows.Forms.SizeType.Percent
        ColumnDefinition3.Width = 33.333333333333336R
        Me.LayoutControlGroup1.OptionsTableLayoutGroup.ColumnDefinitions.AddRange(New DevExpress.XtraLayout.ColumnDefinition() {ColumnDefinition1, ColumnDefinition2, ColumnDefinition3})
        RowDefinition1.Height = 26.0R
        RowDefinition1.SizeType = System.Windows.Forms.SizeType.AutoSize
        RowDefinition2.Height = 26.0R
        RowDefinition2.SizeType = System.Windows.Forms.SizeType.AutoSize
        RowDefinition3.Height = 26.0R
        RowDefinition3.SizeType = System.Windows.Forms.SizeType.AutoSize
        RowDefinition4.Height = 20.0R
        RowDefinition4.SizeType = System.Windows.Forms.SizeType.AutoSize
        RowDefinition5.Height = 324.0R
        RowDefinition5.SizeType = System.Windows.Forms.SizeType.AutoSize
        Me.LayoutControlGroup1.OptionsTableLayoutGroup.RowDefinitions.AddRange(New DevExpress.XtraLayout.RowDefinition() {RowDefinition1, RowDefinition2, RowDefinition3, RowDefinition4, RowDefinition5})
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(709, 442)
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem_tbFolderPath
        '
        Me.LayoutControlItem_tbFolderPath.Control = Me.tbFolderPath
        Me.LayoutControlItem_tbFolderPath.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem_tbFolderPath.Name = "LayoutControlItem_tbFolderPath"
        Me.LayoutControlItem_tbFolderPath.OptionsTableLayoutItem.ColumnSpan = 2
        Me.LayoutControlItem_tbFolderPath.Size = New System.Drawing.Size(459, 26)
        Me.LayoutControlItem_tbFolderPath.Text = "Folder Path"
        Me.LayoutControlItem_tbFolderPath.TextSize = New System.Drawing.Size(71, 13)
        '
        'LayoutControlItem_btChange
        '
        Me.LayoutControlItem_btChange.Control = Me.btChange
        Me.LayoutControlItem_btChange.Location = New System.Drawing.Point(459, 0)
        Me.LayoutControlItem_btChange.Name = "LayoutControlItem_btChange"
        Me.LayoutControlItem_btChange.OptionsTableLayoutItem.ColumnIndex = 2
        Me.LayoutControlItem_btChange.Size = New System.Drawing.Size(230, 26)
        Me.LayoutControlItem_btChange.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem_btChange.TextVisible = False
        '
        'LayoutControlItem_btChangeDatabaseUser
        '
        Me.LayoutControlItem_btChangeDatabaseUser.Control = Me.btChangeDatabaseUser
        Me.LayoutControlItem_btChangeDatabaseUser.Location = New System.Drawing.Point(459, 26)
        Me.LayoutControlItem_btChangeDatabaseUser.Name = "LayoutControlItem_btChangeDatabaseUser"
        Me.LayoutControlItem_btChangeDatabaseUser.OptionsTableLayoutItem.ColumnIndex = 2
        Me.LayoutControlItem_btChangeDatabaseUser.OptionsTableLayoutItem.RowIndex = 1
        Me.LayoutControlItem_btChangeDatabaseUser.Size = New System.Drawing.Size(230, 26)
        Me.LayoutControlItem_btChangeDatabaseUser.Text = "Change Database User"
        Me.LayoutControlItem_btChangeDatabaseUser.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem_btChangeDatabaseUser.TextVisible = False
        '
        'LayoutControlItem_tbDatabaseUser
        '
        Me.LayoutControlItem_tbDatabaseUser.Control = Me.tbDatabaseUser
        Me.LayoutControlItem_tbDatabaseUser.Location = New System.Drawing.Point(0, 26)
        Me.LayoutControlItem_tbDatabaseUser.Name = "LayoutControlItem_tbDatabaseUser"
        Me.LayoutControlItem_tbDatabaseUser.OptionsTableLayoutItem.ColumnSpan = 2
        Me.LayoutControlItem_tbDatabaseUser.OptionsTableLayoutItem.RowIndex = 1
        Me.LayoutControlItem_tbDatabaseUser.Size = New System.Drawing.Size(459, 26)
        Me.LayoutControlItem_tbDatabaseUser.Text = "Database User"
        Me.LayoutControlItem_tbDatabaseUser.TextSize = New System.Drawing.Size(71, 13)
        '
        'LayoutControlItem_tbLinkGlobalConfig
        '
        Me.LayoutControlItem_tbLinkGlobalConfig.Control = Me.tbLinkGlobalConfig
        Me.LayoutControlItem_tbLinkGlobalConfig.Location = New System.Drawing.Point(0, 52)
        Me.LayoutControlItem_tbLinkGlobalConfig.Name = "LayoutControlItem_tbLinkGlobalConfig"
        Me.LayoutControlItem_tbLinkGlobalConfig.OptionsTableLayoutItem.ColumnSpan = 2
        Me.LayoutControlItem_tbLinkGlobalConfig.OptionsTableLayoutItem.RowIndex = 2
        Me.LayoutControlItem_tbLinkGlobalConfig.Size = New System.Drawing.Size(459, 26)
        Me.LayoutControlItem_tbLinkGlobalConfig.Text = "Global Config"
        Me.LayoutControlItem_tbLinkGlobalConfig.TextSize = New System.Drawing.Size(71, 13)
        '
        'LayoutControlItem_btChangeGlobalConfig
        '
        Me.LayoutControlItem_btChangeGlobalConfig.Control = Me.btChangeGlobalConfig
        Me.LayoutControlItem_btChangeGlobalConfig.Location = New System.Drawing.Point(459, 52)
        Me.LayoutControlItem_btChangeGlobalConfig.Name = "LayoutControlItem_btChangeGlobalConfig"
        Me.LayoutControlItem_btChangeGlobalConfig.OptionsTableLayoutItem.ColumnIndex = 2
        Me.LayoutControlItem_btChangeGlobalConfig.OptionsTableLayoutItem.RowIndex = 2
        Me.LayoutControlItem_btChangeGlobalConfig.Size = New System.Drawing.Size(230, 26)
        Me.LayoutControlItem_btChangeGlobalConfig.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem_btChangeGlobalConfig.TextVisible = False
        '
        'Config
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(709, 442)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Config"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Config"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.tbLinkGlobalConfig.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tbDatabaseUser.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tbFolderPath.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem_tbFolderPath, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem_btChange, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem_btChangeDatabaseUser, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem_tbDatabaseUser, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem_tbLinkGlobalConfig, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem_btChangeGlobalConfig, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents tbFolderPath As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents LayoutControlItem_tbFolderPath As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents btChange As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem_btChange As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents btChangeDatabaseUser As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem_btChangeDatabaseUser As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents tbDatabaseUser As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents LayoutControlItem_tbDatabaseUser As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents tbLinkGlobalConfig As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents LayoutControlItem_tbLinkGlobalConfig As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents btChangeGlobalConfig As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem_btChangeGlobalConfig As DevExpress.XtraLayout.LayoutControlItem
End Class
