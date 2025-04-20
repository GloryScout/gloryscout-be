using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace GloryScout.Data.SeedData
{
    public static class SeedData
    {public static void Seed(AppDbContext context)
		{
			//try
			//{
			Console.WriteLine("Seeding started...");
			//using var syntax ensures that once the operations are complete, the context is disposed off properly (i.e., resources are freed).

			context.Database.EnsureCreated();
			context.Database.Migrate();


			//Reading JSON Files for Seeding Data

			//string fileNamePlayers = @"D:\GloryScout\GloryScout.Data\SeedData\Players.json";
			//var JsonStringPlayers = File.ReadAllText(fileNamePlayers);
			////The JSON string is then converted (deserialized) into a list of Player objects
			//var Players = JsonSerializer.Deserialize<List<Player>>(JsonStringPlayers);


			//string fileNamePosts = @"D:\GloryScout\GloryScout.Data\SeedData\/Posts.json";
			//var JsonStringPosts = File.ReadAllText(fileNamePosts);
			//var Posts = JsonSerializer.Deserialize<List<Post>>(JsonStringPosts);


			//string fileNameScouts = @"D:\GloryScout\GloryScout.Data\SeedData\Scouts.json";
			//var JsonStringScouts = File.ReadAllText(fileNameScouts);
			//var Scouts = JsonSerializer.Deserialize<List<Scout>>(JsonStringScouts);


			//string fileNameUsers = @"D:\GloryScout\GloryScout.Data\SeedData\Users.json";
			//var JsonStringUsers = File.ReadAllText(fileNameUsers);
			//var Users = JsonSerializer.Deserialize<List<User>>(JsonStringUsers);


			//// Add data only if the database is empty to prevent duplicates
			//// Add data only if the database is empty to prevent duplicates

			//try{
			//	if (!context.Users.Any())
			//	{
			//		Console.WriteLine("Seeding users...");
			//		context.Users.AddRange(Users!);
			//		context.SaveChanges();
			//	}

			//	if (!context.Scouts.Any())
			//	{
			//		Console.WriteLine("Seeding scouts...");
			//		context.Scouts.AddRange(Scouts!);
			//		context.SaveChanges();
			//	}

			//	if (!context.Players.Any())
			//	{
			//		Console.WriteLine("Seeding players...");
			//		context.Players.AddRange(Players!);
			//		context.SaveChanges();
			//	}

			//	if (!context.Posts.Any())
			//	{
			//		Console.WriteLine("Seeding posts...");
			//		context.Posts.AddRange(Posts!);
			//		context.SaveChanges();
			//	}

			//	Console.WriteLine("Seeding completed successfully.");
			//}
			//         catch (Exception ex)
			//         {
			//             Console.WriteLine($"Seeding failed: {ex.Message}");
			//             Console.WriteLine($"Stack trace: {ex.StackTrace}");

			//         }
			//}
			//	catch (Exception ex)
			//	{
			//		Console.WriteLine("Seeding failed: " + ex.Message);
			//	}

			
		}
	}
}
