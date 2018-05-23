using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    /// <summary>
    /// For the purposes of this demo, the LocalMailService will be a dummy class with a mock send email method
    /// </summary>
    public class LocalMailService : IMailService
    {
        private string _mailTo = Startup.Configuration["mailSettings:mailToAddress"];
        private string _mailFrom = Startup.Configuration["mailSettings:mailFromAddress"];

        public void Send(string subject, string message)
        {
            Debug.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with LocalMailService");
            Debug.WriteLine($"Subject:  {subject}");
            Debug.WriteLine($"Message:  {message}");
        }
    }
}
