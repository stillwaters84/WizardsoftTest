using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WizardsoftTestApp.Models
{
    public class Folder
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string FolderParentId { get; set; }
    }
}
