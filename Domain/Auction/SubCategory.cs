using System;
using System.Collections.Generic;

namespace Domain
{
    public class SubCategory: AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<Item> Items { get; set; } = new HashSet<Item>();
    }
}