namespace FoodMVCWebApp.Interfaces
{
    public interface IImageService
	{
		public Task Upload(IFormFile imgFile);
        public Task<IFormFile> Download(string imgName);
        public Task<string> GetStoragePath();
    }
}

