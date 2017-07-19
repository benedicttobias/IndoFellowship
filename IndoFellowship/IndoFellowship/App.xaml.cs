using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Auth;
using Xamarin.Forms;

namespace IndoFellowship {
	public partial class App : Application {
		public static bool IsUserLoggedIn { get; set; }
		public static string AppName { get; set; }

		public App() {
			InitializeComponent();

			AppName = "indofellowship.android";
		}

		protected override void OnStart() {
			// Handle when your app starts
		}

		protected override void OnSleep() {
			// Handle when your app sleeps
		}

		protected override void OnResume() {
			// Handle when your app resumes
		}
	}
}
