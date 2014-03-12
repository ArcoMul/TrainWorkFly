using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AirplaneManager : MonoBehaviour {

    public static AirplaneManager Instance;

    [SerializeField]
    public List<Airplane> AirplanePrefabsToSpawn;
    public int position = 0;

    void Awake()
    {
        Instance = this;
        SpawnNextAirplane();
    }

    public void SpawnNextAirplane()
    {
        if (AirplanePrefabsToSpawn.Count <= position)
        {
            Application.LoadLevel("GameWon");
            return;
        }
        Instantiate(AirplanePrefabsToSpawn[position], GameObject.Find("StartPoint").transform.position, Quaternion.identity);
        position++;
        GameObject.Find("LevelText").GetComponent<TextMesh>().text = "Level: " + position;
    }
}
