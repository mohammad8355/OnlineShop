using DataAccessLayer.Models;

namespace PresentationLayer.Models.ViewModels
{
    public class ChooseSpecialKeysViewModel
    {
        public List<AdjKey> keys { get; set; }

        public List<int> chooseKeys { get; set; }
        public int ProductId { get; set; }

    }
}
