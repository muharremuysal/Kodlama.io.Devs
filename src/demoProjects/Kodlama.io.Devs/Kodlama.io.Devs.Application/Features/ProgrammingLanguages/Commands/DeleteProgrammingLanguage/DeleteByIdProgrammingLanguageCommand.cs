using AutoMapper;
using Kodlama.io.Devs.Application.Features.ProgrammingLanguages.Commands.CreateProgrammingLanguage;
using Kodlama.io.Devs.Application.Features.ProgrammingLanguages.Commands.UpdateProgrammingLanguage;
using Kodlama.io.Devs.Application.Features.ProgrammingLanguages.Dtos;
using Kodlama.io.Devs.Application.Features.ProgrammingLanguages.Rules;
using Kodlama.io.Devs.Application.Services.Repositories;
using Kodlama.io.Devs.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodlama.io.Devs.Application.Features.ProgrammingLanguages.Commands.DeleteProgrammingLanguage
{
    public class DeleteByIdProgrammingLanguageCommand : IRequest<bool>    
    {
        public int Id { get; set; }

        public class DeleteByIdProgrammingLanguageCommandHandler : IRequestHandler<DeleteByIdProgrammingLanguageCommand, bool>
        {
            private readonly IProgrammingLanguageRepository _programmingLanguageRepository;
            private readonly IMapper _mapper;
            private readonly ProgrammingLanguageBusinessRules _programmingLanguageBusinessRules;

            public DeleteByIdProgrammingLanguageCommandHandler(IProgrammingLanguageRepository programmingLanguageRepository, IMapper mapper, ProgrammingLanguageBusinessRules programmingLanguageBusinessRules)
            {
                _programmingLanguageRepository = programmingLanguageRepository;
                _mapper = mapper;
                _programmingLanguageBusinessRules = programmingLanguageBusinessRules;
            }

            public async Task<bool> Handle(DeleteByIdProgrammingLanguageCommand request, CancellationToken cancellationToken)
            {
                //Request'ten gelen id ye göre Repository den Nesneyi Al
                ProgrammingLanguage? programmingLanguage = await _programmingLanguageRepository.GetAsync(p => p.Id == request.Id);

                //Nesnenin Varlığını Business Rules dan Kontrol Et 
                await _programmingLanguageBusinessRules.ProgrammingLanguageShouldExistWhenRequested(programmingLanguage);

                //Request => ProgrammingLanguage
                _mapper.Map<DeleteByIdProgrammingLanguageCommand, ProgrammingLanguage>(request, programmingLanguage);

                //Repository den Sil
                await _programmingLanguageRepository.DeleteAsync(programmingLanguage);
                return true;
            }
        }
    }
}
