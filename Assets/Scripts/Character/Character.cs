using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Character : MonoBehaviour
{
    public float startingHitPoints;
    public float maxHitPoints;
    public bool holdItem;
    public Vector2 direction = Vector2.zero;

    public enum CharacterCategory
    {
        PLAYER,
        ENEMY
    }

    public CharacterCategory characterCategory;

    public virtual void KillCharacter()
    {
        Destroy(gameObject);
    }
    public abstract void ResetCharacter();
    public abstract IEnumerator DamageCharacter(int damage, float interval);
    public virtual IEnumerator FlickerCharacter()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerEvent(collision);
    }

    public virtual void TriggerEvent(Collider2D collsion)
    {
        
    }
}