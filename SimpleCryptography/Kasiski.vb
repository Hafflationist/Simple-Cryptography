Module Kasiski
    Function CryptAnalysis(ciphertext As String) As Dictionary(Of String, List(Of Integer))
        ciphertext = New String(ciphertext.Where(Function(x) Not Char.IsWhiteSpace(x)).ToArray())
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
        Dim diffs = GetDiffs(relevantPositions)
        Return relevantPositions
    End Function


    Private Function FindFactors(ByVal num As Long) As List(Of
    Long)
        Dim result As List(Of Long) = New List(Of Long)()

        ' Take out the 2s.
        Do While (num Mod 2 = 0)
            result.Add(2)
            num \= 2
        Loop

        ' Take out other primes.
        Dim factor As Long = 3
        Do While (factor * factor <= num)
            If (num Mod factor = 0) Then
                ' This is a factor.
                result.Add(factor)
                num \= factor
            Else
                ' Go to the next odd number.
                factor += 2
            End If
        Loop

        ' If num is not 1, then whatever is left is prime.
        If (num > 1) Then result.Add(num)

        Return result
    End Function

    Function GetDiffs(dicc As Dictionary(Of String, List(Of Integer))) As List(Of Integer)
        Dim diffList = New List(Of Integer)
        For Each posList As List(Of Integer) In dicc.Values
            Dim lastPos = posList(0)
            For Each actualPos As Integer In posList
                If Not lastPos = actualPos Then
                    diffList.Add(actualPos - lastPos)
                    lastPos = actualPos
                End If
            Next
        Next
        Return diffList
    End Function

    Function CryptAnalysis_inner(ciphertext As String, n As Integer, dicc As Dictionary(Of String, List(Of Integer))) As Dictionary(Of String, List(Of Integer))
        If n < 2 Then
            Return dicc
        End If
        Dim toBeChecked = GetSubstrings(ciphertext, n)

        For Each substr As String In toBeChecked
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
            If pos <> -1 Then
                posList.Add(pos)
            End If
        Loop Until pos = -1
        Return posList
    End Function
End Module
