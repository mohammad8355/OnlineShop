using DataAccessLayer.Models;

namespace PresentationLayer.Models.ViewModels
{
    public class SearchFilterViewModel
    {
        public List<AdjKey> keys { get; set; }
        public List<int> Values { get; set; }
        public string Sortby { get; set; }
        public decimal FromPrice { get; set; }
        public decimal ToPrice { get; set; }
        public bool IsExist { get; set; }
        public int Category_Id { get; set; }

    }
}
