using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VagrantBuilder.Classes;

namespace VagrantBuilderConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {

                var xmlasstring = new VagrantBuilder.VagrantBuilderClass().GetReleases("https://releases.decisions.com/releasedversions/PublicReleases.xml");



                var xml = new VagrantBuilder.VagrantBuilderClass().ToXml(xmlasstring);

                IEnumerable<BuildListBuild> buildListBuilds = xml.Builds.OrderBy(x => x.SVNRevision).ToList();

                foreach (var i in buildListBuilds)
                {
                    Console.WriteLine(i.SVNRevision + " Released on the " + i.ReleasedOn);
                }

                Console.WriteLine("Which Build Number??");
                string buildNumber = Console.ReadLine();

                var makeddir = new VagrantBuilder.VagrantBuilderClass().MakeDir(buildNumber);
                new VagrantBuilder.VagrantBuilderClass().CloneBaseFIles(makeddir);
                new VagrantBuilder.VagrantBuilderClass().UpdateDownloadStringInDecisionsAutoInstaller(makeddir, buildNumber);


                Console.WriteLine("-----");
                Console.WriteLine("Done Full Path is " + makeddir);
                Console.WriteLine("-----");
                Console.WriteLine("IN CMD (Run as Administrator)");
                Console.WriteLine("CD "+makeddir);
                Console.WriteLine("vagrant up --provider hyperv");
                     
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
