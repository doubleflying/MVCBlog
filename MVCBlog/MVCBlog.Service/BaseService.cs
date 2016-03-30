﻿using MVCBlog.Common;
using MVCBlog.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Service
{
    public abstract class BaseService<T> : IBase<T>
    {
        public event EventHandler<ModelCacheEventArgs> ModelCreateEventHandler;
        public event EventHandler<ModelCacheEventArgs> ModelDeleteEventHandler;
        public event EventHandler<ModelCacheEventArgs> ModelUpdateEventHandler;



        private void Service_ModelCreateEventHandler(T model)
        {
            if (ModelCreateEventHandler != null)
            {
                ModelCacheEventArgs e = GetEventArgs(model);
                ModelCreateEventHandler(model, e);
            }
        }
        private void Service_ModelDeleteEventHandler(T model)
        {
            if (ModelDeleteEventHandler != null)
            {
                ModelCacheEventArgs e = GetEventArgs(model);
                ModelDeleteEventHandler(model, e);
            }
        }
        private void Service_ModelUpdateEventHandler(T model)
        {
            if (ModelUpdateEventHandler != null)
            {
                ModelCacheEventArgs e = GetEventArgs(model);
                ModelUpdateEventHandler(model, e);
            }
        }
        public virtual void Delete(T model)
        {
            Service_ModelDeleteEventHandler(model);
        }

        public virtual Task DeleteAsync(T model)
        {
            return Task.Factory.StartNew(() =>
            {
                Service_ModelDeleteEventHandler(model);
            });
        }

        public abstract T GetById(int id);

        public abstract Task<T> GetByIdAsync(int id);

        public abstract T GetFromDB(int id);

        public virtual void Insert(T model, int userid = 0)
        {
            Service_ModelCreateEventHandler(model);
        }

        public virtual Task InsertAsync(T model, int userid = 0)
        {
            return Task.Factory.StartNew(() =>
            {
                Service_ModelCreateEventHandler(model);
            });
        }

        public abstract Task<int> SaveChanges();

        public virtual void Update(T model)
        {
            Service_ModelUpdateEventHandler(model);
        }

        public virtual Task UpdateAsync(T model)
        {
            return Task.Factory.StartNew(() =>
            {
                Service_ModelUpdateEventHandler(model);
            });
        }

        public abstract string GetModelKey(T model);

        public virtual ModelCacheEventArgs GetEventArgs(T model)
        {
            var prop = model.GetType().GetProperty("id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            int id = prop == null ? 0 : (int)prop.GetValue(model);
            ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = GetModelKey(model), ID = id };
            return e;
        }
    }
}