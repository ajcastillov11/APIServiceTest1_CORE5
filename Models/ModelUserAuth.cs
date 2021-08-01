using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceAPITest.Models
{
    public class ModelUserAuth
    {
        public string UserName { get; set; }
        public string BearerToken { get; set; }
        public bool IsAuthenticated { get; set; }
        public ModelUserAuth() : base()
        {
            UserName = "Not Authorized";
            BearerToken = Guid.NewGuid().ToString();
            IsAuthenticated = false;
        }

    }
}
