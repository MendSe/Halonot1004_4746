// The RelayCommand class implements the ICommand interface,
// which is used for commanding in WPF applications.
using System.Windows.Input;
using System;

namespace PL
{
    public class RelayCommand : ICommand
    {
        // Declare an Action delegate to hold the method that will be executed when the command is invoked.
        private readonly Action<object> _execute;

        // Declare a Predicate delegate to hold the method that determines if the command can be executed.
        private readonly Predicate<object> _canExecute;

        // Constructor that takes the method to execute and an optional method to check if the command can be executed.
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            // Throw an ArgumentNullException if the execute delegate is null, otherwise assign it to the _execute field.
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));

            // Assign the canExecute delegate to the _canExecute field.
            _canExecute = canExecute;
        }

        // Event handler for the CanExecuteChanged event. 
        // This event is raised when the command manager detects conditions that might change the ability of a command to execute.
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        // Method to determine if the command can be executed.
        public bool CanExecute(object parameter)
        {
            // If there is no _canExecute delegate, return true; otherwise, call the _canExecute delegate with the provided parameter.
            return _canExecute == null || _canExecute(parameter);
        }

        // Method to execute the command.
        public void Execute(object parameter)
        {
            // Call the _execute delegate with the provided parameter.
            _execute(parameter);
        }
    }
}
