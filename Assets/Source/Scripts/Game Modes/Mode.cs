public class Mode : UnityEngine.MonoBehaviour
{
    [System.NonSerialized]
    public LevelManager manager;

    private void Awake()
    {
        enabled = false;
    }

    public virtual void ModeStart()
    {

    }

    public virtual void PlayerJoined()
    {

    }

    public virtual void NextMove()
    {

    }

    public virtual LevelMode GetModeType()
    {
        return LevelMode.Vs;
    }
}