using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;
using TMPro;
/// <summary>
/// Classe permettant de sauvegarder et lire les scores
/// Auteur du code et des commentaires: Jean-Samuel David
/// #tp4 Jean-Samuel
/// </summary>

[CreateAssetMenu(fileName = "Score", menuName = "Score")] // Création du SO dans le menu Assets/Create/Score
public class SOScore : ScriptableObject
{
    // Champs pour les données du fichier
    [Header("Meilleur joueur 1")]
    [SerializeField] string _nom1 = "Joueur 1";
    [SerializeField] int _score1 = 0;

    [Header("Meilleur joueur 2")]
    [SerializeField] string _nom2 = "Joueur 2";
    [SerializeField] int _score2 = 0;

    [Header("Meilleur joueur 3")]
    [SerializeField] string _nom3 = "Joueur 3";
    [SerializeField] int _score3 = 0;

    string _fichier = "score.tim"; // Nom du fichier de sauvegarde

    [SerializeField] public List<NomScore> _lesNomsScores = new List<NomScore>(); // Liste des noms et scores
    [DllImport("__Internal")]
     private static extern void SynchroniserWebGL();
    // Accesseur et mutateur pour le nom du joueur 1
    public string nom1
    {
        get => _nom1;
        set => _nom1 = value;
    }

    // Accesseur et mutateur pour le score du joueur 1
    public int score1
    {
        get => _score1;
        set => _score1 = Mathf.Clamp(value, 0, int.MaxValue); // Clamp permet de limiter la valeur entre 0 et int.MaxValue
    }

    // Accesseur et mutateur pour le nom du joueur 2
    public string nom2
    {
        get => _nom2;
        set => _nom2 = value;
    }

    // Accesseur et mutateur pour le score du joueur 2
    public int score2
    {
        get => _score2;
        set => _score2 = Mathf.Clamp(value, 0, int.MaxValue); // Clamp permet de limiter la valeur entre 0 et int.MaxValue
    }

    // Accesseur et mutateur pour le nom du joueur 3
    public string nom3
    {
        get => _nom3;
        set => _nom3 = value;
    }

    // Accesseur et mutateur pour le score du joueur 3
    public int score3
    {
        get => _score3;
        set => _score3 = Mathf.Clamp(value, 0, int.MaxValue); // Clamp permet de limiter la valeur entre 0 et int.MaxValue
    }
    /// <summary>
    /// Méthode permettant de lire le fichier de sauvegarde
    /// </summary>
    public void LireFichier()
    {
        string fichierEtChemin = Application.persistentDataPath + "/" + _fichier; // Chemin du fichier de sauvegarde
        // Si le fichier existe
        if(File.Exists(fichierEtChemin))
        {
            string contenu = File.ReadAllText(fichierEtChemin);
            JsonUtility.FromJsonOverwrite(contenu, this);
            #if UNITY_EDITOR // Si on est dans l'éditeur
            UnityEditor.EditorUtility.SetDirty(this); // On indique que le SO a été modifié
            UnityEditor.AssetDatabase.SaveAssets(); // On sauvegarde les modifications
            #endif // Fin du if UNITY_EDITOR
            _lesNomsScores.Add(new NomScore() { nom = _nom1, score = _score1 }); // On ajoute les noms et scores dans la liste
            _lesNomsScores.Add(new NomScore() { nom = _nom2, score = _score2 });
            _lesNomsScores.Add(new NomScore() { nom = _nom3, score = _score3 });
            // Inscrit les noms et score dans le SOScore
            nom1 = _lesNomsScores[0].nom; 
            score1 = _lesNomsScores[0].score;
            nom2 = _lesNomsScores[1].nom;
            score2 = _lesNomsScores[1].score;
            nom3 = _lesNomsScores[2].nom;
            score3 = _lesNomsScores[2].score;
            _lesNomsScores.Clear(); // On vide la liste

        }//si le fichier existe pas le créer
        else
        {
            Debug.Log("Fichier inexistant");
            EcrireFichier();
        }

    }
    public void MettreAJourMeilleursScores()
{
    // Mettre à jour la liste des noms et scores
    _lesNomsScores.Clear();
    _lesNomsScores.Add(new NomScore() { nom = _nom1, score = _score1 });
    _lesNomsScores.Add(new NomScore() { nom = _nom2, score = _score2 });
    _lesNomsScores.Add(new NomScore() { nom = _nom3, score = _score3 });

    // Réorganiser les meilleurs scores dans l'ordre décroissant
    _lesNomsScores.Sort((a, b) => b.score.CompareTo(a.score));

    // Mettre à jour les valeurs du top 3
    nom1 = _lesNomsScores[0].nom;
    score1 = _lesNomsScores[0].score;
    nom2 = _lesNomsScores[1].nom;
    score2 = _lesNomsScores[1].score;
    nom3 = _lesNomsScores[2].nom;
    score3 = _lesNomsScores[2].score;

    // Effacer la liste des noms et scores
    _lesNomsScores.Clear();
}
    /// <summary>
    /// Méthode pour écrire dans le fichier de sauvegarde
    /// </summary>
    public void EcrireFichier()
    {
        _lesNomsScores.Sort((a,b) => b.score.CompareTo(a.score)); // On trie la liste par ordre décroissant
        string fichierEtChemin = Application.persistentDataPath + "/" + _fichier; // Chemin du fichier de sauvegarde
        string contenu = JsonUtility.ToJson(this, true); // On convertit le SO en JSON
        File.WriteAllText(fichierEtChemin, contenu); // On écrit dans le fichier
        if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            SynchroniserWebGL();
        }
    }
}
// Création d'une classe pour les noms et scores
[System.Serializable]
public class NomScore
{
    public string nom;
    public int score;
}

