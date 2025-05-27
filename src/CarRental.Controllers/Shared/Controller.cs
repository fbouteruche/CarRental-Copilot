using CarRental.Domain.Shared;
using System.Collections.Generic;

namespace CarRental.Controllers.Shared
{
    public abstract class Controller<T> where T : BaseEntity
    {
        public abstract string InsertNew(T record);
        public abstract string Edit(int id, T record);
        public abstract bool Exists(int id);
        public abstract bool Delete(int id);
        public abstract List<T> SelectAll();

        public abstract T? SelectById(int id);

        protected Dictionary<string, object> AddParameter(string field, object value)
        {
            return new Dictionary<string, object>() { { field, value } };
        }
    }
}
