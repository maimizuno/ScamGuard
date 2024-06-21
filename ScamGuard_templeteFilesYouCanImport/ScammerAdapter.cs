using Android.Content;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using System.Linq;
using Android.Graphics;
using Android.App;
using System;

namespace $safeprojectname$
{
    public class ScammerAdapter : BaseAdapter<Scammer>
    {
        private Context context;
        private List<Scammer> scammers;

        // Define the request code for EditScammerActivity
        private const int EditScammerRequestCode = 1;

        // Constructor to initialize the adapter with context and scammers data
        public ScammerAdapter(Context context, List<Scammer> scammers)
        {
            this.context = context;
            this.scammers = scammers;
        }

        // Get the total number of items in the data set
        public override int Count => scammers.Count;

        // Get the item at a specific position in the data set
        public override Scammer this[int position] => scammers[position];

        // Get the ID of an item at a specific position in the data set
        public override long GetItemId(int position) => position;

        // Update the adapter's data set with new scammers data
        public void UpdateData(List<Scammer> scammers)
        {
            this.scammers.Clear();
            this.scammers.AddRange(scammers.OrderByDescending(s => s.UpdateTimestamp));
            NotifyDataSetChanged();
        }

        // Get the view that displays the data at the specified position in the data set
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            // Get the color from the color resource using the resource ID
            int colorWarning = context.Resources.GetColor(Resource.Color.colorWarning);
            int colorOrange = context.Resources.GetColor(Resource.Color.colorOrange);
            int colorYellow = context.Resources.GetColor(Resource.Color.colorYellow);
            int colorGreen = context.Resources.GetColor(Resource.Color.colorGreen);

            // Inflate the view if it doesn't exist
            if (view == null)
            {
                LayoutInflater inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
                view = inflater.Inflate(Resource.Layout.scammer_list_item, null);
            }

            // Find the TextViews within the view
            TextView labelTextView = view.FindViewById<TextView>(Resource.Id.labelTextView);
            TextView usernameTextView = view.FindViewById<TextView>(Resource.Id.usernameTextView);
            TextView emailTextView = view.FindViewById<TextView>(Resource.Id.emailTextView);
            TextView phoneTextView = view.FindViewById<TextView>(Resource.Id.phoneTextView);
            TextView websiteTextView = view.FindViewById<TextView>(Resource.Id.websiteTextView);
            TextView severityTextView = view.FindViewById<TextView>(Resource.Id.severityTextView);
            TextView descriptionTextView = view.FindViewById<TextView>(Resource.Id.descriptionTextView);
            ImageView editButton = view.FindViewById<ImageView>(Resource.Id.btnEdit);

            // Get the scammer data at the current position from the sorted list
            Scammer scammer = scammers[position];

            // Set the text for each TextView with the corresponding scammer data
            usernameTextView.Text = "Username: " + scammer.Username;
            emailTextView.Text = "Email: " + scammer.Email;
            phoneTextView.Text = "Phone: " + scammer.Phone;
            websiteTextView.Text = "Website: " + scammer.Website;
            severityTextView.Text = "Severity: " + scammer.Severity;
            descriptionTextView.Text = "Description: " + scammer.Description;
            editButton.Click += (sender, e) =>
            {
                // Handle the edit button click
                EditScammer(position);
            };

            // Set the background color of labelTextView based on the severity value
            string severity = scammer.Severity.ToLower();
            if (severity == "low")
            {
                labelTextView.SetBackgroundColor(new Android.Graphics.Color(colorGreen));
            }
            else if (severity == "medium")
            {
                labelTextView.SetBackgroundColor(new Android.Graphics.Color(colorYellow));
            }
            else if (severity == "high")
            {
                labelTextView.SetBackgroundColor(new Android.Graphics.Color(colorOrange));
            }
            else if (severity == "critical")
            {
                labelTextView.SetBackgroundColor(new Android.Graphics.Color(colorWarning));
            }

            return view;
        }

        private void EditScammer(int position)
        {
            Scammer scammer = scammers[position];

            // Create an intent to start the EditScammerActivity
            Intent intent = new Intent(context, typeof(EditScammerActivity));

            // Pass the scammer data to the EditScammerActivity
            intent.PutExtra("ScammerId", scammer.Id);
            intent.PutExtra("Username", scammer.Username);
            intent.PutExtra("Email", scammer.Email);
            intent.PutExtra("Phone", scammer.Phone);
            intent.PutExtra("Website", scammer.Website);
            intent.PutExtra("Severity", scammer.Severity);
            intent.PutExtra("Description", scammer.Description);

            // Start the EditScammerActivity
            ((Activity)context).StartActivityForResult(intent, EditScammerRequestCode);
        }

        public void UpdateScammer(int scammerId, Scammer updatedScammer)
        {
            // Find the index of the scammer with the given scammerId
            int index = scammers.FindIndex(s => s.Id == scammerId);

            if (index != -1)
            {
                scammers[index] = updatedScammer;
                scammers[index].UpdateTimestamp = DateTime.Now.Ticks; // Set the update timestamp
                scammers = scammers.OrderByDescending(s => s.UpdateTimestamp).ToList(); // Sort the scammers based on the update timestamp
                NotifyDataSetChanged();
            }
        }

        public void RemoveScammer(Scammer scammer)
        {
            scammers.Remove(scammer);
            NotifyDataSetChanged();
        }

        public void Clear()
        {
            scammers.Clear();
            NotifyDataSetChanged();
        }

        public void AddAll(List<Scammer> scammers)
        {
            this.scammers.AddRange(scammers);
            NotifyDataSetChanged();
        }
    }
}
