using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    private Mesh mesh;
    GameObject gameObject;
    float angle;
    Vector3 origin;
    Vector3 forward;
    public Vector3[] vertices;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        gameObject = GameObject.Find("Bodyguard");
    }

    void Update()
    {
        float fov = 90f;
        origin = gameObject.transform.position;
        forward = gameObject.transform.forward;
        int rayCount = 10;
        Vector3 axis = new Vector3 (0, 0, 1);
        if (forward.x < 0) {
            angle = Vector3.Angle(axis, forward) * -1;
        } else {
            angle = Vector3.Angle(axis, forward);
        }
        
        float angleInc = fov / rayCount;
        float viewDist = 10f;

        vertices = new Vector3[rayCount + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;
        Quaternion rotation = Quaternion.Euler(-90, -45, 0);


        int vIndex = 1;
        int tIndex = 0;
        for (int i = 0; i <= rayCount; i++) {
            Vector3 vertex;
            Vector3 temp = rotation * VecFromAngle(angle);

            RaycastHit hit;
            if (Physics.Raycast (origin, temp, out hit, viewDist))
            {
                vertex = temp * hit.distance;
                Debug.Log(hit.point);
                Debug.DrawRay(origin, temp * hit.distance, Color.yellow);
                // Debug.Log("Did Hit");
            } else
            {
                vertex = origin + temp * viewDist;
                Debug.DrawRay(origin, temp * viewDist, Color.red);
                // Debug.Log("Did not Hit");
            }

            vertices[vIndex] = vertex;

            if (i > 0)
            {
                triangles[tIndex] = 0;
                triangles[tIndex + 1] = vIndex - 1;
                triangles[tIndex + 2] = vIndex;

                tIndex += 3;    
            }
            vIndex++;

            angle -= angleInc;
            
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

    }

    Vector3 VecFromAngle (float angle)
    {
        float angleRad = angle * (Mathf.PI/180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}
