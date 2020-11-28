using AutoMapper;
using Backend.CQRS.QueriesResults;
using Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Utils.Mapper
{
    public class Mapping : Profile
    {   
        public Mapping()
        {
            CreateMap<Test, TestView>().ReverseMap();
        }
    }
}
