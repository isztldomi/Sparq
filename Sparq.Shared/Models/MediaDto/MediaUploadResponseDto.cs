using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.MediaDto
{
    public class MediaUploadResponseDto
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
    }

}
