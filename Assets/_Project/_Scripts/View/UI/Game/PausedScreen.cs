using UnityEngine;
using View.Button;

namespace View.UI.Game
{
    public class PausedScreen : UIScreen
    {
        [SerializeField] private Misc.SceneManagment.SceneLoader _sceneLoader;
        [SerializeField] private UIScreen _optionsScreen;

        [Space, Header("Buttons")]
        [SerializeField] private CustomButton _continue;
        [SerializeField] private CustomButton _backtomenu;
        [SerializeField] private CustomButton _restart;
        [SerializeField] private CustomButton _settings;

        private void OnEnable()
        {
            _continue.AddListener(ContinueGame);
            _backtomenu.AddListener(BackToMenu);
            _settings.AddListener(OpenOptions);
            _restart.AddListener(Restart);
        }

        private void OnDisable()
        {
            _continue.RemoveListener(ContinueGame);
            _backtomenu.RemoveListener(BackToMenu);
            _settings.RemoveListener(OpenOptions);
            _restart.RemoveListener(Restart);
        }

        public override void StartScreen()
        {
            base.StartScreen();

            Time.timeScale = 0.0f;
        }

        private void ContinueGame()
        {
            Time.timeScale = 1.0f;
            CloseScreen();
        }

        private void BackToMenu()
        {
            Time.timeScale = 1.0f;
            _sceneLoader.ChangeScene(Misc.Data.SceneConstants.MENU_SCENE);
            CloseScreen();
        }

        private void Restart()
        {
            // Time.timeScale = 1.0f;
            GameCore.GameManager.Instance.RestartGame();
            CloseScreen();
        }

        private void OpenOptions()
        {
            _optionsScreen.StartScreen();
        }
    }
}