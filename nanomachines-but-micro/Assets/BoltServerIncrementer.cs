using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class BoltServerIncrementer : Bolt.GlobalEventListener
{
    [SerializeField]public static int connectCount = 1; //1 because host is 1
}
