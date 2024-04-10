using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using KostalApiClient.Model;
using Microsoft.VisualBasic.FileIO;
using RestSharp;

namespace KostalApiClient.Api
{
    public class LogDataEndpoint
    {
        private readonly KostalRestClient _client;

        public int DcInputs { get; set; } = 3;
        public int AcOutputs { get; set; } = 3;
        public int AnalogInputs { get; set; } = 4;
        public int ExternPowers { get; set; } = 3;
        public int AutoConsumptions { get; set; } = 3;


        internal LogDataEndpoint(KostalRestClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Download log data
        /// </summary>
        /// <param name="begin">From this date (time is ignored)</param>
        /// <param name="end">To this date (time is ignored)</param>
        /// <returns></returns>
        public async Task<LogDatas> GetLogData(DateTime begin, DateTime end)
        {
            _client.CheckAuthentication();

            RestRequest request = new("/logdata/download") {RequestFormat = DataFormat.Json};
            request.AddJsonBody(new {begin = begin.ToString("yyyy-MM-dd"), end = end.ToString("yyyy-MM-dd")});
            RestResponse response = await _client.ExecutePostAsync(request);
            DataTable dataTable = ReadCsvFile(response.Content, 6);
            LogDatas datas = new();
            foreach (DataRow row in dataTable.Rows)
            {
                string summaryCheck = row["KB S"].ToString();
                string errorCheck = row["ERR"].ToString();
                if (!string.IsNullOrEmpty(summaryCheck))
                {
                    datas.Summaries.Add(new LogSummary
                    {
                        Time = GetFromDateTime(row["zeit"]),
                        Status = GetFromString(row["KB S"]),
                        TotalEnergy = GetFromString(row["total E"]),
                        AutoConsumption = GetFromString(row["OWN E"]),
                        HomeConsumption = GetFromString(row["HOME E"]),
                        ResistanceIsolation = GetFromString(row["Iso R"])
                    });
                }
                else if (!string.IsNullOrEmpty(errorCheck) && errorCheck != "0")
                {
                    datas.Errors.Add(new LogError
                    {
                        Time = GetFromDateTime(row["zeit"]),
                        Error = GetFromString(row["Err"]),
                        NetworkMonitoringErr = GetFromString(row["ENS Err"])
                    });
                }
                else
                {
                    datas.Datas.Add(GetData(row));
                }
            }

            return datas;
        }

        private LogData GetData(DataRow row)
        {
            LogData data = new LogData
            {
                Time = GetFromDateTime(row["zeit"]),
                AcFreq = GetFromString(row["AC F"]),
                AcStatus = GetFromString(row["AC S"]),
                ResidualCurrent = GetFromString(row["FC I"]),
                GridConsumption = GetFromString(row["HC3 P"]),
                SolarConsumption = GetFromString(row["HC2 P"]),
                BatteryTemp = GetFromString(row["BAT Te"]),
                BatteryCycle = GetFromString(row["BAT Cy"]),
                StateOfCharge = GetFromString(row["SOC H"]),
                AutoConsumptions = new double?[AutoConsumptions],
                ExternPowers = new double?[ExternPowers],
                AnalogInputs = new double?[AnalogInputs],
                DcInputs = new DcInput[DcInputs],
                AcOutputs = new AcOutput[AcOutputs]
            };

            for (int i = 1; i <= AutoConsumptions; i++)
            {
                data.AutoConsumptions[i - 1] = GetFromString(row[$"SC{i} P"]);
            }

            for (int i = 1; i <= ExternPowers; i++)
            {
                data.ExternPowers[i - 1] = GetFromString(row[$"SH{i} P"]);
            }

            for (int i = 1; i <= AnalogInputs; i++)
            {
                data.AnalogInputs[i - 1] = GetFromString(row[$"Ain{i}"]);
            }

            for (int i = 1; i <= DcInputs; i++)
            {
                data.DcInputs[i - 1] = new DcInput
                {
                    Voltage = GetFromString(row[$"DC{i} U"]), Current = GetFromString(row[$"DC{i} I"]),
                    Power = GetFromString(row[$"DC{i} P"]), Temp = GetFromString(row[$"DC{i} T"]),
                    Status = GetFromString(row[$"DC{i} S"]),
                };
            }

            for (int i = 1; i <= 3; i++)
            {
                data.AcOutputs[i - 1] = new AcOutput
                {
                    Voltage = GetFromString(row[$"AC{i} U"]), Current = GetFromString(row[$"AC{i} I"]),
                    Power = GetFromString(row[$"AC{i} P"]), Temp = GetFromString(row[$"AC{i} T"]),
                };
            }

            return data;
        }

        private static DateTime GetFromDateTime(object o)
        {
            DateTime origin = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(Convert.ToInt64(o));
        }

        private static double? GetFromString(object o)
        {
            try
            {
                return Convert.ToDouble(o);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Convert csv string to <see cref="DataTable">DataTable</see>
        /// </summary>
        /// <param name="csvString">Csv string</param>
        /// <param name="skipLines">Number of lines to ignore</param>
        /// <returns></returns>
        private static DataTable ReadCsvFile(string csvString, int skipLines = 0)
        {
            DataTable csvData = new();
            try
            {
                using TextReader textReader = new StringReader(csvString);
                using TextFieldParser csvReader = new(textReader) {TextFieldType = FieldType.Delimited};
                csvReader.SetDelimiters("\t");
                // skip the 6 first lines
                for (int i = 0; i < skipLines; i++)
                {
                    csvReader.ReadLine();
                }

                bool tableCreated = false;
                while (tableCreated == false)
                {
                    string[] colFields = csvReader.ReadFields();
                    if (colFields != null)
                    {
                        foreach (string column in colFields)
                        {
                            DataColumn dateColumn = new(column) {AllowDBNull = true};
                            csvData.Columns.Add(dateColumn);
                        }
                    }

                    tableCreated = true;
                }

                while (!csvReader.EndOfData)
                {
                    csvData.Rows.Add(csvReader.ReadFields());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return csvData;
        }
    }
}