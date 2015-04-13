using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TrappedGame.Control.Hero;
using TrappedGame.Model;
using TrappedGame.Model.LevelLoader.Json;
using TrappedGame.Utils;
using TrappedGame.View.Controllers;
using TrappedGame.View.GUI;
using TrappedGame.View.Graphic;
using TrappedGame.View.Sync;
using UnityEngine;

namespace TrappedGame.Main {
    public class GameEntry : MonoBehaviour, ISyncGameObject {
        
        public PathGOFactory pathGoFactory;
        public CellGOFactory cellGameObjectFactory;
        public ElementsGOFactory elementsGameObjectFactory;

        public GameObject border;

        private Game game;
        private Level level;
        private HeroInput heroInput;
        private InputQueue inputQueue = new InputQueue();

        public LevelUIController uiController;

        public AudioClip next;
        public AudioClip back;
        public AudioClip death;
        public AudioClip wrongTurn;

        private readonly List<ISyncGameObject> syncGameObjects = new List<ISyncGameObject>();
        private readonly IList<PathLinkController> pathLinks = new List<PathLinkController>();

        private readonly IDictionary<Path.PathLink, IList<GameObject>> pathObjects
                = new Dictionary<Path.PathLink, IList<GameObject>>();

        void Start() {   
            var levelName = PlayerPrefs.GetString(Preferences.CURRENT_LEVEL);
            var loader = new JsonLevelLoader();
            level = loader.LoadLevel(levelName);
            level.AddNextTickAction(() => AudioSource.PlayClipAtPoint(next, Vector3.zero));
            level.AddBackTickAction(() => AudioSource.PlayClipAtPoint(back, Vector3.zero));
            game = new Game(level);
            game.AddWrongTurnAAction(() => AudioSource.PlayClipAtPoint(wrongTurn, Vector3.zero));
            game.Hero.AddDeathAction(() => AudioSource.PlayClipAtPoint(death, Vector3.zero));
            uiController.Game = game;

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
            elementsGameObjectFactory.CreateBorder(level);
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
            } else if (IsSync()) {
                uiController.ShowWinMenu();
            }
        }

        private void UpdateInput() {
            var heroMovement = heroInput.GetMovement();
            inputQueue.AddMovement(heroMovement);

            if (IsSync()) {
                var movement = inputQueue.GetNextMovement();
                movement.MoveHeroInGame(game);
            }
        }

        private void UpdateGraphics() {
            UpdatePath();
        }

        private void UpdatePath() {
            var path = game.Hero.Path;
            var existLinks = path.Links;
            var showedLinks = pathLinks.Select(item => item.PathLink);
            var difference = new HashSet<Path.PathLink>(existLinks);
            difference.SymmetricExceptWith(showedLinks);

            foreach (Path.PathLink link in difference) {
                if (!showedLinks.Contains(link)) {
                    if (link.IsAdjacent()) {
                        var pathLinkController = pathGoFactory.CreateLink(link, level, game.Hero);
                        pathLinks.Add(pathLinkController);
                    }
                }
            }
        }

    }
}