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

    ''' <summary>
    ''' Ereignisbehandlung für die Ausführung des Kasiskitests
    ''' </summary>
    ''' <param name="sender">Objekt, dass das Ereignis ausgelöst hat</param>
    ''' <param name="e">Gesonderte Eigenschaften des Ereignisses</param>
    Private Sub Cmd_test_kas_Click(sender As Object, e As RoutedEventArgs)
        txt_output_kas.Text = String.Empty
        Dim result = CryptAnalysis(txt_ciphertext_kas.Text)
        Dim factors = result.FactorsWithCount.Keys.ToList()
        factors.Sort(Function(x, y)
                         Dim countX As Integer
                         Dim countY As Integer
                         result.FactorsWithCount.TryGetValue(x, countX)
                         result.FactorsWithCount.TryGetValue(y, countY)
                         Return countY - countX
                     End Function)

        txt_output_kas.Text += "Primfaktoren nach Häufigkeit:" + vbNewLine

        For Each key As Integer In factors
            Dim numberCount As Integer
            result.FactorsWithCount.TryGetValue(key, numberCount)
            txt_output_kas.Text += key.ToString() + ":" + vbTab + numberCount.ToString() + vbNewLine
        Next
        txt_output_kas.Text += vbNewLine

        Dim possibleKeysLengthSet = New HashSet(Of Integer)

        factors.Add(1)
        For Each number1 As Integer In factors
            For Each number2 As Integer In factors
                possibleKeysLengthSet.Add(number1 * number2)
            Next
        Next
        possibleKeysLengthSet.Remove(1)
        Dim possibleKeysLength = possibleKeysLengthSet.ToList()
        possibleKeysLength.Sort(Function(x, y) y - x)
        txt_output_kas.Text += vbNewLine + "Folgende Werte und einige ihrer multiplikativen Kombinationen entsprechen wahrscheinlich der Länge des verwendeten Schlüssels:" + vbNewLine
        For Each length As Integer In possibleKeysLength
            txt_output_kas.Text += length.ToString() + vbNewLine
        Next


    End Sub
End Class