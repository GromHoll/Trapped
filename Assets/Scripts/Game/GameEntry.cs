using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameEntry : MonoBehaviour {

    private LevelLoader loader = new LevelLoader();
    private Game game;

	public GameObject heroPrefab;
	public GameObject tilePrefab;
	public GameObject unknownPrefab;
	public GameObject bonusPrefab;
	public GameObject finishPrefab;
	public GameObject laserPrefab;
	public GameObject wallPrefab;
	public GameObject spearOnPrefab;
	public GameObject spearOffPrefab;
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
                Debug.Log("Coord = " + x + " " + y);
				GameObject prefab = null;
                Cell cell = level.GetCell(x,y);
                Debug.Log("cell = " + cell.GetCellType());
                switch (cell.GetCellType()) {
					case CellType.EMPTY: 
						prefab = tilePrefab;
						break;
					case CellType.UNKNOWN: 
						prefab = unknownPrefab;
						break;
					case CellType.LASER: 
						prefab = laserPrefab;
						break;
					case CellType.SPEAR: 
                        prefab = cell.IsDeadly() ? spearOnPrefab : spearOffPrefab;
						break;
					case CellType.WALL:  
						prefab = wallPrefab;
						break;
					default:
						prefab = unknownPrefab;	
						break;				
				}
                GameObject gameObject = InstantiateChild(prefab, level.ConvertToGameCoord(x, y), Quaternion.identity);
                if (cell.GetCellType() == CellType.SPEAR) {
                    CountCell countCell = (CountCell) cell;
                    spearsCells.Add(countCell, gameObject);
				}
                if (cell.GetCellType() != CellType.EMPTY && cell.GetCellType() != CellType.WALL) {
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
            SpriteRenderer newRenderer = spear.IsOn() ? spearOnPrefab.GetComponent<SpriteRenderer>()
				                                    : spearOffPrefab.GetComponent<SpriteRenderer>();
			renderer.sprite = newRenderer.sprite;	
			renderer.color = newRenderer.color;		
            spearObject.transform.localScale = spear.IsOn() ? spearOnPrefab.transform.localScale
														  : spearOffPrefab.transform.localScale;
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
		if (!IsWin ()) {
			UpdateCamera();
			UpdateInput();
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

	private bool IsWin() {
		return hero.transform.position == finish.transform.position;
	}

	void UpdateInput() {
		bool left  = Input.GetKeyDown(KeyCode.LeftArrow);
		bool right = Input.GetKeyDown(KeyCode.RightArrow);
		bool up    = Input.GetKeyDown(KeyCode.UpArrow);
		bool down  = Input.GetKeyDown(KeyCode.DownArrow);

		Vector2 heroPos = hero.transform.position;
		if (right && !left) {
			heroPos.x += 1; 
		} else if (!right && left) {
			heroPos.x -= 1; 
		}
		
		if (up && !down) {
			heroPos.y += 1; 
		} else if (!up && down) {
			heroPos.y -= 1; 
		}

		if (HeroOnMap (heroPos) && !HeroIsBocked(heroPos) && HeroIsMoved(heroPos)) {
			if (HeroIsBack(heroPos)) { 
				var last = path [path.Count - 1];
				path.Remove(last);
				GameObject lastObject = (GameObject) pathObjects[last];
				lastObject.SetActive(false);
				pathObjects.Remove(last);

				hero.transform.position = heroPos;
				level.BackTick();
			} else if (!HeroWasHere(heroPos) && !failStep) {
                IntVector2 oldCoord = level.ConvertToLevelCoord(hero.transform.position);
				path.Add(oldCoord);
				GameObject prefab;
                Vector2 coord = level.ConvertToGameCoord(oldCoord);
				if (hero.transform.position.x - heroPos.x == 0) {
					prefab = pathVPrefab;
					if (hero.transform.position.y - heroPos.y < 0) {
						coord.y += 0.5f;
					} else {
						coord.y -= 0.5f;
					}
				} else {
					prefab = pathHPrefab;
					if (hero.transform.position.x - heroPos.x < 0) {
						coord.x += 0.5f;
					} else {
						coord.x -= 0.5f;
					}
				}
				GameObject pathObject = InstantiateChild(prefab, coord, Quaternion.identity);
				pathObjects.Add(oldCoord, pathObject);

				hero.transform.position = heroPos;
				level.NextTick();
			}
		}
	}

	private bool HeroOnMap(Vector2 pos) {
        var p = level.ConvertToLevelCoord(pos);
        return p.x >= 0 && p.x <= level.GetSizeX() - 1 && p.y >= 0 && p.y <= level.GetSizeY() - 1; 
	}

    private bool HeroIsBocked(Vector2 pos) {
        var p = level.ConvertToLevelCoord (pos);
		return level.GetCell(p.x, p.y).IsBocked();
	}

	private bool HeroIsMoved(Vector2 pos) {
		var x = hero.transform.position.x; 
		var y = hero.transform.position.y;
		return pos.x != x || pos.y != y;
	}

	private bool HeroWasHere(Vector2 pos) {
        var p = level.ConvertToLevelCoord(pos);
		return path.Contains(p);
	}

	private bool HeroIsBack(Vector2 pos) {
		if (path.Count != 0) {
            var p = level.ConvertToLevelCoord(pos);
			var last = path[path.Count - 1];
			return p == last;
		} else {
			return false;
		}
	}
}