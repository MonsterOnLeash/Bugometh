using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePanelButton : MonoBehaviour
{
    // id of the panel to be hidden
    public string panelId;

    private PanelManager panel_manager;

    public void OnButtonPress()
    {
        panel_manager.HidePanel(panelId);
    }

    private void Start()
    {
        panel_manager = PanelManager.Instance;
    }
}
