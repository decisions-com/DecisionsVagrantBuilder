using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VagrantBuilderCore.Classes
{
 

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class BuildList
    {

        private BuildListBuild[] buildsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Build", IsNullable = false)]
        public BuildListBuild[] Builds
        {
            get
            {
                return this.buildsField;
            }
            set
            {
                this.buildsField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class BuildListBuild
    {

        private string releasedNameField;

        private string stagedNameField;

        private string fullVersionField;

        private string sVNRevisionField;

        private string pathField;

        private string releasedByField;

        private string releasedOnField;

        private bool isPublicallyReleasedField;

        private string buildFamilyField;

        /// <remarks/>
        public string ReleasedName
        {
            get
            {
                return this.releasedNameField;
            }
            set
            {
                this.releasedNameField = value;
            }
        }

        /// <remarks/>
        public string StagedName
        {
            get
            {
                return this.stagedNameField;
            }
            set
            {
                this.stagedNameField = value;
            }
        }

        /// <remarks/>
        public string FullVersion
        {
            get
            {
                return this.fullVersionField;
            }
            set
            {
                this.fullVersionField = value;
            }
        }

        /// <remarks/>
        public string SVNRevision
        {
            get
            {
                return this.sVNRevisionField;
            }
            set
            {
                this.sVNRevisionField = value;
            }
        }

        /// <remarks/>
        public string Path
        {
            get
            {
                return this.pathField;
            }
            set
            {
                this.pathField = value;
            }
        }

        /// <remarks/>
        public string ReleasedBy
        {
            get
            {
                return this.releasedByField;
            }
            set
            {
                this.releasedByField = value;
            }
        }

        /// <remarks/>
        public string ReleasedOn
        {
            get
            {
                return this.releasedOnField;
            }
            set
            {
                this.releasedOnField = value;
            }
        }

        /// <remarks/>
        public bool IsPublicallyReleased
        {
            get
            {
                return this.isPublicallyReleasedField;
            }
            set
            {
                this.isPublicallyReleasedField = value;
            }
        }

        /// <remarks/>
        public string BuildFamily
        {
            get
            {
                return this.buildFamilyField;
            }
            set
            {
                this.buildFamilyField = value;
            }
        }
    }


}
