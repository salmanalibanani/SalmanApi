using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SalmanApi.Controllers
{
    
    [ApiController]
    [Route("api/answers")]
    public class AnswersController : ControllerBase
    {
        // GET: api/<AnswersController>
        [HttpGet("user")]
        public User GetUser()
        {
            return  new User
            {
                name="Salman Ali Banani", token= "545e00bd-6888-450e-8e0c-743d45c8088d"
            };
        }

        [HttpGet("sort")]
        public async Task<IList<Product>> GetProduct([FromQuery] string sortOption)
        {
            IList<Product> productList = await GetProductList();

            if (sortOption == null)
                return productList;
            else if (sortOption.Equals("Low"))
                return productList.OrderBy(x => x.price).ToList();
            else if (sortOption.Equals("High"))
                return productList.OrderByDescending(x => x.price).ToList();
            else if (sortOption.Equals("Ascending"))
                return productList.OrderBy(x => x.name).ToList();
            else if (sortOption.Equals("Descending"))
                return productList.OrderByDescending(x => x.name).ToList();
            else if (sortOption.Equals("Recommended"))
            {
                var groupedShopperHistory = await GetGroupedShopperHistory();
                return AddMissingProducts(groupedShopperHistory, productList);
            }
            else
                return productList;
        }

        private IList<Product> AddMissingProducts(IList<Product> groupedShopperHistory, IList<Product> productList)
        {
            return 
                (from x in productList
                join y in groupedShopperHistory on x.name equals y.name into prodGroup
                from item in prodGroup.DefaultIfEmpty(new Product() { name = x.name, price = x.price, quantity = 0.0M }) 
                orderby item.quantity descending
                select item).ToList();
        }

        private static async Task<IList<Product>> GetProductList()
        {
            IList<Product> productList;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://dev-wooliesx-recruitment.azurewebsites.net/api/resource/products?token=545e00bd-6888-450e-8e0c-743d45c8088d"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    productList = JsonSerializer.Deserialize<List<Product>>(apiResponse);
                }
            }

            return productList;
        }

        private async Task<IList<Product>> GetGroupedShopperHistory()
        {
            IList<ShopperHistory> shopperHistory;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://dev-wooliesx-recruitment.azurewebsites.net/api/resource/shopperHistory?token=545e00bd-6888-450e-8e0c-743d45c8088d"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    shopperHistory = JsonSerializer.Deserialize<List<ShopperHistory>>(apiResponse);
                }
            }

            List<Product> prod = new List<Product>();
            foreach (var history in shopperHistory)
            {
                prod.AddRange(history.products);
            }

            var groupShortedHistory = 
                (from x in prod group x by x.name into grp 
                select new Product
                {
                    name = grp.Key,
                    quantity = grp.Sum(x => x.quantity),
                    price = grp.First().price
                }
                ).ToList();
            
            return groupShortedHistory;
        }
    }

    
   
}
