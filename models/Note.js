const mongoose = require('mongoose'); // Import mongoose for database schema

// Define schema for a note
const NoteSchema = new mongoose.Schema({
  title: String, // Title of the note
  body: String, // Main content of the note
  files: [String], // Array of uploaded file names
  createdAt: Date // Timestamp of when the note was created
});

module.exports = mongoose.model('Note', NoteSchema); // Export Note model
