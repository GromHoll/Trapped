using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace TrappedGame {
    public class GameEntry : MonoBehaviour {

        public CellGOFactory cellGameObjectFactory;
        public LaserLineGOFactory laserLineGOFactory;
        public PathGOFactory pathGOFactory;

        private LevelLoader loader = new LevelLoader();
        private HeroInput heroInput;
        private Game game;
        private Level level;

        private IDictionary<Path.PathLink, GameObject> pathObjects = new Dictionary<Path.PathLink, GameObject>();
        private IDictionary<LaserCell.Laser, IList<GameObject>> lasers = new Dictionary<LaserCell.Laser, IList<GameObject>>();
        private IDictionary<SpearCell, GameObject> spearsCells = new Dictionary<SpearCell, GameObject>();

        private GameObject winWindow = null;
                
        
        public Camera gameCamera;

        public GameObject winPrefab;
    	public GameObject heroPrefab;
    	public GameObject bonusPrefab;
    	public GameObject finishPrefab;
    	public GameObject skullPrefab;

    	private GameObject hero;
    	private GameObject deadHero;
    	private GameObject finish;
        private IList<GameObject> bonuses = new List<GameObject>();
        

        void Start() {        
            string levelName = PlayerPrefs.GetString("CurrentLevel");
            level = loader.LoadLevel(levelName);
            game = new Game(level);
            
            heroInput = CreateInput();
    		CreateLevelObjects();
    	}

        private HeroInput CreateInput() {
            // TODO Move to factory when we will have more platforms
            if (Application.platform == RuntimePlatform.Android) {
                return new HeroSwipeInput();
            }
            return new HeroKeyInput();
        }

        private void CreateLevelObjects() {
            cellGameObjectFactory.CreateEmptyCells(level);
            cellGameObjectFactory.CreateWallCells(level);

            cellGameObjectFactory.CreateLaserCells(level);
            lasers = laserLineGOFactory.CreateLasersForLevel(level);

            spearsCells = cellGameObjectFactory.CreateSpearCells(level);
                        
            hero = GameUtils.InstantiateChild(heroPrefab, GameUtils.ConvertToGameCoord(level.GetStartX(), level.GetStartY(), level), gameObject);
            deadHero = GameUtils.InstantiateChild(skullPrefab, GameUtils.ConvertToGameCoord(level.GetStartX(), level.GetStartY(), level), gameObject);
            finish = GameUtils.InstantiateChild(finishPrefab, GameUtils.ConvertToGameCoord(level.GetFinishX(), level.GetFinishY(), level), gameObject);
            foreach (IntVector2 coord in level.GetBonuses()) {
                bonuses.Add(GameUtils.InstantiateChild(bonusPrefab, GameUtils.ConvertToGameCoord(coord, level), gameObject)); 
            }
    	}

    	private void Update() {
    		if (!game.IsWin()) {
                UpdateInput();
                UpdateCamera();
                UpdadeGraphics();
            } else {
                ShowWinWindow();
    		}
    	}

        private void UpdateInput() {
            HeroMovement heroMovement = heroInput.GetMovement();
            heroMovement.MoveHeroInGame(game);
    	}

        private void UpdadeGraphics() {
            UpdateHero();
            UpdatePath();
            UpdateLasers();
            UpdateSpears();
        }

        private void UpdatePath() {
            Path path = game.GetHero().GetPath();
            IEnumerable<Path.PathLink> existLinks = path.GetLinks();
            IEnumerable<Path.PathLink> showedLinks = pathObjects.Keys;
            HashSet<Path.PathLink> differens = new HashSet<Path.PathLink>(existLinks);
            differens.SymmetricExceptWith(showedLinks);
            foreach (Path.PathLink link in differens) {
                if (showedLinks.Contains(link)) {
                    GameObject linkGameObject = pathObjects[link];
                    pathObjects.Remove(link);
                    Destroy(linkGameObject);
                } else {
                    if (link.IsAdjacent()) {
                        Vector2 coord = GameUtils.ConvertToGameCoord(link.GetFromX(), link.GetFromY(), level);
                        GameObject pathGameObject = pathGOFactory.CreatePathSegment(link, coord);
                        pathObjects[link] = pathGameObject;
                    }
                }
            }
        }

        private void UpdateHero() {
            int x = game.GetHero().GetX();
            int y = game.GetHero().GetY();
            hero.transform.position = GameUtils.ConvertToGameCoord(x, y, level);
            deadHero.transform.position = GameUtils.ConvertToGameCoord(x, y, level);

            bool isDead = game.GetHero().IsDead();
            hero.SetActive(!isDead);
            deadHero.SetActive(isDead);
        }

        // TODO Find good way for scaling camera
        private void UpdateCamera() {
            if (gameCamera != null) {
                float screenXf = Screen.width;
                float screenYf = Screen.height;
                float screenScale = screenXf/screenYf;
                float levelXf = level.GetSizeX();
                float levelYf = level.GetSizeY();
                float levelScale = levelXf/levelYf;

                float scale = (screenScale > levelScale) ? levelXf : levelYf/screenScale;

                gameCamera.orthographicSize = scale/2;
            }
        }
        
        private void UpdateLasers() {
            foreach (KeyValuePair<LaserCell.Laser, IList<GameObject>> pair in lasers) {
                LaserCell.Laser line = pair.Key;
                foreach (GameObject laserObject in pair.Value) {
                    laserObject.SetActive(line.IsDanger());                        
                }
            }    
        }

        // TODO Union Spears prefabs in single prefab
        private void UpdateSpears() {
            ICollection<SpearCell> spears = spearsCells.Keys;
            foreach (SpearCell spear in spears) {
                GameObject spearObject = (GameObject) spearsCells[spear];
                SpriteRenderer renderer = spearObject.GetComponent<SpriteRenderer>();
                GameObject newPrefab = cellGameObjectFactory.GetCellPrefab(spear);
                SpriteRenderer newRenderer = newPrefab.GetComponent<SpriteRenderer>();
                renderer.sprite = newRenderer.sprite;   
                renderer.color = newRenderer.color;     
                spearObject.transform.localScale = newPrefab.transform.localScale;
            }      
        }
    
        private void ShowWinWindow() {
            if (winWindow == null) {        
                PlayerPrefs.SetInt("Score", game.GetScore());
                PlayerPrefs.SetInt("Death", game.GetHero().GetDeaths());
                winWindow = GameUtils.InstantiateChild(winPrefab, new Vector2(0, 0), gameObject);
            }
        }
    }
}