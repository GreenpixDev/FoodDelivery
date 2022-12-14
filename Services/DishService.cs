using System.Security.Claims;
using FoodDelivery.Database.Context;
using FoodDelivery.Exception;
using FoodDelivery.Models;
using FoodDelivery.Models.Dto;
using FoodDelivery.Models.Entity;
using FoodDelivery.Utils;

namespace FoodDelivery.Services;

public class DishService : IDishService
{
    private const int PageSize = 5;
    
    private readonly FoodDeliveryContext _context;

    public DishService(FoodDeliveryContext context)
    {
        _context = context;
    }

    public DishPagesListDto GetDishPage(IList<DishCategory> categories, bool vegetarian, DishSorting? sorting, int page)
    {
        IQueryable<Dish> query = _context.Dishes;

        if (vegetarian)
        {
            query = query.Where(dish => dish.Vegetarian == true);
        }

        if (categories.Any())
        {
            query = query.Where(dish => categories.Contains(dish.Category));
        }

        if (sorting != null)
        {
            switch (sorting)
            {
                case DishSorting.NameAsc:
                    query = query.OrderBy(dish => dish.Name);
                    break;
                case DishSorting.NameDesc:
                    query = query.OrderByDescending(dish => dish.Name);
                    break;
                case DishSorting.PriceAsc:
                    query = query.OrderBy(dish => dish.Price);
                    break;
                case DishSorting.PriceDesc:
                    query = query.OrderByDescending(dish => dish.Price);
                    break;
                case DishSorting.RatingAsc:
                    query = query.OrderBy(dish => dish.Rating);
                    break;
                case DishSorting.RatingDesc:
                    query = query.OrderByDescending(dish => dish.Rating);
                    break;
            }
        }
        
        int totalDishesCount = query.Count();
        int pageCount = (int) Math.Ceiling(totalDishesCount / (decimal) PageSize);

        if (page > pageCount)
        {
            throw new PageNotFoundException();
        }
        
        List<Dish> dishes = query.Skip(PageSize * (page - 1)).Take(PageSize).ToList();

        return new DishPagesListDto
        {
            Dishes = (from dish in dishes
                select new DishDto
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Description = dish.Description,
                    Price = dish.Price,
                    Image = dish.Image,
                    Vegetarian = dish.Vegetarian,
                    Rating = dish.Rating,
                    Category = dish.Category
                }).ToList(),
            Pagination = new PageInfoModel
            {
                Size = dishes.Count,
                Count = pageCount,
                Current = page
            }
        };
    }

    public DishDto GetDishInfo(Guid dishId)
    {
        Dish? result = _context.Dishes.Find(dishId);

        if (result == null)
        {
            throw new DishNotFoundException();
        }
        
        return new DishDto
        {
            Id = result.Id,
            Name = result.Name,
            Description = result.Description,
            Price = result.Price,
            Image = result.Image,
            Vegetarian = result.Vegetarian,
            Rating = result.Rating,
            Category = result.Category
        };
    }

    public bool CanSetRating(ClaimsPrincipal principal, Guid dishId)
    {
        if (!_context.Dishes.Any(dish => dish.Id == dishId))
        {
            throw new DishNotFoundException();
        }

        return (from order in _context.Orders
                join dish in _context.OrderDishes on order.Id equals dish.OrderId
                where order.UserId == ClaimsUtils.getId(principal) && dish.DishId == dishId
                select 1
            ).Any();
    }
    
    public void SetRating(ClaimsPrincipal principal, Guid dishId, int rating)
    {
        if (!CanSetRating(principal, dishId))
        {
            throw new DishNotOrderedException();
        }

        DishRating? dishRating = _context.DishRatings
            .Find(ClaimsUtils.getId(principal), dishId);

        if (dishRating == null)
        {
            _context.DishRatings.Add(new DishRating
            {
                UserId = ClaimsUtils.getId(principal),
                DishId = dishId,
                Rating = rating
            });
        }
        else
        {
            dishRating.Rating = rating;
            _context.DishRatings.Update(dishRating);
        }
        _context.SaveChanges();
        
        double newRating = _context.DishRatings
            .Where(rating => rating.DishId == dishId)
            .Select(rating => rating.Rating)
            .ToList()
            .Average();

        Dish dish = new Dish {Id = dishId};
        _context.Dishes.Attach(dish);
        dish.Rating = newRating;
        _context.SaveChanges();
    }
}