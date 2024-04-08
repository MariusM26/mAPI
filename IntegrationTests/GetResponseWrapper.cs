using mAPI.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public class GetResponseWrapper
    {
        public int HttpStatusCode { get; set; }

        public IEnumerable<DCandidate>? Value { get; set; }
    }
}
