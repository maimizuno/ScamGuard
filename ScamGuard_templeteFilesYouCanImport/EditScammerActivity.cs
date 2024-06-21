using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using static Xamarin.Essentials.Platform;

namespace $safeprojectname$
{
    [Activity(Label = "Update Scam Report")]
    public class EditScammerActivity : AppCompatActivity
    {
        // Declare the database helper instance
        private DatabaseHelper databaseHelper;

        // Declare the UI elements
        private EditText usernameEditText;
        private EditText emailEditText;
        private EditText phoneEditText;
        private EditText websiteEditText;
        private RadioGroup severityRadioGroup;
        private EditText descriptionEditText;
        private Button saveButton;
        private Button cancelButton2;
        private Button deleteButton;

        // Declare the datatable elements
        private Scammer scammer;
        private int scammerId;

        // Declare the color variables
        private int colorAccent;
        private int colorWarning;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.edit_scammer);

            // Get the color values from resources
            colorAccent = Resources.GetColor(Resource.Color.colorAccent);
            colorWarning = Resources.GetColor(Resource.Color.colorWarning);

            // Instantiate the database helper
            databaseHelper = new DatabaseHelper(this);

            // Get the scammer data passed from the ScammerAdapter
            scammerId = Intent.GetIntExtra("ScammerId", 0);
            scammer = databaseHelper.GetScammerById(scammerId);

            // Find the EditTexts, RadioGroup, and Button within the layout
            usernameEditText = FindViewById<EditText>(Resource.Id.edtUsername2);
            emailEditText = FindViewById<EditText>(Resource.Id.edtEmail2);
            phoneEditText = FindViewById<EditText>(Resource.Id.edtPhone2);
            websiteEditText = FindViewById<EditText>(Resource.Id.edtWebsite2);
            severityRadioGroup = FindViewById<RadioGroup>(Resource.Id.RbtnGroup2);
            descriptionEditText = FindViewById<EditText>(Resource.Id.edtDescription2);
            saveButton = FindViewById<Button>(Resource.Id.btnSave);
            cancelButton2 = FindViewById<Button>(Resource.Id.btnCancel2);
            deleteButton = FindViewById<Button>(Resource.Id.btnDelete);

            // Set the scammer data to the EditTexts
            usernameEditText.Text = scammer.Username;
            emailEditText.Text = scammer.Email;
            phoneEditText.Text = scammer.Phone;
            websiteEditText.Text = scammer.Website;
            SetSeverityRadioButton(scammer.Severity);
            descriptionEditText.Text = scammer.Description;

            // Handle the save, cancel, and delete button clicks
            saveButton.Click += SaveButton_Click;
            cancelButton2.Click += CancelButton2_Click;
            deleteButton.Click += DeleteButton_Click;

            // Set the button background colors
            saveButton.SetBackgroundColor(new Android.Graphics.Color(colorAccent));
            deleteButton.SetBackgroundColor(new Android.Graphics.Color(colorWarning));
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            // Get the updated values from the EditTexts and RadioGroup
            string updatedUsername = usernameEditText.Text;
            string updatedEmail = emailEditText.Text;
            string updatedPhone = phoneEditText.Text;
            string updatedWebsite = websiteEditText.Text;
            string updatedSeverity = GetSelectedSeverity();
            string updatedDescription = descriptionEditText.Text;

            // Update the scammer in the database
            databaseHelper.UpdateScammer(scammerId, updatedUsername, updatedEmail, updatedPhone, updatedWebsite, updatedSeverity, updatedDescription);

            // Show a toast message to indicate the successful update
            Toast.MakeText(this, "Scammer updated successfully", ToastLength.Short).Show();

            // Finish the activity and return to the MainActivity
            Android.Content.Intent intent = new Android.Content.Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }

        private void CancelButton2_Click(object sender, EventArgs e)
        {
            // Finish the activity and return to the MainActivity without saving any changes
            Android.Content.Intent intent = new Android.Content.Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            // Delete the scammer from the database
            databaseHelper.DeleteScammer(scammerId);

            // Show a toast message to indicate the successful deletion
            Toast.MakeText(this, "Scammer deleted successfully", ToastLength.Short).Show();

            // Finish the activity and return to the MainActivity
            Android.Content.Intent intent = new Android.Content.Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }

        private string GetSelectedSeverity()
        {
            // Get the ID of the selected radio button in the severity radio group
            int selectedRadioButtonId = severityRadioGroup.CheckedRadioButtonId;

            // Find the selected radio button by its ID
            RadioButton selectedRadioButton = FindViewById<RadioButton>(selectedRadioButtonId);

            // Return the text of the selected radio button
            return selectedRadioButton.Text;
        }

        private void SetSeverityRadioButton(string severity)
        {
            int radioButtonId = Resource.Id.RbtnLow2;

            // Set the correct radio button based on the severity value
            switch (severity)
            {
                case "Medium":
                    radioButtonId = Resource.Id.RbtnMedium2;
                    break;
                case "High":
                    radioButtonId = Resource.Id.RbtnHigh2;
                    break;
                case "Critical":
                    radioButtonId = Resource.Id.RbtnCritical2;
                    break;
            }

            // Check the radio button corresponding to the severity value
            severityRadioGroup.Check(radioButtonId);
        }
    }
}
