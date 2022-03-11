using System.Collections.Generic;
using System.Threading.Tasks;
using UserPortal.Entities;
using UserPortal.Interfaces;
using UserPortal.Models;
using System;
using System.Linq;
using UserPortal.Enums;
using System.Text.Json;
using UserPortal.Helper;

namespace UserPortalTest
{
    public class UserServiceFake : IUserService
    {
        public Task<string> Authenticate(AuthenticateRequest model)
        {
            var user = DummyData.userList.Where(p => (p.Username == model.Username || p.Email == model.Username)).FirstOrDefault();
            if (user == null)
                throw new Exception("Username or Password not found");
            if (!user.Active)
                throw new Exception("User is not active");

            var token = Utils.GenerateFakeJwtToken(user);
            return Task.FromResult(token);
        }

        public Task<User> GetById(long userId)
        {
            var user = DummyData.userList.Where(p => p.Id == userId).FirstOrDefault();
            return Task.FromResult(user);
        }

        public Task<User> Registration(RegistrationRequest model)
        {
            var user = model.ReverseMapToUser();
            DummyData.userList.Add(user);
            return Task.FromResult(user);
        }

        public Task<User> UpdateCurrentUserDataAsync(UserUpdateRequest model)
        {
            var user = DummyData.userList.Where(p => p.Id == model.Id).FirstOrDefault();
            var newUser = model.ReverseMapToUser(user);
            user = newUser;
            return Task.FromResult(user);
        }
    }
}