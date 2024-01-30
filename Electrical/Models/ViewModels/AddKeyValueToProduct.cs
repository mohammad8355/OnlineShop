using BusinessEntity;
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
    }


}
