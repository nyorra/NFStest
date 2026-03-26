using UnityEngine;
using System.Collections.Generic;

public class CarInit : MonoBehaviour
{
    [Header("Настройки спавна")]
    [SerializeField] private List<CarConfig> allCars; // Список всех конфигов (Skyline, Lancer)
    [SerializeField] private Transform visualRoot;    // Пустышка внутри PlayerCar
    
    private void Awake()
    {
        // 1. Узнаем, какую машину выбрал игрок в гараже (по умолчанию 0)
        int index = PlayerPrefs.GetInt("SelectedCarIndex", 0);
        
        // Проверка на ошибки в списке
        if (index < 0 || index >= allCars.Count) index = 0;

        ApplyConfig(allCars[index]);
    }

    private void ApplyConfig(CarConfig config)
    {
        if (config == null || config.visualPrefab == null) return;

        // 2. Очищаем место спавна (если там что-то было)
        foreach (Transform child in visualRoot) Destroy(child.gameObject);

        // 3. Создаем визуальную модель из префаба внутрь VisualRoot
        GameObject instance = Instantiate(config.visualPrefab, visualRoot);
        instance.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        // 4. Связываем визуал с контроллером физики
        var controller = GetComponent<CarController>();
        if (controller != null)
        {
            controller.config = config; // Передаем настройки мотора/тормозов
            controller.Initialize(config, instance); // Передаем меши колес
        }
    }
}
