using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//tp3 Jean-Samuel
[CreateAssetMenu(fileName = "Perso", menuName = "Perso")]

/// <summary>
///  Données relative au personnage #tp3 Antoine
/// </summary>
public class SOPerso : ScriptableObject
{

    [Header("Valeurs initiale")]
    [SerializeField] int _nbDeBrocolisIni = 0;
    [SerializeField] int _nbDeBrocolisDisponibleIni = 0;
    [SerializeField] int _nbDeVieIni = 3;
    public int nbDeVieIni => _nbDeVieIni;
    [SerializeField] int _niveauIni = 2;
    [SerializeField] bool _aClefIni = false;
    [SerializeField] int _nbEnemisTuerIni;
    [SerializeField] int _tempsRestantIni = 100;
    [SerializeField] int _nbDeFlechesIni = 5;

    [Header("Valeurs Actuelles")]
    [SerializeField] int _nbDeBrocolis = 0;
    [SerializeField] int _nbDeBrocolisdisponible = 0;
    [SerializeField] int _nbDeBrocolisRamasser = 0;
    [SerializeField] int _nbDeVie = 3;
    [SerializeField] int _niveau = 1;
    [SerializeField] bool _aClef = false;
    [SerializeField] int _nbEnemisTuer = 0;
    [SerializeField] int _nbDeFleches = 0;
    [SerializeField] int _tempsRestant;

    [Header("Valeurs Maximal")]
    [SerializeField] int _nbDeVieMax = 6; //nb de vie max #Synthèse Antoine
    [SerializeField] int _nbDeFlechesMax = 20; //nb de flèches max #Synthèse jean-Samuel
    public int nbDeFlechesMax => _nbDeVieMax; //getter nb de vie max #Synthèse Antoine


    /// <summary>
    /// Permet de lire le nb de vie du perso de l'extérieur et d'en changer sa valeur tout l'actualisant dans le UI a l'aide du event.Invoke() #tp3 Antoine
    /// </summary>
    public int nbDeVie
    {
        get => _nbDeVie;
        set
        {
            _nbDeVie = Mathf.Clamp(value, 0, _nbDeVieMax);
            _evenementMiseAJour.Invoke();
        }
    }
    /// <summary>
    /// Change la valeur de la clef et l'actualise dans le UI a l'aide du event.Invoke() #synthese Antoine
    /// </summary>
    public bool clef
    {
        get => _aClef;
        set
        {
            _aClef = value;
            if (_aClef)
            {
                Perso.instance.LaPorte.SwitchCouleurParticulePorte();
                SoundManager.instance.ChangerEtatLecturePiste(TypePiste.musiqueBaseBasse, true);
            }
            Debug.Log("<color=green>Ma clef est: " + _aClef + " .</color>");
        }
    }
    /// <summary>
    /// Permet de lire le nb de brocolis du perso de l'extérieur et d'en changer sa valeur tout en l'actualisant dans le UI a l'aide du event.Invoke() #tp3 Antoine
    /// </summary>
    public int nbDeBrocolis
    {
        get => _nbDeBrocolis;
        set
        {
            _nbDeBrocolis = Mathf.Clamp(value, 1, int.MaxValue);
            _nbDeBrocolisRamasser++;
            _evenementMiseAJour.Invoke();
        }
    }
    public int nbDeBrocolisRamasses => _nbDeBrocolisRamasser;
    public int nbDeBrocolisdisponible
    {
        get => _nbDeBrocolisdisponible;
        set
        {
            _nbDeBrocolisdisponible = Mathf.Clamp(value, 1, int.MaxValue);
            _evenementMiseAJour.Invoke();
        }
    }

    public int niveau
    {
        get => _niveau;
        set
        {
            _niveau = Mathf.Clamp(value, 1, int.MaxValue);
            _evenementMiseAJour.Invoke();
        }
    }
    public int nbEnemisTues
    {
        get => _nbEnemisTuer;
        set => _nbEnemisTuer = value;
    }
    public int tempsRestant
    {
        get => _tempsRestant;
        set
        {
            _tempsRestant = value;
            _evenementMiseAJour.Invoke();
        }
    }
    public int nbDeFleches
    {
        get => _nbDeFleches;
        set
        {
            _nbDeFleches = Mathf.Clamp(value, 0, _nbDeFlechesMax);
            _evenementMiseAJour.Invoke();
        }
    }

    private UnityEvent _evenementMiseAJour = new UnityEvent(); // déclaration du event qui sert d'actualiser les infos du personnage #tp3 Antoine
    public UnityEvent evenementMiseAJour => _evenementMiseAJour;


    List<SOObjet> _lesObjets = new List<SOObjet>(); // liste de SO de tous les objets #tp3 Antoine

    /// <summary>
    /// Rénitialise les infos du perso lorsque le joueur quitte la boutique #tp3 Antoine
    /// </summary>
    public void Initialiser()
    {
        _nbDeBrocolisdisponible = _nbDeBrocolisDisponibleIni;
        _nbDeBrocolis = _nbDeBrocolisIni;
        _nbDeBrocolisRamasser = _nbDeBrocolisIni;
        _nbDeVie = _nbDeVieIni;
        _aClef = _aClefIni;
        // _niveau = _niveauIni; // ne pas réinitialiser le niveau pour la complexification des niveaux #tp4 Antoine
        _nbEnemisTuer = _nbEnemisTuerIni;
        tempsRestant = _tempsRestantIni + (niveau * 10);
        _nbDeFleches = _nbDeFlechesIni;
        _lesObjets.Clear();
    }

    /// <summary>
    /// Méthode pour réinitialiser la clef et le temps
    /// lorsqu'il y a changement de scene dans le jeu
    /// </summary>
    public void InitialiserPartiel()
    {
        _aClef = _aClefIni;
        _tempsRestant = _tempsRestantIni + (niveau * 10);
    }

    /// <summary>
    /// Action réalisé lors de l'achat d'un item par le joueur
    /// Soustrait la valeur de l'item transmis par un SO depuis le panneau objet pour enlevé la bonne somme d'argent
    /// Ajoute l'objet dans une liste de SO
    /// </summary>
    /// <param name="donneesObjets"></param>
    public void Acheter(SOObjet donneesObjets)
    {
        nbDeBrocolis -= donneesObjets.prix;
        if (donneesObjets.nom == "Flèche")
        {
            if (_nbDeFleches < _nbDeFlechesMax)
            {

                _nbDeFleches++;
                if (_nbDeFleches >= _nbDeFlechesMax) _nbDeFleches = _nbDeFlechesMax;
            }
            else AtteindreMaxFleches();
        }
        else if (donneesObjets.nom == "Coeur")
        {
            if (_nbDeVie < _nbDeVieMax)
            {
                _nbDeVie++;
            }

        }
        else if (donneesObjets.nom == "Clef")
        {
            if (_aClef == false)
            {
                _aClef = true;
            }
        }

        else
            _lesObjets.Add(donneesObjets);
        AfficherInventaire();
    }

    /// <summary>
    /// affiche tous les objets de la liste SOObjet 
    /// </summary>
    private void AfficherInventaire()
    {
        string inventaire = "";

        foreach (SOObjet objet in _lesObjets)
        {
            if (inventaire != "") inventaire += ", ";
            inventaire += objet.nom;
        }
        Debug.Log("Inventaire du personnage: " + inventaire);

    }
    private void AtteindreMaxFleches()
    {
        Debug.LogWarning("Vous avez atteint le nombre maximum de flèches");
    }

    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    void OnValidate()
    {
        clef = _aClef; // actionne le changement d'état de la clef #synthese Antoine
        _evenementMiseAJour.Invoke();
    }
}