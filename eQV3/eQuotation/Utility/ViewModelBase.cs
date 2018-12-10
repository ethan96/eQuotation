
using eQuotation.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eQuotation.Utility
{
    public abstract class ViewModelBase<T> : IViewModelBase<T> 
        where T : class, new()  
    {
        public string ID { get; set; }

        public event DataSetHandler OnAddUpdate;

        public IUnitOfWork UnitWork { get; set; }

        public LogEventManager LogManager { get; set; }

        protected bool IsNew { get; set; }

        public T Entity { get; set; }

        public string AppName { get; private set; }

        public ViewModelBase() 
        {
            this.Entity = new T();
            this.OnAddUpdate = new DataSetHandler(AddOrUpdateEventHandler);
            this.IsNew = true;

            //initiate unitofwork
            if (this.UnitWork == null) {
                this.UnitWork = AppContext.UnitOfWork;
            }

            //initiate logging manager
            this.LogManager = new LogEventManager();

            ////initiate plant
            this.AppName = AppContext.AppName;
        }

        //public ModelBase(IUnitOfWork unitOfWork) : this()
        //{
        //    this.UnitOfWork = unitOfWork;
        //    Init();
        //}

        /// <summary>
        /// This method is used mainly to initiate LOV of HTML Element
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// Assign the value of model into data (Entity)
        /// </summary>
        public abstract void SetValue();

        /// <summary>
        /// Assign the value of data into model
        /// </summary>
        /// <param name="data"></param>
        public abstract void GetValue(T data);

        public virtual void GetValueByID(string id)
        {
            if (!typeof(T).Equals(typeof(Object)))
            {
                var repo = new Repository<T>(UnitWork.DbContext);
                GetValue(repo.GetByID(id));
            }
        }

        /// <summary>
        /// This method insert new data or update existing data
        /// </summary>
        public virtual void AddOrUpdate() 
        { 
            OnAddUpdate(this, EventArgs.Empty);
            SetValue();

            var repo = new Repository<T>(UnitWork.DbContext);
            if (IsNew)
                repo.Insert(this.Entity);
            else
                repo.Update(this.Entity);
      
        }

        /// <summary>
        /// This method check whether entity is already in database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool HasEntity(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var repo = new Repository<T>(UnitWork.DbContext);
                var data = repo.GetByID(id);

                if (data != null)
                {
                    this.Entity = data;
                    this.IsNew = false;
                }
            }

            return !this.IsNew;
        }

        protected virtual void AddOrUpdateEventHandler(object sender, EventArgs e)
        {
            //implement code here for data validation
            //before insert into database
        }    
    }
    
    public delegate void DataSetHandler(object sender, EventArgs e);
}