using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(Manager))]
public class editor : Editor {

    Light highlight;
    Manager manager;

    void OnSceneGUI()
    {
        manager = (Manager)target;
        highlight = manager.highlight.GetComponent<Light>();

        //without component
        if (highlight == null)
        {
            highlight = manager.highlight.AddComponent<Light>();
            highlight.type = LightType.Spot;    //spot light
            highlight.color = Color.red;        //color
            highlight.intensity = 8;            //intensity
        }

        highlight.transform.position = Camera.current.transform.position;
        highlight.range = 2 * Vector3.Distance(highlight.transform.position, manager.transform.position);
        highlight.transform.LookAt(manager.transform);

        if(Event.current.keyCode == KeyCode.W || Event.current.keyCode == KeyCode.S)
        {
            Debug.Log(manager.planet.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
            ModifyTerrain();
        }
    }

    void ModifyTerrain()
    {
        Mesh planetMesh = manager.planet.GetComponent<MeshFilter>().sharedMesh;
        List<Vector3> newVertices = new List<Vector3>();

        for(int i = 0; i < planetMesh.vertices.Length; i++)
        {
            //we need know the angle between the vertice and planet center
            float verticesToCenter = Vector3.Distance(planetMesh.vertices[i], manager.planet.transform.position);   //c
            float verticesToLight = Vector3.Distance(planetMesh.vertices[i], highlight.transform.position);         //a
            float lightToCenter = Vector3.Distance(highlight.transform.position, manager.planet.transform.position);        //b
            //餘弦定理  cos = (a^2 + b^2 - c^2) / (2ab)
            float angle = Mathf.Acos(
                (Mathf.Pow(verticesToLight, 2) + Mathf.Pow(lightToCenter, 2) - Mathf.Pow(verticesToCenter, 2)) /
                (2 * verticesToLight * lightToCenter)) * 180 / Mathf.PI;

            if (angle < (highlight.spotAngle / 2) && verticesToLight < lightToCenter)     //verticesToLight > lightToCenter代表vertices在球的另外一面
            {
                Vector3 target = Vector3.zero;

                if (Event.current.keyCode == KeyCode.W)
                {
                    target = (planetMesh.vertices[i] - manager.planet.transform.position).normalized + planetMesh.vertices[i];
                }
                else if(Event.current.keyCode == KeyCode.S)
                {
                    target = manager.planet.transform.position;
                }

                Vector3 newVect = Vector3.MoveTowards(planetMesh.vertices[i], target,  0.01f);
                newVertices.Add(newVect);
            }
            else
                newVertices.Add(planetMesh.vertices[i]);
        }

        Mesh newMesh = new Mesh();
        newMesh.name = "PlanetMesh";
        newMesh.vertices = newVertices.ToArray();
        newMesh.triangles = planetMesh.triangles;
        newMesh.normals = planetMesh.normals;
        newMesh.uv = planetMesh.uv;

        manager.planet.GetComponent<MeshFilter>().sharedMesh = newMesh;
    }

}
