using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace SkiRunRater
{
    /// <summary>
    /// method to write all ski run information to the date file
    /// </summary>
    public class SkiRunRepository : IDisposable
    {
        private List<SkiRun> _skiRuns;
        private AppEnum.PersistenceType _persistenceType = AppEnum.PersistenceType.None;
        private static string _dataFilePath;

        public SkiRunRepository(AppEnum.PersistenceType type, string dataFilePath)
        {
            _persistenceType = type;
            _dataFilePath = dataFilePath;

            _skiRuns = ReadSkiRunsData(type);
        }

        /// <summary>
        /// method to read all ski run information from the data file and return it as a list of SkiRun objects
        /// </summary>
        /// <param name="dataFilePath">path the data file</param>
        /// <returns>list of SkiRun objects</returns>
        public static List<SkiRun> ReadSkiRunsData(AppEnum.PersistenceType type)
        {
            List<SkiRun> skiRunClassList = null;
            switch(type)
            {
                case AppEnum.PersistenceType.CSV:
                    skiRunClassList = ReadSkiRunsCSV();
                    break;
                case AppEnum.PersistenceType.JSON:
                    skiRunClassList = ReadSkiRunsJSON();
                    break;
                case AppEnum.PersistenceType.XML:
                    skiRunClassList = ReadSkiRunsXML();
                    break;
            }

            return skiRunClassList;
        }

        private static List<SkiRun> ReadSkiRunsCSV()
        {
            const char delineator = ',';

            // create lists to hold the ski run strings and objects
            List<string> skiRunStringList = new List<string>();
            List<SkiRun> skiRunClassList = new List<SkiRun>();

            // initialize a StreamReader object for reading
            StreamReader sReader = new StreamReader(_dataFilePath);

            using (sReader)
            {
                // keep reading lines of text until the end of the file is reached
                while (!sReader.EndOfStream)
                {
                    skiRunStringList.Add(sReader.ReadLine());
                }
            }

            foreach (string skiRun in skiRunStringList)
            {
                // use the Split method and the delineator on the array to separate each property into an array of properties
                string[] properties = skiRun.Split(delineator);

                // populate the ski run list with SkiRun objects
                skiRunClassList.Add(new SkiRun() { ID = Convert.ToInt32(properties[0]), Name = properties[1], Vertical = Convert.ToInt32(properties[2]) });
            }

            return skiRunClassList;
        }

        private static List<SkiRun> ReadSkiRunsXML()
        {
            // create lists to hold the ski run strings and objects
            List<SkiRun> skiRunClassList;

            XmlSerializer serializer = new XmlSerializer(typeof(List<SkiRun>), new XmlRootAttribute("SkiRuns"));

            using (FileStream stream = File.OpenRead(_dataFilePath))
            {
                skiRunClassList = (List<SkiRun>)serializer.Deserialize(stream);
            }

                return skiRunClassList;
        }

        private static List<SkiRun> ReadSkiRunsJSON()
        {
            string jsonText;
            List<SkiRun> skiRuns = new List<SkiRun>();

            StreamReader sReader = new StreamReader(_dataFilePath);

            using (sReader)
            {
                jsonText = sReader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<List<SkiRun>>(jsonText);
        }

        /// <summary>
        /// method to write all of the list of ski runs to the text file
        /// </summary>
        public void WriteSkiRunsData(AppEnum.PersistenceType type)
        {
            switch(type)
            {
                case AppEnum.PersistenceType.CSV:
                    WriteSkiRunsCSV();
                    break;
                case AppEnum.PersistenceType.JSON:
                    WriteSkiRunsJSON();
                    break;
                case AppEnum.PersistenceType.XML:
                    WriteSkiRunsXML();
                    break;
            }
        }

        private void WriteSkiRunsCSV()
        {
            string skiRunString;

            // wrap the FileStream object in a StreamWriter object to simplify writing strings
            StreamWriter sWriter = new StreamWriter(_dataFilePath, false);

            using (sWriter)
            {
                foreach (SkiRun skiRun in _skiRuns)
                {
                    skiRunString = skiRun.ID + "," + skiRun.Name + "," + skiRun.Vertical;

                    sWriter.WriteLine(skiRunString);
                }
            }
        }

        private void WriteSkiRunsJSON()
        {
            StreamWriter sWriter = new StreamWriter(_dataFilePath, false);

            string jsonText = JsonConvert.SerializeObject(_skiRuns, Formatting.Indented);

            using (sWriter)
            {
                sWriter.Write(jsonText);
            }
        }

        private void WriteSkiRunsXML()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<SkiRun>), new XmlRootAttribute("SkiRuns"));

            using (FileStream stream = File.OpenWrite(_dataFilePath))
            {
                serializer.Serialize(stream, _skiRuns);
            }
        }

        /// <summary>
        /// method to add a new ski run
        /// </summary>
        /// <param name="skiRun"></param>
        public void InsertSkiRun(SkiRun skiRun, AppEnum.PersistenceType type)
        {
            _skiRuns.Add(skiRun);

            WriteSkiRunsData(type);
        }

        /// <summary>
        /// method to delete a ski run by ski run ID
        /// </summary>
        /// <param name="ID"></param>
        public void DeleteSkiRun(int ID, AppEnum.PersistenceType type)
        {
            _skiRuns.RemoveAll(sr => sr.ID == ID);

            WriteSkiRunsData(type);
        }

        /// <summary>
        /// method to update an existing ski run
        /// </summary>
        /// <param name="skiRun">ski run object</param>
        public void UpdateSkiRun(SkiRun skiRun, AppEnum.PersistenceType type)
        {
            DeleteSkiRun(skiRun.ID, type);
            InsertSkiRun(skiRun, type);

            WriteSkiRunsData(type);
        }

        /// <summary>
        /// method to return a ski run object given the ID
        /// </summary>
        /// <param name="ID">int ID</param>
        /// <returns>ski run object</returns>
        public SkiRun GetSkiRunByID(int ID)
        {
            SkiRun skiRun = null;

            skiRun = _skiRuns.FirstOrDefault(sr => sr.ID == ID);

            return skiRun;
        }

        /// <summary>
        /// method to return a list of ski run objects
        /// </summary>
        /// <returns>list of ski run objects</returns>
        public List<SkiRun> GetSkiAllRuns()
        {
            return _skiRuns;
        }

        /// <summary>
        /// method to query the data by the vertical of each ski run in feet
        /// </summary>
        /// <param name="minimumVertical">int minimum vertical</param>
        /// <param name="maximumVertical">int maximum vertical</param>
        /// <returns></returns>
        public List<SkiRun> QueryByVertical(int minimumVertical, int maximumVertical)
        {
            List<SkiRun> matchingSkiRuns = new List<SkiRun>();

            //
            // use a lambda expression with the Where method to query
            //
            matchingSkiRuns = _skiRuns.Where(sr => sr.Vertical >= minimumVertical && sr.Vertical <= maximumVertical).ToList();

            return matchingSkiRuns;
        }

        /// <summary>
        /// method to handle the IDisposable interface contract
        /// </summary>
        public void Dispose()
        {
            _skiRuns = null;
        }
    }
}
