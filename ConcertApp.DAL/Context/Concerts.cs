﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcertApp.DAL.Context
{
    public class Concerts
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public string Organizer { get; set; }
        public string Photo { get; set; }
        public string Actors { get; set; }
        public string Address { get; set; }

        public DateTime StartTime { get; set; }
    }
}
