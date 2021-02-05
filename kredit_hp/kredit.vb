Imports System.Data.Odbc
Public Class kredit
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
            da = New OdbcDataAdapter("select * from tb_kredit order by kode_kredit asc", con)
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

    Sub simpan()
        koneksi()
        Dim sql As String = "insert into tb_kredit values('" & TextBox1.Text & "','" & DateTimePicker1.Text & "','" & ComboBox1.Text & "','" & ComboBox2.Text & "','" & TextBox8.Text & "','" & TextBox9.Text & "','" & TextBox7.Text & "','" & NumericUpDown1.Value & "','" & TextBox10.Text & "')"
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
            edit = "update tb_kredit set mulai_kredit ='" & DateTimePicker1.Text & "', kode_persyaratan ='" & ComboBox1.Text & "', kode_handphone ='" & ComboBox2.Text & "', harga_kredit ='" & TextBox8.Text & "', uang_muka ='" & TextBox9.Text & "', lama_cicilan ='" & NumericUpDown1.Value & "', angsuran ='" & TextBox10.Text & "' where kode_kredit ='" & TextBox1.Text & "'"
            cmd = New OdbcCommand(edit, con)
            cmd.ExecuteNonQuery()
            MessageBox.Show("Update Data Berhasil")
        Catch ex As Exception
            MessageBox.Show("Update Data Gagal")
        End Try
    End Sub

    Sub hapus()
        Dim a As String = DataGridView1.Item(0, DataGridView1.CurrentRow.Index).Value
        If a = "" Then
            MsgBox("Data Persyataran yang dihapus belum DIPILIH")
        Else
            If (MessageBox.Show("Anda yakin menghapus data dengan kode_kredit=" & a & "...?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.OK) Then
                koneksi()
                cmd = New OdbcCommand("delete from tb_kredit where kode_kredit='" & a & "'", con)
                cmd.ExecuteNonQuery()
                MsgBox("Menghapus data BERHASIL", vbInformation, "INFORMASI")
                con.Close()
                tampil()
            End If
        End If
    End Sub

    Sub IDOtomatis()
        koneksi()
        cmd = New OdbcCommand("select kode_kredit from tb_kredit order by kode_kredit desc", con)
        dr = cmd.ExecuteReader
        dr.Read()
        If Not dr.HasRows Then
            TextBox1.Text = "KR" + "001"
        Else
            TextBox1.Text = "KR" + Format(Microsoft.VisualBasic.Right(dr.Item("kode_kredit"), 2) + 1, "000")
        End If
    End Sub

    Sub comboSY()
        koneksi()
        cmd = New OdbcCommand("select kode_persyaratan from tb_persyaratan", con)
        dr = cmd.ExecuteReader
        Do While dr.Read
            ComboBox1.Items.Add(dr.Item("kode_persyaratan"))
        Loop
    End Sub

    Sub syarat()
        cmd = New OdbcCommand("select * from tb_persyaratan join tb_pelanggan on tb_pelanggan.kode_pelanggan=tb_persyaratan.kode_pelanggan where kode_persyaratan='" & ComboBox1.Text & "'", con)
        dr = cmd.ExecuteReader
        dr.Read()
        If dr.HasRows Then
            TextBox3.Text = dr.Item("kode_pelanggan")
            TextBox4.Text = dr.Item("nama_pelanggan")
            TextBox2.Text = dr.Item("keterangan")
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
            TextBox5.Text = dr.Item("merk")
            TextBox6.Text = dr.Item("harga")
        End If
    End Sub

    Sub baru()
        ComboBox1.Text = "-Pilih-"
        ComboBox2.Text = "-Pilih-"
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox6.Text = "0"
        TextBox7.Text = "0"
        TextBox8.Text = "0"
        TextBox9.Text = "0"
        TextBox10.Text = "0"
        NumericUpDown1.Value = 0
        DateTimePicker1.Text = Format(Today(), "dd-MMMM-yyyy")
        TextBox1.Focus()
    End Sub

    Sub bunga()
        Dim harga As Integer
        Dim bunga As Single
        Dim total As Integer
        Dim dp As Integer
        harga = CInt(TextBox6.Text)
        bunga = CSng(harga * CInt(TextBox7.Text)) / 100
        total = harga + CInt(bunga)
        TextBox8.Text = CStr(total)
        dp = total / 10
        Label17.Text = CStr(dp)
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        syarat()
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        namahp()
    End Sub

    Private Sub kredit_Load_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tampil()
        IDOtomatis()
        TextBox1.ReadOnly = True
        DateTimePicker1.Text = Format(Today(), "dd-MMMM-yyyy")
        comboSY()
        TextBox2.ReadOnly = True
        TextBox3.ReadOnly = True
        TextBox4.ReadOnly = True
        comboHP()
        TextBox5.ReadOnly = True
        TextBox6.ReadOnly = True
        TextBox8.ReadOnly = True
        TextBox10.ReadOnly = True
    End Sub

    Private Sub TextBox7_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox7.TextChanged
        bunga()
    End Sub

    Private Sub TextBox10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox10.Click
        Dim kredit As Integer
        Dim dp As Integer
        Dim cicil As Integer
        Dim angsuran As Integer
        kredit = CInt(TextBox8.Text)
        dp = CInt(TextBox9.Text)
        cicil = CSng(NumericUpDown1.Value)
        angsuran = CSng(kredit - dp) / cicil
        TextBox10.Text = CStr(angsuran)
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Close()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        hapus()
        tampil()
        baru()
        IDOtomatis()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        ubah()
        tampil()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        simpan()
        tampil()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        baru()
        tampil()
        IDOtomatis()
    End Sub

    Private Sub DataGridView1_CellMouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        On Error Resume Next
        TextBox1.Text = DataGridView1.Rows(e.RowIndex).Cells(0).Value
        DateTimePicker1.Text = DataGridView1.Rows(e.RowIndex).Cells(1).Value
        ComboBox1.Text = DataGridView1.Rows(e.RowIndex).Cells(2).Value
        ComboBox2.Text = DataGridView1.Rows(e.RowIndex).Cells(3).Value
        TextBox8.Text = DataGridView1.Rows(e.RowIndex).Cells(4).Value
        TextBox9.Text = DataGridView1.Rows(e.RowIndex).Cells(5).Value
        TextBox7.Text = DataGridView1.Rows(e.RowIndex).Cells(6).Value
        NumericUpDown1.Value = DataGridView1.Rows(e.RowIndex).Cells(7).Value
        TextBox10.Text = DataGridView1.Rows(e.RowIndex).Cells(8).Value
        syarat()
        namahp()
        bunga()
    End Sub
End Class