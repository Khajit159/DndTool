using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BuildingSystem
{
    public class ConstructionLayer : TilemapLayer
    {
        public string SelectedEntityName;
        public string Name;
        public bool IsIconCustom;
        public Texture Icon;
        public string IconName;
        public Color Color;
        public string Armour;
        private FileInfo[] Entity_DirFiles;
        public DirectoryInfo Entity_DirInfo;
        private string dirCustomIcon;
        GameObject itemObject = null;
        private Dictionary<Vector3Int, Buildable> _buildables = new();



        private void Start()
        {
            Entity_DirInfo = new DirectoryInfo(Application.persistentDataPath + "/CustomEntity");
            dirCustomIcon = (Application.persistentDataPath + "/CustomIcon");
        }

        public void Build(Vector3 worldCoords, BuildableItem item)
        {
            if (EventSystem.current.IsPointerOverGameObject()) { return; }
            var coords = _tilemap.WorldToCell(worldCoords);
            if (item.Tile != null)
            {
                _tilemap.SetTile(coords, item.Tile);
            }
            if (item.GameObject != null)
            {
                itemObject = Instantiate(item.GameObject, _tilemap.CellToWorld(coords) + _tilemap.cellSize / 2, Quaternion.identity);
                var buildable = new Buildable(item, coords, itemObject);
                _buildables.Add(coords, buildable);
                if (item.name == "Entity") { UpdateEntityRuntime(); }
            }
        }

        public void UpdatePos(Vector3 worldCoords, Vector3 newCoords)
        {
            var coords = _tilemap.WorldToCell(worldCoords);
            var ncoords = _tilemap.WorldToCell(newCoords);
            if (!_buildables.ContainsKey(coords)) return;
            var buildable = _buildables[coords];
            _buildables.Remove(coords);

            _buildables.Add(ncoords, buildable);

        }

        public void Destroy(Vector3 worldCoords, bool ShiftHolding)
        {
            if (!ShiftHolding)
            {
                var coords = _tilemap.WorldToCell(worldCoords);
                if (!_buildables.ContainsKey(coords)) return;
                var buildable = _buildables[coords];
                _buildables.Remove(coords);
                buildable.Destroy();
            }
            else
            {
                var coords = _tilemap.WorldToCell(worldCoords);
                _tilemap.SetTile(coords, null);
            }
        }

        public bool IsEmpty(Vector3 worldCoords)
        {
            var coords = _tilemap.WorldToCell(worldCoords);
            return !_buildables.ContainsKey(coords);
        }

        public void UpdateEntityRuntime()
        {
            Entity_DirFiles = Entity_DirInfo.GetFiles();

            foreach (FileInfo file in Entity_DirFiles)
            {
                if (file.Name == (SelectedEntityName + ".txt"))
                {
                    string RealName = file.Name.Split('.')[0];
                    string fileRead = File.ReadAllText(file.ToString());
                    JsonUtility.FromJsonOverwrite(fileRead, this);
                    itemObject.transform.Find("Name").GetComponent<TextMeshPro>().text = Name;
                    itemObject.gameObject.name = Name;
                    itemObject.transform.Find("Sprite").Find("Border").GetComponent<SpriteRenderer>().color = Color;
                    itemObject.transform.Find("Armour").Find("Value").GetComponent<TextMesh>().text = Armour;
                    if (Icon == null && IsIconCustom == false)
                    {
                        itemObject.transform.Find("Sprite").Find("Icon").gameObject.SetActive(false);
                    }
                    else
                    {
                        itemObject.transform.Find("Sprite").Find("Icon").gameObject.SetActive(true);
                        if (IsIconCustom == false)
                        {
                            itemObject.transform.Find("Sprite").Find("Icon").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("PortraitIcon/" + Icon.name);
                        }
                        else if (IsIconCustom == true)
                        {
                            Sprite DiskLoadedIcon = IMG2Sprite.LoadNewSprite((dirCustomIcon + "/" + IconName + ".png"), 512, SpriteMeshType.FullRect);
                            itemObject.transform.Find("Sprite").Find("Icon").GetComponent<SpriteRenderer>().sprite = DiskLoadedIcon;
                            itemObject.transform.Find("Sprite").Find("Icon").transform.localPosition = new Vector3(-0.25f, -0.25f, 0);
                            itemObject.transform.Find("Sprite").Find("Icon").GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
                        }

                    }

                    if (IsIconCustom == true)
                    {
                        itemObject.transform.Find("Sprite").Find("Icon").gameObject.SetActive(true);
                    }
                }

            }
        }

    }

    public static class IMG2Sprite
    {

        //Static class instead of _instance
        // Usage from any other script:
        // MySprite = IMG2Sprite.LoadNewSprite(FilePath, [PixelsPerUnit (optional)], [spriteType(optional)])

        public static Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f, SpriteMeshType spriteType = SpriteMeshType.Tight)
        {

            // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference

            Texture2D SpriteTexture = LoadTexture(FilePath);
            Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), SpriteTexture.height, 0, spriteType);

            return NewSprite;
        }

        public static Sprite ConvertTextureToSprite(Texture2D texture, float PixelsPerUnit = 100.0f, SpriteMeshType spriteType = SpriteMeshType.Tight)
        {
            // Converts a Texture2D to a sprite, assign this texture to a new sprite and return its reference

            Sprite NewSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), PixelsPerUnit, 0, spriteType);

            return NewSprite;
        }

        public static Texture2D LoadTexture(string FilePath)
        {

            // Load a PNG or JPG file from disk to a Texture2D
            // Returns null if load fails

            Texture2D Tex2D;
            byte[] FileData;

            if (File.Exists(FilePath))
            {
                FileData = File.ReadAllBytes(FilePath);
                Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
                if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                    return Tex2D;                 // If data = readable -> return texture
            }
            return null;                     // Return null if load failed
        }
    }
}
