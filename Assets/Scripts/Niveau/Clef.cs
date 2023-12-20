using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// S'occupe de s'autodétruire #tp3 Antoine
/// </summary>
public class Clef : MonoBehaviour
{
    [SerializeField] private AudioClip _sonClef; // le son de la clef #tp4 Antoine
    /// <summary>
    /// Detruit la clef
    /// </summary>
    public void Detruire() 
    {
        SoundManager.instance.JouerEffetSonore(_sonClef);
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
            perso.donnees.clef = true;
            Detruire();
        }
    }
}
