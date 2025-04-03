namespace WorkChronicle.Views
{

	public partial class PropertiePage : ContentPage
	{
		public PropertiePage(PropertieViewModel propertieViewModel)
		{
			InitializeComponent();
			BindingContext = propertieViewModel;
		}
	}
}