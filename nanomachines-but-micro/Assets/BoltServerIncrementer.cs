using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class BoltServerIncrementer : Bolt.GlobalEventListener
{
    // aina kun uusi pelaaja liittyy peliin, tämä kutsutaan. (ei hostille)
    public override void Connected(BoltConnection connection)
    {
        GameObject.FindGameObjectWithTag("RaceHandler").GetComponent<BoltEntity>().GetState<IStateOfRace>().NumberOfPlayers += 1;

    }

}
