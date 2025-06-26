using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.API.Dto;
using MediatR;

namespace Book.API.Application.Categories.Commands
{
    public class CreateCategoryCommand: IRequest<CategoryDto>
    {
        public CategoryDto Category { get; set; }

        public CreateCategoryCommand(CategoryDto category)
        {
            Category = category;
        }
    }
}