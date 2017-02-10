using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(Planet))]
public class Manager : MonoBehaviour {
    
    [HideInInspector]
    public Planet planet;

    [HideInInspector]
    public GameObject highlight;

    public void Initialize()
    {
        planet = GetComponent<Planet>();
        planet.Initialize();

        highlight = new GameObject("Highlight");
    }
}
