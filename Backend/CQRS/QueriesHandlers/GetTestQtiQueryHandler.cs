using AutoMapper;
using Backend.CQRS.Queries;
using Backend.CQRS.QueriesResults;
using Backend.Data.Repositories.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.CQRS.QueriesHandlers
{
    public class GetTestQtiQueryHandler : IRequestHandler<GetTestQtiQuery, GetTestQtiQueryResult>
    {
        private ITestRepository _testRepository;

        private IMapper _mapper;

        public GetTestQtiQueryHandler(ITestRepository testRepository, IMapper mapper)
        {
            _testRepository = testRepository ?? throw new ArgumentNullException(nameof(testRepository));

            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GetTestQtiQueryResult> Handle(GetTestQtiQuery request, CancellationToken cancellationToken)
        {
            var test = await _testRepository.GetSingleTestByIdWithIncludes(request.TestId);

            StringBuilder xml = new StringBuilder();
            xml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                $"<qti-assessment-test xmlns=\"http://www.imsglobal.org/xsd/imsqtiasi_v3p0\" xmlns:xsi = \"http://www.w3.org/2001/XMLSchema-instance\"" +
                $" xsi:schemaLocation = \"http://www.imsglobal.org/xsd/imsqtiasi_v3p0 https://purl.imsglobal.org/spec/qti/v3p0/schema/xsd/imsqti_testv3p0_v1p0.xsd\"" +
                $" identifier = \"{test.TestId}\" title = \"{test.Title}\" >\n<qti-assessment-items>\n");

            foreach(var question in test.Questions)
            {
                xml.Append("<qti-assessment-item xmlns=\"http://www.imsglobal.org/xsd/imsqtiasi_v3p0\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" ");
                xml.Append("xsi:schemaLocation = \"http://www.imsglobal.org/xsd/imsqtiasi_v3p0 https://purl.imsglobal.org/spec/qti/v3p0/schema/xsd/imsqti_asiv3p0_v1p0.xsd\" ");
                xml.Append($"identifier=\"{question.QuestionId}\" title=\"{question.Text}\" adaptive=\"false\" time-dependent=\"false\">");

                var correctPart = new StringBuilder($"\t<qti-response-declaration identifier=\"RESPONSE\" cardinality=\"single\" base-type=\"identifier\">\n\t\t<qti-correct-response>\n");

                var choicesPart = new StringBuilder($"\t<qti-outcome-declaration identifier=\"SCORE\" cardinality=\"single\" base-type=\"float\"/>\n\t<qti-item-body>\n" +
                    $"\t\t<qti-choice-interaction response-identifier=\"RESPONSE\" shuffle=\"true\" max-choices=\"0\">\n" +
                    $"\t\t\t<qti-prompt>${question.Text}</qti-prompt>");

                foreach (var answer in question.Answers)
                {
                    choicesPart.Append($"\t\t\t<qti-simple-choice identifier=\"{answer.AnswerId}\" fixed=\"false\">{answer.Text}</qti-simple-choice>");

                    if (answer.Correct)
                    {
                        correctPart.Append($"\t\t\t<qti-value>{answer.AnswerId}</qti-value>");
                    }
                }

                correctPart.Append("\t\t</qti-correct-response>\n\t</qti-response-declaration>\n");
                choicesPart.Append("\t\t</qti-choice-interaction>\n\t</qti-item-body>\n" +
                    "\t<qti-response-processing template=\"https://www.imsglobal.org/question/qti_v3p0/rptemplates/map_response.xml\"/>");

                xml.Append(correctPart);
                xml.Append(choicesPart);
                xml.Append("</qti-assessment-item>\n");
            }
            xml.Append("</qti-assessment-items>\n</qti-assessment-test>\n");


            return new GetTestQtiQueryResult
            {
                File = xml.ToString()
            };
        }
    }
}
