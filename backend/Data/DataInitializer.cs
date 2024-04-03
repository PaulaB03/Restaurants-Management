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
