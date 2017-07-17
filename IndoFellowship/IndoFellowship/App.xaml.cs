using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace IndoFellowship
{
	public static bool IsUserLoggedIn { get; set; }
	
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();
			
			if (!IsUserLoggedIn) {
				// Bring user to login
				MainPage = new NavigationPage (new IndoFellowship ());
			} else {
				// Bring user to main page
				MainPage = new NavigationPage (new IndoFellowship.MainPage());
			}
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
