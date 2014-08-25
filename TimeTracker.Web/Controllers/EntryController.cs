using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http;
using TimeTracker.DataTransferObjects;
using TimeTracker.Services;

namespace TimeTracker.Web.Controllers
{
    [Authorize]
    public class EntryController : ApiController
    {
        private readonly IEntryService _entryService;
        private readonly ISecurityService _securityService;
        private readonly IPrincipal _principal;
        private readonly int _userId;

        public EntryController(IEntryService entryService, IPrincipal principal, ISecurityService securityService)
        {
            _entryService = entryService;
            _principal = principal;
            _securityService = securityService;
            _userId = securityService.GetUserId(principal.Identity.Name);
        }

        public IList<dtoEntryHeader> Get(string weekStartDate, string weekEndDate)
        {
            return _entryService.GetEntries(Convert.ToDateTime(weekStartDate), Convert.ToDateTime(weekEndDate), _userId);
        }
        
        public HttpResponseMessage CreateEntryHeader([FromBody]dtoEntryHeader entryHeader)
        {
            // TODO: Handle creation of new task.
            // TODO: Validation if task already exist for the selected week.

            entryHeader.Task_Id = _entryService.GetTask(entryHeader.Task.Name, _userId).Id;
            entryHeader.Task = null;
            entryHeader.User_Id = _userId;
            //

            var output = _entryService.CreateEntryHeader(entryHeader);
            if (string.IsNullOrEmpty(output))
            {
                var response = Request.CreateResponse(HttpStatusCode.Created);
                return response;
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(output)
                    });
            }
        }
        
        [HttpPost]
        public HttpResponseMessage DeleteEntryHeader([FromBody]int entryHeaderId)
        {
            var output = _entryService.DeleteEntryHeader(entryHeaderId);
            
            return Request.CreateResponse<bool>(HttpStatusCode.OK, output);
        }
        
        public HttpResponseMessage CreateEntry(dtoEntry entry)
        {
            var output = _entryService.CreateEntry(entry);

            if (output)
            {
                var response = Request.CreateResponse(HttpStatusCode.Created);
                return response;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        public void UpdateEntry(dtoEntry entry)
        {
            var output = _entryService.UpdateEntry(entry);
            
            if(!output)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
        }

        public HttpResponseMessage GetGridColumns(string startDate)
        {
            var output = _entryService.GetGridColumns(Convert.ToDateTime(startDate));
            var response = Request.CreateResponse<List<dtoGridColumn>>(HttpStatusCode.OK, output);
            return response;
        }

        public List<string> GetTaskList()
        {
            var output = _entryService.GetTaskList(_userId);

            return output;
        }
    }
}
