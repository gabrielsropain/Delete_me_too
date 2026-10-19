using UnityEngine;

public class ScoreManager : MonoBehaviour
{
  public static ScoreManager Instance { get; private set; }



    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        
        Instance = this;
    }
}
