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
        public async Task<User> Login(string username, string password)
        {

            var user= _context.Users.FirstOrDefault(x=>x.Username==username);

            if(user==null)
                return null;

            if(!VerifyPasswordHash(password,user.PasswordHash,user.PasswordSalt) )
                return null;

           return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {

        using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
        {
            
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for(int i=0;i<passwordSalt.Length;i++)
            {
                if(computedHash[i]!=passwordHash[i])
                return false;
            }

            return true;
        }
            
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

    public async Task<bool> UserExists(string username)
    {
        if(await _context.Users.AnyAsync(x=>x.Username==username))
        {
            return true;
        }

        return false;
    }
}
}