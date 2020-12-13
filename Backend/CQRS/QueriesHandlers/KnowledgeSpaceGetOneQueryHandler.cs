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
    public class KnowledgeSpaceGetOneQueryHandler : IRequestHandler<KnowledgeSpaceGetOneQuery, KnowledgeSpaceGetOneQueryResult>
    {
        private IKnowledgeSpaceRepository _knowledgeSpaceRepository;

        private IMapper _mapper;

        public KnowledgeSpaceGetOneQueryHandler(IKnowledgeSpaceRepository knowledgeSpaceRepository, IMapper mapper)
        {
            _knowledgeSpaceRepository = knowledgeSpaceRepository ?? throw new ArgumentNullException(nameof(knowledgeSpaceRepository));

            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<KnowledgeSpaceGetOneQueryResult> Handle(KnowledgeSpaceGetOneQuery request, CancellationToken cancellationToken)
        {
            var resultExpectedKS = await _knowledgeSpaceRepository.GetSingleKnowledgeSpaceByIdWidthIncludes(request.KnowledgeSpaceId);
            var resultRealKSs = await _knowledgeSpaceRepository.GetAllRealKSOfOriginalKS(request.KnowledgeSpaceId);
            resultRealKSs.Insert(0, resultExpectedKS);
            return new KnowledgeSpaceGetOneQueryResult
            {
                KnowledgeSpaces = resultRealKSs
            };
        }
    }
}
