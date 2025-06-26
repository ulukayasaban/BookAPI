using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.API.Dto;
using MediatR;

namespace Book.API.Application.Categories.Commands
{
    public class UpdateCategoryCommand : IRequest<bool>
    {
        public CategoryDto Category { get; }

        public UpdateCategoryCommand(CategoryDto category)
        {
            Category = category;
        }
    }
}