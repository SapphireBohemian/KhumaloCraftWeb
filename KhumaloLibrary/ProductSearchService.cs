using KhumaloCraftWeb.Data;
using KhumaloCraftWeb.Models;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KhumaloLibrary
{
    public class ProductSearchService
    {
        private readonly IProductRepository _productRepository;

        public ProductSearchService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<Products> SearchProducts(string query)
        {
            // Use _productRepository to perform search
            return _productRepository.GetAllProducts().Where(p => p.Name.Contains(query)).ToList();
        }
    }
}
