using Contracts.Domain;
using Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Services.Web
{
    public class PetShopService : IPetShopService
    {
        public List<Cat> All()
        {
            // dummy data source
            return new List<Cat>
            {
                new Cat
                {
                    Breed = "Birman",
                    Name = "Fluffy",
                    Descendants = new List<Cat> {}
                }
            };
        }
    }
}
