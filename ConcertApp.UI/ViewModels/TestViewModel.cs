﻿using ConcertApp.BLL.DTO;
using ConcertApp.BLL.Services;
using ConcertApp.UI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcertApp.UI.ViewModels
{
    public class TestViewModel : BaseNotifyPropertyChanged
    {
        UserService userService;

        private UserDTO selectedBankUser;

        public UserDTO SelectedUser
        {
            get => selectedBankUser;
            set
            {
                selectedBankUser = value;
                NotifyPropertyChanged();
            }
        }

        public TestViewModel(UserService us)
        {
            userService = us;
            SelectedUser = new UserDTO();
        }

        public void InitUser(int userId)
        {
            SelectedUser = userService.Get(userId);
        }
    }
}
