using WeatherApplication.ViewModel;

namespace WeatherApplication.View;

public partial class SearchView : ContentPage
{
	public SearchView(SearchViewModel searchViewModel)
	{
		InitializeComponent();
		BindingContext = searchViewModel;
	}
}
