using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.API.Data;
using Book.API.Models;

namespace Book.API.Repositories
{
    public class CategoryRepository: Repository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}