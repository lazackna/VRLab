using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelSelect : MonoBehaviour
{
    private bool triggered;
    private GameObject choosingClothes, changingroomWall;
	private void Start()
	{
        triggered = false;
        choosingClothes = GameObject.Find("ChooseClothesCanvas");
        changingroomWall = GameObject.Find("ChangingroomWall");
	}

	private void OnTriggerStay(Collider other)
	{
        if (!triggered)
        {
            triggered = true;
            Debug.Log("triggerd = true");
            StartCoroutine(AdditiveScene(1));
        }
	}

    private IEnumerator AdditiveScene(int seconds)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            yield return new WaitForSeconds(seconds);
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }

        choosingClothes.SetActive(false);
        Destroy(changingroomWall);
    }
}
