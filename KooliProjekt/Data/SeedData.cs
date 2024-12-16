using KooliProjekt.Controllers;

namespace KooliProjekt.Data
{
    public static class SeedData
    {

        public static void Generate(ApplicationDbContext context)
        {
            if (context.Doctors.Any())
            {
                return;
            }

            // Create a new Doctor object and set its properties
            var list = new Doctor
            {
                Specialization = "List 1",
                Name = "Item 1.1",
            };

            // Add the doctor object to the Doctors DbSet
            context.Doctors.Add(list);

            // Save changes to the database
            context.SaveChanges();
        }
    }
}