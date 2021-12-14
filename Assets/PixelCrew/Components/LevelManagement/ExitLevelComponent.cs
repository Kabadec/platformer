using PixelCrew.Model;
using UnityEngine;
using PixelCrew.Utils;

namespace PixelCrew.Components.LevelManagement
{
    public class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;
        [SerializeField] private string _firstCheckpointOnNewLevel;
        public void Exit()
        {
            var session = GameSession.Instance;
            //session.SetDefaultData(session.Data);
            session.SetChecked(_firstCheckpointOnNewLevel);

            var loader = MainGOsUtils.GetLevelLoader();
            loader.LoadLevel(_sceneName);
        }
    }
}