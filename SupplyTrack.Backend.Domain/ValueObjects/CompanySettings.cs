using SupplyTrack.Backend.Domain.Enums.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyTrack.Backend.Domain.ValueObjects
{
    public class CompanySettings
    {
        public OperationMode OperationMode { get; set; }
        /// <summary>
        ///  seuil de déclenchement auto d’approvisionnement
        /// </summary>
        public string AutoOrderThreshold { get; set; }
    }
}
