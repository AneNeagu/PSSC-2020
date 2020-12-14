using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GrainInterfaces
{
    public interface IAsyncHello : Orleans.IGrainWithStringKey
    {
        Task StartAsync();
    }
}