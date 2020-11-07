using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippets.Data.Entities
{
    public class DatabaseEntity
    {
        public virtual int Id { get; protected set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime LastUpdatedDate { get; set; }
        public virtual int CreatorId { get; set; }
        public virtual int LastUpdatorId { get; set; }
    }
}
