using UnityEngine;

public class ScoreProgressEffectManager : MonoBehaviour
{
    [SerializeField]
    private ScoreProgressEffect[] effects;

    public void TriggerEffect(int checkPointIndex)
    {
        effects[checkPointIndex ].TriggerEffect();
    }
}
