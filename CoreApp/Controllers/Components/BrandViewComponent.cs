using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreApp.Controllers.Components
{
    public class BrandViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Task<IViewComponentResult> task = new Task<IViewComponentResult>(View);
            task.Start();
            return await task;
        }
    }
}