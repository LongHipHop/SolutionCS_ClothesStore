using APIService.Models;
using APIService.Models.DTOs;
using APIService.Repository.Interface;
using APIService.Service.Interface;
using AutoMapper;

namespace APIService.Service.Implementations
{
    public class ColorService : IColorService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public ColorService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;

        }

        public async Task<int> CreateColor(ColorCUDTO colorCUDTO)
        {
            if (colorCUDTO == null)
            {
                return 2;
            }

            try
            {
                var entity = _mapper.Map<Colors>(colorCUDTO);

                await _repositoryManager.ColorRepository.CreateColor(entity);

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
        }

        public async Task<int> DeleteColor(int id)
        {
            try
            {
                var ColorExist = await _repositoryManager.ColorRepository.GetColorById(id);
                if (ColorExist == null)
                {
                    return 2;
                }
                await _repositoryManager.ColorRepository.DeleteColor(ColorExist);
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
        }

        public async Task<(List<ColorDTO>, int)> GetAll()
        {
            try
            {
                var ColorExist = await _repositoryManager.ColorRepository.GetAll();

                if (ColorExist.Count != 0)
                {
                    var ColorDto = _mapper.Map<List<ColorDTO>>(ColorExist);

                    return (ColorDto, 0);
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

        public async Task<(ColorDTO, int)> GetColorById(int id)
        {
            try
            {
                var ColorExist = await _repositoryManager.ColorRepository.GetColorById(id);

                if (ColorExist != null)
                {
                    var ColorDto = _mapper.Map<ColorDTO>(ColorExist);
                    return (ColorDto, 0);
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
            throw new NotImplementedException();
        }

        public async Task<int> UpdateColor(ColorCUDTO ColorCUDTO)
        {
            if (ColorCUDTO == null)
                return 2;

            try
            {
                var Color = await _repositoryManager.ColorRepository
                    .GetColorById(ColorCUDTO.Id);

                if (Color == null)
                    return 3;

                _mapper.Map(ColorCUDTO, Color);

                await _repositoryManager.ColorRepository.UpdateColor(Color);

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
