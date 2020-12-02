using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeSnippets.Data.Models
{
    public class DatabaseEntity
    { 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CreatorId { get; set; }

        public DateTime LastUpdatedDate { get; set; } = DateTime.UtcNow;

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LastUpdatorId { get; set; }

        public DateTime LastAccessedDate { get; set; } = DateTime.UtcNow;

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LastAccessorId { get; set; }

        public DatabaseEntity() { }
        
        public void SetCreatorId(int creatorId)
        {
            CreatorId = creatorId;
            LastUpdatorId = creatorId;
            LastAccessorId = creatorId;
        }
    }
}
