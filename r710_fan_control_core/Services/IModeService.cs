namespace r710_fan_control_core.Services
{
    public interface IModeService
    {
        void Auto();
        void AutoLow();
        void Manual(int speedPercent);
    }
}