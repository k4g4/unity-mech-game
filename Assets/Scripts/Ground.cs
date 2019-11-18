using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour {
    public int dim;
    public float scale;
    public Vector3[] hills;

	void Start () {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        Vector3[] vertices = new Vector3[dim * dim];
        Vector3[] normals = new Vector3[vertices.Length];
        Vector2[] uv = new Vector2[vertices.Length];

        float x, y = 0f, z;
        for (int i = 0; i < vertices.Length; i++)
        {
            x = (float)(i % dim) / dim;
            z = (float)(i / dim) / dim;
            vertices[i] = new Vector3(x - .5f, y, z - .5f) * scale;
            normals[i] = vertices[i] + Vector3.up;
            uv[i] = new Vector2(x, z);
        }

        int[] triangles = new int[(dim - 1) * (dim - 1) * 6];

        for (int vi = 0, ti = 0; vi < vertices.Length; vi++)
        {
            if (vi % dim == dim - 1 || vi / dim == dim - 1) continue;
            triangles[ti++] = vi + 1;
            triangles[ti++] = vi;
            triangles[ti++] = vi + dim + 1;
            triangles[ti++] = vi;
            triangles[ti++] = vi + dim;
            triangles[ti++] = vi + dim + 1;
        }

        foreach (Vector3 hill in hills)
        {
            vertices[(int)(hill.z * dim + hill.x)].y = hill.y;
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}
