using System;

namespace KostalApiClient.Model
{
    public class LogData
    {
        public DateTime Time { get; set; }
        public double? AcFreq { get; set; }
        public double? AcStatus { get; set; }
        public double? ResidualCurrent { get; set; }

        public double? GridConsumption { get; set; }
        public double? SolarConsumption { get; set; }
        public double? BatteryTemp { get; set; }
        public double? BatteryCycle { get; set; }
        public double? StateOfCharge { get; set; }

        public DcInput[] DcInputs { get; set; }
        public AcOutput[] AcOutputs { get; set; }
        public double?[] AnalogInputs{ get; set; }
        public double?[] ExternPowers { get; set; }
        public double?[] AutoConsumptions { get; set; }

    }
}