using System;
using UnityEngine;

public abstract class AbstractRoot : MonoBehaviour
{
    public static event Action Dead;
    public static event Action LevelEnd;

    protected abstract void OnTriggerEnter2D(Collider2D col);

    protected virtual void OnDead() => Dead?.Invoke();
    protected virtual void OnEndLevel() => LevelEnd?.Invoke();
}
