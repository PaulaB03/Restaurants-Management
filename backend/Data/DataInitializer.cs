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

        public static void SeedUsers(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, DataContext context)
        {
            // Ensure roles are seeded
            SeedRoles(roleManager);

            var userData = new List<(string Email, string UserName, string Password, string FirstName, string LastName, string[] Roles)>
            {
                ("user@example.com", "user", "Password55!", "User", "", new[] { "User" }),
                ("admin@example.com", "admin", "Password55!", "Admin", "", new[] { "User", "Admin" }),
                ("owner@example.com", "owner", "Password55!", "Owner", "", new[] { "User", "Owner" })
            };

            foreach (var user in userData)
            {
                if (userManager.FindByNameAsync(user.UserName).Result == null)
                {
                    var newUser = new User
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        AddressId = null
                    };

                    var createUserResult = userManager.CreateAsync(newUser, user.Password).Result;
                    if (createUserResult.Succeeded)
                    {
                        foreach (var role in user.Roles)
                        {
                            var addToRoleResult = userManager.AddToRoleAsync(newUser, role).Result;
                        }
                    }
                }
            }

            SeedRestaurantWithProducts(context);
        }

        public static void SeedRestaurantWithProducts(DataContext context)
        {
            // Ensure that categories are seeded first
            SeedCategories(context);

            // Check if the restaurant already exists 
            if (!context.Restaurants.Any(r => r.Name == "Yummy"))
            {
                var categoryId = context.Categories.First().Id; 
                var ownerId = context.Users.FirstOrDefault(u => u.Email == "owner@example.com")?.Id;
                if (ownerId == null)
                {
                    Console.WriteLine("Owner user not found. Ensure that the 'owner' user is seeded.");
                    return;
                }

                var address = new Address { City = "Bucuresti", Number = "34", Street = "Eroilor"};
                context.Addresses.Add(address);
                context.SaveChanges();

                var restaurant = new Restaurant
                {
                    Name = "Yummy",
                    Description = "A sample restaurant description",
                    OwnerId = ownerId, 
                    ImageUrl = "data:image/jpeg;base64,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",
                    Address = address, 
                    Products = new List<Product>
                    {
                        new Product { Name = "Snitel", Price = 10.00f, CategoryId = categoryId },
                        new Product { Name = "Salata", Price = 15.00f, CategoryId = categoryId },
                        new Product { Name = "Paste", Price = 20.00f, CategoryId = categoryId }
                    }
                };

                context.Restaurants.Add(restaurant);
                context.SaveChanges();

                // Linking products to the restaurant
                foreach (var product in restaurant.Products)
                {
                    product.RestaurantId = restaurant.Id;
                }

                context.SaveChanges();
            }
        }
    }
}
