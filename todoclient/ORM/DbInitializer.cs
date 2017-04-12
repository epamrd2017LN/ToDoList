using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<ToDoContext>
    {
    }
}
