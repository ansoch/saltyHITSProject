using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractible
{
    Collider2D Collider { get; }
    Rigidbody2D Rigidbody { get; }
    IPlayerState Interact();
}
