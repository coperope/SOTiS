using Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.CQRS.QueriesResults
{
    public class KnowledgeSpaceGetAllQueryResult : IQueryResult
    {
        public List<KnowledgeSpace> KnowledgeSpaces { get; set; }
    }
}
