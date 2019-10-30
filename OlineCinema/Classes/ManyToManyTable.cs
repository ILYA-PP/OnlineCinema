using System;
using System.Collections.Generic;

namespace OlineCinema
{
    abstract class ManyToManyTable : IAddable
    {
        public List<int> DataID = new List<int>();
        public List<int> ContentID = new List<int>();
        public virtual void AddData(string name, object value)
        {
            throw new NotImplementedException();
        }
    }
}
