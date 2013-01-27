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
        public Cat()
        {
            Descendants = new List<Cat>();
        }

        [DataMember]
        public virtual string Name
        {
            get;
            set;
        }

        [DataMember]
        public virtual string Breed
        {
            get;
            set;
        }

        [DataMember]
        public virtual ICollection<Cat> Descendants
        {
            get;
            set;
        }
    }
}
