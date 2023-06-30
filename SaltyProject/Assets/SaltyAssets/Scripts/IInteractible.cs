using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractible
{
    Collider2D Collider { get; }
    Rigidbody2D Rigidbody { get; }
    bool IsInRange {  get; }
    IPlayerState Interact();
}

