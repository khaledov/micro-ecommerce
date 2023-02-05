using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _catalogContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class.
        /// </summary>
        /// <param name="catalogContext">The catalog context.</param>
        public ProductRepository(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        

        /// <summary>
        /// Gets the product.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<Product> GetProduct(string id) =>await _catalogContext
            .Products
            .Find(p => p.Id == id)
            .FirstOrDefaultAsync();

        /// <summary>
        /// Gets products by category.
        /// </summary>
        /// <param name="categoryName">Name of the category.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName) =>
            await _catalogContext
            .Products
            .Find(Builders<Product>.Filter.Eq(p => p.Category, categoryName))
            .ToListAsync();

        /// <summary>
        /// Gets the product by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public async Task<Product> GetProductByName(string name) =>
            await _catalogContext
            .Products
            .Find(Builders<Product>.Filter.Eq(p => p.Name, name))
            .FirstOrDefaultAsync();

        /// <summary>
        /// Gets the products.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Product>> GetProducts()=> await _catalogContext
            .Products
            .Find(p => true)
            .ToListAsync();



        /// <summary>
        /// Creates the product.
        /// </summary>
        /// <param name="product">The product.</param>
        public async Task CreateProduct(Product product) => await _catalogContext.Products.InsertOneAsync(product);

        /// <summary>
        /// Updates the product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await _catalogContext
                                        .Products
                                        .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        /// <summary>
        /// Deletes the product.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _catalogContext
                                                .Products
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

       
    }
}
