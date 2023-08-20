using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryServiceDomain.DatabaseOperations
{
    public class DatabaseOperations : IDatabaseOperations
    {
        private readonly IDbConnection _connection;

        public DatabaseOperations(IDbConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public T QuerySingleOrDefault<T>(string procedure, object parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            return _connection.QuerySingleOrDefault<T>(procedure, parameters, commandType: commandType);
        }

        public int Execute(string procedure, object parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            return _connection.Execute(procedure, parameters, commandType: commandType);
        }

        public IEnumerable<T> Query<T>(string procedure, object parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            return _connection.Query<T>(procedure, parameters, commandType: commandType);
        }

        public IEnumerable<T> Query<T>(string procedure, CommandType commandType = CommandType.StoredProcedure)
        {
            return _connection.Query<T>(procedure, commandType: commandType);
        }
    }
}
