using System;
using System.Collections;
using UnityEngine;

namespace CasualGame
{
    [RequireComponent(typeof(CanvasGroup))]
    public class SceneNavigatorTransition : PersistentObjectSingleton<SceneNavigatorTransition>
    {
        private Canvas _C;
        private CanvasGroup _CG;

        protected override void Awake()
        {
            base.Awake();
            _C = GetComponent<Canvas>();
            _CG = GetComponent<CanvasGroup>();
        }

        public void Fade(float startAlpha, float endAlpha, float duration) =>
            StartCoroutine(FadeUI(startAlpha, endAlpha, duration));
        
        private IEnumerator FadeUI(float startAlpha, float endAlpha, float duration)
        {
            _C.enabled = true;
            _CG.alpha = startAlpha;
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                _CG.alpha = Mathf.Lerp(startAlpha, endAlpha, t / duration);
                yield return null;
            }
            _CG.alpha = endAlpha;
            _C.enabled = endAlpha > 0.0f;
        }

        public void Fade2Way(Vector2 firstFadeStartEnd, Vector2 secondFadeStartEnd, float duration, float delay, Action action) =>
            StartCoroutine(FadeUI2Way(firstFadeStartEnd, secondFadeStartEnd, duration, delay, action));
        
        private IEnumerator FadeUI2Way(Vector2 firstFadeStartEnd, Vector2 secondFadeStartEnd, float duration, float delay, Action action)
        {
            StartCoroutine(FadeUI(firstFadeStartEnd.x, firstFadeStartEnd.y, duration));
            yield return new WaitForSeconds(duration + (delay / 4));
            action();
            yield return new WaitForSeconds(delay - (delay / 4));
            StartCoroutine(FadeUI(secondFadeStartEnd.x, secondFadeStartEnd.y, duration));
        }
    }
}
