using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Core.Models.HandlerModels
{
    public class ErrorDetails
    {
        public string id { get { return Guid.NewGuid().ToString(); } }

        public int statusCode { get; set; }

        public string userFriendlyMessage { get; set; }

        public string exceptionMessage { get; set; }

        public string stackTrace { get; set; }

        public string queryCode { get; set; }

        public int errorType { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
