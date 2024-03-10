using System;
using api.Utilities;

namespace api.Infrastructure.Services.DocSynchronizer
{
    public interface IDocSynchronizer
    {
        Task SyncDocumentsFromExternalSource(string email);
    }
}

