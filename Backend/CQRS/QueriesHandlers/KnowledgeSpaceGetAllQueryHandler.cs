using AutoMapper;
using Backend.CQRS.Queries;
using Backend.CQRS.QueriesResults;
using Backend.Data.Repositories.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.CQRS.QueriesHandlers
{
    public class KnowledgeSpaceGetAllQueryHandler : IRequestHandler<KnowledgeSpaceGetAllQuery, KnowledgeSpaceGetAllQueryResult>
    {
        private IKnowledgeSpaceRepository _knowledgeSpaceRepository;

        private IMapper _mapper;

        public KnowledgeSpaceGetAllQueryHandler(IKnowledgeSpaceRepository knowledgeSpaceRepository, IMapper mapper)
        {
            _knowledgeSpaceRepository = knowledgeSpaceRepository ?? throw new ArgumentNullException(nameof(knowledgeSpaceRepository));

            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<KnowledgeSpaceGetAllQueryResult> Handle(KnowledgeSpaceGetAllQuery request, CancellationToken cancellationToken)
        {
            var result = await _knowledgeSpaceRepository.GetKnowledgeSpacesOfProfessor(request.ProfessorId);
            return new KnowledgeSpaceGetAllQueryResult
            {
                KnowledgeSpaces = result
            };
        }
    }
}
