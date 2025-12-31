using Azure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Shared.DTOs
{
    public class ApiResponse
    {
        public bool Success {  get; set; }
        public string Message { get; set; }
        public object Data {  get; set; }   
        public static ApiResponse Ok(object data, string message = "Thành công")
        {
            return new ApiResponse 
            {
                Success = true,
                Message = message,
                Data = data 
            };
        }

        public static ApiResponse Fail(string message)
        {
            return new ApiResponse 
            {
                Success=false,
                Message = message,
                Data = null 
            };
        }
    }
}
