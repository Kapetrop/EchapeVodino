using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> #tp4 Antoine
/// Gestion de la porte (effets sonores et particules)
/// </summary>
public class Porte : MonoBehaviour
{
    [SerializeField] ParticleSystem _particulePorteRouge;


    [SerializeField] Animator _particulePorteRougeAnim;
    [SerializeField] ParticleSystem _particulePorteOuverte;


    [SerializeField] private AudioClip _sonPorteOuvre;
    [SerializeField] private AudioClip _sonPorteBarre;
    [SerializeField] private Color _couleurFermer;
    [SerializeField] private Color _couleurOuvert;

    private bool _sonJoue;
    private bool _estActif = false;

    private void Start()
    {

        Perso.instance.LaPorte = this;
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
        _sonJoue = false;
    }
    /// <summary>
    /// Essai d'ouverture de la porte barré
    /// </summary>
    [ContextMenu("ActiverEssai")]
    public void EssaiPorte()
    {
        if (Perso.instance.donnees.clef) // si le perso a la clef, on ouvre la porte #synthese Antoine
        {
            SonOuvrirPorte();
            SoundManager.instance.ChangerEtatLecturePiste(TypePiste.musiqueBaseBasse, false);
            SoundManager.instance.ChangerEtatLecturePiste(TypePiste.musiqueEvenSalle, false);
            SoundManager.instance.ChangerEtatLecturePiste(TypePiste.musiqueEvenBoutique, true);
            Perso.instance.donneesNav.AllerSceneSuivante();

        }
        else if (!_sonJoue) // si il ne possède pas la clef et que le son ne jou pas déjà #synthese Antoine
        {
            _particulePorteRougeAnim.SetTrigger("essai");
            SonBarre();
        }
    }

    /// <summary># synthese Antoine
    /// Active le bon particule systeme selon si le joueur possède la clef ou non
    /// </summary>
    [ContextMenu("ActiverParticulePorte")] // permet d'actionner la fonction avec un clic droit sur le script #tp4 Antoine #débug #merci Jonathan
    public void SwitchCouleurParticulePorte()
    {
        _estActif = !_estActif;
        _particulePorteRouge.gameObject.SetActive(!_estActif);
        if (!_estActif) _particulePorteRouge.Play();
        else _particulePorteRouge.Stop();

        _particulePorteOuverte.gameObject.SetActive(_estActif);
        if (_estActif) _particulePorteOuverte.Play();
        else _particulePorteOuverte.Stop();
    }
    /// <summary> #synthese Antoine
    /// detecte la collision avec le joueur pour lui dire qu'il est en collision avec la porte
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        Perso perso = other.GetComponent<Perso>();
        if (perso != null)
        {
            perso.touchePorte = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Perso perso = other.GetComponent<Perso>();
        if (perso != null)
        {
            perso.touchePorte = false;
        }
    }
    /// <summary> #tp4 Antoine
    /// Effet sonore à l'ouverture de la porte
    /// </summary>
    public void SonOuvrirPorte()
    {
        SoundManager.instance.JouerEffetSonore(_sonPorteOuvre, 0.15f, true);
    }
    /// <summary> #tp4 Antoine
    /// Effet sonore de la porte barré
    /// </summary>
    public void SonBarre()
    {
        SoundManager.instance.JouerEffetSonore(_sonPorteBarre, 0.8f, false, true);
        _sonJoue = true;
        Invoke("SonBarreFini", _sonPorteBarre.length);
    }
    /// <summary> #tp4 Antoine
    /// change status du son lorsqu'il est finit
    /// </summary>
    private void SonBarreFini()
    {
        _sonJoue = false;
    }
    /// <summary> #tp4 Antoine
    /// Sert a savoir si le son jou déjà lorsque le joueur essait d'ouvrir la porte sans la clef
    /// </summary>
    /// <returns></returns>
    public bool SonJoue()
    {
        return _sonJoue;
    }

}
