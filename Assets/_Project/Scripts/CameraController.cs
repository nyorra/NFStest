using UnityEngine;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    public CarController car;
    public CinemachineCamera vCam;

    [Header("FOV Dynamics")]
    public float minFOV = 60f;
    public float maxFOV = 85f;
    public float speedThreshold = 40f;
    public float fovSmoothTime = 5f;

    private void LateUpdate()
    {
        if (car == null || vCam == null) return;

        float speedFactor = Mathf.Clamp01(car.CurrentSpeed / speedThreshold);
        float targetFOV = Mathf.Lerp(minFOV, maxFOV, speedFactor);

        vCam.Lens.FieldOfView = Mathf.Lerp(vCam.Lens.FieldOfView, targetFOV, Time.deltaTime * fovSmoothTime);
    }
}
