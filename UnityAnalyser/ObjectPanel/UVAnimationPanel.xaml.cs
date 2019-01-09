﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UnityAnalyzer
{
    /// <summary>
    /// UVAnimationPanel.xaml 的交互逻辑
    /// </summary>
    public partial class UVAnimationPanel : UserControl
    {
        private UVAnimation uvAnimation;
        public UVAnimationPanel()
        {
            InitializeComponent();
        }

        public void SetUVAnimation(UVAnimation uvAnimation)
        {
            this.uvAnimation=uvAnimation;
            this.txtXTile.Text = uvAnimation.XTile.ToString();
            this.txtYTile.Text = uvAnimation.YTile.ToString();
            this.txtCycles.Text = uvAnimation.Cycles.ToString();
        }
    }
}
