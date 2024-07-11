using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PortlandXeroLib
{
    public class ReturnedAuthCodes : INotifyPropertyChanged
    {
        string _authorisationCode = string.Empty;
        string _state = string.Empty;

        public string AuthorisationCode
        {
            get { return _authorisationCode; }
            set
            {
                if (value != _authorisationCode)
                {
                    _authorisationCode = value;
                    OnPropertyChanged("AuthorisationCode");
                }
            }
        }

        public string State
        {
            get { return _state; }
            set
            {
                if (value != _state)
                {
                    _state = value;
                    OnPropertyChanged(nameof(State));
                }
            }
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }

    public class XeroTokens : INotifyPropertyChanged
    {
        string _id = string.Empty;
        string _access = string.Empty;
        string _refresh = string.Empty;

        public string Id
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public string Access
        {
            get { return _access; }
            set
            {
                if (value != _access)
                {
                    _access = value;
                    OnPropertyChanged("Access");
                }
            }
        }

        public string Refresh
        {
            get { return _refresh; }
            set
            {
                if (value != _refresh)
                {
                    _refresh = value;
                    OnPropertyChanged("Refresh");
                }
            }
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
