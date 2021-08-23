using System;
using System.Windows.Input;

namespace ViewModels.Commands
{
    class ButtonCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private ConnectionViewModel obj; // Point 1

        public ButtonCommand(ConnectionViewModel obj) // Point 2
        {
            this.obj = obj;
        }

        public bool CanExecute(object parameter)
        {
            return true; // Point 3
        }
        public void Execute(object parameter)
        {
            // Switch states
            if(obj.Status == "Online")
                obj.Status = "Offline";
            else
                obj.Status = "Online";
        }
    }
}
