namespace PresentationLayer.Models.ViewModels
{
    public class OrderFilterViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Search { get; set; }
        public string Status { get; set; }
    }
}
