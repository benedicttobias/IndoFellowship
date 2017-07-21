using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Text.Method;
using Android.Widget;
using Auth0.OidcClient;
using IdentityModel.OidcClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Auth;
using Xamarin.Forms;

namespace IndoFellowship.Droid {
	// Define App icon and name
	[Activity(
		Label = "Indo App",
		MainLauncher = true,
		Icon = "@drawable/icon2",
		LaunchMode = LaunchMode.SingleTask)
	]

	// This will bring auth back to app
	[IntentFilter(
		new[] { Intent.ActionView },
		Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
		DataScheme = "indofellowship.android",
		DataHost = "benedicttobias.auth0.com",
		DataPathPrefix = "/android/indofellowship.android/callback")
	]

	public class MainActivity : Activity {

		private Auth0Client client;
		private Android.Widget.Button loginButton;
		private Android.Widget.Button logoutButton;
		private TextView userDetailsTextView;
		private AuthorizeState authorizeState;
		ProgressDialog progress;

		public string UserName {
			get {
				var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
				return (account != null) ? account.Username : null;
			}
		}

		public string Token {
			get {
				var account = AccountStore.Create(this).FindAccountsForService(App.AppName).FirstOrDefault();
				return (account != null) ? account.Properties["token"] : null;
			}
		}

		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Main);
			
			logoutButton = this.FindViewById<Android.Widget.Button>(Resource.Id.LogoutButton);
			logoutButton.Click += LogoutButtonOnClick;

			userDetailsTextView = FindViewById<TextView>(Resource.Id.UserDetailsTextView);
			userDetailsTextView.MovementMethod = new ScrollingMovementMethod();
			userDetailsTextView.Text = String.Empty;

			loginButton = this.FindViewById<Android.Widget.Button>(Resource.Id.LoginButton);
			loginButton.Click += LoginButtonOnClick;

			client = new Auth0Client(new Auth0ClientOptions {
				Domain = "benedicttobias.auth0.com",
				ClientId = "IKg1u5Vw5nSxgx401vgYCQNq57R5QRNG",
				Activity = this
			});

			if (UserName != null) {
				userDetailsTextView.Text = "Welcome Back, " + UserName + "!";
				logoutButton.Visibility = Android.Views.ViewStates.Visible;
				loginButton.Visibility = Android.Views.ViewStates.Gone;

				// Your layout reference
				LinearLayout mainLayout = (LinearLayout)FindViewById(Resource.Id.mainLayoutID);

				mainLayout.RemoveAllViews();

				StartActivity(typeof(XamarinForm));

			} else {
				userDetailsTextView.Text = "Please Login";
				logoutButton.Visibility = Android.Views.ViewStates.Gone;
				loginButton.Visibility = Android.Views.ViewStates.Visible;
			}
		}

		protected override void OnResume() {
			base.OnResume();

			if (progress != null) {
				progress.Dismiss();

				progress.Dispose();
				progress = null;
			}
		}

		protected override async void OnNewIntent(Intent intent) {
			base.OnNewIntent(intent);

			var loginResult = await client.ProcessResponseAsync(intent.DataString, authorizeState);

			var sb = new StringBuilder();
			if (loginResult.IsError) {
				sb.AppendLine($"An error occurred during login: {loginResult.Error}");
			} else {
				sb.AppendLine($"ID Token: {loginResult.IdentityToken}");
				sb.AppendLine($"Access Token: {loginResult.AccessToken}");
				sb.AppendLine($"Refresh Token: {loginResult.RefreshToken}");
				sb.AppendLine();

				sb.AppendLine("-- Claims --");
				foreach (var claim in loginResult.User.Claims) {
					if (claim.Type == "nickname") {
						// Save username and token
						SaveCredentials(claim.Value, loginResult.AccessToken);
					}
					sb.AppendLine($"{claim.Type} = {claim.Value}");
				}

				//// Show logout button
				//logoutButton.Visibility = Android.Views.ViewStates.Visible;
				//loginButton.Visibility = Android.Views.ViewStates.Gone;
			}

			userDetailsTextView.Text = sb.ToString();
		}

		public void SaveCredentials(string userName, string token) {
			if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(token)) {
				Account account = new Account {
					Username = userName
				};
				account.Properties.Add("token", token);
				AccountStore.Create(this).Save(account, App.AppName);
			}
		}

		private async void LoginButtonOnClick(object sender, EventArgs eventArgs) {
			userDetailsTextView.Text = "";

			progress = new ProgressDialog(this);
			progress.SetTitle("Log In");
			progress.SetMessage("Please wait while redirecting to login screen...");
			progress.SetCancelable(false); // disable dismiss by tapping outside of the dialog
			progress.Show();

			// Prepare for the login
			authorizeState = await client.PrepareLoginAsync();

			// Send the user off to the authorization endpoint
			var uri = Android.Net.Uri.Parse(authorizeState.StartUrl);
			var intent = new Intent(Intent.ActionView, uri);
			intent.AddFlags(ActivityFlags.NoHistory);
			StartActivity(intent);
		}

		private void LogoutButtonOnClick(object sender, EventArgs eventArgs) {
			var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
			if (account != null) {
				AccountStore.Create().Delete(account, App.AppName);
			}

			//userDetailsTextView.Text = string.Empty;

			//// Show Login button
			//logoutButton.Visibility = Android.Views.ViewStates.Gone;
			//loginButton.Visibility = Android.Views.ViewStates.Visible;
		}
	}
}

