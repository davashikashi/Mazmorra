using UnityEngine;

public class Final : MonoBehaviour
{
   
    public GameObject Running;
    public GameObject final;

    // Start is called before the first frame update


    public void StartGame()
    {
        final.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Running.SetActive(false);
            final.SetActive(true);
        }
    }
    
}
