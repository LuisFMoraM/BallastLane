using DataAccess.Entities;
using LinqToDB;
using LinqToDB.Data;

namespace DataAccess
{
    /// <summary>
    /// Manages database resources to expose in the application
    /// </summary>
    public class AppDataConnection : DataConnection
    {
        public ITable<Medication> Medications => this.GetTable<Medication>();

        public ITable<User> Users => this.GetTable<User>();

        public AppDataConnection(DataOptions<AppDataConnection> options)
            : base(options.Options)
        {
        }
    }
}
