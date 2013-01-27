using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Domain
{
    public class Cat
    {
        public virtual string Name
        {
            get;
            set;
        }

        public virtual string Breed
        {
            get;
            set;
        }

        public virtual ICollection<Cat> Descendants
        {
            get;
            set;
        }
    }
}
