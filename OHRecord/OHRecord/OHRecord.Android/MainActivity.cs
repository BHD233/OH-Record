using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using Android.OS;
using Android;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.Design.Widget;
using Android.Content.PM;
using Android.App;
using Android.Views;


namespace OHRecord.Droid
{
    [Activity(Label = "OHRecord By BHD", Icon = "@drawable/icon1", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public bool isHave = false;
        private bool isCall = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            while (!isHave)
            {
                TryToGetPermissions();
            }
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        void TryToGetPermissions()
        {
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                GetPermissionsAsync();
                return;
            }
        }
        const int RequestLocationId = 0;

        readonly string[] PermissionsGroupLocation =
            {
                    //TODO add more permissions
                    Manifest.Permission.ReadExternalStorage,
                    Manifest.Permission.WriteExternalStorage,
             };
        void GetPermissionsAsync()
        {
            const string permission = Manifest.Permission.ReadExternalStorage;

            if (CheckSelfPermission(permission) == (int)Android.Content.PM.Permission.Granted)
            {
                //TODO change the message to show the permissions name
                isHave = true;
                return;
            }

            //is have not asked yet then ask
            if (!isCall)
            {
                RequestPermissions(PermissionsGroupLocation, RequestLocationId);
                isCall = true;
            }
        }
    }
}