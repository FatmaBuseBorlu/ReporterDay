using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReporterDay.BusinessLayer.Abstract;
using ReporterDay.EntityLayer.Entities;

namespace ReporterDay.PresentationLayer.Controllers
{
    public class AuthorController : Controller
    {
        private readonly UserManager<AppUser> _userManager; 
        private readonly IArticleService _articleService;

        public async Task <IActionResult> CreateArticle()
        {
            var user= await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.id = user.Id;
            ViewBag.name = user.Name +" "+ user.Surname;
            return View();
        }
        
          
     
        [HttpPost]
        public async Task<IActionResult> CreateArticle(Article article)
        {
            return RedirectToAction("Index","Default");
        }
    }
}
