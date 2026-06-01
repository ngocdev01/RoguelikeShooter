using UnityEngine;
public class GameTime
{
    public static float timeScale { get; set; } = 1f;
    public static float deltaTime { get => Time.deltaTime * timeScale; }
    public static float unscaledDeltaTime { get => Time.deltaTime; }
    public static float time { get; private set; }
}


