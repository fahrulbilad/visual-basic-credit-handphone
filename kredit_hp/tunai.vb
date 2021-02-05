Imports System.Data.Odbc
Public Class tunai
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

    Sub tampil()
        DataGridView1.Rows.Clear()
        Try
            koneksi()
            da = New OdbcDataAdapter("select *from tb_tunai order by kode_tunai asc", con)
            dt = New DataTable
            da.Fill(dt)
            For Each row In dt.Rows
                DataGridView1.Rows.Add(row(0), row(1), row(2), row(3), row(4), row(5))
            Next
            dt.Rows.Clear()
        Catch ex As Exception
            MsgBox("Menampilkan data GAGAL")
        End Try
    End Sub

    Sub IDOtomatis()
        koneksi()
        cmd = New OdbcCommand("select kode_tunai from tb_tunai order by kode_tunai desc", con)
        dr = cmd.ExecuteReader
        dr.Read()
        If Not dr.HasRows Then
            TextBox1.Text = "TN" + "001"
        Else
            TextBox1.Text = "TN" + Format(Microsoft.VisualBasic.Right(dr.Item("kode_tunai"), 2) + 1, "000")
        End If
    End Sub

    Sub comboPL()
        koneksi()
        cmd = New OdbcCommand("select kode_pelanggan from tb_pelanggan", con)
        dr = cmd.ExecuteReader
        Do While dr.Read
            ComboBox1.Items.Add(dr.Item("kode_pelanggan"))
        Loop
    End Sub

    Sub namapl()
        cmd = New OdbcCommand("select * from tb_pelanggan where kode_pelanggan='" & ComboBox1.Text & "'", con)
        dr = cmd.ExecuteReader
        dr.Read()
        If dr.HasRows Then
            TextBox2.Text = dr.Item("nama_pelanggan")
        End If
    End Sub

    Sub comboHP()
        koneksi()
        cmd = New OdbcCommand("select kode_handphone from tb_handphone", con)
        dr = cmd.ExecuteReader
        Do While dr.Read
            ComboBox2.Items.Add(dr.Item("kode_handphone"))
        Loop
    End Sub

    Sub namahp()
        cmd = New OdbcCommand("select * from tb_handphone where kode_handphone='" & ComboBox2.Text & "'", con)
        dr = cmd.ExecuteReader
        dr.Read()
        If dr.HasRows Then
            TextBox3.Text = dr.Item("merk")
            TextBox4.Text = dr.Item("harga")
        End If
    End Sub

    Sub simpan()
        koneksi()
        Dim sql As String = "insert into tb_tunai values('" & TextBox1.Text & "','" & DateTimePicker1.Text & "','" & ComboBox1.Text & "','" & ComboBox2.Text & "','" & TextBox4.Text & "','" & TextBox5.Text & "')"
        cmd = New OdbcCommand(sql, con)
        cmd.ExecuteNonQuery()
        Try
            MsgBox("Menyimpan data BERHASIL", vbInformation, "INFORMASI")
        Catch ex As Exception
            MsgBox("Menyimpan data GAGAL", vbInformation, "PERINGATAN")
        End Try
    End Sub

    Sub ubah()
        Try
            koneksi()
            Dim edit As String
            edit = "update tb_tunai set tanggal_pembelian ='" & DateTimePicker1.Text & "', kode_pelanggan ='" & ComboBox1.Text & "', kode_handphone ='" & ComboBox2.Text & "', harga ='" & TextBox4.Text & "', jumlah_bayar ='" & TextBox5.Text & "' where kode_tunai ='" & TextBox1.Text & "'"
            cmd = New OdbcCommand(edit, con)
            cmd.ExecuteNonQuery()
            MessageBox.Show("Update Data Berhasil")
        Catch ex As Exception
            MessageBox.Show("Update Data Gagal")
        End Try
    End Sub

    Sub baru()
        DateTimePicker1.Text = Format(Today(), "dd-MMMM-yyyy")
        ComboBox1.Text = "-Pilih-"
        TextBox2.Text = ""
        ComboBox2.Text = "-Pilih-"
        TextBox3.Text = ""
        TextBox4.Text = "0"
        TextBox5.Text = "0"
        TextBox6.Text = "0"
        DateTimePicker1.Focus()
    End Sub

    Sub hapus()
        Dim a As String = DataGridView1.Item(0, DataGridView1.CurrentRow.Index).Value
        If a = "" Then
            MsgBox("Data Transaksi yang dihapus belum DIPILIH")
        Else
            If (MessageBox.Show("Anda yakin menghapus data Transaksi=" & a & "...?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.OK) Then
                koneksi()
                cmd = New OdbcCommand("delete from tb_tunai where kode_tunai='" & a & "'", con)
                cmd.ExecuteNonQuery()
                MsgBox("Menghapus data BERHASIL", vbInformation, "INFORMASI")
                con.Close()
                tampil()
            End If
        End If

    End Sub

    Private Sub tunai_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tampil()
        IDOtomatis()
        TextBox1.ReadOnly = True
        comboPL()
        TextBox2.ReadOnly = True
        comboHP()
        TextBox3.ReadOnly = True
        TextBox4.ReadOnly = True
        DateTimePicker1.Text = Format(Today(), "dd-MMMM-yyyy")
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        namapl()
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        namahp()
    End Sub

    Private Sub TextBox5_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox5.TextChanged
        TextBox6.Text = CStr(TextBox5.Text) - CInt(TextBox4.Text)
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
        baru()
        tampil()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        hapus()
        tampil()
        baru()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Close()
    End Sub

    Private Sub DataGridView1_CellMouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        On Error Resume Next
        TextBox1.Text = DataGridView1.Rows(e.RowIndex).Cells(0).Value
        DateTimePicker1.Text = DataGridView1.Rows(e.RowIndex).Cells(1).Value
        ComboBox1.Text = DataGridView1.Rows(e.RowIndex).Cells(2).Value
        ComboBox2.Text = DataGridView1.Rows(e.RowIndex).Cells(3).Value
        TextBox4.Text = DataGridView1.Rows(e.RowIndex).Cells(4).Value
        TextBox5.Text = DataGridView1.Rows(e.RowIndex).Cells(5).Value
        namapl()
        namahp()
    End Sub
End Class