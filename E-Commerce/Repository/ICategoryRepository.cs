﻿using E_Commerce.Models;

namespace E_Commerce.Repository
{
    public interface ICategoryRepository : IRepository<Category>
    {
         Task<bool> IsCategoryExistAsync(string name);
    }
}
