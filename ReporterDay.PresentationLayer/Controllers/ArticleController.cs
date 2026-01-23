using Microsoft.AspNetCore.Mvc;
using ReporterDay.BusinessLayer.Abstract;
using ReporterDay.PresentationLayer.Security;

namespace ReporterDay.PresentationLayer.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IdProtector _idProtector;

        public ArticleController(IArticleService articleService, IdProtector idProtector)
        {
            _articleService = articleService;
            _idProtector = idProtector;
        }

        public IActionResult ArticleDetail(string id)
        {

            if (int.TryParse(id, out var numericId))
            {
                var token = _idProtector.Protect(numericId);
                return RedirectToAction(nameof(ArticleDetail), new { id = token });
            }

            if (!_idProtector.TryUnprotect(id, out var articleId))
                return NotFound();

            ViewBag.i = articleId;
            return View();
        }
    }
}
