using System;
using System.Collections.Generic;
using System.Text;
using TinyCsvParser.Mapping;

namespace MacLookup.Models
{
    public class OUIObjectMap: CsvMapping<Mac> {
        public OUIObjectMap():base()
        {
            MapProperty(0,m => m.Registry);
            MapProperty(1,m => m.Assignment);
            MapProperty(2,m => m.OrganizationName);
            MapProperty(3,m => m.OrganizationAddress);
        }
    }
}
