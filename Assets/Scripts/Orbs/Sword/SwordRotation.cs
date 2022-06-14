using UnityEngine;

public class SwordRotation : MonoBehaviour
{
    public float rotationSpeed;
    public float zDifference;

    GameObject player;

    bool attackFacingDone;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Rotate();
        Facing();
    }

    void Rotate()
    {
        if (!GetComponent<MoveToCurser>().moving)
        {
            if (GetComponent<MoveToCurser>().movingRight)
            {
                transform.eulerAngles = new Vector3(0, 0, zDifference);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 180, zDifference);
            }
            attackFacingDone = false;
        }
        else
        {
            transform.Rotate(0, 0, rotationSpeed);
        }
    }

    void Facing()
    {
        if (GetComponent<MoveToCurser>().followPlayer)
        {
            if (player.transform.eulerAngles == new Vector3(0, 0, 0))
            {
                transform.eulerAngles = new Vector3(0, 0, zDifference);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 180, zDifference);
            }
        }else if (GetComponent<MoveToCurser>().moving && !attackFacingDone)
        {
            if (GetComponent<MoveToCurser>().movingRight)
            {
                transform.eulerAngles = new Vector3(0, 0, zDifference);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 180, zDifference);
            }

            attackFacingDone = true;
        }
    }
}
