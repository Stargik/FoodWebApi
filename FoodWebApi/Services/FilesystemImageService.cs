using System;
using Azure.Core;
using FoodMVCWebApp.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FoodMVCWebApp.Services
{
	public class FilesystemImageService : IImageService
	{
        private readonly StaticFilesSettings imgSettings;
        private readonly IWebHostEnvironment appEnvironment;

        public FilesystemImageService(IOptions<StaticFilesSettings> imgSettings, IWebHostEnvironment appEnvironment)
		{
            this.imgSettings = imgSettings.Value;
            this.appEnvironment = appEnvironment;
		}

        public async Task Upload(IFormFile imgFile)
        {
            string directoryPath = appEnvironment.WebRootPath + "/" + imgSettings.Path;
            string fullPath = directoryPath + "/" + imgFile.FileName;
            string[] fileEntries = Directory.GetFiles(directoryPath);

            if (fileEntries.Contains(fullPath))
            {
                throw new Exception("File is already exist.");
            }
            

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await imgFile.CopyToAsync(stream);
            }
        }
        public async Task<string> GetStoragePath()
        {
            string directoryPath = appEnvironment.WebRootPath + "/" + imgSettings.Path;
            return directoryPath;
        }

        public async Task<IFormFile> Download(string imgName)
        {
            string directoryPath = appEnvironment.WebRootPath + "/" + imgSettings.Path;
            string fullPath = directoryPath + "/" + imgName;
            using (var stream = new FileStream(fullPath, FileMode.Open))
            {
                return new FormFile(stream, 0, stream.Length, imgName, imgName);
            }
        }
    }
}

