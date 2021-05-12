using UnityEngine;
//using Unity.Mathfx;

public class BounceAnimation : MonoBehaviour
{
    [SerializeField] private float startTime = 0f;
    [SerializeField] private float time;

    // Start is called before the first frame update
    private void Start()
    {
        time = startTime;
    }

    private void OnEnable()
    {
        time = startTime;
    }

    // Update is called once per frame
    private void Update()
    {        
        if (time <= 1f)
        {
            transform.localScale = Vector3.one * Mathfx.Berp(0f, 1f, time);
            time = time + Time.deltaTime * 1.5f;
        }
    }
}

public sealed class Mathfx : MonoBehaviour
{
    public static float Berp(float start, float end, float value)
    {
        value = Mathf.Clamp01(value);
        value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
        return start + (end - start) * value;
    }

    public static Vector2 Berp(Vector2 start, Vector2 end, float value)
    {
        return new Vector2(Berp(start.x, end.x, value), Berp(start.y, end.y, value));
    }

    public static Vector3 Berp(Vector3 start, Vector3 end, float value)
    {
        return new Vector3(Berp(start.x, end.x, value), Berp(start.y, end.y, value), Berp(start.z, end.z, value));
    }
    
}
