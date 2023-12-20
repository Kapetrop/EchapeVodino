using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Classe qui gère les projectiles de Vodino
/// Code fait et commenté par Jean-Samuel
/// #synthese Jean-Samuel
/// </summary>
public class ProjectileVodino : BaseProjectils
{
    void Awake()
    {
        //Trouver la position de la souris dans le monde
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Calculer la direction du projectile
        target = mousePosition - (Vector2)transform.position;

    }

    // Réagir aux collisions avec les ennemis ou le personnage
    override protected void OnCollisionEnter2D(Collision2D other)
    {
        BaseEnnemi ennemi = other.gameObject.GetComponent<BaseEnnemi>();
        if (ennemi != null)
        {
            if (ennemi.GetComponent<EnnemiJS>() != null) // tu seulement les ennemis qui ont le script EnnemiJS #synthèse Antoine
            {
                ennemi.Meurt();
            }
            Debug.Log("Ennemi touché");
            DetruireProjectile();
        }
        base.OnCollisionEnter2D(other);
    }
    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        CibleBonus cible = other.gameObject.GetComponent<CibleBonus>();
        if (cible != null)
        {
            cible.DetruireCible();
            Debug.Log("Cible bonus touchée");
            DetruireProjectile();
        }
    }

}
