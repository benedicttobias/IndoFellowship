using Auth0.OidcClient;
using IdentityModel.OidcClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;

namespace IndoFellowship
{
	public partial class MainPage : ContentPage {
		public MainPage()
		{
			InitializeComponent();
			var browser = new WebView();

			browser.Source = "http://xamarin.com";

			Content = browser;
		}
	}
}
