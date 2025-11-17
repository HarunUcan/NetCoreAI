using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreAI.Project09_TranslateAI.Interfaces
{
    public interface ITranslateService
    {
        Task<string> TranslateAsync(string content, string targetLang);
        string ProviderName { get; }
    }
}
