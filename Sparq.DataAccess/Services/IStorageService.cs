using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    public interface IStorageService
    {
        Task<string> UploadAsync(Stream stream, string fileName, string contentType);
        Task<Stream> DownloadAsync(string storageKey);
        Task DeleteAsync(string storageKey);
    }
}
