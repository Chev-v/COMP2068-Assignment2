const express = require('express'); // Import express
const passport = require('passport'); // Import passport
const User = require('../models/User'); // Import user model

const router = express.Router(); // Create router

// GET register page
router.get('/register', (req, res) => {
  res.render('auth/register', { error: null }); // Render register view with no error
});

// POST register new user
router.post('/register', async (req, res) => {
  const { username, password } = req.body; // Get username and password from form

  try {
    const existingUser = await User.findOne({ username }); // Check if user exists
    if (existingUser) {
      return res.render('auth/register', { error: 'Username already taken' }); // Show error
    }

    const user = new User({ username, password }); // Create new user
    await user.save(); // Save to DB
    res.redirect('/auth/login'); // Redirect to login
  } catch (err) {
    console.error(err); // Log error
    res.render('auth/register', { error: 'Error registering user' }); // Show error
  }
});

// GET login page
router.get('/login', (req, res) => {
  res.render('auth/login', { error: null }); // Render login view with no error
});

// POST login
router.post('/login', (req, res, next) => {
  passport.authenticate('local', (err, user, info) => {
    if (err || !user) {
      return res.render('auth/login', { error: 'Invalid credentials' }); // Show error
    }

    req.login(user, (err) => {
      if (err) return next(err); // Error during login
      res.redirect('/notes'); // Redirect to notes if successful
    });
  })(req, res, next);
});

// GET logout
router.get('/logout', (req, res) => {
  req.logout(() => {
    res.redirect('/'); // Logout and redirect to home
  });
});

// GitHub login
router.get('/github', passport.authenticate('github', { scope: ['user:email'] }));

// GitHub callback
router.get('/github/callback',
  passport.authenticate('github', {
    failureRedirect: '/auth/login' // Fail redirect
  }),
  (req, res) => {
    res.redirect('/notes'); // Success redirect
  }
);

module.exports = router; // Export router
