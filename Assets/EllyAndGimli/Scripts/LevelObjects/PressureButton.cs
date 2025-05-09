using System;
using UnityEngine;

public class PressureButton : MonoBehaviour
{
    [Header("Что запускаем")]
    public MovingPlatform platform;

    [Header("Какими слоями можно нажать кнопку")]
    public LayerMask activatorLayers;

    private int objectsOnPlate = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsActivator(other.gameObject)) return;

        objectsOnPlate++;
        if (objectsOnPlate >= 1)
            platform.StartMoving();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsActivator(other.gameObject)) return;

        objectsOnPlate--;
        if (objectsOnPlate <= 0)
            platform.StopMoving();
    }

    bool IsActivator(GameObject obj)
    {
        // Сравниваем слой объекта с битовой маской-разрешением
        return (activatorLayers.value & (1 << obj.layer)) != 0;
    }
}