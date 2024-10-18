using UnityEngine;

public class MainGame : MonoBehaviour
{
    [SerializeField] private GameObject Boat1, Boat2;
    [SerializeField] private GameObject[] DecoyContainer;
    private int decoyIndex = 0;
    private bool blueDockedSuccessFully = false;

    [SerializeField] private GameObject SuccessImage;

    void OnEnable()
    {
        CarScript.onDockedSuccessfully += DockedSuccessfully;
    }

    void OnDisable()
    {
        CarScript.onDockedSuccessfully -= DockedSuccessfully;
    }

    void DockedSuccessfully(string carColor)
    {
        if (carColor.Contains("Blue"))
        {
            Boat1.gameObject.GetComponent<BoatScript>().ShowContainer();
            if (Boat1.gameObject.GetComponent<BoatScript>().allDocked)
            {
                blueDockedSuccessFully = true;
                Boat2.gameObject.transform.position = Boat1.gameObject.GetComponent<BoatScript>().startPosition;
                CheckForDecoyContainers();
            }
        }
        else
        {
            if (!blueDockedSuccessFully)
            {
                DecoyContainer[decoyIndex].SetActive(true);
                decoyIndex++;
            }
            else
            {
                Boat2.gameObject.GetComponent<BoatScript>().ShowContainer();
            }
        }

        if (decoyIndex == 3 && blueDockedSuccessFully)
        {
            for (int i = 0; i < decoyIndex; i++)
            {
                Boat2.gameObject.GetComponent<BoatScript>().ShowContainer();
            }
        }
    }

    private void CheckForDecoyContainers()
    {
        for (int i = 0; i < decoyIndex; i++)
        {
            Boat2.gameObject.GetComponent<BoatScript>().ShowContainer();
        }
    }

    private void Update()
    {
        if (blueDockedSuccessFully)
        {
            foreach (GameObject dc in DecoyContainer)
            {
                dc.SetActive(false);
            }
        }

        if (Boat2 != null && Boat2.gameObject.GetComponent<BoatScript>().allDocked)
        {
            Debug.Log("GAME COMPLETED");
            SuccessImage.SetActive(true);
        }



    }
}
