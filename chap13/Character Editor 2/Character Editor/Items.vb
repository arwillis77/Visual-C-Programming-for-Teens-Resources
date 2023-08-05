Imports System.Xml

Public Class Items

    REM keep public for easy access
    Public items As List(Of Item)

    Public Sub New()
        items = New List(Of Item)
    End Sub

    Private Function getElement(ByVal field As String, ByRef element As XmlElement) As String
        Dim value As String = ""
        Try
            value = element.GetElementsByTagName(field)(0).InnerText
        Catch ex As Exception
            REM ignore error, just return empty
            Console.WriteLine(ex.Message)
        End Try
        Return value
    End Function

    Public Function Load(ByVal filename As String) As Boolean
        Try
            REM open the xml file 
            Dim doc As New XmlDocument()
            doc.Load(filename)
            Dim list As XmlNodeList = doc.GetElementsByTagName("item")
            For Each node As XmlNode In list

                REM get next item in table
                Dim element As XmlElement = node
                Dim item As New Item()

                REM store fields in new Item
                item.Name = getElement("name", element)
                item.Description = getElement("description", element)
                item.DropImageFilename = getElement("dropimagefilename", element)
                item.InvImageFilename = getElement("invimagefilename", element)
                item.Category = getElement("category", element)
                item.Weight = Convert.ToSingle(getElement("weight", element))
                item.Value = Convert.ToSingle(getElement("value", element))
                item.AttackNumDice = Convert.ToInt32(getElement("attacknumdice", element))
                item.AttackDie = Convert.ToInt32(getElement("attackdie", element))
                item.Defense = Convert.ToInt32(getElement("defense", element))
                item.STR = Convert.ToInt32(getElement("STR", element))
                item.DEX = Convert.ToInt32(getElement("DEX", element))
                item.STA = Convert.ToInt32(getElement("STA", element))
                item.INT = Convert.ToInt32(getElement("INT", element))
                item.CHA = Convert.ToInt32(getElement("CHA", element))

                REM add new item to list
                items.Add(item)
            Next
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Public Function getItem(ByVal name As String) As Item
        For Each it As Item In items
            If it.Name = name Then
                Return it
            End If
        Next
        Return Nothing
    End Function

End Class
