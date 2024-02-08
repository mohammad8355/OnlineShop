using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.ViewModels
{
    public class AddEditCommentViewModel
    {
        public int Id { get; set;}
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public int? Ticket_Id { get; set; }
        public string User_Id { get; set; }
        public int? reply_Id { get; set; }



    }
}
