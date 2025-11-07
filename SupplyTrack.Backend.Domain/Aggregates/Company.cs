using SupplyTrack.Backend.Domain.Enums.Company;
using SupplyTrack.Backend.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyTrack.Backend.Domain.Aggregates
{
    public class Company
    {
        #region properties
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Industry Type { get; private set; }
        public ContactInfo ContactInfo { get; private set; }
        public CompanySettings Settings { get; private set; }

        #endregion

    }
}
