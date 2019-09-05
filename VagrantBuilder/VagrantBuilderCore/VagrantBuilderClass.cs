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
            System.IO.Compression.ZipFile.ExtractToDirectory(zippedfiles, unzipToPath);

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
