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

        public MediaService(SparqDbContext context)
        {
            _context = context;
        }

        public async Task<Media> UploadAsync(IFormFile file, string userId)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Empty file");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var media = new Media
            {
                OwnerId = userId,
                FileName = file.FileName,
                StorageKey = fileName,
                ContentType = file.ContentType,
                CreatedAt = DateTime.UtcNow
            };

            _context.Media.Add(media);
            await _context.SaveChangesAsync();

            return media;
        }
        public async Task<Media?> GetByIdAsync(int id)
        {
            return await _context.Media
                .Include(m => m.Owner)
                .FirstOrDefaultAsync(m => m.Id == id && m.DeletedAt == null);
        }
        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var media = await _context.Media
                .FirstOrDefaultAsync(m => m.Id == id && m.DeletedAt == null);

            if (media == null)
                return false;

            // ownership check
            if (media.OwnerId != userId)
                return false;

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            var filePath = Path.Combine(uploadsFolder, media.StorageKey);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            media.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }
    }

}
