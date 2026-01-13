using Microsoft.AspNetCore.Mvc;
using ReporterDay.BusinessLayer.Abstract;

namespace ReporterDay.PresentationLayer.ViewComponents.ArticleDetailViewComponents
{
    public class _ArticleDetailTagListComponentPartial:ViewComponent
    {
        private readonly ITagService _tagService;
        public _ArticleDetailTagListComponentPartial(ITagService tagService)
        {
            _tagService = tagService;
        }
        public IViewComponentResult Invoke()
        {
            var values= _tagService.TGetListAll();
            return View(values);
        }
    } 
}
