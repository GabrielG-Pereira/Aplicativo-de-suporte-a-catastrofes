namespace Aplicativo_de_suporte_a_catastrofes
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage()) { Title = "Aplicativo de suporte a catastrofes" };
        }
    }
}
