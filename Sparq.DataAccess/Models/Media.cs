using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public class Media
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public virtual User? Owner { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string StorageKey { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
    }
}
