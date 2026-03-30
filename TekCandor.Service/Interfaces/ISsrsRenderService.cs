using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Service.Interfaces
{
    public interface ISsrsRenderService
    {
        Task<(byte[] Content, string ContentType, string FileExtension)> RenderAsync(
           string reportPath,
           string format,
           IDictionary<string, string>? parameters,
           CancellationToken cancellationToken = default);
    }
}
