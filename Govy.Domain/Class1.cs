using System.Collections.Generic;
using MongoRepository;

namespace Govy.Domain
{


    public class Customer : Entity  //Inherit from Entity!
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Product> Products { get; set; }

        public Customer()
        {
            this.Products = new List<Product>();
        }
    }

    public class Product //No need to inherit from Entity; This object is not contained in
                         //it's "own" MongoDb document. It is only contained in a customer
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
