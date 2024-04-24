using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.DTO
{
    public class ResultDto
    {
        public object? Data { get; set; }
        public bool value { get; set; } = true;
        public string Message { get; set; } = "";

    }
}
