const express = require('express');
const router = express.Router();
const multer = require('multer');
const Note = require('../models/Note');

// Configure multer storage
const storage = multer.diskStorage({
  destination: function (req, file, cb) {
    cb(null, './public/uploads');
  },
  filename: function (req, file, cb) {
    const uniqueName = Date.now() + '-' + file.originalname;
    cb(null, uniqueName);
  }
});
const upload = multer({ storage });

// GET all notes (authenticated view)
router.get('/', async (req, res) => {
  try {
    const notes = await Note.find().lean();
    const updatedNotes = notes.map(note => {
      const fileTypes = (note.files || []).map(name => {
        const ext = name.split('.').pop().toLowerCase();
        const isVideo = ['mp4', 'mov', 'avi', 'webm'].includes(ext);
        return { name, isVideo };
      });

      return {
        ...note,
        formattedDate: new Date(note.createdAt).toLocaleString(),
        fileTypes
      };
    });

    res.render('notes/index', { notes: updatedNotes });
  } catch (err) {
    console.error('Failed to get notes:', err);
    res.status(500).send('Failed to get notes');
  }
});

// GET form to add note
router.get('/add', (req, res) => {
  res.render('notes/add');
});

// POST add new note
router.post('/add', upload.array('files', 5), async (req, res) => {
  try {
    const fileNames = req.files.map(file => file.filename);

    const note = new Note({
      title: req.body.title,
      body: req.body.body,
      files: fileNames,
      createdAt: new Date()
    });

    await note.save();
    res.redirect('/notes');
  } catch (err) {
    console.error('Error creating note:', err);
    res.status(500).send('Error saving note.');
  }
});

// GET edit note form
router.get('/:id/edit', async (req, res) => {
  try {
    const note = await Note.findById(req.params.id).lean();
    if (!note) return res.status(404).send('Note not found');
    res.render('notes/edit', { note });
  } catch (err) {
    res.status(500).send('Error retrieving note');
  }
});

// POST update edited note
router.post('/:id/edit', async (req, res) => {
  try {
    await Note.findByIdAndUpdate(req.params.id, {
      title: req.body.title,
      body: req.body.body
    });
    res.redirect('/notes');
  } catch (err) {
    res.status(500).send('Error updating note');
  }
});

// POST delete note
router.post('/:id/delete', async (req, res) => {
  try {
    await Note.findByIdAndDelete(req.params.id);
    res.redirect('/notes');
  } catch (err) {
    console.error('Failed to delete note:', err);
    res.status(500).send('Error deleting note');
  }
});

module.exports = router;
    