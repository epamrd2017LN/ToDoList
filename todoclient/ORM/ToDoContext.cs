using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using ORM;

namespace ORM
{
    public partial class ToDoContext : DbContext
    {
        private const string dbConnectionName = "dbConnection";

        static ToDoContext()
        {
            Database.SetInitializer(new DbInitializer());
        }

        public ToDoContext()
            : base(dbConnectionName)
        {
        }

        public DbSet<ToDo> ToDos { get; set; }

        public DbSet<User> Users { get; set; }
    }
}