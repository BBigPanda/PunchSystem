using UnityEngine;

public class JiggleHead : MonoBehaviour
{
    [Header("Position Jiggle")] [SerializeField]
    private float jiggleStrength = 0.1f;

    [SerializeField] private float jiggleSpeed = 5f;

    [Header("Rotation Jiggle")] [SerializeField]
    private float rotationJiggleStrength = 5f;

    [SerializeField] private float rotationJiggleSpeed = 5f;

    private Vector3 originalPosition;
    private Vector3 velocity;

    private Quaternion originalRotation;
    private Vector3 angularVelocity;

    void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    public void AddJiggle(Vector3 force)
    {
        velocity += force * jiggleStrength;
        Vector3 torque = Vector3.Cross(Vector3.up, force).normalized;
        angularVelocity += torque * rotationJiggleStrength;
    }

    void Update()
    {
        DrowRaycast();
        // Position
        velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime * jiggleSpeed);
        transform.localPosition = originalPosition + velocity;

        // Rotation
        angularVelocity = Vector3.Lerp(angularVelocity, Vector3.zero, Time.deltaTime * rotationJiggleSpeed);
        Quaternion jiggleRotation = Quaternion.Euler(angularVelocity);
        transform.localRotation = originalRotation * jiggleRotation;
    }


    private void DrowRaycast()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Debug.DrawLine(transform.position, hit.point, Color.red, 1f);
                Vector3 vectorAB = hit.point - transform.position;
                AddJiggle(vectorAB);
            }
        }
    }
}