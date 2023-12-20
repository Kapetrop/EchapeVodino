using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//tp3 Jean-Samuel
/// <summary>
/// Classe contenant les information pour les bonus
/// </summary>
[CreateAssetMenu(fileName = "Bonus", menuName = "Bonus")]
public class SOBonus : ScriptableObject
{
    // Information possible pour le bonus
    [Header("Information")]
    [SerializeField] string _nom = "Vitesse";
    [Header("Valeurs Initiale")]
    [SerializeField] private bool _estActifIni = false;

    [SerializeField] private float _tempsBeneficeActifMaxIni = 10f;
    [SerializeField] private float _tempsBeneficeActifRestantIni = 0;
    [Header("Valeurs actuels")]
    [SerializeField] private bool _estActif = false;
    [SerializeField] private float _tempsBeneficeActifMax;
    [SerializeField] private float _tempsBeneficeActifRestant;
    [SerializeField] private float _modificateurBonus;
    [SerializeField] private Color _modificateurCouleurParticule;
    [SerializeField, Range(0,5f)] private float _modificateurGrosseurParticule;

    public string nom => _nom;
    public bool estActif
    {
        get => _estActif;
        set
        {
            _estActif = !_estActif;
            _evenementMiseAJour.Invoke();
        }
    }

   
    public float tempsBeneficeActifRestant
    {
        get => _tempsBeneficeActifRestant;
        set
        {
            _tempsBeneficeActifRestant = Mathf.Clamp(value, 1, float.MaxValue); // Bloque la valeur possible de 0 à la valeur maximal d'un float
            _evenementMiseAJour.Invoke();
        }
    }
    public float tempsBeneficeActifMax
    {
        get => _tempsBeneficeActifMax;
        set
        {
            _tempsBeneficeActifMax = Mathf.Clamp(value, 1, float.MaxValue); // Bloque la valeur possible de 0 à la valeur maximal d'un float
            _evenementMiseAJour.Invoke();
        }

    }
    public Color modificateurCourleurParticule => _modificateurCouleurParticule;
    public float modificateurGrosseurParticule => _modificateurGrosseurParticule;
    public float modificateurBonus => _modificateurBonus;


    private UnityEvent _evenementMiseAJour = new UnityEvent();
    public UnityEvent evenementMiseAJour => _evenementMiseAJour;

  
    /// <summary>
    /// Méthode permettant de réinitialiser les informations du bonus
    /// </summary>
    public void Initialiser()
    {
        _estActif = _estActifIni;
        _tempsBeneficeActifRestant = _tempsBeneficeActifRestantIni;
        _tempsBeneficeActifMax = _tempsBeneficeActifMaxIni;
    }



    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    void OnValidate()
    {
        _evenementMiseAJour.Invoke();
    }
}
