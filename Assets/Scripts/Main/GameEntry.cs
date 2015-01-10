using System.Collections.Generic;
using System.Linq;
using TrappedGame.Control.Hero;
using TrappedGame.Model;
using TrappedGame.Model.LevelLoader;
using TrappedGame.Utils;
using TrappedGame.View.Controllers;
using TrappedGame.View.Graphic;
using TrappedGame.View.GUI;
using TrappedGame.View.Sync;
using UnityEngine;

namespace TrappedGame.Main {
    public class GameEntry : MonoBehaviour, ISyncGameObject {

        public PathGOFactory pathGoFactory;
        public CellGOFactory cellGameObjectFactory;

        public GameObject keyPrefab;
    	public GameObject heroPrefab;
        public GameObject bonusPrefab;
    	public GameObject finishPrefab;
        public GameObject platformPrefab;
        public GameObject timeBonusPrefab;

		public GameObject winMenuObject;

        private Game game;
        private Level level;
        private HeroInput heroInput;
        private HeroController heroController;

        private WinMenu winMenu;

        private IList<ISyncGameObject> syncGameObjects = new List<ISyncGameObject>();
        private readonly IDictionary<Path.PathLink, GameObject> pathObjects = new Dictionary<Path.PathLink, GameObject>();

        void Start() {   
			var levelName = PlayerPrefs.GetString(Preferences.CURRENT_LEVEL);
			var loader = LevelLoaderFactory.GetLoader(levelName);
            level = loader.LoadLevel(levelName);
            game = new Game(level);
            heroInput = CreateInput();
            CreateLevelObjects();
            UpdateCameraScale();
        }

        private HeroInput CreateInput() {
            // TODO Move to factory when we will have more platforms or input styles
            if (Application.platform == RuntimePlatform.Android) {
                return new HeroSwipeInput();
            }
            return new HeroKeyInput();
        }

        private void CreateLevelObjects() {
            syncGameObjects = cellGameObjectFactory.CreateLevel(level);
                        
            var hero = GameObjectUtils.InstantiateChild(heroPrefab, GameUtils.ConvertToGameCoord(level.StartX, level.StartY, level), gameObject);
            heroController = hero.GetComponent<HeroController>();
            heroController.Game = game;
            syncGameObjects.Add(heroController);

            GameObjectUtils.InstantiateChild(finishPrefab, GameUtils.ConvertToGameCoord(level.FinishX, level.FinishY, level), gameObject);
            foreach (var coord in level.Bonuses) {
                GameObjectUtils.InstantiateChild(bonusPrefab, GameUtils.ConvertToGameCoord(coord, level), gameObject); 
            }
            foreach (var coord in level.TimeBonuses.Keys) {
                GameObjectUtils.InstantiateChild(timeBonusPrefab, GameUtils.ConvertToGameCoord(coord, level), gameObject);
            } 
            foreach (var key in level.Keys) {
                GameObjectUtils.InstantiateChild(keyPrefab, GameUtils.ConvertToGameCoord(key.Coordinate, level), gameObject);
            }
            foreach (var platform in level.Platforms) {
                var platformGameObject = GameObjectUtils.InstantiateChild(platformPrefab, GameUtils.ConvertToGameCoord(platform.Coordinate, level), gameObject);
                var controller = platformGameObject.GetComponent<PlatformController>();
                controller.SerPlatform(platform, level);
            }
            
            winMenu = winMenuObject.GetComponent<WinMenu>();
            winMenu.SetGame(game);
            winMenu.Hide();
        }

        public bool IsSync() {
            return syncGameObjects.All(sync => sync.IsSync());
        }

        void Update() {
    		if (!game.IsWin()) {
                UpdateInput();
                UpdateGraphics();
            } else {
                ShowWinWindow();
            }
    	}

        private void UpdateInput() {
            if (IsSync()) {
                var heroMovement = heroInput.GetMovement();
                heroMovement.MoveHeroInGame(game);
            }
        }

        private void UpdateGraphics() {
            UpdatePath();
        }

        private void UpdatePath() {
            var path = game.Hero.Path;
            var existLinks = path.Links;
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
                        var coord = GameUtils.ConvertToGameCoord(link.FromX, link.FromY, level);
                        var pathGameObject = pathGoFactory.CreatePathSegment(link, coord);
                        pathObjects[link] = pathGameObject;
                    }
                }
            }
        } 

        private void UpdateCameraScale() {
            float screenX = Screen.width;
            float screenY = Screen.height;
            float screenScale = screenX / screenY;
  
            float levelX = level.SizeX;
            float levelY = level.SizeY;  

            float xScale = screenX/levelX;
            float yScale = screenY/levelY;

			float scale = xScale >= yScale ? levelY : levelX/screenScale;
            Camera.main.orthographicSize = scale / 2f;
        }
    
        private void ShowWinWindow() {      
            winMenu.Show();
        }
    }
}