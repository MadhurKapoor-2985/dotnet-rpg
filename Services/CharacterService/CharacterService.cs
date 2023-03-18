using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Character;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            this._context = context;
            this._mapper = mapper;
        }
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var character = _mapper.Map<Character>(newCharacter);
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            ServiceResponse<List<GetCharacterDto>> response = new ServiceResponse<List<GetCharacterDto>>();
            response.Data = _context.Characters.Select(x => _mapper.Map<GetCharacterDto>(x)).ToList();
            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> response = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var character = await _context.Characters.FirstOrDefaultAsync(x => x.Id == id);

                if(character == null) {
                    throw new Exception($"Character with Id {id} not found");
                }
                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();
               response.Data = await _context.Characters.Select(x => _mapper.Map<GetCharacterDto>(x)).ToListAsync();;


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
            response.Data = await _context.Characters.Select(x => _mapper.Map<GetCharacterDto>(x)).ToListAsync();
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();
            var character = await _context.Characters.FirstOrDefaultAsync(x => x.Id == id);
            response.Data = _mapper.Map<GetCharacterDto>(character);
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await _context.Characters.FirstOrDefaultAsync(x => x.Id == updateCharacter.Id);

                if(character == null) {
                    throw new Exception($"Character with Id {updateCharacter.Id} not found");
                }

               character =  _mapper.Map(updateCharacter, character);
               await _context.SaveChangesAsync();
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