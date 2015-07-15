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
using TrappedGame.Model.LevelUtils;
using UnityEngine;

namespace TrappedGame.Main {
    public class GameEntry : MonoBehaviour, ISyncGameObject {
        
        public CellGOFactory cellGameObjectFactory;
        public ElementsGOFactory elementsGameObjectFactory;

        public GameObject border;

        private Game game;
        private Level level;
        private Levels.LevelInfo levelInfo;
        private HeroInput heroInput;
        private InputQueue inputQueue = new InputQueue();

        public LevelUIController uiController;

        public AudioClip next;
        public AudioClip back;
        public AudioClip death;
        public AudioClip wrongTurn;

        private HeroController heroController;

        private readonly List<ISyncGameObject> syncGameObjects = new List<ISyncGameObject>();

        void Start() {
            levelInfo = getLevelInfo();
            var loader = new JsonLevelLoader();
            level = loader.LoadLevel(levelInfo.Path);
            level.AddNextTickAction(() => AudioSource.PlayClipAtPoint(next, Vector3.zero));
            level.AddBackTickAction(() => AudioSource.PlayClipAtPoint(back, Vector3.zero));
            game = new Game(level);
            game.AddWrongTurnAAction(() => AudioSource.PlayClipAtPoint(wrongTurn, Vector3.zero));
            game.Hero.AddDeathAction(() => AudioSource.PlayClipAtPoint(death, Vector3.zero));
            uiController.Game = game;

            heroInput = CreateInput();
            CreateLevelObjects();
            startLevelProgres();
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

            heroController = elementsGameObjectFactory.CreateHero(game);
            syncGameObjects.Add(heroController);

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
            } else if (IsSync()) {
                saveLevelProgres(); // TODO It's can be called only once
                uiController.ShowWinMenu();
            }
        }

        private void startLevelProgres() {
            levelInfo.State = Levels.LevelInfo.LEVEL_STATE.STARTED;
        }

        private void saveLevelProgres() {
            levelInfo.State = Levels.LevelInfo.LEVEL_STATE.FINISHED;
        }

        private Levels.LevelInfo getLevelInfo() {
            var levelName = PlayerPrefs.GetString(Preferences.CURRENT_LEVEL);
           return Levels.GetLevelByName(levelName);
        }

        private void UpdateInput() {
            var heroMovement = heroInput.GetMovement();
            inputQueue.AddMovement(heroMovement);

            if (IsSync()) {
                var movement = inputQueue.GetNextMovement();
                movement.MoveHeroInGame(game);
            }
        }

    }
}