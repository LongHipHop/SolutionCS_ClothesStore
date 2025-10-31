using APIService.Models;
using APIService.Models.DTOs;
using APIService.Repository.Interface;
using APIService.Service.Interface;
using AutoMapper;

namespace APIService.Service.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public ProductService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<int> CountAllProductAsync()
        {
            try
            {
                var products = await _repositoryManager.ProductRepository.GetAll();

                return products.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
        }

        public async Task<int> CreateProduct(ProductCUDTO productDTO)
        {
            if (productDTO == null)
                return 1;

            try
            {
                var product = _mapper.Map<Products>(productDTO);
                product.CreatedAt = DateTime.Now;
                product.UpdatedAt = DateTime.Now;
                await _repositoryManager.ProductRepository.AddProduct(product);
                return 0;
            }
            catch (AutoMapperMappingException)
            {
                return 3;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                return 2;
            }
        }


        public async Task<int> DeleteProduct(int id)
        {
            try
            {
                var productExist = await _repositoryManager.ProductRepository.GetProductById(id);
                if(productExist == null)
                {
                    return 2;
                }
                await _repositoryManager.ProductRepository.DeleteProduct(productExist);
                return 0;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message );
                return 1;
            }
        }

        public async Task<(List<ProductDTO>, int)> GetAll()
        {
            try
            {
                var productExist = await _repositoryManager.ProductRepository.GetAll();

                if(productExist.Count != 0)
                {
                    var productDto = _mapper.Map<List<ProductDTO>>(productExist);
                    return (productDto, 0);
                }
                else
                {
                    return (new(), 2);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return (new(), 1);
            }
        }

        public async Task<(ProductDTO, int)> GetProductById(int id)
        {
            try
            {
                var productExist = await _repositoryManager.ProductRepository.GetProductById(id);

                if(productExist != null)
                {
                    var productDto = _mapper.Map<ProductDTO>(productExist);
                    return (productDto, 0);
                }
                else
                {
                    return (new(), 2);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message );
                return (new(), 1);
            }
            
        }

        public async Task<int> UpdateProduct(ProductCUDTO productDTO)
        {
            if(productDTO == null)
            {
                return 1;
            }

            try
            {
                var productExist = await _repositoryManager.ProductRepository.GetProductById(productDTO.Id);

                if(productExist == null)
                {
                    return 2;
                }

                productExist.ProductName = productDTO.ProductName;
                productExist.Description = productDTO.Description;
                productExist.Price = productDTO.Price;
                productExist.Image = productDTO.Image;
                productExist.StockQuantity = productDTO.StockQuantity;
                productExist.UpdatedAt = DateTime.Now;

                await _repositoryManager.ProductRepository.UpdateProduct(productExist);

                return 0;
            }
            catch(AutoMapperMappingException)
            {
                return 3;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 4;
            }
        }
    }
}
