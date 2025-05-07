using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishDetector : MonoBehaviour
{
    [HideInInspector]
    public ElevatorController elevatorController;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("Win");
        }
    }
}

