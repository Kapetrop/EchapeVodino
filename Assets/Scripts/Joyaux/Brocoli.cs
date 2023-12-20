using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Classe permettant la comptabilisation des information lier au brocoli
/// Auteur du code et des commentaires: Jean-Samuel #synthese Jean-Samuel
/// </summary>
public class Brocoli : MonoBehaviour
{
    private static List<Brocoli> _lesBrocolis = new List<Brocoli>();
    public static int nbBrocoli = _lesBrocolis.Count;
    [SerializeField] private AudioClip _sonBrocoli; // le son du brocoli #tp4 Antoine
    [SerializeField] private int _valeurBrocoli = 1; // la valeur du brocoli #synthese Jean-Samuel
    public int valeurBrocoli => _valeurBrocoli;



    void Awake()
    {
        _lesBrocolis.Add(this);
    }
    /// <summary>
    /// Méthode permettant de détruire le brocoli et de le retirer de la liste
    /// </summary>
    public void EnleverBrocoli()
    {
        _lesBrocolis.Remove(this); 
        SoundManager.instance.JouerEffetSonore(_sonBrocoli);
        Destroy(gameObject);
    }
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

            perso.AjouterBroco(valeurBrocoli);
   
            EnleverBrocoli();
        }
    }
}
