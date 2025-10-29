using APIService.Models;
using APIService.Models.DTOs;
using APIService.Repository.Interface;
using APIService.Service.Interface;
using AutoMapper;

namespace APIService.Service.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public CategoryService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<(List<CategoryDTO>, int)> GetAll()
        {
            try
            {
                var categoryExist = await _repositoryManager.CategoryRepository.GetAll();

                if(categoryExist.Count != 0)
                {
                    var categoryDto = _mapper.Map<List<CategoryDTO>>(categoryExist);

                    return (categoryDto, 0);
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
    }
}
