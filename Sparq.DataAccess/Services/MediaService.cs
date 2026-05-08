using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    public class MediaService : IMediaService
    {
        private readonly SparqDbContext _context;
        private readonly IStorageService _storage;

        public MediaService(SparqDbContext context, IStorageService storage)
        {
            _context = context;
            _storage = storage;
        }

        public async Task<Media> UploadAsync(IFormFile file, string userId)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Empty file");

            using var stream = file.OpenReadStream();

            var storageKey = await _storage.UploadAsync(
                stream,
                file.FileName,
                file.ContentType
            );

            var media = new Media
            {
                OwnerId = userId,
                FileName = file.FileName,
                StorageKey = storageKey,
                ContentType = file.ContentType,
                CreatedAt = DateTime.UtcNow
            };

            _context.Media.Add(media);
            await _context.SaveChangesAsync();

            return media;
        }

        public async Task<(Media Media, Stream Stream)> GetFileAsync(string id, string userId)
        {
            var media = await _context.Media
                .FirstOrDefaultAsync(m => m.Id == id && m.DeletedAt == null);

            if (media == null)
                throw new Exception("Not found");

            if (media.OwnerId != userId)
                throw new UnauthorizedAccessException();

            return (media, await _storage.DownloadAsync(media.StorageKey));
        }

        public async Task<bool> DeleteAsync(string id, string userId)
        {
            var media = await _context.Media
                .FirstOrDefaultAsync(m => m.Id == id && m.DeletedAt == null);

            if (media == null)
                return false;

            if (media.OwnerId != userId)
                return false;

            await _storage.DeleteAsync(media.StorageKey);

            media.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }
    }

}
