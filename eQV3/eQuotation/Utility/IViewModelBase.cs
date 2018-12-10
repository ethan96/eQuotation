using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eQuotation.Utility
{
    interface IViewModelBase<T>
    {
        string ID { get; set; }

        LogEventManager LogManager { get; set; }

        T Entity { get; set; }

        string AppName { get; }

        void SetValue();

        void GetValue(T TEntity);

        void GetValueByID(string id);
    }
}
