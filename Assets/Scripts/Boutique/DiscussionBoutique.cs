using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// Classe qui gère les bulles de discussion dans la boutique
/// Créer et commenter par Jean-Samuel
/// #synthese Jean-Samuel
/// </summary>
public class DiscussionBoutique : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _texte; // Référence au composant TextMeshPro de l'objet
    [SerializeField] private float _tempsAffichage = 2f; // Temps d'affichage du texte

    [SerializeField, TextArea] private string _introduction; // Texte à afficher


    void Start()
    {
        _texte.text = ""; // Vider le texte
        StartCoroutine(CoroutineAfficherTexte(_introduction,_tempsAffichage)); // Lancer la coroutine
        Debug.Log("DiscussionBoutique");
    }
    /// <summary>
    /// Coroutine permettant d'afficher le texte lettre par lettre
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineAfficherTexte(string texte, float tempsAffichage = 2f)
    {
;
        float delaiEntreLettres = tempsAffichage / texte.Length; // Calculer le délai entre chaque lettre

        foreach (char lettre in texte) // Pour chaque lettre dans le texte
        {
            _texte.text += lettre;
            yield return new WaitForSeconds(delaiEntreLettres);
        }

        yield return new WaitForSeconds(tempsAffichage * 5); // Attendre la fin du temps d'affichage


        // Désactiver l'objet de la bulle de discussion
        gameObject.SetActive(false);
    }
    /// <summary>
    /// Méthode pour afficher le texte d'achat
    /// </summary>
    /// <param name="nomObj">contient le nom de l'objet acheter</param>
    public void AfficherAchat(string nomObj)
    {
        StopAllCoroutines();
        string sexeObjet = "";
        switch (nomObj)
        {
            case "Coeur":
                sexeObjet = "un";
                break;
            case "Clef":
                sexeObjet = "une";
                break;
            case "Flèche":
                sexeObjet = "une";
                break;
        }
        StartCoroutine(CoroutineAfficherTexte("Vous avez acheté " + sexeObjet + " " + nomObj + ".", _tempsAffichage));
    }
}
