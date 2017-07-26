using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IndoFellowship
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Settings : ContentPage
	{
		public Settings ()
		{
			InitializeComponent ();

			Label label = new Label { Text = App.AppAccount.Username.ToString(), BackgroundColor = Color.White, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, TextColor = Color.Black };

			Button button = new Button {
				Text = "Logout",
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
					label,
					button
				}
			};
		}

		void OnButtonClicked(object sender, EventArgs e) {
			App.RemoveCredentials();
			switch (Device.RuntimePlatform) {
				case Device.iOS:
					//iOS.OS.Process.KillProcess(iOS.OS.Process.MyPid());
					break;
				case Device.Android:
					Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
					break;
			}
		}
	}
}