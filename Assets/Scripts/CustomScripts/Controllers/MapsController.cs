using System.Collections.Generic;
using System.Linq;
using Models;
using Naninovel;
using Naninovel.UI;
using UnityEngine;
using Views;

namespace Controllers
{
    public class MapsController : CustomUI
    {
        [SerializeField] private MapsVo[] mapId;
        
        private Dictionary<int, MapItemView> _mapItemViews;
        
        private void Start()
        {
            _mapItemViews = mapId.ToDictionary(id => id.MapId, id => id.MapItemView);

            foreach (var mapItem in _mapItemViews)
                mapItem.Value.EnableMap(mapItem.Key == 1);
        }
        
        public void SetActiveMapByID()
        {
            var valueManager = Engine.GetService<ICustomVariableManager>();
            if (valueManager.TryGetVariableValue<int>("MapID", out var intValue))
            {
                foreach (var mapItem in _mapItemViews)
                    mapItem.Value.EnableMap(mapItem.Key == intValue);
            }
            else
                Debug.LogWarning("MapID variable not found.");
        }

        public void SetLastMapActiveByID()
        {
            foreach (var mapItem in _mapItemViews)
                mapItem.Value.EnableLastMap(mapItem.Key != 2);
        }
    }
}