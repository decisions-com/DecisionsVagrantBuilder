using System;
using System.Collections.Generic;
using System.Text;
using VagrantBuilderCore;
using VagrantBuilderCore.Classes;
using VagrantBuilderCore.Config;

namespace VagrantBuilderMakeFiles
{
    public class VagrantBuild
    {
        public bool RunnerHyperV(string customerName, string InstanceName, int CPU, int RAMinGig, int DecisionsVersion, string BaseURL, string resultfilePath)
        {
            try
            {

                var pathtoOutputFile = resultfilePath;

                var spec = new HyperVHostingSpecific() { CPU = CPU, RAM = RAMinGig, VMName = customerName + "-" + InstanceName, BaseURL = BaseURL, DecisionsVersion = DecisionsVersion };

                if (!spec.BaseURL.Contains("http"))
                {
                    Console.WriteLine("Base URL needs to be a URL");
                    throw new Exception("Base URL needs to be a URL");
                }

                System.IO.File.Copy("HyperVHeaderFile.txt", pathtoOutputFile, true);

                var ScriptFile = System.IO.File.ReadAllText(pathtoOutputFile);
                spec.RAM = spec.RAM * 1024;

                ScriptFile = ScriptFile.Replace("$$CPUS$$", spec.CPU.ToString());
                ScriptFile = ScriptFile.Replace("$$RAM$$", spec.RAM.ToString());
                ScriptFile = ScriptFile.Replace("$$VMName$$", "\"" + spec.VMName + "\"");

                ApplyScript(pathtoOutputFile, ScriptFile, spec.BaseURL, spec.DecisionsVersion);


                return true;

            }
            catch (Exception ex2)
            {
                Console.WriteLine(ex2.Message);
                Console.WriteLine(ex2.InnerException);
                throw;
            }


        }

        public bool RunnerAWS(string customerName, string InstanceName, int CPU, int RAM, int DecisionsVersion, string BaseURL, string resultfilePath)
        {
            try
            {

                var pathtoOutputFile = resultfilePath;

                var spec = new VagrantSpec() { CPU = 8, RAM = 16, Provider = "HyperV", Region = "eu-west-2", vmname = "nameofVM2", DecisionsVersion = 64509, BaseURL = "http://localhost" };

                if (!spec.BaseURL.StartsWith("http"))
                {
                    Console.WriteLine("Base URL needs to be a URL");
                    throw new Exception("Base URL needs to be a URL");
                }

                var AWSSpec = new VagrantBuilderCore.GenericToHostSpecific().GenericToAWS(spec);

                var config = new AWSConfig();


                var fileheader = "";
                if (spec.Provider == "AWS")
                {

                    System.IO.File.Copy("AWSHeaderFile.txt", pathtoOutputFile, true);
                    config = Newtonsoft.Json.JsonConvert.DeserializeObject<AWSConfig>(System.IO.File.ReadAllText(@"C:\Vagrant\Config\awsConfig.json"));



                    var secgroupAsSingleString = "";
                    var EndItem = config.aws_security_groups.Length;
                    var count = 1;
                    foreach (var i in config.aws_security_groups)
                    {
                        if (count != EndItem)
                        {
                            secgroupAsSingleString = secgroupAsSingleString + "\"" + i + "\",";
                        }
                        else
                        {
                            secgroupAsSingleString = secgroupAsSingleString + "\"" + i + "\"";
                        }
                        count++;
                    }



                    var ScriptFile = System.IO.File.ReadAllText(pathtoOutputFile);

                    ScriptFile = ScriptFile.Replace("$$AWS.DISKSIZE$$", AWSSpec.Disk.ToString());
                    ScriptFile = ScriptFile.Replace("$$aws.access_key_id$$", config.aws_access_key_id);
                    ScriptFile = ScriptFile.Replace("$$aws.secret_access_key$$", config.aws_secret_access_key);
                    ScriptFile = ScriptFile.Replace("$$aws.region$$", AWSSpec.region);
                    ScriptFile = ScriptFile.Replace("$$aws.instance_type$$", AWSSpec.instanceType);
                    ScriptFile = ScriptFile.Replace("$$override.ssh.private_key_path$$", config.aws_SSH_private_key_path.Replace(@"\", "/"));
                    ScriptFile = ScriptFile.Replace("$$aws.keypair_name$$", config.aws_keypair_name);
                    ScriptFile = ScriptFile.Replace("$$aws.security_groups$$", secgroupAsSingleString);

                    ScriptFile = ScriptFile.Replace("$$VMName$$", "\"" + spec.vmname + "\"");

                    ApplyScript(pathtoOutputFile, ScriptFile, BaseURL, DecisionsVersion);
                }

                
                return true;

            }
            catch (Exception ex2)
            {
                Console.WriteLine(ex2.Message);
                Console.WriteLine(ex2.InnerException);
                throw;
            }


        }

        public static void ApplyScript(string pathtoOutputFile, string ScriptFile, string BaseURL, int DecisionsVersion)
        {
            var VagrantSctipingDefault = System.IO.File.ReadAllText("VagrantScriptingDefault.txt");

            ScriptFile = ScriptFile.Replace("$$SCRIPT$$", VagrantSctipingDefault);
            ScriptFile = ScriptFile.Replace("$$DecisionsVersion$$",DecisionsVersion.ToString());
            ScriptFile = ScriptFile.Replace("$$BaseURL$$", BaseURL);

            var URLatRoot = BaseURL.Split("/");
            if (URLatRoot.Length == 3)
            {
                ScriptFile = ScriptFile.Replace("$$BaseRoot$$", "true");
            }
            else
            {
                ScriptFile = ScriptFile.Replace("$$BaseRoot$$", "false");
            }

            System.IO.File.WriteAllText(pathtoOutputFile, ScriptFile);
            //var resultingFile = fileheader + ScriptFile;

        }
    }
}

