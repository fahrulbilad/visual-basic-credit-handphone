Imports System.Data.Odbc
Public Class data_persyaratan
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
            da = New OdbcDataAdapter("select *from tb_persyaratan order by kode_persyaratan asc", con)
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
        cmd = New OdbcCommand("select kode_persyaratan from tb_persyaratan order by kode_persyaratan desc", con)
        dr = cmd.ExecuteReader
        dr.Read()
        If Not dr.HasRows Then
            TextBox1.Text = "SY" + "001"
        Else
            TextBox1.Text = "SY" + Format(Microsoft.VisualBasic.Right(dr.Item("kode_persyaratan"), 2) + 1, "000")
        End If
    End Sub

    Sub combo()
        koneksi()
        cmd = New OdbcCommand("select kode_pelanggan from tb_pelanggan", con)
        dr = cmd.ExecuteReader
        Do While dr.Read
            ComboBox1.Items.Add(dr.Item("kode_pelanggan"))
        Loop
    End Sub

    Sub namapl()
        Dim drr As OdbcDataReader
        cmd = New OdbcCommand("select nama_pelanggan from tb_pelanggan where kode_pelanggan='" & ComboBox1.Text & "'", con)
        drr = cmd.ExecuteReader
        drr.Read()
        If drr.HasRows Then
            TextBox2.Text = drr.Item("nama_pelanggan")
        End If
    End Sub

    Sub keterangan()
        If CheckBox1.Checked = True And CheckBox2.Checked = True Then
            TextBox3.Text = "Syarat Terpenuhi"
            Button1.Enabled = True
        Else
            TextBox3.Text = "Syarat Belum Terpenuhi"
            Button1.Enabled = False
        End If
    End Sub

    Sub simpan()
        koneksi()
        Dim sql As String = "insert into tb_persyaratan values('" & TextBox1.Text & "','" & ComboBox1.Text & "','" & Label7.Text & "','" & Label8.Text & "','" & Label9.Text & "','" & TextBox3.Text & "')"
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
            edit = "update tb_persyaratan set kode_pelanggan ='" & ComboBox1.Text & "', fc_ktp ='" & Label7.Text & "', fc_kk ='" & Label8.Text & "', slip_gaji ='" & Label9.Text & "', keterangan ='" & TextBox3.Text & "' where kode_persyaratan ='" & TextBox1.Text & "'"
            cmd = New OdbcCommand(edit, con)
            cmd.ExecuteNonQuery()
            MessageBox.Show("Update Data Berhasil")
        Catch ex As Exception
            MessageBox.Show("Update Data Gagal")
        End Try
    End Sub

    Sub baru()
        ComboBox1.Text = "-Pilih-"
        TextBox2.Text = ""
        CheckBox1.Checked = False
        CheckBox2.Checked = False
        CheckBox3.Checked = False
        TextBox3.Text = ""
        ComboBox1.Focus()
    End Sub

    Sub hapus()
        Dim a As String = DataGridView1.Item(0, DataGridView1.CurrentRow.Index).Value
        If a = "" Then
            MsgBox("Data Persyataran yang dihapus belum DIPILIH")
        Else
            If (MessageBox.Show("Anda yakin menghapus data dengan Kode Persyaratan=" & a & "...?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.OK) Then
                koneksi()
                cmd = New OdbcCommand("delete from tb_persyaratan where kode_persyaratan='" & a & "'", con)
                cmd.ExecuteNonQuery()
                MsgBox("Menghapus data BERHASIL", vbInformation, "INFORMASI")
                con.Close()
                tampil()
            End If
        End If
    End Sub

    Sub CariKDpersyaratan()
        cmd = New OdbcCommand("select * from tb_persyaratan where kode_persyaratan='" & TextBox1.Text & "'", con)
        dr = cmd.ExecuteReader
        dr.Read()
    End Sub

    Sub Ketemu()
        On Error Resume Next
        ComboBox1.Text = dr.Item(1)
        Label7.Text = dr.Item(2)
        Label8.Text = dr.Item(3)
        Label9.Text = dr.Item(4)
        TextBox3.Text = dr.Item(5)
        ComboBox1.Focus()

        If Label7.Text = "Y" Then
            CheckBox1.Checked = True
        End If
        If Label8.Text = "Y" Then
            CheckBox2.Checked = True
        End If
        If Label9.Text = "Y" Then
            CheckBox3.Checked = True
        Else
            CheckBox3.Checked = False
        End If
    End Sub

    Private Sub data_persyaratan_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tampil()
        IDOtomatis()
        TextBox1.ReadOnly = True
        TextBox2.ReadOnly = True
        TextBox3.ReadOnly = True
        combo()
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
        Call CariKDpersyaratan()
        If dr.HasRows Then
            Call Ketemu()
        End If
        namapl()
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            Label7.Text = "Y"
        Else
            Label7.Text = "N"
        End If
        keterangan()
    End Sub

    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked = True Then
            Label8.Text = "Y"
        Else
            Label8.Text = "N"
        End If
        keterangan()
    End Sub

    Private Sub CheckBox3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox3.Checked = True Then
            Label9.Text = "Y"
        Else
            Label9.Text = "N"
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        namapl()
    End Sub
End Class