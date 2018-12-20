Public Structure KasiskiResult
    Public FactorsWithCount As Dictionary(Of Integer, Integer)
    Public Distances As List(Of Integer)
End Structure

Module Kasiski
    ''' <summary>
    ''' Führt auf einen Eingabestring den Kasiskitest durch und gibt ein KasiskiResult-Objekt zurück
    ''' </summary>
    ''' <param name="ciphertext">Das Chiffrat, worauf der Test angewandt werden soll.</param>
    ''' <returns>Objekt mit Ergebnissen des Tests</returns>
    Function CryptAnalysis(ciphertext As String) As KasiskiResult
        Dim result As KasiskiResult
        ciphertext = New String(ciphertext.Where(Function(x) Not Char.IsWhiteSpace(x)).ToArray())
        Dim length = ciphertext.Length / 2
        Dim positionsOfSubstrings = GetSubstrWithPosition(ciphertext, length, New Dictionary(Of String, List(Of Integer)))
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
        result.Distances = GetDiffs(relevantPositions)

        Dim primeFactors = New List(Of Integer)
        For Each num As Integer In result.Distances
            primeFactors.AddRange(Factorize(num))
        Next
        result.FactorsWithCount = CountCopies(primeFactors)

        Dim factorCount = result.FactorsWithCount.Values.Sum()
        Dim reducedFactors = New Dictionary(Of Integer, Integer)
        For Each key As Integer In result.FactorsWithCount.Keys
            Dim count As Integer
            result.FactorsWithCount.TryGetValue(key, count)
            If count > factorCount / 10 Then
                reducedFactors.Add(key, count)
            End If
        Next
        result.FactorsWithCount = reducedFactors
        Return result
    End Function


    ''' <summary>
    ''' Zählt die Kopien einer List und gibt ein Dictionary zurück, das jedes Element der Liste mit dessen Häufigkeit aufführt.
    ''' </summary>
    ''' <param name="list">Die zu untersuchende Liste</param>
    ''' <returns>Dictionary mit den einzelnen Elementen der Eingabeliste und deren Häufigkeiten</returns>
    Private Function CountCopies(list As List(Of Integer)) As Dictionary(Of Integer, Integer)
        Dim dicc = New Dictionary(Of Integer, Integer)
        For Each number As Integer In list
            If dicc.ContainsKey(number) Then
                Dim numberCount As Integer
                dicc.TryGetValue(number, numberCount)
                dicc.Remove(number)
                numberCount += 1
                dicc.Add(number, numberCount)
            Else
                dicc.Add(number, 1)
            End If
        Next
        Return dicc
    End Function

    ''' <summary>
    ''' Primfaktorzerlegung
    ''' </summary>
    ''' <param name="num">Die Zahl, zu der die Primfaktoren bestimmt werden sollen</param>
    ''' <returns>Liste mit allen Primfaktoren</returns>
    Private Function Factorize(ByVal num As Integer) As List(Of Integer)
        Dim result As List(Of Integer) = New List(Of Integer)()
        Do While (num Mod 2 = 0)
            result.Add(2)
            num \= 2
        Loop
        Dim factor As Integer = 3
        Do While (factor * factor <= num)
            If (num Mod factor = 0) Then
                result.Add(factor)
                num \= factor
            Else
                factor += 2
            End If
        Loop
        If (num > 1) Then result.Add(num)
        Return result
    End Function

    ''' <summary>
    ''' Berechnet die Abstände von Datenwerten
    ''' </summary>
    ''' <param name="dicc">Datenwerte werde als Dictionary mit Datenwert und ihrer Position erwartet, aus der die Abstände berechnet werden</param>
    ''' <returns></returns>
    Private Function GetDiffs(dicc As Dictionary(Of String, List(Of Integer))) As List(Of Integer)
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

    ''' <summary>
    ''' Berechnet aus einem Chiffrat Substrings und deren Postionen
    ''' </summary>
    ''' <param name="ciphertext">Das Chiffrat</param>
    ''' <param name="n">Maximale Länge der Substrings, die gesucht werden</param>
    ''' <param name="dicc">Auf dieses Dictionary werden die Werte reingespeichert</param>
    ''' <returns></returns>
    Private Function GetSubstrWithPosition(ciphertext As String, n As Integer, dicc As Dictionary(Of String, List(Of Integer))) As Dictionary(Of String, List(Of Integer))
        If n < 2 Then
            Return dicc
        End If
        Dim toBeChecked = GetSubstrings(ciphertext, n)

        For Each substr As String In toBeChecked
            Dim positions = GetPositions(ciphertext, substr)
            dicc.Add(substr, positions)
        Next
        Return GetSubstrWithPosition(ciphertext, n - 1, dicc)
    End Function


    ''' <summary>
    ''' Findet alle Substrings einer Länge innerhalb eines gegebenen Textes
    ''' </summary>
    ''' <param name="text">Der String, aus dem die Substrings bestimmt werden</param>
    ''' <param name="n">Die Länge der zu bestimmenden Substrings.</param>
    ''' <returns></returns>
    Private Function GetSubstrings(text As String, n As Integer) As HashSet(Of String)
        Dim knownSubstrings = New HashSet(Of String)
        For index As Integer = 0 To (text.Length - (2 * n))
            Dim substring = text.Substring(index, n)
            knownSubstrings.Add(substring)
        Next
        Return knownSubstrings
    End Function

    ''' <summary>
    ''' Berechnet Positionen eines Substrings innerhalb eines Strings
    ''' </summary>
    ''' <param name="text">String, in dem gesucht werden soll</param>
    ''' <param name="substring">String, nach dem gesucht werden soll</param>
    ''' <returns>Eine Liste mit allen Positionen eines Substrings</returns>
    Private Function GetPositions(text As String, substring As String) As List(Of Integer)
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