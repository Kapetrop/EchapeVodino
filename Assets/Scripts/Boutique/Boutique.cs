using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
/// <summary>
/// Classe qui gère ce qui se passe dans la boutique
/// Créer et commenter par Jean-Samuel
/// </summary>
public class Boutique : MonoBehaviour
{
    [SerializeField] SOPerso _donneesPerso; // Acces au données du personnage
    public SOPerso donneesPerso => _donneesPerso; // Accesseur des données du perso publique
    

    [SerializeField] TextMeshProUGUI _champBrocoli; // Afficheur de texte dans le UI

    /// <summary>
    /// Création du singleton de la boutique
    /// </summary>
    static Boutique _instance;
    static public Boutique instance => _instance;
    
    void Awake()
    {
        // Empêcher plusieurs singleton de rouler en même temps
        if (_instance != null) { Destroy(gameObject); return; }
        _instance = this;
        MettreAJourInfos(); 
        _donneesPerso.evenementMiseAJour.AddListener(MettreAJourInfos); // Mise à jour continuel si modification dans _donneesPerso
    }
    /// <summary>
    /// Méthode permettant d'afficher le nombre de brocolis que le joueur a
    /// </summary>
    private void MettreAJourInfos()
    {
        _champBrocoli.text = ""+_donneesPerso.nbDeBrocolis; // Envoyer l'information vers un champs de texte
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        _donneesPerso.evenementMiseAJour.RemoveAllListeners();
        Debug.Log("Destroy!");
    }
}