using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Getting_Real_______
{
    internal interface iProductRepository
    {
        Products addproducts(List<Products> products);
        void Deleteproduct(List<Products> products);
        Products Updateproduct(List<Products> products);
        void Checkout(List<Products> inventory);


    }
}