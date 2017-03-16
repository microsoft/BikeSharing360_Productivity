using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeSharing.SeedWork
{
    public interface ILog
    {
        void Log(string msg);
        void Trace(string msg);
        void Debug(string msg);
    }
}
