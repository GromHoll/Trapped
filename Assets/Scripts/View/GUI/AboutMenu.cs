using SimpleJSON;
using System.Globalization;
using TrappedGame.Model;
using UnityEngine;
using UnityEngine.UI;

namespace TrappedGame.View.GUI {
    public class AboutMenu : MonoBehaviour {

        public void Start() {
            var manifestAsset = (TextAsset) Resources.Load("UnityCloudBuildManifest.json");
            if (manifestAsset != null) {
                var manifest = JSON.Parse(manifestAsset.text);
                var commitId = manifest["scmCommitId"];
                var buildNumber = manifest["buildNumber"];
                var buildStartTime = manifest["buildStartTime"];

                var text = GetComponent<Text>();
                text.text = "Build #" + buildNumber + "\n" +
                            "Date : " + buildStartTime + "\n" +
                            "Commit : " + commitId;
            }
        }

    }
}