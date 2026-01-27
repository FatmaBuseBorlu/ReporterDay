using Microsoft.AspNetCore.Mvc;
using ReporterDay.BusinessLayer.Abstract;
using ReporterDay.PresentationLayer.Models.Components;
using System.Linq;
using System.Threading.Tasks;

namespace ReporterDay.PresentationLayer.ViewComponents.ArticleDetailViewComponents
{
    public class _ArticleDetailCommentsComponentPartial : ViewComponent
    {
        private readonly ICommentService _commentService;
        private readonly IToxicityService _toxicityService;

        public _ArticleDetailCommentsComponentPartial(ICommentService commentService, IToxicityService toxicityService)
        {
            _commentService = commentService;
            _toxicityService = toxicityService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var comments = _commentService.TGetCommentsByArticleId(id) ?? new();

            var tasks = comments.Select(async c =>
            {
                var text = (c.CommentDetail ?? "").Trim();
                var check = await _toxicityService.CheckAsync(text);

                return new CommentListItemViewModel
                {
                    CommentId = c.CommentId,
                    CommentDetail = text,
                    CommentDate = c.CommentDate,
                    UserFullName = c.AppUser != null ? $"{c.AppUser.Name} {c.AppUser.Surname}" : "Anonim",

                    IsToxic = check.IsToxic,
                    ToxicityScore = check.Score,
                    ToxicLabel = check.Label ?? "",
                    IsCheckAvailable = check.IsAvailable
                };
            });

            var vm = (await Task.WhenAll(tasks))
                .OrderByDescending(x => x.CommentDate)
                .ToList();

            return View(vm);
        }
    }
}
