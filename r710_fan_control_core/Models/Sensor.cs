using System.ComponentModel.DataAnnotations;

namespace r710_fan_control_core.Models
{
    public class Sensor
    {
        [Display(Name = "Probe Name")]
        public string ProbeName { get; set; }
        public string Reading { get; set; }

        public Measurement Measurement { get; private set; }

        public void SetMeasurement(string value)
        {
            switch (value)
            {
                case "Amps":
                    Measurement = Measurement.Amps;
                    break;
                case "degrees C":
                    Measurement = Measurement.DegreesC;
                    break;
                case "discrete":
                    Measurement = Measurement.Discrete;
                    break;
                case "RPM":
                    Measurement = Measurement.RPM;
                    break;
                case "Volts":
                    Measurement = Measurement.Volts;
                    break;
                case "Watts":
                    Measurement = Measurement.Watts;
                    break;
                default:
                    Measurement = Measurement.None;
                    break;
            }
        }

        public string Status { get; set; }
        public Thresholds Thresholds { get; set; }
    }

    public class Thresholds
    {
        public Warning Warning { get; set; }
        public Failure Failure { get; set; }
    }

    public class Warning
    {
        public ushort? Min { get; set; }
        public ushort? Max { get; set; }
    }

    public class Failure
    {
        public ushort? Min { get; set; }
        public ushort? Max { get; set; }
    }

    public enum Measurement
    {
        None,
        Amps,

        [Display(Name = "Degrees C")]
        DegreesC,

        Discrete,
        RPM,
        Volts,
        Watts
    }
}
