using UnityEngine;
using PixelCrew.Utils;

namespace PixelCrew.Components.LevelManagement
{
    public class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;
        public void Exit()
        {
            var session = MainGOsUtils.GetGameSession();
            session.SetDefaultData(session.Data);

            var loader = MainGOsUtils.GetLevelLoader();
            loader.LoadLevel(_sceneName);
        }
    }
}