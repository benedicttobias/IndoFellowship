using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Text.Method;
using Android.Widget;
using Auth0.OidcClient;
using IdentityModel.OidcClient;
using System;
using System.Text;

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
		private Button loginButton;
		private TextView userDetailsTextView;
		private AuthorizeState authorizeState;
		ProgressDialog progress;

		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);
			this.SetContentView(Resource.Layout.Main);

			loginButton = this.FindViewById<Button> (Resource.Id.LoginButton);
			loginButton.Click += LoginButtonOnClick;

			userDetailsTextView = FindViewById<TextView>(Resource.Id.UserDetailsTextView);
			userDetailsTextView.MovementMethod = new ScrollingMovementMethod();
			userDetailsTextView.Text = String.Empty;

			client = new Auth0Client(new Auth0ClientOptions {
				Domain = "benedicttobias.auth0.com",
				ClientId = "IKg1u5Vw5nSxgx401vgYCQNq57R5QRNG",
				Activity = this
			});
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
					sb.AppendLine($"{claim.Type} = {claim.Value}");
				}
			}

			userDetailsTextView.Text = sb.ToString();
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
	}
}

