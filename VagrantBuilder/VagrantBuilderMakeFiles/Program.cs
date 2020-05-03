using System;
using VagrantBuilderCore;
using VagrantBuilderCore.Classes;
using VagrantBuilderCore.Config;
namespace VagrantBuilderMakeFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            //dotnet C:\\Vagrant\\code\\DecisionsVagrantBuilder\\VagrantBuilder\\VagrantBuilderConsoleRunAnywhere\\bin\\Debug\\netcoreapp2.1\\VagrantBuilderConsoleRunAnywhere.dll Rundeck:${ option.Version}:${ option.Customer}\${ option.ServerInstance}
            try
            {
                Console.WriteLine("CustomerName, InstanceName, CPU, RAM(gigs), Version, BaseURL, OutputPathOfFile");
                string CustomerName = args[0];

                string InstanceName = args[1];
                int CPU = int.Parse(args[2]);
                int RAM = int.Parse(args[3]);
                int Version = int.Parse(args[4]);
                string BaseURL = args[5];
                string OutputPathOfFile = args[6];


                var f = new VagrantBuild().RunnerHyperV(CustomerName,InstanceName, CPU, RAM, Version, BaseURL, OutputPathOfFile);
            }
            catch (Exception ex2)
            {
                Console.WriteLine(ex2.Message);
                Console.WriteLine(ex2.InnerException);
                throw;
            }
        }
    }
}
