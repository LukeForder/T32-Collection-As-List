﻿using Contracts.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Services
{
    [ServiceContract]
    public interface IPetShopService
    {
        [OperationContract]
        List<Cat> All();

        [OperationContract]
        List<Cat> Cyclic();

        [OperationContract]
        Cat NestedCats();
    }
}
