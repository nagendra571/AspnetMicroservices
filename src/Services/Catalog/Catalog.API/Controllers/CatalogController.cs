using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly ILogger<CatalogController> logger;

        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
        {
            this.productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await productRepository.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await productRepository.GetProduct(id);

            if (product == null)
            {
                logger.LogError($"Product with id: {id}, not found");
                return NotFound();
            }

            return Ok(product);
        }

        [Route("[action]/{category}", Name = "GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            var products = await productRepository.GetProductByCategory(category);

            return Ok(products);
        }




        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await productRepository.CreateProduct(product);

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            return Ok(await productRepository.UpdateProduct(product));
        }


        [HttpDelete("{id:length(24)}", Name = "DeelteProduct")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        public async Task<IActionResult> DeelteProductById(string id)
        {
            return Ok(await productRepository.DeleteProduct(id));
        }
    }
}
