using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    private bool trigger;
    private float damage;

    private void Start()
    {
        trigger = false;
    }

    public void BeamInit(float h, float v, float duration, float damage)
    {
        this.damage = damage;
        this.trigger = true;
        this.transform.localScale = new Vector2(h, v);
        StartCoroutine(Activation(duration));
    }

    protected IEnumerator Activation(float beamDuration)
    {
        yield return new WaitForSeconds(beamDuration);
        this.trigger = false;
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (trigger)
        {
            if (collision.TryGetComponent(out Monster monster))
            {
                monster.Hit(damage);
            }
        }
    }
}
