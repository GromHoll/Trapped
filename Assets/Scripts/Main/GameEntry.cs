using System.Collections.Generic;
using System.Linq;
using TrappedGame.Control.Hero;
using TrappedGame.Model;
using TrappedGame.Model.LevelLoader.Json;
using TrappedGame.View.Graphic;
using TrappedGame.View.GUI;
using TrappedGame.View.Sync;
using UnityEngine;

namespace TrappedGame.Main {
    public class GameEntry : MonoBehaviour, ISyncGameObject {
        
        public PathGOFactory pathGoFactory;
        public CellGOFactory cellGameObjectFactory;
        public ElementsGOFactory elementsGameObjectFactory;
        
        private Game game;
        private Level level;
        private HeroInput heroInput;

        public LevelUIController uiController;

        private readonly List<ISyncGameObject> syncGameObjects = new List<ISyncGameObject>();
        private readonly IDictionary<Path.PathLink, IList<GameObject>> pathObjects
                = new Dictionary<Path.PathLink, IList<GameObject>>();

        void Start() {   
            var levelName = PlayerPrefs.GetString(Preferences.CURRENT_LEVEL);
            var loader = new JsonLevelLoader();
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
            syncGameObjects.AddRange(cellGameObjectFactory.CreateLevel(level));
            syncGameObjects.AddRange(elementsGameObjectFactory.CreateGameElements(game));
            
            pathGoFactory.CreatePathStart(level);

            uiController.ShowTutorial(level.LevelTutorial);
            syncGameObjects.Add(uiController.tutorialMenu);
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
            foreach (Path.PathLink link in differens) {
                if (showedLinks.Contains(link)) {
                    var linkGameObjects = pathObjects[link];
                    pathObjects.Remove(link);
                    foreach (GameObject go in linkGameObjects) {
                        Destroy(go);
                    }
                } else {
                    if (link.IsAdjacent()) {
                        var pathGameObjects = pathGoFactory.CreatePathSegment(link, level);
                        pathObjects[link] = pathGameObjects;
                    }
                }
            }
        }
    
        private void ShowWinWindow() {      
            uiController.ShowWinMenu(game);
        }
    }
}