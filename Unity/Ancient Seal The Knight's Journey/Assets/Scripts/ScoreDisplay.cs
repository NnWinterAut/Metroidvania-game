using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    static Text ScoreText;
    void Awake()
    {
        ScoreText = GetComponent<Text>();

    }
    void Start()
    {
        ScoreManager.Instance.ResetScore();
    }
    // Update is called once per frame
    void Update()
    {
       
    }

    public static void UpdateText(int score)
    {
        ScoreText.text=score.ToString();
    }

    public static void ScaleText(Vector3 targetScale)
    {
        ScoreText.rectTransform.localScale = targetScale;
    }
}
