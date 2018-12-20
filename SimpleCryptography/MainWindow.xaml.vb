Class MainWindow

    ''' <summary>
    ''' Ereignisbehandlung für die Verschlüsselung eines Textes nach Caeser
    ''' </summary>
    ''' <param name="sender">Objekt, dass das Ereignis ausgelöst hat</param>
    ''' <param name="e">Gesonderte Eigenschaften des Ereignisses</param>
    Private Sub ButtonVerschlüsseln_Click(sender As Object, e As RoutedEventArgs)
        Dim key As Integer
        fehlerText.Text = ""
        Try
            key = Convert.ToInt32(txt_key.Text)
        Catch ex As Exception
            MsgBox("Fehler bei der Schlüsseleingabe:\n" + ex.Message)
        End Try
        txt_ciphertext.Text = CryptCaesar(key, txt_plaintext.Text)
    End Sub

    ''' <summary>
    ''' Ereignisbehandlung für die Entschlüsselung eines ciphertext s nach Caeser
    ''' </summary>
    ''' <param name="sender">Objekt, dass das Ereignis ausgelöst hat</param>
    ''' <param name="e">Gesonderte Eigenschaften des Ereignisses</param>
    Private Sub ButtonEntschlüsseln_Click(sender As Object, e As RoutedEventArgs)
        Dim key As Integer
        fehlerText.Text = String.Empty
        Try
            key = Convert.ToInt32(txt_key.Text)
        Catch ex As Exception
            MsgBox("Fehler bei der Schlüsseleingabe:\n" + ex.Message)
        End Try
        txt_ciphertext.Text = CryptCaesar(-key, txt_plaintext.Text)
    End Sub


    ''' <summary>
    ''' Ereignisbehandlung für die Verschlüsselung eines Textes nach Vigenère
    ''' </summary>
    ''' <param name="sender">Objekt, dass das Ereignis ausgelöst hat</param>
    ''' <param name="e">Gesonderte Eigenschaften des Ereignisses</param>
    Private Sub ButtonVerschlüsseln_Vig_Click(sender As Object, e As RoutedEventArgs)
        Dim key = txt_schlüssel_vig.Text
        txt_ciphertext_vig.Text = CryptVigenère(key, txt_plaintext_vig.Text, Function(x) x)
    End Sub

    ''' <summary>
    ''' Ereignisbehandlung für die Entschlüsselung eines ciphertext s nach Vigenère
    ''' </summary>
    ''' <param name="sender">Objekt, dass das Ereignis ausgelöst hat</param>
    ''' <param name="e">Gesonderte Eigenschaften des Ereignisses</param>
    Private Sub ButtonEntschlüsseln_Vig_Click(sender As Object, e As RoutedEventArgs)
        Dim key = txt_schlüssel_vig.Text
        txt_plaintext_vig.Text = CryptVigenère(key, txt_ciphertext_vig.Text, Function(x) -x)
    End Sub

    Private Sub Cmd_test_kas_Click(sender As Object, e As RoutedEventArgs)
        txt_output_kas.Text = String.Empty
        Dim hugo = CryptAnalysis(txt_ciphertext_kas.Text)
        For Each key As String In hugo.Keys
            Dim list As List(Of Integer)
            If hugo.TryGetValue(key, list) Then
                txt_output_kas.Text += key + ": " + vbTab
                For Each pos As Integer In list
                    txt_output_kas.Text += pos.ToString() + ", "
                Next
                txt_output_kas.Text += vbNewLine
            End If
        Next
        txt_output_kas.Text += vbNewLine + "Abstände:" + vbNewLine
        Dim diffs = GetDiffs(hugo)
        For Each item As Integer In diffs
            txt_output_kas.Text += item.ToString() + vbNewLine
        Next
    End Sub
End Class
