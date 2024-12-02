using IIMSv1.Models.Shared;

namespace IIMSv1.Models.ViewModel
{
    public class CategoriesViewModel
    {
        public List<Supplies> supplies { get; set; } = new List<Supplies>();
        public List<ItemUnit> unit { get; set; } = new List<ItemUnit>();
        public List<ItemSpecType> itemSpecTypes { get; set; } = new List<ItemSpecType>();
        public AddSupplyInputModel AddSupplyInputModel { get; set; } = new AddSupplyInputModel();
        public AddUnitInputModel AddUnitInputModel { get; set; } = new AddUnitInputModel();
        public AddSpecinputModel AddSpecinputModel { get; set; } = new AddSpecinputModel();
        public EditTypeInputModel EditTypeModel { get; set; } = new EditTypeInputModel();
    }
}
