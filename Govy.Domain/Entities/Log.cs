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
    public class Log
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }
        [Required]
        public String Pergunta { get; set; }
        [Required]
        public String Resposta { get; set; }
        [Display(Name = "Contador")]
        public int Count { get; set; }
    }
}
