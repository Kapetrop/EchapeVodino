using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Classe qui gère l'ouverture du niveau bonus
/// Si le perso touche 3 cibles, le niveau bonus est accecible
/// Code fait et commenté par Jean-Samuel
/// #synthese Jean-Samuel
/// </summary>
public class OuvertureBonus : MonoBehaviour
{
    [SerializeField] ParticleSystem _particulePorteRouge;


    [SerializeField] Animator _particulePorteRougeAnim;
    [SerializeField] ParticleSystem _particulePorteOuverte;


    [SerializeField] private AudioClip _sonPorteOuvre;
    [SerializeField] private Color _couleurFermer;
    [SerializeField] private Color _couleurOuvert;
    private bool _estActif = false;

    [SerializeField] private SONavigation _donneesNav; // lien vers SO navigation #tp4 Antoine
      private void Start()
    {


        _estActif = false;
        _particulePorteRouge.gameObject.SetActive(!_estActif);
        _particulePorteOuverte.gameObject.SetActive(_estActif);
        var mainModuleRouge = _particulePorteRouge.main;
        mainModuleRouge.startColor = _couleurFermer;

        var mainModuleOuvert = _particulePorteOuverte.main;
        mainModuleOuvert.startColor = _couleurOuvert;
        if (!_estActif) _particulePorteRouge.Play();
        else _particulePorteRouge.Stop();
        if (_estActif) _particulePorteOuverte.Play();
        else _particulePorteOuverte.Stop();

    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        switch (Perso.instance.lesCiblesTouchees)
        {
            case 3:
            _estActif = true;
            SoundManager.instance.JouerEffetSonore(_sonPorteOuvre, 0.1f);
            _particulePorteRouge.Stop();
            _particulePorteOuverte.Play();
            _particulePorteOuverte.gameObject.SetActive(_estActif);
            _particulePorteRouge.gameObject.SetActive(!_estActif);
            break;
            default:
            _estActif = false;
            _particulePorteRouge.Play();
            _particulePorteOuverte.Stop();
            _particulePorteOuverte.gameObject.SetActive(_estActif);
            _particulePorteRouge.gameObject.SetActive(!_estActif);
            break;
        }
    }
    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        Perso perso = other.GetComponent<Perso>(); // va chercher le composant perso 
        if (perso != null && Perso.instance.lesCiblesTouchees >= 3)
        {
            Debug.Log("Ouverture du bonus");
           _donneesNav.AllerAuNiveauBonus();
        }
    }
}
