using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class PlaneUi : MonoBehaviour
{
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"Altitude: {GameManager.instance.playerController.Altitude}\n" +
                    $"Velocity: {GameManager.instance.playerController.Target.Body.AbsoluteVelocity.magnitude}\n" +
                    $"Engine: {GameManager.instance.playerController.Target.force}";

    }
}
