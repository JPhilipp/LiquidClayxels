using UnityEngine;

public class Grower : MonoBehaviour
{
    bool didGrow = false;

    void Update()
    {
        if (!didGrow && Input.GetKeyDown(KeyCode.Space))
        {
            didGrow = true;
            
            gameObject.AddComponent<Tweener>().Animate(
                scale: new Vector3(0.2f, 10f, 0.2f),
                seconds: 10f
            );
        }
    }
}
