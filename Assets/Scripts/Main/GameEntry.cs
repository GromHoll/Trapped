using System.Collections.Generic;
using TrappedGame.Control.Hero;
using TrappedGame.Model;
using TrappedGame.Model.Loader;
using TrappedGame.Utils;
using TrappedGame.View.Controllers;
using TrappedGame.View.Graphic;
using TrappedGame.View.GUI;
using UnityEngine;

namespace TrappedGame.Main {
    public class GameEntry : MonoBehaviour {

        public CellGOFactory cellGameObjectFactory;
        public PathGOFactory pathGoFactory;

        private readonly LevelLoader loader = new LevelLoader();
        private HeroInput heroInput;
        private Game game;
        private Level level;

        private readonly IDictionary<Path.PathLink, GameObject> pathObjects = new Dictionary<Path.PathLink, GameObject>();

        private WinMenu winMenu;
		public GameObject winMenuObject;
                
        public Camera gameCamera;
	
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
            cellGameObjectFactory.CreateSpearCells(level);
                        
            var hero = GameUtils.InstantiateChild(heroPrefab, GameUtils.ConvertToGameCoord(level.StartX, level.StartY, level), gameObject);
            heroController = hero.GetComponent<HeroController>();
            heroController.SetGame(game);

            GameUtils.InstantiateChild(finishPrefab, GameUtils.ConvertToGameCoord(level.FinishX, level.FinishY, level), gameObject);
            foreach (var coord in level.Bonuses) {
                GameUtils.InstantiateChild(bonusPrefab, GameUtils.ConvertToGameCoord(coord, level), gameObject); 
            }
            foreach (var coord in level.TimeBonuses.Keys) {
                GameUtils.InstantiateChild(timeBonusPrefab, GameUtils.ConvertToGameCoord(coord, level), gameObject); 
            }
            
            winMenu = winMenuObject.GetComponent<WinMenu>();
            winMenu.SetGame(game);
            winMenu.Hide();
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

        // TODO Find good way for scaling camera
        private void UpdateCamera() {
            if (gameCamera == null) return;
            
            var screenXf = Screen.width;
            var screenYf = Screen.height;
            var screenScale = screenXf/screenYf;
            var levelXf = level.SizeX;
            var levelYf = level.SizeY;
            var levelScale = levelXf/levelYf;

            var scale = (screenScale > levelScale) ? levelXf : levelYf/screenScale;
            gameCamera.orthographicSize = scale/2f;
        }
    
        private void ShowWinWindow() {      
            winMenu.Show();
        }
    }
}