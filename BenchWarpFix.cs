using System.Collections;
using System.Reflection;
using Modding;
using UnityEngine;

namespace BenchWarpFix
{
    public class BenchWarpFix : Mod
    {
        internal static BenchWarpFix Instance;

        public override string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

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

                var hero_pos = HeroController.instance.transform.position;
                while (hero_pos.x >= -100.0f && hero_pos.y >= -100.0f)
                {
                    hero_pos = HeroController.instance.transform.position;
                    yield return null;
                }

                var failsafe = new GameObject("FAILSAFE");
                failsafe.transform.SetPosition2D(hero_pos);
                var tp = failsafe.AddComponent<TransitionPoint>();

                var bc = failsafe.AddComponent<BoxCollider2D>();
                bc.size = new Vector2(10f, 10f);
                bc.isTrigger = true;
                tp.SetTargetScene("Town");
                tp.entryPoint = "top1";

                var rm = new GameObject("Hazard Respawn Marker");
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