using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using PBS.Advice.API.DataModel;
using PBS.Advice.API.Helpers;
using PBS.Advice.API.Models;
using PBS.Advice.DataModel.Common;
using PBS.Claims.Extensions;
using Action = PBS.Advice.API.DataModel.Action;

namespace PBS.Advice.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/advice/jobs")]
   // [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class JobController : ApiController
    {
        private readonly IAdviceDbContext _adviceDbContext;

        public JobController(IAdviceDbContext adviceDbContext)
        {
            _adviceDbContext = adviceDbContext;
        }

        [Route("{jobID}")]
        public IHttpActionResult GetJobByID(long jobID)
        {
            try
            {
                //SGG: this feels wrong - get job with all its actions, which we don't return?
                Job job = _adviceDbContext.Jobs.FirstOrDefault(x => x.JobID == jobID 
                                                                    && x.Deleted == false 
                                                                    && x.Sensitive == false 
                                                                    && x.Sensitive == false 
                                                                    && x.DepartmentID == Department.Advice);

                if (job != null && !User.CanViewJobsForClientId(job.ClientID))
                {
                    return Unauthorized();
                }

                if (job == null)
                {
                    return NotFound();
                }                

                return Ok(job.ToJobModel());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("Client/{PBSClientID}")]    
        public IHttpActionResult GetJobsByClientID(long PBSClientID)
        {
                      
            if (!User.CanViewJobsForClientId(PBSClientID))
            {
                return Unauthorized();
            }

            var jobs = _adviceDbContext
                            .Jobs
                            .Where(x => x.ClientID == PBSClientID 
                                && x.Deleted == false 
                                && x.Sensitive == false
                                && x.DepartmentID == Department.Advice)
                            .ToList()
                            .Select(x => x.ToJobModel());

            return Ok(jobs);
           
        }

        [Route("Client/{PBSClientID}/Open")]
        public IHttpActionResult GetOpenJobsByClientID(long PBSClientID)
        {
            if (!User.CanViewJobsForClientId(PBSClientID))
            {
                return Unauthorized();
            }

            var jobs = _adviceDbContext
                            .Jobs
                            .Where(x => x.ClientID == PBSClientID 
                                && x.Closed == false 
                                && x.Deleted == false 
                                && x.Sensitive == false
                                && x.DepartmentID == Department.Advice)
                            .ToList()
                            .Select(x => x.ToJobModel());

            return Ok(jobs);
        }


        [Route("Client/{PBSClientID}/Closed")]
        public IHttpActionResult GetClosedJobsByClientID(long PBSClientID)
        {
                     
            if (!User.CanViewJobsForClientId(PBSClientID))
                {
                    return Unauthorized();
                }

            var jobs = _adviceDbContext
                             .Jobs
                             .Where(x => x.ClientID == PBSClientID 
                                 && x.Closed == true 
                                 && x.Deleted == false 
                                 && x.Sensitive == false
                                 && x.DepartmentID == Department.Advice)
                             .ToList()
                             .Select(x => x.ToJobModel());

            return Ok(jobs);
           
        }

        [Route("{jobID}/actions")]
        public IHttpActionResult GetActionsForJob(long jobID)
        {
            try
            {
                Job job = _adviceDbContext.Jobs.FirstOrDefault(x => x.JobID == jobID 
                                                                    && x.Deleted == false 
                                                                    && x.Sensitive == false
                                                                    && x.DepartmentID == Department.Advice);

                if (job != null && !User.CanViewJobsForClientId(job.ClientID))
                {
                    return Unauthorized();
                }

                if (job == null)
                {
                    return NotFound();
                }

                IEnumerable<ActionModel> actions =
                    _adviceDbContext
                        .Actions
                        .Where(a => a.JobID == jobID)
                        .ToList()
                        .Select(a => a.ToActionModel());

                return Ok(actions);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        
    }
}
