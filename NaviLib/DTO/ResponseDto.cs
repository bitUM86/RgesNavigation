using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviLib.DTO
{
    public class ResponseDto
    {
        public ResponseDto(string role, string name, string encodedJwt)
        {
            Role = role;
            Name = name;
            EncodedJwt = encodedJwt;
        }
        public string Role { get; }
        public string Name { get; }
        public string EncodedJwt { get; }
    }
}
