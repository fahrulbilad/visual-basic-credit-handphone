Imports System.Data.Odbc
Public Class data_pelanggan
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
            da = New OdbcDataAdapter("select *from tb_pelanggan order by kode_pelanggan asc", con)
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
        cmd = New OdbcCommand("select kode_pelanggan from tb_pelanggan order by kode_pelanggan desc", con)
        dr = cmd.ExecuteReader
        dr.Read()
        If Not dr.HasRows Then
            TextBox1.Text = "PL" + "001"
        Else
            TextBox1.Text = "PL" + Format(Microsoft.VisualBasic.Right(dr.Item("kode_pelanggan"), 2) + 1, "000")
        End If
    End Sub

    Sub simpan()
        koneksi()
        Dim sql As String = "insert into tb_pelanggan values('" & TextBox1.Text & "','" & TextBox2.Text & "','" & ComboBox1.Text & "','" & TextBox3.Text & "','" & TextBox4.Text & "','" & TextBox5.Text & "')"
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
            edit = "update tb_pelanggan set nama_pelanggan ='" & TextBox2.Text & "', jekel ='" & ComboBox1.Text & "', alamat ='" & TextBox3.Text & "', no_telepon ='" & TextBox4.Text & "', pekerjaan ='" & TextBox5.Text & "' where kode_pelanggan ='" & TextBox1.Text & "'"
            cmd = New OdbcCommand(edit, con)
            cmd.ExecuteNonQuery()
            MessageBox.Show("Update Data Berhasil")
        Catch ex As Exception
            MessageBox.Show("Update Data Gagal")
        End Try
    End Sub

    Sub baru()
        TextBox2.Text = ""
        ComboBox1.Text = "--Pilih Jenis Kelamin--"
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox2.Focus()
    End Sub

    Sub hapus()
        Dim a As String = DataGridView1.Item(0, DataGridView1.CurrentRow.Index).Value
        If a = "" Then
            MsgBox("Data Pelanggan yang dihapus belum DIPILIH")
        Else
            If (MessageBox.Show("Anda yakin menghapus data dengan Kode Pelanggan=" & a & "...?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.OK) Then
                koneksi()
                cmd = New OdbcCommand("delete from tb_pelanggan where kode_pelanggan='" & a & "'", con)
                cmd.ExecuteNonQuery()
                MsgBox("Menghapus data BERHASIL", vbInformation, "INFORMASI")
                con.Close()
                tampil()
            End If
        End If
    End Sub

    Sub CariKDpelanggan()
        cmd = New OdbcCommand("select * from tb_pelanggan where kode_pelanggan='" & TextBox1.Text & "'", con)
        dr = cmd.ExecuteReader
        dr.Read()
    End Sub

    Sub Ketemu()
        On Error Resume Next
        TextBox2.Text = dr.Item(1)
        ComboBox1.Text = dr.Item(2)
        TextBox3.Text = dr.Item(3)
        TextBox4.Text = dr.Item(4)
        TextBox5.Text = dr.Item(5)
        TextBox2.Focus()
    End Sub

    Private Sub data_pelanggan_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tampil()
        IDOtomatis()
        TextBox1.ReadOnly = True
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
        IDOtomatis()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        hapus()
        tampil()
        baru()
        IDOtomatis()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Close()
    End Sub

    Private Sub DataGridView1_CellMouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        On Error Resume Next
        TextBox1.Text = DataGridView1.Rows(e.RowIndex).Cells(0).Value
        Call CariKDpelanggan()
        If dr.HasRows Then
            Call Ketemu()
        End If
    End Sub
End Class
