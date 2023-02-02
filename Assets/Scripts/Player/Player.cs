using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    public void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
        float interactDistance = 2f;

        bool canInteract = Physics.Raycast(transform.position, lastInteractDir,
            out RaycastHit raycastHit, interactDistance, countersLayerMask);

        if (canInteract)
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                //object has component ClearCounter
                if (selectedCounter != baseCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            } else
            {
                SetSelectedCounter(null);
            }
        } else
        {
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float moveDistance = moveSpeed * Time.deltaTime;


        bool canMove = CanMove(moveDir, moveDistance);

        if (!canMove)
        {
            //Cannot move towards moveDir

            //Attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f);
            canMove = moveDir.x != 0 && CanMove(moveDirX, moveDistance);

            if (canMove)
            {
                //Can move only on X
                moveDir = moveDirX;
            }
            else
            {
                //Cannot move on X
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z);
                canMove = moveDir.z != 0 && CanMove(moveDirZ, moveDistance);

                if (canMove)
                {
                    //Can move only on Z
                    moveDir = moveDirZ;
                }
            }
        }

        isWalking = moveDir != Vector3.zero;
        if (isWalking)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDir,
                Time.deltaTime * rotateSpeed);
        }
        if (canMove)
        {
            transform.position += moveDir.normalized * moveDistance;
        }
    }

    private bool CanMove(Vector3 moveDirection, float moveDistance)
    {
        float playerRadius = .7f;
        float playerHeight = 2f;

        return !Physics.CapsuleCast(transform.position,
            transform.position + Vector3.up * playerHeight,
            playerRadius, moveDirection, moveDistance);
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return (kitchenObject != null);
    }
}