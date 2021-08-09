using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Core.Models
{
    public class GenericResponse<T> where T : class
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public T Model { get; set; }
        public List<T> ListModel { get; set; }
    }
}
