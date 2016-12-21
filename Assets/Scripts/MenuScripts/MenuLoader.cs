using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuLoader : MonoBehaviour
{
    public AudioManager audioManager;   // prefab to instantiate

    [SerializeField] private SpriteRenderer background;
    [SerializeField] private float backgroundFadeInTime = 1.5f;
    [SerializeField] private float backgroundFadePauseTime = 1.5f;

    private void Awake()
    {
        if (AudioManager.instance == null)
        {
            Instantiate(audioManager);
        }
    }

    private void Start()
    {
        StartCoroutine(IntroFade());
    }

    private IEnumerator IntroFade()
    {
        // Fade In
        StartCoroutine(FadeSprite(background, 0f, 1f, backgroundFadeInTime));
        yield return new WaitForSeconds(backgroundFadeInTime + backgroundFadePauseTime);

        // Fade Out
        StartCoroutine(FadeSprite(background, 1f, 0f, backgroundFadeInTime));
        yield return new WaitForSeconds(backgroundFadeInTime);

        SceneManager.LoadScene("Menu");
    }

    private IEnumerator FadeSprite(SpriteRenderer sprite, float startAlpha, float endAlpha, float fadeTime)
    {
        Color color = sprite.color;
        color.a = startAlpha;

        float timer = 0f;
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, timer / fadeTime);
            sprite.color = color;
            yield return null;
        }
    }
}
