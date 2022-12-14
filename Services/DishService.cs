using System.Security.Claims;
using FoodDelivery.Database.Context;
using FoodDelivery.Exception;
using FoodDelivery.Models;
using FoodDelivery.Models.Dto;
using FoodDelivery.Models.Entity;
using FoodDelivery.Utils;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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
        Dish? result = (
            from dish in _context.Dishes
            where dish.Id == dishId
            select dish
        ).SingleOrDefault();

        if (result == null)
        {
            throw new NotFoundException();
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
       return _context.OrderDishes
           .Any(dish => dish.UserId == ClaimsUtils.getId(principal) && dish.DishId == dishId);
    }

    public void SetRating(ClaimsPrincipal principal, Guid dishId, int rating)
    {
        if (!CanSetRating(principal, dishId))
        {
            // TODO throw
            throw new NotImplementedException();
        }

        DishRating? dishRating = _context.DishRatings
            .Where(rating => rating.UserId == ClaimsUtils.getId(principal) && rating.DishId == dishId)
            .Select(rating => rating)
            .SingleOrDefault();

        if (dishRating == null)
        {
            dishRating = new DishRating();
            dishRating.UserId = ClaimsUtils.getId(principal);
            dishRating.DishId = dishId;
            dishRating.Rating = rating;
            _context.DishRatings.Add(dishRating);
        }
        else
        {
            dishRating.Rating = rating;
            _context.DishRatings.Update(dishRating);
        }

        try
        {
            _context.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            if (PostgresUtils.HasErrorCode(e, PostgresErrorCodes.ForeignKeyViolation))
            {
                throw new NotFoundException();
            }

            throw;
        }
        
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