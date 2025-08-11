using Hermes.Models;

namespace Hermes.Repository
{
    interface IArticleRepository
    {
        void Insert(IEnumerable<Article> articles);
    }
}
