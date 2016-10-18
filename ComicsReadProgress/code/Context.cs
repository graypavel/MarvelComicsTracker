using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;

namespace ComicsReadProgress.code
    {
    public class Context: DbContext
    {
        private const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|data.mdf;Integrated Security=True;Connect Timeout=30";

        public Context() : base(ConnectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Issue>()
                .Property(i => i.WikiaAddress)
                .HasMaxLength(400)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Address") {IsUnique = true}));

        }

        public DbSet<Issue> Issues { get; set; }
    }
}
