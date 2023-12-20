using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanneauScore : MonoBehaviour
{
    [Header("Source")]
    [SerializeField] private SOPerso _donnees; // Source de données pour les scores
    [SerializeField] private Sauvegardeur _sauvegardeur; // Référence au gestionnaire de sauvegarde

    [System.Serializable]
    public class ScoreChamp
    {
        public string nomObjet; // Nom de l'objet associé au champ
        public TextMeshProUGUI nomChamp; // Texte affichant le nom de l'objet
        public TextMeshProUGUI valeurChamp; // Texte affichant la valeur de l'objet
        public TextMeshProUGUI pointsChamp; // Texte affichant le multiplicateur de points
        public int multiplicateur; // Multiplicateur de points associé à l'objet
        public TextMeshProUGUI totalChamp; // Texte affichant le total de points pour l'objet
        public int valeurAssociee; // Valeur associée à l'objet (utilisée pour les calculs)
    }

    [Header("Champs noms")]
    [SerializeField] private List<ScoreChamp> scoreChamps; // Liste des champs de score à afficher
    [SerializeField] private TextMeshProUGUI _champTotal; // Texte affichant le total des points

    [SerializeField] private float _intervale = 1f; // Intervalle entre les mises à jour des champs
    private bool _scoreCharge = true; // Indique si les scores ont été chargés
    public bool scoreCharge => _scoreCharge;

    private int _total; // Total des points
    public int total => _total;

    void Start()
    {
        // Calcul du total des points pour chaque objet
        int totalBrocoPts = _donnees.nbDeBrocolisRamasses * scoreChamps[0].multiplicateur;
        int totalEnemisPts = _donnees.nbEnemisTues * scoreChamps[1].multiplicateur;
        int totalNiveauPts = (_donnees.niveau - 1) * scoreChamps[2].multiplicateur;
        int totalTempsPts = _donnees.tempsRestant * scoreChamps[3].multiplicateur;
        int totalViePts = _donnees.nbDeVie * scoreChamps[4].multiplicateur;
        _total = totalBrocoPts + totalEnemisPts + totalNiveauPts + totalTempsPts + totalViePts;

        // Lancement de la coroutine pour mettre à jour les champs de score
        StartCoroutine(CoroutineMiseAJourScoresTable(totalBrocoPts, totalEnemisPts, totalNiveauPts, totalTempsPts, totalViePts, _total));

        // Vérification si le total dépasse le score minimal
        if (total > _sauvegardeur.scoreMin)
        {
            Debug.Log("Score minimal atteint");
            _sauvegardeur.AfficherEntrerNom();
        }
    }

    IEnumerator CoroutineMiseAJourScoresTable(int totalBrocoPts, int totalEnemisPts, int totalNiveauPts, int totalTempsPts, int totalViePts, int total)
{
    foreach (ScoreChamp champ in scoreChamps)
    {
        string valeurChampText = string.Empty; // Texte de la valeur du champ
        string champTotalText = string.Empty;

        // Vérification du nom de l'objet et mise à jour des champs correspondants
        if (champ.nomObjet == "Brocoli")
        {
            valeurChampText = _donnees.nbDeBrocolisRamasses.ToString();
            champ.nomChamp.text = champ.nomObjet + (valeurChampText != "1" ? "s" : "") + ":";
            yield return new WaitForSeconds(_intervale);
            champ.valeurChamp.text = valeurChampText;
            yield return new WaitForSeconds(_intervale);
            champTotalText = totalBrocoPts.ToString();
        }
        else if (champ.nomObjet == "Ennemi")
        {
            valeurChampText = _donnees.nbEnemisTues.ToString();
            champ.nomChamp.text = champ.nomObjet + (valeurChampText != "1" ? "s" : "") + ":";
            yield return new WaitForSeconds(_intervale);
            champ.valeurChamp.text = valeurChampText;
            yield return new WaitForSeconds(_intervale);
            champTotalText = totalEnemisPts.ToString();
        }
        else if (champ.nomObjet == "Niveau")
        {
            valeurChampText = (_donnees.niveau - 1).ToString();
            champ.nomChamp.text = champ.nomObjet + (valeurChampText != "1" ? "x" : "") + " complété" + (valeurChampText != "1" ? "s" : "") + ":";
            yield return new WaitForSeconds(_intervale);
            champ.valeurChamp.text = valeurChampText;
            yield return new WaitForSeconds(_intervale);
            champTotalText = totalNiveauPts.ToString();
        }
        else if (champ.nomObjet == "Seconde")
        {
            valeurChampText = _donnees.tempsRestant.ToString();
            champ.nomChamp.text = champ.nomObjet + (valeurChampText != "1" ? "s" : "") + ":";
            yield return new WaitForSeconds(_intervale);
            champ.valeurChamp.text = valeurChampText;
            yield return new WaitForSeconds(_intervale);
            champTotalText = totalTempsPts.ToString();
        }
        else if (champ.nomObjet == "Vie")
        {
            valeurChampText = _donnees.nbDeVie.ToString();
            champ.nomChamp.text = champ.nomObjet + (valeurChampText != "1" ? "s" : "") + " restante" + (valeurChampText != "1" ? "s" : "") + ":";
            yield return new WaitForSeconds(_intervale);
            champ.valeurChamp.text = valeurChampText;
            yield return new WaitForSeconds(_intervale);
            champTotalText = totalViePts.ToString();
        }

        champ.pointsChamp.text = "x" + champ.multiplicateur;
        yield return new WaitForSeconds(_intervale);

        champ.totalChamp.text = champTotalText;
        yield return new WaitForSeconds(_intervale);
    }

    _champTotal.text = total.ToString(); // Affecter le total au champ de texte du total

    _scoreCharge = false;
}

}
