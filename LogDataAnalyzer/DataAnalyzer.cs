/*
 * LogDataAnalyzer
 * Author: Lachezar Iliev
 * Faculty №: 121217151
 * Group: 37
 */
using System;

namespace LogDataAnalyzer
{
    public static class DataAnalyzer
    {
        private static string _criteria;
        private static ExcelFileReader _reader;
        private static StatisticService _service;

        public static void SetCriteria(string criteria)
        {
            _criteria = criteria;
        }

        public static void CreateReader(string filePath)
        {
            if (_reader != null && _reader.FilePath.Equals(filePath)) return;
            _reader = new ExcelFileReader(filePath, _criteria);
            var data = _reader.GetData();
            _service = new StatisticService(data);
        }

        public static string Analyze(Operation operationType)
        {
            string result;
            switch (operationType)
            {
                case Operation.Absolute:
                    result = _service.CalculateAbsoluteFrequency();
                    break;
                case Operation.Relative:
                    result = _service.CalculateRelativeFrequency();
                    break;
                case Operation.Median:
                    result = _service.CalculateMedian();
                    break;
                case Operation.Dispersion:
                    result = _service.CalculateDispersion();
                    break;
                default:
                    throw new InvalidOperationException("Избраната операция е невалидна!");
            }
            return result;
        }
    }
}
