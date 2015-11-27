using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zpi2WebApiWithLib.Models
{
    public class ResponseModel
    {
        public bool Result { get; set; }
        public string Data { get; set; }

        public static ResponseModel ErrorResponse(string data)
        {
            return new ResponseModel()
            {
                Result = false,
                Data = data
            };
        }

        public static ResponseModel SuccessResponse(string data)
        {
            return new ResponseModel()
            {
                Result = true,
                Data = data
            };
        }
    }
}