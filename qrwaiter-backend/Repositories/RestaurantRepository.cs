using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.EntityFrameworkCore;
using qrwaiter_backend.Repositories.Interfaces;
using qrwaiter_backend.Data.Models;
using qrwaiter_backend.Data;
using qrwaiter_backend.Extensions;
using shortid;
namespace qrwaiter_backend.Repositories
{

    public class RestaurantRepository : Repository<Restaurant>, IRestaurantRepository
    {
        private readonly ApplicationDbContext _context;

        public RestaurantRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public void SoftDelete(Guid id)
        {
            Restaurant? restaurant = _context.Restaurant.Find(id);
            if (restaurant == null)
            {
                throw new InvalidOperationException();
            }
            restaurant.IsDeleted = true;
            _context.Entry(restaurant).State = EntityState.Modified;
        }
        public async Task<Restaurant> GenerateNewLink(Guid id)
        {
            Restaurant? restaurant = await _context.Restaurant.FindAsync(id);
            if (restaurant == null)
            {
                throw new InvalidOperationException();
            }

            restaurant.WaiterLink = ShortId.Generate();
            _context.Restaurant.Update(restaurant);

            return restaurant;
        }



    }
}

