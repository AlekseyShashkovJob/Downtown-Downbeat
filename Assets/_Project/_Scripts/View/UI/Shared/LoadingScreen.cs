using System.Collections;
using UnityEngine;

namespace View.UI.Shared
{
    [RequireComponent(typeof(CanvasGroup))]
    public class LoadingScreen : MonoBehaviour
    {
        private readonly float _speed = 3.0f;
        private readonly float _height = 60.0f;

        [SerializeField] private GameObject _backgroundPortrait;
        [SerializeField] private GameObject _backgroundLandscape;
        [SerializeField] private RectTransform _loadingLogo;

        private Vector2 _baseAnchoredPos;
        private Vector2 _lastScreenSize;

        private void Start()
        {
            _baseAnchoredPos = _loadingLogo.anchoredPosition;
            _lastScreenSize = new Vector2(Screen.width, Screen.height);
        }

        private void Update()
        {
            if (ScreenSizeChanged())
            {
                ResetBackground();
                ResetBasePosition();
            }

            float offsetY = Mathf.Sin(Time.time * _speed) * _height;
            _loadingLogo.anchoredPosition = _baseAnchoredPos + new Vector2(0, offsetY);
        }

        private bool ScreenSizeChanged()
        {
            Vector2 currentSize = new Vector2(Screen.width, Screen.height);
            if (currentSize != _lastScreenSize)
            {
                _lastScreenSize = currentSize;
                return true;
            }
            return false;
        }

        private void ResetBasePosition()
        {
            _baseAnchoredPos = _loadingLogo.anchoredPosition - new Vector2(0, Mathf.Sin(Time.time * _speed) * _height);
        }

        private void ResetBackground()
        {
            bool isPortrait = Screen.height >= Screen.width;

            _backgroundPortrait.SetActive(isPortrait);
            _backgroundLandscape.SetActive(!isPortrait);
        }
    }
}