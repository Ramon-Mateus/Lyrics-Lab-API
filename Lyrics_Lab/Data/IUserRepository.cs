﻿using Lyrics_Lab.Models;

namespace Lyrics_Lab.Data
{
    public interface IUserRepository
    {
        User Create(User user);
        User GetByEmail(string email);
        User GetById(int id);
    }
}
