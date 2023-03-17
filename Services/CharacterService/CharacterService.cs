using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Dtos.Character;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character>() {
            new Character(),
            new Character() { Id=1, Name = "Sam"}
        };
        private readonly IMapper _mapper;

        public CharacterService(IMapper mapper)
        {
            this._mapper = mapper;
        }
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var character = _mapper.Map<Character>(newCharacter);
            character.Id = characters.Max(x => x.Id) + 1;
            characters.Add(character);
            ServiceResponse<List<GetCharacterDto>> response = new ServiceResponse<List<GetCharacterDto>>();
            response.Data = characters.Select(x => _mapper.Map<GetCharacterDto>(x)).ToList();
            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> response = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var character = characters.FirstOrDefault(x => x.Id == id);

                if(character == null) {
                    throw new Exception($"Character with Id {id} not found");
                }
                characters.Remove(character);
               response.Data = characters.Select(x => _mapper.Map<GetCharacterDto>(x)).ToList();;


            }
            catch(Exception ex) {
                response.Data = null;
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;

            }
            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            ServiceResponse<List<GetCharacterDto>> response = new ServiceResponse<List<GetCharacterDto>>();
            response.Data = characters.Select(x => _mapper.Map<GetCharacterDto>(x)).ToList();
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();
            var character = characters.FirstOrDefault(x => x.Id == id);
            response.Data = _mapper.Map<GetCharacterDto>(character);
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = characters.FirstOrDefault(x => x.Id == updateCharacter.Id);

                if(character == null) {
                    throw new Exception($"Character with Id {updateCharacter.Id} not found");
                }

               character =  _mapper.Map(updateCharacter, character);

               response.Data = _mapper.Map<GetCharacterDto>(character);


            }
            catch(Exception ex) {
                response.Data = null;
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;

            }
            return response;
        }
    }
}