using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Classe qui gère les projectiles des ennemis
/// Code fait et commenté par Jean-Samuel
/// #synthese Jean-Samuel
/// </summary>
public class ProjectileEnnemi : BaseProjectils
{
       // Réagir aux collisions avec les ennemis ou le personnage
    override protected void OnCollisionEnter2D(Collision2D other)
    {
        Perso perso = other.gameObject.GetComponent<Perso>();
        if (perso != null)
        {
            Debug.Log("Perso touché");
            perso.PerdreVie();
            DetruireProjectile();
        }
        base.OnCollisionEnter2D(other);
    }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Vector2 posVodino = Perso.instance.transform.position; // Trouvre la position du perso
        target = posVodino - (Vector2)transform.position; // envoie la position du perso à la variable target de la classe de base
    }


}
