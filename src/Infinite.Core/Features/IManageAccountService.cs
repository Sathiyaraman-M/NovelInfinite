﻿using Infinite.Shared.Wrapper;

namespace Infinite.Core.Features;

public interface IManageAccountService
{
    Task<IResult<string>> GetPortFolioMd(string userId);
}