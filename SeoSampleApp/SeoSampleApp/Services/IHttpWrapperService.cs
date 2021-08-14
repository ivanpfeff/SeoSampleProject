using System.Threading.Tasks;

namespace SeoSampleApp.Services
{
    public interface IHttpWrapperService
    {
        Task<string> ExecuteGETRequest(string searchUrl);
    }
}