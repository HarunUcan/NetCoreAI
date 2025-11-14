using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreAI.Project04_ChatAI.Interfaces
{
    public interface IChatService
    {
        Task<string> GenerateResponseAsync(string prompt);
        string ProviderName { get; }
    }
}
