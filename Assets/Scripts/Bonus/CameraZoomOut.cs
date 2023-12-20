using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomOut : MonoBehaviour
{
    float _sizeOrtho = 10f;
    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        Perso perso = other.GetComponent<Perso>();
        if (perso != null)
        {
            NiveauBonus.instance.ChangerSizeOrtho(_sizeOrtho);
        }
    }
}
