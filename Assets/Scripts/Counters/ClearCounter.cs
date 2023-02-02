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
            } else
            {   // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        throw new System.NotImplementedException();
    }
}
