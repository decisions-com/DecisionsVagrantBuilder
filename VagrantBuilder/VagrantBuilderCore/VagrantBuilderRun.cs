using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using VagrantBuilderCore.Classes;


namespace VagrantBuilderConsoleRunAnywhere
{
    class Program
    {
        static void Main(string[] args)
        {

            RequireAdministrator();
            try
            {


                try
                {
                    if (args[0].Contains(':'))
                    {
                        System.IO.File.WriteAllText("vagrantDrive.config", args[0]);
                    }

                }
                catch
                {
                    //dont care

                }


                var xmlasstringPublic = new VagrantBuilderCore.VagrantBuilderClass().GetReleases("https://releases.decisions.com/releasedversions/PublicReleases.xml");
                var xmlasstringStaged = new VagrantBuilderCore.VagrantBuilderClass().GetReleases("http://releases.decisions.com/stagedversions/StagedReleases.xml");

                List<BuildListBuild> ListBuilds = new List<BuildListBuild>();

                var xmlPublic = new VagrantBuilderCore.VagrantBuilderClass().ToXml(xmlasstringPublic);
                var xmlStaged = new VagrantBuilderCore.VagrantBuilderClass().ToXml(xmlasstringStaged);

                ListBuilds.AddRange(xmlPublic.Builds);
                ListBuilds.AddRange(xmlStaged.Builds);

                IEnumerable<BuildListBuild> buildListBuilds = ListBuilds.OrderBy(x => x.SVNRevision).ToList();

                foreach (var i in buildListBuilds.Where(x => x.IsPublicallyReleased == true).OrderBy(x => x.SVNRevision))
                {
                    if (i.SVNRevision.Equals("9791") || i.SVNRevision.Equals("9856") || i.SVNRevision.Equals("9971"))
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

                    if (BuildItemToCreatecheck.Count > 1)
                    {
                        BuildItemToCreate = ListBuilds.Where(x => x.SVNRevision == buildNumber).ToList().First();
                    }
                    else
                    {
                        BuildItemToCreate = ListBuilds.Where(x => x.SVNRevision == buildNumber).ToList().First();
                    }

                    if (BuildItemToCreate.IsPublicallyReleased == true)
                    {
                        geturlworkd = new VagrantBuilderCore.VagrantBuilderClass().CheckHTTPURLExists("http://releases.decisions.com/releasedversions/" + buildNumber);
                    }
                    else
                    {
                        geturlworkd = new VagrantBuilderCore.VagrantBuilderClass().CheckHTTPURLExists("http://releases.decisions.com/stagedversions/" + buildNumber);
                    }


                    if (geturlworkd == true)
                    {
                       

                        if (System.Runtime.InteropServices.RuntimeInformation
                                               .IsOSPlatform(OSPlatform.Windows))
                        {

                            var driveid = "c:";
                            if (System.IO.File.Exists("vagrantDrive.config"))
                            {
                                driveid = System.IO.File.ReadAllText("vagrantDrive.config");
                                if (string.IsNullOrEmpty(driveid))
                                {
                                    driveid = "c:";
                                }
                            }

                            var makeddir = new VagrantBuilderCore.VagrantBuilderClass().MakeDir(buildNumber, driveid);
                            new VagrantBuilderCore.VagrantBuilderClass().CloneBaseFIles(makeddir);
                            new VagrantBuilderCore.VagrantBuilderClass().UpdateDownloadStringInDecisionsAutoInstaller(makeddir, BuildItemToCreate);


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
                        {//linux / mac option
                            var makeddir2 = new VagrantBuilderCore.VagrantBuilderClass().MakeDir(buildNumber, "c");
                            new VagrantBuilderCore.VagrantBuilderClass().CloneBaseFIles(makeddir2);
                            new VagrantBuilderCore.VagrantBuilderClass().UpdateDownloadStringInDecisionsAutoInstaller(makeddir2, BuildItemToCreate);

                        }
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

        [DllImport("libc")]
        public static extern uint getuid();

        /// <summary>
        /// Asks for administrator privileges upgrade if the platform supports it, otherwise does nothing
        /// </summary>

        

        /// <summary>
        /// Asks for administrator privileges upgrade if the platform supports it, otherwise does nothing
        /// </summary>
        public static void RequireAdministrator()
        {
            string name = System.AppDomain.CurrentDomain.FriendlyName;
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
                    {
                        WindowsPrincipal principal = new WindowsPrincipal(identity);
                        if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
                        {
                            throw new InvalidOperationException($"Application must be run as administrator. Right click the {name} file and select 'run as administrator'.");
                        }
                    }
                }
                else if (getuid() != 0)
                {
                    throw new InvalidOperationException($"Application must be run as root/sudo. From terminal, run the executable as 'sudo {name}'");
                }
            }
            catch (Exception ex)
            {
                //throw new ApplicationException("Unable to determine administrator or root status", ex);
            }
        }
    }
}
