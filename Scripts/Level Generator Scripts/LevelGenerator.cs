using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private int levelLength;

    [SerializeField]
    private int startPlatformLength = 5, endPlatformLength = 5;

    [SerializeField]
    private int distance_Between_Platforms;

    [SerializeField]
    private Transform platformPrefab, platform_Parent;

    [SerializeField]
    private Transform monster, monster_Parent;

    [SerializeField]
    private Transform health_Collectable, healthCollectable_Parent;

    [SerializeField]
    private float platformPosition_MinY = 0f, platformPosition_MaxY = 10f;

    [SerializeField]
    private int platformLength_Min = 1, platformLength_Max = 4;

    [SerializeField]
    private float chanceForMonsterExistence = 0.25f, chanceForCollectableExistence = 0.1f;

    [SerializeField]
    private float healthCollectable_MinY = 1f, healthCollectable_MaxY = 3f;

    private float platformLastPositionX;

    private enum PlatformType
    {
        None,
        Flat
    }

    private class PlatformPositionInfo
    {
        public PlatformType platformType;
        public float positionY;
        public bool hasMonster;
        public bool hasHealthCollectable;

        public PlatformPositionInfo(PlatformType type,float posY,bool has_monster,bool has_collectable)
        {
            platformType = type;
            positionY = posY;
            hasMonster = has_monster;
            hasHealthCollectable = has_collectable;
        }

    }//class platformPositionInfo

    void Start()
    {
        GenerateLevel(true);
    }

    void FillOutPositionInfo(PlatformPositionInfo[] platformInfo)
    {
        int currentPlatformInfoIndex = 0;

        for (int i = 0; i < startPlatformLength; i++)
        {
            platformInfo[currentPlatformInfoIndex].platformType = PlatformType.Flat;
            platformInfo[currentPlatformInfoIndex].positionY = 0f;

            currentPlatformInfoIndex++;
        }

        while (currentPlatformInfoIndex < levelLength - endPlatformLength)
        {
            if (platformInfo[currentPlatformInfoIndex - 1].platformType != PlatformType.None)
            {
                currentPlatformInfoIndex++;
                continue;
            }

            float platformPositionY = Random.Range(platformPosition_MinY, platformPosition_MaxY);

            int platformLength = Random.Range(platformLength_Min, platformLength_Max);

            for(int i = 0; i < platformLength; i++)
            {
                bool has_Monster = (Random.Range(0f, 1f) < chanceForMonsterExistence);
                bool has_healthCollectable = (Random.Range(0f, 1f) < chanceForCollectableExistence);

                platformInfo[currentPlatformInfoIndex].platformType = PlatformType.Flat;
                platformInfo[currentPlatformInfoIndex].positionY = platformPositionY;
                platformInfo[currentPlatformInfoIndex].hasMonster = has_Monster;
                platformInfo[currentPlatformInfoIndex].hasHealthCollectable = has_healthCollectable;

                currentPlatformInfoIndex++;

                if (currentPlatformInfoIndex > (levelLength - endPlatformLength))
                {
                    currentPlatformInfoIndex = levelLength - endPlatformLength;
                    break;
                }

            }

            for(int i = 0; i < endPlatformLength; i++)
            {
                platformInfo[currentPlatformInfoIndex].platformType = PlatformType.Flat;
                platformInfo[currentPlatformInfoIndex].positionY = 0f;

                currentPlatformInfoIndex++;
            }

        }//while loop
    }

    void CreatePlatformsFromPositionInfo(PlatformPositionInfo[] platformPositionInfo, bool gameStarted)
    {
        for(int i = 0; i < platformPositionInfo.Length; i++)
        {
            PlatformPositionInfo positionInfo = platformPositionInfo[i];

            if (positionInfo.platformType == PlatformType.None)
            {
                continue;
            }

            Vector3 platformPosition;

            //here we are going to check whether the game is started or not
            if (gameStarted)
            {
                platformPosition = new Vector3(distance_Between_Platforms * i, positionInfo.positionY, 0);
            }
            else
            {
                platformPosition = new Vector3(distance_Between_Platforms + platformLastPositionX, positionInfo.positionY, 0);
            }

            //save the platform position x for later use
            platformLastPositionX = platformPosition.x;

            Transform createBlock = (Transform)Instantiate(platformPrefab, platformPosition, Quaternion.identity);
            createBlock.parent = platform_Parent;

            if (positionInfo.hasMonster)
            {
                if (gameStarted)
                {
                    platformPosition = new Vector3(distance_Between_Platforms * i, positionInfo.positionY + 0.1f, 0);
                }
                else
                {
                    platformPosition = new Vector3(distance_Between_Platforms + platformLastPositionX, positionInfo.positionY + 0.1f, 0);
                }

                Transform createMonster = (Transform)Instantiate(monster, platformPosition, Quaternion.Euler(0, -90, 0));
                createMonster.parent = monster_Parent;

            }
            if (positionInfo.hasHealthCollectable)
            {
                if (gameStarted)
                {
                    platformPosition = new Vector3(distance_Between_Platforms * i,
                        positionInfo.positionY + Random.Range(healthCollectable_MinY, healthCollectable_MaxY), 0);
                }
                else
                {
                    platformPosition = new Vector3(distance_Between_Platforms + platformLastPositionX,
                        positionInfo.positionY + Random.Range(healthCollectable_MinY, healthCollectable_MaxY), 0);
                }

                Transform createHealthCollectable = (Transform)Instantiate(health_Collectable,
                    platformPosition, Quaternion.identity);
                createHealthCollectable.parent = healthCollectable_Parent;
            }


        }//for loop
    }

    public void GenerateLevel(bool gameStarted)
    {
        PlatformPositionInfo[] platformInfo = new PlatformPositionInfo[levelLength];
        for(int i = 0; i < platformInfo.Length; i++)
        {
            platformInfo[i] = new PlatformPositionInfo(PlatformType.None, -1f, false, false);
        }

        FillOutPositionInfo(platformInfo);
        CreatePlatformsFromPositionInfo(platformInfo, gameStarted);
    }



    
}//end
