using System.Windows.Controls;

namespace UnityAnalyzer.ObjectPanel
{
    /// <summary>
    /// RenderSettingsPanel.xaml 的交互逻辑
    /// </summary>
    public partial class RenderSettingsPanel
    {
        private RenderSettings _renderSettings;
        public RenderSettings RenderSettings
        {
            get { return _renderSettings; }
        }

        public RenderSettingsPanel()
        {
            InitializeComponent();
            _renderSettings = null;
        }

        public void SetUnityObject(RenderSettings renderSettings)
        {
            this._renderSettings = renderSettings;

            this.txtFog.Text = renderSettings.Fog.ToString();
            this.txtFogColor.Text = Util.GetColorRgbaString(renderSettings.FogColor);

            this.txtFogMode.Text = renderSettings.FogMode.ToString();
            this.txtFogDensity.Text = renderSettings.FogDensity.ToString();

            this.txtFogStart.Text = renderSettings.LinearFogStart.ToString();
            this.txtFogEnd.Text = renderSettings.LinearFogEnd.ToString();

            this.txtAmbientLight.Text= Util.GetColorRgbaString(renderSettings.AmbientLight);

            Material skyboxMaterial = renderSettings.GetSkyBoxMaterial();
            if (skyboxMaterial != null)
            {
                this.txtSkyBoxMaterial.Text = skyboxMaterial.Name;
            }

            this.txtHaloStrength.Text = renderSettings.HaloStrength.ToString();
            this.txtFlareStrength.Text = renderSettings.FlareStrength.ToString();

            this.txtFlareFadeSpeed.Text = renderSettings.FlareFadeSpeed.ToString();
            Texture2D haloTexture = renderSettings.GetHaloTexture();
            if (haloTexture != null)
            {
                this.txtHaloTexture.Text = haloTexture.Name;
            }

            Texture2D spotCookie = renderSettings.GetSpotCookie();
            if (spotCookie != null)
            {
                this.txtSpotCookie.Text = spotCookie.Name;
            }
        }

        private void Button_Click_SkyboxMaterial(object sender, System.Windows.RoutedEventArgs e)
        {
            Material skyboxMaterial = _renderSettings.GetSkyBoxMaterial();
            if (skyboxMaterial != null)
            {
                MainWindow.instance.ShowUnityObject(skyboxMaterial);
            }

           
        }

        private void Button_Click_HaloTexture(object sender, System.Windows.RoutedEventArgs e)
        {
            Texture2D haloTexture = _renderSettings.GetHaloTexture();
            if (haloTexture != null)
            {
                MainWindow.instance.ShowUnityObject(haloTexture);
            }
        }

        private void Button_Click_SpotCookie(object sender, System.Windows.RoutedEventArgs e)
        {
            Texture2D spotCookie = _renderSettings.GetSpotCookie();
            if (spotCookie != null)
            {
                MainWindow.instance.ShowUnityObject(spotCookie);
            }
        }
    }
}
