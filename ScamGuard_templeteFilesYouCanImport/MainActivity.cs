using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.Linq;
using Android.Graphics.Drawables;

namespace $safeprojectname$
{
    [Activity(Label = "@string/app_name", Icon = "@mipmap/ic_launcher", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private ImageView logoImage;
        private Button loginButton;
        private Button reportButton;
        private SearchView searchView;
        private ListView listViewScammers;
        private ImageView backGround; // Changed the type to ImageView
        private ScammerAdapter scammerAdapter;
        private DatabaseHelper databaseHelper;
        private List<Scammer> scammers;
        private List<Scammer> filteredScammers;

        private int colorAccent;
        private int originalBackgroundResource;  // Store the original background resource ID

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            databaseHelper = new DatabaseHelper(this);

            // Initialize UI elements
            logoImage = FindViewById<ImageView>(Resource.Id.logo);
            loginButton = FindViewById<Button>(Resource.Id.btnLogIn);
            reportButton = FindViewById<Button>(Resource.Id.btnReport);
            searchView = FindViewById<SearchView>(Resource.Id.searchBox);
            listViewScammers = FindViewById<ListView>(Resource.Id.listScammers);
            backGround = FindViewById<ImageView>(Resource.Id.mainlayout); // Changed the type to ImageView

            // Set query hint for the SearchView
            searchView.SetQueryHint("Search for scammers...");

            // Register event handlers
            logoImage.Click += ChangeBackground;
            loginButton.Click += LoginButton_Click;
            reportButton.Click += ReportButton_Click;
            searchView.QueryTextSubmit += SearchView_QueryTextSubmit;

            // Get the color values from resources
            colorAccent = Resources.GetColor(Resource.Color.colorAccent);
            // Set the button background colors
            reportButton.SetBackgroundColor(new Android.Graphics.Color(colorAccent));

            // Load the scammers from the database
            scammers = databaseHelper.GetAllScammers();
            // Initialize filteredScammers with all scammers
            filteredScammers = new List<Scammer>(scammers);

            // Create the scammer adapter with the loaded data
            scammerAdapter = new ScammerAdapter(this, filteredScammers);

            // Set the adapter for the ListView
            listViewScammers.Adapter = scammerAdapter;

            // Store the original background resource ID
            originalBackgroundResource = Resource.Mipmap.background;
        }

        private void ChangeBackground(object sender, EventArgs e)
        {
            ImageView mainLayout = FindViewById<ImageView>(Resource.Id.mainlayout);
            Drawable currentBackground = mainLayout.Drawable;

            int newBackgroundResource;
                if (currentBackground == null || currentBackground.GetConstantState().Equals(Resources.GetDrawable(Resource.Mipmap.background).GetConstantState()))
                {
                    // If the current background is null or matches the "background",
                    // set the new background to "background2"
                    newBackgroundResource = Resource.Mipmap.background2;
                }
                else if (currentBackground.GetConstantState().Equals(Resources.GetDrawable(Resource.Mipmap.background2).GetConstantState()))
                {
                    // If the current background matches "background2",
                    // set the new background to "background3"
                    newBackgroundResource = Resource.Mipmap.background4;
                }
                else if (currentBackground.GetConstantState().Equals(Resources.GetDrawable(Resource.Mipmap.background4).GetConstantState()))
                {
                    // If the current background matches "background3",
                    // set the new background to "background"
                    newBackgroundResource = Resource.Mipmap.background;
                }
                else
                {
                    // Default case: if the current background doesn't match any of the specified backgrounds,
                    // set the new background to "background_portrait"
                    newBackgroundResource = Resource.Mipmap.background;
                }

            // Set the new background resource for the mainLayout ImageView
            mainLayout.SetImageResource(newBackgroundResource);
        }




        private void SearchView_QueryTextSubmit(object sender, SearchView.QueryTextSubmitEventArgs e)
        {
            string query = e.Query;

            // Filter the list of scammers based on the query
            filteredScammers = scammers.Where(s =>
                s.Username.ToLower().Contains(query.ToLower()) ||
                s.Description.ToLower().Contains(query.ToLower()) ||
                s.Website.ToLower().Contains(query.ToLower()) ||
                s.Email.ToLower().Contains(query.ToLower())
            ).ToList();

            // Update the scammer adapter with the filtered data
            scammerAdapter.Clear();
            scammerAdapter.AddAll(filteredScammers);
            scammerAdapter.NotifyDataSetChanged();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            // Handle login button click
        }

        private void ReportButton_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ReportActivity));
            StartActivity(intent);
        }
    }
}
