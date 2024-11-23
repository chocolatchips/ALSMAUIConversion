using System;
using System.Windows.Input;

namespace WPSConversion
{
    public class DelegateCommand : ICommand
    {
        private readonly Func<bool> _canExecute;
        private readonly Action _execute;

        public event EventHandler? CanExecuteChanged;

        public DelegateCommand(Action execute, Func<bool> canExecute = null)
        {
            _canExecute = canExecute;
            _execute = execute;
        }

        //public event EventHandler CanExecuteChanged
        //{
        //    add => CommandManager.RequerySuggested += value;
        //    remove => CommandManager.RequerySuggested -= value;
        //}

        public bool CanExecute()
        {
            return _canExecute?.Invoke() ?? true;
        }

        bool ICommand.CanExecute(object parameter) => CanExecute();

        public void Execute()
        {
            _execute?.Invoke();
        }

        void ICommand.Execute(object parameter) => Execute();

        public void RaiseCanExecuteChanged()
        {
            //CommandManager.InvalidateRequerySuggested();
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
