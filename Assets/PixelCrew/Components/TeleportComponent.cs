using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Components
{
    public class TeleportComponent : MonoBehaviour
    {
        [SerializeField] private Transform _destTransform;
        [SerializeField] private float _alphaTime = 1;
        [SerializeField] private float _moveTime = 1;


        public void Teleport(GameObject target)
        {
            //target.transform.position = _destTransform.position;
            StartCoroutine(AnimateTeleport(target));
        }

        private IEnumerator AnimateTeleport(GameObject target)
        {
            var sprite = target.GetComponent<SpriteRenderer>();
            var input = target.GetComponent<PlayerInput>();

            SetLockInput(input, true);

            yield return AlphaAnimation(sprite, 0);
            target.SetActive(false);

            yield return MoveAnimation(target);
            // var hitParticlesObject = target.transform.Find("ParticleSystem").gameObject;
            // var hitParticles = hitParticlesObject.GetComponent<ParticleSystem>();
            // // if(hitParticles != null)
            // // {
            // //     Debug.Log("zaebis");
            // // }
            //  var burst1 = hitParticles.emission.GetBurst(0);
            //  burst1.count = 0;
            //  hitParticles.emission.SetBurst(0, burst1);
            target.SetActive(true);
            yield return AlphaAnimation(sprite, 1);

            SetLockInput(input, false);

        }

        private void SetLockInput(PlayerInput input, bool isLocked)
        {
            if (input != null)
            {
                input.enabled = !isLocked;
            }
        }

        private IEnumerator MoveAnimation(GameObject target)
        {
            var moveTime = 0f;
            while (moveTime < _moveTime)
            {
                moveTime += Time.deltaTime;
                var progress = moveTime / _moveTime;
                target.transform.position = Vector3.Lerp(target.transform.position, _destTransform.position, progress);

                yield return null;
            }
        }
        private IEnumerator AlphaAnimation(SpriteRenderer sprite, float destAlpha)
        {
            var time = 0f;
            var spriteAlpha = sprite.color.a;
            while (time < _alphaTime)
            {
                time += Time.deltaTime;
                var progress = time / _alphaTime;
                var timeAlpha = Mathf.Lerp(spriteAlpha, destAlpha, progress);
                var color = sprite.color;
                color.a = timeAlpha;
                sprite.color = color;

                yield return null;
            }
        }
    }
}