Public Class login

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If TextBox1.Text = "" And TextBox2.Text = "" Then
            MsgBox("Username dan Password tidak boleh kosong")
        ElseIf TextBox1.Text = "" Then
            MsgBox("Username tidak boleh kosong")
        ElseIf TextBox2.Text = "" Then
            MsgBox("Password tidak boleh kosong")
        Else
            If TextBox1.Text = "admin" And TextBox2.Text = "1234" Then
                Me.Visible = False
                menu_utama.Show()
            Else
                MsgBox("Username atau Password salah")
            End If
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox1.Focus()
    End Sub
End Class