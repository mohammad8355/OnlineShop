using DataAccessLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.ViewModels
{
    public class SearchViewModel
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string SearchInput { get; set; } = "";
        public int CategoryId { get; set; }
        public List<Category> Categories { get; set; }
    }
}
