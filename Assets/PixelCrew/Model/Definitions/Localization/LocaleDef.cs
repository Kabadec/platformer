using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace PixelCrew.Model.Definitions.Localization
{
    [CreateAssetMenu(menuName = "Defs/LocaleDef", fileName = "LocaleDef")]
    public class LocaleDef: ScriptableObject
    {
        // en - https://docs.google.com/spreadsheets/d/e/2PACX-1vS4fHadUWXo3aGt6ISxHL8gb076M6g3hGawByW7rjSsXuU23q3JvCKf1_5Lv89QGxPjlOPqWwbkH_CC/pub?gid=0&single=true&output=tsv
        // ru - https://docs.google.com/spreadsheets/d/e/2PACX-1vS4fHadUWXo3aGt6ISxHL8gb076M6g3hGawByW7rjSsXuU23q3JvCKf1_5Lv89QGxPjlOPqWwbkH_CC/pub?gid=1114976474&single=true&output=tsv
        // ua - https://docs.google.com/spreadsheets/d/e/2PACX-1vS4fHadUWXo3aGt6ISxHL8gb076M6g3hGawByW7rjSsXuU23q3JvCKf1_5Lv89QGxPjlOPqWwbkH_CC/pub?gid=1220832505&single=true&output=tsv

        [SerializeField] private string _url;
        [SerializeField] private List<LocaleItem> _localeItems;

        private UnityWebRequest _request;

        public Dictionary<string, string> GetData()
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var localeItem in _localeItems)
            {
                dictionary.Add(localeItem.Key, localeItem.Value);
            }

            return dictionary;
        }

        [ContextMenu("Update locale")]
        public void LoadLocale()
        {
            if(_request != null) return;

            _request = UnityWebRequest.Get(_url);
            _request.SendWebRequest().completed += OnDataLoaded;
        }

        private void OnDataLoaded(AsyncOperation operation)
        {
            if (operation.isDone)
            {
                var rows = _request.downloadHandler.text.Split('\n');
                _localeItems.Clear();
                foreach (var row in rows)
                {
                    AddLocaleItem(row);
                }
            }
        }

        private void AddLocaleItem(string row)
        {
            try
            {
                var parts = row.Split('\t');
                _localeItems.Add(new LocaleItem{Key = parts[0], Value = parts[1]});
            }
            catch(Exception e)
            {
                Debug.LogError($"Can't parse row: {row}.\n {e}");
            }
        }

        private void OnDestroy()
        {
            _request.SendWebRequest().completed -= OnDataLoaded;
        }

        [Serializable]
        private class LocaleItem
        {
            public string Key;
            public string Value;
        }
    }
}