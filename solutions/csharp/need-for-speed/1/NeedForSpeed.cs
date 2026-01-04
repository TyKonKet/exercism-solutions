class RemoteControlCar
{
    public int Speed { get; set; }

    public int BatteryDrain { get; set; }

    int DrivenDistance { get; set; }

    int Battery { get; set; } = 100;
    
    public RemoteControlCar(int speed, int batteryDrain)
    {
        Speed = speed;
        BatteryDrain = batteryDrain;
    }

    public bool BatteryDrained() => Battery < BatteryDrain;

    public int DistanceDriven() => DrivenDistance;

    public void Drive()
    {
        if (!BatteryDrained())
        {
            Battery -= BatteryDrain;
            DrivenDistance += Speed;
        }
    }

    public static RemoteControlCar Nitro() => new(50, 4);
}

class RaceTrack
{
    int Distance { get; set; }

    public RaceTrack(int distance) => Distance = distance;
    
    public bool TryFinishTrack(RemoteControlCar car) => ((100 / car.BatteryDrain) * car.Speed) >= Distance;
}
