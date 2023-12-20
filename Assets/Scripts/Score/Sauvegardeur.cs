using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// Classe permettant de sauvegarder les scores
/// Auteur du code et des commentaires: Jean-Samuel David
/// </summary>
public class Sauvegardeur : MonoBehaviour
{
    //tp4 Jean-Samuel
    [SerializeField] SOScore _donnees;  // lien vers le SO des scores
    [Header("Joueur 1")]   // Données position 1
    [SerializeField] TextMeshProUGUI _texteNom1;
    [SerializeField] TextMeshProUGUI _texteScore1;

    [Header("Joueur 2")]    // Données position 2
    [SerializeField] TextMeshProUGUI _texteNom2;
    [SerializeField] TextMeshProUGUI _texteScore2;

    [Header("Joueur 3")]    // Données position 3
    [SerializeField] TextMeshProUGUI _texteNom3;
    [SerializeField] TextMeshProUGUI _texteScore3;
    [SerializeField] GameObject _entrerNom;     // Accès au prefabs du panneau d'entrée de nom

    //Acces à la doner champsTotal de la classe PanneauScore
    [SerializeField] private PanneauScore _panneauScore;
    [SerializeField] GameObject _panneauPosition1;
    [SerializeField] GameObject _panneauPosition2;
    [SerializeField] GameObject _panneauPosition3;
    [SerializeField] private RectTransform _RectTransformDuChampsDeText; // Position du joueur dans le top 3
    private int _scoreMin;      // Score minimum pour entrer dans le top 3
    public int scoreMin => _scoreMin;   // Accesseur pour le score minimum



    int _position; // Position du joueur dans le top 3
    //Accès au champs inputField du panneau d'entrée de nom
    [Header("InputField")]
    [SerializeField] private TMP_InputField _inputField;



    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

        _donnees.LireFichier(); // Lit le fichier de sauvegarde
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(!_panneauScore.scoreCharge)StartCoroutine(CoroutineAfficherScore()); // Affiche les scores
       
    }
    public void EnregistrerNouveauScore()
    {
        int score = _panneauScore.total; // Récupère le score du joueur


        // Détermine la position du joueur dans le top 3
        if (score >= _donnees.score1)
        {
            _position = 1;
        }
        else if (score >= _donnees.score2)
        {
            _position = 2;
        }
        else if (score >= _donnees.score3)
        {
            _position = 3;
        }
        else
        {
            _position = 0;
        }
        // Enregistre le score du joueur dans le top 3
        switch (_position)
        {
            case 3:
                _donnees.nom3 = _inputField.text;  // Récupère le nom du joueur du champs de l'inputField
                _donnees.score3 = score; // Récupère le score du joueur du SOPerso
                break;
            case 2:
                // Décale les anciennes valeurs du top 2 vers le top 3
                _donnees.nom3 = _donnees.nom2;
                _donnees.score3 = _donnees.score2;
                _donnees.nom2 = _inputField.text; // Enregistre le nom du joueur dans le top 2
                _donnees.score2 = score; // Enregistre le score du joueur dans le top 2
                break;
            case 1:
                // Décale les anciennes valeurs du top 1 vers le top 2
                _donnees.nom2 = _donnees.nom1;
                _donnees.score2 = _donnees.score1;
                _donnees.nom1 = _inputField.text; // Enregistre le nom du joueur dans le top 1
                _donnees.score1 = score; // Enregistre le score du joueur dans le top 1
                break;
        }
        _donnees.MettreAJourMeilleursScores();
        Enregistercore(); // Enregistre les scores dans le fichier
    }
    /// <summary>
    /// Méthode permettant d'enregistrer les scores dans le fichier
    /// </summary>
    public void Enregistercore()
    {
        _donnees.EcrireFichier();
    }
    /// <summary>
    /// Méthode permettant d'activer le panneau d'entrée de nom
    /// </summary>
    public void AfficherEntrerNom()
    {
        _entrerNom.SetActive(true);
        _scoreMin = _donnees.score3;

        switch (_position)
        {
            case 1:
                _RectTransformDuChampsDeText.anchoredPosition = _panneauPosition1.GetComponent<RectTransform>().anchoredPosition;
                break;
            case 2:
                _RectTransformDuChampsDeText.anchoredPosition = _panneauPosition2.GetComponent<RectTransform>().anchoredPosition;
                break;
            case 3:
                _RectTransformDuChampsDeText.anchoredPosition = _panneauPosition3.GetComponent<RectTransform>().anchoredPosition;
                break;
        }

    }
    /// <summary>
    ///  Coroutine permettant d'afficher les scores
    /// mais avec un délai de 0.5 secondes entre chaque affichage
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineAfficherScore()
    {

            _texteNom1.text = _donnees.nom1;
            _texteScore1.text = _donnees.score1.ToString(); // Affiche le score mais transformant le int en string
            yield return new WaitForSeconds(0.5f);
            _texteNom2.text = _donnees.nom2;
            _texteScore2.text = _donnees.score2.ToString();
            yield return new WaitForSeconds(0.5f);
            _texteNom3.text = _donnees.nom3;
            _texteScore3.text = _donnees.score3.ToString();
        
    }

}
