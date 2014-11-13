using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TrappedGame {
    public class GameEntry : MonoBehaviour {

        private LevelLoader loader = new LevelLoader();
        private HeroInput heroInput = new HeroInput();
        private Game game;

        public CellGameObjectFactory cellGameObjectFactory;

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

        private IList<IntVector2> path = new List<IntVector2>();
        private IDictionary<IntVector2, GameObject> pathObjects = new Dictionary<IntVector2, GameObject>();

    	bool failStep = false;

    	int  failCounter = 0;
    	bool failChanged = false;

    	void Start() {        
            string levelName = PlayerPrefs.GetString("CurrentLevel");
            level = loader.LoadLevel(levelName);
            game = new Game(level);

    		CreateLevelObjects();

            hero = InstantiateChild(heroPrefab, level.ConvertToGameCoord(level.GetStartX(), level.GetStartY()), Quaternion.identity);
            deadHero = InstantiateChild(skullPrefab, level.ConvertToGameCoord(level.GetStartX(), level.GetStartY()), Quaternion.identity);
            finish = InstantiateChild(finishPrefab, level.ConvertToGameCoord(level.GetFinishX(), level.GetFinishY()), Quaternion.identity);
    		foreach (IntVector2 coord in level.GetBonuses()) {
                bonuses.Add(InstantiateChild(bonusPrefab, level.ConvertToGameCoord(coord), Quaternion.identity));	
    		}
    		UpdateLasers();
    	}

    	void CreateLevelObjects() {
    		for (int x = 0; x < level.GetSizeX(); x++) {
                for (int y = 0; y < level.GetSizeY(); y++) {
                    Cell cell = level.GetCell(x,y);
                    GameObject prefab = cellGameObjectFactory.GetCellPrefab(cell);
                    GameObject gameObject = InstantiateChild(prefab, level.ConvertToGameCoord(x, y), Quaternion.identity);
                    if (cell.GetCellType() == CellType.SPEAR) {
                        CountCell countCell = (CountCell) cell;
                        spearsCells.Add(countCell, gameObject);
    				}
                    if (cell.GetCellType() != CellType.EMPTY && cell.GetCellType() != CellType.WALL) {
                        GameObject tilePrefab = cellGameObjectFactory.GetCellPrefab(CellType.EMPTY);
                        InstantiateChild(tilePrefab, level.ConvertToGameCoord(x, y), Quaternion.identity);
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
    							
                                prefabs.Add (InstantiateChild(lineVPrefab, level.ConvertToGameCoord(x, yCoord), Quaternion.identity));
    						}
    					}
    					if(laserCell.IsRight()) {
    						int xCoord = laserCell.GetX();
                            while(++xCoord!=level.GetSizeX()) {
                                if(level.GetCell(xCoord, y).IsBocked()) { break; }
    							
                                prefabs.Add (InstantiateChild(lineHPrefab, level.ConvertToGameCoord(xCoord, y), Quaternion.identity));
    						}
    					}
    					
                        if(laserCell.IsUp()) {
                            int yCoord = laserCell.GetY();
    						while(++yCoord!=level.GetSizeY() ) {
    							if(level.GetCell(x, yCoord).IsBocked()) { break; }
    							
                                prefabs.Add (InstantiateChild(lineVPrefab, level.ConvertToGameCoord(x, yCoord), Quaternion.identity));
    						}
    					}
    					if(laserCell.IsLeft()) {
                            int xCoord = laserCell.GetX();
    						while(--xCoord!=-1) {
                                if(level.GetCell(xCoord, y).IsBocked()) { break; }    							
                                prefabs.Add (InstantiateChild(lineHPrefab, level.ConvertToGameCoord(xCoord, y), Quaternion.identity));
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

    	private bool isFail() {
    		Vector2 heroCoord = hero.transform.position;

            ICollection<IntVector2> lasers = laserCoverCells.Keys;
            foreach (IntVector2 laserCoordinate in lasers) {
    			LaserCell laser = (LaserCell) level.GetCell(laserCoordinate.x, laserCoordinate.y);
                if(laser.IsOn()) {
    				ICollection covered = (ICollection) laserCoverCells[laserCoordinate];
    				foreach (GameObject obj in covered) {
    					if( heroCoord == (Vector2) obj.transform.position ) {
    						return true;
    					}
    				}
    			}
    		}

            ICollection<CountCell> spears = spearsCells.Keys;
    		foreach (CountCell spear in spears) {
                if(spear.IsOn()) {
    				GameObject spearObject = (GameObject) spearsCells[spear];
    				if(heroCoord == (Vector2) spearObject.transform.position) {
    					return true;
    				}
    			}
    		}

    		return false;
    	}

    	private void Update() {
    		if (!game.IsWin()) {
                UpdateInput();
                // TODO Update Graphic
                UpdateCamera();
    			UpdateLasers();
    			UpdateSpears();

    			failStep = isFail();
    			deadHero.transform.position = hero.transform.position;
    			hero.SetActive(!failStep);
    			deadHero.SetActive(failStep);

    			if(failChanged != failStep) {
    				failChanged = failStep;
    				if (failStep) {
    					failCounter ++;
    				}
    			}

    		} else {
    			if (!isFinish) {
    				int score = 0;
    				foreach (IntVector2 bonus in level.GetBonuses()) {
    					if (path.Contains(bonus)) {
    						score++;
    					}
    				}
    				
    				PlayerPrefs.SetInt("Score", score);
    				PlayerPrefs.SetInt("Death", failCounter);

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
            hero.transform.position = level.ConvertToGameCoord(x, y);
    	}
    }
}