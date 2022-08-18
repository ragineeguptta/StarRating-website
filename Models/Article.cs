using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarRating.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public ICollection<ArticleComment> ArticlesComments { get; set; }
    }
}
