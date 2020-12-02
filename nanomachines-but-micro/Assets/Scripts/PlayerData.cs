using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class PlayerData
{
    public Guid id;
    public List<int> checkpointsPassed;
    public List<float> lapTimes;
    public string name;
    public float bestLap;
    public PlayerData(Guid id, List<int> cpP, List<float> lap, string name)
    {
        this.id = id;
        this.checkpointsPassed = cpP;
        this.lapTimes = lap;
        this.name = name;
    }
    public void ClearCheckpointsList()
    {
        checkpointsPassed = new List<int>();
    }

    public float FindBestLap()
    {
        float bestLapFound = 999;
        foreach (float laptime in lapTimes)
        {
            if (laptime < bestLapFound)
            {
                bestLapFound = laptime;
            }
        }
        decimal retval = Math.Round((decimal)bestLapFound, 3);
        return (float)retval;
    }
}
