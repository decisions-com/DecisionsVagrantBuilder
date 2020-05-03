using System;
using System.Collections.Generic;
using System.Text;

namespace VagrantBuilderCore.Config
{
    public class AWSConfig
    {

        public string aws_access_key_id { get; set; }

        public string aws_secret_access_key { get; set; }

        public string aws_SSH_private_key_path { get; set; }
        public string aws_keypair_name { get; set; }

        public string[] aws_security_groups { get; set; }

        public string AWS_Name { get; set; }
    }
}
