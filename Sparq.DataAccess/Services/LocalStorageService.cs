using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Sparq.DataAccess.Services
{
    public class LocalStorageService : IStorageService
    {
        private readonly string _basePath;

        public LocalStorageService()
        {
            _basePath = Path.Combine(Directory.GetCurrentDirectory(), "storage");

            if (!Directory.Exists(_basePath))
                Directory.CreateDirectory(_basePath);
        }

        public async Task<string> UploadAsync(Stream stream, string fileName, string contentType)
        {
            var key = $"{Guid.NewGuid()}_{fileName}";
            var path = Path.Combine(_basePath, key);

            using var fileStream = new FileStream(path, FileMode.Create);
            await stream.CopyToAsync(fileStream);

            return key;
        }

        public Task<Stream> DownloadAsync(string storageKey)
        {
            var path = Path.Combine(_basePath, storageKey);

            if (!File.Exists(path))
                throw new FileNotFoundException();

            Stream stream = File.OpenRead(path);
            return Task.FromResult(stream);
        }

        public Task DeleteAsync(string storageKey)
        {
            var path = Path.Combine(_basePath, storageKey);

            if (File.Exists(path))
                File.Delete(path);

            return Task.CompletedTask;
        }
    }
}
