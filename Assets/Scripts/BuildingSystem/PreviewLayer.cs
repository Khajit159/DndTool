using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingSystem;

namespace BuildingSystem
{
    public class PreviewLayer : TilemapLayer
    {
        [SerializeField]
        private SpriteRenderer _previewRenderer;

        public void ShowPreview(BuildableItem item, Vector3 worldCoords)
        {
            if (item.GameObject != null) { _previewRenderer.transform.localScale = item.GameObject.transform.localScale; }
            else { _previewRenderer.transform.localScale = Vector3.one; }
            var coords = _tilemap.WorldToCell(worldCoords);
            _previewRenderer.enabled = true;
            _previewRenderer.transform.position = _tilemap.CellToWorld(coords) + _tilemap.cellSize / 2;
            _previewRenderer.sprite = item.PreviewSprite;
            _previewRenderer.color = new Color(1, 1, 1, 0.5f);

        }

        public void ClearPreview()
        {
            _previewRenderer.enabled = false;
        }
    }
}
