using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

using PBS.Portal.API.Common;
using PBS.Portal.API.Data.CommandHandlers;
using PBS.Portal.API.Data.Commands;
using PBS.Portal.API.Data.Queries;
using PBS.Portal.API.Data.QueriesHandlers;
using PBS.Portal.API.DataModel;

namespace PBS.Portal.API.Tests.CommandHandlerTests
{
    [TestFixture]
    public class SendRegistrationEmailCommandHandlerTests
    {
        //private Mock<IApplicationDbContext> _mockDbContext;

        //[TestFixtureSetUp]
        //public void TestSetup()
        //{   
        //    //// DB CONTEXT
        //    _mockDbContext = new Mock<IApplicationDbContext>();


        //}


        //[Test]
        //public void Given_CAN_When_Value_is_Demo1_then_Peninsula_Client_with_client_id_1_is_returned()
        //{
        //    var handler = new SendRegistrationEmailCommandHandler(_mockDbContext.Object);
        //    var cmd = new SendRegistrationEmailCommand() { UserName = "" };

        //    handler.Execute(cmd);
            
        //}

    }  
}
