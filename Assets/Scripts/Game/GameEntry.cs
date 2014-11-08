using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameEntry : MonoBehaviour {

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

	private bool ifFinish = false; 

	public Camera camera;

	private Level level;
	private GameObject hero;
	private GameObject deadHero;
	private GameObject finish;
	private IList bonuses = new ArrayList();

	private IDictionary laserCoverCells; 
	private IDictionary spearsCells; 

	private IList path;
	private IDictionary pathObjects;

	bool failStep = false;

	int  failCounter = 0;
	bool failChanged = false;

	void Start() {
		laserCoverCells = new Dictionary<Vector2, ArrayList>();
		spearsCells = new Dictionary<CountCell, GameObject>();

		path = new ArrayList();
		pathObjects = new Dictionary<Vector2, GameObject>();

		Loader loader = new Loader(PlayerPrefs.GetString("CurrentLevel"));
		level = loader.makeLevel();
		CreateLevelObjects();

		hero = InstantiateChild(heroPrefab, GameCoord(level.start), Quaternion.identity);
		deadHero = InstantiateChild(skullPrefab, GameCoord(level.start), Quaternion.identity);
		finish = InstantiateChild(finishPrefab, GameCoord(level.finish), Quaternion.identity);
		foreach (Vector2 coord in level.bonuses) {
			bonuses.Add(InstantiateChild(bonusPrefab, GameCoord(coord), Quaternion.identity));	
		}
		UpdateLasers();
	}

	void CreateLevelObjects() {
		for (int x = 0; x < level.xSize; x++) {
			for (int y = 0; y < level.ySize; y++) {
				GameObject prefab = null;
				switch (level.cells[x,y].type) {
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
						prefab = ((CountCell) level.cells[x,y]).isOn ? spearOnPrefab : spearOffPrefab;
						break;
					case CellType.WALL:  
						prefab = wallPrefab;
						break;
					default:
						prefab = unknownPrefab;	
						break;				
				}
				GameObject gameObject = InstantiateChild(prefab, GameCoord(x, y), Quaternion.identity);
				if (level.cells[x,y].type == CellType.SPEAR) {
					spearsCells.Add(level.cells[x,y], gameObject);
				}
				if (level.cells[x,y].type != CellType.EMPTY && level.cells[x,y].type != CellType.WALL) {
					InstantiateChild(tilePrefab, GameCoord(x, y), Quaternion.identity);
				}
			}		
		}

		CreateLaserCover();
	}

	private void CreateLaserCover() {
		for (int x = 0; x < level.xSize; x++) {
			for (int y = 0; y < level.ySize; y++) {
				if(level.cells[x,y].type == CellType.LASER) {
					LaserCell laserCell = (LaserCell)level.cells[x,y];
					ArrayList prefabs = new ArrayList();
					
					if(laserCell.up) {
						int yCoord = (int)laserCell.coordinate.y;
						
						while(--yCoord!=-1 ) {
							if(level.cells[x, yCoord].IsBocked()) { break; }
							
							prefabs.Add (InstantiateChild(lineVPrefab, GameCoord(x, yCoord), Quaternion.identity));
						}
					}
					if(laserCell.right) {
						int xCoord = (int)laserCell.coordinate.x;
						while(++xCoord!=level.xSize) {
							if(level.cells[xCoord, y].IsBocked()) { break; }
							
							prefabs.Add (InstantiateChild(lineHPrefab, GameCoord(xCoord, y), Quaternion.identity));
						}
					}
					
					if(laserCell.down) {
						int yCoord = (int)laserCell.coordinate.y;
						while(++yCoord!=level.ySize ) {
							if(level.cells[x, yCoord].IsBocked()) { break; }
							
							prefabs.Add (InstantiateChild(lineVPrefab, GameCoord(x, yCoord), Quaternion.identity));
						}
					}
					if(laserCell.left) {
						int xCoord = (int)laserCell.coordinate.x;
						while(--xCoord!=-1) {
							if(level.cells[xCoord, y].IsBocked()) { break; }
							
							prefabs.Add (InstantiateChild(lineHPrefab, GameCoord(xCoord, y), Quaternion.identity));
						}
					}
					laserCoverCells.Add (new Vector2(x, y), prefabs);
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
			camera.orthographicSize = Mathf.Max(level.xSize, level.ySize)/2f;
		}
	}

	private void UpdateLasers() {
		ICollection lasers = laserCoverCells.Keys;
		foreach (Vector2 laserCoordinate in lasers) {
			LaserCell laser = (LaserCell) level.cells[(int)laserCoordinate.x, (int)laserCoordinate.y];
			if(laser.isOn) {
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
		ICollection spears = spearsCells.Keys;
		foreach (CountCell spear in spears) {
			GameObject spearObject = (GameObject) spearsCells[spear];
			SpriteRenderer renderer = spearObject.GetComponent<SpriteRenderer>();
			SpriteRenderer newRenderer = spear.isOn ? spearOnPrefab.GetComponent<SpriteRenderer>()
				                                    : spearOffPrefab.GetComponent<SpriteRenderer>();
			renderer.sprite = newRenderer.sprite;	
			renderer.color = newRenderer.color;		
			spearObject.transform.localScale = spear.isOn ? spearOnPrefab.transform.localScale
														  : spearOffPrefab.transform.localScale;
		}
	}

	private bool isFail() {
		Vector2 heroCoord = hero.transform.position;

		ICollection lasers = laserCoverCells.Keys;
		foreach (Vector2 laserCoordinate in lasers) {
			LaserCell laser = (LaserCell) level.cells[(int)laserCoordinate.x, (int)laserCoordinate.y];
			if(laser.isOn) {
				ICollection covered = (ICollection) laserCoverCells[laserCoordinate];
				foreach (GameObject obj in covered) {
					if( heroCoord == (Vector2) obj.transform.position ) {
						return true;
					}
				}
			}
		}

		ICollection spears = spearsCells.Keys;
		foreach (CountCell spear in spears) {
			if(spear.isOn) {
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
			if (!ifFinish) {
				int score = 0;
				foreach (Vector2 bonus in level.bonuses) {
					if (path.Contains(bonus)) {
						score++;
					}
				}
				
				PlayerPrefs.SetInt("Score", score);
				PlayerPrefs.SetInt("Death", failCounter);

				GameObject win = InstantiateChild(winPrefab, new Vector2(0, 0), Quaternion.identity);
			}
			ifFinish = true;
		}

	}

	private bool IsWin() {
		return hero.transform.position == finish.transform.position;
	}

	private Vector2 GameCoord(float x, float y) {
		return new Vector2(x - (level.xSize-1)/2f, y - (level.ySize-1)/2f);
	}

	private Vector2 GameCoord(Vector2 pos) {
		return GameCoord(pos.x, pos.y);
	}
	
	private Vector2 LevelCoord(Vector2 pos) {
		return new Vector2(pos.x + (level.xSize-1)/2f, pos.y + (level.ySize-1)/2f);
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

		if (HeroOnMap (heroPos) && !HeroIsBocked (heroPos) && HeroIsMoved(heroPos)) {
			if (HeroIsBack(heroPos)) { 
				var last = (Vector2) path [path.Count - 1];
				path.Remove(last);
				GameObject lastObject = (GameObject) pathObjects[last];
				lastObject.SetActive(false);
				pathObjects.Remove(last);

				hero.transform.position = heroPos;
				level.backTick();
			} else if (!HeroWasHere(heroPos) && !failStep) {
				Vector2 oldCoord = LevelCoord (hero.transform.position);
				path.Add(oldCoord);
				GameObject prefab;
				Vector2 coord = GameCoord(oldCoord);
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
				level.nextTick();
			}
		}
	}



	private bool HeroOnMap(Vector2 pos) {
		var p = LevelCoord (pos);
		return p.x >= 0 && p.x <= level.xSize - 1 && p.y >= 0 && p.y <= level.ySize - 1; 
	}

	private bool HeroIsBocked(Vector2 pos) {
		var p = LevelCoord (pos);
		return level.cells [(int)p.x, (int)p.y].IsBocked ();
	}

	private bool HeroIsMoved(Vector2 pos) {
		var x = hero.transform.position.x; 
		var y = hero.transform.position.y;
		return pos.x != x || pos.y != y;
	}

	private bool HeroWasHere(Vector2 pos) {
		var p = LevelCoord (pos);
		return path.Contains(p);
	}

	private bool HeroIsBack(Vector2 pos) {
		if (path.Count != 0) {
			var p = LevelCoord (pos);
			var last = (Vector2)path [path.Count - 1];
			return p == last;
		} else {
			return false;
		}
	}

}
