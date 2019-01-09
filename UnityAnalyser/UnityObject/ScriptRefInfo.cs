using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace UnityAnalyzer
{
    /// <summary>
    /// 该类的对象在ScriptRef中，比如Canvas Scaler的Script
    /// 该Script还有一些其余信息，例如UI scale mode等
    /// 这些信息存储在ScriptRefInfo的派生类中
    /// </summary>
    public class ScriptRefInfo
    {
        //对于CanvasScaler,因为它有其他一些信息，这些信息先存放在scriptInfoContent数组中，
        //然后根据Script的内容来决定如何解析这些内容
        private byte[] scriptInfoContent;
        public byte[] ScriptInfoContent
        {
            get { return scriptInfoContent; }
            set { scriptInfoContent = value; }
        }

        public ScriptRefInfo(byte[] scriptInfoContentArg)
        {
            this.scriptInfoContent = scriptInfoContentArg;
        }

        /// <summary>
        /// 用于显示这些Script附加信息的Panel，各个派生类独自生成该Panel并且赋值给scriptRefInfoPanel
        /// </summary>
        private UserControl scriptRefInfoPanel;
        public UserControl ScriptRefInfoPanel
        {
            get { return scriptRefInfoPanel; }
            set { scriptRefInfoPanel = value; }
        }
    }
}
