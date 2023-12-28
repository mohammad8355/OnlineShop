using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.ReturnMultipleData
{
    public class ReturnMultipleData<T> where T : class
    {
        public async Task<List<object>> Return(bool status)
        {
            List<object> list = new List<object>();
            list.Add(status);
            return list;
        }
        public async Task<List<object>> Return(bool status, string message,T model)
        {
            List<object> list = new List<object>();
            list.Add(status);
            list.Add(message);
            list.Add(model);
            return list;
        }
        public async Task<List<object>> Return(bool status, string message)
        {
            List<object> list = new List<object>();
            list.Add(status);
            list.Add(message);
            return list;
        }
        public async Task<List<object>> Return(bool status,string message,List<T> model)
        {
            List<object> list = new List<object>();
            list.Add(status);
            list.Add(message);
            list.Add(model);
            return list;
        }
    }
}
