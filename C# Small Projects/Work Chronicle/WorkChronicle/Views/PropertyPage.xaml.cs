namespace WorkChronicle.Views
{
	public partial class PropertyPage : ContentPage
	{
		public PropertyPage(PropertyViewModel propertyViewModel)
		{
			InitializeComponent();
			BindingContext = propertyViewModel;
		}

        //private void OnLanguageChanged(object sender, EventArgs e)
        //{
        //    switch (LanguagePicker.SelectedIndex)
        //    {
        //        case 0:
        //            LocalizationHelper.SetCulture("en");
        //            break;
        //        case 1:
        //            LocalizationHelper.SetCulture("bg");
        //            break;
        //    }

        //    // Optionally force app/UI refresh
        //    Application.Current.MainPage = new AppShell(); // or your MainPage class
        //}
    }
}