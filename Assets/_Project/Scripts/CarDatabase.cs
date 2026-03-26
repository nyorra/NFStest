using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CarDatabase", menuName = "Racing/Car Database")]
public class CarDatabase : ScriptableObject 
{
    public List<CarConfig> cars;
}