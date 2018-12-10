using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
  public  interface IRepository<T> where T: class 
    {
      DbContext CurrentContext{get;set;}
      void Add();
      void Update();
      void Remove();
    }
}
