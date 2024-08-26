using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralBuildings : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject[] buildingSlot;
    [SerializeField] GameObject[] basePrefabs;
    [SerializeField] GameObject[] towerPrefabs;
    [SerializeField] GameObject[] roofHuts;
    GameObject building;
    

    void Start()
    {
        BuildingCount();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void BuildingCount()
    {
        for (int i = 0; i < buildingSlot.Length; i++)
        {
            SpawnBuilding(i);

        }
    }
    void SpawnBuilding(int loopIndex)
    {
        int buildingSeed = Random.Range(0, basePrefabs.Length);
        building = Instantiate(basePrefabs[buildingSeed], buildingSlot[loopIndex].transform.position, Quaternion.identity);
        
        GameObject towerPos = building.GetComponent<Building>().towerSlot;
        building.transform.SetParent(transform, true);

        int makeTower = Random.Range(0, 2);
        if(makeTower != 0 && towerPos != null) {
            int towerIndex = Random.Range(0, towerPrefabs.Length);
           GameObject tower = Instantiate(towerPrefabs[towerIndex], towerPos.transform.position, Quaternion.identity);
        
            tower.transform.parent = building.transform;
            building = tower;
        }

        GameObject[] roofHutPos = building.GetComponent<Building>().roofHutSlot;

        int roofHutIndex = Random.Range(0, roofHuts.Length);
        Debug.Log("roof hut index " + roofHutIndex);

        int roofHutPosIndex = Random.Range(0, roofHuts.Length);
        Debug.Log("roof hut position " + roofHutPosIndex);


        if(roofHutPos[roofHutPosIndex] != null) {

                GameObject hut = Instantiate(roofHuts[roofHutIndex], roofHutPos[roofHutPosIndex].transform.position, roofHutPos[roofHutPosIndex].transform.rotation);
                hut.transform.parent = building.transform;

        } else {
            return;
        }
        
    }
}
