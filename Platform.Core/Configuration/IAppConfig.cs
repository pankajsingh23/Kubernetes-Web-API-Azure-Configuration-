using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Core.Configuration
{
    public interface IAppConfig
    {
        public string GetLocation();
        public string GetJwtSecret();
    }
}
