using AutoMapper;
using Backend.CQRS.Commands;
using Backend.CQRS.QueriesResults;
using Backend.Entities;

namespace Backend.Utils.Mapper
{
    public class Mapping : Profile
    {   
        public Mapping()
        {
            CreateMap<Test, TestView>().ReverseMap();
            CreateMap<ProfessorTestView, Professor>().ReverseMap();

            CreateMap<SubmitTestTest, Test>().ReverseMap();
            CreateMap<SubmitTestQuestion, Question>().ReverseMap();
            CreateMap<SubmitTestAnswer, Answer>().ReverseMap();

        }
    }
}
