using System.Data.Entity;

namespace ComicsReadProgress
    {
    public class Context: DbContext
    {
        private static readonly string ConnectionString =
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=d:\\data.mdf;Integrated Security=True;Connect Timeout=30";

        public Context() : base(ConnectionString) { }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Check>().Ignore(c => c.CashierPerson);
        //    modelBuilder.Entity<Check>().Ignore(c => c.Customer);
        //    modelBuilder.Entity<Check>().Ignore(c => c.Discount);
        //    modelBuilder.Entity<Check>().Ignore(c => c.DiscountCard);
        //}

        public DbSet<Issue> Issues { get; set; }
    }
}
