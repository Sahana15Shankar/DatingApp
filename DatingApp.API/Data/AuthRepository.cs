using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Mvc;




namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {

        private readonly DbContext _context;

        public AuthRepository(DbContext context)
        {
            _context = context;
        }
        public Task<User> Login(string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> Register(User user, string password)
        {
        
        byte[] passwordHash, passwordSalt;

        createPasswordHash(password, out passwordHash, out passwordSalt );
        user.PasswordHash = passwordHash ;
        user.PasswordSalt = passwordSalt ;

        await _context.Users.AddSync(user);
        await _context.SaveChangesAsync();

        return user;

               
        }

    private void createPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordHash = hmac.Key;
            passwordSalt = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    public Task<bool> UserExists(string username)
    {
        throw new System.NotImplementedException();
    }
}
}