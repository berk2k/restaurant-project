﻿using System.Net;

namespace restaurant_backend.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public string Status { get; set; } = "SUCCESS";
        public bool IsSuccess { get; set; } = true;

        public string ErrorMessage { get; set; }

        public object Result { get; set; }
    }
}
