'Date: 08\23\2010 - Ludvik Jerabek - Initial Release
'Version: 1.0
'Comment: Allow INI manipulation in .NET
'License: CPOL
'
'Revisions:
'
'08\23\2010 - Ludvik Jerabek - Initial Release
'11\12\2010 - Ludvik Jerabek - Fixed section regex matching on key values with brackets
'06\20\2015 - Ludvik Jerabek - Fixed key parsing regex to account for keys with spaces in names
'
'
'**DISCLAIMER**
'THIS MATERIAL IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
'EITHER EXPRESS OR IMPLIED, INCLUDING, BUT Not LIMITED TO, THE
'IMPLIED WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
'PURPOSE, OR NON-INFRINGEMENT. SOME JURISDICTIONS DO NOT ALLOW THE
'EXCLUSION OF IMPLIED WARRANTIES, SO THE ABOVE EXCLUSION MAY NOT
'APPLY TO YOU. IN NO EVENT WILL I BE LIABLE TO ANY PARTY FOR ANY
'DIRECT, INDIRECT, SPECIAL OR OTHER CONSEQUENTIAL DAMAGES FOR ANY
'USE OF THIS MATERIAL INCLUDING, WITHOUT LIMITATION, ANY LOST
'PROFITS, BUSINESS INTERRUPTION, LOSS OF PROGRAMS OR OTHER DATA ON
'YOUR INFORMATION HANDLING SYSTEM OR OTHERWISE, EVEN If WE ARE
'EXPRESSLY ADVISED OF THE POSSIBILITY OF SUCH DAMAGES.

Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Collections
Imports System.Diagnostics

' IniFile class used to read and write ini files by loading the file into memory
Public Class IniFile
    ' List of IniSection objects keeps track of all the sections in the INI file
    Private m_sections As Hashtable

    ' Public constructor
    Public Sub New()
        m_sections = New Hashtable(StringComparer.InvariantCultureIgnoreCase)
    End Sub

    ' Loads the Reads the data in the ini file into the IniFile object
    Public Sub Load(ByVal sFileName As String, Optional ByVal bMerge As Boolean = False)
        If Not bMerge Then
            RemoveAllSections()
        End If
        '  Clear the object... 
        Dim tempsection As IniSection = Nothing
        Dim oReader As New StreamReader(sFileName)
        Dim regexcomment As New Regex("^([\s]*#.*)", (RegexOptions.Singleline Or RegexOptions.IgnoreCase))
        Dim regexsection As New Regex("^[\s]*\[[\s]*([^\[\s].*[^\s\]])[\s]*\][\s]*$", (RegexOptions.Singleline Or RegexOptions.IgnoreCase))
        Dim regexkey As New Regex("^\s*([^=]*[^\s=])\s*=(.*)", (RegexOptions.Singleline Or RegexOptions.IgnoreCase))
        While Not oReader.EndOfStream
            Dim line As String = oReader.ReadLine()
            If line <> String.Empty Then
                Dim m As Match = Nothing
                If regexcomment.Match(line).Success Then
                    m = regexcomment.Match(line)
                    Trace.WriteLine(String.Format("Skipping Comment: {0}", m.Groups(0).Value))
                ElseIf regexsection.Match(line).Success Then
                    m = regexsection.Match(line)
                    Trace.WriteLine(String.Format("Adding section [{0}]", m.Groups(1).Value))
                    tempsection = AddSection(m.Groups(1).Value)
                ElseIf regexkey.Match(line).Success AndAlso tempsection IsNot Nothing Then
                    m = regexkey.Match(line)
                    Trace.WriteLine(String.Format("Adding Key [{0}]=[{1}]", m.Groups(1).Value, m.Groups(2).Value))
                    tempsection.AddKey(m.Groups(1).Value).Value = m.Groups(2).Value
                ElseIf tempsection IsNot Nothing Then
                    '  Handle Key without value
                    Trace.WriteLine(String.Format("Adding Key [{0}]", line))
                    tempsection.AddKey(line)
                Else
                    '  This should not occur unless the tempsection is not created yet...
                    Trace.WriteLine(String.Format("Skipping unknown type of data: {0}", line))
                End If
            End If
        End While
        oReader.Close()
    End Sub

    ' Used to save the data back to the file or your choice
    Public Sub Save(ByVal sFileName As String)
        Dim oWriter As New StreamWriter(sFileName, False)
        For Each s As IniSection In Sections
            Trace.WriteLine(String.Format("Writing Section: [{0}]", s.Name))
            oWriter.WriteLine(String.Format("[{0}]", s.Name))
            For Each k As IniSection.IniKey In s.Keys
                If k.Value <> String.Empty Then
                    Trace.WriteLine(String.Format("Writing Key: {0}={1}", k.Name, k.Value))
                    oWriter.WriteLine(String.Format("{0}={1}", k.Name, k.Value))
                Else
                    Trace.WriteLine(String.Format("Writing Key: {0}", k.Name))
                    oWriter.WriteLine(String.Format("{0}", k.Name))
                End If
            Next
        Next
        oWriter.Close()
    End Sub

    ' Gets all the sections
    Public ReadOnly Property Sections() As System.Collections.ICollection
        Get
            Return m_sections.Values
        End Get
    End Property

    ' Adds a section to the IniFile object, returns a IniSection object to the new or existing object
    Public Function AddSection(ByVal sSection As String) As IniSection
        Dim s As IniSection = Nothing
        sSection = sSection.Trim()
        ' Trim spaces
        If m_sections.ContainsKey(sSection) Then
            s = DirectCast(m_sections(sSection), IniSection)
        Else
            s = New IniSection(Me, sSection)
            m_sections(sSection) = s
        End If
        Return s
    End Function

    ' Removes a section by its name sSection, returns trus on success
    Public Function RemoveSection(ByVal sSection As String) As Boolean
        sSection = sSection.Trim()
        Return RemoveSection(GetSection(sSection))
    End Function

    ' Removes section by object, returns trus on success
    Public Function RemoveSection(ByVal Section As IniSection) As Boolean
        If Section IsNot Nothing Then
            Try
                m_sections.Remove(Section.Name)
                Return True
            Catch ex As Exception
                Trace.WriteLine(ex.Message)
            End Try
        End If
        Return False
    End Function

    '  Removes all existing sections, returns trus on success
    Public Function RemoveAllSections() As Boolean
        m_sections.Clear()
        Return (m_sections.Count = 0)
    End Function

    ' Returns an IniSection to the section by name, NULL if it was not found
    Public Function GetSection(ByVal sSection As String) As IniSection
        sSection = sSection.Trim()
        ' Trim spaces
        If m_sections.ContainsKey(sSection) Then
            Return DirectCast(m_sections(sSection), IniSection)
        End If
        Return Nothing
    End Function

    '  Returns a KeyValue in a certain section
    Public Function GetKeyValue(ByVal sSection As String, ByVal sKey As String) As String
        Dim s As IniSection = GetSection(sSection)
        If s IsNot Nothing Then
            Dim k As IniSection.IniKey = s.GetKey(sKey)
            If k IsNot Nothing Then
                Return k.Value
            End If
        End If
        Return String.Empty
    End Function

    ' Sets a KeyValuePair in a certain section
    Public Function SetKeyValue(ByVal sSection As String, ByVal sKey As String, ByVal sValue As String) As Boolean
        Dim s As IniSection = AddSection(sSection)
        If s IsNot Nothing Then
            Dim k As IniSection.IniKey = s.AddKey(sKey)
            If k IsNot Nothing Then
                k.Value = sValue
                Return True
            End If
        End If
        Return False
    End Function

    ' Renames an existing section returns true on success, false if the section didn't exist or there was another section with the same sNewSection
    Public Function RenameSection(ByVal sSection As String, ByVal sNewSection As String) As Boolean
        '  Note string trims are done in lower calls.
        Dim bRval As Boolean = False
        Dim s As IniSection = GetSection(sSection)
        If s IsNot Nothing Then
            bRval = s.SetName(sNewSection)
        End If
        Return bRval
    End Function

    ' Renames an existing key returns true on success, false if the key didn't exist or there was another section with the same sNewKey
    Public Function RenameKey(ByVal sSection As String, ByVal sKey As String, ByVal sNewKey As String) As Boolean
        '  Note string trims are done in lower calls.
        Dim s As IniSection = GetSection(sSection)
        If s IsNot Nothing Then
            Dim k As IniSection.IniKey = s.GetKey(sKey)
            If k IsNot Nothing Then
                Return k.SetName(sNewKey)
            End If
        End If
        Return False
    End Function

    ' Remove a key by section name and key name
    Public Function RemoveKey(ByVal sSection As String, ByVal sKey As String) As Boolean
        Dim s As IniSection = GetSection(sSection)
        If s IsNot Nothing Then
            Return s.RemoveKey(sKey)
        End If
        Return False
    End Function

    ' IniSection class 
    Public Class IniSection
        '  IniFile IniFile object instance
        Private m_pIniFile As IniFile
        '  Name of the section
        Private m_sSection As String
        '  List of IniKeys in the section
        Private m_keys As Hashtable

        ' Constuctor so objects are internally managed
        Protected Friend Sub New(ByVal parent As IniFile, ByVal sSection As String)
            m_pIniFile = parent
            m_sSection = sSection
            m_keys = New Hashtable(StringComparer.InvariantCultureIgnoreCase)
        End Sub

        ' Returns all the keys in a section
        Public ReadOnly Property Keys() As System.Collections.ICollection
            Get
                Return m_keys.Values
            End Get
        End Property

        ' Returns the section name
        Public ReadOnly Property Name() As String
            Get
                Return m_sSection
            End Get
        End Property

        ' Adds a key to the IniSection object, returns a IniKey object to the new or existing object
        Public Function AddKey(ByVal sKey As String) As IniKey
            sKey = sKey.Trim()
            Dim k As IniSection.IniKey = Nothing
            If sKey.Length <> 0 Then
                If m_keys.ContainsKey(sKey) Then
                    k = DirectCast(m_keys(sKey), IniKey)
                Else
                    k = New IniSection.IniKey(Me, sKey)
                    m_keys(sKey) = k
                End If
            End If
            Return k
        End Function

        ' Removes a single key by string
        Public Function RemoveKey(ByVal sKey As String) As Boolean
            Return RemoveKey(GetKey(sKey))
        End Function

        ' Removes a single key by IniKey object
        Public Function RemoveKey(ByVal Key As IniKey) As Boolean
            If Key IsNot Nothing Then
                Try
                    m_keys.Remove(Key.Name)
                    Return True
                Catch ex As Exception
                    Trace.WriteLine(ex.Message)
                End Try
            End If
            Return False
        End Function

        ' Removes all the keys in the section
        Public Function RemoveAllKeys() As Boolean
            m_keys.Clear()
            Return (m_keys.Count = 0)
        End Function

        ' Returns a IniKey object to the key by name, NULL if it was not found
        Public Function GetKey(ByVal sKey As String) As IniKey
            sKey = sKey.Trim()
            If m_keys.ContainsKey(sKey) Then
                Return DirectCast(m_keys(sKey), IniKey)
            End If
            Return Nothing
        End Function

        ' Sets the section name, returns true on success, fails if the section
        ' name sSection already exists
        Public Function SetName(ByVal sSection As String) As Boolean
            sSection = sSection.Trim()
            If sSection.Length <> 0 Then
                ' Get existing section if it even exists...
                Dim s As IniSection = m_pIniFile.GetSection(sSection)
                If s IsNot Me AndAlso s IsNot Nothing Then
                    Return False
                End If
                Try
                    ' Remove the current section
                    m_pIniFile.m_sections.Remove(m_sSection)
                    ' Set the new section name to this object
                    m_pIniFile.m_sections(sSection) = Me
                    ' Set the new section name
                    m_sSection = sSection
                    Return True
                Catch ex As Exception
                    Trace.WriteLine(ex.Message)
                End Try
            End If
            Return False
        End Function

        ' Returns the section name
        Public Function GetName() As String
            Return m_sSection
        End Function

        ' IniKey class
        Public Class IniKey
            '  Name of the Key
            Private m_sKey As String
            '  Value associated
            Private m_sValue As String
            '  Pointer to the parent CIniSection
            Private m_section As IniSection

            ' Constuctor so objects are internally managed
            Protected Friend Sub New(ByVal parent As IniSection, ByVal sKey As String)
                m_section = parent
                m_sKey = sKey
            End Sub

            ' Returns the name of the Key
            Public ReadOnly Property Name() As String
                Get
                    Return m_sKey
                End Get
            End Property

            ' Sets or Gets the value of the key
            Public Property Value() As String
                Get
                    Return m_sValue
                End Get
                Set(ByVal value As String)
                    m_sValue = value
                End Set
            End Property

            ' Sets the value of the key
            Public Sub SetValue(ByVal sValue As String)
                m_sValue = sValue
            End Sub
            ' Returns the value of the Key
            Public Function GetValue() As String
                Return m_sValue
            End Function

            ' Sets the key name
            ' Returns true on success, fails if the section name sKey already exists
            Public Function SetName(ByVal sKey As String) As Boolean
                sKey = sKey.Trim()
                If sKey.Length <> 0 Then
                    Dim k As IniKey = m_section.GetKey(sKey)
                    If k IsNot Me AndAlso k IsNot Nothing Then
                        Return False
                    End If
                    Try
                        ' Remove the current key
                        m_section.m_keys.Remove(m_sKey)
                        ' Set the new key name to this object
                        m_section.m_keys(sKey) = Me
                        ' Set the new key name
                        m_sKey = sKey
                        Return True
                    Catch ex As Exception
                        Trace.WriteLine(ex.Message)
                    End Try
                End If
                Return False
            End Function

            ' Returns the name of the Key
            Public Function GetName() As String
                Return m_sKey
            End Function
        End Class
        ' End of IniKey class
    End Class
    ' End of IniSection class
End Class
' End of IniFile class
