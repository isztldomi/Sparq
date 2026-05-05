using Microsoft.AspNetCore.Http;
using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    public interface IMediaService
    {
        Task<Media> UploadAsync(IFormFile file, string userId);
        Task<(Media Media, Stream Stream)> GetFileAsync(int id, string userId);
        Task<bool> DeleteAsync(int id, string userId);
    }
}
