using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Data (Auto-assigned)")]
    public CarConfig config;

    [Header("Physics Wheel Colliders")]
    public WheelCollider fl; public WheelCollider fr; 
    public WheelCollider rl; public WheelCollider rr; 

    [Header("Visual Meshes (Auto-assigned)")]
    [SerializeField] private Transform flMesh; [SerializeField] private Transform frMesh;
    [SerializeField] private Transform rlMesh; [SerializeField] private Transform rrMesh;

    private Rigidbody rb; 
    private Controls controls; 
    private Vector2 moveInput; 
    private bool isBraking;

    public float CurrentSpeed => rb != null ? rb.linearVelocity.magnitude : 0f;

    private void Awake()
    {
        controls = new Controls(); 
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);     
        Debug.Log("<color=cyan>[CarSystem]</color> Rigidbody и управление инициализированы.");
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    public void Initialize(CarConfig carData, GameObject spawnedPrefab)
    {
        if (carData == null) return;

        config = carData;
        var links = spawnedPrefab.GetComponent<CarVisualLinks>();

        if (links != null)
        {
            flMesh = links.fl; frMesh = links.fr;
            rlMesh = links.rl; rrMesh = links.rr;

            // 1. Сначала расставляем коллайдеры по координатам модели
            AlignPhysicsToModel();

            // 2. Настраиваем их параметры (радиус, трение)
            ConfigureWheel(fl, "Front Left"); 
            ConfigureWheel(fr, "Front Right");
            ConfigureWheel(rl, "Rear Left"); 
            ConfigureWheel(rr, "Rear Right");

            Debug.Log($"<color=green><b>[CarSystem] УСПЕХ:</b></color> {config.carName} собрана. База: {config.wheelBase}x{config.wheelTrack}");
        }
    }

    private void AlignPhysicsToModel()
{
    // Если конфига нет — выходим, чтобы не спамить ошибками
    if (config == null) return;

    float x = config.wheelTrack / 2f;
    float z = config.wheelBase / 2f;
    float y = config.wheelHeight;

    // Прямое назначение позиций
    fl.transform.localPosition = new Vector3(-x, y, z + config.centerOffsetZ);
    fr.transform.localPosition = new Vector3(x, y, z + config.centerOffsetZ);
    rl.transform.localPosition = new Vector3(-x, y, -z + config.centerOffsetZ);
    rr.transform.localPosition = new Vector3(x, y, -z + config.centerOffsetZ);
    
    // Debug.Log($"Обновление позиций: Base {config.wheelBase}, Track {config.wheelTrack}");
}


    private void ConfigureWheel(WheelCollider col, string wheelName)
    {
        if (col == null) return;
        col.radius = config.wheelRadius;
        UpdateFriction(col, config.normalStiffness);
    }

    private void Update()
    {
        if (config == null) return;
        moveInput = controls.Player.Move.ReadValue<Vector2>();
        isBraking = controls.Player.Handbrake.IsPressed();
    }

   private void FixedUpdate()
{
    if (config == null || fl == null) return;

    // ВРЕМЕННО ДОБАВЬ ЭТУ СТРОКУ ДЛЯ КАЛИБРОВКИ:
    AlignPhysicsToModel(); 

    ApplyEngine();
    ApplySteering();
    ApplyBrakesAndDrift();
    ApplyDownforce();
    UpdateAllWheels();
}

    private void ApplyEngine()
    {
        if (CurrentSpeed > config.maxSpeed) { rl.motorTorque = 0; rr.motorTorque = 0; return; }
        rl.motorTorque = moveInput.y * config.motorForce;
        rr.motorTorque = moveInput.y * config.motorForce;
    }

    private void ApplySteering()
    {
        float steer = moveInput.x * config.maxSteerAngle;
        fl.steerAngle = steer;
        fr.steerAngle = steer;
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

    private void ApplyDownforce() => rb.AddForce(-transform.up * 50f * CurrentSpeed);

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
        // Используем оффсет из конфига каждой машины
        mesh.rotation = rot * Quaternion.Euler(config.wheelRotationOffset);   
    }

    private void OnDrawGizmos()
    {
        // Рисуем только если игра запущена и конфиг назначен
        if (!Application.isPlaying || config == null) return;

        Gizmos.color = Color.green; // Цвет для физических точек
        
        // Рисуем маленькие сферы в позициях WheelColliders
        if (fl) Gizmos.DrawWireSphere(fl.transform.position, config.wheelRadius);
        if (fr) Gizmos.DrawWireSphere(fr.transform.position, config.wheelRadius);
        if (rl) Gizmos.DrawWireSphere(rl.transform.position, config.wheelRadius);
        if (rr) Gizmos.DrawWireSphere(rr.transform.position, config.wheelRadius);
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        // Позволяет видеть изменения базы и ширины прямо в редакторе без запуска игры
        if (config != null && fl != null) 
        {
            AlignPhysicsToModel();
        }
    }
    #endif


}
