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
    public class Duvida
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }

        [Required]
        public String Pergunta { get; set; }
        public String Resposta { get; set; }
    }
}
