using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : Singleton<PanelManager>
{
    // all the existing panel instances
    private List<PanelInstanceModel> instances_list = new List<PanelInstanceModel>();

    private ObjectPool objectPool;

    private void Start()
    {
        objectPool = ObjectPool.Instance;
    }
    public void ShowPanel(string PanelId, PanelShowBehaviour behaviour = PanelShowBehaviour.KEEP_PREVIOUS)
    {
        if (instances_list.Find(panel => panel.PanelId == PanelId) == null) // not to create multiple copies of the same prefab
        {
            GameObject panelInstance = objectPool.GetObjectFromPool(PanelId);
            if (panelInstance == null)
            {
                Debug.LogWarning($"Trying to show inexistent panel. PanelId: {PanelId}");
                return;
            }
            if (behaviour == PanelShowBehaviour.HIDE_LAST)
            {
                var lastPanel = GetLastPanel();
                if (lastPanel != null)
                {
                    lastPanel.PanelInstance.SetActive(false);
                }
            }
            
            instances_list.Add(new PanelInstanceModel
            {
                PanelId = PanelId,
                PanelInstance = panelInstance
            });
        }
    }

    public void HidePanel(string PanelId)
    {
        PanelInstanceModel panelToHide = instances_list.Find(panel => panel.PanelId == PanelId);
        if (panelToHide == null)
        {
            Debug.LogWarning($"Trying to hide inexistent panel. PanelId: {PanelId}");
            return;
        }
        instances_list.Remove(panelToHide);
        objectPool.AddObjectToPool(panelToHide.PanelInstance);
        var previousPanel = GetLastPanel();
        if (previousPanel != null)
        {
            previousPanel.PanelInstance.SetActive(true);
        }
    }

    public void HideLastPanel()
    {
        if (instances_list.Count > 0)
        {
            var lastPanel = instances_list[instances_list.Count - 1];
            instances_list.Remove(lastPanel);
            objectPool.AddObjectToPool(lastPanel.PanelInstance);
            var previousPanel = GetLastPanel();
            if (previousPanel != null)
            {
                previousPanel.PanelInstance.SetActive(true);
            }
        }
    }

    public void HideAllPanels()
    {
        while(instances_list.Count > 0)
        {
            HideLastPanel();
        }
    }

    private PanelInstanceModel GetLastPanel()
    {
        if (instances_list.Count > 0)
        {
            return instances_list[instances_list.Count - 1];
        }
        return null;
    }

    public bool IsAnythingShown()
    {
        return instances_list.Count > 0;
    }

    public int GetNumberOfShownPanels()
    {
        return instances_list.Count;
    }
}
