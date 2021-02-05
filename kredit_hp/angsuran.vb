Imports System.Data.Odbc
Public Class angsuran
    Dim con As OdbcConnection
    Dim dr As OdbcDataReader
    Dim da As OdbcDataAdapter
    Dim ds As DataSet
    Dim dt As DataTable
    Dim cmd As OdbcCommand

    Sub koneksi()
        con = New OdbcConnection
        con.ConnectionString = "dsn=dsn_kredit_hp"
        con.Open()
    End Sub

    Sub simpan()
        koneksi()
        Dim sql As String = "insert into tb_angsuran values('" & TextBox1.Text & "','" & ComboBox1.Text & "','" & NumericUpDown1.Value & "','" & DateTimePicker1.Text & "','" & TextBox2.Text & "','" & TextBox3.Text & "')"
        cmd = New OdbcCommand(sql, con)
        cmd.ExecuteNonQuery()
        Try
            MsgBox("Menyimpan data BERHASIL", vbInformation, "INFORMASI")
        Catch ex As Exception
            MsgBox("Menyimpan data GAGAL", vbInformation, "PERINGATAN")
        End Try
    End Sub

    Sub tampil()
        DataGridView2.Rows.Clear()
        Try
            koneksi()
            da = New OdbcDataAdapter("select *from tb_angsuran where kode_kredit='" & ComboBox1.Text & "' order by kode_angsuran asc", con)
            dt = New DataTable
            da.Fill(dt)
            For Each row In dt.Rows
                DataGridView2.Rows.Add(row(0), row(1), row(2), row(3), row(4), row(5))
            Next
            dt.Rows.Clear()
        Catch ex As Exception
            MsgBox("Menampilkan data GAGAL")
        End Try
    End Sub

    Sub TampilTabelKredit()
        koneksi()
        da = New OdbcDataAdapter("select * from tb_kredit", con)
        ds = New DataSet
        da.Fill(ds)
        DataGridView1.DataSource = ds.Tables(0)
        DataGridView1.ReadOnly = True
    End Sub

    Sub ubah()
        Try
            koneksi()
            Dim edit As String
            edit = "update tb_angsuran set kode_kredit ='" & ComboBox1.Text & "', pembayaran_ke ='" & NumericUpDown1.Value & "', tgl_bayar ='" & DateTimePicker1.Text & "', bayar ='" & TextBox2.Text & "', keterangan ='" & TextBox3.Text & "' where kode_angsuran ='" & TextBox1.Text & "'"
            cmd = New OdbcCommand(edit, con)
            cmd.ExecuteNonQuery()
            MessageBox.Show("Update Data Berhasil")
        Catch ex As Exception
            MessageBox.Show("Update Data Gagal")
        End Try
    End Sub

    Sub hapus()
        Dim a As String = DataGridView2.Item(0, DataGridView2.CurrentRow.Index).Value
        If a = "" Then
            MsgBox("Data angsuran yang dihapus belum DIPILIH")
        Else
            If (MessageBox.Show("Anda yakin menghapus data dengan Kode Angsuran=" & a & "...?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.OK) Then
                koneksi()
                cmd = New OdbcCommand("delete from tb_angsuran where kode_angsuran='" & a & "'", con)
                cmd.ExecuteNonQuery()
                MsgBox("Menghapus data BERHASIL", vbInformation, "INFORMASI")
                con.Close()
                tampil()
            End If
        End If
    End Sub

    Sub baru()
        TextBox1.Text = ""
        ComboBox1.Text = "-Pilih-"
        NumericUpDown1.Value = 0
        DateTimePicker1.Text = Format(Today(), "dd-MMMM-yyyy")
        TextBox2.Text = "0"
        TextBox3.Text = ""
        Label8.Text = "0"
        TextBox3.Enabled = True
        ComboBox1.Focus()
    End Sub

    Sub IDOtomatis()
        koneksi()
        cmd = New OdbcCommand("select kode_angsuran from tb_angsuran order by kode_angsuran desc", con)
        dr = cmd.ExecuteReader
        dr.Read()
        If Not dr.HasRows Then
            TextBox1.Text = "AN" + "001"
        Else
            TextBox1.Text = "AN" + Format(Microsoft.VisualBasic.Right(dr.Item("kode_angsuran"), 2) + 1, "000")
        End If
    End Sub

    Sub combo()
        koneksi()
        cmd = New OdbcCommand("select kode_kredit from tb_kredit", con)
        dr = cmd.ExecuteReader
        Do While dr.Read
            ComboBox1.Items.Add(dr.Item("kode_kredit"))
        Loop
    End Sub

    Sub angsuranoto()
        koneksi()
        Dim data As Integer
        cmd = New OdbcCommand("select count(pembayaran_ke) from tb_angsuran where kode_kredit='" & ComboBox1.Text & "'", con)
        data = cmd.ExecuteScalar
        NumericUpDown1.Value = data + 1
    End Sub

    Private Sub angsuran_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        TampilTabelKredit()
        IDOtomatis()
        combo()
        TextBox1.Enabled = False
        TextBox4.Enabled = False
        DateTimePicker1.Text = Format(Today(), "dd-MMMM-yyyy")
    End Sub

    Private Sub DataGridView1_CellMouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        ComboBox1.Text = DataGridView1.Rows(e.RowIndex).Cells(0).Value
        Label8.Text = DataGridView1.Rows(e.RowIndex).Cells(8).Value
        TextBox4.Text = DataGridView1.Rows(e.RowIndex).Cells(4).Value
        angsuranoto()
        tampil()
    End Sub

    Private Sub DataGridView2_CellMouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DataGridView2.CellMouseClick
        On Error Resume Next
        TextBox1.Text = DataGridView2.Rows(e.RowIndex).Cells(0).Value
        ComboBox1.Text = DataGridView2.Rows(e.RowIndex).Cells(1).Value
        NumericUpDown1.Value = DataGridView2.Rows(e.RowIndex).Cells(2).Value
        DateTimePicker1.Text = DataGridView2.Rows(e.RowIndex).Cells(3).Value
        TextBox2.Text = DataGridView2.Rows(e.RowIndex).Cells(4).Value
        TextBox3.Text = DataGridView2.Rows(e.RowIndex).Cells(5).Value
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        simpan()
        tampil()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        ubah()
        tampil()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        hapus()
        tampil()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        baru()
        IDOtomatis()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Close()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        angsuranoto()
        tampil()
    End Sub

    Private Sub TextBox3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.Click
        koneksi()
        Dim jumlah1 As Integer
        Dim jumlah2 As Integer
        Dim bayaran As Integer
        Dim harga As Integer
        Dim total As Integer
        bayaran = CInt(TextBox2.Text)
        cmd = New OdbcCommand("select COALESCE(sum(bayar),0) from tb_angsuran where kode_kredit='" & ComboBox1.Text & "'", con)
        jumlah1 = cmd.ExecuteScalar
        jumlah2 = jumlah1 + bayaran
        harga = CInt(TextBox4.Text)
        total = harga - jumlah2
        If jumlah2 >= harga Then
            TextBox3.Text = "Lunas"
            TextBox3.Enabled = False
        Else
            TextBox3.Text = "Kurang " + CStr(total)
            TextBox3.Enabled = False
        End If
    End Sub
End Class