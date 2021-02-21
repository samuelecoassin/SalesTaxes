Class Application

    ' Gli eventi a livello di applicazione, ad esempio Startup, Exit e DispatcherUnhandledException,
    ' possono essere gestiti in questo file.
    Protected Overrides Sub OnStartup(e As StartupEventArgs)
        MyBase.OnStartup(e)

        Dim stVM = New SalesTaxesViewModel
        Dim st = New SalesTaxes With {.DataContext = stVM}

        st.Show()
    End Sub

End Class
