using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftSupply.Help.Models
{
    public class Manifest
    {

        public string Name { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public ICollection<Item> Index { get; set; }

        public override string ToString()
        {
            return Title ?? base.ToString();
        }
    }

    public class Item
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Target { get; set; }
        public string Class { get; set; }
        public ICollection<Item> Items { get; set; }

        public override string ToString()
        {
            return Title ?? base.ToString();
        }
    }

}
