using Newtonsoft.Json;
using UnityEngine;
using Zork.Common;
using TMPro;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI LocationText;

    [SerializeField]
    private TextMeshProUGUI ScoreText;

    [SerializeField]
    private TextMeshProUGUI HealthText;

    [SerializeField]
    private TextMeshProUGUI MovesText;

    [SerializeField]
    private UnityInputService InputService;
    [SerializeField]
    private UnityOutputService OutputService;

    private void Awake()
    {
        TextAsset gameJson = Resources.Load<TextAsset>("GameJson");
        _game = JsonConvert.DeserializeObject<Game>(gameJson.text);
        _game.Player.LocationChanged += Player_LocationChanged;
       _game.Run(InputService, OutputService);
       _game.Player.ScoreChanged += Player_ScoreChanged;
       _game.Player.MovesChanged += Player_MovesChanged;
        _game.Player.HealthChanged += Player_HealthChanged;
        
    }

    private void Player_LocationChanged(object sender, Room location)
    {
        LocationText.text = location.Name;
    }

    private void Player_ScoreChanged(object sender, int Score)
    {
       string scoretext = Score.ToString();
        ScoreText.text = scoretext;
        
    }

     private void Player_MovesChanged(object sender, int Moves)
    {
       string movetext = Moves.ToString();
        MovesText.text = movetext;
        
    }

      private void Player_HealthChanged(object sender, int Health)
    {
       string healthtext = Health.ToString();
        HealthText.text = healthtext;
        
    }

    private void Start()
    {
        InputService.SetFocus();
        LocationText.text = _game.Player.CurrentRoom.Name;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            InputService.ProcessInput();
            InputService.SetFocus();
        }

        if (_game.IsRunning == false)
        {
#if UNITY_EDITOR
            // Application.Quit() does not work in editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
    private Game _game;
}
