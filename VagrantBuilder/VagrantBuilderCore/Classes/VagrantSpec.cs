using System;
using System.Collections.Generic;
using System.Text;

namespace VagrantBuilderCore.Classes
{
    public class VagrantSpec
    {
        public int CPU { get; set; }
        public int RAM { get; set; }
        public int Disk { get; set; }
        public string vmname { get; set; }
        public string Provider { get; set; }
        public string Region { get; set; }
        public string instanceType { get; set; }
        public Int32 DecisionsVersion { get; set; }
        public string BaseURL { get; set; }
    }
}
