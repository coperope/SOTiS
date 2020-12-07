using Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.CQRS.QueriesResults
{
    public class KnowledgeSpaceGetOneQueryResult : IQueryResult
    {
        public KnowledgeSpace KnowledgeSpace { get; set; }
    }
}
