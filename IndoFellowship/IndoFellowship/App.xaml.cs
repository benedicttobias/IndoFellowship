using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Auth;
using Xamarin.Forms;

namespace IndoFellowship {
	public partial class App : Application {
		public static string AppName { get { return "indofellowship"; } }

		public static Account AppAccount {
			get { return getAppAccount(); }
			set { SaveCredentials(value); }
		}

		private static Account getAppAccount() {
			Account thisAppAccount = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault(); ;

			if (thisAppAccount == null) {
				return null;
			} else {
				return thisAppAccount;
			}
		}

		public App() {
			InitializeComponent();
			

			MainPage = new NavigationPage(new MainPage());
		}

		public App(Account passedAccount) {
			InitializeComponent();

			AppAccount = passedAccount;

			MainPage = new NavigationPage(new MainPage());
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

		public static void SaveCredentials(Account account) {
			AccountStore.Create().Save(account, App.AppName);
		}

		public static void RemoveCredentials() {
			AccountStore.Create().Delete(AppAccount, App.AppName);
		}
	}
}
