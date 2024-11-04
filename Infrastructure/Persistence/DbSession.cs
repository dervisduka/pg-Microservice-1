using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Persistence
{
    /// <summary>
    /// Bejme kete ne menyre qe te kemi nje sesion, qe hap connectionin heren e pare kur i duhet 
    /// dhe e perdor dhe mban te hapur ne te gjithe jeten e unitOfWork. Pra nqs kemi disa objekte do te
    /// jene transaksionale.
    /// sepse Transaksioni gjithashtu hapet heren e pare kur hapet connectioni, dhe riperdoret
    /// ky session do perdoret nga repositoret per te marre connectionin dhe transactionin
    /// </summary>
    public sealed class DbSession : IDisposable
    {
        private readonly IConfiguration _configuration;

        private IDbConnection _Connection;
        public IDbConnection Connection
        {
            get
            {
                if (_Connection == null)
                {
                    _Connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                    _Connection.Open();
                    Transaction = _Connection.BeginTransaction(IsolationLevel.ReadCommitted);
                }
                return _Connection;
            }

        }
        public IDbTransaction Transaction { get; set; }

        public DbSession(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Dispose()
        {
            _Connection?.Dispose();
            Transaction?.Dispose();
        }

        /// <summary>
        /// Support start new transaction after commit changes to the DB
        /// </summary>
        public void StartNewTransaction()
        {
            _Connection?.Close();
            _Connection?.Dispose();
            _Connection = null;
        }
    }
}