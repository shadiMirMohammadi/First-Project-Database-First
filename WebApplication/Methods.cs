using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.Models;

namespace WebApplication
{
    public class Methods
    {
        Action<T_Categories,T_Products> action = (T_Categories category, T_Products product) =>
        {
            if (product.CategoryName.Trim() == category.CategoryName)
            {
                product.CategoryId = 1;
            }
        };
    }
}