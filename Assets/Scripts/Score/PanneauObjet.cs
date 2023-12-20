using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
// tp3 Jean-Samuel
/// <summary>
/// Classe qui gère les action fait ppour les objets dans la boutique
/// Créer et commenter par Jean-Samuel
/// grandement inspirer du devoir
/// </summary>
public class PanneauObjet : MonoBehaviour
{
    /// <summary>
    /// Scriptable objet associer au gameObject
    /// </summary>
    [Header("LES DONNÉES")]
    [SerializeField] SOObjet[] _tDonnees;
    SOObjet _donnees;
    public SOObjet donnees => _donnees;
    [SerializeField] DiscussionBoutique _texteAchat;

    /// <summary>
    /// Association des différents élément de la hiérarchie pour affichage des données
    /// </summary>
    [Header("LES CONTENEURS")]
    [SerializeField] TextMeshProUGUI _champNom;
    [SerializeField] TextMeshProUGUI _champPrix;
    [SerializeField] TextMeshProUGUI _champDescription;
    [SerializeField] Image _image;
    [SerializeField] CanvasGroup _canvasGroup;

    void Start()
    {
        _donnees = _tDonnees[Random.Range(0, _tDonnees.Length)]; // Choix aléatoire d'un objet
        MettreAJourInfos(); // Appel de la méthode pour mettre les information au bon endroits
        Boutique.instance.donneesPerso.evenementMiseAJour.AddListener(MettreAJourInfos); // Mise à jour continuel des éléments dans la boutique
    }
    /// <summary>
    /// Méthode servant à mettre les information au endroit désirer
    /// </summary>
    private void MettreAJourInfos()
    {
        _champNom.text = _donnees.nom;
        _champPrix.text = _donnees.prix + "";
        _champDescription.text = _donnees.description;
        _image.sprite = _donnees.sprite;
        GererDispo(); // Appel de la méthode
    }
    /// <summary>
    /// Méthode pour mettre en visuel si le joueur peut acheter ou non des objet dans la boutique
    /// </summary>
    void GererDispo()
    {
        bool aAssezBrocoli= Boutique.instance.donneesPerso.nbDeBrocolis >= _donnees.prix; // booléin par apport au prix de l'objet avec l'argent du joueur

        // Condition si le joueur peut
        if(aAssezBrocoli)
        {
            _canvasGroup.interactable = true; // Peu intéragir
            _canvasGroup.alpha = 1; // Affichage non changer
        }
        else
         {
            _canvasGroup.interactable = false; // Ne peut interagir
            _canvasGroup.alpha = 0.5f; // Grisse l'objet
        }
    }
    /// <summary>
    /// Méthode appeler quand le joueur achete un objet
    /// </summary>
    public void Acheter()
    {
        Boutique.instance.donneesPerso.Acheter(_donnees);
        _texteAchat.AfficherAchat(_donnees.nom);
    }
}