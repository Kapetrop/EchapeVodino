using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Classe qui contrôle la taille d'une salle
/// Classe qui dessine un carré autour de la salle
/// Auteurs du code: Antoine Chartier et Jean-Samuel David
/// Auteur des commentaires Jean-Samuel David
///  </summary>
public class Salle : MonoBehaviour
{
    static Vector2Int _taille = new Vector2Int(36, 18); // Champs privé et statique pour la taille d'une salle
    public static Vector2Int taille { get => _taille; } // accesseur 
    [SerializeField] Transform _repere; // position du repere pour les objects ( bonus & activateur) dans cette salle #tp3 Antoine
    [SerializeField, Tooltip("Est-ce que la salle accueille des ennemis ?")] bool _contientEnnemis; // est-ce que la salle doit contenir des ennemis ? #synthèse Antoine
    [SerializeField] private Transform[] _tPointsPatrouille; // liste des points de patrouille #synthese Antoine
    [SerializeField] private Transform[] _tPointsPatrouille2; // liste des points de patrouille #synthese Antoine
    public Transform[] tPointsPatrouille { get => _tPointsPatrouille; } // accesseur #synthese Antoine
    public Transform[] tPointsPatrouille2 { get => _tPointsPatrouille2; } // accesseur #synthese Antoine
    private BoxCollider2D _collider; // collider de la salle #tp4 Antoine


    private void Start()
    {
        // ajoute un collider pour la salle, utile pour détecter dans quel salle est le perso #tp4 Antoine
        _collider = gameObject.AddComponent<BoxCollider2D>(); 
        _collider.isTrigger = true;
        _collider.size = _taille - Vector2Int.one*2; // taille du collider moins 2 pour le safety #tp4 Antoine
    }

    /// <summary> #tp3 Antoine
    /// Place l'objet donné en paramètre sur le repère donné en serialized field
    /// </summary>
    /// <param name="modele">objet transmis depuis niveau</param>
    /// <returns>la position du repère vers niveau.cs </returns>
    public Vector2Int PlacerSurRepere(GameObject modele)
    {
        Vector3 pos;
        pos = _repere.position;
        GameObject objet = Instantiate(modele, pos, Quaternion.identity, transform.parent);
        
        if (modele.name == "Perso") // mecanisme pour détruie l'ennemi dans la salle où le perso Spawn #tp4 Antoine
        {
            BaseEnnemi[] ennemis = GetComponentsInChildren<BaseEnnemi>(); 
            foreach (BaseEnnemi ennemi in ennemis) // détruit tous les ennemis dans la salle #synthèse Antoine
            {
                Destroy(ennemi.gameObject);
            };
        }
        return Vector2Int.FloorToInt(pos);
    }

    /// <summary> #tp4 Antoine
    /// Place un gameObject d'ennemie envoyé par Niveau sur un repère dans la salle
    /// </summary>
    /// <param name="modele">Game Object d'ennemi</param>
    public void PlacerEnnemi(GameObject modele, int numEnnemi)
    {
        // si la salle ne contient pas d'ennemis, ne rien faire #synthese Antoine
        if(!_contientEnnemis) return;
  
        GameObject ennemi = Instantiate(modele, transform.position, Quaternion.identity, transform);
        ennemi.GetComponent<BaseEnnemi>().numEnnemi = numEnnemi;
    }

    /// <summary> #synthese Antoine
    /// Detecte si le perso est dans la salle
    /// Pour savoir si la musique doit changer en fonction de si la salle contient encore des ennemis
    /// </summary>
    /// <param name="other">l'objet touché</param>
    private void OnTriggerEnter2D(Collider2D other) {
        Perso perso = other.GetComponent<Perso>(); // detecte si le perso est dans la salle #tp4 Antoine
        if (perso != null) // si le perso est trouvé #tp4 Antoine
        {
            VerifieSiContientEnnemi(); // vérifie si la salle contient encore des ennemis pour changer la musique #tp4 Antoine
        }
    }
    /// <summary>
    /// le délais est pour laissé le temps a l'ennemi de s'auto-déruitre #tp4 Antoine
    /// </summary>
    public void VerifieSiContientEnnemiAvecDelais()
    {
        Invoke("VerifieSiContientEnnemi", 0.5f);
    }
    /// <summary> #tp4 Antoine
    /// va chercher les ennemis dans la salle et vérifier s'il y en a encore
    /// si oui continue la musique lugubre
    /// si non ajoute la musique pour la musique de la salle vide
    /// </summary>
    public void VerifieSiContientEnnemi()
    {
        if (GetComponentInChildren<BaseEnnemi>() == null)
        {
            Debug.Log("la salle" + gameObject.name + " ne contient pas d'ennemi");
            SoundManager.instance.ChangerEtatLecturePiste(TypePiste.musiqueEvenSalle, true);
        }
        else
        {
            SoundManager.instance.ChangerEtatLecturePiste(TypePiste.musiqueEvenSalle, false);
        }
    }


    /// <summary>
    /// Méthode pour dessiner un carré rouge
    /// qui apparaît même dans le mode "auteur"
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Choix de la couleur

        Gizmos.DrawWireCube(transform.position, (Vector3Int)_taille); // Positione le gizmos avec en lien la taille de la salle
    }
}
