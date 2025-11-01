using APIService.Models;
using APIService.Models.DTOs;
using APIService.Repository.Interface;
using APIService.Service.Interface;
using AutoMapper;

namespace APIService.Service.Implementations
{
    public class SizeService : ISizeService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public SizeService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<int> CreateSize(SizeCUDTO sizeCUDTO)
        {
            if (sizeCUDTO == null)
            {
                return 2;
            }

            try
            {
                var entity = _mapper.Map<Sizes>(sizeCUDTO);


                await _repositoryManager.SizeRepository.CreateSize(entity);

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
        }

        public async Task<int> DeleteSize(int id)
        {
            try
            {
                var sizeExist = await _repositoryManager.SizeRepository.GetSizesById(id);
                if (sizeExist == null)
                {
                    return 2;
                }
                await _repositoryManager.SizeRepository.DeleteSizeAsync(sizeExist);
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
        }

        public async Task<(List<SizeDTO>, int)> GetAll()
        {
            try
            {
                var sizeExist = await _repositoryManager.SizeRepository.GetAllSize();

                if (sizeExist.Count != 0)
                {
                    var sizeDto = _mapper.Map<List<SizeDTO>>(sizeExist);

                    return (sizeDto, 0);
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

        public async Task<(SizeDTO, int)> GetSizeById(int id)
        {
            try
            {
                var sizeExist = await _repositoryManager.SizeRepository.GetSizesById(id);

                if (sizeExist != null)
                {
                    var sizeDto = _mapper.Map<SizeDTO>(sizeExist);
                    return (sizeDto, 0);
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

        public async Task<int> UpdateSize(SizeCUDTO sizeCUDTO)
        {
            if (sizeCUDTO == null)
                return 2;

            try
            {
                var size = await _repositoryManager.SizeRepository
                    .GetSizesById(sizeCUDTO.Id);

                if (size == null)
                    return 3;

                _mapper.Map(sizeCUDTO, size);

                await _repositoryManager.SizeRepository.UpdateSizeAsync(size);

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
