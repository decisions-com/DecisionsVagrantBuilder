using System;
using System.Collections.Generic;
using VagrantBuilderCore.Config;
namespace VagarantBuilderConsoleRunAnywhereConfigLocal
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This to Create a config files, it wont overwrite");
            var basepath = @"C:\Vagrant\Config\";
            if (System.IO.File.Exists(basepath + "awsConfig.json"))
            {
                var ConfigAWS = new AWSConfig();
                ConfigAWS.aws_access_key_id = "";
                ConfigAWS.aws_keypair_name = "";
                ConfigAWS.aws_secret_access_key = "";
                List<string> list = new List<string>();
                list.Add("securityGroupA");
                list.Add("securityGroupA");
                ConfigAWS.aws_security_groups = list.ToArray();
                ConfigAWS.aws_SSH_private_key_path = "";

                var stringOfConfigAWS = Newtonsoft.Json.JsonConvert.SerializeObject(ConfigAWS);
                System.IO.File.WriteAllText(basepath + "awsConfig.json", stringOfConfigAWS, System.Text.Encoding.Default);
            }

            if (!System.IO.File.Exists(basepath + "VagrantScriptingDefault.txt"))
            {
                System.IO.File.Copy("VagrantScriptingDefault.txt", basepath + "VagrantScriptingDefault.txt");
            }
        }
    }
}
