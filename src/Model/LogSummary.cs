using System;

namespace KostalApiClient.Model
{
    public class LogSummary
    {
        public DateTime Time { get; set; }
        public double? TotalEnergy { get; set; }
        public double? Status { get; set; }
        public double? HomeConsumption { get; set; }
        public double? ResistanceIsolation { get; set; }
        public double? AutoConsumption { get; set; }
    }
}