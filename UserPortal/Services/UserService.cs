using Microsoft.EntityFrameworkCore;
using UserPortal.Context;
using UserPortal.Entities;
using UserPortal.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using UserPortal.Helper;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UserPortal.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;
using UserPortal.Enums;

namespace UserPortal.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly Utils _utils;
        public UserService(AppDbContext context, Utils utils)
        {
            _context = context;
            _utils = utils;
        }
        public async Task<string> Authenticate(AuthenticateRequest model)
        {
            var user = await _context.Users.Where(p => (p.Username == model.Username || p.Email == model.Username)).FirstOrDefaultAsync();
            if (user == null)
                throw new Exception("Username or Password not found");
            if (!user.Active)
                throw new Exception("User is not active");

            var token = _utils.GenerateJwtToken(user);
            return token;
        }

        public async Task<User> Registration(RegistrationRequest model)
        {
            //check user or email exists
            var userList = await _context.Users.Where(p => (p.Username == model.Username || p.Email == model.Username)).ToListAsync();
            if (userList.Count > 0)
            {
                throw new Exception("Kullanıcı Adı veya Email Adresi Zaten Sistemde Mevcuttur!");
            }
            var user = model.ReverseMapToUser();
            _context.Users.Add(user);
            try
            {
                var result = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw new Exception(message);
            }

            var topicModel = new TopicMessageModel
            {
                Data = user,
                Type = (int)TopicMessageType.Register

            };
            KafkaService.SendToKafka(JsonSerializer.Serialize(topicModel));
            return user;
        }

        public async Task<User> GetById(long userId)
        {
            var user = await _context.Users.Where(w => w.Id == userId).FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> UpdateCurrentUserDataAsync(UserUpdateRequest model)
        {
            var existingUser = await _context.Users.Where(w => w.Id == model.Id).FirstOrDefaultAsync();
            if (existingUser == null)
            {
                throw new Exception("Kullanıcı Sistemde Mevcut Değildir!");
            }
            var user = model.ReverseMapToUser(existingUser);
            _context.Users.Update(user);

            try
            {
                var result = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw new Exception(message);
            }
            return user;
        }

    }
}
