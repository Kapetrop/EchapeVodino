using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Classe controllant les particules de poussière, chute et atterrissage
/// Auteur code et Commentaires: Jean-Samuel David
/// #tp4 Jean-Samuel
/// </summary>
public class GestParticules : MonoBehaviour
{
    [Header("Particule")]
    // Particules de poussière lorsque le joueur se déplace
    
    [SerializeField] ParticleSystem _partPoussiere;
    ParticleSystem.MainModule _mainPoussiere;
    ParticleSystem.ColorOverLifetimeModule _colorPoussiere;
    float _GrosseurDeBase;
    Color _couleurDeBase;
    // Particules de chute lorsque le joueur tombe
    [SerializeField] ParticleSystem _partChute;
    // Particules d'atterrissage lorsque le joueur atterit
    
    [SerializeField] ParticleSystem _partAterrissage;


    private void Start()
    {
        _mainPoussiere = _partPoussiere.main;
        _colorPoussiere = _partPoussiere.colorOverLifetime;
        _GrosseurDeBase = _mainPoussiere.startSize.constant;
        _couleurDeBase = _colorPoussiere.color.gradient.colorKeys[0].color;
        ArreterPartPoussiere(); // Arreter les particules de poussière pour le d/placement du joueur
        ArreterPartAterrissage(); // Arreter les particules d'atterrissage
        ArreterPartChute(); // Arreter les particules de chute
    }
    /// <summary>
    /// Méthode permettant d'arrêter les particules de poussière
    /// </summary>
    public void ArreterPartPoussiere()
    {

        _partPoussiere.gameObject.SetActive(false);
        _partPoussiere.Stop();
    }
    /// <summary>
    /// Méthode permettant de débuter les particules de poussière
    /// </summary>
    void DebuterPartPoussiere()
    {
        _partPoussiere.gameObject.SetActive(true);
        _partPoussiere.Play();
    }
    /// <summary>
    /// Méthode permettant d'arrêter les particules de chute
    /// </summary>
    public void ArreterPartChute()
    {
        _partChute.gameObject.SetActive(false);
        _partChute.Stop();

    }
    /// <summary>
    /// Méthode permettant de débuter les particules de chute
    /// </summary>
    void DebuterPartChute()
    {
        _partChute.gameObject.SetActive(true);
        _partChute.Play();
    }
    /// <summary>
    /// Méthode permettant d'arrêter les particules d'atterrissage
    /// </summary>
    public void ArreterPartAterrissage()
    {
        _partAterrissage.gameObject.SetActive(false);
        _partAterrissage.Stop();
    }
    /// <summary>
    /// Méthode permettant de débuter les particules d'atterrissage
    /// </summary>
    void DebuterPartAterrissage()
    {
        _partAterrissage.gameObject.SetActive(true);
        _partAterrissage.Play();
    }

    public void AfficherParticulesMarche()
    {
        DebuterPartPoussiere();
        ArreterPartAterrissage();
        ArreterPartChute();
    }

    public void AfficherParticulesChute()
    {
        DebuterPartChute();
        ArreterPartAterrissage();
        ArreterPartPoussiere();
    }

    public void EnclancherParticuleAtterrissage()
    {
        ArreterPartChute();
        Coroutine corout = StartCoroutine(CoroutineParticuleAterrissage());
    }
    /// <summary>
    /// Méthode pour modifier la grosseur et la couleur des particules de poussière
    /// Lorsque le joueur prend un bonus
    /// </summary>
    /// <param name="grosseurVoulue">La grosseur désirer</param>
    /// <param name="couleurVoulue">La couleur ésirer</param>
    public void ModiffierParticulesEnBonus(float grosseurVoulue, Color couleurVoulue)
    {
        _mainPoussiere.startSize = grosseurVoulue;
        Gradient grad = new Gradient();
        grad.SetKeys(
            new GradientColorKey[] { new GradientColorKey(couleurVoulue, 0.0f), new GradientColorKey(couleurVoulue, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
        );
        _colorPoussiere.color = grad;
    }
    /// <summary>
    /// Méthode pour les valeur par default des particules de poussière
    /// </summary>
    public void RemettreLesParticulesNormal()
    {
        _mainPoussiere.startSize = _GrosseurDeBase;
        _colorPoussiere.color = _couleurDeBase;
    }
    IEnumerator CoroutineParticuleAterrissage()
    {
        DebuterPartAterrissage();
        yield return new WaitForSeconds(0.1f);
        ArreterPartAterrissage();
    }

}
