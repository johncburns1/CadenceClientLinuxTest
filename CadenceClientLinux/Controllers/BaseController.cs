using Microsoft.AspNetCore.Mvc;
using Neon.Web;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CadenceClientLinux.Cadence;

namespace CadenceClientLinux.Controllers
{
    /// <summary>
    /// Implements the service base controller.
    /// </summary>
    [Route("/v1")]
    public class BaseController : NeonController
    {
        /// <summary>
        /// Instance of the Cadence.Client class. 
        /// </summary>
        protected readonly Client _client;

        /// <summary>
        /// Constructor for the ValuesController class.  createa a new instance of cadence client.
        /// </summary>
        public BaseController(Client client)
        {
            this._client = client;
        }
    }
}
