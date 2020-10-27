using AzureFunctions.Extensions.Swashbuckle;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using OpenNotes_API;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

[assembly: WebJobsStartup(typeof(SwashBuckleStartup))]
namespace OpenNotes_API
{
    internal class SwashBuckleStartup : IWebJobsStartup
    {
        [Obsolete]
        public void Configure(IWebJobsBuilder builder)
        {
            //Register the extension
            builder.AddSwashBuckle(Assembly.GetExecutingAssembly());
        }
    }
}
