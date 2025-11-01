using APIService.Models;
using APIService.Models.DTOs;
using APIService.Repository.Interface;
using APIService.Service.Interface;
using AutoMapper;

namespace APIService.Service.Implementations
{
    public class ProductVariantService : IProductVariantService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public ProductVariantService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<int> CreateProductVariant(ProductVariantCUDTO dto)
        {
            if (dto == null)
                return 2;

            try
            {
                // 🔎 Lấy ID dựa trên tên
                var product = await _repositoryManager.ProductRepository
                    .FindByCondition(p => p.ProductName == dto.ProductName, false, false);

                var color = await _repositoryManager.ColorRepository
                    .FindByCondition(c => c.ColorName == dto.ColorName, false, false);

                var size = await _repositoryManager.SizeRepository
                    .FindByCondition(s => s.SizeName == dto.SizeName, false, false);

                if (product == null || color == null || size == null)
                {
                    Console.WriteLine("❌ Product/Color/Size không tồn tại trong database!");
                    return 3;
                }

                // 🔄 Map sang entity
                var entity = _mapper.Map<ProductVariants>(dto);
                entity.ProductId = product.Id;
                entity.ColorId = color.Id;
                entity.SizeId = size.Id;

                // ✅ Thêm vào DB
                await _repositoryManager.ProductVariantRepository.CreateProductVariant(entity);

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error CreateProductVariant: {ex.Message}");
                return 1;
            }
        }


        public async Task<int> DeleteProductVariant(int id)
        {
            try
            {
                var productExist = await _repositoryManager.ProductVariantRepository.GetProductVariantsById(id);
                if(productExist == null)
                {
                    return 2;
                }
                await _repositoryManager.ProductVariantRepository.DeleteProductVariantAsync(productExist);
                return 0;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
        }

        public async Task<(List<ProductVariantDTO>, int)> GetAll()
        {
            try
            {
                var productExist = await _repositoryManager.ProductVariantRepository.GetAllProductVariant();

                if(productExist.Count != 0)
                {
                    var productDto = _mapper.Map<List<ProductVariantDTO>>(productExist);

                    return (productDto, 0);
                }
                else
                {
                    return (new(), 2);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (new(), 1);
            }
        }

        public async Task<(ProductVariantDTO, int)> GetProductVariantById(int id)
        {
            try
            {
                var productExist = await _repositoryManager.ProductVariantRepository.GetProductVariantsById(id);

                if(productExist != null)
                {
                    var productDto = _mapper.Map<ProductVariantDTO>(productExist);
                    return (productDto, 0);
                }
                else
                {
                    return (new(), 2);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (new(), 1);
            }
        }

        public async Task<int> UpdateProductVariant(ProductVariantCUDTO dto)
        {
            if (dto == null)
                return 2;

            try
            {
                var productVariant = await _repositoryManager.ProductVariantRepository
                    .GetProductVariantsById(dto.Id);

                if (productVariant == null)
                    return 3;

                _mapper.Map(dto, productVariant);

                await _repositoryManager.ProductVariantRepository.UpdateProductVariantAsync(productVariant);

                return 0; 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
        }

    }
}
