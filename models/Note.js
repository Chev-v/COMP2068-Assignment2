const mongoose = require('mongoose');

const NoteSchema = new mongoose.Schema({
  title: String,
  body: String,
  files: [String],
  createdAt: Date
});

module.exports = mongoose.model('Note', NoteSchema);
