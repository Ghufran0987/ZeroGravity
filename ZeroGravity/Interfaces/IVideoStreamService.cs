﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Interfaces
{
    public interface IVideoStreamService
    {
        Task<IEnumerable<StreamContentDto>> GetAvailableContentAsync(StreamContentType type);
    }
}