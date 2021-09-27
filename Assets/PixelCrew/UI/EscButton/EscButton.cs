using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrew.Creatures;

public class EscButton : MonoBehaviour
{
    private GameObject _gameMenuWindow;
    //private Image _image;
    //[SerializeField] private GameObject _creatures;
    void Awake()
    {
        //_image = GetComponent<Image>();
    }
    public void OnShowGameMenu()
    {
        if (_gameMenuWindow == null)
        {
            var window = Resources.Load<GameObject>("UI/GameMenuWindow");
            var canvas = FindObjectOfType<Canvas>();
            _gameMenuWindow = Instantiate(window, canvas.transform);
            //Time.timeScale = 0.1f;
            //_creatures.SetActive(false);
            var parentCreatures = GameObject.FindWithTag("CREATURES");
            var creatures = parentCreatures.GetComponentsInChildren<Creature>(true);
            foreach (var creature in creatures)
            {
                creature.gameObject.SetActive(false);
            }

        }
        else
        {
            //_gameMenuWindow.
        }
    }
}
