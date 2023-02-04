using System;
using UnityEngine;

public abstract class AbstractRoot : MonoBehaviour
{
    public static event Action Dead;

    protected abstract void OnTriggerEnter2D(Collider2D col);

    protected virtual void OnDead() => Dead?.Invoke();
}
