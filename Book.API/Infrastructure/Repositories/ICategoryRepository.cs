using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.API.Domain;

namespace Book.API.Infrastructure.Repositories
{
    public interface ICategoryRepository: IRepository<Category>
    {
        
    }
}