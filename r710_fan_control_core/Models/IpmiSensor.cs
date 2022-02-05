using System.ComponentModel.DataAnnotations;

namespace r710_fan_control_core.Models
{
    public class IpmiSensor
    {
        [Display(Name = "Probe Name")]
        public string ProbeName { get; set; }
        public string Reading { get; set; }

        public Measurement Measurement { get; private set; }

        public void SetMeasurement(string value)
        {
            Measurement = value switch
            {
                "Amps" => Measurement.Amps,
                "degrees C" => Measurement.DegreesC,
                "discrete" => Measurement.Discrete,
                "RPM" => Measurement.RPM,
                "Volts" => Measurement.Volts,
                "Watts" => Measurement.Watts,
                _ => Measurement.None,
            };
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
