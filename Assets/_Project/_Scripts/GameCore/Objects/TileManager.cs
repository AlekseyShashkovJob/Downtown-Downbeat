using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Objects
{
    public class TileManager : MonoBehaviour
    {
        [SerializeField] private ObjectPool _tilePool;

        [SerializeField] private RectTransform _rowsRoot;

        [SerializeField] private RectTransform _containerRect;

        [SerializeField] private Canvas _canvas;

        private const float BaseTileSpeed = 1000f;
        private const int TilesPerRow = 3;
        private const int TilesPerColumn = 5;

        private float _speed;

        private float _tileWidth;
        private float _tileHeight;

        private float _nextRowY;

        private readonly Queue<List<Tile>> _rows = new();

        private void Start()
        {
            _tileWidth = _containerRect.rect.width / TilesPerRow;
            _tileHeight = _containerRect.rect.height / TilesPerColumn;

            SnapSizesToPixelGrid();
        }

        public void OnGameStart()
        {
            ClearAllRows();

            float difficultyMultiplier = GameManager.Instance.GetSpawnIntervalMultiplier();
            _speed = BaseTileSpeed * difficultyMultiplier;

            _rowsRoot.anchoredPosition = Vector2.zero;

            float topYInContainer = _containerRect.rect.height / 2f + _tileHeight / 2f;

            _nextRowY = topYInContainer;

            int initialRows = TilesPerColumn + 2;
            for (int i = 0; i < initialRows; i++)
            {
                SpawnRowAt(_nextRowY);
                _nextRowY += _tileHeight;
            }
        }

        private void Update()
        {
            if (!GameManager.Instance.IsGameActive) return;

            var p = _rowsRoot.anchoredPosition;
            p.y -= _speed * Time.deltaTime;

            p.y = SnapToPixel(p.y);
            _rowsRoot.anchoredPosition = p;

            float topVisibleY = _containerRect.rect.height / 2f;

            while (_nextRowY + _rowsRoot.anchoredPosition.y < topVisibleY + _tileHeight)
            {
                SpawnRowAt(_nextRowY);
                _nextRowY += _tileHeight;
            }

            float bottomVisibleY = -_containerRect.rect.height / 2f - _tileHeight;

            while (_rows.Count > 0)
            {
                float lowestRowY = _nextRowY - _rows.Count * _tileHeight;

                if (lowestRowY + _rowsRoot.anchoredPosition.y >= bottomVisibleY)
                    break;

                var row = _rows.Dequeue();
                for (int i = 0; i < row.Count; i++)
                    row[i].ReturnToPool();
            }
        }

        private void SpawnRowAt(float y)
        {
            var row = new List<Tile>(TilesPerRow);

            float leftX = -_containerRect.rect.width / 2f + _tileWidth / 2f;

            for (int i = 0; i < TilesPerRow; i++)
            {
                GameObject tileObj = _tilePool.GetObject(_rowsRoot);
                RectTransform rt = tileObj.GetComponent<RectTransform>();
                rt.SetParent(_rowsRoot, false);

                rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
                rt.sizeDelta = new Vector2(_tileWidth, _tileHeight);

                float x = leftX + i * _tileWidth;

                rt.anchoredPosition = new Vector2(SnapToPixel(x), SnapToPixel(y));

                var tile = tileObj.GetComponent<Tile>();
                tile.Initialize(GetRandomTileType(), _tilePool);
                row.Add(tile);
            }

            _rows.Enqueue(row);
        }

        private void ClearAllRows()
        {
            while (_rows.Count > 0)
            {
                var row = _rows.Dequeue();
                for (int i = 0; i < row.Count; i++)
                    row[i].ReturnToPool();
            }
        }

        private void SnapSizesToPixelGrid()
        {
            _tileWidth = SnapToPixel(_tileWidth);
            _tileHeight = SnapToPixel(_tileHeight);
        }

        private float SnapToPixel(float v)
        {
            float sf = (_canvas != null && _canvas.isRootCanvas) ? _canvas.scaleFactor : 1f;
            if (sf <= 0f) sf = 1f;
            return Mathf.Round(v * sf) / sf;
        }

        private TileType GetRandomTileType()
        {
            int rand = Random.Range(0, 7);

            return rand switch
            {
                0 => TileType.Destroyed1,
                1 => TileType.Destroyed2,
                2 => TileType.Destroyed3,
                3 => TileType.Red,
                4 => TileType.Empty1,
                5 => TileType.Empty2,
                6 => TileType.Empty3,
                _ => TileType.Empty1
            };
        }
    }
}