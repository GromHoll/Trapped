using System.Collections.Generic;
using TrappedGame.Model;
using TrappedGame.Utils;
using TrappedGame.View.Controllers;
using TrappedGame.View.Sync;
using UnityEditor;
using UnityEngine;

namespace TrappedGame.View.Graphic {
    public class ElementsGOFactory : MonoBehaviour {

        public GameObject keyPrefab;
        public GameObject heroPrefab;
        public GameObject bonusPrefab;
        public GameObject borderPrefab;
        public GameObject finishPrefab;
        public GameObject platformPrefab;
        public GameObject timeBonusPrefab;

        public IList<ISyncGameObject> CreateGameElements(Game game) {
            var syncGameObjects = new List<ISyncGameObject>();

            var hero = CreateHero(game);
            syncGameObjects.Add(hero);

            var platforms = CreatePlatforms(game.Level);
            syncGameObjects.AddRange(platforms);

            CreateFinish(game.Level);
            CreateBonuses(game.Level);
            CreateTimeBonuses(game.Level);
            CreateKeys(game.Level);

            return syncGameObjects;
        }

        private ISyncGameObject CreateHero(Game game) {
            var level = game.Level;
            var position = GameUtils.ConvertToGameCoord(level.StartX, level.StartY, level);
            var hero = GameObjectUtils.InstantiateChild(heroPrefab, position, gameObject);
            var heroController = hero.GetComponent<HeroController>();
            heroController.Game = game;
            return heroController;
        }

        private void CreateFinish(Level level) {
            var position = GameUtils.ConvertToGameCoord(level.FinishX, level.FinishY, level);
            GameObjectUtils.InstantiateChild(finishPrefab, position, gameObject);  
        }

        private void CreateBonuses(Level level) {
            foreach (var coord in level.Bonuses) {
                var position = GameUtils.ConvertToGameCoord(coord, level);
                GameObjectUtils.InstantiateChild(bonusPrefab, position, gameObject); 
            }
        }

        private void CreateTimeBonuses(Level level) {
            foreach (var coord in level.TimeBonuses.Keys) {
                var position = GameUtils.ConvertToGameCoord(coord, level);
                GameObjectUtils.InstantiateChild(timeBonusPrefab, position, gameObject);
            }
        }

        private void CreateKeys(Level level) {
            foreach (var key in level.Keys) {
                var position = GameUtils.ConvertToGameCoord(key.Coordinate, level);
                GameObjectUtils.InstantiateChild(keyPrefab, position, gameObject);
            }
        }

        private IList<ISyncGameObject> CreatePlatforms(Level level) {
            var controllers = new List<ISyncGameObject>();
            foreach (var platform in level.Platforms) {
                var position = GameUtils.ConvertToGameCoord(platform.Coordinate, level);
                var platformGameObject = GameObjectUtils.InstantiateChild(platformPrefab, position, gameObject);
                var controller = platformGameObject.GetComponent<PlatformController>();
                controller.SerPlatform(platform, level);
                controllers.Add(controller);
            }
            return controllers;
        }

        public void CreateBorder(Level level) {
            var folder = GameObjectUtils.GetSubFolderByPath(gameObject, "Borders");

            var rightBorder = GameObjectUtils.InstantiateChild(borderPrefab, new Vector2((level.SizeX + 1)/2f, 0), folder);
            var rightBorderScale = rightBorder.transform.localScale;
            rightBorderScale.y = level.SizeY + 1;
            rightBorder.transform.localScale = rightBorderScale;

            var leftBorder = GameObjectUtils.InstantiateChild(borderPrefab, new Vector2(-(level.SizeX + 1)/2f, 0), folder);
            var leftBorderScale = leftBorder.transform.localScale;
            leftBorderScale.y = level.SizeY + 1;
            leftBorder.transform.localScale = leftBorderScale;

            var upBorder = GameObjectUtils.InstantiateChild(borderPrefab, new Vector2(0, (level.SizeY + 1)/2f), folder);
            var upBorderScale = upBorder.transform.localScale;
            upBorderScale.y = level.SizeX + 1;
            upBorder.transform.localScale = upBorderScale;
            upBorder.transform.Rotate(0, 0, 90);

            var downBorder = GameObjectUtils.InstantiateChild(borderPrefab, new Vector2(0, -(level.SizeY + 1)/2f), folder);
            var downBorderScale = downBorder.transform.localScale;
            downBorderScale.y = level.SizeX + 1;
            downBorder.transform.localScale = downBorderScale;
            downBorder.transform.Rotate(0, 0, 90);
        }

    }
}
