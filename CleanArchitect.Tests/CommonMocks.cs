using CleanArchitect.Application.Options;
using Microsoft.Extensions.Options;

namespace CleanArchitect.Tests
{
    public class CommonMocks
    {
        public static IOptions<ApplicationErrors> ApplicationErros { get { return Options.Create(new ApplicationErrors()); } }
    }
}
