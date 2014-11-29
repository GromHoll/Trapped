using System.Collections.Generic;
using System.Linq;
using TrappedGame.Control.Hero;
using TrappedGame.Model.Cells;
using UnityEngine;

namespace TrappedGame.Main {
    public class GameEntry : MonoBehaviour {

        public CellGOFactory cellGameObjectFactory;
        public LaserLineGOFactory laserLineGoFactory;
        public PathGOFactory pathGoFactory;

        private LevelLoader loader = new LevelLoader();
        private HeroInput heroInput;
        private Game game;
        private Level level;

        private IDictionary<Path.PathLink, GameObject> pathObjects = new Dictionary<Path.PathLink, GameObject>();
        private IDictionary<LaserCell.Laser, IList<GameObject>> lasers = new Dictionary<LaserCell.Laser, IList<GameObject>>();
        private IDictionary<SpearCell, GameObject> spearsCells = new Dictionary<SpearCell, GameObject>();

        private WinMenu winMenu;
                
        public Camera gameCamera;

        public GameObject winPrefab;
    	public GameObject heroPrefab;
        public GameObject bonusPrefab;
        public GameObject timeBonusPrefab;
    	public GameObject finishPrefab;

        private HeroController heroController;

        void Start() {        
            var levelName = PlayerPrefs.GetString(Preferences.CURRENT_LEVEL);
            level = loader.LoadLevel(levelName);
            game = new Game(level);
            
            heroInput = CreateInput();
    		CreateLevelObjects();
    	}

        private HeroInput CreateInput() {
            // TODO Move to factory when we will have more platforms or input styles
            if (Application.platform == RuntimePlatform.Android) {
                return new HeroSwipeInput();
            }
            return new HeroKeyInput();
        }

        private void CreateLevelObjects() {
            cellGameObjectFactory.CreateEmptyCells(level);
            cellGameObjectFactory.CreateWallCells(level);

            cellGameObjectFactory.CreateLaserCells(level);
            lasers = laserLineGoFactory.CreateLasersForLevel(level);

            spearsCells = cellGameObjectFactory.CreateSpearCells(level);
                        
            var hero = GameUtils.InstantiateChild(heroPrefab, GameUtils.ConvertToGameCoord(level.GetStartX(), level.GetStartY(), level), gameObject);
            heroController = hero.GetComponent<HeroController>();
            heroController.SetGame(game);

            GameUtils.InstantiateChild(finishPrefab, GameUtils.ConvertToGameCoord(level.GetFinishX(), level.GetFinishY(), level), gameObject);
            foreach (var coord in level.GetBonuses()) {
                GameUtils.InstantiateChild(bonusPrefab, GameUtils.ConvertToGameCoord(coord, level), gameObject); 
            }
            foreach (var coord in level.GetTimeBonuses().Keys) {
                GameUtils.InstantiateChild(timeBonusPrefab, GameUtils.ConvertToGameCoord(coord, level), gameObject); 
            }
            
            var winWindow = GameUtils.InstantiateChild(winPrefab, new Vector2(0, 0), gameObject);
            winMenu = winWindow.GetComponent<WinMenu>();
            winMenu.SetGame(game);
            winMenu.SetActive(false);
        }

        void Update() {
    		if (!game.IsWin()) {
                UpdateInput();
                UpdateGraphics();
            } else {
                ShowWinWindow();
            }
            UpdateCamera();
    	}

        private void UpdateInput() {
            if (heroController.IsMoving()) return;
            var heroMovement = heroInput.GetMovement();
            heroMovement.MoveHeroInGame(game);
        }

        private void UpdateGraphics() {
            UpdatePath();
            UpdateLasers();
        }

        private void UpdatePath() {
            var path = game.GetHero().GetPath();
            var existLinks = path.GetLinks();
            var showedLinks = pathObjects.Keys;
            var differens = new HashSet<Path.PathLink>(existLinks);
            differens.SymmetricExceptWith(showedLinks);
            foreach (var link in differens) {
                if (showedLinks.Contains(link)) {
                    var linkGameObject = pathObjects[link];
                    pathObjects.Remove(link);
                    Destroy(linkGameObject);
                } else {
                    if (link.IsAdjacent()) {
                        var coord = GameUtils.ConvertToGameCoord(link.GetFromX(), link.GetFromY(), level);
                        var pathGameObject = pathGoFactory.CreatePathSegment(link, coord);
                        pathObjects[link] = pathGameObject;
                    }
                }
            }
        }        

        // TODO Move to laser controller
        private void UpdateLasers() {
            foreach (var pair in lasers) {
                var line = pair.Key;
                foreach (var laserObject in pair.Value) {
                    laserObject.SetActive(line.IsDanger());                        
                }
            }    
        }

        // TODO Find good way for scaling camera
        private void UpdateCamera() {
            if (gameCamera == null) return;
            
            var screenXf = Screen.width;
            var screenYf = Screen.height;
            var screenScale = screenXf/screenYf;
            var levelXf = level.GetSizeX();
            var levelYf = level.GetSizeY();
            var levelScale = levelXf/levelYf;

            var scale = (screenScale > levelScale) ? levelXf : levelYf/screenScale;
            gameCamera.orthographicSize = scale/2f;
        }
    
        private void ShowWinWindow() {      
            winMenu.SetActive(true);
        }
    }
}