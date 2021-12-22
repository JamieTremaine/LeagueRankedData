using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueTierLevels
{
    class ApiArgs
    {
        public MingweiSamuel.Camille.Enums.Region Region { get; set; }
        public string Tier { get; set; }
        public MingweiSamuel.Camille.Enums.Division? DivisionName { get; set; }
        public string Queue { get; set; }
        public int? Page { get; set; }
        public string ID { get; set; }

        public ApiArgs(MingweiSamuel.Camille.Enums.Region Region = default, 
                        string Tier = "",MingweiSamuel.Camille.Enums.Division? DivisionName = null, 
                        string Queue = "", int? Page = null, string ID = "")
        {
            this.Region = Region;
            this.Tier = Tier;
            this.DivisionName = DivisionName;
            this.Queue = Queue;
            this.Page = Page;
            this.ID = ID;
        }

    }
}
