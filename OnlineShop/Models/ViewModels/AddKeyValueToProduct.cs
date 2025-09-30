using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;

namespace PresentationLayer.Models.ViewModels
{
    public class AddKeyValueToProduct
    {
        
        [Required]
        public int KeyId { get; set; }

        [Required]
        public IEnumerable<int> ValueIds { get; set; }

        [Required]
        public IEnumerable<int> ProductIds { get; set; }

        // Data for dropdowns
        public IEnumerable<SelectListItem> Products { get; set; }
        public IEnumerable<SelectListItem> Keys { get; set; }
    }


}
