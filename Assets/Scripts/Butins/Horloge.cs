using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> #synthèse Antoine
/// Horloge qui permet de ralentir ou accélérer le temps
/// </summary>
public class Horloge : MonoBehaviour
{
    private bool _estBonusOveride; // oblige l'horloge à être un bonus
    public bool estBonusOveride
    {
        set { _estBonusOveride = value; }
    }
    private bool _estMalusOveride; // oblige l'horloge à être un malus
    public bool estMalusOveride
    {
        set { _estMalusOveride = value; }
    }
    private bool _estBonus = true; // choisit aléatoirement si l'horloge est un bonus ou un malus #synthèse Antoine

    [Header("Propriétés")]
    [SerializeField, Range(2, 10)] private int _duree;
    [SerializeField, Range(1, 0), Tooltip("1= aucun effet ; 0 = effet maximal")] private float _intensiteRalentissement;
    [SerializeField, Range(1, 2), Tooltip("1= aucun effet ; 2 = effet maximal")] private float _intensiteAccelerateur;
    private float _intensite; // intensité du modificateur de temps #synthese Antoine
    private SpriteRenderer _sr;
    private Color _couleurRouge = new Color(1, 0, 0, 1); // rouge 
    private Color _couleurVert = new Color(0, 1, 0, 1); // vert


    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        // choisit aléatoirement si l'horloge est un bonus ou un malus, utile pour butin #synthèse Antoine
        if (Random.Range(0, 2) == 0)
        {
            _estBonus = false;
            _intensite = _intensiteAccelerateur;
            _sr.color = _couleurRouge;
        }
        else
        {
            _estBonus = true;
            _intensite = _intensiteRalentissement;
            _sr.color = _couleurVert;
        }

        if (_estBonusOveride) // si elle doit absolument être un bonus #synthèse Antoine
        {
            _estBonus = true;
            _intensite = _intensiteRalentissement;
            _sr.color = _couleurVert;
        }
        if (_estMalusOveride) // si elle doit absolument être un malus #synthèse Antoine
        {
            _estBonus = false;
            _intensite = _intensiteAccelerateur;
            _sr.color = _couleurRouge;
        }
    }
    /// <summary> #synthèse Antoine
    /// detecte si le joueur entre en contact avec l'horloge et si oui, ralentit le temps
    /// va affecter tous les coroutines d'ennemis et le timer du niveau
    /// </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        Perso perso = other.GetComponent<Perso>();
        if (perso != null)
        {
            Niveau.instance.modificateurTemps = _intensite;
            Niveau.instance.dureeHorloge = _duree;
            Niveau.instance.AltererTemps();
            Destroy(gameObject);
        }
    }
}
