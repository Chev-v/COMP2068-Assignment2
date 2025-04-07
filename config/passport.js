const LocalStrategy = require('passport-local').Strategy; // Import the local strategy
const GitHubStrategy = require('passport-github2').Strategy; // Import GitHub strategy
const User = require('../models/User'); // Load the User model

module.exports = function (passport) {
  // Local strategy for user authentication using username and password
  passport.use(new LocalStrategy(async (username, password, done) => {
    try {
      const user = await User.findOne({ username }); // Look for the user by username
      if (!user) return done(null, false); // Fail if user not found

      const isMatch = await user.comparePassword(password); // Compare password
      if (!isMatch) return done(null, false); // Fail if password doesn't match

      return done(null, user); // Success
    } catch (err) {
      return done(err); // Handle errors
    }
  }));

  // GitHub strategy for OAuth login using GitHub
  passport.use(new GitHubStrategy({
    clientID: process.env.GITHUB_CLIENT_ID, // GitHub client ID from .env
    clientSecret: process.env.GITHUB_CLIENT_SECRET, // GitHub secret from .env
    callbackURL: '/auth/github/callback' // Redirect URL after GitHub login
  },
    async (accessToken, refreshToken, profile, done) => {
      try {
        let user = await User.findOne({ username: profile.username }); // Search by GitHub username
        if (!user) {
          user = await User.create({ username: profile.username, password: 'placeholder' }); // Create user if not found
        }
        return done(null, user); // Success
      } catch (err) {
        return done(err); // Handle errors
      }
    }
  ));

  // Serialize user to store in session
  passport.serializeUser((user, done) => {
    done(null, user.id); // Store user ID
  });

  // Deserialize user from session using ID
  passport.deserializeUser(async (id, done) => {
    try {
      const user = await User.findById(id); // Find user by ID
      done(null, user); // Return user object
    } catch (err) {
      done(err); // Handle errors
    }
  });
};
