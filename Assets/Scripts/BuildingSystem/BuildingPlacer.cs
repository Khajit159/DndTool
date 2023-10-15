using GameInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingSystem
{
    public class BuildingPlacer : MonoBehaviour
    {
        [field: SerializeField]
        public BuildableItem ActiveBuildable { get; private set; }

        [SerializeField]
        private ConstructionLayer _constructionLayer;
        [SerializeField]
        private PreviewLayer _previewLayer;

        [SerializeField]
        private MouseUser _mouseUser;

        private void Update()
        {
            if (ActiveBuildable == null) { _previewLayer.ClearPreview(); return; }
            if (_constructionLayer == null) return;

            var mousePos = _mouseUser.MouseInWorldPosition;
            _previewLayer.ShowPreview(ActiveBuildable, mousePos);

            if (Input.GetMouseButtonUp(1) && !Input.GetKey(KeyCode.LeftControl))
            {
                _constructionLayer.Destroy(mousePos, false);
            }

            if (Input.GetMouseButton(1) && Input.GetKey(KeyCode.LeftControl))
            {
                _constructionLayer.Destroy(mousePos, true);
            }



            if (Input.GetMouseButton(0) && ActiveBuildable.Tile != null)
            {
                _constructionLayer.Build(mousePos, ActiveBuildable);
            }

            if (Input.GetMouseButtonUp(0) && ActiveBuildable.GameObject != null && _constructionLayer.IsEmpty(mousePos))
            {
                _constructionLayer.Build(mousePos, ActiveBuildable);
            }

            if (ActiveBuildable != null && Input.GetKeyDown(KeyCode.Escape)) { ActiveBuildable = null; }
        }

        public void SetActiveBuildable(BuildableItem item, string EntityName = null)
        {
            ActiveBuildable = item;
            _constructionLayer.SelectedEntityName = EntityName;
        }
    }
}
