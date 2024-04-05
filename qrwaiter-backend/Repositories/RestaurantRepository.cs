using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.EntityFrameworkCore;
using qrwaiter_backend.Repositories.Interfaces;
using qrwaiter_backend.Data.Models;
using qrwaiter_backend.Data;
namespace qrwaiter_backend.Repositories
{

    public class RestaurantRepository : Repository<Restaurant>, IRestaurantRepository 
    {
        private readonly ApplicationDbContext _context;

        public RestaurantRepository(ApplicationDbContext context) :base(context)
        {
            this._context = context;
        }

        public void SoftDelete(Guid id)
        {
            Restaurant? restaurant = _context.Restaurant.Find(id);
            if (restaurant == null) {   
                throw new InvalidOperationException();
            }
            restaurant.IsDeleted = true;
            _context.Entry(restaurant).State = EntityState.Modified;
        }
        
        

    }
}

