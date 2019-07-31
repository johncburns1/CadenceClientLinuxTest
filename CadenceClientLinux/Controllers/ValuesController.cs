using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CadenceClientLinux.Cadence;
using Microsoft.Extensions.Options;
using System.Text;

namespace CadenceClientLinux.Controllers
{
    public class ValuesController : BaseController
    {
        public ValuesController(Client client)
            : base(client)
        {

        }

        // GET v1/execute
        [HttpGet("execute")]
        public async Task<ActionResult<string>> Execute()
        {
            // cadence client instance
            var response = await _client.HelloWorld_Workflow_ByName();
            if (response.CompletedWithSuccess)
            {
                return Ok(Encoding.UTF8.GetString(response.Result));
            }

            return BadRequest($"An error occurred while running HelloWorld Workflow, ERROR: { response.FormattedErrorMessages }");
        }

        // GET v1/register
        [HttpGet("register")]
        public async Task<ActionResult<string>> Register()
        {
            // cadence client instance
            var response = await _client.Register();
            if (response.CompletedWithSuccess)
            {
                return Ok(Encoding.UTF8.GetString(response.Result));
            }

            return BadRequest($"An error occurred while running HelloWorld Workflow, ERROR: { response.FormattedErrorMessages }");
        }

        // GET v1/startworker
        [HttpGet("startworker")]
        public async Task<ActionResult<string>> StartWorker()
        {
            // cadence client instance
            var response = await _client.StartWorker();
            if (response.CompletedWithSuccess)
            {
                return Ok(Encoding.UTF8.GetString(response.Result));
            }

            return BadRequest($"An error occurred while running HelloWorld Workflow, ERROR: { response.FormattedErrorMessages }");
        }

        // Get v1/terminate
        [HttpGet("terminate")]
        public ActionResult<string> Terminate()
        {
            // cadence client instance
            var response = _client.Terminate();
            
            return Ok(Encoding.UTF8.GetString(response.Result));
        }
    }
}
