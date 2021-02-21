Imports System.ComponentModel
Imports GalaSoft.MvvmLight.Command

Public Class SalesTaxesViewModel
    Inherits NotifyPropertyChanged

    Public TassaBase As Decimal
    Public TassaImportazione As Decimal

    Sub New()
        Me.Articoli = New List(Of Articolo) From {New Articolo(0, " ", 0, False, False),
                                                  New Articolo(1, "Book", 12.49, True, False),
                                                  New Articolo(2, "Music CD", 14.99, False, False),
                                                  New Articolo(3, "Chocolate bar", 0.85, True, False),
                                                  New Articolo(4, "Box of chocolates I", 10, True, True),
                                                  New Articolo(5, "Bottle of perfume I", 47.5, False, True),
                                                  New Articolo(6, "Bottle of perfume II", 27.99, False, True),
                                                  New Articolo(7, "Bottle of perfume III", 18.99, False, False),
                                                  New Articolo(8, "Headache pills", 9.75, True, False),
                                                  New Articolo(9, "Box of chocolates II", 11.25, True, True)}
        Me.Acquisti = New List(Of Acquisto)

        ' Percentuale 
        Me.TassaBase = 10
        Me.TassaImportazione = 5
    End Sub

#Region "Property"

    Public Property Articoli() As List(Of Articolo)
        Get
            Return _articoli
        End Get
        Set(ByVal value As List(Of Articolo))
            _articoli = value

            OnPropertyChanged()
        End Set
    End Property
    Private _articoli As List(Of Articolo)

    Public Property ArticoloSelezionato() As Articolo
        Get
            Return _articoloSelezionato
        End Get
        Set(ByVal value As Articolo)
            _articoloSelezionato = value

            OnPropertyChanged()
        End Set
    End Property
    Private _articoloSelezionato As Articolo

    Public Property Acquisti() As List(Of Acquisto)
        Get
            Return _acquisti
        End Get
        Set(ByVal value As List(Of Acquisto))
            _acquisti = value

            OnPropertyChanged()
        End Set
    End Property
    Private _acquisti As List(Of Acquisto)

    Public Property QuantitàSelezionata() As Integer
        Get
            Return _quantitàSelezionata
        End Get
        Set(ByVal value As Integer)
            _quantitàSelezionata = value

            OnPropertyChanged()
        End Set
    End Property
    Private _quantitàSelezionata As Integer

    Public Property Risultato() As String
        Get
            Return _risultato
        End Get
        Set(ByVal value As String)
            _risultato = value

            OnPropertyChanged()
        End Set
    End Property
    Private _risultato As String
#End Region

#Region "Aggiunta articolo"

    Public ReadOnly Property AggiungiArticolo() As ICommand
        Get
            If _aggiungiArticolo Is Nothing Then _aggiungiArticolo = New AggiungiArticoloCommand(Me)
            Return _aggiungiArticolo
        End Get
    End Property
    Private _aggiungiArticolo As ICommand

    Private Class AggiungiArticoloCommand
        Inherits CommandBase

        Private _vm As SalesTaxesViewModel

        Sub New(vm As SalesTaxesViewModel)
            _vm = vm
        End Sub

        Public Overrides Sub Execute(parameter As Object)
            Dim acquisto = _vm.Acquisti.FirstOrDefault(Function(acq) acq.Articolo.ID = _vm.ArticoloSelezionato.ID)

            If acquisto IsNot Nothing Then
                acquisto.Quantità += _vm.QuantitàSelezionata
            Else
                _vm.Acquisti.Add(New Acquisto() With {.Articolo = _vm.ArticoloSelezionato, .Quantità = _vm.QuantitàSelezionata})
            End If

            _vm.QuantitàSelezionata = 0
            _vm.ArticoloSelezionato = _vm.Articoli.FirstOrDefault(Function(x) x.ID = 0)

            Me.AggiornaRisultato()
        End Sub

        Public Overrides Function CanExecute(parameter As Object) As Boolean
            Return _vm.ArticoloSelezionato IsNot Nothing AndAlso _vm.ArticoloSelezionato.ID <> 0 AndAlso _vm.QuantitàSelezionata > 0
        End Function

        Private Sub AggiornaRisultato()
            Dim risultato = String.Empty
            Dim totale As Decimal
            Dim totaleTasse As Decimal

            _vm.Acquisti.ForEach(Sub(acq)
                                     Dim totaleArticolo As Decimal = acq.Articolo.Prezzo * acq.Quantità
                                     Dim tassa1 As Decimal, tassa2 As Decimal
                                     If Not acq.Articolo.Esente Then
                                         tassa1 = Me.Arrotonda((totaleArticolo * _vm.TassaBase) / 100D)
                                     End If
                                     If acq.Articolo.Importato Then
                                         tassa2 = Me.Arrotonda((totaleArticolo * _vm.TassaImportazione) / 100D)
                                     End If
                                     totaleArticolo += tassa1
                                     totaleArticolo += tassa2
                                     totaleTasse += tassa1
                                     totaleTasse += tassa2

                                     risultato &= $"{acq.Quantità} {acq.Articolo.Nome}: {String.Format("{0:0.00}", totaleArticolo)}{vbNewLine}"

                                     totale += totaleArticolo
                                 End Sub)

            risultato &= $"Sales Taxes: {String.Format("{0:0.00}", totaleTasse)}{vbNewLine}"
            risultato &= $"Total: {String.Format("{0:0.00}", totale)}"

            _vm.Risultato = risultato
        End Sub

        Private Function Arrotonda(valore As Decimal) As Decimal
            Dim parteDecimale As Decimal = valore * 10 - Math.Floor(valore * 10)
            If parteDecimale < 0.25 Then
                Return Math.Floor(valore * 10) / 10
            ElseIf parteDecimale >= 0.75 Then
                Return (Math.Floor(valore * 10) + 1) / 10
            Else
                Return (Math.Floor(valore * 10) + 0.5) / 10
            End If

        End Function
    End Class
#End Region

#Region "Aggiunta articolo"

    Public ReadOnly Property Azzera() As ICommand
        Get
            If _azzera Is Nothing Then _azzera = New AzzeraCommand(Me)
            Return _azzera
        End Get
    End Property
    Private _azzera As ICommand

    Private Class AzzeraCommand
        Inherits CommandBase

        Private _vm As SalesTaxesViewModel

        Sub New(vm As SalesTaxesViewModel)
            _vm = vm
        End Sub

        Public Overrides Sub Execute(parameter As Object)
            _vm.Acquisti.Clear()
            _vm.Risultato = String.Empty
        End Sub

        Public Overrides Function CanExecute(parameter As Object) As Boolean
            Return True
        End Function
    End Class
#End Region
End Class
