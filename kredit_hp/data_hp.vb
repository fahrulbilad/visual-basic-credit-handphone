Imports System.Data.Odbc
Public Class data_hp
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
            da = New OdbcDataAdapter("select * from tb_handphone order by kode_handphone asc", con)
            dt = New DataTable
            da.Fill(dt)
            For Each row In dt.Rows
                DataGridView1.Rows.Add(row(0), row(1), row(2), row(3), row(4), row(5), row(6), row(7), row(8))
            Next
            dt.Rows.Clear()
        Catch ex As Exception
            MsgBox("Menampilkan data GAGAL")
        End Try
    End Sub

    Sub IDOtomatis()
        koneksi()
        cmd = New OdbcCommand("select kode_handphone from tb_handphone order by kode_handphone desc", con)
        dr = cmd.ExecuteReader
        dr.Read()
        If Not dr.HasRows Then
            TextBox1.Text = "HP" + "001"
        Else
            TextBox1.Text = "HP" + Format(Microsoft.VisualBasic.Right(dr.Item("kode_handphone"), 2) + 1, "000")
        End If
    End Sub

    Sub simpan()
        koneksi()
        Dim sql As String = "insert into tb_handphone values('" & TextBox1.Text & "','" & TextBox2.Text & "','" & TextBox3.Text & "','" & TextBox4.Text & "','" & TextBox5.Text & "','" & TextBox6.Text & "','" & TextBox7.Text & "','" & TextBox8.Text & "','" & TextBox9.Text & "')"
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
            edit = "update tb_handphone set merk ='" & TextBox2.Text & "', warna ='" & TextBox3.Text & "', tahun_launching ='" & TextBox4.Text & "', type ='" & TextBox5.Text & "', sistem_operasi ='" & TextBox6.Text & "', ram ='" & TextBox7.Text & "', memori_internal ='" & TextBox8.Text & "', harga ='" & TextBox9.Text & "' where kode_handphone ='" & TextBox1.Text & "'"
            cmd = New OdbcCommand(edit, con)
            cmd.ExecuteNonQuery()
            MessageBox.Show("Update Data Berhasil")
        Catch ex As Exception
            MessageBox.Show("Update Data Gagal")
        End Try
    End Sub

    Sub baru()
        TextBox2.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox6.Text = ""
        TextBox7.Text = ""
        TextBox8.Text = ""
        TextBox9.Text = ""
        TextBox2.Focus()
    End Sub

    Sub hapus()
        Dim a As String = DataGridView1.Item(0, DataGridView1.CurrentRow.Index).Value
        If a = "" Then
            MsgBox("Data Handphone yang dihapus belum DIPILIH")
        Else
            If (MessageBox.Show("Anda yakin menghapus data dengan Kode Handphone=" & a & "...?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.OK) Then
                koneksi()
                cmd = New OdbcCommand("delete from tb_handphone where kode_handphone='" & a & "'", con)
                cmd.ExecuteNonQuery()
                MsgBox("Menghapus data BERHASIL", vbInformation, "INFORMASI")
                con.Close()
                tampil()
            End If
        End If
    End Sub

    Sub CariKDhp()
        cmd = New OdbcCommand("select * from tb_handphone where kode_handphone='" & TextBox1.Text & "'", con)
        dr = cmd.ExecuteReader
        dr.Read()
    End Sub

    Sub Ketemu()
        On Error Resume Next
        TextBox2.Text = dr.Item(1)
        TextBox3.Text = dr.Item(2)
        TextBox4.Text = dr.Item(3)
        TextBox5.Text = dr.Item(4)
        TextBox6.Text = dr.Item(5)
        TextBox7.Text = dr.Item(6)
        TextBox8.Text = dr.Item(7)
        TextBox9.Text = dr.Item(8)
        TextBox2.Focus()
    End Sub

    Private Sub data_hp_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
        Call CariKDhp()
        If dr.HasRows Then
            Call Ketemu()
        End If
    End Sub
End Class