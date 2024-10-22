using UnityEngine;

public struct OnMovementInputChanged 
{
    public readonly Vector2 movementInput;

    public OnMovementInputChanged(Vector2 movementInput)
    {
        this.movementInput = movementInput;
    }
}
