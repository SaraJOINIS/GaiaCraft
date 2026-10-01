using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSlicerScaffolding : MonoBehaviour
{
    [SerializeField] private MeshFilter _meshFilter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
    }

    public GameObject[] SliceMesh(Vector3 planeNormal, Vector3 planeOrigin)
    {
        Mesh[] meshes = MeshSlicer.SliceMesh(_meshFilter.sharedMesh, planeNormal, planeOrigin);
        GameObject[] gameObjects = new GameObject[meshes.Length];
        for (int index = 0; index < meshes.Length; index++)
        {
            Mesh mesh = meshes[index];
            GameObject submesh = Instantiate(this.gameObject);
            submesh.GetComponent<MeshFilter>().sharedMesh = mesh;
            gameObjects[index] = submesh;
        }
        return gameObjects;
    }
}
