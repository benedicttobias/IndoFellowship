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
		public bool IsLogin = false;

		public MainPage()
		{
			InitializeComponent();

			CheckIfLogin();
			Button button = new Button {
				Text = IsLogin.ToString(),
				Font = Font.SystemFontOfSize(NamedSize.Large),
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};
			button.Clicked += OnButtonClicked;

			// Build the page.
			this.Content = new StackLayout {
				Children =
				{
					button
				}
			};
		}

		private void CheckIfLogin() {
			Account account = App.AppAccount;
			if (account != null) {
				IsLogin = true;
				ShowMessage("account registered");
			} else {
				IsLogin = false;
				ShowMessage("account not registered");
			}
		}

		void OnButtonClicked(object sender, EventArgs e) {
			App.RemoveCredentials();
			ShowMessage("account deleted");
			switch (Device.RuntimePlatform) {
				case Device.iOS:
					//iOS.OS.Process.KillProcess(iOS.OS.Process.MyPid());
					break;
				case Device.Android:
					Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
					break;
			}
		}

		async void ShowMessage(string message) {
			await DisplayAlert("Alert", message, "OK");
		}
	}
}
