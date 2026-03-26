using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    [Header("Links")]
    public CarController car;
    
    [Header("Cycle Cameras (Button C)")]
    public List<CinemachineCamera> cycleCameras; // 3-е лицо, Бампер, Капот
    
    [Header("Side Cameras (Numpad)")]
    public CinemachineCamera leftCam;
    public CinemachineCamera rightCam;

    private int currentCycleIndex = 0;
    private Controls controls;
    private CinemachineCamera activeSideCam = null;

    private void Awake()
    {
        controls = new Controls();
        
        // Циклический перебор на C
        controls.Player.ChangeCamera.performed += _ => SwitchCycleCamera();

        // Взгляд по сторонам (нажали - включили, отпустили - вернули основную)
        controls.Player.LookLeft.performed += _ => SetSideCamera(leftCam);
        controls.Player.LookLeft.canceled += _ => ResetSideCamera();
        
        controls.Player.LookRight.performed += _ => SetSideCamera(rightCam);
        controls.Player.LookRight.canceled += _ => ResetSideCamera();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Start() => ResetAllPriorities();

    void SwitchCycleCamera()
    {
        if (cycleCameras.Count == 0) return;
        currentCycleIndex = (currentCycleIndex + 1) % cycleCameras.Count;
        ResetAllPriorities();
    }

    void SetSideCamera(CinemachineCamera sideCam)
    {
        activeSideCam = sideCam;
        ResetAllPriorities();
    }

    void ResetSideCamera()
    {
        activeSideCam = null;
        ResetAllPriorities();
    }

    void ResetAllPriorities()
    {
        foreach (var cam in cycleCameras) cam.Priority = 10;
        if (leftCam) leftCam.Priority = 10;
        if (rightCam) rightCam.Priority = 10;

        if (activeSideCam != null) {
            activeSideCam.Priority = 30;
        } else {
            cycleCameras[currentCycleIndex].Priority = 20;
        }
    }
}
