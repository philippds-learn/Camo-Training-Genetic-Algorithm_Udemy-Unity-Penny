using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA : MonoBehaviour
{
    // gene for colour
    public float r;
    public float g;
    public float b;
    public float scaleFactor;

    bool dead = false;
    public float timeToDie = 0;
    SpriteRenderer sRenderer;
    Collider2D sCollider;

    private void OnMouseDown()
    {
        this.dead = true;
        this.timeToDie = PopulationManager.elapsed;
        Debug.Log("Dead At: " + timeToDie);
        this.sRenderer.enabled = false;
        this.sCollider.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.sRenderer = GetComponent<SpriteRenderer>();
        this.sCollider = GetComponent<Collider2D>();
        this.sRenderer.color = new Color(this.r, this.g, this.b);
        this.transform.localScale = new Vector3(this.transform.localScale.x * this.scaleFactor, this.transform.localScale.y * this.scaleFactor, 1);
    }
}
