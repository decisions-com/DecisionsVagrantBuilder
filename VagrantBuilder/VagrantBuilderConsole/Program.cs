using System;
using System.Collections.Generic;
using System.Diagnostics;
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



                var xmlasstringPublic = new VagrantBuilder.VagrantBuilderClass().GetReleases("https://releases.decisions.com/releasedversions/PublicReleases.xml");
                var xmlasstringStaged = new VagrantBuilder.VagrantBuilderClass().GetReleases("http://releases.decisions.com/stagedversions/StagedReleases.xml");

                List<BuildListBuild> ListBuilds = new List<BuildListBuild>();

                var xmlPublic = new VagrantBuilder.VagrantBuilderClass().ToXml(xmlasstringPublic);
                var xmlStaged = new VagrantBuilder.VagrantBuilderClass().ToXml(xmlasstringStaged);

                ListBuilds.AddRange(xmlPublic.Builds);
                ListBuilds.AddRange(xmlStaged.Builds);

                IEnumerable<BuildListBuild> buildListBuilds = ListBuilds.OrderBy(x => x.SVNRevision).ToList();

                foreach (var i in buildListBuilds.Where(x=> x.IsPublicallyReleased == true).OrderBy(x=> x.SVNRevision))
                {
                    if (i.SVNRevision.Equals("9791") || i.SVNRevision.Equals("9856") || i.SVNRevision.Equals( "9971"))
                   {

                    }
                    else
                    {
                        Console.WriteLine("BUILDNUMBER = " + i.SVNRevision + " -- Staged Name " + i.StagedName + " -- Released on the " + i.ReleasedOn + " -- Is Publically Released: " + i.IsPublicallyReleased);

                    }
                }



                Console.WriteLine("Which Build Number?? (note we support Staged versions too, type 'S' for staged Listing)");
                string buildNumber = Console.ReadLine();

                if (buildNumber.Equals("S"))
                {


                    foreach (var i in buildListBuilds.Where(x => x.IsPublicallyReleased == false).OrderBy(x => x.SVNRevision))
                    {
                        if (i.SVNRevision.Equals("9791") || i.SVNRevision.Equals("9856") || i.SVNRevision.Equals("9971"))
                        {

                        }
                        else
                        {
                            Console.WriteLine("BUILDNUMBER = " + i.SVNRevision + " -- Staged Name " + i.StagedName + " -- Released on the " + i.ReleasedOn + " -- Is Publically Released: " + i.IsPublicallyReleased);

                        }
                    }
                    Console.WriteLine("Which Build Number??");
                    buildNumber = Console.ReadLine();
                }
               
                

                bool geturlworkd;
                BuildListBuild BuildItemToCreate = new BuildListBuild();
                try
                {
                    
                    var BuildItemToCreatecheck = ListBuilds.Where(x => x.SVNRevision == buildNumber).ToList();

                    if(BuildItemToCreatecheck.Count > 1)
                    {
                        BuildItemToCreate = ListBuilds.Where(x => x.SVNRevision == buildNumber).ToList().First();
                    }
                    else
                    {
                        BuildItemToCreate = ListBuilds.Where(x => x.SVNRevision == buildNumber).ToList().First();
                    }

                    if (BuildItemToCreate.IsPublicallyReleased == true)
                        {
                            geturlworkd = new VagrantBuilder.VagrantBuilderClass().CheckHTTPURLExists("http://releases.decisions.com/releasedversions/" + buildNumber);
                        }
                        else
                        {
                            geturlworkd = new VagrantBuilder.VagrantBuilderClass().CheckHTTPURLExists("http://releases.decisions.com/stagedversions/" + buildNumber);
                        }


                        if (geturlworkd == true)
                        {
                            var makeddir = new VagrantBuilder.VagrantBuilderClass().MakeDir(buildNumber);
                            new VagrantBuilder.VagrantBuilderClass().CloneBaseFIles(makeddir);
                            new VagrantBuilder.VagrantBuilderClass().UpdateDownloadStringInDecisionsAutoInstaller(makeddir, BuildItemToCreate);


                            Console.WriteLine("-----");
                            Console.WriteLine("Done Full Path is " + makeddir);
                            Console.WriteLine("-----");
                            Console.WriteLine("IN CMD (Run as Administrator)");
                            Console.WriteLine("CD " + makeddir);
                            Console.WriteLine("vagrant up --provider hyperv");

                            var proc1 = new ProcessStartInfo();
                            string anyCommand;
                            proc1.UseShellExecute = true;

                            proc1.WorkingDirectory = makeddir;

                            proc1.FileName = @"C:\Windows\System32\cmd.exe";
                            proc1.Verb = "runas";
                            proc1.Arguments = " /C vagrant up --provider hyperv";
                            proc1.WindowStyle = ProcessWindowStyle.Normal;
                        
                            Process.Start(proc1);
                        }
                        else
                        {
                            Console.WriteLine("Build Doenst Exist on the Servers");
                            Console.Read();
                        }

                }
                catch
                {
                    Console.WriteLine("Build Doesnt Exist..");
                }




               


            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
