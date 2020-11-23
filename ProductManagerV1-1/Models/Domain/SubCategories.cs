using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManagerV1_1.Models.Domain
{
    class SubCategories
    {
        public static int ID { get; set; }
        public static string Name { get; set; }
        public static int FK_Category { get; set; }
        public static int FK_products { get; set; }
    }
}
