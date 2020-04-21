Imports DevExpress.XtraEditors.Controls
Public Class Add_Item
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public link_app_config As String = appPath & "App_Config.txt"
    Public folder_path As String
    Public folder_path_save_file As String
    Public link_database_DA As String

    Sub refresh_link()
        folder_path = SQL_QUERY_TO_STRING(link_app_config, "SELECT Field_Value FROM Config WHERE Field_Name = 'Link_Folder_Path_DA'")
        If folder_path.Substring(folder_path.Length - 1, 1) <> "\" Then
            folder_path = folder_path & "\"
        End If
        folder_path_save_file = folder_path & "ARCHIVE"
        link_database_DA = folder_path & "Database_DA.txt"
    End Sub

    Private Sub Add_Item_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        refresh_link()

        tbLinkPDF.ReadOnly = True

        'autocomplete tu dong dien thong tin cho textbox tbDAFrom_Name, tbDATo_Name
        Dim DataCollection_DA_Name As New AutoCompleteStringCollection()
        Module_DA.AUTO_COMPLETE_GET_DATA(DataCollection_DA_Name, link_database_DA, "SELECT DISTINCT DA_From FROM Main_DA")
        Module_DA.AUTO_COMPLETE_GET_DATA(DataCollection_DA_Name, link_database_DA, "SELECT DISTINCT DA_To FROM Main_DA")
        tbDAFrom_Name.AutoCompleteMode = AutoCompleteMode.Suggest
        tbDAFrom_Name.AutoCompleteSource = AutoCompleteSource.CustomSource
        tbDAFrom_Name.AutoCompleteCustomSource = DataCollection_DA_Name

        tbDATo_Name.AutoCompleteMode = AutoCompleteMode.Suggest
        tbDATo_Name.AutoCompleteSource = AutoCompleteSource.CustomSource
        tbDATo_Name.AutoCompleteCustomSource = DataCollection_DA_Name

        'autocomplete tu dong dien thong tin cho textbox tbDAFrom_PWID, tbDATo_PWID
        Dim DataCollection_DA_PWID As New AutoCompleteStringCollection()
        Module_DA.AUTO_COMPLETE_GET_DATA(DataCollection_DA_PWID, link_database_DA, "SELECT DISTINCT PWID_DA_From FROM Main_DA")
        Module_DA.AUTO_COMPLETE_GET_DATA(DataCollection_DA_PWID, link_database_DA, "SELECT DISTINCT PWID_DA_To FROM Main_DA")
        tbDAFrom_PWID.AutoCompleteMode = AutoCompleteMode.Suggest
        tbDAFrom_PWID.AutoCompleteSource = AutoCompleteSource.CustomSource
        tbDAFrom_PWID.AutoCompleteCustomSource = DataCollection_DA_PWID

        tbDATo_PWID.AutoCompleteMode = AutoCompleteMode.Suggest
        tbDATo_PWID.AutoCompleteSource = AutoCompleteSource.CustomSource
        tbDATo_PWID.AutoCompleteCustomSource = DataCollection_DA_PWID

        'autocomplete tu dong dien thong tin cho textbox tbDAFrom_Position, tbDATo_Position
        Dim DataCollection_DA_Position As New AutoCompleteStringCollection()
        Module_DA.AUTO_COMPLETE_GET_DATA(DataCollection_DA_Position, link_database_DA, "SELECT DISTINCT Position_To FROM Main_DA")
        Module_DA.AUTO_COMPLETE_GET_DATA(DataCollection_DA_Position, link_database_DA, "SELECT DISTINCT Position_From FROM Main_DA")
        tbDAFrom_Position.AutoCompleteMode = AutoCompleteMode.Suggest
        tbDAFrom_Position.AutoCompleteSource = AutoCompleteSource.CustomSource
        tbDAFrom_Position.AutoCompleteCustomSource = DataCollection_DA_Position

        tbDATo_Position.AutoCompleteMode = AutoCompleteMode.Suggest
        tbDATo_Position.AutoCompleteSource = AutoCompleteSource.CustomSource
        tbDATo_Position.AutoCompleteCustomSource = DataCollection_DA_Position

        'autocomplete tu dong dien thong tin cho textbox tbDepartment
        Dim DataCollection_DA_Department As New AutoCompleteStringCollection()
        Module_DA.AUTO_COMPLETE_GET_DATA(DataCollection_DA_Department, link_database_DA, "SELECT DISTINCT Department FROM Main_DA")
        tbDepartment.AutoCompleteMode = AutoCompleteMode.Suggest
        tbDepartment.AutoCompleteSource = AutoCompleteSource.CustomSource
        tbDepartment.AutoCompleteCustomSource = DataCollection_DA_Department

        'autocomplete tu dong dien thong tin cho textbox tbDepartment
        Dim DataCollection_DA_Delegation As New AutoCompleteStringCollection()
        Module_DA.AUTO_COMPLETE_GET_DATA(DataCollection_DA_Delegation, link_database_DA, "SELECT DISTINCT Delegation FROM Main_DA")
        tbDelegation.AutoCompleteMode = AutoCompleteMode.Suggest
        tbDelegation.AutoCompleteSource = AutoCompleteSource.CustomSource
        tbDelegation.AutoCompleteCustomSource = DataCollection_DA_Delegation

        'autocomplete tu dong dien thong tin cho textbox tbDepartment
        Dim DataCollection_DA_Note As New AutoCompleteStringCollection()
        Module_DA.AUTO_COMPLETE_GET_DATA(DataCollection_DA_Note, link_database_DA, "SELECT DISTINCT Note FROM Main_DA")
        tbNote.AutoCompleteMode = AutoCompleteMode.Suggest
        tbNote.AutoCompleteSource = AutoCompleteSource.CustomSource
        tbNote.AutoCompleteCustomSource = DataCollection_DA_Note

        tbEffectiveDate.Format = DateTimePickerFormat.Custom
        tbEffectiveDate.CustomFormat = "dd/MM/yyyy"
        tbExpiryDate.Format = DateTimePickerFormat.Custom
        tbExpiryDate.CustomFormat = "dd/MM/yyyy"
        tbExpiryDate.Value = DateTime.ParseExact("31/12/2099", "dd/MM/yyyy", Nothing)

        'add value to ComboBox_Segment
        ComboBox_Segment.Items.Add("WB")
        ComboBox_Segment.Items.Add("SME")
        ComboBox_Segment.Items.Add("INDIVIDUAL")

        'Add column for DataGridView
        Dim col1 As New DataGridViewTextBoxColumn
        col1.Name = "SeqNo"
        DataGridView1.Columns.Add(col1)
        Dim col2 As New DataGridViewTextBoxColumn
        col2.Name = "DAFrom_PWID"
        DataGridView1.Columns.Add(col2)
        Dim col3 As New DataGridViewTextBoxColumn
        col3.Name = "DAFrom_Name"
        DataGridView1.Columns.Add(col3)
        Dim col4 As New DataGridViewTextBoxColumn
        col4.Name = "DAFrom_Position"
        DataGridView1.Columns.Add(col4)
        Dim col5 As New DataGridViewTextBoxColumn
        col5.Name = "DATo_PWID"
        DataGridView1.Columns.Add(col5)
        Dim col6 As New DataGridViewTextBoxColumn
        col6.Name = "DATo_Name"
        DataGridView1.Columns.Add(col6)
        Dim col7 As New DataGridViewTextBoxColumn
        col7.Name = "DATo_Position"
        DataGridView1.Columns.Add(col7)
        Dim col8 As New DataGridViewTextBoxColumn
        col8.Name = "Segment"
        DataGridView1.Columns.Add(col8)
        Dim col9 As New DataGridViewTextBoxColumn
        col9.Name = "Department"
        DataGridView1.Columns.Add(col9)
        Dim col10 As New DataGridViewTextBoxColumn
        col10.Name = "Effective Date"
        DataGridView1.Columns.Add(col10)
        Dim col11 As New DataGridViewTextBoxColumn
        col11.Name = "Expiry Date"
        DataGridView1.Columns.Add(col11)
        Dim col12 As New DataGridViewTextBoxColumn
        col12.Name = "Delegation"
        DataGridView1.Columns.Add(col12)
        Dim col13 As New DataGridViewTextBoxColumn
        col13.Name = "Note"
        DataGridView1.Columns.Add(col13)
    End Sub

    Private Sub tbLinkPDF_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles tbLinkPDF.ButtonClick
        On Error GoTo err_handle

        btClear.PerformClick()

        DataGridView1.Rows.Clear()

        Dim fd As OpenFileDialog = New OpenFileDialog()
        fd.Title = "Select PDF file - DA Details"
        fd.InitialDirectory = "C:\"
        fd.Filter = "PDF File|*.pdf"

        fd.FilterIndex = 2
        fd.RestoreDirectory = True

        If fd.ShowDialog() = DialogResult.OK Then
            If fd.FileName.Length > 0 Then
                tbLinkPDF.Text = fd.FileName
            End If
        End If
        btCreate.Enabled = True

        PDFViewer.src = fd.FileName

        Exit Sub
err_handle:
        Module_DA.Error_handle()
    End Sub

    Private Sub tbDAFrom_PWID_TextChanged(sender As Object, e As EventArgs) Handles tbDAFrom_PWID.TextChanged
        tbDAFrom_Name.Text = Module_DA.SQL_QUERY_TO_STRING(link_database_DA, "SELECT DISTINCT DA_From FROM Main_DA WHERE PWID_DA_From = '" & tbDAFrom_PWID.Text & "'")
        If tbDAFrom_Name.Text.Length = 0 Then
            tbDAFrom_Name.Text = Module_DA.SQL_QUERY_TO_STRING(link_database_DA, "SELECT DISTINCT DA_To FROM Main_DA WHERE PWID_DA_To = '" & tbDAFrom_PWID.Text & "'")
        End If

        tbDAFrom_Position.Text = Module_DA.SQL_QUERY_TO_STRING(link_database_DA, "SELECT DISTINCT Position_From FROM Main_DA WHERE PWID_DA_From = '" & tbDAFrom_PWID.Text & "'")
        If tbDAFrom_Position.Text.Length = 0 Then
            tbDAFrom_Position.Text = Module_DA.SQL_QUERY_TO_STRING(link_database_DA, "SELECT DISTINCT Position_To FROM Main_DA WHERE PWID_DA_To = '" & tbDAFrom_PWID.Text & "'")
        End If
    End Sub

    Private Sub tbDATo_PWID_TextChanged(sender As Object, e As EventArgs) Handles tbDATo_PWID.TextChanged
        tbDATo_Name.Text = Module_DA.SQL_QUERY_TO_STRING(link_database_DA, "SELECT DISTINCT DA_To FROM Main_DA WHERE PWID_DA_To = '" & tbDATo_PWID.Text & "'")
        If tbDATo_Name.Text.Length = 0 Then
            tbDATo_Name.Text = Module_DA.SQL_QUERY_TO_STRING(link_database_DA, "SELECT DISTINCT DA_From FROM Main_DA WHERE PWID_DA_From = '" & tbDATo_PWID.Text & "'")
        End If

        tbDATo_Position.Text = Module_DA.SQL_QUERY_TO_STRING(link_database_DA, "SELECT DISTINCT Position_To FROM Main_DA WHERE PWID_DA_To = '" & tbDATo_PWID.Text & "'")
        If tbDATo_Position.Text.Length = 0 Then
            tbDATo_Position.Text = Module_DA.SQL_QUERY_TO_STRING(link_database_DA, "SELECT DISTINCT Position_From FROM Main_DA WHERE PWID_DA_From = '" & tbDATo_PWID.Text & "'")
        End If
    End Sub

    Private Sub DataGridView1_CellMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseDoubleClick

        Dim rowUp As Integer = e.RowIndex

        If Len(DataGridView1.Rows(rowUp).Cells(0).Value.ToString()) > 0 Then
            tbSequence.Text = DataGridView1.Rows(rowUp).Cells(0).Value.ToString()
        Else
            tbSequence.Text = ""
        End If
        If Len(DataGridView1.Rows(rowUp).Cells(1).Value.ToString()) > 0 Then
            tbDAFrom_PWID.Text = DataGridView1.Rows(rowUp).Cells(1).Value.ToString()
        Else
            tbDAFrom_PWID.Text = ""
        End If
        If Len(DataGridView1.Rows(rowUp).Cells(2).Value.ToString()) > 0 Then
            tbDAFrom_Name.Text = DataGridView1.Rows(rowUp).Cells(2).Value.ToString()
        Else
            tbDAFrom_Name.Text = ""
        End If
        If Len(DataGridView1.Rows(rowUp).Cells(3).Value.ToString()) > 0 Then
            tbDAFrom_Position.Text = DataGridView1.Rows(rowUp).Cells(3).Value.ToString()
        Else
            tbDAFrom_Position.Text = ""
        End If
        If Len(DataGridView1.Rows(rowUp).Cells(4).Value.ToString()) > 0 Then
            tbDATo_PWID.Text = DataGridView1.Rows(rowUp).Cells(4).Value.ToString()
        Else
            tbDATo_PWID.Text = ""
        End If
        If Len(DataGridView1.Rows(rowUp).Cells(5).Value.ToString()) > 0 Then
            tbDATo_Name.Text = DataGridView1.Rows(rowUp).Cells(5).Value.ToString()
        Else
            tbDATo_Name.Text = ""
        End If
        If Len(DataGridView1.Rows(rowUp).Cells(6).Value.ToString()) > 0 Then
            tbDATo_Position.Text = DataGridView1.Rows(rowUp).Cells(6).Value.ToString()
        Else
            tbDATo_Position.Text = ""
        End If
        If Len(DataGridView1.Rows(rowUp).Cells(7).Value.ToString()) > 0 Then
            ComboBox_Segment.Text = DataGridView1.Rows(rowUp).Cells(7).Value.ToString()
        Else
            ComboBox_Segment.Text = ""
        End If
        If Len(DataGridView1.Rows(rowUp).Cells(8).Value.ToString()) > 0 Then
            tbDepartment.Text = DataGridView1.Rows(rowUp).Cells(8).Value.ToString()
        Else
            tbDepartment.Text = ""
        End If
        If Len(DataGridView1.Rows(rowUp).Cells(9).Value.ToString()) > 0 Then
            tbEffectiveDate.Value = DateTime.ParseExact(DataGridView1.Rows(rowUp).Cells(9).Value.ToString(), "dd/MM/yyyy", Nothing)
        Else
            tbEffectiveDate.Value = Now.ToString("dd/MM/yyyy")
        End If
        If Len(DataGridView1.Rows(rowUp).Cells(10).Value.ToString()) > 0 Then
            tbExpiryDate.Value = DateTime.ParseExact(DataGridView1.Rows(rowUp).Cells(10).Value.ToString(), "dd/MM/yyyy", Nothing)
        Else
            tbExpiryDate.Value = Now.ToString("dd/MM/yyyy")
        End If
        If Len(DataGridView1.Rows(rowUp).Cells(11).Value.ToString()) > 0 Then
            tbDelegation.Text = DataGridView1.Rows(rowUp).Cells(11).Value.ToString()
        Else
            tbDelegation.Text = ""
        End If
        If Len(DataGridView1.Rows(rowUp).Cells(12).Value.ToString()) > 0 Then
            tbNote.Text = DataGridView1.Rows(rowUp).Cells(12).Value.ToString()
        Else
            tbNote.Text = ""
        End If
    End Sub
    Private Sub DataGridView1_KeyDown(sender As Object, e As KeyEventArgs) Handles DataGridView1.KeyDown
        Dim rowUp As Integer = DataGridView1.CurrentRow.Index

        If e.KeyCode = Keys.Down Then
            If (rowUp >= DataGridView1.Rows.Count - 1) Then
                rowUp = DataGridView1.Rows.Count - 1
            Else
                rowUp = rowUp + 1
            End If
        Else
            If e.KeyCode = Keys.Up Then
                If (rowUp <= 0) Then
                    rowUp = 0
                Else
                    rowUp = rowUp - 1
                End If
            End If
        End If

        If Len(DataGridView1.Rows(rowUp).Cells(0).Value.ToString()) > 0 Then
            tbSequence.Text = DataGridView1.Rows(rowUp).Cells(0).Value.ToString()
        Else
            tbSequence.Text = ""
        End If
        If Len(DataGridView1.Rows(rowUp).Cells(1).Value.ToString()) > 0 Then
            tbDAFrom_PWID.Text = DataGridView1.Rows(rowUp).Cells(1).Value.ToString()
        Else
            tbDAFrom_PWID.Text = ""
        End If
        If Len(DataGridView1.Rows(rowUp).Cells(2).Value.ToString()) > 0 Then
            tbDAFrom_Name.Text = DataGridView1.Rows(rowUp).Cells(2).Value.ToString()
        Else
            tbDAFrom_Name.Text = ""
        End If
        If Len(DataGridView1.Rows(rowUp).Cells(3).Value.ToString()) > 0 Then
            tbDAFrom_Position.Text = DataGridView1.Rows(rowUp).Cells(3).Value.ToString()
        Else
            tbDAFrom_Position.Text = ""
        End If
        If Len(DataGridView1.Rows(rowUp).Cells(4).Value.ToString()) > 0 Then
            tbDATo_PWID.Text = DataGridView1.Rows(rowUp).Cells(4).Value.ToString()
        Else
            tbDATo_PWID.Text = ""
        End If
        If Len(DataGridView1.Rows(rowUp).Cells(5).Value.ToString()) > 0 Then
            tbDATo_Name.Text = DataGridView1.Rows(rowUp).Cells(5).Value.ToString()
        Else
            tbDATo_Name.Text = ""
        End If
        If Len(DataGridView1.Rows(rowUp).Cells(6).Value.ToString()) > 0 Then
            tbDATo_Position.Text = DataGridView1.Rows(rowUp).Cells(6).Value.ToString()
        Else
            tbDATo_Position.Text = ""
        End If
        If Len(DataGridView1.Rows(rowUp).Cells(7).Value.ToString()) > 0 Then
            ComboBox_Segment.Text = DataGridView1.Rows(rowUp).Cells(7).Value.ToString()
        Else
            ComboBox_Segment.Text = ""
        End If
        If Len(DataGridView1.Rows(rowUp).Cells(8).Value.ToString()) > 0 Then
            tbDepartment.Text = DataGridView1.Rows(rowUp).Cells(8).Value.ToString()
        Else
            tbDepartment.Text = ""
        End If
        If Len(DataGridView1.Rows(rowUp).Cells(9).Value.ToString()) > 0 Then
            tbEffectiveDate.Value = DateTime.ParseExact(DataGridView1.Rows(rowUp).Cells(9).Value.ToString(), "dd/MM/yyyy", Nothing)
        Else
            tbEffectiveDate.Value = Now.ToString("dd/MM/yyyy")
        End If
        If Len(DataGridView1.Rows(rowUp).Cells(10).Value.ToString()) > 0 Then
            tbExpiryDate.Value = DateTime.ParseExact(DataGridView1.Rows(rowUp).Cells(10).Value.ToString(), "dd/MM/yyyy", Nothing)
        Else
            tbExpiryDate.Value = Now.ToString("dd/MM/yyyy")
        End If
        If Len(DataGridView1.Rows(rowUp).Cells(11).Value.ToString()) > 0 Then
            tbDelegation.Text = DataGridView1.Rows(rowUp).Cells(11).Value.ToString()
        Else
            tbDelegation.Text = ""
        End If
        If Len(DataGridView1.Rows(rowUp).Cells(12).Value.ToString()) > 0 Then
            tbNote.Text = DataGridView1.Rows(rowUp).Cells(12).Value.ToString()
        Else
            tbNote.Text = ""
        End If
    End Sub
    Private Sub btClear_Click(sender As Object, e As EventArgs) Handles btClear.Click
        tbDAFrom_Name.Text = ""
        tbDAFrom_Position.Text = ""
        tbDAFrom_PWID.Text = ""
        tbDATo_Name.Text = ""
        tbDATo_Position.Text = ""
        tbDATo_PWID.Text = ""
        tbDelegation.Text = ""
        tbDepartment.Text = ""
        tbEffectiveDate.Value = DateTime.ParseExact(Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", Nothing)
        tbExpiryDate.Value = DateTime.ParseExact("31/12/2099", "dd/MM/yyyy", Nothing)
        tbNote.Text = ""
        tbSequence.Text = ""
    End Sub

    Private Sub btAdd_Click(sender As Object, e As EventArgs) Handles btAdd.Click

        If ComboBox_Segment.Text.Length = 0 Then
            MsgBox("Please select Segment for this DA", vbCritical + vbOKOnly)
            Exit Sub
        End If

        Dim FromDate As DateTime = tbEffectiveDate.Value
        Dim toDate As DateTime = tbExpiryDate.Value

        DataGridView1.Rows.Add(New String() {"", tbDAFrom_PWID.Text, tbDAFrom_Name.Text, tbDAFrom_Position.Text, tbDATo_PWID.Text, tbDATo_Name.Text, tbDATo_Position.Text, ComboBox_Segment.Text, tbDepartment.Text, Format(FromDate, "dd/MM/yyyy"), Format(toDate, "dd/MM/yyyy"), tbDelegation.Text, tbNote.Text})

        For i = 0 To DataGridView1.Rows.Count - 1
            DataGridView1.Rows(i).Cells(0).Value = i + 1
        Next
    End Sub

    Private Sub btEdit_Click(sender As Object, e As EventArgs) Handles btEdit.Click
        If tbSequence.Text.Length = 0 Then
            MsgBox("Textbox Seq No is null. Please select record for edit", vbCritical)
            Exit Sub
        End If

        For i = 0 To DataGridView1.Rows.Count - 1
            If DataGridView1.Rows(i).Cells(0).Value = tbSequence.Text Then
                DataGridView1.Rows(i).Cells(1).Value = tbDAFrom_PWID.Text
                DataGridView1.Rows(i).Cells(2).Value = tbDAFrom_Name.Text
                DataGridView1.Rows(i).Cells(3).Value = tbDAFrom_Position.Text
                DataGridView1.Rows(i).Cells(4).Value = tbDATo_PWID.Text
                DataGridView1.Rows(i).Cells(5).Value = tbDATo_Name.Text
                DataGridView1.Rows(i).Cells(6).Value = tbDATo_Position.Text
                DataGridView1.Rows(i).Cells(7).Value = ComboBox_Segment.Text
                DataGridView1.Rows(i).Cells(8).Value = tbDepartment.Text
                DataGridView1.Rows(i).Cells(9).Value = tbEffectiveDate.Value
                DataGridView1.Rows(i).Cells(10).Value = tbExpiryDate.Value
                DataGridView1.Rows(i).Cells(11).Value = tbDelegation.Text
                DataGridView1.Rows(i).Cells(12).Value = tbNote.Text
            End If
        Next
    End Sub

    Private Sub btDelete_Click(sender As Object, e As EventArgs) Handles btDelete.Click
        If tbSequence.Text.Length = 0 Then
            MsgBox("Textbox Seq No is null. Please select record for delete", vbCritical)
            Exit Sub
        End If

        For Each row As DataGridViewRow In DataGridView1.Rows
            If DataGridView1.Rows(row.Index).Cells(0).Value = tbSequence.Text Then
                DataGridView1.Rows.Remove(row)
            End If
        Next

        For i = 0 To DataGridView1.Rows.Count - 1
            DataGridView1.Rows(i).Cells(0).Value = i + 1
        Next
    End Sub

    Private Sub btCreate_Click(sender As Object, e As EventArgs) Handles btCreate.Click
        If DataGridView1.Rows.Count = 0 Then
            MsgBox("Please add information of DA")
            Exit Sub
        End If

        If tbLinkPDF.Text.Length = 0 Then
            MsgBox("Please select file DA details in pdf")
            Exit Sub
        End If

        If folder_path_save_file.Substring(folder_path_save_file.Length - 1, 1) <> "\" Then
            folder_path_save_file = folder_path_save_file & "\"
        End If

        Dim new_file_name As String = Now.ToString("yyyyMMddhhmmsstt")

        Dim new_file_name2 As String = DataGridView1.Rows(0).Cells(7).Value & "_" & DataGridView1.Rows(0).Cells(2).Value & "_" & new_file_name & ".pdf"

        If My.Computer.FileSystem.FileExists(tbLinkPDF.Text) Then
            My.Computer.FileSystem.CopyFile(tbLinkPDF.Text, folder_path_save_file & new_file_name2)
        End If

        For i = 0 To DataGridView1.Rows.Count - 1
            Dim Str_SQL_Add_Record As String = "INSERT INTO Main_DA([Department], [DA_To], [PWID_Da_To], [Position_To], [Delegation], [Effective_Date], [Expiry_Date], [Status], [Note], [DA_From], [PWID_DA_From], [Position_From], [File_Name], [User_Created], [Segment], [CaseID]) VALUES ('" &
                                                                    DataGridView1.Rows(i).Cells(8).Value & "', '" & DataGridView1.Rows(i).Cells(5).Value & "', '" & DataGridView1.Rows(i).Cells(4).Value & "', '" & DataGridView1.Rows(i).Cells(6).Value & "', '" & DataGridView1.Rows(i).Cells(11).Value & "', '" & DataGridView1.Rows(i).Cells(9).Value & "', '" & DataGridView1.Rows(i).Cells(10).Value & "', '" & "In Progress" & "', '" & DataGridView1.Rows(i).Cells(12).Value & "', '" & DataGridView1.Rows(i).Cells(2).Value & "', '" & DataGridView1.Rows(i).Cells(1).Value & "', '" & DataGridView1.Rows(i).Cells(3).Value & "', '" & new_file_name2 & "', '" & UCase(Main.BarStaticItem_User.Caption) & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss tt") & "', '" & DataGridView1.Rows(i).Cells(7).Value & "', '" & Now.ToString("yyyyMMddhhmmsstt") & "_" & Format(DataGridView1.Rows(i).Cells(0).Value, "000") & "');"
            SQL_QUERY_WRITE_LOG(link_database_DA, Str_SQL_Add_Record)
        Next
        Main.Auto_Set_Expired()
        MsgBox("COMPLETED")
    End Sub


End Class