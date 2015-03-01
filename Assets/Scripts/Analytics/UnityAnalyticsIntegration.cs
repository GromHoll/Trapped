using UnityEngine;
using System.Collections;
using UnityEngine.Cloud.Analytics;

namespace TrappedGame.Analytics {
    public class UnityAnalyticsIntegration : MonoBehaviour {
        void Start() {
            const string projectId = "b28ae908-63af-4e77-b004-87c831a62007";
            UnityAnalytics.StartSDK(projectId);
        }
    }
}