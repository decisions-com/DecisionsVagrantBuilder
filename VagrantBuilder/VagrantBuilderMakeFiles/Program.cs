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

            var f = new VagrantBuild().RunnerHyperV("Jon", "instance1", 4, 16, 64509, "http://localhost", @"C:\vagrant\output");
        }
    }
}
