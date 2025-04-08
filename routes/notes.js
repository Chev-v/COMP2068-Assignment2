const express = require('express');
const router = express.Router();
const Note = require('../models/Note');
const multer = require('multer');
const path = require('path');
const fs = require('fs');

// Multer setup
const storage = multer.diskStorage({
  destination: function (req, file, cb) {
    cb(null, 'uploads/');
  },
  filename: function (req, file, cb) {
    cb(null, Date.now() + '-' + file.originalname);
  }
});
const upload = multer({ storage: storage });

// GET all notes
router.get('/', async (req, res) => {
  try {
    const notes = await Note.find().lean();
    res.render('notes/index', { notes });
  } catch (err) {
    console.error(err);
    res.render('error', { message: 'Failed to load notes' });
  }
});

// Show add form
router.get('/add', (req, res) => {
  res.render('notes/add');
});

// Handle add with multiple files
router.post('/add', upload.array('files', 10), async (req, res) => {
  try {
    const { title, body } = req.body;
    const files = req.files ? req.files.map(f => f.filename) : [];

    await Note.create({ title, body, file: files });

    res.redirect('/notes');
  } catch (err) {
    console.error(err);
    res.render('error', { message: 'Failed to add note' });
  }
});

// Show edit form
router.get('/:id/edit', async (req, res) => {
  try {
    const note = await Note.findById(req.params.id).lean();
    if (!note) return res.render('error', { message: 'Note not found' });
    res.render('notes/edit', { note });
  } catch (err) {
    console.error(err);
    res.render('error', { message: 'Failed to load note for editing' });
  }
});

// Handle edit and add new files
router.post('/:id/edit', upload.array('files', 10), async (req, res) => {
  try {
    const { title, body } = req.body;
    const note = await Note.findById(req.params.id);

    if (!note) {
      return res.status(404).render('error', { message: 'Note not found' });
    }

    const newFiles = req.files ? req.files.map(f => f.filename) : [];
    note.title = title;
    note.body = body;
    note.file = [...(note.file || []), ...newFiles];

    await note.save();

    res.redirect('/notes');
  } catch (err) {
    console.error(err);
    res.render('error', { message: 'Failed to update note' });
  }
});

// Handle note delete
router.post('/:id/delete', async (req, res) => {
  try {
    const note = await Note.findById(req.params.id);
    if (!note) return res.redirect('/notes');

    // Delete all associated files
    if (Array.isArray(note.file)) {
      note.file.forEach(file => {
        const filePath = path.join(__dirname, '../uploads', file);
        if (fs.existsSync(filePath)) fs.unlinkSync(filePath);
      });
    }

    await Note.findByIdAndDelete(req.params.id);
    res.redirect('/notes');
  } catch (err) {
    console.error(err);
    res.render('error', { message: 'Failed to delete note' });
  }
});

// Handle individual file delete
router.post('/:id/files/delete', async (req, res) => {
  try {
    const { fileName } = req.body;
    const note = await Note.findById(req.params.id);

    if (!note || !fileName) {
      return res.redirect(`/notes/${req.params.id}/edit`);
    }

    note.file = note.file.filter(f => f !== fileName);
    await note.save();

    const filePath = path.join(__dirname, '../uploads', fileName);
    if (fs.existsSync(filePath)) {
      fs.unlinkSync(filePath);
    }

    res.redirect(`/notes/${note._id}/edit`);
  } catch (err) {
    console.error(err);
    res.render('error', { message: 'Failed to delete file' });
  }
});

module.exports = router;
