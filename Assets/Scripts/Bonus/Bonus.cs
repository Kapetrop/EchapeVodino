using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



/// <summary>  # tp3 Antoine
/// Ecoute l'evenement "activeBonus" pour se faire activer #tp3 Antoine
/// Liste les différent bonus
/// </summary>
public class Bonus : MonoBehaviour
{
    [SerializeField] SOBonus _bonus; // Scriptable object du bonus #tp3 Antoine
    private SpriteRenderer _sr;
    private bool _estActif = false;
    public enum Effets { Vitesse = 0, Saut = 1, Autre = 2 } // enum pour faire une menu déroulant #tp3 Antoine
    public Effets effets; // variable représentant L'énum des bonus #tp3 Antoine
    private AudioClip _son; // son jouer lors du ramassage de du bonus #tp4 Antoine

    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _estActif = false;

        Niveau.instance.activeBonus.AddListener(ActiveToi); // ecoute l'evenement "activeBonus" #tp3 Antoine
        _sr.color = new Color(0.5f, _sr.color.g, _sr.color.b, 0.50f); // Grise le bonus
    }

    /// <summary> #tp3 Antoine
    /// avtive le bonus
    /// </summary>
    void ActiveToi()
    {
        _estActif = true;
        _sr.color = new Color(1, _sr.color.g, _sr.color.b, 1f); ; // DeGrise le bonus
    }

    /// <summary> #tp3 Antoine
    /// Lorsque le bonus est triggeré il regarde si il est actif, si oui appelle le bonus qui lui correspond au perso
    /// </summary>
    /// <param name="other">objet triggeré</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_estActif)
        {
            if (effets == Effets.Vitesse) Perso.instance.MarchePlusVite(_bonus.modificateurBonus, _bonus.modificateurCourleurParticule, _bonus.modificateurGrosseurParticule);
            if (effets == Effets.Saut) Perso.instance.SautePlusHaut(_bonus.modificateurBonus, _bonus.modificateurCourleurParticule, _bonus.modificateurGrosseurParticule);
            Destroy(gameObject);
        }
    }
}
