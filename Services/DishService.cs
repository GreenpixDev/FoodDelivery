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
        IEnumerable<Dish> result = (from dish in _context.Dishes
            where dish.Vegetarian == vegetarian && categories.Contains(dish.Category)
            orderby dish.Name
            // TODO offset
            select dish).Take(PageSize);

        return new DishPagesListDto
        {
            Dishes = (from dish in result
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
                Size = PageSize,
                Count = 1, // TODO
                Current = 1 // TODO
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
        throw new NotImplementedException();
    }

    public void SetRating(ClaimsPrincipal principal, Guid dishId, int rating)
    {
        if (!CanSetRating(principal, dishId))
        {
            // TODO throw
        }

        _context.DishRatings.Add(new DishRating
        {
            UserId = ClaimsUtils.getId(principal),
            DishId = dishId,
            Rating = rating
        });
        
        try
        {
            _context.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            if (PostgresUtils.HasErrorCode(e, PostgresErrorCodes.UniqueViolation))
            {
                // TODO
            }
            
            if (PostgresUtils.HasErrorCode(e, PostgresErrorCodes.ForeignKeyViolation))
            {
                throw new NotFoundException();
            }

            throw;
        }
    }
}