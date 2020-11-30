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
            CreateMap<Test, StudentGetAllTestsQueryResult.Test>().ReverseMap();
            CreateMap<StudentGetAllTestsQueryResult.Professor, Professor>().ReverseMap();

            CreateMap<SubmitTestTest, Test>().ReverseMap();
            CreateMap<SubmitTestQuestion, Question>().ReverseMap();
            CreateMap<SubmitTestAnswer, Answer>().ReverseMap();

            CreateMap<Test, StudentGetOneTestQueryResult.TestView>().ReverseMap();
            CreateMap<StudentGetOneTestQueryResult.Professor, Professor>().ReverseMap();
            CreateMap<StudentGetOneTestQueryResult.Question, Question>().ReverseMap();
            CreateMap<StudentGetOneTestQueryResult.Answer, Answer>().ReverseMap();


        }
    }
}
