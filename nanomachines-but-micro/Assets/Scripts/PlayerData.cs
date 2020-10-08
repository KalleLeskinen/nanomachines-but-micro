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
    public PlayerData(Guid id, List<int> cpP, List<float> lap)
    {
        this.id = id;
        this.checkpointsPassed = cpP;
        this.lapTimes = lap;
    }
}
