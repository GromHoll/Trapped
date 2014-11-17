using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace TrappedGame {
    public class GameEntry : MonoBehaviour {
        
        public CellGOFactory cellGameObjectFactory;
        public LaserLineGOFactory laserLineGOFactory;

        private LevelLoader loader = new LevelLoader();
        private HeroInput heroInput = new HeroInput();
        private Game game;
        private Level level;

        private IDictionary<Path.PathLink, GameObject> pathObjects = new Dictionary<Path.PathLink, GameObject>();
        private IDictionary<LaserCell.Laser, IList<GameObject>> lasers = new Dictionary<LaserCell.Laser, IList<GameObject>>();
        
        public Camera gameCamera;

        private GameObject winWindow = null;
        
        // TODO refactor
        public GameObject pathHPrefab;
        public GameObject pathVPrefab;
                
        // TODO refactor
    	public GameObject heroPrefab;
    	public GameObject bonusPrefab;
    	public GameObject finishPrefab;
    	public GameObject skullPrefab;
        
        // TODO refactor
    	public GameObject winPrefab;

        // TODO refactor
    	private GameObject hero;
    	private GameObject deadHero;
    	private GameObject finish;
    	private IList bonuses = new ArrayList();
        
        // TODO refactor
        private IDictionary<CountCell, GameObject> spearsCells = new Dictionary<CountCell, GameObject>();

    	void Start() {        
            string levelName = PlayerPrefs.GetString("CurrentLevel");
            level = loader.LoadLevel(levelName);
            game = new Game(level);

    		CreateLevelObjects();
    	}

        void CreateLevelObjects() {
            // TODO refactor
    		for (int x = 0; x < level.GetSizeX(); x++) {
                for (int y = 0; y < level.GetSizeY(); y++) {
                    Cell cell = level.GetCell(x,y);
                    GameObject prefab = cellGameObjectFactory.GetCellPrefab(cell);
                    GameObject cellObject = GameUtils.InstantiateChild(prefab, GameUtils.ConvertToGameCoord(x, y, level), gameObject);
                    if (cell.GetCellType() == CellType.SPEAR) {
                        CountCell countCell = (CountCell) cell;
                        spearsCells.Add(countCell, cellObject);
    				}
                    if (cell.GetCellType() != CellType.EMPTY && cell.GetCellType() != CellType.WALL) {
                        GameObject tilePrefab = cellGameObjectFactory.GetCellPrefab(CellType.EMPTY);
                        GameUtils.InstantiateChild(tilePrefab, GameUtils.ConvertToGameCoord(x, y, level), gameObject);
    				}
    			}		
    		}
            
            lasers = laserLineGOFactory.CreateLasersForLevel(level);
            
            // TODO refactor
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

        void UpdateInput() {
            HeroMovement heroMovement = heroInput.GetMovement();
            heroMovement.MoveHeroInGame(game);
    	}

        void UpdadeGraphics() {
            UpdateHero();
            UpdatePath();
            UpdateLasers();
            UpdateSpears();
        }

        void UpdatePath() {
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
                        GameObject pathLinkPrefab = link.IsHorizontal() ? pathHPrefab : pathVPrefab;
                        Vector2 coord = GameUtils.ConvertToGameCoord(link.GetFromX(), link.GetFromY(), level);
                        if (link.IsWentUp()) { coord.y += 0.5f; }
                        if (link.IsWentRight()) { coord.x += 0.5f; }
                        if (link.IsWentDown()) { coord.y -= 0.5f; }
                        if (link.IsWentLeft()) { coord.x -= 0.5f; }
                        GameObject linkGameObject = GameUtils.InstantiateChild(pathLinkPrefab, coord, gameObject);
                        pathObjects[link] = linkGameObject;
                    }
                }
            }
        }

        void UpdateHero() {
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
                gameCamera.orthographicSize = Mathf.Max(level.GetSizeX(), level.GetSizeY())/2f;
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
            ICollection<CountCell> spears = spearsCells.Keys;
            foreach (CountCell spear in spears) {
                GameObject spearObject = (GameObject) spearsCells[spear];
                SpriteRenderer renderer = spearObject.GetComponent<SpriteRenderer>();
                GameObject newPrefab = cellGameObjectFactory.GetCellPrefab(spear);
                SpriteRenderer newRenderer = newPrefab.GetComponent<SpriteRenderer>();
                renderer.sprite = newRenderer.sprite;   
                renderer.color = newRenderer.color;     
                spearObject.transform.localScale = newPrefab.transform.localScale;
            }      
        }
    
        void ShowWinWindow() {
            if (winWindow == null) {        
                PlayerPrefs.SetInt("Score", game.GetScore());
                PlayerPrefs.SetInt("Death", game.GetHero().GetDeaths());
                winWindow = GameUtils.InstantiateChild(winPrefab, new Vector2(0, 0), gameObject);
            }
        }
    }
}