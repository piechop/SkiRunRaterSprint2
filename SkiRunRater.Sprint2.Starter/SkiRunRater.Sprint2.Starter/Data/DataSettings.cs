using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiRunRater
{
    public class DataSettings
    {
        public string dataFilePath = "Data\\Data.csv";
        public DataSettings(AppEnum.PersistenceType type)
        {
            switch(type)
            {
                case AppEnum.PersistenceType.CSV:
                    dataFilePath = "Data\\Data.csv";
                    break;
                case AppEnum.PersistenceType.JSON:
                    dataFilePath = "Data\\Data.json";
                    break;
                case AppEnum.PersistenceType.XML:
                    dataFilePath = "Data\\Data.xml";
                    break;
            }
        }
    }
}
