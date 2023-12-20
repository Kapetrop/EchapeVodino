using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// CLasse pour permettre le gain de vie
/// Créer et commenter par Jean-Samuel
/// #synthese Jean-Samuel
/// </summary>
public class VieSupp : MonoBehaviour
{
//     /// <summary>
//     /// Sent when another object enters a trigger collider attached to this
//     /// object (2D physics only).
//     /// </summary>
//     /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        Perso perso = other.gameObject.GetComponent<Perso>();
        if(perso != null)
        {
            perso.AugmenterNbVie();
            Destroy(gameObject);
        }
    }
}
