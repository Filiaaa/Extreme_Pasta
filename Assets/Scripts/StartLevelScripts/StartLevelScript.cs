using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartLevelScript : MonoBehaviour
{

    public GameObject[] connections;
    public AudioSource acceptSound;
    public GameObject[] mainLight;
    public GameObject[] secondLight;
    public PlayerMover player;
    public GameObject[] secondObjects;
    public Collider2D[] secondWindows;
    public GameObject[] mainObjects;
    public Collider2D[] mainWindows;
    public int mainWindowNumb = 0;
    public int secondWindowNumb = 0;
    public int[] mainObjectsUsed;
    public int[] secondObjectsUsed;
   
    public List<int> lastMainObject = new List<int>();
    public List<int> lastSecondObject = new List<int>();

    public List<KeyValuePair_<int, int>> settedObjescts = new List<KeyValuePair_<int, int>>();
    public GameObject[] scriptsInScene;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (lastSecondObject.Count == lastMainObject.Count && lastMainObject.Count == mainObjects.Length &&
            ((secondObjects[lastSecondObject[lastSecondObject.Count - 1]].GetComponent<SecondIteractiveClass>().haveTimer &&
            secondObjects[lastSecondObject[lastSecondObject.Count - 1]].GetComponent<SecondIteractiveClass>().timerSet) ||
            !secondObjects[lastSecondObject[lastSecondObject.Count - 1]].GetComponent<SecondIteractiveClass>().haveTimer))
            {
                acceptSound.Play();
                for (int i = 0; i < scriptsInScene.Length; i++)
                {
                    scriptsInScene[i].GetComponent<ItemInScene>().caseNumb = settedObjescts[i].Key;
                    scriptsInScene[i].GetComponent<ItemInScene>().waitTime = settedObjescts[i].Value;
                    scriptsInScene[i].GetComponent<ItemInScene>().enabled = true;
                }
                player.enabled = true;
                gameObject.SetActive(false);
            }
        }
    }

    public void ReturnBack()
    {
        int mainCount = lastMainObject.Count;
        bool flag = false;

        if (lastSecondObject.Count != mainCount 
            || (lastSecondObject.Count == mainCount && 
            (  (secondObjects[lastSecondObject[lastSecondObject.Count - 1]].GetComponent<SecondIteractiveClass>().haveTimer &&
            secondObjects[lastSecondObject[lastSecondObject.Count - 1]].GetComponent<SecondIteractiveClass>().timerSet) || 
            !secondObjects[lastSecondObject[lastSecondObject.Count - 1]].GetComponent<SecondIteractiveClass>().haveTimer)))
        {
            if (lastMainObject.Count != 0)
            {
                mainObjects[lastMainObject[lastMainObject.Count - 1]].GetComponent<DragFirstObject>().SetStartPos();
                lastMainObject.Remove(lastMainObject[lastMainObject.Count - 1]);
                flag = true;
            }

            if (lastSecondObject.Count != 0 && lastSecondObject.Count == mainCount)
            {
                secondObjects[lastSecondObject[lastSecondObject.Count - 1]].GetComponent<SecondIteractiveClass>().SetStartPos();
                lastSecondObject.Remove(lastSecondObject[lastSecondObject.Count - 1]);

            }
            if (flag)
            {
                SetMain();
                SetOffSecondObjects();
                mainWindows[mainWindowNumb].enabled = true;
                if (mainWindowNumb != mainWindows.Length - 1)
                {
                    mainWindows[mainWindowNumb + 1].enabled = false;
                }
                if (secondWindowNumb != secondWindows.Length - 1)
                {
                    secondWindows[secondWindowNumb + 1].enabled = false;
                }

                secondWindows[secondWindowNumb].enabled = true;

            }
        }



    }

    public void UpdateMainWindows()
    {
        if (mainWindowNumb != mainWindows.Length)
        {
            mainWindows[mainWindowNumb - 1].enabled = false;
            mainWindows[mainWindowNumb].enabled = true;

        }

    }
    public void SetSecondObjects(GameObject[] secondObjects)
    {
        for (int i = 0; i < secondLight.Length; i++)
        {
            if (secondObjectsUsed[i] == 0)
            {
                secondLight[i].SetActive(true);
            }
            else
            {
                secondLight[i].SetActive(false);
            }
        }
        foreach (var obj in secondObjects)
        {
            obj.SetActive(true);
        }
    }
    public void SetOffMainObjects()
    {
        for (int i = 0; i < mainLight.Length; i++)
        {
            mainLight[i].SetActive(false);
        }
        for (int i = 0; i < mainObjects.Length; i++)
        {
            if (mainObjectsUsed[i] != 1)
            {
                mainObjects[i].SetActive(false);
            }
        }
    }

    public void SetMain()
    {
        for (int i = 0; i < mainLight.Length; i++)
        {
            if (mainObjectsUsed[i] == 0)
            {
                mainLight[i].SetActive(true);
            }
            else
            {
                mainLight[i].SetActive(false);
            }
        }
        foreach (var obj in mainObjects)
        {
            obj.SetActive(true);
        }
    }

    public void SetOffSecondObjects()
    {
        for (int i = 0; i < secondLight.Length; i++)
        {
            secondLight[i].SetActive(false);
        }
        for (int i = 0; i < secondObjects.Length; i++)
        {
            if (secondObjectsUsed[i] != 1)
            {
                secondObjects[i].SetActive(false);
            }
        }
    }

    public void UpdateSecondWindows()
    {
        if (secondWindowNumb != secondWindows.Length)
        {
            secondWindows[secondWindowNumb - 1].enabled = false;
            secondWindows[secondWindowNumb].enabled = true;

        }

    }
    [System.Serializable]
    public class KeyValuePair_<TKey, TValue>
    {
        public KeyValuePair_()
        {
        }

        public KeyValuePair_(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        [field: SerializeField] public TKey Key { set; get; }
        [field: SerializeField] public TValue Value { set; get; }
    }
}
