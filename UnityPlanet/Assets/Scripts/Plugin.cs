using UnityEngine;
using UnityEditor;

public class Plugin : MonoBehaviour {

	[MenuItem("Plugin/Create New Planet")]
    static void CreatePlanet()
    {
        GameObject newPlanet = new GameObject("Planet");
        newPlanet.AddComponent<Manager>().Initialize();
    }

}
