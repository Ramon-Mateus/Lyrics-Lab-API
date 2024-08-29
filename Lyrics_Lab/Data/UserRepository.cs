﻿using Lyrics_Lab.Contexts;
using Lyrics_Lab.Models;

namespace Lyrics_Lab.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public User Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            var album = new Album
            {
                Name = "Default",
                Description = "Default Album",
                UserId = user.Id
            };
            
            _context.Albums.Add(album);
            _context.SaveChanges();

            return user;
        }

        public User GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }
        
        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }
    }
}
