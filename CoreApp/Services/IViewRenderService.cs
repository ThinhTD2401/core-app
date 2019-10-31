using System.Threading.Tasks;

namespace CoreApp.Services
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}