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

        public virtual DbSet<TreeListTable> TreeListTable { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<TreeListTable>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Tree).IsRequired();
            });
        }
    }
}
