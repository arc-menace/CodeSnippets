using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeSnippets.Data.Models
{
    public class DatabaseEntity
    {
        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CreatorId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedDate { get; set; } = DateTime.UtcNow;

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LastUpdatorId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastAccessedDate { get; set; } = DateTime.UtcNow;

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LastAccessorId { get; set; }

        public DatabaseEntity() { }
        public DatabaseEntity(int creatorId)
        {
            CreatorId = creatorId;
            LastUpdatorId = creatorId;
            LastAccessorId = creatorId;
        }
    }
}
