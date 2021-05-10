using System;

namespace KostalApiClient.Model
{
    public class LogError
    {
        public DateTime Time { get; set; }
        public double? Error { get; set; }
        public double? NetworkMonitoringErr { get; set; }
    }
}