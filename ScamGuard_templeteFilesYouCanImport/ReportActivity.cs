using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace $safeprojectname$
{
    [Activity(Label = "Scam Report Form")]
    public class ReportActivity : AppCompatActivity
    {
        private EditText usernameEditText;
        private EditText emailEditText;
        private EditText phoneEditText;
        private EditText websiteEditText;
        private RadioGroup severityRadioGroup;
        private EditText descriptionEditText;
        private Button submitButton;
        private Button cancelButton;

        private int colorAccent;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_report_form);

            // Get the color values from resources
            colorAccent = Resources.GetColor(Resource.Color.colorAccent);

            // Find the views by their respective IDs
            usernameEditText = FindViewById<EditText>(Resource.Id.edtUsername);
            emailEditText = FindViewById<EditText>(Resource.Id.edtEmail);
            phoneEditText = FindViewById<EditText>(Resource.Id.edtPhone);
            websiteEditText = FindViewById<EditText>(Resource.Id.edtWebsite);
            severityRadioGroup = FindViewById<RadioGroup>(Resource.Id.RbtnGroup1);
            descriptionEditText = FindViewById<EditText>(Resource.Id.edtDescription);
            submitButton = FindViewById<Button>(Resource.Id.btnSubmit);
            cancelButton = FindViewById<Button>(Resource.Id.btnCancel);

            // Set click listeners for the submit and cancel buttons
            submitButton.Click += SubmitButton_Click;
            cancelButton.Click += CancelButton_Click;

            // Set background color for the submit button
            submitButton.SetBackgroundColor(new Android.Graphics.Color(colorAccent));
        }

        private void SubmitButton_Click(object sender, System.EventArgs e)
        {
            // Get the values entered by the user
            string username = usernameEditText.Text;
            string email = emailEditText.Text;
            string phone = phoneEditText.Text;
            string website = websiteEditText.Text;
            string severity = GetSelectedSeverity();
            string description = descriptionEditText.Text;

            // Save the information to the database
            DatabaseHelper databaseHelper = new DatabaseHelper(this);
            databaseHelper.InsertScammer(username, email, phone, website, severity, description);

            // Go back to the MainActivity
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }

        private void CancelButton_Click(object sender, System.EventArgs e)
        {
            // Go back to the MainActivity
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }

        private string GetSelectedSeverity()
        {
            int selectedRadioButtonId = severityRadioGroup.CheckedRadioButtonId;
            RadioButton selectedRadioButton = FindViewById<RadioButton>(selectedRadioButtonId);
            return selectedRadioButton.Text;
        }
    }
}
