using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Laser;

namespace Laser
{
    public class LaserBarController : MonoBehaviour
    {
        // Start is called before the first frame update    // Start is called before the first frame update
        private Slider slider;
        public Image fill;
        public Gradient gradient;

        void Start()
        {
            slider = GetComponent<Slider>();
            slider.maxValue = LaserState.maxDuration;
            slider.value = LaserState.maxDuration;
            fill.color = Color.green;
        }

        // Update is called once per frame
        void Update()
        {
            slider.value = LaserState.maxDuration - LaserState.currentDuration;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    }
}