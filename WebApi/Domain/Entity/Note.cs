using System;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Domain.Common;

namespace WebApi.Domain.Entity
{
    public class Note : AuditableEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public Guid Id { get; init; }
        public string Topic { get; private set; }
        public string Description { get; private set; }

        public Note(string topic, string description)
        {
            if (string.IsNullOrEmpty(topic))
                throw new ArgumentException("Topic is empty or NULL");
            if (string.IsNullOrEmpty(description))
                throw new ArgumentException("Description is empty or NULL");
            
            Topic = topic;
            Description = description;
        }
    }
}