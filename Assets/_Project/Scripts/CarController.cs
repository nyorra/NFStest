using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [Header("Колеса (Физика)")]
    public WheelCollider fl;
    public WheelCollider fr; // front
    public WheelCollider rl;
    public WheelCollider rr; // rear - зад

    [Header("Колеса (Визуал)")]
    public Transform flMesh;
    public Transform frMesh;
    public Transform rlMesh;
    public Transform rrMesh;

    [Header("Настройки")]
    public float motorForce = 2000f; // engine
    public float breakForce = 3000f; // brake
    public float maxSteer = 30f; // angle

    private void FixedUpdate(){

        // 1. Read the input W/S and A/D or ARROWs
        float v = Input.GetAxis("Vertical"); // forward 1, backward -1
        float h = Input.GetAxis("Horizontal"); // right 1, left -1;

        // 2. Force to the rear wheel
        rl.motorTorque = v * motorForce;
        rr.motorTorque = v * motorForce;

        // 3. Turn forward wheels
        fl.steerAngle = h * maxSteer;
        fr.steerAngle = h * maxSteer;

        if (Input.GetKey(KeyCode.Space)) {
            ApplyBrakes(breakForce);
        } 
        else { 
            ApplyBrakes(0);
        }

        UpdateWheel(fl, flMesh);
        UpdateWheel(fr, frMesh);
        UpdateWheel(rl, rlMesh);
        UpdateWheel(rr, rrMesh);
    }

    void UpdateWheel(WheelCollider col, Transform mesh){
        if (mesh == null) return;
        Vector3 pos;
        Quaternion rot;
        col.GetWorldPose(out pos, out rot);
        mesh.position = pos;
        mesh.rotation = rot;   
    }

    void ApplyBrakes(float  force)
    {
        fl.brakeTorque = force; fr.brakeTorque = force;
        rl.brakeTorque = force; rr.brakeTorque = force;
    }
}
