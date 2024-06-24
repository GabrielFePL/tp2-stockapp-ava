using Microsoft.AspNetCore.Mvc;
using StockApp.Application.DTOs;
using StockApp.Application.Interfaces;
using StockApp.Application.Services;
using StockApp.Domain.Entities;
using StockApp.Infra.Data.Repositories;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductsController : Controller
    {

        private readonly IProductService _productService;
        private readonly IProductComparisonService _productComparisonService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            var produtos = await _productService.GetProducts();
            if (produtos == null)
            {
                return NotFound("Products not found");
            }
            return Ok(produtos);
        }

        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<ActionResult<ProductDTO>> Get(int id)
        {
            var produto = await _productService.GetProductById(id);
            if (produto == null)
            {
                return NotFound("Product not found");
            }
            return Ok(produto);
        }
        
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductDTO produtoDto)
        {
            if (produtoDto == null)
                return BadRequest("Data Invalid");

            await _productService.Add(produtoDto);

            return new CreatedAtRouteResult("GetProduct",
                new { id = produtoDto.Id }, produtoDto);
        }

        [HttpPut("{id:int}", Name = "UpdateProduct")]
        public async Task<ActionResult> Put(int id, [FromBody] ProductDTO produtoDto)
        {
            if (id != produtoDto.Id)
            {
                return BadRequest("Data invalid");
            }

            if (produtoDto == null)
                return BadRequest("Data invalid");

            await _productService.Update(produtoDto);

            return Ok(produtoDto);
        }

        [HttpDelete("{id:int}", Name = "DeleteProduct")]
        public async Task<ActionResult<ProductDTO>> Delete(int id)
        {
            var produtoDto = await _productService.GetProductById(id);

            if (produtoDto == null)
            {
                return NotFound("Product not found");
            }

            await _productService.Remove(id);

            return Ok(produtoDto);
        }

        [HttpGet("lowstock", Name = "GetLowStockProducts")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetLowStockProducts(int limiteEstoque)
        {
            var produtoDto = await _productService.EstoqueBaixo(limiteEstoque);
            if (produtoDto == null)
            {
                return NotFound("Products not found");
            }
            return Ok(produtoDto);
        }

        [HttpPut("bulk-update", Name = "BulkUpdateProducts")]
        public async Task<IActionResult> BulkUpdate([FromBody] List<ProductDTO> produtosDto)
        {
            if (produtosDto == null || !produtosDto.Any())
            {
                return BadRequest("Product Null or Invalid");
            };

            await _productService.BulkUpdateAsync(produtosDto);

            return Ok(produtosDto);
        }

        [HttpPost("compare")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> CompareProducts([FromBody] List<int> productIds)
        {
            var product = await _productComparisonService.CompareProductsAsync(productIds);
            return Ok(product);
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportToCsv()
        {
            var produtosDto = await _productService.GetProducts();
            if (produtosDto == null || !produtosDto.Any())
            {
                return NotFound("Products not found to export");
            }

            var csv = new StringBuilder();
            csv.AppendLine("Id,Name,Description,Price,Stock");

            foreach (var produto in produtosDto)
            {
                csv.AppendLine($"{produto.Id},{produto.Name},{produto.Description},{produto.Price},{produto.Stock}");
            }

            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "products.csv");
        }
    }
}
