using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace PBS.Portal.API.Tests.Helpers
{
    public static class AdUsers
    {
        public static IPrincipal Create(string userName, bool isAuthenticated)
        {
            //Mocking Ad User
            var identity = new Mock<IIdentity>();
            identity.Setup(x => x.Name).Returns(userName);
            identity.Setup(x => x.IsAuthenticated).Returns(isAuthenticated);

            var principal = new Mock<IPrincipal>();
            principal.Setup(x => x.Identity).Returns(identity.Object);

           return principal.Object;
        }
    }
}
