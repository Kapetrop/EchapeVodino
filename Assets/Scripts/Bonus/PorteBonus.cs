using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Classe permettant de sortir des niveaux bonus par une des protes disponibles
/// Auteur du code et des commentaires: Jean-Samuel #synthese Jean-Samuel
/// #synthese Jean-Samuel
/// </summary>
public class PorteBonus : MonoBehaviour
{
    [SerializeField] private SONavigation _donneesNav; // lien vers SO navigation #tp4 Antoine
  /// <summary>
  /// Sent when another object enters a trigger collider attached to this
  /// object (2D physics only).
  /// </summary>
  /// <param name="other">The other Collider2D involved in this collision.</param>
  void OnTriggerEnter2D(Collider2D other)
  {
      Perso perso = other.GetComponent<Perso>(); // va chercher le composant perso 
      if(perso !=null) _donneesNav.AllerALaBoutique(); // si le perso touche la porte, il va à la boutique
  }
}
