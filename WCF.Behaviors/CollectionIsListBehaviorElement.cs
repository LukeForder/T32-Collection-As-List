using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace WCF.Behaviors
{
    public class CollectionIsListBehaviorElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(CollectionIsListEndpointBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new CollectionIsListEndpointBehavior();
        }
    }
}
