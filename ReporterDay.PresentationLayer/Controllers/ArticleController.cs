using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReporterDay.BusinessLayer.Abstract;
using ReporterDay.EntityLayer.Entities;
using ReporterDay.PresentationLayer.Models.Articles;
using ReporterDay.PresentationLayer.Models.Components;
using ReporterDay.PresentationLayer.Security;
using System;
using System.Threading.Tasks;

namespace ReporterDay.PresentationLayer.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ICommentService _commentService;
        private readonly ICommentModerationService _moderation;
        private readonly UserManager<AppUser> _userManager;
        private readonly IArticleIdProtector _articleIdProtector;

        public ArticleController(
            IArticleService articleService,
            ICommentService commentService,
            ICommentModerationService moderation,
            UserManager<AppUser> userManager,
            IArticleIdProtector articleIdProtector)
        {
            _articleService = articleService;
            _commentService = commentService;
            _moderation = moderation;
            _userManager = userManager;
            _articleIdProtector = articleIdProtector;
        }

        [HttpGet]
        public IActionResult ArticleDetail(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("id boş geldi");

            var articleId = _articleIdProtector.Unprotect(id);

            var vm = new ArticleDetailPageVm
            {
                ArticleId = articleId,
                ProtectedArticleId = id
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult CommentsPartial(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || !_articleIdProtector.TryUnprotect(id, out var articleId))
                return BadRequest("Geçersiz makale id.");

            return ViewComponent("_ArticleDetailCommentsComponentPartial", new { id = articleId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCommentAjax(AddCommentRequestViewModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.CommentDetail))
                return BadRequest(new { ok = false, message = "Yorum metni boş olamaz." });

            if (string.IsNullOrWhiteSpace(model.ProtectedArticleId) ||
                !_articleIdProtector.TryUnprotect(model.ProtectedArticleId, out var articleId))
            {
                return BadRequest(new { ok = false, message = "Geçersiz makale id." });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized(new { ok = false, message = "Giriş yapılmamış." });

            var text = model.CommentDetail.Trim();

  
            var decision = await _moderation.CheckAsync(text);
            if (!decision.Allow)
                return BadRequest(new { ok = false, message = decision.Message });

            var comment = new Comment
            {
                ArticleId = articleId,
                AppUserId = user.Id,
                CommentDetail = text,
                CommentDate = DateTime.Now,
                IsValid = true
            };

            _commentService.TInsert(comment);

            return Json(new { ok = true });
        }
    }
}
