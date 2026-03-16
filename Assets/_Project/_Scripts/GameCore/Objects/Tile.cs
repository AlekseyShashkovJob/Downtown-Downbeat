using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace GameCore.Objects
{
    public class Tile : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _destroyed1;
        [SerializeField] private Sprite _destroyed2;
        [SerializeField] private Sprite _destroyed3;
        [SerializeField] private Sprite _red;
        [SerializeField] private Sprite[] _emptySprites;

        private TileType _type;
        private ObjectPool _pool;

        public void Initialize(TileType type, ObjectPool pool)
        {
            _type = type;
            _pool = pool;

            switch (_type)
            {
                case TileType.Destroyed1:
                    _image.sprite = _destroyed1;
                    break;
                case TileType.Destroyed2:
                    _image.sprite = _destroyed2;
                    break;
                case TileType.Destroyed3:
                    _image.sprite = _destroyed3;
                    break;
                case TileType.Red:
                    _image.sprite = _red;
                    break;
                case TileType.Empty1:
                case TileType.Empty2:
                case TileType.Empty3:
                    _image.sprite = _emptySprites[(int)_type - (int)TileType.Empty1];
                    break;
            }
        }

        public void ReturnToPool()
        {
            _pool.ReturnObject(gameObject);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!GameManager.Instance.IsGameActive) return;

            switch (_type)
            {
                case TileType.Red:
                    GameManager.Instance.FinishGame();
                    break;

                case TileType.Destroyed1:
                case TileType.Destroyed2:
                case TileType.Destroyed3:
                    GameManager.Instance.AddPoints(1);
                    _type = TileType.Red;
                    _image.sprite = _red;
                    break;

                case TileType.Empty1:
                case TileType.Empty2:
                case TileType.Empty3:
                case TileType.Empty4:
                    break;
            }
        }
    }

    public enum TileType
    {
        Destroyed1,
        Destroyed2,
        Destroyed3,
        Red,
        Empty1,
        Empty2,
        Empty3,
        Empty4
    }
}