using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiRunRater
{
    public class AppEnum
    {
        public enum ManagerAction
        {
            None,
            ListAllSkiRuns,
            DisplaySkiRunDetail,
            DeleteSkiRun,
            AddSkiRun,
            UpdateSkiRun,
            QuerySkiRunsByVertical,
            Quit
        }

        public enum PersistenceType
        {
            None,
            CSV,
            XML,
            JSON
        }
    }
}
