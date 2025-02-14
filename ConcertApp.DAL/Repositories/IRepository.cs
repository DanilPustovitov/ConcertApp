﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcertApp.DAL.Context;

namespace ConcertApp.DAL.Repositories
{
    public interface IRepository<T> where T:class
    {
        T Get(int id);
        IEnumerable<T> GetAll();
        void CreateOrUpdate(T entity);
        void Delete(T entity);
        void SaveChanges();
    }
}
