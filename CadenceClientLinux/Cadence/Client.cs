using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Neon.Cadence;
using CadenceClientLinux.Controllers;
using Neon.Net;
using System.Threading;

namespace CadenceClientLinux.Cadence
{
    public sealed class Client : IDisposable
    {
        private readonly TimeSpan warmupDelay   = TimeSpan.FromSeconds(2);      // Time to allow Cadence to start

        public async Task<HubResponse<byte[]>> HelloWorld_Workflow_ByName()
        {
            HubResponse<byte[]> response = new HubResponse<byte[]>();

            // Run a workflow passing NULL args.

            WorkflowRun workflowRun;
            byte[] result;
            try
            {
                workflowRun = await client.StartWorkflowAsync("hello-workflow-by-name", args: null, domain: "test-domain");
                result = await client.GetWorkflowResultAsync(workflowRun);
            }

            catch (Exception ex)
            {
                return response.AddErrorMessage($"Workflow Failed: {ex.Message}");
            }
                
            if (result == null) {
                return response.AddErrorMessage("Result is null");
            }

            if (!Encoding.UTF8.GetString(result).Equals("workflow: Hello World!"))
            {
                return response.AddErrorMessage($"result does not match.  Expected: \"workflow: Hello World!\", Actual: \"{Encoding.UTF8.GetString(result)}\"");
            }

            // Run a workflow passing a string argument.

            var args = Encoding.UTF8.GetBytes("custom args");

            try
            {
                workflowRun = await client.StartWorkflowAsync("hello-workflow-by-name", args: args, domain: "test-domain");
                result = await client.GetWorkflowResultAsync(workflowRun);
            }
                    
            catch (Exception ex)
            {
                return response.AddErrorMessage($"Workflow Failed: {ex.Message}");
            }

            if (result == null)
            {
                return response.AddErrorMessage("Result is null");
            }

            if (!args.SequenceEqual(result))
            {
                return response.AddErrorMessage($"result does not match.  Expected: ${args}, Actual: {result}");
            }

            response.Result = result;
            return response;
        }

        public async Task<HubResponse<byte[]>> Register()
        {
            HubResponse<byte[]> response = new HubResponse<byte[]>();

            await client.RegisterDomainAsync("test-domain", ignoreDuplicates: true);
            await client.RegisterWorkflowAsync<HelloWorkflowByName>("hello-workflow-by-name");

            response.Result = Encoding.UTF8.GetBytes("successfully registered domains");
            return response;
        }

        public async Task<HubResponse<byte[]>> StartWorker()
        {
            HubResponse<byte[]> response = new HubResponse<byte[]>();

            await client.StartWorkerAsync("test-domain");

            response.Result = Encoding.UTF8.GetBytes("successfully started worker on test-domain");
            return response;
        }

        public HubResponse<byte[]> Terminate()
        {
            HubResponse<byte[]> response = new HubResponse<byte[]>();

            client.Dispose();

            response.Result = Encoding.UTF8.GetBytes("terminating client");
            return response;
        }

        CadenceClient client;
        HttpClient proxyClient;

        public Client()
        {
            var settings            = new CadenceSettings()
            {
                Mode                = ConnectionMode.ListenOnly,
                Debug               = true,
                ProxyTimeoutSeconds = 2000,
            };

            Thread.Sleep(warmupDelay);

            // Initialize the settings.

            settings = settings ?? new CadenceSettings()
            {
                CreateDomain = true
            };

            if (string.IsNullOrEmpty(settings.DefaultDomain))
            {
                settings.DefaultDomain = "test-domain";
            }

            settings.Servers.Clear();
            var cadenceHostIP = System.Environment.GetEnvironmentVariable("CADENCE_HOST_IP");
            if (cadenceHostIP == null)
            {
                settings.Servers.Add($"http://localhost:{NetworkPorts.Cadence}");
            }
            else
            {
                settings.Servers.Add($"http://{System.Environment.GetEnvironmentVariable("CADENCE_HOST_IP")}");
            }
                
            // Establish the Cadence connection.

            this.client = CadenceClient.ConnectAsync(settings).Result;
            this.proxyClient = new HttpClient() { BaseAddress = this.client.ProxyUri };
        }

        public void Dispose()
        {
            if (proxyClient != null)
            {
                proxyClient.Dispose();
                proxyClient = null;
            }
        }

        /// <summary>
        /// This workflow does the same thing as <see cref="HelloWorkflow"/> except that it
        /// executes the child activities by name and not type.
        /// </summary>
        private class HelloWorkflowByName : WorkflowBase
        {
            protected async override Task<byte[]> RunAsync(byte[] args)
            {
                string arg = null;

                if (args != null)
                {
                    arg = Encoding.UTF8.GetString(args);
                }

                if (arg == null)
                {
                    return await Task.FromResult(Encoding.UTF8.GetBytes("workflow: Hello World!"));
                }
                else if (arg == "activity")
                {
                    return await CallActivityAsync("hello-activity", Encoding.UTF8.GetBytes("activity: Hello World!"));
                }
                else if (arg == "local-activity")
                {
                    // NOTE: It's not possible to call local activities by name so we'll
                    // use the type here instead.

                    return await CallLocalActivityAsync<HelloActivity>(Encoding.UTF8.GetBytes("local-activity: Hello World!"));
                }
                else
                {
                    return await Task.FromResult(Encoding.UTF8.GetBytes(arg));
                }
            }
        }

        /// <summary>
        /// Test activity that returns the argument passed.
        /// </summary>
        private class HelloActivity : ActivityBase
        {
            protected async override Task<byte[]> RunAsync(byte[] args)
            {
                return await Task.FromResult(args);
            }
        }
    }
}
