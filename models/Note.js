const mongoose = require('mongoose');

const noteSchema = new mongoose.Schema({
  title: {
    type: String,
    required: true
  },
  body: {
    type: String,
    required: true
  },
  createdAt: {
    type: Date,
    default: Date.now
  },
  file: {
    type: String, // file upload path (image/video)
    required: false
  },
  user: {
    type: mongoose.Schema.Types.ObjectId,
    ref: 'User' // we’ll set this up later for authentication
  }
});

module.exports = mongoose.model('Note', noteSchema);
