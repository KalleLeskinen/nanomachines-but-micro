using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class SetCarName : MonoBehaviour
{
    private string[] adj = {
        "Adamant","Adroit","Amatory","Animistic","Antic","Arcadian","Baleful","Bellicose","Bilious","Boorish","Calamitous","Caustic","Cerulean","Comely","Concomitant","Contumacious","Corpulent","Crapulous","Defamatory","Didactic","Dilatory","Dowdy","Efficacious","Effulgent","Egregious","Endemic","Equanimous","Execrable","Fastidious","Feckless","Fecund","Friable","Fulsome","Garrulous","Guileless","Gustatory","Heuristic","Histrionic","Hubristic","Incendiary","Insidious","Insolent","Intransigent","Inveterate","Invidious","Irksome","Jejune","Jocular","Judicious","Lachrymose","Limpid","Loquacious","Luminous","Mannered","Mendacious","Meretricious","Minatory","Mordant","Munificent","Nefarious","Noxious","Obtuse","Parsimonius","Pendulous","Pernicious","Pervasive","Petulant","Platitudinou","Precipitate","Propitious","Puckish","Querulous","Quiescent","Rebarbative","Recalcitrant","Redolent","Rhadamanthine","Risible","Ruminative","Sagacious","Salubrious","Sartorial","Sclerotic","Serpentine","Spasmodic","Strident","Taciturn","Tenacious","Tremulous","Trenchant","Turbulent","Turgid","Ubiquitous","Uxorious","Verdant","Voluble","Voracious","Wheedling","Withering","Zealous"
    };
    private string[] subst =
    {
        "Lawyer","Chef","SOFTWARE ENGINEER FYEAH","Plumber","Painter","Musician","Architect","Farmhand","Prostitute","Doctor","Nurse","Stenographer","Artist","Soldier","Truck Driver","Drunk Driver","Mason","Freemason","Carrot","Cabbage","Tomato","Potato","Onion","Garlic","Cucumber","Pumpkin","Lettuce","Celery","Eggplant","Ginger","Capsicum","Kale","Chili","Elephant","Hound","Kangaroo","Crocodile","Lynx","Platypus","Goat","Wolfie","Rabbit","Bear","Hamster","Chihuahua","Rock Hyrax","Frog","Toad","Merge Conflict","Dragon","Orc","Unicorn","Goblin"
    };
    
    private void Start()
    {
        GameObject.FindGameObjectWithTag("score_panel").SetActive(false);
        if (BoltNetwork.IsServer)
        {
            GameObject.FindGameObjectWithTag("client_start_ui").SetActive(false);
        }
        else
        {
            GameObject.FindGameObjectWithTag("host_name_laps_ui").SetActive(false);
        }

    }
    public void SetCarNameForMyEntity()
    {
        if (BoltNetwork.IsClient && Time.timeSinceLevelLoad > 3.1f)
        {
            foreach (var obj in BoltNetwork.Entities)
            {
                if (obj.IsOwner&&obj.StateIs<IVehicleState>())
                {
                    string nametoset = GameObject.FindGameObjectWithTag("player_name").GetComponent<Text>().text;
                    if (nametoset == "")
                    {
                        nametoset = GenerateRandom();
                    }
                    Debug.Log("My object was " + obj.name);
                    Debug.Log("field text was " + nametoset);
                    obj.GetState<IVehicleState>().PlayerName = nametoset;
                    var ClientReadyEvent = PlayerReadyEvent.Create();
                    ClientReadyEvent.name = nametoset;
                    ClientReadyEvent.Send();
                }
            }

            GameObject.FindGameObjectWithTag("client_start_ui").SetActive(false);
        }
        else
        {
            GameObject.FindGameObjectWithTag("RaceHandler").GetComponent<RaceScript>().UpdatePlayerBase();
        }
    }

    private string GenerateRandom()
    {
        var random = new Random();
        string name1 = adj[random.Next(0, adj.Length)];
        string name2 = subst[random.Next(0, subst.Length)];
        return $"{name1} {name2}";
    }

    public void HostSetCarNameAndNumberOfLaps()
    {
        if (BoltNetwork.IsServer && Time.timeSinceLevelLoad > 3.1f)
        {
            Debug.Log("HOST!");
            foreach (var obj in BoltNetwork.Entities)
            {
                if (obj.StateIs<IStateOfRace>() && BoltNetwork.IsServer)
                {
                    int nbOfLaps = Mathf.FloorToInt(GameObject.FindGameObjectWithTag("host_name_laps_ui").GetComponentInChildren<Slider>().value);
                    obj.GetState<IStateOfRace>().NumberOfLaps = nbOfLaps;
                }
                if (obj.IsOwner && obj.StateIs<IVehicleState>())
                {
                    string nametoset = GameObject.FindGameObjectWithTag("player_name").GetComponent<Text>().text;
                    if (nametoset == "")
                    {
                        nametoset = GenerateRandom();
                    }
                    obj.GetState<IVehicleState>().PlayerName = nametoset;
                    var hostready = HostReadyEvent.Create();
                    hostready.name = nametoset;
                    hostready.Send();
                }
            }
            GameObject.FindGameObjectWithTag("host_name_laps_ui").SetActive(false);
        }
    }
}
