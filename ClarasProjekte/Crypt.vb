''' <summary>
''' Modul für kryptografische Funktionen
''' </summary>
Public Module Crypt
    ''' <summary>
    ''' Diese Funktion veschiebt ein Zeichen um den angegebenen Offset innerhalb eines angegebenen, sortierten Zeichensatzes
    ''' </summary>
    ''' <param name="alphabet">Der zu nutzende Zeichensatz</param>
    ''' <param name="character">Ein Exemplar des Zeichensatzes, das verschlüsselt werden soll</param>
    ''' <param name="key">Verschlüsselungsschlüssel (Offset der Caeserverschlüsselung)</param>
    ''' <returns></returns>
    Function CaeserizeChar(alphabet As String, character As Char, key As Integer) As Char
        If alphabet.ToLower().Contains(character) Then ' Zeichen liegt im Kleinbuchstabenbereich
            alphabet = alphabet.ToLower()
        ElseIf alphabet.ToUpper().Contains(character) Then
            alphabet = alphabet.ToUpper()
        Else
            Return "*"
        End If

        Dim realKey = key
        While realKey < 0
            realKey += alphabet.Length
        End While
        Dim newPosition = (alphabet.IndexOf(character) + realKey) Mod alphabet.Length
        Return alphabet.Chars(newPosition)
    End Function

    Private Const alphabetLowercase As String = "abcdefghijklmnopqrstuvwxyz"

    ''' <summary>
    ''' Diese Funktion verschlüsselt einen Text nach Vigenère per Schlüssel und einem Keymapping. Die Verschlüsselung basiert auf dem Standardalphabet von a bis z.
    ''' </summary>
    ''' <param name="keyString">Der Schlüssel als Zeichenkette</param>
    ''' <param name="text">Der zu verschlüsselnde Text</param>
    ''' <param name="keyMapping">Jeder aus dem keyString berechnete Offset wird durch diese Funktion auf einen anderen Offset gemapt. Diese Funktion sollte innerhalb des Restklassenringes bijektiv sein.</param>
    ''' <returns></returns>
    Function CryptVigenère(keyString As String, text As String, keyMapping As Func(Of Integer, Integer)) As String
        Dim enumerable = text.AsEnumerable()
        Dim ciphertext = New List(Of Char)
        Dim counter = 0
        For Each character As Char In text.AsEnumerable()
            Dim charOfKey = keyString.ToLower().Chars(counter Mod keyString.Length)
            Dim key = alphabetLowercase.IndexOf(charOfKey)
            ciphertext.Add(CaeserizeChar(alphabetLowercase, character, keyMapping(key)))
            counter += 1
        Next
        If ciphertext.Contains("*") Then
            MsgBox("Sie haben versucht invalide Zeichen zu verschlüsseln! (Markiert mit '*') Es sind nur Groß- und Kleinbuchstaben des einfachen Alphabets zulässig.")
        End If
        Return New String(ciphertext.ToArray())
    End Function

    ''' <summary>
    ''' Diese Funktion verschlüsselt einen Text nach Caser per Schlüssel. Die Verschlüsselung basiert auf dem Standardalphabet von a bis z.
    ''' </summary>
    ''' <param name="key">Der Schlüssel als Ganzzahl</param>
    ''' <param name="text">Der zu verschlüsselnde Text</param>
    ''' <returns></returns>
    Function CryptCaesar(key As Integer, text As String) As String
        Dim enumerable As IEnumerable(Of Char) = text.AsEnumerable()
        Dim ciphertext = New List(Of Char)
        For Each character As Char In text.AsEnumerable()
            ciphertext.Add(CaeserizeChar(alphabetLowercase, character, key))
        Next
        If ciphertext.Contains("*") Then
            MsgBox("Sie haben versucht invalide Zeichen zu verschlüsseln! (Markiert mit '*') Es sind nur Groß- und Kleinbuchstaben des einfachen Alphabets zulässig.")
        End If
        Return New String(ciphertext.ToArray())
    End Function




    Function CryptAnalysis(ciphertext As String)
        Dim length = ciphertext.Length / 2
        CryptAnalysis_inner(ciphertext, length)
    End Function

    Function CryptAnalysis_inner(ciphertext As String, n As Integer)
        Dim toBeChecked = GetSubstrings(ciphertext, n)
        Console.WriteLine("toBeChecked: " + toBeChecked(0))
        Return GetPositions(ciphertext, toBeChecked(0))
    End Function



    Function GetSubstrings(text As String, n As Integer) As List(Of String)
        Dim knownSubstrings As List(Of String) = New List(Of String)
        For index As Integer = 0 To (text.Length - (2 * n))
            Dim substring = text.Substring(index, n)
            If Not knownSubstrings.Contains(substring) Then
                knownSubstrings.Add(substring)
                Console.WriteLine()
            End If
        Next
        Return knownSubstrings
    End Function


    Function GetPositions(text As String, substring As String) As List(Of Integer)
        Dim posList = New List(Of Integer)
        Dim pos = -1
        Do
            pos += 1
            pos = text.IndexOf(substring, pos)
            Console.WriteLine("pos: " + pos.ToString())
            If pos <> -1 Then
                posList.Add(pos)
            End If
        Loop Until pos = -1
        Return posList
    End Function

End Module
