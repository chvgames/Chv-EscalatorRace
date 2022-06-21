using UnityEngine;

public class OnHitEffect : MonoBehaviour
{
    public bool isHit = false;

    Material myMaterial;
    public Material onHitMaterial;

    MeshRenderer mesh;

    private float time;
    public float blinkTime = 0.2f;


    private void Start()
    {
        mesh = this.GetComponent<MeshRenderer>();
        myMaterial = mesh.material;

        time = blinkTime;
    }

    private void Update()
    {
        if (isHit) {

            time -= Time.deltaTime;

            if (time <= 0)
            {

                time = blinkTime;

                if (mesh.material == myMaterial)
                {
                    mesh.material = onHitMaterial;
                }
                else
                {

                    mesh.material = myMaterial;
                }
            }
        }        
    }
}
