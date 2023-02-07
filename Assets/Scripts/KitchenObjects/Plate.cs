using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOs;

    private List<KitchenObjectSO> kitchenObjectSOs;

    private void Awake()
    {
        kitchenObjectSOs = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (kitchenObjectSOs.Contains(kitchenObjectSO)) { return false; }
        if (!validKitchenObjectSOs.Contains(kitchenObjectSO)) { return false; }

        kitchenObjectSOs.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
        {
            kitchenObjectSO = kitchenObjectSO,
        });
        return true;
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOs;
    }
}
