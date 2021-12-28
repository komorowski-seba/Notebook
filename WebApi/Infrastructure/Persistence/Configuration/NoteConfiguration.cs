using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entity;

namespace WebApi.Infrastructure.Persistence.Configuration
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.HasKey(note => note.Id);
            builder
                .Property(note => note.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
        }
    }
}