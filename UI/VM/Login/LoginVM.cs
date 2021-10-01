using UI.View.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace UI.VM.Login
{
    public class LoginVM : BaseVM
    {
        public DelegateCommand LoginCommand { get; private set; }
        public LoginVM()
        {
            MainVM = Main.MainVM.Instance();
            LoginCommand = new DelegateCommand(OnLoginCommand);
        }

        /// <summary>
        /// login button click method
        /// </summary>
        /// <param name="obj"></param>
        private void OnLoginCommand(object obj)
        {

            MainVM.MainView = new HomeView();

        }
    }
}
