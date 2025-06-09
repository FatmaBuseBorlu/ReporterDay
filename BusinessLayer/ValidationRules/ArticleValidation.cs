using FluentValidation;
using ReporterDay.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReporterDay.BusinessLayer.ValidationRules
{
    public class ArticleValidation:AbstractValidator<Article>
    {
        public ArticleValidation() 
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Makale başlğı boş geçilemez");
            RuleFor(x => x.Content).NotEmpty().WithMessage("Makale içeriği boş geçilemez");
            RuleFor(x => x.Title).NotEmpty().MinimumLength(10).WithMessage("Başlık en az 10 karakter olmalıdır");
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100).WithMessage("Başlık en fazla 100 karakter olmalıdır");
            RuleFor(x => x.CategoryId).LessThan(0).WithMessage("Kategori ıd 0'dan küçük olamaz");   
        }
    }
}
