﻿using ConcertApp.BLL.DTO;
using ConcertApp.BLL.Services;
using ConcertApp.UI.Infrastructure;
using ConcertApp.UI.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ConcertApp.UI.ViewModels
{
    public class CreateTicketViewModel:BaseNotifyPropertyChanged
    {
       
        private ConcertDTO selected;
        public ConcertDTO SelectedConcert
        {
            get => selected;
            set
            {
                selected = value;
                NotifyPropertyChanged();
            }
        }
        TicketService service;

        private string type;
        public string TypeOfTicket
        {
            get => type;
            set
            {
                type = value;
                NotifyPropertyChanged();
            }
        }

        private int row;
        public int Row
        {
            get => row;
            set
            {
                row = value;
                NotifyPropertyChanged();
            }
        }

        private string count;
        public string Count
        {
            get => count;
            set
            {
                count = value;
                NotifyPropertyChanged();
            }
        }

        private int place;
        public int Place
        {
            get => place;
            set
            {
                place = value;
                NotifyPropertyChanged();
            }
        }

        private UserDTO selectedUser;

        public UserDTO SelectedUser
        {
            get => selectedUser;
            set
            {
                selectedUser = value;
                NotifyPropertyChanged();
            }
        }

        public CreateTicketViewModel(TicketService s)
        {
            service = s;
            Count = "";
            Task t = new Task(InitCommand);
            t.Start();

            GoBackCommand = new RelayCommand((param) =>
            {
                if (SelectedUser == null)
                    Switcher.Switch(new ListConcertsView());
                else
                {
                    ListConcertsView page = new ListConcertsView();
                    ListConcertsViewModel lvm = page.DataContext as ListConcertsViewModel;
                    lvm.SelectedUser = SelectedUser;
                    Switcher.Switch(page);
                }
            });
        }

        private void InitCommand()
        {

            ConfirmCommand = new RelayCommand(x =>
            {
                if (SelectedUser != null)
                {
                    if (SelectedUser.CardId == null)
                    {
                        MessageBox.Show("You didn't added credit card in your profile");
                        AddCreditCardView page = new AddCreditCardView();
                        AddCreditCardViewModel vm = page.DataContext as AddCreditCardViewModel;
                        vm.CurrentUser = SelectedUser;
                        Switcher.Switch(page);
                    }
                    else
                    {
                        try
                        {
                            if (Row == 0 || Place == 0 || TypeOfTicket == "")
                            {
                                MessageBox.Show("Wrong parameters!");
                            }
                            else
                            {
                                int place_ = Place;
                                List<TicketDTO> list = new List<TicketDTO>();
                                for (int i = 0; i < int.Parse(Count); i++)
                                {
                                    TicketDTO tmp = new TicketDTO();
                                    tmp.ConcertId = SelectedConcert.Id;
                                    tmp.UserId = SelectedUser.Id;
                                    tmp.Row = Row;
                                    
                                    tmp.Place = place_++;
                                    tmp.Type = this.TypeOfTicket;
                                    if (tmp.Type == "VIP")
                                    {
                                        tmp.Price = 1000;
                                    }
                                    else
                                        tmp.Price = 600;

                                    service.CreateOrUpdate(tmp);
                                    list.Add(tmp);
                                    ChangeSelectedUserInTopBar();
                                    foreach (var item in list)
                                    {
                                        SelectedUser.Tickets.Add(item);
                                    }
                                }

                                MailAddress from = new MailAddress("concertapp.notificationmail@gmail.com");

                                MailAddress to = new MailAddress(SelectedUser.Email);
                                using (MailMessage m = new MailMessage(from, to))
                                {
                                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                                    {
                                        for (int i = 0; i < int.Parse(Count); i++)
                                        {
                                            string path = $"{SelectedUser.Email}ticket{i + 1}.txt";
                                            File.WriteAllText(path, "");
                                            List<string> info = new List<string>();
                                            info.Add($"Event: {SelectedConcert.Title}");
                                            info.Add($"City: {SelectedConcert.City}");
                                            info.Add($"Adress: {SelectedConcert.Address}");
                                            info.Add($"Row: {list[i].Row}");
                                            info.Add($"Place: {list[i].Place}\n");
                                            info.Add($"Price: {list[i].Price}");
                                            Random rand = new Random();
                                            int code = rand.Next(100000, 999999);
                                            info.Add($"Special code: {code.ToString()}");
                                            File.AppendAllLines(path, info);
                                            m.Attachments.Add(new Attachment(path));
                                        }

                                        smtp.Credentials = new NetworkCredential("concertapp.notificationmail@gmail.com", "notificationmail");
                                        smtp.EnableSsl = true;
                                        try
                                        {
                                            smtp.Send(m);
                                        }
                                        catch { }
                                    }
                                }

                                MessageBox.Show("Successfully! Check your email for ticket doc.");
                            }
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Wrong count!");
                        }
                    }
                }
                else Switcher.Switch(new LogInAppView());

            });
        }

        private void ChangeSelectedUserInTopBar()
        {
            TopBarAfterLogInView view = new TopBarAfterLogInView();
            (view.DataContext as TopBarAfterLogInViewModel).CurrentUser = SelectedUser;
            (Switcher.ContentArea as MainViewModel).CurrentTopPage = view;
        }

        public ICommand ConfirmCommand { get; private set; }
        public ICommand GoBackCommand { get; private set; }
    }
}
