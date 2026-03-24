using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [Header("Data")]
    public CarConfig config;

    [Header("Physics")]
    public WheelCollider fl; public WheelCollider fr; 
    public WheelCollider rl; public WheelCollider rr; 

    [Header("Visual")]
    public Transform flMesh; public Transform frMesh;
    public Transform rlMesh; public Transform rrMesh;

    private Rigidbody rb; 
    private Controls controls; 
    private Vector2 moveInput; 
    private bool isBraking;

    public float CurrentSpeed => rb != null ? rb.linearVelocity.magnitude : 0f;

    private void Awake()
    {
        controls = new Controls(); 
        rb = GetComponent<Rigidbody>();
        if (rb == null) Debug.LogError("Rigidbody Missing!");    
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update()
    {
        moveInput = controls.Player.Move.ReadValue<Vector2>();
        isBraking = controls.Player.Handbrake.IsPressed();
    }

    private void FixedUpdate()
    {
        ApplyEngine();
        ApplySteering();
        ApplyBrakesAndDrift();
        ApplyDownforce();
        UpdateAllWheels();
    }

    private void ApplyEngine()
    {
        rl.motorTorque = moveInput.y * config.motorForce;
        rr.motorTorque = moveInput.y * config.motorForce;
    }

    private void ApplySteering()
    {
        fl.steerAngle = moveInput.x * config.maxSteer;
        fr.steerAngle = moveInput.x * config.maxSteer;
    }

    private void ApplyBrakesAndDrift()
    {
        float currentBrake = isBraking ? config.breakForce : 0;
        float currentStiffness = isBraking ? config.driftStiffness : config.normalStiffness;

        fl.brakeTorque = fr.brakeTorque = rl.brakeTorque = rr.brakeTorque = currentBrake;

        UpdateFriction(rl, currentStiffness);
        UpdateFriction(rr, currentStiffness);
    }

    private void UpdateFriction(WheelCollider wheel, float stiffness)
    {
        WheelFrictionCurve sf = wheel.sidewaysFriction;
        sf.stiffness = stiffness;
        wheel.sidewaysFriction = sf;
    }

    private void ApplyDownforce()
    {
        rb.AddForce(-transform.up * 100f * CurrentSpeed);
    }

    private void UpdateAllWheels()
    {
        UpdateWheel(fl, flMesh); UpdateWheel(fr, frMesh);
        UpdateWheel(rl, rlMesh); UpdateWheel(rr, rrMesh);
    }

    private void UpdateWheel(WheelCollider col, Transform mesh)
    {
        if (mesh == null) return;
        Vector3 pos; Quaternion rot;
        col.GetWorldPose(out pos, out rot);
        mesh.position = pos;
        mesh.rotation = rot;   
    }
}
