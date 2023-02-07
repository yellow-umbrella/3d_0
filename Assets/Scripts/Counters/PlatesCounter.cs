using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateTaken;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    private float spawnPlateTimerMax = 4f;
    private float spawnPlateTimer;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;


    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer -= spawnPlateTimerMax;
            if (platesSpawnedAmount < platesSpawnedAmountMax)
            {
                platesSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if (platesSpawnedAmount > 0)
            {
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                platesSpawnedAmount--;
                OnPlateTaken?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
