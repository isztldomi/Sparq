using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.MediaDto
{
    public class MediaFileResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
    }
}
