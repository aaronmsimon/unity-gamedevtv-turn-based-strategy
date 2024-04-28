using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private static MouseWorld instance;

    [SerializeField] private LayerMask mousePlaneLayerMask;

    private void Awake()
    {
        // Since static
        instance = this;
    }

    public static Vector3 GetPosition()
    {
        // Set a ray from the screen to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Shoot a raycast infinite distance until it hits the ground layer only
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.mousePlaneLayerMask);
        // Return the raycast point, which will be on the ground layer
        return raycastHit.point;
    }
}
