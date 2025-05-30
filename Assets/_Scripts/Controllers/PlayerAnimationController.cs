using UnityEngine;

namespace _Scripts.Controllers
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer leftHandRenderer;
        [SerializeField] private SpriteRenderer rightHandRenderer;
        [SerializeField] private SpriteRenderer headHandRenderer;
        
        [SerializeField] private Sprite[] leftHandSprites;
        [SerializeField] private Sprite[] rightHandSprites;
        [SerializeField] private Sprite[] headSprites;

        public void ChangePlayerSprites(bool isHolding)
        {
            int value = isHolding ? 1 : 0;
            leftHandRenderer.sprite = leftHandSprites[value];
            rightHandRenderer.sprite = rightHandSprites[value];
            headHandRenderer.sprite = headSprites[value];
        }
            
    }
}