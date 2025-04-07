const mongoose = require('mongoose'); // Import mongoose for defining schema
const bcrypt = require('bcryptjs'); // Import bcrypt for hashing passwords

// Define schema for a user
const UserSchema = new mongoose.Schema({
  username: { type: String, required: true, unique: true }, // Unique username
  password: { type: String, required: true } // Hashed password
});

// Middleware to hash password before saving to the database
UserSchema.pre('save', async function (next) {
  if (!this.isModified('password')) return next(); // Skip if password hasn't changed
  const salt = await bcrypt.genSalt(10); // Generate salt
  this.password = await bcrypt.hash(this.password, salt); // Hash password
  next(); // Continue
});

// Method to compare a password input with the stored hash
UserSchema.methods.comparePassword = function (candidatePassword) {
  return bcrypt.compare(candidatePassword, this.password); // Return match result
};

module.exports = mongoose.model('User', UserSchema); // Export User model
