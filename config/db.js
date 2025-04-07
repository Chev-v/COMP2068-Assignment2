// Load environment variables from .env file
require('dotenv').config();

// Import mongoose for MongoDB interactions
const mongoose = require('mongoose');

// Connect to MongoDB using the connection string from the .env file
mongoose.connect(process.env.MONGO_URI)
  .then(() => console.log('MongoDB connected')) // Log on successful connection
  .catch((err) => console.error('MongoDB connection error:', err)); // Log error if connection fails
