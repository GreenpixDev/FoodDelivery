namespace FoodDelivery.Models.Dto;

public class DishPagesListDto
{
    public List<DishDto> Dishes { get; set; }
    
    public PageInfoModel Pagination { get; set; }
}