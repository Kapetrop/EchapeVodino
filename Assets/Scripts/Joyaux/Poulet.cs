using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> #synthese Antoine
/// Classe qui gère les malus fait par les cuisse de poulet
/// </summary>
public class Poulet : MonoBehaviour
{
    /// <summary> #synthese Antoine
    /// Soustrait un brocolie du personnage lorsqu'il mange une cuisse de poulet
    /// </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        Perso perso = other.GetComponent<Perso>();
        if (perso != null)
        {
            perso.AjouterBroco(-1); // soustrait un brocoli
            Destroy(gameObject);
        }
    }
}
