using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryServiceDomain
{
    public interface IDatabaseOperations
    {
        T QuerySingleOrDefault<T>(string procedure, object parameters = null, CommandType commandType = CommandType.StoredProcedure);
        int Execute(string procedure, object parameters = null, CommandType commandType = CommandType.StoredProcedure);
        IEnumerable<T> Query<T>(string procedure, object parameters = null, CommandType commandType = CommandType.StoredProcedure);
        IEnumerable<T> Query<T>(string procedure, CommandType commandType = CommandType.StoredProcedure);
    }
}
