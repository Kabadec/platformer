using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace PixelCrew.UI.Widgets
{

    public class ButtonSound : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private AudioClip _audioClip;
        private AudioSource _source;
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_source == null)
                _source = GameObject.FindWithTag("SfxAudioSource").GetComponent<AudioSource>();

            _source.PlayOneShot(_audioClip);

        }
    }
}