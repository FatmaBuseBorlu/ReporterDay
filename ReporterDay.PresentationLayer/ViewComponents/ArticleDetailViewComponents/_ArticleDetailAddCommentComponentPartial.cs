using Microsoft.AspNetCore.Mvc;
using ReporterDay.PresentationLayer.Models.Components;

namespace ReporterDay.PresentationLayer.ViewComponents.ArticleDetailViewComponents
{
    public class _ArticleDetailAddCommentComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke(string protectedArticleId)
        {
            var model = new AddCommentRequestViewModel
            {
                ProtectedArticleId = protectedArticleId
            };

            return View(model);
        }
    }
}
