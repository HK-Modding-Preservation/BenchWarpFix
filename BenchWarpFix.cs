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

            GameManager.instance.StartCoroutine(FixMissingBench());
        }

        private static IEnumerator FixMissingBench()
        {
            while (true)
            {
                yield return new WaitWhile(() => !HeroController.instance);
                var hero = HeroController.instance;
                var heroPos = hero.transform.position;
                while (heroPos.x >= -100.0f && heroPos.y >= -100.0f)
                {
                    heroPos = hero.transform.position;
                    yield return null;
                }

                var failSafe = new GameObject("FAILSAFE");
                failSafe.transform.SetPosition2D(heroPos);
                var tp = failSafe.AddComponent<TransitionPoint>();

                var bc = failSafe.AddComponent<BoxCollider2D>();
                bc.size = new Vector2(10f, 10f);
                bc.isTrigger = true;
                tp.SetTargetScene("Town");
                tp.entryPoint = "top1";

                var rm = new GameObject("Hazard Respawn Marker");
                rm.transform.parent = failSafe.transform;
                rm.transform.SetPosition2D(new Vector2(0f, 0f));
                var tmp = rm.AddComponent<HazardRespawnMarker>();
                tmp.respawnFacingRight = false;
                tp.respawnMarker = rm.GetComponent<HazardRespawnMarker>();
                tp.sceneLoadVisualization = GameManager.SceneLoadVisualizations.Default;
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}