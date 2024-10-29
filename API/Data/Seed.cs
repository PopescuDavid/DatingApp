using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    public static async Task SeedUsersAsync(DataContext context)
    {
        if(await context.Users.AnyAsync()){
            return;
        }

        var userData = await File.ReadAllTextAsync("Data/UserSeedData.json"); // If you want to add more users to the app, just add them via https://json-generator.com/#

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

        if(users == null){
            return;
        }

        foreach (var user in users)
        {
            using var hmac = new HMACSHA512();

            user.Username = user.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
            user.PasswordSalt = hmac.Key;

            context.Users.Add(user);
        }

        await context.SaveChangesAsync();
    }
}
