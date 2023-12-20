using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Classe du ScriptableObject ObjetBoutique contenant les informations de l'objet pour affichage dans la boutique
/// Auteur du code et des commentaires: Jean-Samuel David
/// #tp3 Jean-Samuel
/// </summary>
[CreateAssetMenu(fileName = "Objet", menuName = "Objet boutique")] // Création du menu pour créer les diférents objets
public class SOObjet : ScriptableObject
{
    //Données relative à l'objets
    [Header("LES DONNÉES")]
    [SerializeField] string _nom = "Flèche rouge";
    [SerializeField, Tooltip("Image de l'icone à afficher")] Sprite _sprite;
    [SerializeField][Range(0, 1000)] int _prix = 30;

    [SerializeField, TextArea] string _description;

    //Accesseurs et mutateurs pour les différentes données
    public string nom { get => _nom; set => _nom = value; }
    public Sprite sprite { get => _sprite; set => _sprite = value; }
    public int prix => _prix;
    
    public string description { get => _description; set => _description = value; }

}