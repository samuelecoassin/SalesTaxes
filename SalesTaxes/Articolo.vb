Public Class Articolo

    Sub New(id As Integer, nome As String, prezzo As Decimal, esente As Boolean, importato As Boolean)
        Me.ID = id
        Me.Nome = nome
        Me.Prezzo = prezzo
        Me.Esente = esente
        Me.Importato = importato
    End Sub

    Public Property ID() As Integer

    Public Property Nome() As String

    Public Property Prezzo() As Decimal

    'TODO: sostituibile con categoria di appartenenza. In seguito fare un controllo sulla categoria per determinare se è esente tasse o meno.
    Public Property Esente() As Boolean

    Public Property Importato() As Boolean

End Class
