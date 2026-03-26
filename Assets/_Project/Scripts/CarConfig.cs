using UnityEngine;

[CreateAssetMenu(fileName = "NewCarConfig", menuName = "Racing/Car Config")]
public class CarConfig : ScriptableObject 
{
    [Header("Engine Settings")]
    public float motorForce = 2000f;     // Мощность
    public float maxSpeed = 200f;        // Макс. скорость
    public float breakForce = 3000f;     // Сила тормозов

    [Header("Steering Settings")]
    public float maxSteerAngle = 35f;    // Угол поворота колес
    [Range(0, 1)] 
    public float steeringSensitivity = 0.5f; // Чувствительность руля

    [Header("Drift Settings")]
    public float driftStiffness = 0.5f;  // Насколько сильно заносит
    public float normalStiffness = 1.0f; // Обычный зацеп
}
