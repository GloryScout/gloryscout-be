using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GloryScout.API.Services
{
	public class CloudinaryService
	{
		private readonly Cloudinary _cloudinary;

		public CloudinaryService(IConfiguration configuration)
		{
			var cloudName = configuration["Cloudinary:CloudName"];
			var apiKey = configuration["Cloudinary:ApiKey"];
			var apiSecret = configuration["Cloudinary:ApiSecret"];

			var account = new Account(cloudName, apiKey, apiSecret);
			_cloudinary = new Cloudinary(account);
		}

		public async Task<string> UploadProfilePhotoAsync(IFormFile file, string userId)
		{
			if (file == null || file.Length == 0)
			{
				return null;
			}

			using (var stream = file.OpenReadStream())
			{
				var uploadParams = new ImageUploadParams
				{
					File = new FileDescription(file.FileName, stream),
					Folder = $"GLORYSCOUT/User/pfp/{userId}",
					PublicId = "photo",
					Transformation = new Transformation().Width(500).Height(500).Crop("fill")
				};

				var uploadResult = await _cloudinary.UploadAsync(uploadParams);

				if (uploadResult.Error != null)
				{
					return null;
				}

				return uploadResult.SecureUrl.ToString();
			}
		}

		public async Task<string> UploadPostAsync(IFormFile file, string userId, string postId)
		{
			if (file == null || file.Length == 0)
			{
				return null;
			}

			using (var stream = file.OpenReadStream())
			{
				var uploadParams = new RawUploadParams
				{
					File = new FileDescription(file.FileName, stream),
					Folder = $"GLORYSCOUT/User/Post/{userId}/{postId}",
					PublicId = file.FileName
				};

				var uploadResult = await _cloudinary.UploadAsync(uploadParams);

				if (uploadResult.Error != null)
				{
					return null;
				}

				return uploadResult.SecureUrl.ToString();
			}
		}

		public Task<string> GetPhotoURL(string publicId)
		{
			var url = _cloudinary.Api.UrlImgUp.BuildUrl(publicId);
			return Task.FromResult(url);
		}

		public Task<string> DownloadProfilePhotoAsync(string publicId)
		{
			var url = _cloudinary.Api.UrlImgUp.Action("attachment").BuildUrl(publicId);
			return Task.FromResult(url);
		}

		public async Task<bool> DeletePhotoAsync(string publicId)
		{
			var deletionParams = new DeletionParams(publicId);
			var deletionResult = await _cloudinary.DestroyAsync(deletionParams);
			return deletionResult.Result == "ok";
		}
	}
}
