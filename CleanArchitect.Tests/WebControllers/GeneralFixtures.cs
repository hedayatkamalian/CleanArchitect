using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace CleanArchitect.Tests.WebControllers
{
    public class GeneralFixtures
    {
        public static Mock<IUrlHelper> CreateMockUrlHelper()
        {
            UrlActionContext actual = null;
            var userId = new Guid();

            var actionContext = new ActionContext
            {
                ActionDescriptor = new ActionDescriptor(),
                RouteData = new RouteData(),
            };

            var urlHelper = new Mock<IUrlHelper>();
            urlHelper.SetupGet(h => h.ActionContext).Returns(actionContext);
            urlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>()))
                .Callback((UrlActionContext context) => actual = context);

            return urlHelper;
        }
    }
}
