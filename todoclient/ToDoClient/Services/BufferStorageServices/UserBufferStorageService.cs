using System.Collections.Generic;
using System.Linq;
using ORM;
using todoclient.Infrastructure;
using todoclient.Models;

namespace todoclient.Services.BufferStorageServices
{
    public class UserBufferStorageService
    {
        private ToDoContext db = new ToDoContext();

        public void CreateUser(UserModel user)
        {
            if (db.Users.FirstOrDefault(u => u.Id == user.Id) == null)
            {
                db.Users.Add(user.ToOrmUser());
            }
        }


        public IList<UserModel> GetAllUsers()
        {
            var users = db.Users.ToList();
            return users.Select(u => u.ToModelUser()).ToList();
        }
    }
}