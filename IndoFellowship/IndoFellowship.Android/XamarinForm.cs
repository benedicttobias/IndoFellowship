using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace IndoFellowship.Droid {
	[Activity(Label = "XamarinForm")]
	public class XamarinForm : Xamarin.Forms.Platform.Android.FormsApplicationActivity {
		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			Xamarin.Forms.Forms.Init(this, bundle);
			LoadApplication(new App());
		}
	}
}