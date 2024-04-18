using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luveck.Service.Security.Utils.Jwt.Interface
{
    public interface IUtils
    {
        void SaveDataInCache(string key, object data, string parameterTime);
        void RemoveDataInCache(string key);
        T GetDataInCache<T>(object key);
    }
}
