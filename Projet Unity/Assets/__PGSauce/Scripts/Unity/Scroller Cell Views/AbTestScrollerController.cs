using System;
using EnhancedUI.EnhancedScroller;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PGSauce.PGRemote.ABTest
{
    public class AbTestScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
    {
        [SerializeField] private float cellSize = 500;
        [SerializeField] private ABTestHandler abTestHandler;
        [SerializeField] private EnhancedScroller scroller;
        [SerializeField, AssetsOnly] private AbTestCellView abTestCellViewPrefab;

        private void Start()
        {
            scroller.Delegate = this;
            scroller.ReloadData();
        }

        public int GetNumberOfCells(EnhancedScroller s)
        {
            return abTestHandler.DebugAbTests.Count;
        }

        public float GetCellViewSize(EnhancedScroller s, int dataIndex)
        {
            return cellSize;
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller s, int dataIndex, int cellIndex)
        {
            if (s.GetCellView(abTestCellViewPrefab) is AbTestCellView cell)
            {
                cell.SetData(this, abTestHandler.DebugAbTests[dataIndex], dataIndex, abTestHandler);
                return cell;
            }

            return null;
        }
    }
}