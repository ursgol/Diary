using Diary.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Models
{
    public class UserSettings : IDataErrorInfo
    {
        private bool _isAddressNameValid;
        private bool _isServerNameValid;
        private bool _isDbNameValid;
        private bool _isPasswordValid;
        private bool _isUserValid;


        public string AddressName { 
            get 
            {
                return Settings.Default.AddressName;
            }
            set 
            {
                Settings.Default.AddressName = value;
            } 
        }
        public string ServerName 
        {
            get
            {
                return Settings.Default.ServerName;
            }
            set
            {
                Settings.Default.ServerName = value;
            }
        }
        public string DbName 
        {
            get
            {
                return Settings.Default.DbName;
            }
            set
            {
                Settings.Default.DbName = value;
            }
        }
        public string User 
        {
            get 
            {
                return Settings.Default.user;
            }
            set 
            {
                Settings.Default.user = value;
            } 
        }
        public string Password 
        {
            get
            {
                return Settings.Default.password;
            }
            set
            {
                Settings.Default.password = value;
            }
        }

        public void Save()
        {
            Settings.Default.Save();
        }

        public string Error
        {
            get; set;
        }

        public bool IsValid
        {
            get
            {
                return _isAddressNameValid &&
                    _isServerNameValid &&
                    _isDbNameValid &&
                    _isUserValid &&
                    _isPasswordValid;
            }
        }

        public string this[string columnName] 
        {
            get
            {
                switch (columnName)
                {
                    case nameof(AddressName):
                        if (string.IsNullOrWhiteSpace(AddressName))
                        {
                            Error = "Pole adres serwera jest wymagane.";
                            _isAddressNameValid = false;
                        }
                        else
                        {
                            Error = string.Empty;
                            _isAddressNameValid = true;
                        }
                        break;
                    case nameof(ServerName):
                        if (string.IsNullOrWhiteSpace(ServerName))
                        {
                            Error = "Pole nazwa serwera jest wymagane.";
                            _isServerNameValid = false;
                        }
                        else
                        {
                            Error = string.Empty;
                            _isServerNameValid = true;
                        }
                        break;
                    case nameof(DbName):
                        if (string.IsNullOrWhiteSpace(DbName))
                        {
                            Error = "Pole nazwa bazy danych jest wymagane.";
                            _isDbNameValid = false;
                        }
                        else
                        {
                            Error = string.Empty;
                            _isDbNameValid = true;
                        }
                        break;
                    case nameof(User):
                        if (string.IsNullOrWhiteSpace(User))
                        {
                            Error = "Pole użytkownika jest wymagane.";
                            _isUserValid = false;
                        }
                        else
                        {
                            Error = string.Empty;
                            _isUserValid = true;
                        }
                        break;
                    case nameof(Password):
                        if (string.IsNullOrWhiteSpace(Password))
                        {
                            Error = "Pole hasło jest wymagane.";
                            _isPasswordValid = false;
                        }
                        else
                        {
                            Error = string.Empty;
                            _isPasswordValid = true;
                        }
                        break;
                    default:
                        break;
                }
                return Error;
            }
        }

     
       
    }
}
