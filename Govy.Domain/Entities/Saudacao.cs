using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Govy.Domain.Entities
{
    public class Saudacao
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }

        [Required]
        [Display(Name = "Saudação")]
        public String Text { get; set; }
    }
}
