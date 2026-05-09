using Sparq.Shared.Models.SnapshotDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.SessionDto
{
    public class SessionPublicWaitingListDto
    {
        public string Id { get; set; } = string.Empty;
        public SnapshotMetaDetails2ResponseDto? Snapshot { get; set; }
    }
}
