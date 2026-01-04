class RemoteControlCar
{
    private byte batteryLevel = 100;

    private uint drivenDistance = 0;
    
    public static RemoteControlCar Buy() => new();

    public string DistanceDisplay() => $"Driven {drivenDistance} meters";

    public string BatteryDisplay() => batteryLevel == 0 ? "Battery empty" : $"Battery at {batteryLevel}%" ;

    public void Drive()
    {
        if (batteryLevel > 0)
        {
            drivenDistance += 20;
            batteryLevel--;
        }
    }
}
