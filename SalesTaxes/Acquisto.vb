Public Class Acquisto
    Inherits NotifyPropertyChanged

    Public Property Articolo() As Articolo
        Get
            Return _articolo
        End Get
        Set(ByVal value As Articolo)
            _articolo = value

            OnPropertyChanged()
        End Set
    End Property
    Private _articolo As Articolo

    Public Property Quantità() As Integer
        Get
            Return _quantità
        End Get
        Set(ByVal value As Integer)
            _quantità = value

            OnPropertyChanged()
        End Set
    End Property
    Private _quantità As Integer
End Class
