public class GameStateManager
{
    private static GameStateManager _instance;

    public static GameStateManager Instance
    {
        get {
            if (_instance == null)
            {
                _instance = new GameStateManager();
            }
            return _instance;
        }
    }
    public GameState CurrentGameState { get; private set; }

    public delegate void ChangeGameState(GameState gameState);
    public event ChangeGameState OnGameStateChanged;

    private GameStateManager()
    {

    }

    public void SetState(GameState gameState)
    {
        if (gameState == CurrentGameState)
        {
            return;
        }
        CurrentGameState = gameState;
        OnGameStateChanged?.Invoke(gameState);
    }
}
