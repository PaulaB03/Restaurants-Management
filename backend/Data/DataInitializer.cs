using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Data
{
    public class DataInitializer
    {
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("User").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "User";
                roleManager.CreateAsync(role).Wait();
            }

            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                roleManager.CreateAsync(role).Wait();
            }

            if (!roleManager.RoleExistsAsync("Owner").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Owner";
                roleManager.CreateAsync(role).Wait();
            }
        }

        public static void SeedCategories(DataContext context)
        {
            // List of categories to add
            var categories = new List<Category>
            {
                new Category { Name = "Feluri principale" },
                new Category { Name = "Garnituri" },
                new Category { Name = "Supe" },
                new Category { Name = "Pizza" },
                new Category { Name = "Cafea"},
                new Category { Name = "Sucuri"},
                new Category { Name = "Bauturi Alcoolice"}
            };

            foreach (var category in categories)
            {
                // Check if the category already exists to avoid duplicates
                if (!context.Categories.Any(c => c.Name == category.Name))
                {
                    context.Categories.Add(category);
                }
            }

            // Save changes if any categories were added
            context.SaveChanges();
        }
    }
}
