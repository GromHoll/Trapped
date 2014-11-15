using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace TrappedGame {
    public class GameEntry : MonoBehaviour {

        private LevelLoader loader = new LevelLoader();
        private HeroInput heroInput = new HeroInput();
        private Game game;

        public CellGameObjectFactory cellGameObjectFactory;
        
        private IDictionary<Path.PathLink, GameObject> pathObjects = new Dictionary<Path.PathLink, GameObject>();


    	public GameObject heroPrefab;
    	public GameObject bonusPrefab;
    	public GameObject finishPrefab;
    	public GameObject lineHPrefab;
    	public GameObject lineVPrefab;
    	public GameObject skullPrefab;
    	public GameObject pathHPrefab;
    	public GameObject pathVPrefab;
    	
    	public GameObject winPrefab;

    	private bool isFinish = false; 

    	public Camera camera;

    	private Level level;
    	private GameObject hero;
    	private GameObject deadHero;
    	private GameObject finish;
    	private IList bonuses = new ArrayList();

        private IDictionary<IntVector2, ArrayList> laserCoverCells = new Dictionary<IntVector2, ArrayList>();
        private IDictionary<CountCell, GameObject> spearsCells = new Dictionary<CountCell, GameObject>();

    	bool failStep = false;
    	bool failChanged = false;

    	void Start() {        
            string levelName = PlayerPrefs.GetString("CurrentLevel");
            level = loader.LoadLevel(levelName);
            game = new Game(level);

    		CreateLevelObjects();

            hero = InstantiateChild(heroPrefab, ConvertToGameCoord(level.GetStartX(), level.GetStartY()), Quaternion.identity);
            deadHero = InstantiateChild(skullPrefab, ConvertToGameCoord(level.GetStartX(), level.GetStartY()), Quaternion.identity);
            finish = InstantiateChild(finishPrefab, ConvertToGameCoord(level.GetFinishX(), level.GetFinishY()), Quaternion.identity);
    		foreach (IntVector2 coord in level.GetBonuses()) {
                bonuses.Add(InstantiateChild(bonusPrefab, ConvertToGameCoord(coord), Quaternion.identity));	
    		}
    		UpdateLasers();
    	}

    	void CreateLevelObjects() {
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
    	}

    	private void CreateLaserCover() {
    		for (int x = 0; x < level.GetSizeX(); x++) {
                for (int y = 0; y < level.GetSizeY(); y++) {
                    if(level.GetCell(x,y).GetCellType() == CellType.LASER) {
                        LaserCell laserCell = (LaserCell)level.GetCell(x,y);
    					ArrayList prefabs = new ArrayList();
    					
                        if(laserCell.IsDown()) {
    						int yCoord = laserCell.GetY();
    						
    						while(--yCoord!=-1 ) {
                                if(level.GetCell(x, yCoord).IsBocked()) { break; }
    							
                                prefabs.Add (InstantiateChild(lineVPrefab, ConvertToGameCoord(x, yCoord), Quaternion.identity));
    						}
    					}
    					if(laserCell.IsRight()) {
    						int xCoord = laserCell.GetX();
                            while(++xCoord!=level.GetSizeX()) {
                                if(level.GetCell(xCoord, y).IsBocked()) { break; }
    							
                                prefabs.Add (InstantiateChild(lineHPrefab, ConvertToGameCoord(xCoord, y), Quaternion.identity));
    						}
    					}
    					
                        if(laserCell.IsUp()) {
                            int yCoord = laserCell.GetY();
    						while(++yCoord!=level.GetSizeY() ) {
    							if(level.GetCell(x, yCoord).IsBocked()) { break; }
    							
                                prefabs.Add (InstantiateChild(lineVPrefab, ConvertToGameCoord(x, yCoord), Quaternion.identity));
    						}
    					}
    					if(laserCell.IsLeft()) {
                            int xCoord = laserCell.GetX();
    						while(--xCoord!=-1) {
                                if(level.GetCell(xCoord, y).IsBocked()) { break; }    							
                                prefabs.Add (InstantiateChild(lineHPrefab, ConvertToGameCoord(xCoord, y), Quaternion.identity));
    						}
    					}
    					laserCoverCells.Add (new IntVector2(x, y), prefabs);
    				}
    			}
    		}
    	}

    	private GameObject InstantiateChild(GameObject gameObject, Vector2 vector, Quaternion quaternion) {
    		GameObject child = Instantiate(gameObject, vector, quaternion) as GameObject;
    		child.transform.parent = transform;
    		return child;
    	}

        // TODO Remove convector methods after refactor
        public Vector2 ConvertToGameCoord(IntVector2 pos) {
            return ConvertToGameCoord(pos.x, pos.y);
        }
        
        public Vector2 ConvertToGameCoord(float x, float y) {
            float gameX = x - (level.GetSizeX() - 1)/2f;
            float gameY = y - (level.GetSizeY() - 1)/2f;
            return new Vector2(gameX, gameY);
        }

        // TODO Find good way for scaling camera
    	private void UpdateCamera() {
    		if (camera != null) {
                camera.orthographicSize = Mathf.Max(level.GetSizeX(), level.GetSizeY())/2f;
    		}
    	}

    	private void UpdateLasers() {
    		ICollection<IntVector2> lasers = laserCoverCells.Keys;
    		foreach (IntVector2 laserCoordinate in lasers) {
    			LaserCell laser = (LaserCell) level.GetCell(laserCoordinate.x, laserCoordinate.y);
                if(laser.IsOn()) {
    				ICollection covered = (ICollection) laserCoverCells[laserCoordinate];
    				foreach (GameObject obj in covered) {
    					obj.SetActive(true);
    				}
    			} else {
    				ICollection covered = (ICollection) laserCoverCells[laserCoordinate];
    				foreach (GameObject obj in covered) {
    					obj.SetActive(false);
    				}
    			}
    		}
    	}

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

    	private void Update() {
    		if (!game.IsWin()) {
                UpdateInput();
                UpdateCamera();
                UpdadeGraphics();
    		} else {
    			if (!isFinish) {
    				int score = 0;
    				foreach (IntVector2 bonus in level.GetBonuses()) {
                        // TODO Calculate score
    					if (/*path.Contains(bonus)*/ false) {
    						score++;
    					}
    				}
    				
    				PlayerPrefs.SetInt("Score", score);
    				PlayerPrefs.SetInt("Death", game.GetHero().GetDeaths());

    				InstantiateChild(winPrefab, new Vector2(0, 0), Quaternion.identity);
    			}
    			isFinish = true;
    		}
    	}

        void UpdateInput() {
            HeroMovement heroMovement = heroInput.GetMovement();
            heroMovement.MoveHeroInGame(game);
            // TODO Move to graphic update
            int x = game.GetHero().GetX();
            int y = game.GetHero().GetY();
            hero.transform.position = ConvertToGameCoord(x, y);
    	}

        void UpdadeGraphics() {
            UpdatePath();

            UpdateLasers();
            UpdateSpears();
            
            failStep = game.GetHero().IsDead();
            deadHero.transform.position = hero.transform.position;
            hero.SetActive(!failStep);
            deadHero.SetActive(failStep);
            
            if(failChanged != failStep) {
                failChanged = failStep;
            }
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
                        GameObject linkGameObject = InstantiateChild(pathLinkPrefab, coord, Quaternion.identity);
                        pathObjects[link] = linkGameObject;
                    }
                }
            }
        }
    }
}