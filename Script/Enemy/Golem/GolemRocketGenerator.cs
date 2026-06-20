using UnityEngine;
using System.Collections.Generic;

public class GolemRocketGenerator : MonoBehaviour
{
    public GameObject GolemRocket;

    public void GolemRocketShot(GameObject obj, GameObject Player)
    {
        GameObject rocket = Instantiate(GolemRocket, obj.transform.position, obj.transform.rotation);
        GolemRocketController grc = rocket.GetComponent<GolemRocketController>();
        grc.Init_Rocket(Player.transform.position);
    }

/*
    
    public void GolemRocketShot()
    {
        Vector3 GolemPos = this.transform.position;
        List<int> OffsetX = new List<int> { -2, -1, 0, 1, 2};
        List<int> OffsetY = new List<int> { -2, -1, 0, -1, -2};
        for(int i = 0; i<RocketCount; i++){
            float offset = (i -(RocketCount-1)/2.0f)*dist;
            Vector3 spawnPos = new Vector3(GolemPos.x + OffsetX[i], GolemPos.y + (15 + OffsetY[i]), GolemPos.z);
            GameObject rocket = Instantiate(GolemRocket, spawnPos, this.transform.rotation);
        }
    }
    public void GolemRocketShot()
    {
        if(Player!=null)
        {
            Vector3 PlayerPos = this.Player.transform.position;
            int [] rockets = { -2, -1, 0, 1, 2};
            for(int i = 0; i<RocketCount; i++)
            {
                GameObject rocket = Instantiate(GolemRocket);

                Vector3 rocketPos = 
                    new Vector3(PlayerPos.x+rockets[i], PlayerPos.y + 5, PlayerPos.z);
                rocket.transform.position = rocketPos;
            }
        }
    }
*/
}
