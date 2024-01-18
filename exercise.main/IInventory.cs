using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exercise.main
{
    public interface IInventory
    {
        bool IsItemInStock(string sku);
        decimal GetProductPrice(string sku);
    }
}