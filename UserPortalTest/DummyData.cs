using System;
using System.Collections.Generic;
using UserPortal.Entities;

namespace UserPortalTest
{
    public class DummyData
    {
        public static List<User> userList = new List<User> {
            new User {  Id = 1,  Active = true,  Email = "haktor@gmail.com",  FirstName = "haktor",    LastName = "cupet", Username = "haktor" , Password="1",CreateDate = DateTime.Now},
            new User {  Id = 2,  Active = true,  Email = "eda@gmail.com",  FirstName = "eda",    LastName = "cupet", Username = "eda", Password="1",CreateDate = DateTime.Now.AddDays(1)},
            new User {  Id = 3,  Active = true,  Email = "kagan@gmail.com",  FirstName = "kagan",    LastName = "cupet", Username = "kagan", Password="1",CreateDate = DateTime.Now.AddHours(1)}
        };

    }
}
