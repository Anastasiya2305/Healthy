using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 2f, -10f);
    [SerializeField] private float smoothSpeed = 8f;
    [SerializeField] private bool preserveCurrentZ = true;

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 desiredPosition = target.position + offset;
        desiredPosition.z = preserveCurrentZ ? transform.position.z : -10f;

        float t = Mathf.Clamp01(smoothSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, t);
    }
}
