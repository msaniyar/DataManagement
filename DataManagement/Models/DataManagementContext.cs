using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataManagement.Models
{
    public partial class DataManagementContext : DbContext
    {
        public DataManagementContext()
        {
        }

        public DataManagementContext(DbContextOptions<DataManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DataTable> DataTable { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<DataTable>(entity =>
            {
                entity.HasKey(e => e.UserName)
                    .HasName("PK__DataTabl__C9F284570C406E1B");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Tree).IsRequired();
            });
        }
    }
}
