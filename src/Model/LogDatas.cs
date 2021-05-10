using System.Collections.Generic;

namespace KostalApiClient.Model
{
    public class LogDatas
    {
        public List<LogData> Datas { get; set; } = new ();
        public List<LogSummary> Summaries { get; set; }= new ();
        public List<LogError> Errors { get; set; }= new ();
    }
}