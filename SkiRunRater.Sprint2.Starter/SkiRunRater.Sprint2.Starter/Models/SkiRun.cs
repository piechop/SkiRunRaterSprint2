using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiRunRater
{
    public class SkiRun
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Vertical { get; set; }

        public SkiRun()
        {

        }

        public SkiRun(int id, string Name, int vertical)
        {
            this.ID = id;
            this.Name = Name;
            this.Vertical = vertical;
        }
    }
}
