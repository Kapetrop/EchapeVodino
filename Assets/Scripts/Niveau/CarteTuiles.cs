using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Classe qui sert à générer les modèles de salle
/// Affiche les possibilités possible
/// Rend transparente les salles selon leur probabilité d'être afficher
/// Envoit directement vers niveau les possibiliter choisi pour la salle
/// Auteurs du code: Antoine Chartier et Jean-Samuel David
/// Auteur des commentaires Jean-Samuel David
///  </summary>
public class CarteTuiles : MonoBehaviour
{
    [SerializeField, Range(1, 100)]
    private int _chanceDepart; // valeur choisi pour la chance que le modèle soit afficher

    public int chanceDepart { get => _chanceDepart; } // Accesseur pour la chance d'être afficher

    private Tilemap _tilemap; // Donne accès au tilemaps
  
    void Awake()
    {

        int chance = Random.Range(0, 100); // Valeur aléatoire pour décider si une salle est "choisi"

        if (chance <= _chanceDepart) // si la salle est "chanceuse"
        {
            _tilemap = GetComponent<Tilemap>(); // Accès au tilemap
            Niveau niveau = GetComponentInParent<Niveau>(); // Accès pour générer les tuiles dans Niveau
            BoundsInt bounds = _tilemap.cellBounds; // Trouve les limite de la salle
            Vector3Int decalage = Vector3Int.FloorToInt(transform.position); // Valeur qui aide à bien placer les tuile
            for (int y = bounds.yMin; y < bounds.yMax; y++) // Boucle pour déterminer les tuile à chaque position
            {
                for (int x = bounds.xMin; x < bounds.xMax; x++)
                {

                    Vector3Int pos = new Vector3Int(x, y, 0); // Inertion des valeur x et y pour le positionnement
                    TraiterUneTuile(_tilemap, niveau, pos, decalage); // Appele de la méthode pour traiter la tuile
                }

            }
        }
        gameObject.SetActive(false); // Désactivation du modele de salle
    }
    /// <summary>
    /// Méthode pour modifier le canal alpha sur la scène
    /// lorsque le mode "Auteur" est activé
    /// </summary>
    /// <param name="demarer">le jeu est en fonction?</param>
    void ChangerAlpha( bool demarer) 
    {
        _tilemap = GetComponent<Tilemap>(); // Accès au tilemap
        if(demarer == false) // Si le jeu roule
        {
            _tilemap.color = new Color(1, 1, 1, (float)_chanceDepart / 100); // afficher les tuiles selon la chance d'apparaître
        }
        else
        {
            _tilemap.color = new Color(1, 1, 1, 1); // Sinon remettre l'alpha à 100%
        }
    }
    /// <summary>
    /// Méthode qui traite les tuiles pour les envoyer dans niveau
    /// </summary>
    /// <param name="tm">valeur de la tilemap</param>
    /// <param name="pos">la position de la tuile</param>
    /// <param name="decalage">le décalage de la tuile</param>
    void TraiterUneTuile(Tilemap tm, Niveau niveau, Vector3Int pos, Vector3Int decalage)
    {

        TileBase tuile = tm.GetTile(pos); // prend la tuile à la position choisi

  
        if (tuile != null) // si la tuile est pas nvide
        {
            Niveau.instance.PlacerTuile(pos, decalage, tuile); // Générer la tuile dans le niveau #tp3 Antoine

        }

    }
    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    void OnDrawGizmos()
    {
        if (Application.isPlaying == false) ChangerAlpha( Application.isPlaying); // Si l'application ne jou pas, appelle la méthode ChangerAlpha
    }

}
