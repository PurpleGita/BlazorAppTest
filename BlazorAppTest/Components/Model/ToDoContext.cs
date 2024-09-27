using Microsoft.EntityFrameworkCore;

namespace BlazorAppTest.Components.Model
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
        {
        }

        public DbSet<Todolist> Todolists { get; set; }
        public DbSet<Cpr> Cprs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Todolist entity
            modelBuilder.Entity<Todolist>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.task)
                      .IsRequired()
                      .HasMaxLength(200);
            });

            // Configure Cpr entity
            modelBuilder.Entity<Cpr>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.CprNumber)
                      .IsRequired();
                entity.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.HasMany(e => e.Todolist)
                      .WithOne()
                      .HasForeignKey("CprId");
            });
        }

        public async Task<Cpr?> GetCPRObjectByCPRNumberAndEmail(int cprNumber, string email)
        {
            if (Cprs == null)
            {
                throw new InvalidOperationException("Cprs collection is not initialized.");
            }

            Database.OpenConnection();

            Console.WriteLine("cprNumber:   " + cprNumber);
            Console.WriteLine("Email:   " + email);

            return await Cprs
                .Include(c => c.Todolist) // Include the Todolist collection
                .Where(c => c.CprNumber == cprNumber && c.Email.ToLower() == email.ToLower())
                .FirstOrDefaultAsync();
        }

        public async Task<Cpr?> AddToDoListToCpr(Cpr cprObject,Todolist toDoItem) 
        {
            if (cprObject == null || toDoItem == null)
            {
                throw new ArgumentNullException("Cpr object or ToDo item cannot be null.");
            }
            // Ensure the Todolist property is initialized
            if (cprObject.Todolist == null)
            {
                cprObject.Todolist = new List<Todolist>();
            }

            // Add the new ToDo item to the Cpr's Todolist
            cprObject.Todolist.Add(toDoItem);

            // Update the Cpr object in the database
            Cprs.Update(cprObject);

            // Save changes to the database
            await SaveChangesAsync();

            return cprObject;

        }
    }
}
