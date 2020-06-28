using System.Reflection;
using Modding;
using UnityEngine;
using ModCommon.Util;
using System.Collections;

namespace BenchWarpFix
{
    public class BenchWarpFix : Mod
    {
        internal static BenchWarpFix Instance;

        public override string GetVersion() => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public override void Initialize()
        {
            Instance = this;

            GameManager.instance.StartCoroutine(fixMissingBench());
        }

        private IEnumerator fixMissingBench()
        {
            while (true)
            {
                yield return new WaitWhile(() => !HeroController.instance);

                Vector3 hero_pos = HeroController.instance.transform.position;
                while ((hero_pos.x >= -100.0f) && (hero_pos.y >= -100.0f))
                {
                    hero_pos = HeroController.instance.transform.position;
                    yield return null;
                }

                GameObject failsafe = new GameObject("FAILSAFE");
                failsafe.transform.SetPosition2D(hero_pos);
                var tp = failsafe.AddComponent<TransitionPoint>();

                var bc = failsafe.AddComponent<BoxCollider2D>();
                bc.size = new Vector2(10f, 10f);
                bc.isTrigger = true;
                tp.SetTargetScene("Town");
                tp.entryPoint = "top1";

                GameObject rm = new GameObject("Hazard Respawn Marker");
                rm.transform.parent = failsafe.transform;
                rm.transform.SetPosition2D(new Vector2(0f, 0f));
                var tmp = rm.AddComponent<HazardRespawnMarker>();
                tmp.respawnFacingRight = false;
                tp.respawnMarker = rm.GetComponent<HazardRespawnMarker>();
                tp.sceneLoadVisualization = GameManager.SceneLoadVisualizations.Default;
            }
        }
    }
}
