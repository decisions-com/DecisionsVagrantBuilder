using System;
using System.Collections.Generic;
using System.Text;

namespace VagrantBuilderCore.Classes
{
    public class AWSHostingSpecific
    {
        public string region { get; set; }
        public int Disk { get; set; }
        public string VMName { get; set; }
        public string instanceType { get; set; }
   }

    public class HyperVHostingSpecific
    {

        public int CPU { get; set; }
        public int RAM { get; set; }
        public string VMName { get; set; }
        public Int32 DecisionsVersion { get; set; }
        public string BaseURL { get; set; }
    }
}
