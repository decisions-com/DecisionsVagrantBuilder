using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using HtmlAgilityPack;
using VagrantBuilderCore.Classes;


namespace VagrantBuilderCore
{
    public class VagrantBuilderClass
    {
        public string GetReleases (string pathToFile)
        {
            var request = (HttpWebRequest)WebRequest.Create(pathToFile);
            var response = (HttpWebResponse)request.GetResponse();
            string responseString;
            using (var stream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    responseString = reader.ReadToEnd();
                }
            }
            return responseString;
        }
        public BuildList ToXml (string xmlAsString)
        {
           
            var serializer = new XmlSerializer(typeof(BuildList));

            BuildList _BuildList;

            using (var reader = new StringReader(xmlAsString))
            {
                 _BuildList = (BuildList)serializer.Deserialize(reader);
            }
            return _BuildList;
        }

        public string MakeDir (string BuildNumber, string DriveLetter)
        {
            if (string.IsNullOrEmpty(DriveLetter))
            {
                DriveLetter = "c:";
            }

            if (!System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                DriveLetter = "";
            }




            var defaultroot = "";
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                defaultroot = DriveLetter + @"\Vagrant\Decisions";
            }
            else
            {
                defaultroot = DriveLetter + @"Vagrant/Decisions";
            }

            var pathtoBuild = defaultroot + BuildNumber;
            var exists = Directory.Exists(pathtoBuild);
            if (exists == true)
            {
                DateTime date = DateTime.Now;
                var newpathtobuild = pathtoBuild+" DUPE "+ date.Month + date.Day + date.Second + date.Millisecond;
                Directory.CreateDirectory(newpathtobuild);
                return newpathtobuild;
            }
            else
            {
                Directory.CreateDirectory(pathtoBuild);
                return pathtoBuild;
            }
            
        }

        public void CloneBaseFIles(string unzipToPath)
        {
            var zippedfiles = "";
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                zippedfiles = Directory.GetCurrentDirectory() + @"\Content\Archive.zip";
            }
            else
            {
                zippedfiles = Directory.GetCurrentDirectory() + @"/Content/Archive.zip";
            }
            //zippedfiles = zippedfiles.Replace("Console", string.Empty);
            var exists = System.IO.File.Exists(zippedfiles);
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var foldertoUnzipInto =  unzipToPath;
                System.IO.Compression.ZipFile.ExtractToDirectory(zippedfiles, foldertoUnzipInto);
            }
            else
            {
                var foldertoUnzipInto = Directory.GetCurrentDirectory() + "/" + unzipToPath;
                System.IO.Compression.ZipFile.ExtractToDirectory(zippedfiles, foldertoUnzipInto);
            }
                

        }

        public bool CheckHTTPURLExists (string URL)
        {
            try
            {
                GetReleases(URL);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public bool DownloadWebFolderForInstallation(string URL, string BuildVersion, string file, string path)
        {
            try
            {
                WebClient client = new WebClient();
                Console.WriteLine("downloading... "+ file);
                var curpath = "";
                if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    curpath = "c:";
                }
                else
                {
                    curpath = Directory.GetCurrentDirectory();
                }
                     
                var fullpath = curpath + "/Cache/" + BuildVersion + "/" + file;
                var doesfolderexist = System.IO.Directory.Exists(fullpath);
                if (doesfolderexist==false)
                {
                    System.IO.Directory.CreateDirectory(curpath + "/Cache/" + BuildVersion);
                }
                client.DownloadFile( URL + "\\" + file, fullpath );
                return true;
            }
            catch (System.Exception)
            {
                
                return false;
            }
        }

        public bool DownloadDotNetFramework(string BuildNumber, string Version, string path)
        {
            var file = "";
            var urldownload = "";
            if (Version == "4.8");
            {
            file = "ndp48-x86-x64-allos-enu.exe";
            urldownload = "https://go.microsoft.com/fwlink/?linkid=2088631";
            }
            var curpath = "";
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                curpath = "c:";

            }
            else

            {
                curpath = Directory.GetCurrentDirectory();

            }
           
            var fullpath = curpath + "/Cache/" +  file;
            var doeslocalfileexists = System.IO.File.Exists(fullpath);
            if(doeslocalfileexists == true)
            {
                Console.WriteLine("Local Cache Exists...for " + file + " using this instead");
                
            }
            else
            {
                Console.WriteLine("Downloading " + file  );
                WebClient wc = new WebClient();
                wc.DownloadFile(urldownload, fullpath);
                doeslocalfileexists = true;
            }
            var topath = "";
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                topath =  path + "/DecisionsInstaller/" + file;

            }
            else
            {
                topath = curpath + "/" + path + "/DecisionsInstaller/" + file;

            }

            System.IO.File.Copy(fullpath,topath, true);
            return doeslocalfileexists;

        }

        public bool GetAllLinksOnInstallationPageAndDownload (string URL, string BuildNumber, string path)
        {
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = hw.Load(URL);
            foreach(HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
               if( link.InnerHtml.StartsWith("All")   )
               {
                    continue;
               }
               if( link.InnerHtml.StartsWith("["))
               {
                   continue;
               }

               {
                   var f = CheckIfExistsInLocalCache(URL, BuildNumber, link.InnerHtml, path);
                   if (f ==false)
                   {
                       DownloadWebFolderForInstallation(URL, BuildNumber, link.InnerHtml, path);
                       
                   }
                    var curpath = "";
                    if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        curpath = "c:";

                    }
                    else

                    { 
                        curpath = Directory.GetCurrentDirectory();

                    }
                    var fullpath = curpath + "/Cache/" + BuildNumber + "/" + link.InnerHtml;
                    var topath = "";
                    if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                         topath =  path + "/DecisionsInstaller/" + link.InnerHtml;
                    }
                    else
                    {
                         topath = curpath + "/" + path + "/DecisionsInstaller/" + link.InnerHtml;
                    }
                        
                        System.IO.File.Copy(fullpath,topath, true);
                   

               }
            }

            

            return false;
        }

        public bool CheckIfExistsInLocalCache (string URL, string BuildNumber, string link, string path)
        {
            try
            {
                var curpath = "";
                if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                     curpath = "c:";
                }
                else
                {
                     curpath = Directory.GetCurrentDirectory();
                }
                    
                var fullpath = curpath + "/Cache/" + BuildNumber + "/" + link;
                var doeslocalfileexists = System.IO.File.Exists(fullpath);
                if(doeslocalfileexists == true)
                {
                    Console.WriteLine("Local Cache Exists...for " + link + " using this instead");
                }
                return doeslocalfileexists;
            }
            catch (System.Exception)
            {
                return false;
            }
        }


        public bool UpdateDownloadStringInDecisionsAutoInstaller(string PathToRootOfBuildFolder, BuildListBuild BuildNumber)
        {


            try
            {
                var FileTochangeFullPath = "";

                if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    FileTochangeFullPath = PathToRootOfBuildFolder + @"\DecisionsInstaller\InstallerSetup.xml";
                }
                else
                {

                    FileTochangeFullPath = PathToRootOfBuildFolder + @"/DecisionsInstaller/InstallerSetup.xml";
                }



                var filesContent = File.ReadAllText(FileTochangeFullPath);

                if (BuildNumber.IsPublicallyReleased == true)
                {
                    filesContent = filesContent.Replace("<TxtAlternateURLSource>https://releases.decisions.com/releasedversions/60577</TxtAlternateURLSource>", "<TxtAlternateURLSource>https://releases.decisions.com/releasedversions/" + BuildNumber.SVNRevision + "</TxtAlternateURLSource>");

                }

                else
                {
                    filesContent = filesContent.Replace("<TxtAlternateURLSource>https://releases.decisions.com/releasedversions/60577</TxtAlternateURLSource>", "<TxtAlternateURLSource>http://releases.decisions.com/stagedversions/" + BuildNumber.SVNRevision + "</TxtAlternateURLSource>");
                }
                System.IO.File.WriteAllText(FileTochangeFullPath, filesContent);
                return true;
            }
            catch (Exception)
            {
                return false;
               
            }
        }
    }
}
