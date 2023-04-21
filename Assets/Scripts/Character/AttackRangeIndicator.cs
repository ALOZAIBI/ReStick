using UnityEngine;

public class AttackRangeIndicator : MonoBehaviour {
    [SerializeField]
    private float attackRange = 3f; // The attack range of the character

    [SerializeField]
    private Color indicatorColor = Color.red; // The color of the attack range indicator

    private void OnDrawGizmosSelected() {
        // Draw a wire sphere with the attack range as the radius and the indicator color
        Gizmos.color = indicatorColor;
        Gizmos.DrawWireSphere(transform.position, attackRange);z
    }
}