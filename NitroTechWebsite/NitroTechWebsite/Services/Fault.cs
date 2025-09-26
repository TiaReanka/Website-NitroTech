using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroTechWebsite.Services
{
    [Serializable]
    public class Fault
    {
        public string FaultDescription { get; set; }
        public int ProductID { get; set; }
        public int AmountUsed { get; set; }
        
        public decimal TotalAmount { get; set; }
    }
}
