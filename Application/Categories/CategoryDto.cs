

using System;
using System.Collections.Generic;

namespace Application.Categories
{
    public class CategoryDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<SubCategoriesDto> SubCategories { get; set; }
    }
}
