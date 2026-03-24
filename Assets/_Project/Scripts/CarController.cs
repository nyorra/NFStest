using UnityEngine;
using UnityEngine.InputSystem; // Важно для новой системы ввода

public class CarController : MonoBehaviour
{
    [Header("Колеса (Физика)")]
    public WheelCollider fl;
    public WheelCollider fr; 
    public WheelCollider rl;
    public WheelCollider rr; 

    [Header("Колеса (Визуал)")]
    public Transform flMesh;
    public Transform frMesh;
    public Transform rlMesh;
    public Transform rrMesh;

    [Header("Настройки")]
    public float motorForce = 2000f; 
    public float breakForce = 3000f; 
    public float maxSteer = 30f; 

    // Поля для новой системы ввода
    private Controls controls; 
    private Vector2 moveInput; 
    private bool isBraking;

    private void Awake()
    {
        // Инициализируем конфиг, который ты создал в Unity
        controls = new Controls(); 
    }

    // Включаем и выключаем ввод (обязательно для корректной работы)
    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update()
    {
        // Читаем ввод каждый кадр
        moveInput = controls.Player.Move.ReadValue<Vector2>();
        isBraking = controls.Player.Handbrake.IsPressed();
    }

    private void FixedUpdate()
    {
        // Используем данные, полученные в Update()
        // y — это W/S (Vertical), x — это A/D (Horizontal)
        float v = moveInput.y; 
        float h = moveInput.x;

        // 1. Тяга на задние колеса
        rl.motorTorque = v * motorForce;
        rr.motorTorque = v * motorForce;

        // 2. Поворот передних колес
        fl.steerAngle = h * maxSteer;
        fr.steerAngle = h * maxSteer;

        // 3. Логика торможения (через ручник/пробел)
        if (isBraking) {
            ApplyBrakes(breakForce);
        } 
        else { 
            ApplyBrakes(0);
        }

        // 4. Обновление визуальных мешей колес
        UpdateWheel(fl, flMesh);
        UpdateWheel(fr, frMesh);
        UpdateWheel(rl, rlMesh);
        UpdateWheel(rr, rrMesh);
    }

    void UpdateWheel(WheelCollider col, Transform mesh)
    {
        if (mesh == null) return;
        Vector3 pos;
        Quaternion rot;
        col.GetWorldPose(out pos, out rot);
        mesh.position = pos;
        mesh.rotation = rot;   
    }

    void ApplyBrakes(float force)
    {
        fl.brakeTorque = force; 
        fr.brakeTorque = force;
        rl.brakeTorque = force; 
        rr.brakeTorque = force;
    }
}
