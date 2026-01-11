using Microsoft.AspNetCore.Mvc;

namespace ReporterDay.PresentationLayer.ViewComponents.ArticleDetailViewComponents
{
    public class _ArticleDetailContentComponentPartial : ViewComponent
    {
         public IViewComponentResult Invoke()
          {
                return View();
        }
    }
}
