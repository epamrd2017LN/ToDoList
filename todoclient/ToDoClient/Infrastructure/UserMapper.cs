using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ORM;
using todoclient.Models;

namespace todoclient.Infrastructure
{
    public static class UserMapper
    {
        public static UserModel ToModelUser(this User user)
        {
            if (user == null)
            {
                return null;
            }
            UserModel userModel = new UserModel();
            userModel.Id = user.Id;
            return userModel;
        }

        public static User ToOrmUser(this UserModel userModel)
        {
            if (userModel == null)
            {
                return null;
            }
            User user = new User();
            user.Id = userModel.Id;
            return user;
        }
    }
}