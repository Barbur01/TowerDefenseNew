using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFactory
{
    static string TOWER_PATH_BASE = "Towers/";
    static string[] TOWER_PATH = { "Basic/" };

    public static Tower CreateTower(Tower.Type type)
    {
        Tower tower = null;

        string fullpath = TOWER_PATH_BASE + TOWER_PATH[(int)type];
        Object[] allTowerLevels = Resources.LoadAll(fullpath) as Object[];

        if (allTowerLevels != null)
        {
            tower = new Tower(Tower.Type.BASIC);

            foreach (GameObject towerObject in allTowerLevels)
            {
                TowerLevel newLevel = towerObject.GetComponent<TowerLevel>();
                tower.AddLevel(newLevel);
            }

            Debug.Log(Tower.Type.BASIC.ToString() + " tower created. Contains " + allTowerLevels.Length + " number of levels.");
        }

        return tower;
    }
}
