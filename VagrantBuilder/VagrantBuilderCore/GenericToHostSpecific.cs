using System;
using System.Collections.Generic;
using System.Text;
using VagrantBuilderCore.Classes;

namespace VagrantBuilderCore
{
    public class GenericToHostSpecific
    {
        public AWSHostingSpecific GenericToAWS(VagrantSpec spec)
        {
            var returnResult = new Classes.AWSHostingSpecific();
            if (spec.Provider.Contains("AWS") || spec.Provider.Contains("Amazon"))
            {
                if ((spec.CPU == 2) && (spec.RAM == 4))
                {
                    returnResult.instanceType = "t3.medium";
                    returnResult.Disk = 60;
                }
                else if ((spec.CPU == 2) && (spec.RAM == 8))
                {
                    returnResult.instanceType = "t3.large";
                    returnResult.Disk = 80;
                }
                else if ((spec.CPU == 4) && (spec.RAM == 16))
                {
                    returnResult.instanceType = "t3.xlarge";
                    returnResult.Disk = 100;
                }
                else if ((spec.CPU == 8) && (spec.RAM == 32))
                {
                    returnResult.instanceType = "t3.2xlarge";
                    returnResult.Disk = 120;
                }

                returnResult.region = spec.Region;
                returnResult.VMName = spec.vmname;
            }

            return returnResult;
        }

        public HyperVHostingSpecific GenericToHyperV(VagrantSpec spec)
        {
            return new HyperVHostingSpecific { CPU = spec.CPU, RAM = spec.RAM*1024, VMName = spec.vmname  };
        }
    }
}
