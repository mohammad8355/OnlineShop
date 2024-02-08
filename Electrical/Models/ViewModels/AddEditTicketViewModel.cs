using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.ViewModels
{
    public class AddEditTicketViewModel
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        [MaxLength(30)]
        public string Status { get; set; }
        public DateTime LastUpdate { get; set; }
        public string User_Id { get; set; }

    }
}
