﻿namespace CoreApp.Utilities.Dtos
{
    public class GenericResult
    {
        public object Data { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }

        public object Error { get; set; }

        public GenericResult()
        {
        }

        public GenericResult(bool success)
        {
            Success = success;
        }

        public GenericResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public GenericResult(bool success, object data)
        {
            Success = success;
            Data = data;
        }
        
    }
}