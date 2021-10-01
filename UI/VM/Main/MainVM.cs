using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.VM.Main
{
    public class MainVM : BaseVM
    {

        private static MainVM _instance;
        private static object syncLock = new object();
        public static MainVM Instance()
        {

            if (_instance == null)
            {
                lock (syncLock)
                {
                    if (_instance == null)
                    {
                        _instance = new MainVM();
                    }
                }
            }

            return _instance;
        }
        public MainVM()
        {
            MainVM = this;

        }
    }
}
