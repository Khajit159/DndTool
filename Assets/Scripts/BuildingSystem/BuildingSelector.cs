using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

namespace BuildingSystem
{
    public class BuildingSelector : MonoBehaviour
    {
        [SerializeField]
        private List<BuildableItem> _buildables;

        [SerializeField]
        private BuildingPlacer _buildingPlacer;

        [SerializeField]
        private ConstructionLayer _constructionLayer;

        public void SetEntityToBuild(string EntityName)
        {
            _buildingPlacer.SetActiveBuildable(_buildables.Where(obj => obj.name == "Entity").SingleOrDefault());
            _constructionLayer.SelectedEntityName = EntityName;
        }
        public void SetItemToBuild()
        {
            string ClickedButtonName = EventSystem.current.currentSelectedGameObject.name;
            _buildingPlacer.SetActiveBuildable(_buildables.Where(obj => obj.name == ClickedButtonName).SingleOrDefault());
            Debug.Log(ClickedButtonName);
            _constructionLayer.SelectedEntityName = null;
        }
    }
}
