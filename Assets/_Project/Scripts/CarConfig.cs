using UnityEngine;

[CreateAssetMenu(fileName = "NewCarConfig", menuName = "Racing/Car Config")]
public class CarConfig : ScriptableObject 
{
    [Header("Main Settings")]
    public string carName;
    public GameObject visualPrefab;

    [Header("UI Stats")]
    [Range(0, 100)] public float topSpeedStat;
    [Range(0, 100)] public float handlingStat;
    
    [Header("Engine Settings")]
    public float motorForce;
    public float maxSpeed;
    public float breakForce;

    [Header("Wheel Placement (Offsets)")]
    public float wheelTrack = 1.5f;   // Ширина (расстояние между левым и правым)
    public float wheelBase = 2.3f;    // База (расстояние между передним и задним)
    public float wheelHeight = 0.5f;  // Высота коллайдера от земли
    public float centerOffsetZ = 0f;  // Смещение всей оси вперед/назад (если модель не в центре)

    [Header("Visual Correction")]
    public Vector3 wheelRotationOffset = new Vector3(0, 0, -90);

    [Header("Wheel Settings")]
    public float wheelRadius;

    [Header("Steering Settings")]
    public float maxSteerAngle;
    [Range(0, 1)] public float steeringSensitivity;

    [Header("Drift Settings")]
    public float driftStiffness;
    public float normalStiffness;
}
