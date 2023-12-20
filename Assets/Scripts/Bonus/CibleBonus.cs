using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Classe de base pour faire réagire les cibles de la salle bonus
/// Code et commentaire fait par Jean-Samuel
/// #synthese Jean-Samuel
/// </summary>
public class CibleBonus : MonoBehaviour
{
    /// <summary>
    /// Méthode qui détruit la cible et ajoute 1 au nombre de cible touchée au perso
    /// </summary>
    public void DetruireCible()
    {
        Perso.instance.lesCiblesTouchees++; // Ajouter 1 au nombre de cible touchée
        Destroy(gameObject);
    }
}
