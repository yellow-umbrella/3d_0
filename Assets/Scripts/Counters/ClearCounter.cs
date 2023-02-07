using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {   // No KitchenObject here
            if (player.HasKitchenObject())
            {   // Player is carrying sth
                player.GetKitchenObject().SetKitchenObjectParent(this);
            } else
            {   // Player not carrying anything
            }
        } else
        {   // KitchenObject is here
            if (player.HasKitchenObject())
            {   // Player is carrying sth
                if (player.GetKitchenObject().TryGetPlate(out Plate plate))
                {
                    if (plate.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                } else if (GetKitchenObject().TryGetPlate(out plate))
                {
                    if (plate.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                    {
                        player.GetKitchenObject().DestroySelf();
                    }
                }
            } else
            {   // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
