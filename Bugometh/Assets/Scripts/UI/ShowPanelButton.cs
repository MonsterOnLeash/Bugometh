using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPanelButton : MonoBehaviour
{
    // id of the panel to be shown
    public string panelId;

    public PanelShowBehaviour behaviour = PanelShowBehaviour.KEEP_PREVIOUS;

    private PanelManager panel_manager;

    public void OnButtonPress()
    {
        panel_manager.ShowPanel(panelId, behaviour);
    }

    private void Start()
    {
        panel_manager = PanelManager.Instance;
    }
}
