using UnityEngine;
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
#endif

public class IsoCameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 isoEuler = new Vector3(30f, 45f, 0f);
    public float distance = 20f;
    [Range(0f, 1f)] public float followSmooth = 0.15f;
    public float zoomMin = 5f, zoomMax = 20f, zoomSpeed = 5f;

    Camera cam;
    Vector3 vel;

    void Awake() { cam = GetComponent<Camera>() ?? Camera.main; }
    void Start() { if (cam) cam.orthographic = true; transform.rotation = Quaternion.Euler(isoEuler); }

    void LateUpdate()
    {
        if (!target) return;

        // Folgen
        Vector3 desired = target.position - transform.forward * distance;
        transform.position = Vector3.SmoothDamp(transform.position, desired, ref vel, followSmooth);

        // --- Scroll auslesen (neu ODER alt) ---
        float scroll = 0f;
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
        if (Mouse.current != null)
            scroll = Mouse.current.scroll.ReadValue().y / 120f; // ~1 pro Rädchen-Klick
#else
        scroll = Input.GetAxis("Mouse ScrollWheel");
#endif

        if (cam && Mathf.Abs(scroll) > 0.0001f)
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scroll * zoomSpeed, zoomMin, zoomMax);
    }
}
