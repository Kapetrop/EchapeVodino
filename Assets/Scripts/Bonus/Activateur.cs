using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Va appeller l'evenement pour activer les bonus 
/// #tp3 Antonie
/// </summary>
public class Activateur : MonoBehaviour
{
    private SpriteRenderer _sr;

    private void Start() 
    {
        _sr = GetComponent<SpriteRenderer>();    
    }
    /// <summary>
    /// detecte les trigger avec d'autre collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        Niveau.instance.activeBonus.Invoke();
        _sr.color = Color.blue;
    }
}
