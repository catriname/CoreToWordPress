using CoreApi.Interfaces;
using CoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CoreApi.Controllers
{
    [ApiController]
    [Route("")]
    public class ProductController : ControllerBase
    {
        private readonly IRepository _repository;

        public ProductController(IRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Listing of product lines available.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET productlines/
        ///     {        
        ///     }
        /// </remarks>
        [HttpGet("ProductLines")]
        public async Task<IActionResult> ProductLines()
        {
            return Ok(await _repository.ReadOnlyList<ProductLine>("_ProductLine"));
        }

        /// <summary>
        /// Products under a specific product line.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET products/
        ///     {        
        ///       "productLine": "motorcycles"
        ///     }
        /// </remarks>
        /// <param name="productLine"></param> 
        [HttpGet("Products/{productLine}")]
        public async Task<IActionResult> Products(string productLine)
        {
            productLine = productLine.Replace(" ", "-").ToLower();

            var products = await _repository.ReadOnlyList<Product>("_Product_" + productLine);
            products = products.Where(x => x.ProductLine.Replace(" ","-").ToLower() == productLine).ToList();

            return Ok(products);
        }
    }
}