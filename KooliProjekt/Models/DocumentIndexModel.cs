using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class DocumentIndexModel
    {
        public DocumentSearch Search { get; set; }
        public PagedResult<Document> Data { get; set; }
    }
}
