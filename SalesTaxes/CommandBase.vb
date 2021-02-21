Public MustInherit Class CommandBase
    Implements ICommand

    Public MustOverride Sub Execute(parameter As Object) Implements ICommand.Execute

    Public MustOverride Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute

    Public Custom Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
        AddHandler(value As EventHandler)
            AddHandler CommandManager.RequerySuggested, value
        End AddHandler

        RemoveHandler(value As EventHandler)
            RemoveHandler CommandManager.RequerySuggested, value
        End RemoveHandler

        RaiseEvent(sender As Object, e As EventArgs)
            OnCanExecuteChanged(e)
        End RaiseEvent
    End Event

    Protected Overridable Sub OnCanExecuteChanged(e As EventArgs)
        CommandManager.InvalidateRequerySuggested()
    End Sub
End Class
