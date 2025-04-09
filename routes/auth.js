const express = require('express');
const router = express.Router();
const passport = require('passport');
const User = require('../models/User');

// Register Page
router.get('/register', (req, res) => {
  res.render('auth/register');
});

// Register Handler
router.post('/register', async (req, res) => {
  const { username, password } = req.body;
  try {
    await User.create({ username, password });
    res.redirect('/auth/login');
  } catch (err) {
    console.error(err);
    res.send('Registration failed. Username may already exist.');
  }
});

// Login Page
router.get('/login', (req, res) => {
  res.render('auth/login');
});

// Login Handler
router.post('/login', passport.authenticate('local', {
  successRedirect: '/notes',
  failureRedirect: '/auth/login'
}));

// Logout
router.get('/logout', (req, res) => {
  req.logout(() => {
    res.redirect('/');
  });
});

module.exports = router;
