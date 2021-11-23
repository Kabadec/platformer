using System;
using System.Collections.Generic;
using System.Linq;
using PixelCrew.Model.Definitions.Player;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class LevelData
    {
        [SerializeField] private List<LevelProgress> _progress;

        public int GetLevel(StatId id)
        {
            foreach (var levelProgress in _progress)
            {
                if (levelProgress.Id == id)
                    return levelProgress.Level;
            }

            return 0;
            // var progress = _progress.FirstOrDefault(x => x.Id == id);
            // return progress?.Level ?? 0;
        }

        public void LevelUp(StatId id)
        {
            var progress = _progress.FirstOrDefault(x => x.Id == id);
            if (progress == null)
                _progress.Add(new LevelProgress(id, 1));
            else
                progress.Level++;
        }
    }

    [Serializable]
    public class LevelProgress
    {
        public StatId Id;
        public int Level;

        public LevelProgress(StatId id, int level)
        {
            Id = id;
            Level = level;
        }
    }
}