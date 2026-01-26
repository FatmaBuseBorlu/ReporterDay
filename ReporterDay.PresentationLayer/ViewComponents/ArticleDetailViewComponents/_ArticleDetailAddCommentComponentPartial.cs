using Microsoft.AspNetCore.Mvc;
using ReporterDay.PresentationLayer.Models.Components;

namespace ReporterDay.PresentationLayer.ViewComponents.ArticleDetailViewComponents
{
    public class _ArticleDetailAddCommentComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke(string protectedId)
        {
            return View(new AddCommentRequestViewModel { ProtectedArticleId = protectedId });
        }
    }
}
