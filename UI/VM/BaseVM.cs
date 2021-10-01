using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Utils;
using Utils.Extension;

namespace UI.VM
{
    public abstract class BaseVM : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        

        #region Constructors
        public BaseVM()
        {
                   
        }
        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }

        #endregion

        #region INotifyDataErrorInfo Members
        public List<string> propertyErrors { get; set; }
        public Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        [field: NonSerialized]
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
                if (errors.ContainsKey(propertyName))
                    return errors[propertyName];
            return new string[0];
        }

        public void AddError(string propertyName, string errorMessage)
        {
            bool oldHasErrors = HasErrors;
            if (!errors.ContainsKey(propertyName)) { errors.Add(propertyName, new List<string>()); }

            propertyErrors = errors[propertyName];
            if (propertyErrors.Count == 0)
            {
                propertyErrors.Add(errorMessage);
                RaiseErrorsChanged(propertyName);
            }

            if (oldHasErrors != HasErrors) { OnHasErrorsChanged(); }
        }

        public void RemoveError(string propertyName)
        {
            if (errors.ContainsKey(propertyName))
            {
                propertyErrors.Remove(propertyName);
                errors.Remove(propertyName);
            }

            RaiseErrorsChanged(propertyName);
        }

        protected virtual void OnHasErrorsChanged()
        {
            this.MutateVerbose(ref hasErrors, HasErrors, RaisePropertyChanged());
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        bool hasErrors = false;
        public bool HasErrors
        {
            get
            {
                hasErrors = errors.Count > 0;
                return hasErrors;
            }
        }

        #endregion INotifyDataErrorInfo Members

        #region properties
        public BaseVM MainVM { set; get; }


        private UserControl _maintView;
        public UserControl MainView
        {
            get
            {
                return _maintView;
            }
            set
            {
                this.MutateVerbose(ref _maintView, value, RaisePropertyChanged());
            }
        }
        #endregion



    }
}
