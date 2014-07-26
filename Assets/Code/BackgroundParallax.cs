using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    public Transform[] Backgrounds;
    public float ParrallaxScale;
    public float ParallaxReductionFactor;
    public float Smoothing;

    private Vector3 _lastPostition;

    public void Start()
    {
        _lastPostition = transform.position;
    }

    public void Update()
    {
        var parallax = (_lastPostition.x - transform.position.x) * ParrallaxScale;

        for (var i =0; i<Backgrounds.Length; i++)
        {
            var backgroundTargetPosition = Backgrounds[i].position.x + parallax* (i*ParallaxReductionFactor+1);
            Backgrounds[i].position = Vector3.Lerp(Backgrounds[i].position, new Vector3(backgroundTargetPosition, Backgrounds[i].position.y, Backgrounds[i].position.z), Smoothing*Time.deltaTime);
        }
        _lastPostition = transform.position;
    }
}
