using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
public class MeshDeformer : MonoBehaviour
{
    public float deformRadius = 0.5f;
    public float deformStrength = 0.3f;

    private Mesh deformingMesh;
    private Vector3[] originalVertices;
    private Vector3[] displacedVertices;

    void Start()
    {
        deformingMesh = GetComponent<MeshFilter>().mesh = Instantiate(GetComponent<MeshFilter>().mesh);
        originalVertices = deformingMesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
        originalVertices.CopyTo(displacedVertices, 0);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {
                DeformMesh(hit.point, hit.normal);
                UpdateMesh();
            }
        }
    }

    void DeformMesh(Vector3 point, Vector3 normal)
    {
        Vector3 localPoint = transform.InverseTransformPoint(point);

        for (int i = 0; i < displacedVertices.Length; i++)
        {
            Vector3 vertex = displacedVertices[i];
            float distance = Vector3.Distance(vertex, localPoint);

            if (distance < deformRadius)
            {
                float falloff = 1f - (distance / deformRadius);
                displacedVertices[i] += -normal * (deformStrength * falloff);
            }
        }
    }

    void UpdateMesh()
    {
        deformingMesh.vertices = displacedVertices;
        deformingMesh.RecalculateNormals();
        deformingMesh.RecalculateBounds();
        GetComponent<MeshCollider>().sharedMesh = deformingMesh; 
    }
}