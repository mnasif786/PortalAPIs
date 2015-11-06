using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using PBS.Advice.API.Controllers;
using PBS.Advice.API.DataModel;
using PBS.Advice.API.Models;
using PBS.Advice.DataModel.Common;
using Action = PBS.Advice.API.DataModel.Action;

namespace PBS.Advice.API.Tests
{

    public static class TestData
    {
        public static readonly List<Job> JobDataList = new List<Job>
        {
            new Job() {JobID = 111, ContactID = 1, Subject = "Test Job 1", CreatedBy = "", Closed = false, LastActionId = 3,      ClientID = 1234, Deleted = false,
                LastActionDate = new DateTime(2015, 1, 1 ), CreatedDate = new DateTime(2015, 1, 10 ), DepartmentID = Department.Advice},
            new Job() {JobID = 222, ContactID = 2, Subject = "Test Job 2", CreatedBy = "", Closed = false, LastActionId = null,   ClientID = 888 , Deleted = false,
                LastActionDate = new DateTime(2015, 2, 2 ), CreatedDate = new DateTime(2015, 2, 20 ), DepartmentID = Department.Advice },
            new Job() {JobID = 333, ContactID = 3, Subject = "Test Job 3", CreatedBy = "", Closed = true, LastActionId = null,   ClientID = 1234, Deleted = false,
                LastActionDate = new DateTime(2015, 3, 3 ), CreatedDate = new DateTime(2015, 3, 30 ), DepartmentID = Department.Advice },

            new Job() {JobID = 444, ContactID = 3, Subject = "Test Job 4 - DELETED JOB", CreatedBy = "", Closed = true, LastActionId = null,   ClientID = 1234, Deleted = true,
                LastActionDate = new DateTime(2015, 3, 3 ), CreatedDate = new DateTime(2015, 3, 30 ), DepartmentID = Department.Advice },


            new Job() {JobID = 555, ContactID = 3, Subject = "Test Job 5 - SENSITIVE JOB", CreatedBy = "", Closed = false, LastActionId = null,   ClientID = 1234, Deleted = false,
                LastActionDate = new DateTime(2015, 3, 3 ), CreatedDate = new DateTime(2015, 3, 30 ), Sensitive = true, DepartmentID = Department.Advice },

            new Job() {JobID = 555, ContactID = 3, Subject = "Test Job 6 - HEALTH AND SAFETY JOB", CreatedBy = "", 
                Closed = false, LastActionId = null,   ClientID = 1234, Deleted = false,
                LastActionDate = new DateTime(2015, 3, 3 ), CreatedDate = new DateTime(2015, 3, 30 ), Sensitive = false, DepartmentID = Department.HealthAndSafety },

            new Job() {JobID = 555, ContactID = 3, Subject = "Test Job 7 - TAXWISE JOB", 
                CreatedBy = "", Closed = false, LastActionId = null,   ClientID = 1234, Deleted = false,
                LastActionDate = new DateTime(2015, 3, 3 ), CreatedDate = new DateTime(2015, 3, 30 ), Sensitive = false,
                DepartmentID = Department.Taxwise}
        };


        public static List<ActionType> ActionTypes = new List<ActionType>()
        {
            new ActionType() {ActionTypeID = 0, Description = "Action Type Zero"},
            new ActionType() {ActionTypeID = 1, Description = "Action Type One"}
        };

        public static readonly List<Action> ActionDataList = new List<Action>
        {
            new Action() {
                            ActionID = 1, 
                            JobID = 111,  
                            AdviceGiven = "One for the money", 
                            ActionType = ActionTypes[0],   
                            StartDate = new DateTime(2012, 1, 1),
                            CompletedDate = new DateTime(2012, 3, 2),
                            GeneralNotes = "Test general notes",
                            InternalNotes = "Test internal notes",
                            DocumentCount = 1             
                        },

            new Action() {  ActionID = 2, 
                            JobID = 111,                              
                            ActionType = ActionTypes[1],   
                            AdviceGiven = "Two For The Show"   },

            new Action() { ActionID = 3, JobID = 111,   ActionType = ActionTypes[0],     AdviceGiven = "Three to get ready" }
        };

       
         

         

        public static void AddClaimsPrincipal(this JobController controller, Dictionary<string, string> claims = null)
        {
            var cp = new Mock<ClaimsPrincipal>();

            cp.Setup( m => m.HasClaim(It.IsAny<string>(), It.IsAny<string>()) )
                .Returns((string key, string value) => Validate(key, value, claims));

            controller.User = cp.Object;
        }

        private static bool Validate(string key, string value, Dictionary<string, string> claims)
        {
            if (claims == null)
                return true;
           
            return (claims[key] == value);            
        }        
    }


    [TestFixture]
    public class JobControllerTests
    {
        private Mock<IAdviceDbContext> _mockAdviceDbContext;

    
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            // set up data

            // JOBS
            var mockJobs = new Mock<DbSet<Job>>();
            var jobData = TestData.JobDataList.AsQueryable();

            mockJobs.As<IQueryable<Job>>().Setup(m => m.Provider).Returns(jobData.Provider);
            mockJobs.As<IQueryable<Job>>().Setup(m => m.Expression).Returns(jobData.Expression);
            mockJobs.As<IQueryable<Job>>().Setup(m => m.ElementType).Returns(jobData.ElementType);
            mockJobs.As<IQueryable<Job>>().Setup(m => m.GetEnumerator()).Returns(jobData.GetEnumerator);
            
            // ACTIONS
            var mockActions = new Mock<DbSet<Action>>();
            var actionData = TestData.ActionDataList.AsQueryable();
            mockActions.As<IQueryable<Action>>().Setup(m => m.Provider).Returns(actionData.Provider);
            mockActions.As<IQueryable<Action>>().Setup(m => m.Expression).Returns(actionData.Expression);
            mockActions.As<IQueryable<Action>>().Setup(m => m.ElementType).Returns(actionData.ElementType);
            mockActions.As<IQueryable<Action>>().Setup(m => m.GetEnumerator()).Returns(actionData.GetEnumerator);

            // ACTIONTYPES
            //var mockActionTypes = new Mock<DbSet<ActionType>>();
            //var actionData = TestData.ActionDataList.AsQueryable();
            

            // ADVICEDBCONTEXT
            _mockAdviceDbContext = new Mock<IAdviceDbContext>();
            _mockAdviceDbContext.Setup(m => m.Jobs).Returns(mockJobs.Object);
            _mockAdviceDbContext.Setup(m => m.Actions).Returns(mockActions.Object);
        }


        [Test]
        public void Given_Job_Exists_When_GetJobsByID_Then_Return_Job()
        {
            JobController controller = GetTarget();
            controller.AddClaimsPrincipal();


            IHttpActionResult actionResult = controller.GetJobByID(TestData.JobDataList[0].JobID);
            OkNegotiatedContentResult<JobModel> contentResult = actionResult as OkNegotiatedContentResult<JobModel>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);

            JobModel result = contentResult.Content;
            Job expected = TestData.JobDataList[0];

            Assert.AreEqual(result.JobId, expected.JobID);
            Assert.AreEqual(result.ContactId, expected.ContactID);                   
            Assert.AreEqual(result.Subject, expected.Subject);
            Assert.AreEqual(result.CreatedBy, expected.CreatedBy);
            Assert.AreEqual(result.CreatedDate, expected.CreatedDate);
            Assert.AreEqual(result.Closed, expected.Closed);
            Assert.AreEqual(result.LastActionId, expected.LastActionId);
            Assert.AreEqual(result.LastActionDate, expected.LastActionDate);
            Assert.AreEqual(result.ClientID, expected.ClientID);         
        }

        [Test]
        public void Given_Job_Exists_But_Is_Deleted_When_GetJobsByID_Then_No_Job_Returned()
        {
            JobController controller = GetTarget();
            controller.AddClaimsPrincipal();

            IHttpActionResult actionResult = controller.GetJobByID(TestData.JobDataList[3].JobID);
            OkNegotiatedContentResult<JobModel> contentResult = actionResult as OkNegotiatedContentResult<JobModel>;

            Assert.IsNull(contentResult);            
        }

        [Test]
        public void Given_Jobs_Exist_For_Client_When_GetJobsByID_WithoutClaim_Then_Return_Unauthorised()
        {
            JobController controller = GetTarget();

            Dictionary<string,string> claims = new Dictionary<string, string>();
            claims.Add(PBS.Claims.ClaimTypes.PBSClientId, "111");
            
            controller.AddClaimsPrincipal( claims );

            IHttpActionResult actionResult = controller.GetJobByID(222);
            Assert.IsInstanceOf<UnauthorizedResult>(actionResult);
         }

        [Test]
        public void Given_Job_Exists_For_Client_When_GetJobsByID_WithClaim_Then_Return_Job()
        {
            JobController controller = GetTarget();

            Dictionary<string, string> claims = new Dictionary<string, string>();
            claims.Add(PBS.Claims.ClaimTypes.PBSClientId, "888");

            controller.AddClaimsPrincipal(claims);

            IHttpActionResult actionResult = controller.GetJobByID(222);

            Assert.IsInstanceOf<OkNegotiatedContentResult<JobModel>>(actionResult);
        }  

    
        [Test]
        public void Given_Open_And_Closed_Jobs_Exist_For_Client_When_GetJobsByClientID_Called_Then_Return_All_Jobs()
        {
            JobController controller = GetTarget();

            Dictionary<string, string> claims = new Dictionary<string, string>();
            claims.Add(PBS.Claims.ClaimTypes.PBSClientId, "1234");

            controller.AddClaimsPrincipal(claims);

            IHttpActionResult actionResult = controller.GetJobsByClientID(1234);

            Assert.IsInstanceOf<OkNegotiatedContentResult<IEnumerable<JobModel>>>(actionResult);


            OkNegotiatedContentResult<IEnumerable<JobModel>> contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<JobModel>>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);

            IEnumerable<JobModel> result = contentResult.Content;

            Assert.AreEqual(2, result.Count());
        }

       
        [Test]
        public void Given_Open_Job_Exists_For_Client_When_GetOpenJobs_Called_Then_Return_Job()
        {
            JobController controller = GetTarget();

            Dictionary<string, string> claims = new Dictionary<string, string>();
            claims.Add(PBS.Claims.ClaimTypes.PBSClientId, "1234");

            controller.AddClaimsPrincipal(claims);

            IHttpActionResult actionResult = controller.GetOpenJobsByClientID(1234);

            Assert.IsInstanceOf<OkNegotiatedContentResult<IEnumerable<JobModel>>>(actionResult);


            OkNegotiatedContentResult<IEnumerable<JobModel>> contentResult =
                actionResult as OkNegotiatedContentResult<IEnumerable<JobModel>>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);

            IEnumerable<JobModel> result = contentResult.Content;

            Assert.AreEqual(1, result.Count());
            JobModel model = result.First();


            Job expected = TestData.JobDataList[0];


            Assert.AreEqual(model.JobId, expected.JobID);
            Assert.AreEqual(model.ContactId, expected.ContactID);
            Assert.AreEqual(model.Subject, expected.Subject);
            Assert.AreEqual(model.CreatedBy, expected.CreatedBy);
            Assert.AreEqual(model.Closed, expected.Closed);
            Assert.AreEqual(model.LastActionId, expected.LastActionId);
            Assert.AreEqual(model.ClientID, expected.ClientID);
        }

        [Test]
        public void Given_Closed_Job_Exists_For_Client_When_GetOpenJobs_Called_Then_Return_Job()
        {
            JobController controller = GetTarget();

            Dictionary<string, string> claims = new Dictionary<string, string>();
            claims.Add(PBS.Claims.ClaimTypes.PBSClientId, "1234");

            controller.AddClaimsPrincipal(claims);

            IHttpActionResult actionResult = controller.GetClosedJobsByClientID(1234);

            Assert.IsInstanceOf<OkNegotiatedContentResult<IEnumerable<JobModel>>>(actionResult);


            OkNegotiatedContentResult<IEnumerable<JobModel>> contentResult =
                actionResult as OkNegotiatedContentResult<IEnumerable<JobModel>>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);

            IEnumerable<JobModel> result = contentResult.Content;

            Assert.AreEqual(1, result.Count());
            JobModel model = result.First();

            Job expected = TestData.JobDataList[2];


            Assert.AreEqual(model.JobId,        expected.JobID);
            Assert.AreEqual(model.ContactId,    expected.ContactID);
            Assert.AreEqual(model.Subject,      expected.Subject);
            Assert.AreEqual(model.CreatedBy,    expected.CreatedBy);
            Assert.AreEqual(model.Closed,       expected.Closed);
            Assert.AreEqual(model.LastActionId, expected.LastActionId);
            Assert.AreEqual(model.ClientID,     expected.ClientID);
        }


        [Test]
        public void Given_Actions_Exist_For_Job_When_GetActionsForJob_Then_Return_Actions()
        {
            JobController controller = GetTarget();

            Dictionary<string, string> claims = new Dictionary<string, string>();
            claims.Add(PBS.Claims.ClaimTypes.PBSClientId, "1234");
            controller.AddClaimsPrincipal(claims);



            IHttpActionResult actionResult = controller.GetActionsForJob(111);
            Assert.IsInstanceOf<OkNegotiatedContentResult<IEnumerable<ActionModel>>>(actionResult);


            OkNegotiatedContentResult<IEnumerable<ActionModel>> contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<ActionModel>>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);

            IEnumerable<ActionModel> result = contentResult.Content;

            Assert.AreEqual(3, result.Count());

            Action expected = TestData.ActionDataList[0];
            ActionModel resultModel = result.First();


            Assert.AreEqual(resultModel.ActionId,       expected.ActionID);
            Assert.AreEqual(resultModel.AdviceGiven,    expected.AdviceGiven);
            Assert.AreEqual(resultModel.ActionTypeID,   expected.ActionType.ActionTypeID);
            Assert.AreEqual(resultModel.ActionType,     expected.ActionType.Description);            
            Assert.AreEqual(resultModel.StartDate,      expected.StartDate);
            Assert.AreEqual(resultModel.CompletedDate,  expected.CompletedDate);
            Assert.AreEqual(resultModel.GeneralNotes,   expected.GeneralNotes);
            Assert.AreEqual(resultModel.InternalNotes,  expected.InternalNotes);
            Assert.AreEqual(resultModel.DocumentCount,  expected.DocumentCount);
            Assert.AreEqual(resultModel.CreatedBy,      expected.CreatedBy);
            Assert.AreEqual(resultModel.CreatedDate,    expected.CreatedDate);

            
        }



        [Test]
        public void Given_Job_Exists_And_Is_Marked_As_Sensitive_When_GetJobs_Then_Job_Is_Not_Returned()
        {
            JobController controller = GetTarget();

            Dictionary<string, string> claims = new Dictionary<string, string>();
            claims.Add(PBS.Claims.ClaimTypes.PBSClientId, "1234");

            controller.AddClaimsPrincipal(claims);

            IHttpActionResult actionResult = controller.GetJobsByClientID(1234);

            Assert.IsInstanceOf<OkNegotiatedContentResult<IEnumerable<JobModel>>>(actionResult);


            OkNegotiatedContentResult<IEnumerable<JobModel>> contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<JobModel>>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);

            IEnumerable<JobModel> result = contentResult.Content;

            Assert.AreEqual(2, result.Count());
        }

         public JobController GetTarget()
        {
            return new JobController(_mockAdviceDbContext.Object);                                     
        }
    }
}
