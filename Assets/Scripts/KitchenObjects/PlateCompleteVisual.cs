using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] private Plate plate;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSOGameObjects;

    private void Start()
    {
        plate.OnIngredientAdded += Plate_OnIngredientAdded;

        foreach (var kitchenObjectSOGameObject in kitchenObjectSOGameObjects)
        {
            kitchenObjectSOGameObject.gameObject.SetActive(false);
        }
    }

    private void Plate_OnIngredientAdded(object sender, Plate.OnIngredientAddedEventArgs e)
    {
        foreach (var kitchenObjectSOGameObject in kitchenObjectSOGameObjects)
        {
            if (kitchenObjectSOGameObject.kitchenObjectSO == e.kitchenObjectSO)
            {
                kitchenObjectSOGameObject.gameObject.SetActive(true);
            }
        }   
    }
}
