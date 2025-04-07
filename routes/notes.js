const express = require('express');
const router = express.Router();
const Note = require('../models/Note');

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

// Show the form to create a new note
router.get('/add', (req, res) => {
  res.render('notes/add');
});

// Handle note creation
router.post('/add', async (req, res) => {
  try {
    const { title, body } = req.body;
    await Note.create({ title, body });
    res.redirect('/notes');
  } catch (err) {
    console.error(err);
    res.render('error', { message: 'Failed to add note' });
  }
});

module.exports = router;
