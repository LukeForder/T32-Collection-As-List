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
                new Cat("Fluffy")
                {
                    Breed = "Birman",
                }
            };
        }
        
        public List<Cat> Cyclic()
        {
            var catA = new Cat
            {
            };

            var catB = new Cat
            {
            };

            var catC = new Cat
            {
            };

            catC.AddDescendant(catA);
            catA.AddDescendant(catC);

            return new List<Cat>
            {
                catA,
                catB,
                catC
            };
        }
        
        public Cat NestedCats()
        {
            Cat grandMother = new Cat();
            Cat mother = new Cat();
            Cat daughter = new Cat();
            Cat grandDaughter = new Cat();
            Cat son = new Cat();

            daughter.AddDescendant(grandDaughter);
            
            mother.AddDescendant(son);
            mother.AddDescendant(daughter);

            grandMother.AddDescendant(daughter);

            return grandMother;
        }
    }
}
