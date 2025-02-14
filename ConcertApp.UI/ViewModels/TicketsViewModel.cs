﻿using ConcertApp.BLL.DTO;
using ConcertApp.BLL.Services;
using ConcertApp.UI.Infrastructure;
using ConcertApp.UI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConcertApp.UI.ViewModels
{
    public class TicketsViewModel:BaseNotifyPropertyChanged
    {
      
        public ObservableCollection<ConcertDTO> Concerts { get; set; }
       
        ConcertService concertService;


        UserDTO user;
        public UserDTO CurrentUser 
        {
            get => user; 
            set
            {
                user = value;
                UploadConcerts();
                NotifyPropertyChanged();
            }
        }

        CreditCardService CreditCardService;

        public TicketsViewModel(ConcertService service, CreditCardService cs)
        {
            Concerts = new ObservableCollection<ConcertDTO>();
            concertService = service;
            CreditCardService = cs;
            GoBackCommand = new RelayCommand((param) =>
            {
                ProfileView view = new ProfileView();
                (view.DataContext as ProfileViewModel).CurrentUser = CurrentUser;
                (view.DataContext as ProfileViewModel).CreditCard = CreditCardService.Get((int)CurrentUser.CardId);
                Switcher.Switch(view);
            });
        }

        private void UploadConcerts()
        {
            foreach(var item in CurrentUser.Tickets)
            {
                Concerts.Add(concertService.Get((int)item.ConcertId));
            }
        }

        public ICommand GoBackCommand { get; private set; }
    }
}
