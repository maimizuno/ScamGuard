using System;
using System.Collections.Generic;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;

namespace $safeprojectname$
{
    public class DatabaseHelper : SQLiteOpenHelper
    {
        private static readonly string DatabaseName = "ScamGuardDatabase.db";
        private static readonly int DatabaseVersion = 2;
        private static readonly string TableName = "Scammers";
        private static readonly string ColumnId = "Id";
        private static readonly string ColumnUsername = "Username";
        private static readonly string ColumnEmail = "Email";
        private static readonly string ColumnPhone = "Phone";
        private static readonly string ColumnWebsite = "Website";
        private static readonly string ColumnSeverity = "Severity";
        private static readonly string ColumnDescription = "Description";
        private static readonly string ColumnUpdateTimestamp = "UpdateTimestamp"; // New column name

        private Context context;

        public DatabaseHelper(Context context) : base(context, DatabaseName, null, DatabaseVersion)
        {
            this.context = context;
        }

        public override void OnCreate(SQLiteDatabase db)
        {
            // Create the table query with all the columns
            string createTableQuery = $"CREATE TABLE IF NOT EXISTS {TableName} " +
                $"({ColumnId} INTEGER PRIMARY KEY AUTOINCREMENT, " +
                $"{ColumnUsername} TEXT, " +
                $"{ColumnEmail} TEXT, " +
                $"{ColumnPhone} TEXT, " +
                $"{ColumnWebsite} TEXT, " +
                $"{ColumnSeverity} TEXT, " +
                $"{ColumnDescription} TEXT, " +
                $"{ColumnUpdateTimestamp} INTEGER)"; // Include the new column in the create table query

            // Execute the create table query
            db.ExecSQL(createTableQuery);
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            // Drop the existing table and recreate it
            string dropTableQuery = $"DROP TABLE IF EXISTS {TableName}";
            db.ExecSQL(dropTableQuery);
            OnCreate(db);
        }

        public void InsertScammer(string username, string email, string phone, string website, string severity, string description)
        {
            // Get the writable database
            SQLiteDatabase db = WritableDatabase;

            // Create ContentValues object and populate it with the column-value pairs
            ContentValues values = new ContentValues();
            values.Put(ColumnUsername, username);
            values.Put(ColumnEmail, email);
            values.Put(ColumnPhone, phone);
            values.Put(ColumnWebsite, website);
            values.Put(ColumnSeverity, severity);
            values.Put(ColumnDescription, description);
            values.Put(ColumnUpdateTimestamp, DateTime.Now.Ticks);

            // Insert the values into the table
            db.Insert(TableName, null, values);

            // Close the database
            db.Close();
        }

        public List<Scammer> GetAllScammers()
        {
            // Create a list to store the scammers
            List<Scammer> scammers = new List<Scammer>();

            // Get the readable database
            SQLiteDatabase db = ReadableDatabase;

            // Execute the query to retrieve all rows from the table
            ICursor cursor = db.Query(TableName, null, null, null, null, null, $"{ColumnUpdateTimestamp} DESC"); // Sort by UpdateTimestamp in descending order

            // Iterate through the cursor to extract the data and create Scammer objects
            while (cursor.MoveToNext())
            {
                // Extract column values from the cursor
                int id = cursor.GetInt(cursor.GetColumnIndex(ColumnId));
                string username = cursor.GetString(cursor.GetColumnIndex(ColumnUsername));
                string email = cursor.GetString(cursor.GetColumnIndex(ColumnEmail));
                string phone = cursor.GetString(cursor.GetColumnIndex(ColumnPhone));
                string website = cursor.GetString(cursor.GetColumnIndex(ColumnWebsite));
                string severity = cursor.GetString(cursor.GetColumnIndex(ColumnSeverity));
                string description = cursor.GetString(cursor.GetColumnIndex(ColumnDescription));
                long updateTimestamp = cursor.GetLong(cursor.GetColumnIndex(ColumnUpdateTimestamp)); // Retrieve the long value

                // Create a new Scammer object and add it to the list
                Scammer scammer = new Scammer(id, username, email, phone, website, severity, description, updateTimestamp);
                scammers.Add(scammer);
            }

            // Close the cursor and database
            cursor.Close();
            db.Close();

            // Return the list of scammers
            return scammers;
        }


        public void UpdateScammer(int scammerId, string updatedUsername, string updatedEmail, string updatedPhone, string updatedWebsite, string updatedSeverity, string updatedDescription)
        {
            // Get the writable database
            SQLiteDatabase db = WritableDatabase;

            // Create ContentValues object and populate it with the updated column-value pairs
            ContentValues values = new ContentValues();
            values.Put(ColumnUsername, updatedUsername);
            values.Put(ColumnEmail, updatedEmail);
            values.Put(ColumnPhone, updatedPhone);
            values.Put(ColumnWebsite, updatedWebsite);
            values.Put(ColumnSeverity, updatedSeverity);
            values.Put(ColumnDescription, updatedDescription);
            values.Put(ColumnUpdateTimestamp, DateTime.Now.Ticks); // Update the timestamp

            // Create the where clause and arguments for the update query
            string whereClause = $"{ColumnId} = ?";
            string[] whereArgs = { scammerId.ToString() };

            // Execute the update query
            db.Update(TableName, values, whereClause, whereArgs);

            // Close the database
            db.Close();
        }

        public void DeleteScammer(int scammerId)
        {
            // Get the writable database
            SQLiteDatabase db = WritableDatabase;

            // Create the where clause and arguments for the delete query
            string whereClause = $"{ColumnId} = ?";
            string[] whereArgs = { scammerId.ToString() };

            // Execute the delete query
            db.Delete(TableName, whereClause, whereArgs);

            // Close the database
            db.Close();
        }

        public Scammer GetScammerById(int scammerId)
        {
            // Get the readable database
            SQLiteDatabase db = ReadableDatabase;

            // Create the selection and selection arguments for the query
            string selection = $"{ColumnId} = ?";
            string[] selectionArgs = { scammerId.ToString() };

            // Execute the query to retrieve the row with the specified scammerId
            ICursor cursor = db.Query(TableName, null, selection, selectionArgs, null, null, null);

            Scammer scammer = null;

            if (cursor.MoveToFirst())
            {
                // Extract column values from the cursor
                string username = cursor.GetString(cursor.GetColumnIndex(ColumnUsername));
                string email = cursor.GetString(cursor.GetColumnIndex(ColumnEmail));
                string phone = cursor.GetString(cursor.GetColumnIndex(ColumnPhone));
                string website = cursor.GetString(cursor.GetColumnIndex(ColumnWebsite));
                string severity = cursor.GetString(cursor.GetColumnIndex(ColumnSeverity));
                string description = cursor.GetString(cursor.GetColumnIndex(ColumnDescription));
                long updateTimestamp = cursor.GetLong(cursor.GetColumnIndex(ColumnUpdateTimestamp));

                // Create a new Scammer object with the retrieved data
                scammer = new Scammer(scammerId, username, email, phone, website, severity, description, updateTimestamp);
            }

            // Close the cursor and database
            cursor.Close();
            db.Close();

            // Return the Scammer object
            return scammer;
        }
    }

    public class Scammer
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Severity { get; set; }
        public string Description { get; set; }
        public long UpdateTimestamp { get; set; }

        public Scammer(int id, string username, string email, string phone, string website, string severity, string description, long updateTimestamp)
        {
            Id = id;
            Username = username;
            Email = email;
            Phone = phone;
            Website = website;
            Severity = severity;
            Description = description;
            UpdateTimestamp = updateTimestamp;
        }


        public Scammer()
        {
            // Empty constructor needed for database operations
        }
    }
}
