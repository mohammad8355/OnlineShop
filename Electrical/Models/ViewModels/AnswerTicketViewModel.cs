namespace PresentationLayer.Models.ViewModels
{
    public class AnswerTicketViewModel
    {
        public int TicketId { get; set; }
        public string TicketDescription { get; set; }
        public string AnswerDescription { get; set; }
        public string Username { get; set; }
        public DateTime LastUpdate { get; set; }

    }
}
