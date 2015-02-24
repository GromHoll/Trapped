﻿using TrappedGame.Model.LevelUtils;
using TrappedGame.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TrappedGame.View.GUI {
    public class PacksMenu : MonoBehaviour {

        public Button buttonPrefab;
        public LevelsMenu levelsMenuPrefab;

        void Start() {
            var packs = Levels.Instance.Packs;
            foreach (var packInfo in packs) {
                CreatePack(packInfo);
            }
        }

        private void CreatePack(Levels.PackInfo pack) {
            var levelsMenuGO = GameObjectUtils.InstantiateChildForWorld(levelsMenuPrefab.gameObject, Vector2.zero, 
                                                                        gameObject.transform.parent.gameObject, false);
            levelsMenuGO.SetActive(false);
            levelsMenuGO.name = "LevelsMenu - " + pack.Name;
            var levelsMenu = levelsMenuGO.GetComponent<LevelsMenu>();
            levelsMenu.PackInfo = pack;
            levelsMenu.PacksMenu = this;

            var buttonGO = GameObjectUtils.InstantiateChild(buttonPrefab.gameObject, Vector2.zero, gameObject);
            var button = buttonGO.GetComponent<Button>();
            button.onClick.AddListener(() => {
                gameObject.SetActive(false);
                levelsMenuGO.SetActive(true);
            });
            button.GetComponentInChildren<Text>().text = pack.Name;
        }

	}
}
