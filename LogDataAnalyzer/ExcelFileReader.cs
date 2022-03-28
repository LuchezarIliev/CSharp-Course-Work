/*
 * LogDataAnalyzer
 * Author: Lachezar Iliev
 * Faculty №: 121217151
 * Group: 37
 */
using System.IO;
using System.Data;
using ExcelDataReader;
using System.Collections.Generic;

namespace LogDataAnalyzer
{
    public class ExcelFileReader
    {
        public string FilePath { get; }

        private readonly Dictionary<int, int> _userWikiData;

        public ExcelFileReader(string filePath, string criteria)
        {
            FilePath = filePath;
            _userWikiData = new Dictionary<int, int>();
            ReadData(criteria);
        }

        public Dictionary<int, int> GetData()
        {
            return _userWikiData;
        }

        private void ReadData(string criteria)
        {
            using (var stream = File.Open(FilePath, FileMode.Open, FileAccess.Read))
            {
                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var table = reader.AsDataSet().Tables[0];
                    var rows = table.Rows;
                    foreach (DataRow row in rows)
                    {
                        MapData(row.ItemArray, criteria);
                    }
                }
            }
        }

        private void MapData(object[] items, string criteria)
        {
            var eventName = items[3].ToString();
            if (!eventName.Equals(criteria)) return;
            var log = items[4].ToString();
            var userId = int.Parse(log.Split('\'')[1].Trim('\''));
            if (_userWikiData.ContainsKey(userId))
                _userWikiData[userId]++;
            else
                _userWikiData.Add(userId, 1);
        }
    }
}
