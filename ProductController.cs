using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project;

namespace Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AdventureWorks2022Context _context;

        // Constructor to inject the AdventureWorksContext
        public ProductsController(AdventureWorks2022Context context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            // Ensure _context.Products is recognized as DbSet<Product>
            return await _context.Products.ToListAsync();
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            // Find the product by its ID
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(); // Return 404 if the product doesn't exist
            }
            return product;  // Return the product if found
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();  // Return 400 if the IDs don't match
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.ProductId == id))
                {
                    return NotFound();  // Return 404 if the product doesn't exist
                }
                throw;
            }

            return NoContent();  // Return 204 if the update is successful
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();  // Return 404 if the product doesn't exist
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();  // Return 204 after successful deletion
        }
    }
}