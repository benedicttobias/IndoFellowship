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
	public partial class MainPage : CarouselPage {
		public bool IsLogin = false;

		public MainPage()
		{
			InitializeComponent();

			NavigationPage.SetHasNavigationBar(this, false);
			
			CheckIfLogin();
			
			Children.Add(new Event());
			Children.Add(new Birthday());
			Children.Add(new News());
			Children.Add(new Devotion());
			Children.Add(new Settings());

		}

		private void CheckIfLogin() {
			Account account = App.AppAccount;
			if (account != null) {
				IsLogin = true;
				//ShowMessage("account registered");
			} else {
				IsLogin = false;
				//ShowMessage("account not registered");
			}
		}

		async void ShowMessage(string message) {
			await DisplayAlert("Alert", message, "OK");
		}
	}
}
