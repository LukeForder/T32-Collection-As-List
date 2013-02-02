using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Domain
{
    [DataContract(IsReference=true)]
    public class Cat
    {
        [DataMember]
        private string _name;

        public Cat()
        {
            _descendants = new List<Cat>();
        }

        public Cat(string name)
            : this()
        {
            _name = name;
        }

        public virtual string Name
        {
            get
            {
                return _name;
            }
        }

        [DataMember]
        public virtual string Breed
        {
            get;
            set;
        }

        [DataMember]
        private ICollection<Cat> _descendants;

        public virtual IEnumerable<Cat> Descendants
        {
            get
            {
                return _descendants;
            }
        }

        public void AddDescendant(Cat cat)
        {
            _descendants.Add(cat);
        }


    }
}
