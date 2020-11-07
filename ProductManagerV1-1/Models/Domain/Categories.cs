using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManagerV1_1.Models.Domain
{
    public class Categories
    {

        public static string Category { get; set; }
        public static int Inventory { get; set; }

        public Categories(string category)
        {
            Category = category;
        }


    }
}
