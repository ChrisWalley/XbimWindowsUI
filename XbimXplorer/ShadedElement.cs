using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace XbimXplorer
{
    public class ShadedElement
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
        public string Space { get; set; }
        public string System { get; set; }
        public string ExtObject { get; set; }
        public int ExtIdentifier { get; set; }
        public string InstallationDate { get; set; }
     

        public criticality currCriticality { get; set; }
        public enum criticality
        {
            Critical,
            Major,
            Moderate,
            Minor,
            Negligible,
            None
        }
    }
}
