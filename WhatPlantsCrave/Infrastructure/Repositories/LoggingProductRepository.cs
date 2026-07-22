using Brawndo_Components.Models;

namespace WhatPlantsCrave.Infrastructure.Repositories
{
    /// <summary>
    /// Decorator that adds logging to ProductRepository operations.
    /// This demonstrates the Decorator Pattern: same interface, wraps another implementation,
    /// adds behavior (logging) without modifying the original class.
    /// </summary>
    public class LoggingProductRepository : IProductRepository
    {
        private readonly IProductRepository _inner;
        private readonly ILogger<LoggingProductRepository> _logger;

        public LoggingProductRepository(IProductRepository inner, ILogger<LoggingProductRepository> logger)
        {
            _inner = inner;
            _logger = logger;
        }

        public int Create(string name, string productNumber, decimal standardCost, decimal listPrice,
            DateTime sellStartDate, string? color = null, string? size = null, decimal? weight = null,
            int? productCategoryId = null, int? productModelId = null, DateTime? sellEndDate = null,
            DateTime? discontinuedDate = null, string? thumbnailPhotoFileName = null)
        {
            _logger.LogInformation(
                "ProductRepository.Create called: Name={Name}, ProductNumber={ProductNumber}, Cost={Cost}, Price={Price}",
                name, productNumber, standardCost, listPrice);

            try
            {
                var id = _inner.Create(name, productNumber, standardCost, listPrice, sellStartDate,
                    color, size, weight, productCategoryId, productModelId, sellEndDate, discontinuedDate,
                    thumbnailPhotoFileName);

                _logger.LogInformation("ProductRepository.Create succeeded: Created product ID={ProductId}", id);
                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ProductRepository.Create failed for product {Name}", name);
                throw;
            }
        }

        public bool Delete(int productId)
        {
            _logger.LogInformation("ProductRepository.Delete called: ProductId={ProductId}", productId);

            try
            {
                var result = _inner.Delete(productId);
                _logger.LogInformation("ProductRepository.Delete completed: ProductId={ProductId}, Success={Success}",
                    productId, result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ProductRepository.Delete failed for ProductId={ProductId}", productId);
                throw;
            }
        }

        public List<Product> GetActive()
        {
            _logger.LogInformation("ProductRepository.GetActive called");

            try
            {
                var result = _inner.GetActive();
                _logger.LogInformation("ProductRepository.GetActive succeeded: Retrieved {Count} products",
                    result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ProductRepository.GetActive failed");
                throw;
            }
        }

        public List<Product> GetAll()
        {
            _logger.LogInformation("ProductRepository.GetAll called");

            try
            {
                var result = _inner.GetAll();
                _logger.LogInformation("ProductRepository.GetAll succeeded: Retrieved {Count} products",
                    result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ProductRepository.GetAll failed");
                throw;
            }
        }

        public Product? GetByID(int productId)
        {
            _logger.LogInformation("ProductRepository.GetByID called: ProductId={ProductId}", productId);

            try
            {
                var result = _inner.GetByID(productId);
                if (result != null)
                {
                    _logger.LogInformation("ProductRepository.GetByID succeeded: Found product {Name}",
                        result.Name);
                }
                else
                {
                    _logger.LogInformation("ProductRepository.GetByID: Product not found for ID={ProductId}",
                        productId);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ProductRepository.GetByID failed for ProductId={ProductId}", productId);
                throw;
            }
        }

        public List<Product> SearchByName(string searchTerm)
        {
            _logger.LogInformation("ProductRepository.SearchByName called: SearchTerm={SearchTerm}", searchTerm);

            try
            {
                var result = _inner.SearchByName(searchTerm);
                _logger.LogInformation("ProductRepository.SearchByName succeeded: Found {Count} products matching '{SearchTerm}'",
                    result.Count, searchTerm);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ProductRepository.SearchByName failed for SearchTerm={SearchTerm}", searchTerm);
                throw;
            }
        }

        public bool Update(int productId, string? name = null, decimal? standardCost = null,
            decimal? listPrice = null, string? color = null)
        {
            _logger.LogInformation(
                "ProductRepository.Update called: ProductId={ProductId}, Name={Name}, Cost={Cost}, Price={Price}",
                productId, name, standardCost, listPrice);

            try
            {
                var result = _inner.Update(productId, name, standardCost, listPrice, color);
                _logger.LogInformation("ProductRepository.Update completed: ProductId={ProductId}, Success={Success}",
                    productId, result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ProductRepository.Update failed for ProductId={ProductId}", productId);
                throw;
            }
        }
    }
}
