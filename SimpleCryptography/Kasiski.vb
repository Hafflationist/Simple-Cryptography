Module Kasiski
    Function CryptAnalysis(ciphertext As String) As Dictionary(Of String, List(Of Integer))
        Dim length = ciphertext.Length / 2
        Dim positionsOfSubstrings = CryptAnalysis_inner(ciphertext, length, New Dictionary(Of String, List(Of Integer)))
        ' Neues Dictionary ohne Einzelvorkommen erstellen:
        Dim relevantPositions = New Dictionary(Of String, List(Of Integer))
        For Each key As String In positionsOfSubstrings.Keys
            Dim list As List(Of Integer)
            If positionsOfSubstrings.TryGetValue(key, list) Then
                If list.Count >= 2 Then
                    relevantPositions.Add(key, list)
                End If
            End If
        Next
        Return relevantPositions
    End Function

    Function CryptAnalysis_inner(ciphertext As String, n As Integer, dicc As Dictionary(Of String, List(Of Integer))) As Dictionary(Of String, List(Of Integer))
        If n < 2 Then
            Return dicc
        End If
        Dim toBeChecked = GetSubstrings(ciphertext, n)

        For Each substr As String In toBeChecked
            Console.WriteLine("toBeChecked: " + substr)
            Dim positions = GetPositions(ciphertext, substr)
            dicc.Add(substr, positions)
        Next
        ' weitere Berechnungen mit den Positionen

        Return CryptAnalysis_inner(ciphertext, n - 1, dicc)
    End Function



    Function GetSubstrings(text As String, n As Integer) As HashSet(Of String)
        Dim knownSubstrings = New HashSet(Of String)
        For index As Integer = 0 To (text.Length - (2 * n))
            Dim substring = text.Substring(index, n)
            knownSubstrings.Add(substring)
        Next
        Return knownSubstrings
    End Function


    Function GetPositions(text As String, substring As String) As List(Of Integer)
        Dim posList = New List(Of Integer)
        Dim pos = -1
        Do
            pos += 1
            pos = text.IndexOf(substring, pos)
            Console.WriteLine(vbTab + "pos: " + pos.ToString())
            If pos <> -1 Then
                posList.Add(pos)
            End If
        Loop Until pos = -1
        Return posList
    End Function
End Module
