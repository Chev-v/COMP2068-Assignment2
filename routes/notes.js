const express = require('express');
const router = express.Router();
const upload = require('../middleware/multer');
const Note = require('../models/Note');

// Get all notes
router.get('/', async (req, res) => {
  try {
    const notes = await Note.find().sort({ createdAt: -1 });
    res.render('notes/index', { notes });
  } catch (err) {
    res.status(500).send('Failed to get notes');
  }
});

// Add note form
router.get('/add', (req, res) => {
  res.render('notes/add');
});

// Handle add note
router.post('/add', upload.array('files'), async (req, res) => {
  try {
    const fileNames = req.files ? req.files.map(file => file.filename) : [];

    const note = new Note({
      title: req.body.title,
      body: req.body.body,
      files: fileNames,
      createdAt: new Date()
    });

    await note.save();
    res.redirect('/notes');
  } catch (err) {
    res.status(500).send('Error saving note.');
  }
});

// Edit note form
router.get('/:id/edit', async (req, res) => {
  try {
    const note = await Note.findById(req.params.id);
    res.render('notes/edit', { note });
  } catch (err) {
    res.status(500).send('Error loading note');
  }
});

// Handle edit
router.post('/:id/edit', upload.array('files'), async (req, res) => {
  try {
    const fileNames = req.files ? req.files.map(file => file.filename) : [];

    await Note.findByIdAndUpdate(req.params.id, {
      title: req.body.title,
      body: req.body.body,
      $push: { files: { $each: fileNames } }
    });

    res.redirect('/notes');
  } catch (err) {
    res.status(500).send('Error updating note');
  }
});

// Handle delete
router.post('/:id/delete', async (req, res) => {
  try {
    await Note.findByIdAndDelete(req.params.id);
    res.redirect('/notes');
  } catch (err) {
    res.status(500).send('Error deleting note');
  }
});

module.exports = router;
