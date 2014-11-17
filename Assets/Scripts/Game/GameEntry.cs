using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace TrappedGame {
    public class GameEntry : MonoBehaviour {
        
        public CellGameObjectFactory cellGameObjectFactory;

        private LevelLoader loader = new LevelLoader();
        private HeroInput heroInput = new HeroInput();
        private Game game;
        private Level level;

        private IDictionary<Path.PathLink, GameObject> pathObjects = new Dictionary<Path.PathLink, GameObject>();
        private IDictionary<LaserCell.Laser, IList<GameObject>> lasers = new Dictionary<LaserCell.Laser, IList<GameObject>>();

        private GameObject winWindow = null;
        
        // TODO refactor
        public GameObject pathHPrefab;
        public GameObject pathVPrefab;
        public GameObject lineHPrefab;
        public GameObject lineVPrefab;
        
        // TODO refactor
    	public GameObject heroPrefab;
    	public GameObject bonusPrefab;
    	public GameObject finishPrefab;
    	public GameObject skullPrefab;
        
        // TODO refactor
    	public GameObject winPrefab;

    	public Camera gameCamera;
        
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
                    GameObject gameObject = InstantiateChild(prefab, ConvertToGameCoord(x, y), Quaternion.identity);
                    if (cell.GetCellType() == CellType.SPEAR) {
                        CountCell countCell = (CountCell) cell;
                        spearsCells.Add(countCell, gameObject);
    				}
                    if (cell.GetCellType() != CellType.EMPTY && cell.GetCellType() != CellType.WALL) {
                        GameObject tilePrefab = cellGameObjectFactory.GetCellPrefab(CellType.EMPTY);
                        InstantiateChild(tilePrefab, ConvertToGameCoord(x, y), Quaternion.identity);
    				}
    			}		
    		}

    		CreateLaserCover();
            
            // TODO refactor
            hero = InstantiateChild(heroPrefab, ConvertToGameCoord(level.GetStartX(), level.GetStartY()), Quaternion.identity);
            deadHero = InstantiateChild(skullPrefab, ConvertToGameCoord(level.GetStartX(), level.GetStartY()), Quaternion.identity);
            finish = InstantiateChild(finishPrefab, ConvertToGameCoord(level.GetFinishX(), level.GetFinishY()), Quaternion.identity);
            foreach (IntVector2 coord in level.GetBonuses()) {
                bonuses.Add(InstantiateChild(bonusPrefab, ConvertToGameCoord(coord), Quaternion.identity)); 
            }
    	}

    	private void CreateLaserCover() {
            foreach (LaserCell.Laser line in level.GetLaserLines()) {
                GameObject prefab = line.IsHorizontal() ? lineHPrefab : lineVPrefab;
                IList<GameObject> laserObjects = new List<GameObject>();
                IntRect cover = line.GetCover();
                for (int x = cover.GetMinX(); x <= cover.GetMaxX(); x++) {
                    for (int y = cover.GetMinY(); y <= cover.GetMaxY(); y++) {
                        GameObject laserObject = InstantiateChild(prefab, ConvertToGameCoord(x, y), Quaternion.identity);
                        laserObject.SetActive(line.IsDanger());     
                        laserObjects.Add(laserObject);                        
                    }
                }
                lasers.Add(line, laserObjects);
            }
    	}
        
        // TODO refactor
    	private GameObject InstantiateChild(GameObject gameObject, Vector2 vector, Quaternion quaternion) {
    		GameObject child = Instantiate(gameObject, vector, quaternion) as GameObject;
    		child.transform.parent = transform;
    		return child;
    	}
        
        // TODO refactor
        // TODO Remove convector methods after refactor
        public Vector2 ConvertToGameCoord(IntVector2 pos) {
            return ConvertToGameCoord(pos.x, pos.y);
        }
        
        // TODO refactor
        public Vector2 ConvertToGameCoord(float x, float y) {
            float gameX = x - (level.GetSizeX() - 1)/2f;
            float gameY = y - (level.GetSizeY() - 1)/2f;
            return new Vector2(gameX, gameY);
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
                        Vector2 coord = ConvertToGameCoord(link.GetFromX(), link.GetFromY());
                        if (link.IsWentUp()) { coord.y += 0.5f; }
                        if (link.IsWentRight()) { coord.x += 0.5f; }
                        if (link.IsWentDown()) { coord.y -= 0.5f; }
                        if (link.IsWentLeft()) { coord.x -= 0.5f; }
                        GameObject linkGameObject = InstantiateChild(pathLinkPrefab, coord, Quaternion.identity);
                        pathObjects[link] = linkGameObject;
                    }
                }
            }
        }

        void UpdateHero() {
            int x = game.GetHero().GetX();
            int y = game.GetHero().GetY();
            hero.transform.position = ConvertToGameCoord(x, y);
            deadHero.transform.position = ConvertToGameCoord(x, y);

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
                winWindow = InstantiateChild(winPrefab, new Vector2(0, 0), Quaternion.identity);
            }
        }
    }
}