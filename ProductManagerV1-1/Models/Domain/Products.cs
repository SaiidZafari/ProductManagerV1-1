using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManagerV1_1.Models.Domain
{
    class Products
    {
        public static int ID { get; set; }
        public static string ArticleNumber { get; set; }
        public static string Name { get; set; }
        public static string Material { get; set; }
        public static string Color { get; set; }
        public static int Price { get; set; }
        public static string Description { get; set; }
        public static int FK_Categories { get; set; }

        //public Products(string articleNumber, string name, string material, string color, int price, string description, int fK_Categories) =>(ArticleNumber, Name, Material, Color,Price, Description, FK_Categories) = (articleNumber, name, material, color, price, description, fK_Categories);
    }
}
