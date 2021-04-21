﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcertApp.BLL.DTO
{
    public class CreditCardDTO
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public string Cvv2 { get; set; }

        public DateTime ExpireDate { get; set; }

        public string Pin { get; set; }
    }
}
