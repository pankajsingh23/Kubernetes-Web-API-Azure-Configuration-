using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Core.Configuration
{
    public class AppConfig : IAppConfig
    {
        private readonly IConfiguration _configuration;

        public AppConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetLocation()
        {
            return _configuration["AppSettings:Location"];
        }
        public string GetJwtSecret()
        {
           return _configuration["JwtSettings:Secret"];
        }
    }
}
