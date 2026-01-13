using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ReporterDay.PresentationLayer.ViewComponents.ArticleDetailViewComponents
{
    public class _ArticleDetailCommentsComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
            
        }
    }
}
