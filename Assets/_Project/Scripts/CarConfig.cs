using UnityEngine;

[CreateAssetMenu(fileName = "NewCarConfig", menuName = "Racing/Car Config")]
public class CarConfig : ScriptableObject
{
    [Header("Engine & Handling")]
    public float motorForce = 2000f; 
    public float breakForce = 3000f; 
    public float maxSteer = 45f; 

    [Header("Drift Settings")]
    public float normalStiffness = 1.0f;
    public float driftStiffness = 0.5f; 
}
