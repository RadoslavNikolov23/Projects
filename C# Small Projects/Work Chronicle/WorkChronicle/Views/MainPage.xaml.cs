namespace WorkChronicle.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel mainPageViewModel)
        {
            InitializeComponent();
            BindingContext = mainPageViewModel;

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is MainViewModel mv)
            {
                await mv.RefreshThePage();

            }
        }
    }
}
