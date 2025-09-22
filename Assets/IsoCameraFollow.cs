using UnityEngine;

public class IsoCameraFollow : MonoBehaviour
{
    public Transform target;

    [Header("Iso-Winkel & Abstand")]
    public Vector3 isoEuler = new Vector3(30f, 45f, 0f);
    public float distance = 20f;  // egal bei Ortho, aber gut für Clipping

    [Header("Follow/Zoom")]
    [Range(0f, 1f)] public float followSmooth = 0.15f;
    public float zoomMin = 5f, zoomMax = 20f, zoomSpeed = 5f;

    Camera cam;
    Vector3 vel;

    void Awake() { cam = GetComponent<Camera>() ?? Camera.main; }

    void Start()
    {
        if (cam) cam.orthographic = true;
        transform.rotation = Quaternion.Euler(isoEuler);
    }

    void LateUpdate()
    {
        if (!target) return;

        // WICHTIG: Offset ALONG view direction → Ziel bleibt mittig
        Vector3 desired = target.position - transform.forward * distance;
        transform.position = Vector3.SmoothDamp(transform.position, desired, ref vel, followSmooth);

        // Zoom (Ortho Size)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (cam && Mathf.Abs(scroll) > 0.0001f)
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scroll * zoomSpeed, zoomMin, zoomMax);
    }
}
