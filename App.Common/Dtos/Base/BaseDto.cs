using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace App.Common.Dtos.Base
{
    public class BaseDto
    {
        [JsonIgnore]

        public Guid Id { get; set; }=new Guid();
        [JsonIgnore]
        public string? CreatedBy { get; set; }
        [JsonIgnore]

        public DateTime? CreatedOn { get; set; }
        [JsonIgnore]

        public string? UpdatedBy { get; set; }
        [JsonIgnore]

        public DateTime? UpdatedOn { get; set; }
    }
}
