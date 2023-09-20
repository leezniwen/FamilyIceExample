using System;
using System.Collections.Generic;

namespace family_icecream
{
	public class FamilyStoreDB
	{
		public string city_name { get; set; }
		public List<stores> stores { get; set; }
	}

	public class stores
    {
        public string? NAME { get; set; }
        public string? TEL { get; set; }
        public string? POSTel { get; set; }
        public float? px { get; set; }
        public float? py { get; set; }
        public string? addr { get; set; }
        public long? serid { get; set; }
        public string? pkey { get; set; }
        public string? post { get; set; }
        public string? all { get; set; }
        public string? twoice { get; set; }
            
	}
}

