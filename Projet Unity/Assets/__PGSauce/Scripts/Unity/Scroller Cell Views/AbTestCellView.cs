using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PGSauce.Core.PGDebugging;
using EnhancedUI.EnhancedScroller;
using TMPro;
using UnityEngine.UI;

namespace PGSauce.PGRemote.ABTest
{
    public class AbTestCellView : EnhancedScrollerCellView
    {
        [SerializeField] private TMP_Text keyText;
        [SerializeField] private TMP_Dropdown abTestDropDown;
        [SerializeField] private TMP_Dropdown variantDropDown;
        
        public void SetData(AbTestScrollerController controller, ABTestHandler.DebugAbTest debugAbTest, int dIndex,
            ABTestHandler abTestHandler)
        {
            keyText.text = $"{debugAbTest.key} : {debugAbTest.GetValue()}";
            var options = new List<TMP_Dropdown.OptionData>();
            var abTests = debugAbTest.SupportedAbTests();
            var index = 0;

            for (var i = 0; i < abTests.Count; i++)
            {
                var abTest = abTests[i];
                options.Add(new TMP_Dropdown.OptionData(abTest.name));
                if (abTest.Equals(debugAbTest.abTest))
                {
                    index = i;
                }
            }

            abTestDropDown.options = options;
            abTestDropDown.value = index;
            abTestDropDown.onValueChanged.AddListener(i =>
            {
                var debug = abTestHandler.DebugAbTests[dIndex];
                var newDebug = new ABTestHandler.DebugAbTest(debug.key, abTests[i]);
                abTestHandler.DebugAbTests[dIndex] = newDebug;
                UpdateVariantDropdown(newDebug);
                keyText.text = $"{newDebug.key} : {newDebug.GetValue()}";
            });

            UpdateVariantDropdown(debugAbTest);
            variantDropDown.onValueChanged.AddListener(i =>
            {
                var debug = abTestHandler.DebugAbTests[dIndex];
                var newDebug = new ABTestHandler.DebugAbTest(debug.key, debug.abTest, debug.SupportedVariants()[i]);
                abTestHandler.DebugAbTests[dIndex] = newDebug;
                keyText.text = $"{newDebug.key} : {newDebug.GetValue()}";
            });
        }

        private void UpdateVariantDropdown(ABTestHandler.DebugAbTest debugAbTest)
        {
            var index2 = 0;
            var options2 = new List<TMP_Dropdown.OptionData>();
            var variants = debugAbTest.SupportedVariants();

            for (var i = 0; i < variants.Count; i++)
            {
                var variant = variants[i];
                options2.Add(new TMP_Dropdown.OptionData(variant));
                if (variant.Equals(debugAbTest.variant))
                {
                    index2 = i;
                }
            }

            variantDropDown.options = options2;
            variantDropDown.value = index2;
        }
    }
}
