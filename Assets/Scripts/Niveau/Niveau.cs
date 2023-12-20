using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;
using Cinemachine;


/// <summary>
/// Classe qui contrôle l'apparition d'une salle
/// Classe qui décide combien de salles à faire apparaître
/// Classe qui Génère les tuiles qui ferme les bordures exterieurs du niveau entier
/// Auteurs du code: Antoine Chartier et Jean-Samuel David
/// Auteur des commentaires Jean-Samuel David & Antoine
///  </summary>
public class Niveau : MonoBehaviour
{
    static private Niveau _instance; // instance de la classe
    static public Niveau instance => _instance; // getter
    [SerializeField] private SOPerso _donneePerso; // donnee du perso #tp4 Antoine
    [SerializeField] Tilemap _tilemap; // Accès au notion de tilemap
    public Tilemap tilemap => _tilemap; // Accesseur publique de tilemap
    [SerializeField] Vector2Int _taille;    // Taille du niveau #tp3 Antoine
    [SerializeField] Salle[] _tSallesModeles; // Tableau avec les salles possible
    private Vector2Int _tailleMoinsUneBordure; // store la taille des salles #tp3 Antoine
    [SerializeField] TileBase[] _tTuilesFermantes; // Tableau avec les tuiles fermantes suggérées
    UnityEvent _activeBonus = new UnityEvent(); // L'évènement qui active les bonus #tp3 Antoine
    public UnityEvent activeBonus => _activeBonus; // L'accesseur du Event Bonus #tp3 Antoine
    [SerializeField] GameObject _gmVirtualCam; // Parle au GameObjet cinemachine #tp4 Antoine
    private CinemachineVirtualCamera _cmVirtualCam; // Parle a la composante virtual caméra du cinemachine #tp4 Antoine
    private CinemachineConfiner2D _cmConfiner; // Parle a la composante confiner du cinemachine #tp4 Antoine
    [SerializeField] PolygonCollider2D _polyConfiner; // le polygone confiner #tp4 Antoine
    private float _modificateurTemps = 1; // le modificateur de temps pour l'horloge #synthèse Antoine
    public float modificateurTemps { set => _modificateurTemps = value; get => _modificateurTemps; } // muttateur et accesseur du modificateur de temps #synthèse Antoine
    private float _dureeHorloge; // La durée de l'effet de l'horloge #synthèse Antoine
    public float dureeHorloge { set => _dureeHorloge = value; } // muttateur de la durée de l'effet de l'horloge #synthèse Antoine
    UnityEvent _activeHorloge = new UnityEvent(); // L'évènement qui active l'horloge #synthèse Antoine
    public UnityEvent activeHorloge => _activeHorloge; // L'accesseur du Event horloge #synthèse Antoine
    private IEnumerator _coroutineRetablirTempsNormal; // la coroutine qui rétablie le temps normal #synthèse Antoine

    [Header("Placement objets")]
    [SerializeField] private GameObject _clef; // #tp3 Antoine
    [SerializeField] private GameObject _porte; // #tp3 Antoine
    [SerializeField] private GameObject _activateur; // #tp3 Antoine
    [SerializeField] private int _nbBonus; // #tp3 Antoine
    [SerializeField] private GameObject[] _bonusModeles; // Sabot(saut) & bijou(vitesse) #tp3 Antoine
    [SerializeField] private int _nbMalus; // #synthèse Antoine
    [SerializeField] private GameObject[] _malusModeles; // poulet $ horloge #synthèse Antoine
    [SerializeField] private GameObject _perso; // #tp3 Antoine
    [SerializeField] private GameObject[] _ennemis; // #tp4 Antoine
    [SerializeField] private GameObject _brocoliModele; // #tp3 Jean-Samuel
    [SerializeField] private int _nbBroco; // #tp3 Jean-Samuel
    private List<Vector2Int> _lesPosLibres = new List<Vector2Int>(); // liste des positions libres sur le superTileMap
    private List<List<Salle>> _mesSalles = new List<List<Salle>>(); // liste de listes de salle #tp3 Antoine
    private bool _salleBonusEstPlace = false; // booléen qui vérifie si la salle bonus est placé #synthèse Antoine
    [SerializeField] Parallax _parallax; // va chercher le script parallax #synthese Antoine
    private ParallaxLayer _parallaxLayerCiel; // change la vitesse des nuages #synthese Antoine
    private float _vitesseCielSauvegarde;  // sauvegarde la vitesse du ciel #synthese Antoine

    void Awake()
    {
        if (_instance != null) { Destroy(gameObject); return; } // Singleton
        _instance = this; // Singleton
        _cmVirtualCam = _gmVirtualCam.GetComponent<CinemachineVirtualCamera>(); // va chercher le composant VirtualCam pour lui assigner quoi folllow # tp4 Antoine
        _cmConfiner = _gmVirtualCam.GetComponent<CinemachineConfiner2D>();  // va chercher le composant Confiner pour le modifier selon la taille du niveau # tp4 Antoine

        ComplexifierLeNiveau();
        CreerLesSalles();
        Vector2Int placementClef = ChoixEmplacementClef(); // trouve position clef ( contour du niveau )#tp3 Antoine
        Vector2Int placementPorte = ChoixEmplacementPorte(placementClef);// trouve position porte à l'oposée de celle de la clef #tp3 Antoine
        Vector2Int placementActivateur = ChoixEmplacementActivateur(placementClef, placementPorte); // trouve position activateur dans une salle libre de clef ou porte #tp3 Antoine

        Vector2Int posClef = _mesSalles[placementClef.x][placementClef.y].PlacerSurRepere(_clef); // appel la salle spécifique où placer la clef #tp3 Antoine
        Vector2Int posPorte = _mesSalles[placementPorte.x][placementPorte.y].PlacerSurRepere(_porte); // appel la salle spécifique où placer la porte #tp3 Antoine
        Vector2Int posActivateur = _mesSalles[placementActivateur.x][placementActivateur.y].PlacerSurRepere(_activateur);// appel la salle spécifique où placer l'activateur #tp3 Antoine
        int placementPersoX = Random.Range(0, _taille.x);
        Vector2Int posPerso = _mesSalles[placementPersoX][placementPorte.y].PlacerSurRepere(_perso); // récupère l'emplacement du repere pour le perso et appel la bonne salle ( sur la meme rangé que la porte) #tp3 Antoine
        Debug.Log("<color=orange> L'index de la salle PERSO : <b>" + placementPersoX + ", " + placementPorte.y + " </b></color>");
        CreerContour();
        TrouverPosLibres(posClef, posPorte, posActivateur, posPerso); // trouve les pos libres restantes pour les brocolis #tp3 Antoine
        PlacerLesBrocolis();
        PlacerLesBonusEtMalus();

        Debug.Log("les pos libre: " + _lesPosLibres.Count + string.Join(", ", _lesPosLibres));
    }

    /// <summary> # tp4 Antoine
    /// est appellé juste après Awake pour délcenché le compteur de temps
    /// </summary>
    private void Start()
    {
        StartCoroutine(CoroutineTemps(Perso.instance.donnees.tempsRestant));
        _coroutineRetablirTempsNormal = CoroutineRetablirTempsNormal();
        _parallaxLayerCiel = _parallax.parallaxLayers[1]; // va chercher le layer du ciel #synthèse Antoine
        _vitesseCielSauvegarde = _parallaxLayerCiel.constantSpeedX;
    }
    IEnumerator CoroutineTemps(int tempsSecondes) // Compte à rebours du jeu #tp4 Antoine
    {
        while (tempsSecondes > 0)
        {
            yield return new WaitForSeconds(1f / _modificateurTemps); // les secondes sont affectées par l'horloge bonus/malus #synthèse Antoine
            tempsSecondes--;
            // passe par l'instance du personnage pour communiquer à son SO pour modifier le temps # tp4 Antoine
            Perso.instance.donnees.tempsRestant = tempsSecondes;
        }
    }

    public void AltererTemps()
    {
        StopCoroutine(_coroutineRetablirTempsNormal); // safety pour ne pas que les coroutine s'accumule #synthèse Antoine
        StartCoroutine(CoroutineRetablirTempsNormal());
        activeHorloge.Invoke(); // appel l'event qui active l'horloge pour modifier la patrouille des ennemis #synthèse Antoine
        SoundManager.instance.AltererPitch();
        if (_modificateurTemps != 1) // si le temps est modifié par une horloge alors on modifie la vitesse du ciel #synthèse Antoine
        {
            Debug.Log("le temps est modifié");
            foreach (ParallaxLayer layer in _parallax.parallaxLayers) // change la vitesse de tous les nuages #synthèse Antoine
            {
                if (_modificateurTemps > 1) layer.constantSpeedX = _modificateurTemps * 2; // si le temps est accéléré alors les nuages vont plus vite #synthèse Antoine
                if (_modificateurTemps < 1) layer.constantSpeedX = _modificateurTemps * 0.5f; // si le temps est ralenti alors les nuages vont moins vite #synthèse Antoine
            }
        }
    }

    IEnumerator CoroutineRetablirTempsNormal()
    {
        yield return new WaitForSeconds(_dureeHorloge);
        _modificateurTemps = 1;
        Debug.Log("J'appelle l'event désactiveHorloge" + Niveau.instance.modificateurTemps);
        activeHorloge.Invoke();
        SoundManager.instance.AltererPitch();
            foreach (ParallaxLayer layer in _parallax.parallaxLayers) // rétablie la vitesse de tous les nuages #synthèse Antoine
            {
                layer.constantSpeedX = _vitesseCielSauvegarde;
                Debug.Log("je rétablie la vitesse enregistré: " +_vitesseCielSauvegarde + "pour le layer" + layer.ToString());
            }
    }
    /// <summary> # tp4 Antoine
    /// Verifie a quel niveau est rendu le joueur et le complexifie en fonction en lui ajoutant des salles
    /// </summary>
    private void ComplexifierLeNiveau()
    {
        int modificateurChanceTailleY = 0;
        if (_donneePerso.niveau > 1)
        {
            // 60% de chance d'augmenter la chance d'augmenter la taille du niveau en Y  
            // modificateur désactivé après le niveau 6 # tp4 Antoine
            if (Random.Range(1, 10) > 4 || _donneePerso.niveau < 6) modificateurChanceTailleY = Random.Range(1, _donneePerso.niveau + 1);
            _taille.x = Random.Range(_taille.x, _donneePerso.niveau);
            _taille.y = Random.Range(_taille.y + 1, _donneePerso.niveau + modificateurChanceTailleY);
        }
        if (_taille.x > 6) _taille.x = 6; // taille max en X # synthese Antoine
        if (_taille.y > 6) _taille.y = 6; // taille max en Y # synthese Antoine

        _nbBonus += _donneePerso.niveau * 2; // augmente le nombre de bonus selon le niveau #synthèse Antoine
        _nbMalus += _donneePerso.niveau * 2; // augmente le nombre de malus selon le niveau #synthèse Antoine
        _nbBroco += _donneePerso.niveau * 2; // augmente le nombre de brocoli selon le niveau #synthèse Antoine
    }

    private void PlacerLesBrocolis() // #tp3 Jean-Samuel
    {
        Transform conteneur = new GameObject("Brocolis").transform;
        conteneur.parent = transform;
        int nbBroco = _nbBroco * (_taille.x * _taille.y);

        for (int i = 0; i < nbBroco; i++)
        {
            Vector2Int pos = ObtenirUnePosLibre();

            Vector3 pos3 = (Vector3)(Vector2)pos + _tilemap.transform.position + _tilemap.tileAnchor;
            Instantiate(_brocoliModele, pos3, Quaternion.identity, conteneur);

            if (_lesPosLibres.Count - _nbBonus == 0) { Debug.LogWarning("Aucun espace libre"); break; } // termine la boucle si le nombre d'espace - nb de bonus est écoulé 
        }
    }
    private void PlacerLesBonusEtMalus() // #tp3 Antoine
    {
        Transform conteneur = new GameObject("Bonus").transform;
        conteneur.parent = transform;
        Transform conteneurMal = new GameObject("Malus").transform;
        conteneurMal.parent = transform;
        int indexBonusModele = 0; // sert a shuffler a travers les modèles de bonus disponnibles dans le tableau #tp3 Antoine
        for (int i = 0; i < _nbBonus; i++)
        {
            Vector2Int pos = ObtenirUnePosLibre();

            Vector3 pos3 = (Vector3)(Vector2)pos + _tilemap.transform.position + _tilemap.tileAnchor;
            indexBonusModele++;
            if (indexBonusModele >= _bonusModeles.Length) indexBonusModele = 0; // si index dépasse les modèles dispo alors retourne a 0 #tp3 Antoine
            Instantiate(_bonusModeles[indexBonusModele], pos3, Quaternion.identity, conteneur);
            if (_bonusModeles[indexBonusModele].name == "horloge") // si c'est une horloge, rend la bonus #synthese Antoine
            {
                _bonusModeles[indexBonusModele].GetComponent<Horloge>().estBonusOveride = true;
            }
        }
        int indexMalusModele = 0; // sert a shuffler a travers les modèles de Malus disponnibles dans le tableau #tp3 Antoine
        for (int i = 0; i < _nbMalus; i++)
        {
            Vector2Int pos = ObtenirUnePosLibre();

            Vector3 pos3 = (Vector3)(Vector2)pos + _tilemap.transform.position + _tilemap.tileAnchor;
            indexMalusModele++;
            if (indexMalusModele >= _malusModeles.Length) indexMalusModele = 0; // si index dépasse les modèles dispo alors retourne a 0 #tp3 Antoine
            Instantiate(_malusModeles[indexMalusModele], pos3, Quaternion.identity, conteneurMal);
            if (_malusModeles[indexMalusModele].name == "horloge") // si c'est une horloge, rend la malus #synthese Antoine
            {
                _malusModeles[indexMalusModele].GetComponent<Horloge>().estMalusOveride = true;
            }
        }
    }
    private Vector2Int ObtenirUnePosLibre() // #tp3 Jean-Samuel
    {
        int indexPosLibre = Random.Range(0, _lesPosLibres.Count);
        Vector2Int pos = _lesPosLibres[indexPosLibre];
        _lesPosLibres.RemoveAt(indexPosLibre);
        return pos;
    }
    private void CreerLesSalles()
    {
        _tailleMoinsUneBordure = Salle.taille - Vector2Int.one; // Détermine la taille d'une salle moins une bordure #tp3 Antoine
        for (int x = 0; x < _taille.x; x++) // Boucle pour générer les salles sur axe X
        {
            List<Salle> rangee = new List<Salle>(); // creer le bon nombre de "rangés" de liste #tp3 Antoine

            for (int y = 0; y < _taille.y; y++) // Boucle pour générer les salles sur axe des Y
            {
                Vector2 pos = new Vector2(_tailleMoinsUneBordure.x * x, _tailleMoinsUneBordure.y * y); // Calcule place la salle par apport au valeur de la boucle
                rangee.Add(Instantiate(_tSallesModeles[ChoisirUneSalle()], pos, Quaternion.identity, transform)); // instancie les salles dans chacunne des rangés #tp3 Antoine

                rangee[y].name = "Salle_" + x + "_" + y; // Nomme la salle par rapport à sa position

                for (int i = 0; i < Random.Range(1, 3); i++)
                {
                    int ennemiChoisi = Random.Range(0, _ennemis.Length);
                    PlacerEnnemis(rangee[y], ennemiChoisi, i); // Place un ennemi dans chaque salle en lui transmettant la salle #tp4 Antoine
                }
            }
            _mesSalles.Add(rangee);
        }
    }

    /// <summary> #synthese Antoine & Jean-Samuel
    /// Choisit une salle aléatoirement dans le tableau de salles
    /// Lorsque le salle bonus est placé ( la derniere du tableau ) La pige change pour s'assurer qu'elle ne soit pas choisie à nouveau
    /// </summary>
    private int ChoisirUneSalle()
    {
        int salleChoisi = Random.Range(0, _tSallesModeles.Length);
        if (!_salleBonusEstPlace)
        {
            if (salleChoisi == _tSallesModeles.Length - 1)
            {
                _salleBonusEstPlace = true;
                Debug.Log("Salle bonus placé");
                return salleChoisi;
            }
        }
        else if (_salleBonusEstPlace) salleChoisi = Random.Range(0, _tSallesModeles.Length - 1);
        return salleChoisi;
    }

    /// <summary> #tp4 Antoine
    /// place les ennemis au endroit disponnible pour pouvoir faire un test de son
    /// lorsqu'il y a ennemi dans une salle la musique doit changer 
    /// </summary>
    private void PlacerEnnemis(Salle salle, int ennemiChoisi, int numEnnemi)
    {
        salle.PlacerEnnemi(_ennemis[ennemiChoisi], numEnnemi);
    }

    /// <summary>
    /// Choix de la salle où mettre la clef selon la taille du niveau #tp3 Antoine
    /// Trouve une index de salle dans la liste de listes au rebord du niveau
    /// </summary>
    /// <returns>Numero de la salle</returns>
    private Vector2Int ChoixEmplacementClef()
    {
        List<int[]> edgeIndices = new List<int[]>(); // list de tableau de type int permet de recevoir les index des salles au extremité #tp3 Antoine

        //isole les indices aux extrémités et les ajoute dans une liste de tableau.
        for (int x = 0; x < _mesSalles.Count; x++)
        {
            for (int y = 0; y < _mesSalles[x].Count; y++)
            {
                if (x == 0 || y == 0 || x == _mesSalles.Count - 1 || y == _mesSalles[x].Count - 1) // si la valeur de x ou y = 0 ou la limite alors c'est une pair valide #tp3 Antoine
                {
                    int[] indexPair = new int[2] { x, y }; // tableau enregistrant les pairs valide de x,y #tp3 Antoine
                    edgeIndices.Add(indexPair);
                }
            }
        }
        int nbSallesPossible = edgeIndices.Count;
        int choixAleatoire = Random.Range(0, nbSallesPossible);
        int[] indexSalle = edgeIndices[choixAleatoire];
        Debug.Log("<color=green>L'index de la salle CLEF: <b>" + indexSalle[0] + ", " + indexSalle[1] + "</b></color>");

        Vector2Int placementClef = new Vector2Int(indexSalle[0], indexSalle[1]);
        return placementClef;
    }
    /// <summary>
    /// Choix de la salle ou mettre la porte selon la pos de la clef 
    /// #tp3 Antoine
    /// </summary>
    /// <param name="posPorte">Numero de la salle avec la clef</param>
    /// <returns>Numero de la salle avec la porte</returns>
    private Vector2Int ChoixEmplacementPorte(Vector2Int posClef)
    {
        int posPorteX = 0;
        int posPorteY = 0;
        // si la pos X de la clef est plus grande que la moitié des x du niveau alors choisi un nombre aléatoire entre 0 et la moitié # tp3 Antoine
        // sinon choisi un nombre entre la moitié et taille max
        if (posClef.x >= _taille.x / 2) posPorteX = Random.Range(0, _taille.x / 2);
        else posPorteX = Random.Range(_taille.x / 2, _taille.x);

        if (posClef.y >= _taille.y / 2) posPorteY = Random.Range(0, _taille.y / 2);
        else posPorteY = Random.Range(_taille.y / 2, _taille.y);

        // if pour ne pas faire de while infinie
        // si le niveau est trop petit alors c'est possible que la clef apparaisse dans la même salle que le perso
        // empêche la clef d'apparaite sur la meme rangé que la porte (comme le perso)
        // #tp3 Antoine
        if (_taille.y > 1)
        {
            while (posPorteY == posClef.y)
            {
                if (posClef.y >= _taille.y / 2) posPorteY = Random.Range(0, _taille.y / 2);
                else posPorteY = Random.Range(_taille.y / 2, _taille.y);
            }

        }

        Debug.Log("<color=red> L'index de la salle PORTE : <b>" + posPorteX + ", " + posPorteY + " </b></color>");
        Vector2Int emplacementPorte = new Vector2Int(posPorteX, posPorteY);
        return emplacementPorte;
    }
    /// <summary>
    /// Choix du numero de salle où mettre l'activateur
    /// Prend en considération les place occupé par la clef et la porte
    /// </summary>
    /// <param name="posClef">numero de salle de la clef</param>
    /// <param name="posPorte">numero de salle de la porte</param>
    /// <returns>numero de salle de l'activateur</returns>
    private Vector2Int ChoixEmplacementActivateur(Vector2Int posClef, Vector2Int posPorte)
    {
        Vector2Int posActivateur = new Vector2Int(Random.Range(0, _taille.x), Random.Range(0, _taille.y)); // numero de salle aléatoire à travers le niveau #tp3 Antoine
        if (_taille.y > 1 && _taille.x > 1) // grandeur de niveau minimum pour ne pas que la boucle soit infinie #tp3 Antoine
            while (posActivateur == posClef || posActivateur == posPorte) // tant que sa position est la même que celle de la clef ou la porte refait une pige #tp3 Antoine
            {
                posActivateur = new Vector2Int(Random.Range(0, _taille.x), Random.Range(0, _taille.y));
            }
        return posActivateur;
    }

    /// <summary>
    /// Trouve les positions libre à travers le niveau et les affichent
    /// </summary>
    /// <param name="posClef">pos de la clef à supprimer</param>
    /// <param name="posPorte">pos de la porte à supprimer</param>
    void TrouverPosLibres(Vector2Int posClef, Vector2Int posPorte, Vector2Int posActivateur, Vector2Int posPerso)
    {
        BoundsInt _bornes = _tilemap.cellBounds;

        for (int y = _bornes.yMin; y < _bornes.yMax; y++)
        {
            for (int x = _bornes.xMin; x < _bornes.xMax; x++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                TileBase tuile = _tilemap.GetTile(pos);
                if (tuile == null)
                {
                    _lesPosLibres.Add(new Vector2Int(x, y));
                }
            }
        }

        _lesPosLibres.Remove(posClef); // enlève la clef des pos libre
        _lesPosLibres.Remove(posActivateur); // enlève l'activateur des pos libre
        _lesPosLibres.Remove(posPerso); // enlève le perso des pos libre
        for (int y = 0; y <= 2; y++) // enlève la porte élargie de 3x3 a partir de son centre vers le haut de la liste des pos libre
        {
            for (int x = -1; x <= 1; x++)
            {
                Vector2Int pos = new Vector2Int(posPorte.x + x, posPorte.y + y);
                _lesPosLibres.Remove(pos);
            }
        }
    }
    /// <summary>
    /// Place des tuiles tout autour du niveau #tp3 Antoine
    /// Ajuste la Virtual Cinemachine #tp4 Antoine
    /// </summary>
    private void CreerContour()
    {
        Vector2Int tailleNiveau = _taille * _tailleMoinsUneBordure; // Détermine la grosseur total du niveau
        Vector2Int min = Vector2Int.zero - Salle.taille / 2; // Détermine les coordonées min et max
        Vector2Int max = min + tailleNiveau;

        for (int y = min.y; y <= max.y; y++) // Boucle pour générer les tuiles sur les limites du niveau
        {
            for (int x = min.x; x <= max.x; x++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0); // Détermine la position d'une tuile
                if (y == min.y) tilemap.SetTile(pos, _tTuilesFermantes[1]); // place une tuile sur le dessus
                if (y == max.y) tilemap.SetTile(pos, _tTuilesFermantes[2]); // place une tuile sur le sol
                if (x == min.x || x == max.x) tilemap.SetTile(pos, _tTuilesFermantes[0]); // place une tuile sur les murs
            }
        }
        int decalageY = 2; // pour include plus de base dans la caméra #synthèse Antoine
        _polyConfiner.transform.position = new Vector3(min.x, min.y - decalageY, 0); // place le cam confiner en bas a guache #tp4 Antoine
        _polyConfiner.transform.localScale = new Vector3(tailleNiveau.x + 1, tailleNiveau.y + 1 + decalageY, 0); // ajuste la taille du confiner selon la taille du niveai #tp4 Antoine
        _cmVirtualCam.Follow = Perso.instance.transform; // assigne le perso à la caméra #tp4 Antoine
    }


    /// <summary>
    /// Place la tuile recu par CarteTuile #tp3 Antoine
    /// </summary>
    public void PlacerTuile(Vector3Int pos, Vector3Int decalage, TileBase tuile)
    {
        tilemap.SetTile(pos + decalage, tuile); // Générer la tuile dans le niveau
    }
}
