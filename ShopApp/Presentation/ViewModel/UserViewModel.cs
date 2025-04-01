using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Interfaces;
using Presentation.Model;

namespace Presentation.ViewModel
{
    public class UserViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        private UserModel _user;

        public UserModel User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged("User");
            }
        }

        public UserViewModel(IUserService userService)
        {
            _userService = userService;
            LoadUserData();
        }

        private void LoadUserData()
        {
            // W rzeczywistej aplikacji pobieralibyśmy dane z serwisu
            User = new UserModel
            {
                Id = 1,
                Name = "Jan Kowalski",
                Email = "jan.kowalski@example.com",
                Address = "ul. Przykładowa 123, 00-001 Warszawa",
                PhoneNumber = "+48 123 456 789",
                IsActive = true
            };
        }

        public void SaveUserData()
        {
            // Implementacja zapisywania danych użytkownika
            // W rzeczywistej aplikacji wywoływalibyśmy tu metodę serwisu
        }
    }

}
