using System;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PixelCrew.UI.Windows
{
    public class TestInventory : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        //[SerializeField] private Image _image;
        //[SerializeField] private Camera _camera;
        //private float _posX = 0;
        //private float _posY = 0;
       // private Vector3 _dist;

        //private Canvas _canvas;

        private void Start()
        {
            // var canvases = FindObjectsOfType<Canvas>();
            // foreach (var canvas in canvases)
            // {
            //     if (!canvas.CompareTag("MainCanvas")) continue;
            //     _canvas = canvas;
            //     return;
            // }
            //
            // _camera = MainGOsUtils.GetMainCamera();
        }

        public void OnDrag(PointerEventData eventData)
        {         
            // _camera = MainGOsUtils.GetMainCamera();
            // //_canvas.worldCamera.WorldToScreenPoint(transform.position);
            // _dist = _camera.WorldToScreenPoint(transform.position);
            // _posX = Mouse.current.position.x.ReadValue() - _dist.x;
            // _posY = Mouse.current.position.y.ReadValue() - _dist.y;
            // //Debug.Log(Mouse.current.position.x.ReadValue());
            // Debug.Log("OnDrag");

        }
        public void OnEndDrag(PointerEventData eventData)
        {
            // Debug.Log("OnEndDrag");
            // //Debug.Log(Mouse.current.position.x.ReadValue());
            // //Debug.Log(Mouse.current.position.y.ReadValue());
            // _camera = MainGOsUtils.GetMainCamera();
            //
            // //_canvas.
            // Vector3 curPos = new Vector3 (Mouse.current.position.x.ReadValue() - _posX, Mouse.current.position.y.ReadValue() - _posY, _dist.z);
            // Vector3 worldPos = _camera.ScreenToWorldPoint(curPos);
            // transform.position = worldPos;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("OnPointerEnter");

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("OnPointerExit");

        }
    }
}